using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

public class FTP
{
	private static FTPState state;

	private static FTPConnection connections;

	public static void Inititalize()
	{
		state = new FTPState();
		connections.active = false;
		Connect();
	}

	public static void Inititalize(string server, short port, string username, string password, bool useSSL)
	{
		state = new FTPState();
		connections.server = server;
		connections.port = port;
		connections.username = username;
		connections.password = password;
		connections.useSSL = useSSL;
		connections.active = false;
	}

	public static bool IsConnected()
	{
		return connections.active;
	}

	public static FTPConnection Connect()
	{
		return Connect("ftp.traplightgames.com", 21, "traplightgamescom", "YRRdBMkJ", true);
	}

	public static FTPConnection Connect(string server, short port, string username, string password, bool useSSL)
	{
		connections.active = true;
		connections.server = server;
		connections.port = port;
		connections.username = username;
		connections.password = password;
		connections.useSSL = useSSL;
		return connections;
	}

	public static bool FolderExists(string folder)
	{
		Debug.Log("Connecting...");
		state.request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + connections.server + "/" + folder));
		state.request.Method = "NLST";
		state.request.UseBinary = true;
		Debug.Log("Logging in...");
		state.request.Credentials = new NetworkCredential(connections.username, connections.password);
		Debug.Log("Checking if ./" + folder + " exists...");
		try
		{
			FtpWebResponse ftpWebResponse = (FtpWebResponse)state.request.GetResponse();
			Debug.Log("Folder exists.");
			return true;
		}
		catch
		{
			Debug.Log("Folder does not exist.");
			return false;
		}
	}

	public static bool CreateFolder(string folder)
	{
		Debug.Log("Connecting...");
		state.request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + connections.server + "/" + folder));
		Debug.Log("Logging in...");
		state.request.Credentials = new NetworkCredential(connections.username, connections.password);
		state.request.Method = "MKD";
		Debug.Log("Creating folder " + connections.server + "/" + folder + "...");
		try
		{
			FtpWebResponse ftpWebResponse = (FtpWebResponse)state.request.GetResponse();
			if (ftpWebResponse.StatusCode == FtpStatusCode.PathnameCreated)
			{
				Debug.Log("Folder created.");
			}
		}
		catch (WebException ex)
		{
			FtpWebResponse ftpWebResponse2 = (FtpWebResponse)ex.Response;
			Debug.Log(string.Concat("Couldn't create folder: (", ftpWebResponse2.StatusCode, ") ", ftpWebResponse2.StatusDescription));
			return false;
		}
		return true;
	}

	public static string[] GetFolderContents(string folder)
	{
		if (!connections.active)
		{
			Debug.Log("Not Connected.");
			return null;
		}
		Debug.Log("Connecting...");
		state.request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + connections.server + "/" + folder));
		state.request.Method = "NLST";
		state.request.UseBinary = true;
		Debug.Log("Logging in...");
		state.request.Credentials = new NetworkCredential(connections.username, connections.password);
		Debug.Log("Listing directory ./" + folder);
		FtpWebResponse ftpWebResponse = (FtpWebResponse)state.request.GetResponse();
		Stream responseStream = ftpWebResponse.GetResponseStream();
		StreamReader streamReader = new StreamReader(responseStream);
		List<string> list = new List<string>();
		string text = streamReader.ReadLine();
		while (!string.IsNullOrEmpty(text))
		{
			list.Add(text);
			text = streamReader.ReadLine();
		}
		streamReader.Close();
		Debug.Log("Directory List Complete.");
		return list.ToArray();
	}

	public static bool Upload(string sourcePath, string sourceFilename, string targetPath, string targetFilename)
	{
		string sourcePathAndFilename = sourcePath + "/" + sourceFilename;
		if (sourcePath.EndsWith("/"))
		{
			sourcePathAndFilename = sourcePath + sourceFilename;
		}
		string targetPathAndFilename = targetPath + "/" + targetFilename;
		if (targetPath.EndsWith("/"))
		{
			targetPathAndFilename = targetPath + targetFilename;
		}
		return Upload(sourcePathAndFilename, targetPathAndFilename);
	}

	public static bool Upload(string sourcePathAndFilename, string targetPathAndFilename)
	{
		if (!connections.active)
		{
			return false;
		}
		state.request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + connections.server + "/" + targetPathAndFilename));
		state.request.Method = "DELE";
		state.request.KeepAlive = false;
		state.request.UseBinary = true;
		state.request.Credentials = new NetworkCredential(connections.username, connections.password);
		try
		{
			state.request.GetResponse();
		}
		catch
		{
		}
		Debug.Log("Connecting...");
		state.request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + connections.server + "/" + targetPathAndFilename));
		state.request.Method = "STOR";
		state.request.KeepAlive = false;
		state.request.UseBinary = true;
		Debug.Log("Logging in...");
		state.request.Credentials = new NetworkCredential(connections.username, connections.password);
		state.fileName = sourcePathAndFilename;
		Debug.Log("Uploading file " + sourcePathAndFilename + " -->> " + targetPathAndFilename);
		ManualResetEvent operationComplete = state.operationComplete;
		state.request.BeginGetRequestStream(BeginUploadStream, state);
		operationComplete.WaitOne();
		if (state.opEx != null)
		{
			throw state.opEx;
		}
		Debug.Log("Done uploading a file.");
		return true;
	}

	public static void BeginUploadStream(IAsyncResult result)
	{
		FTPState fTPState = (FTPState)result.AsyncState;
		Stream stream = null;
		try
		{
			stream = fTPState.request.EndGetRequestStream(result);
			byte[] array = File.ReadAllBytes(fTPState.fileName);
			fTPState.request.ContentLength = array.Length;
			Debug.Log("Sending " + array.Length + "bytes to server...");
			int num = array.Length;
			int num2 = 1024;
			int num3 = Mathf.CeilToInt(num / num2) + 1;
			int num4 = 0;
			for (int i = 0; i < num3; i++)
			{
				int num5 = Mathf.Min(num - i * num2, num2);
				num4 += num5;
				stream.Write(array, i * num2, num5);
			}
			stream.Close();
			fTPState.request.BeginGetResponse(EndUploadResponse, fTPState);
		}
		catch (Exception opEx)
		{
			Debug.Log("Could not get request stream.");
			fTPState.opEx = opEx;
			fTPState.operationComplete.Set();
		}
	}

	public static void EndUploadResponse(IAsyncResult result)
	{
		FTPState fTPState = (FTPState)result.AsyncState;
		FtpWebResponse ftpWebResponse = null;
		try
		{
			ftpWebResponse = (FtpWebResponse)fTPState.request.EndGetResponse(result);
			ftpWebResponse.Close();
			fTPState.fileName = string.Empty;
			fTPState.fileSize = 0;
			fTPState.operationComplete.Set();
			Debug.Log("File upload complete.");
		}
		catch (Exception opEx)
		{
			Debug.Log("Could not get response.");
			fTPState.opEx = opEx;
			fTPState.operationComplete.Set();
		}
	}

	public static bool Download(string sourcePathAndFilename, string targetPathAndFilename)
	{
		if (!connections.active)
		{
			return false;
		}
		ManualResetEvent manualResetEvent = null;
		state.request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + connections.server + "/" + sourcePathAndFilename));
		state.request.Method = "SIZE";
		state.request.KeepAlive = false;
		state.request.UseBinary = true;
		Debug.Log("Logging in...");
		state.request.Credentials = new NetworkCredential(connections.username, connections.password);
		try
		{
			FtpWebResponse ftpWebResponse = (FtpWebResponse)state.request.GetResponse();
			state.fileSize = (int)ftpWebResponse.ContentLength;
		}
		catch (WebException ex)
		{
			FtpWebResponse ftpWebResponse2 = (FtpWebResponse)ex.Response;
			Debug.Log(string.Concat("File does not exist. (", ftpWebResponse2.StatusCode, ") ", ftpWebResponse2.StatusDescription));
			return false;
		}
		Debug.Log("Connecting...");
		state.request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + connections.server + "/" + sourcePathAndFilename));
		state.request.Method = "RETR";
		state.request.KeepAlive = false;
		state.request.UseBinary = true;
		Debug.Log("Logging in...");
		state.request.Credentials = new NetworkCredential(connections.username, connections.password);
		state.fileName = targetPathAndFilename;
		Debug.Log("Downloading file " + targetPathAndFilename + " <<-- " + sourcePathAndFilename);
		manualResetEvent = state.operationComplete;
		state.request.BeginGetResponse(EndDownloadResponse, state);
		manualResetEvent.WaitOne();
		if (state.opEx != null)
		{
			throw state.opEx;
		}
		Debug.Log("Done downloading a file.");
		return true;
	}

	public static void EndDownloadResponse(IAsyncResult result)
	{
		FTPState fTPState = (FTPState)result.AsyncState;
		FtpWebResponse ftpWebResponse = null;
		try
		{
			ftpWebResponse = (FtpWebResponse)fTPState.request.EndGetResponse(result);
			Stream responseStream = ftpWebResponse.GetResponseStream();
			Debug.Log("Receiving " + fTPState.fileSize + "bytes from server...");
			int fileSize = fTPState.fileSize;
			int num = 1024;
			int num2 = Mathf.CeilToInt(fileSize / num) + 1;
			int num3 = 0;
			BinaryReader binaryReader = new BinaryReader(responseStream);
			FileStream fileStream = new FileStream(fTPState.fileName, FileMode.OpenOrCreate);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			for (int i = 0; i < num2; i++)
			{
				int num4 = Mathf.Min(fileSize - i * num, num);
				binaryWriter.Write(binaryReader.ReadBytes(num4));
				num3 += num4;
			}
			binaryWriter.Close();
			fileStream.Close();
			binaryReader.Close();
			responseStream.Close();
			Debug.Log("Download complete.");
			fTPState.fileName = string.Empty;
			fTPState.fileSize = 0;
			fTPState.operationComplete.Set();
		}
		catch (Exception opEx)
		{
			Debug.Log("Could not recieve file.");
			fTPState.opEx = opEx;
			fTPState.operationComplete.Set();
		}
	}
}

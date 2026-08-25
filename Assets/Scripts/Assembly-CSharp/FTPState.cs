using System;
using System.Net;
using System.Threading;

public class FTPState
{
	private ManualResetEvent wait;

	private FtpWebRequest _request;

	private string _fileName;

	private int _fileSize;

	private Exception _opEx;

	public string status;

	public ManualResetEvent operationComplete
	{
		get
		{
			return wait;
		}
	}

	public FtpWebRequest request
	{
		get
		{
			return _request;
		}
		set
		{
			_request = value;
		}
	}

	public string fileName
	{
		get
		{
			return _fileName;
		}
		set
		{
			_fileName = value;
		}
	}

	public int fileSize
	{
		get
		{
			return _fileSize;
		}
		set
		{
			_fileSize = value;
		}
	}

	public Exception opEx
	{
		get
		{
			return _opEx;
		}
		set
		{
			_opEx = value;
		}
	}

	public FTPState()
	{
		wait = new ManualResetEvent(false);
	}
}

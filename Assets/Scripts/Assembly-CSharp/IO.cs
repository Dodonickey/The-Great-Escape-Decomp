using System.IO;
using UnityEngine;

public static class IO
{
	public static string GetApplicationRunPath()
	{
		string dataPath = Application.dataPath;
		if (Application.platform == RuntimePlatform.OSXPlayer)
		{
			string text = dataPath;
			dataPath = text + "/../../SaveData/" + Main.m_currentGame.m_projectCode + "/" + Main.m_currentGame.m_projectVersion;
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer)
		{
			string text = dataPath;
			dataPath = text + "/../SaveData/" + Main.m_currentGame.m_projectCode + "/" + Main.m_currentGame.m_projectVersion;
		}
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			dataPath = Application.dataPath + "/" + Main.m_currentGame.m_projectCode + "/Resources/" + Main.m_currentGame.m_projectCode;
		}
		else
		{
			Debug.Log(Application.dataPath);
			Debug.Log(Application.persistentDataPath);
			dataPath = Application.persistentDataPath + "/Documents";
		}
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		return dataPath;
	}

	public static string[] GetCustomLevelFolderContents()
	{
		string path = GetApplicationRunPath() + "/Levels";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		return Directory.GetFiles(path);
	}

	public static string GetResourceLevelPath()
	{
		string text = GetApplicationRunPath() + "/" + Main.m_currentGame.m_projectCode + "/Resources/" + Main.m_currentGame.m_projectCode + "/Levels";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	public static string[] GetResourceLevelFolderContents()
	{
		string path = GetApplicationRunPath() + "/" + Main.m_currentGame.m_projectCode + "/Resources/" + Main.m_currentGame.m_projectCode + "/Levels";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		return Directory.GetFiles(path);
	}
}

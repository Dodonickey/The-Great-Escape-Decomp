using System;
using System.IO;
using UnityEngine;

public static class IO
{
    public static string GetApplicationRunPath()
    {
        /* ---------------------------------------------------------------------
		 * LEGACY CODE (Unity 4 Era):
		 * Messy hardcoded relative paths that failed or caused permissions issues
		 * on Windows, iOS, Android, and WebGL.
		 * ---------------------------------------------------------------------
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
		--------------------------------------------------------------------- */

        // NEW CODE: Unity's official persistent path works natively on 
        // Windows, Mac, iOS, Android, and WebGL without permission crashes.
        string dataPath = Application.persistentDataPath;
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        return dataPath;
    }


    public static string GetCustomLevelPath()
    {
        // NEW METHOD: Helper to centralize the level folder location
        string path = Path.Combine(GetApplicationRunPath(), "Levels");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }


    public static string[] GetCustomLevelFolderContents()
    {
        /* ---------------------------------------------------------------------
		 * LEGACY CODE:
		 * string path = GetApplicationRunPath() + "/Levels";
		 * if (!Directory.Exists(path))
		 * {
		 *     Directory.CreateDirectory(path);
		 * }
		 * return Directory.GetFiles(path);
		 * --------------------------------------------------------------------- */

        // NEW CODE: Uses clean Path.Combine helper
        string path = GetCustomLevelPath();
        return Directory.GetFiles(path);
    }


    public static string GetResourceLevelPath()
    {
        /* ---------------------------------------------------------------------
		 * LEGACY CODE:
		 * Duplicated subfolders: "MyGame/Resources/MyGame/Resources/MyGame/Levels"
		 * string text = GetApplicationRunPath() + "/" + Main.m_currentGame.m_projectCode + "/Resources/" + Main.m_currentGame.m_projectCode + "/Levels";
		 * if (!Directory.Exists(text))
		 * {
		 *     Directory.CreateDirectory(text);
		 * }
		 * return text;
		 * --------------------------------------------------------------------- */

        // NEW CODE: Handles Unity Editor project paths cleanly, 
        // and falls back to persistent level path in standalone builds.
#if UNITY_EDITOR
        string text = Path.Combine(Application.dataPath, Main.m_currentGame.m_projectCode + "/Resources/" + Main.m_currentGame.m_projectCode + "/Levels");
        if (!Directory.Exists(text))
        {
            Directory.CreateDirectory(text);
        }
        return text;
#else
		return GetCustomLevelPath();
#endif
    }

    public static string[] GetResourceLevelFolderContents()
    {
        /* ---------------------------------------------------------------------
		 * LEGACY CODE:
		 * string path = GetApplicationRunPath() + "/" + Main.m_currentGame.m_projectCode + "/Resources/" + Main.m_currentGame.m_projectCode + "/Levels";
		 * if (!Directory.Exists(path))
		 * {
		 *     Directory.CreateDirectory(path);
		 * }
		 * return Directory.GetFiles(path);
		 * --------------------------------------------------------------------- */

        // NEW CODE: Reuses GetResourceLevelPath() cleanly
        string path = GetResourceLevelPath();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return Directory.GetFiles(path);
    }

    /// <summary>
    /// NEW METHOD: Flushes WebGL virtual filesystem memory to browser IndexedDB
    /// so player saves aren't erased when refreshing or closing the browser.
    /// </summary>
    public static void SaveSync()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
		try
		{
			Application.ExternalEval("FS.syncfs(false, function (err) {});");
		}
		catch (Exception e)
		{
			Debug.LogWarning("[IO] WebGL Sync failed: " + e.Message);
		}
#endif
    }
}
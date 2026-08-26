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
		 * on Windows, iOS, and Android.
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

        // NEW CODE: Application.persistentDataPath has existed since Unity 3
        // and works natively on Windows, macOS, iOS, and Android.
        string dataPath = Application.persistentDataPath;
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        return dataPath;
    }

    public static string GetCustomLevelPath()
    {
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

        string path = GetResourceLevelPath();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return Directory.GetFiles(path);
    }
}
using System.Collections.Generic;
using System.IO;

public static class LevelManager
{
	public static List<ILevel> m_levels = new List<ILevel>();

	public static ILevel m_currentLevel;

	public static uint m_currentLevelIndex;

	public static uint m_currentChapterIndex;

	public static bool m_lastLevelInChapter;

	public static void ChangeLevel(IResource _newLevelResource, bool _unloadResource)
	{
		RemoveLevels();
		AppendLevel(_newLevelResource, _unloadResource);
	}

	public static bool ChangeLevel(uint _chapterIndex, uint _levelIndex, bool _unloadResource)
	{
		string text = "c" + _chapterIndex + "l" + _levelIndex;
		string path = IO.GetApplicationRunPath() + "/Levels/" + text + ".bytes";
		if (!File.Exists(path))
		{
			if (text.Length > 8 && text.Substring(text.Length - 8) == " (build)")
			{
				text = text.Substring(0, text.Length - 8);
			}
			path = IO.GetResourceLevelPath() + "/" + text + ".bytes";
		}
		if (File.Exists(path))
		{
			ChangeLevel(new GELevelResource(null, text, path, ResourceType.Level), _unloadResource);
			return true;
		}
		GEBuiltinLevelResource gEBuiltinLevelResource = new GEBuiltinLevelResource(text, Main.m_currentGame.m_projectCode + "/Levels/" + text, ResourceType.Level);
		ResourceManager.LoadResource(gEBuiltinLevelResource);
		if (gEBuiltinLevelResource.resourceObject != null)
		{
			ChangeLevel(gEBuiltinLevelResource, _unloadResource);
			return true;
		}
		return false;
	}

	public static bool ChangeLevel(string _levelName, bool _unloadResource)
	{
		string path = IO.GetApplicationRunPath() + "/Levels/" + _levelName + ".bytes";
		if (!File.Exists(path))
		{
			if (_levelName.Length > 8 && _levelName.Substring(_levelName.Length - 8) == " (build)")
			{
				_levelName = _levelName.Substring(0, _levelName.Length - 8);
			}
			path = IO.GetResourceLevelPath() + "/" + _levelName + ".bytes";
		}
		if (File.Exists(path))
		{
			ChangeLevel(new GELevelResource(null, _levelName, path, ResourceType.Level), _unloadResource);
			return true;
		}
		GEBuiltinLevelResource gEBuiltinLevelResource = new GEBuiltinLevelResource(_levelName, Main.m_currentGame.m_projectCode + "/Levels/" + _levelName, ResourceType.Level);
		ResourceManager.LoadResource(gEBuiltinLevelResource);
		if (gEBuiltinLevelResource.resourceObject != null)
		{
			ChangeLevel(gEBuiltinLevelResource, _unloadResource);
			return true;
		}
		return false;
	}

	public static void ResetCurrent(bool _unloadResource)
	{
		string text = m_currentLevel.name;
		RemoveLevels();
		string path = IO.GetApplicationRunPath() + "/Levels/" + text + ".bytes";
		if (!File.Exists(path))
		{
			if (text.Length > 8 && text.Substring(text.Length - 8) == " (build)")
			{
				text = text.Substring(0, text.Length - 8);
			}
			path = IO.GetResourceLevelPath() + "/" + text + ".bytes";
		}
		if (File.Exists(path))
		{
			AppendLevel(new GELevelResource(null, text, path, ResourceType.Level), _unloadResource);
			return;
		}
		GEBuiltinLevelResource gEBuiltinLevelResource = new GEBuiltinLevelResource(text, Main.m_currentGame.m_projectCode + "/Levels/" + text, ResourceType.Level);
		ResourceManager.LoadResource(gEBuiltinLevelResource);
		if (gEBuiltinLevelResource.resourceObject != null)
		{
			AppendLevel(gEBuiltinLevelResource, _unloadResource);
		}
	}

	public static void ResetAll(bool _unloadResource)
	{
		List<string> list = new List<string>();
		while (m_levels.Count > 0)
		{
			int index = m_levels.Count - 1;
			list.Add(m_levels[index].name);
			RemoveLevel(m_levels[index]);
		}
		for (int i = 0; i < list.Count; i++)
		{
			int index2 = list.Count - i - 1;
			string path = IO.GetApplicationRunPath() + "/Levels/" + list[index2] + ".bytes";
			if (!File.Exists(path))
			{
				if (list[index2].Length > 8 && list[index2].Substring(list[index2].Length - 8) == " (build)")
				{
					list[index2] = list[index2].Substring(0, list[index2].Length - 8);
				}
				path = IO.GetResourceLevelPath() + "/" + list[index2] + ".bytes";
			}
			if (File.Exists(path))
			{
				AppendLevel(new GELevelResource(null, list[index2], path, ResourceType.Level), _unloadResource);
				continue;
			}
			GEBuiltinLevelResource gEBuiltinLevelResource = new GEBuiltinLevelResource(list[index2], Main.m_currentGame.m_projectCode + "/Levels/" + list[index2], ResourceType.Level);
			ResourceManager.LoadResource(gEBuiltinLevelResource);
			if (gEBuiltinLevelResource.resourceObject != null)
			{
				AppendLevel(gEBuiltinLevelResource, _unloadResource);
			}
		}
	}

	public static void ResetAllButCurrent(bool _unloadResource)
	{
		List<string> list = new List<string>();
		while (m_levels.Count > 0)
		{
			int index = m_levels.Count - 1;
			if (m_levels[index] != m_currentLevel)
			{
				list.Add(m_levels[index].name);
			}
			RemoveLevel(m_levels[index]);
		}
		for (int i = 0; i < list.Count; i++)
		{
			int index2 = list.Count - i - 1;
			string path = IO.GetApplicationRunPath() + "/Levels/" + list[index2] + ".bytes";
			if (!File.Exists(path))
			{
				if (list[index2].Length > 8 && list[index2].Substring(list[index2].Length - 8) == " (build)")
				{
					list[index2] = list[index2].Substring(0, list[index2].Length - 8);
				}
				path = IO.GetResourceLevelPath() + "/" + list[index2] + ".bytes";
			}
			if (File.Exists(path))
			{
				AppendLevel(new GELevelResource(null, list[index2], path, ResourceType.Level), _unloadResource);
				continue;
			}
			GEBuiltinLevelResource gEBuiltinLevelResource = new GEBuiltinLevelResource(list[index2], Main.m_currentGame.m_projectCode + "/Levels/" + list[index2], ResourceType.Level);
			ResourceManager.LoadResource(gEBuiltinLevelResource);
			if (gEBuiltinLevelResource.resourceObject != null)
			{
				AppendLevel(gEBuiltinLevelResource, _unloadResource);
			}
		}
	}

	public static ILevel CreateNewLevel()
	{
		RemoveLevels();
		int count = m_levels.Count;
		m_levels.Add(null);
		ILevel level = Main.m_currentGame.GenerateLevel(null);
		m_levels[count] = level;
		m_currentLevel = level;
		return level;
	}

	public static bool AppendLevel(uint _chapterIndex, uint _levelIndex)
	{
		string text = "c" + _chapterIndex + "l" + _levelIndex;
		string path = IO.GetApplicationRunPath() + "/Levels/" + text + ".bytes";
		if (!File.Exists(path))
		{
			if (text.Length > 8 && text.Substring(text.Length - 8) == " (build)")
			{
				text = text.Substring(0, text.Length - 8);
			}
			path = IO.GetResourceLevelPath() + "/" + text + ".bytes";
		}
		if (File.Exists(path))
		{
			AppendLevel(new GELevelResource(null, text, path, ResourceType.Level), true);
			return true;
		}
		GEBuiltinLevelResource gEBuiltinLevelResource = new GEBuiltinLevelResource(text, Main.m_currentGame.m_projectCode + "/Levels/" + text, ResourceType.Level);
		ResourceManager.LoadResource(gEBuiltinLevelResource);
		if (gEBuiltinLevelResource.resourceObject != null)
		{
			AppendLevel(gEBuiltinLevelResource, true);
			return true;
		}
		return false;
	}

	public static bool AppendLevel(string _levelName, bool _unloadResource)
	{
		string text = _levelName;
		string path = IO.GetApplicationRunPath() + "/Levels/" + _levelName + ".bytes";
		if (!File.Exists(path))
		{
			if (_levelName.Length > 8 && _levelName.Substring(_levelName.Length - 8) == " (build)")
			{
				_levelName = _levelName.Substring(0, _levelName.Length - 8);
			}
			path = IO.GetResourceLevelPath() + "/" + _levelName + ".bytes";
		}
		if (File.Exists(path))
		{
			AppendLevel(new GELevelResource(null, _levelName, path, ResourceType.Level), _unloadResource);
			return true;
		}
		GEBuiltinLevelResource gEBuiltinLevelResource = new GEBuiltinLevelResource(text, Main.m_currentGame.m_projectCode + "/Levels/" + text, ResourceType.Level);
		ResourceManager.LoadResource(gEBuiltinLevelResource);
		if (gEBuiltinLevelResource.resourceObject != null)
		{
			AppendLevel(gEBuiltinLevelResource, _unloadResource);
			return true;
		}
		return false;
	}

	public static bool AppendLevel(IResource _levelResource, bool _unloadResource)
	{
		if (_levelResource.loadState != ResourceLoadState.Loaded)
		{
			ResourceManager.LoadResource(_levelResource);
		}
		bool result = false;
		if (_levelResource.resourceObject != null)
		{
			int count = m_levels.Count;
			m_levels.Add(null);
			ILevel level = Main.m_currentGame.GenerateLevel(_levelResource.resourceObject as ILevel);
			if (level != null)
			{
				m_levels[count] = level;
			}
			else
			{
				m_levels.RemoveAt(count);
			}
			result = true;
		}
		if (_unloadResource)
		{
			ResourceManager.UnloadResource(_levelResource);
		}
		return result;
	}

	public static void ClearLevel(ILevel _level)
	{
		if (_level != null)
		{
			Main.m_currentGame.ClearLevel(_level);
		}
	}

	public static void ClearLevel(string _levelName)
	{
		for (int i = 0; i < m_levels.Count; i++)
		{
			if (m_levels[i].name == _levelName)
			{
				ClearLevel(m_levels[i]);
			}
		}
	}

	public static void RemoveLevel(string _levelName)
	{
		for (int i = 0; i < m_levels.Count; i++)
		{
			if (m_levels[i].name == _levelName)
			{
				RemoveLevel(m_levels[i]);
			}
		}
	}

	public static void RemoveLevel(ILevel _level)
	{
		ClearLevel(_level);
		m_levels.Remove(_level);
		if (m_levels.Count == 0)
		{
			GES.m_uniqueId = 0u;
		}
	}

	public static void RemoveLevels()
	{
		while (m_levels.Count > 0)
		{
			int index = m_levels.Count - 1;
			ClearLevel(m_levels[index]);
			m_levels.RemoveAt(index);
		}
		GES.m_uniqueId = 0u;
	}

	public static void SaveLevelData(ILevel _levelData, string _fileName)
	{
		Main.m_currentGame.SaveLevel(_levelData, _fileName);
	}
}

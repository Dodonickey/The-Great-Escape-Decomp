using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class GELevel : ILevel, ISerializable
{
	private string _name;

	private string _projectCode;

	private uint _levelIndex;

	private uint _chapterIndex;

	public List<EIC> items;

	public List<EIC> connections;

	public List<string> requiredResources;

	public string name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public string projectCode
	{
		get
		{
			return _projectCode;
		}
		set
		{
			_projectCode = value;
		}
	}

	public uint levelIndex
	{
		get
		{
			return _levelIndex;
		}
		set
		{
			_levelIndex = value;
		}
	}

	public uint chapterIndex
	{
		get
		{
			return _chapterIndex;
		}
		set
		{
			_chapterIndex = value;
		}
	}

	public GELevel()
	{
	}

	public GELevel(SerializationInfo info, StreamingContext ctxt)
	{
		name = (string)info.GetValue("name", typeof(string));
		projectCode = (string)info.GetValue("projectCode", typeof(string));
		EIC[] collection = (EIC[])info.GetValue("items", typeof(EIC[]));
		items = new List<EIC>(collection);
		EIC[] collection2 = (EIC[])info.GetValue("connections", typeof(EIC[]));
		connections = new List<EIC>(collection2);
		try
		{
			chapterIndex = (uint)info.GetValue("chapterIndex", typeof(uint));
			levelIndex = (uint)info.GetValue("levelIndex", typeof(uint));
		}
		catch
		{
			chapterIndex = 0u;
			levelIndex = 0u;
		}
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("name", name);
		info.AddValue("chapterIndex", chapterIndex);
		info.AddValue("levelIndex", levelIndex);
		info.AddValue("projectCode", projectCode);
		info.AddValue("items", items.ToArray());
		info.AddValue("connections", connections.ToArray());
	}
}

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class BasicLevelData : ISerializable, ILevelData
{
	private uint _dataType;

	private string _name;

	private uint _id;

	private bool _active;

	private bool _initialized;

	private Vertex3 _position;

	private Vertex3 _rotation;

	private Vertex3 _scale;

	public uint dataType
	{
		get
		{
			return _dataType;
		}
		set
		{
			_dataType = value;
		}
	}

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

	public uint id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
		}
	}

	public bool initialized
	{
		get
		{
			return _initialized;
		}
		set
		{
			_initialized = value;
		}
	}

	public Vertex3 position
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
		}
	}

	public Vertex3 rotation
	{
		get
		{
			return _rotation;
		}
		set
		{
			_rotation = value;
		}
	}

	public Vertex3 scale
	{
		get
		{
			return _scale;
		}
		set
		{
			_scale = value;
		}
	}

	public BasicLevelData()
	{
		dataType = 0u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
	}

	public BasicLevelData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
	}

	public virtual ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (BasicLevelData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public void Init(uint _id, string _name)
	{
		id = _id;
		name = _name;
		initialized = true;
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("dataType", dataType);
		info.AddValue("name", name);
		info.AddValue("id", id);
		info.AddValue("active", active);
		info.AddValue("position", position);
		info.AddValue("rotation", rotation);
		info.AddValue("scale", scale);
	}
}

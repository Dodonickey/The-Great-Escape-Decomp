using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class ConnectionData : BasicLevelData
{
	public uint startId;

	public uint endId;

	public uint startType;

	public uint endType;

	public ConnectionData()
	{
		dataType = 2u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
	}

	public ConnectionData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		startId = (uint)info.GetValue("startId", typeof(uint));
		endId = (uint)info.GetValue("endId", typeof(uint));
		startType = (uint)info.GetValue("startType", typeof(uint));
		endType = (uint)info.GetValue("endType", typeof(uint));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (ConnectionData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("startId", startId);
		info.AddValue("endId", endId);
		info.AddValue("startType", startType);
		info.AddValue("endType", endType);
	}
}

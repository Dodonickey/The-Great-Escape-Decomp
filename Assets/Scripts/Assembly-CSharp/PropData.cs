using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class PropData : BasicLevelData
{
	public int random;

	public uint color;

	public int location;

	public bool isPrefab;

	public bool isSpritePrefab;

	public string assetIdentifier;

	public PropData()
	{
		dataType = 5u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
		location = 2;
	}

	public PropData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		color = (uint)info.GetValue("color", typeof(uint));
		random = (int)info.GetValue("random", typeof(int));
		location = (int)info.GetValue("location", typeof(int));
		isPrefab = (bool)info.GetValue("isPrefab", typeof(bool));
		isSpritePrefab = (bool)info.GetValue("isSpritePrefab", typeof(bool));
		assetIdentifier = (string)info.GetValue("assetIdentifier", typeof(string));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            BinaryFormatter binaryFormatter = GELevelSerializer.CreateFormatter();
            binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (PropData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("color", color);
		info.AddValue("random", random);
		info.AddValue("location", location);
		info.AddValue("isPrefab", isPrefab);
		info.AddValue("isSpritePrefab", isSpritePrefab);
		info.AddValue("assetIdentifier", assetIdentifier);
	}
}

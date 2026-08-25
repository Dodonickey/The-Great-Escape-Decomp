using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class BlobData : BasicLevelData
{
	public uint blobType;

	public float radius;

	public float friction;

	public float elasticy;

	public float minElasticy;

	public float shapeDamp;

	public float segmentLength;

	public BlobData()
	{
		dataType = 30u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
	}

	public BlobData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		blobType = (uint)info.GetValue("blobType", typeof(uint));
		radius = (float)info.GetValue("radius", typeof(float));
		friction = (float)info.GetValue("friction", typeof(float));
		elasticy = (float)info.GetValue("elasticy", typeof(float));
		minElasticy = (float)info.GetValue("minElasticy", typeof(float));
		shapeDamp = (float)info.GetValue("shapeDamp", typeof(float));
		segmentLength = (float)info.GetValue("segmentLength", typeof(float));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (BlobData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("blobType", blobType);
		info.AddValue("radius", radius);
		info.AddValue("friction", friction);
		info.AddValue("elasticy", elasticy);
		info.AddValue("minElasticy", minElasticy);
		info.AddValue("shapeDamp", shapeDamp);
		info.AddValue("segmentLength", segmentLength);
	}
}

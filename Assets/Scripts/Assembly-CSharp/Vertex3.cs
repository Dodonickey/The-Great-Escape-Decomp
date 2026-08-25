using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Vertex3 : ISerializable
{
	public float x;

	public float y;

	public float z;

	public Vertex3()
	{
	}

	public Vertex3(Vector2 _v2)
	{
		x = _v2.x;
		y = _v2.y;
		z = 0f;
	}

	public Vertex3(Vector3 _v3)
	{
		x = _v3.x;
		y = _v3.y;
		z = _v3.z;
	}

	public Vertex3(SerializationInfo info, StreamingContext ctxt)
	{
		x = (float)info.GetValue("x", typeof(float));
		y = (float)info.GetValue("y", typeof(float));
		z = (float)info.GetValue("z", typeof(float));
	}

	public Vector2 ToVector2()
	{
		return new Vector2(x, y);
	}

	public Vector3 ToVector3()
	{
		return new Vector3(x, y, z);
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("x", x);
		info.AddValue("y", y);
		info.AddValue("z", z);
	}
}

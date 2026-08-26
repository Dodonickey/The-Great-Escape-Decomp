using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class ShipData : BasicLevelData
{
	public int plrIdx;

	public int accSpeed;

	public int maxSpeed;

	public float rotSpeed;

	public int breakStrength;

	public int frameSpeed;

	public int bulletSpeed;

	public int health;

	public int bulletLifetime;

	public int firingDelay;

	public ShipData()
	{
		dataType = 20u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
		plrIdx = 0;
		accSpeed = 600;
		maxSpeed = 1000;
		rotSpeed = 2.5f;
		breakStrength = 10;
		frameSpeed = 1;
		bulletSpeed = 300;
		health = 10;
		bulletLifetime = 50;
		firingDelay = 0;
	}

	public ShipData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		plrIdx = (int)info.GetValue("plrIdx", typeof(int));
		maxSpeed = (int)info.GetValue("maxSpeed", typeof(int));
		accSpeed = (int)info.GetValue("accSpeed", typeof(int));
		rotSpeed = (float)info.GetValue("rotSpeed", typeof(float));
		breakStrength = (int)info.GetValue("breakStrength", typeof(int));
		frameSpeed = (int)info.GetValue("frameSpeed", typeof(int));
		bulletSpeed = (int)info.GetValue("bulletSpeed", typeof(int));
		bulletLifetime = (int)info.GetValue("bulletLifetime", typeof(int));
		health = (int)info.GetValue("health", typeof(int));
		firingDelay = (int)info.GetValue("firingDelay", typeof(int));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            BinaryFormatter binaryFormatter = GELevelSerializer.CreateFormatter();
            binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (ShapeData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("plrIdx", plrIdx);
		info.AddValue("accSpeed", accSpeed);
		info.AddValue("maxSpeed", maxSpeed);
		info.AddValue("rotSpeed", rotSpeed);
		info.AddValue("breakStrength", breakStrength);
		info.AddValue("frameSpeed", frameSpeed);
		info.AddValue("bulletSpeed", bulletSpeed);
		info.AddValue("bulletLifetime", bulletLifetime);
		info.AddValue("health", health);
		info.AddValue("firingDelay", firingDelay);
	}
}

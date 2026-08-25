using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class ShapeData : BasicLevelData
{
	public bool tiled;

	public int tileSize;

	public Polygon polygon;

	public GroundSettings groundSettings;

	public bool convex;

	public bool separate;

	public bool isStatic;

	public uint colliderGroup;

	public uint colliderLayer;

	public bool isOneWay;

	public Vertex3 oneWayDirection;

	public Vertex3 gravity;

	public Vertex3 linearDamp;

	public float angularDamp;

	public bool isBreakable;

	public float breakingImpulse;

	public uint breakEventType;

	public Vertex3 breakEventDirection;

	public float breakEventForce;

	public bool isPowerLane;

	public uint powerLaneType;

	public Vertex3 powerLaneDirection;

	public float powerLaneForce;

	public ShapeData()
	{
		dataType = 7u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
	}

	public ShapeData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		tiled = (bool)info.GetValue("tiled", typeof(bool));
		tileSize = (int)info.GetValue("tileSize", typeof(int));
		polygon = (Polygon)info.GetValue("polygon", typeof(Polygon));
		groundSettings = (GroundSettings)info.GetValue("groundSettings", typeof(GroundSettings));
		convex = (bool)info.GetValue("convex", typeof(bool));
		separate = (bool)info.GetValue("separate", typeof(bool));
		isStatic = (bool)info.GetValue("isStatic", typeof(bool));
		colliderGroup = (uint)info.GetValue("colliderGroup", typeof(uint));
		colliderLayer = (uint)info.GetValue("colliderLayer", typeof(uint));
		isOneWay = (bool)info.GetValue("isOneWay", typeof(bool));
		oneWayDirection = (Vertex3)info.GetValue("oneWayDirection", typeof(Vertex3));
		gravity = (Vertex3)info.GetValue("gravity", typeof(Vertex3));
		linearDamp = (Vertex3)info.GetValue("LinearDamp", typeof(Vertex3));
		angularDamp = (float)info.GetValue("angularDamp", typeof(float));
		isBreakable = (bool)info.GetValue("isBreakable", typeof(bool));
		breakingImpulse = (float)info.GetValue("breakingImpulse", typeof(float));
		breakEventType = (uint)info.GetValue("breakEventType", typeof(uint));
		breakEventDirection = (Vertex3)info.GetValue("breakEventDirection", typeof(Vertex3));
		breakEventForce = (float)info.GetValue("breakEventForce", typeof(float));
		isPowerLane = (bool)info.GetValue("isPowerLane", typeof(bool));
		powerLaneType = (uint)info.GetValue("powerLaneType", typeof(uint));
		powerLaneDirection = (Vertex3)info.GetValue("powerLaneDirection", typeof(Vertex3));
		powerLaneForce = (float)info.GetValue("powerLaneForce", typeof(float));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (ShapeData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("tiled", tiled);
		info.AddValue("tileSize", tileSize);
		info.AddValue("polygon", polygon);
		info.AddValue("groundSettings", groundSettings);
		info.AddValue("convex", convex);
		info.AddValue("separate", separate);
		info.AddValue("isStatic", isStatic);
		info.AddValue("colliderGroup", colliderGroup);
		info.AddValue("colliderLayer", colliderLayer);
		info.AddValue("isOneWay", isOneWay);
		info.AddValue("oneWayDirection", oneWayDirection);
		info.AddValue("gravity", gravity);
		info.AddValue("LinearDamp", linearDamp);
		info.AddValue("angularDamp", angularDamp);
		info.AddValue("isBreakable", isBreakable);
		info.AddValue("breakingImpulse", breakingImpulse);
		info.AddValue("breakEventType", breakEventType);
		info.AddValue("breakEventDirection", breakEventDirection);
		info.AddValue("breakEventForce", breakEventForce);
		info.AddValue("isPowerLane", isPowerLane);
		info.AddValue("powerLaneType", powerLaneType);
		info.AddValue("powerLaneDirection", powerLaneDirection);
		info.AddValue("powerLaneForce", powerLaneForce);
	}
}

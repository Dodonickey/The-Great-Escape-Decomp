using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class VoxelData : ShapeData
{
	public byte[,,] map;

	public Color[,,] colors;

	public int width;

	public int height;

	public int depth;

	public float voxelScale;

	public bool freeSculpt;

	public bool isPhysical;

	public byte iso;

	public VoxelData()
	{
		dataType = 10u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
		map = null;
		colors = null;
		width = 64;
		height = 64;
		depth = 9;
		tileSize = 32;
		voxelScale = 10f;
		freeSculpt = true;
		isPhysical = true;
		iso = 128;
	}

	public VoxelData(SerializationInfo info, StreamingContext ctxt)
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
		map = (byte[,,])info.GetValue("map", typeof(byte[,,]));
		colors = (Color[,,])info.GetValue("colors", typeof(Color[,,]));
		width = (int)info.GetValue("width", typeof(int));
		height = (int)info.GetValue("height", typeof(int));
		depth = (int)info.GetValue("depth", typeof(int));
		voxelScale = (float)info.GetValue("voxelScale", typeof(float));
		freeSculpt = (bool)info.GetValue("freeSculpt", typeof(bool));
		isPhysical = (bool)info.GetValue("isPhysical", typeof(bool));
		iso = (byte)info.GetValue("iso", typeof(byte));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (VoxelData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("map", map);
		info.AddValue("colors", colors);
		info.AddValue("width", width);
		info.AddValue("height", height);
		info.AddValue("depth", depth);
		info.AddValue("voxelScale", voxelScale);
		info.AddValue("freeSculpt", freeSculpt);
		info.AddValue("isPhysical", isPhysical);
		info.AddValue("iso", iso);
	}
}

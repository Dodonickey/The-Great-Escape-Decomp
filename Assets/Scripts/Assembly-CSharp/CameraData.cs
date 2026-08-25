using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class CameraData : TriggerData
{
	public Vertex3 offset;

	public Vertex3 rotationOffset;

	public Vertex3 lowVelocity;

	public Vertex3 highVelocity;

	public float destinationSmooth;

	public float directionalSmooth;

	public float lowVelocityDistance;

	public float highVelocityDistance;

	public float directionalOffset;

	public float maxDisplacement;

	public bool keepDirOffsetUntilLowVelocity;

	public int border;

	public bool keepInside;

	public CameraData()
	{
		dataType = 1u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
	}

	public CameraData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotationOffset = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		triggerType = (uint)info.GetValue("triggerType", typeof(uint));
		connect = (bool)info.GetValue("connect", typeof(bool));
		shapeType = (int)info.GetValue("shapeType", typeof(int));
		toggle = (bool)info.GetValue("toggle", typeof(bool));
		triggerOnlyOnce = (bool)info.GetValue("triggerOnlyOnce", typeof(bool));
		triggerOnlyOnFullEnergy = (bool)info.GetValue("triggerOnlyOnFullEnergy", typeof(bool));
		triggerUntilOutOfEnergy = (bool)info.GetValue("triggerUntilOutOfEnergy", typeof(bool));
		action = (int)info.GetValue("action", typeof(int));
		autoTrigger = (bool)info.GetValue("autoTrigger", typeof(bool));
		energyGain = (float)info.GetValue("powerGain", typeof(float));
		energyConsume = (float)info.GetValue("powerConsume", typeof(float));
		gainInterval = (float)info.GetValue("gainInterval", typeof(float));
		consumeInterval = (float)info.GetValue("consumeInterval", typeof(float));
		cooldown = (float)info.GetValue("cooldown", typeof(float));
		energy = (float)info.GetValue("power", typeof(float));
		energyClips = (int)info.GetValue("energyClips", typeof(int));
		reloadCooldown = (float)info.GetValue("reloadCooldown", typeof(float));
		triggerCooldown = (float)info.GetValue("triggerCooldown", typeof(float));
		energyMultipler = (float)info.GetValue("powerMultipler", typeof(float));
		eventIdentifier = (string)info.GetValue("eventIdentifier", typeof(string));
		eventDispatchOnlyOnce = (bool)info.GetValue("eventDispatchOnlyOnce", typeof(bool));
		eventDispatchDelay = (float)info.GetValue("eventDispatchDelay", typeof(float));
		eventTarget = (string)info.GetValue("eventTarget", typeof(string));
		offset = (Vertex3)info.GetValue("offset", typeof(Vertex3));
		rotationOffset = (Vertex3)info.GetValue("rotationOffset", typeof(Vertex3));
		lowVelocity = (Vertex3)info.GetValue("lowVelocity", typeof(Vertex3));
		highVelocity = (Vertex3)info.GetValue("highVelocity", typeof(Vertex3));
		destinationSmooth = (float)info.GetValue("destinationSmooth", typeof(float));
		directionalSmooth = (float)info.GetValue("directionalSmooth", typeof(float));
		lowVelocityDistance = (float)info.GetValue("lowVelocityDistance", typeof(float));
		highVelocityDistance = (float)info.GetValue("highVelocityDistance", typeof(float));
		directionalOffset = (float)info.GetValue("directionalOffset", typeof(float));
		maxDisplacement = (float)info.GetValue("maxDisplacement", typeof(float));
		keepDirOffsetUntilLowVelocity = (bool)info.GetValue("keepDirOffsetUntilLowVelocity", typeof(bool));
		border = (int)info.GetValue("border", typeof(int));
		keepInside = (bool)info.GetValue("keepInside", typeof(bool));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (CameraData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("offset", offset);
		info.AddValue("rotationOffset", rotationOffset);
		info.AddValue("lowVelocity", lowVelocity);
		info.AddValue("highVelocity", highVelocity);
		info.AddValue("destinationSmooth", destinationSmooth);
		info.AddValue("directionalSmooth", directionalSmooth);
		info.AddValue("lowVelocityDistance", lowVelocityDistance);
		info.AddValue("highVelocityDistance", highVelocityDistance);
		info.AddValue("directionalOffset", directionalOffset);
		info.AddValue("maxDisplacement", maxDisplacement);
		info.AddValue("keepDirOffsetUntilLowVelocity", keepDirOffsetUntilLowVelocity);
		info.AddValue("border", border);
		info.AddValue("keepInside", keepInside);
	}
}

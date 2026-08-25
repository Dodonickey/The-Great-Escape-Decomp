using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class ControlSchemeData : TriggerData
{
	public int playerIndex;

	public string playerName;

	public ControlSchemeData()
	{
		dataType = 4u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
	}

	public ControlSchemeData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
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
		playerIndex = (int)info.GetValue("playerIndex", typeof(int));
		playerName = (string)info.GetValue("playerName", typeof(string));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (ControlSchemeData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("playerIndex", playerIndex);
		info.AddValue("playerName", playerName);
	}
}

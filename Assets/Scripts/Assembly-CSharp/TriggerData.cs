using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class TriggerData : BasicLevelData
{
	public uint triggerType;

	public bool connect;

	public int shapeType;

	public Polygon shape;

	public uint colliderType;

	public bool toggle;

	public bool triggerOnlyOnce;

	public bool triggerOnlyOnFullEnergy;

	public bool triggerUntilOutOfEnergy;

	public int action;

	public bool autoTrigger;

	public float energyGain;

	public float energyConsume;

	public float gainInterval;

	public float consumeInterval;

	public float cooldown;

	public float energy;

	public int energyClips;

	public float reloadCooldown;

	public float triggerCooldown;

	public float energyMultipler;

	public string eventIdentifier;

	public bool eventDispatchOnlyOnce;

	public float eventDispatchDelay;

	public string eventTarget;

	public Vertex3 defaultNumericValue;

	public string defaultTextualValue;

	public TriggerData()
	{
		dataType = 8u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
		defaultNumericValue = new Vertex3(Vector3.one);
		active = true;
		toggle = false;
		triggerOnlyOnce = false;
		triggerUntilOutOfEnergy = false;
		triggerOnlyOnFullEnergy = false;
		autoTrigger = false;
		energy = 1f;
		energyClips = -1;
		energyGain = 0f;
		energyConsume = 0f;
		gainInterval = 0f;
		consumeInterval = 0f;
		cooldown = 0f;
	}

	public TriggerData(SerializationInfo info, StreamingContext ctxt)
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
		shape = (Polygon)info.GetValue("shape", typeof(Polygon));
		colliderType = (uint)info.GetValue("colliderType", typeof(uint));
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
		try
		{
			defaultNumericValue = (Vertex3)info.GetValue("defaultNumericValue", typeof(Vertex3));
			defaultTextualValue = (string)info.GetValue("defaultTextualValue", typeof(string));
		}
		catch
		{
			defaultNumericValue = new Vertex3(Vector3.one);
			defaultTextualValue = string.Empty;
		}
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (TriggerData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("triggerType", triggerType);
		info.AddValue("connect", connect);
		info.AddValue("shapeType", shapeType);
		info.AddValue("shape", shape);
		info.AddValue("colliderType", colliderType);
		info.AddValue("toggle", toggle);
		info.AddValue("triggerOnlyOnce", triggerOnlyOnce);
		info.AddValue("triggerOnlyOnFullEnergy", triggerOnlyOnFullEnergy);
		info.AddValue("triggerUntilOutOfEnergy", triggerUntilOutOfEnergy);
		info.AddValue("action", action);
		info.AddValue("autoTrigger", autoTrigger);
		info.AddValue("powerGain", energyGain);
		info.AddValue("powerConsume", energyConsume);
		info.AddValue("gainInterval", gainInterval);
		info.AddValue("consumeInterval", consumeInterval);
		info.AddValue("cooldown", cooldown);
		info.AddValue("power", energy);
		info.AddValue("energyClips", energyClips);
		info.AddValue("reloadCooldown", reloadCooldown);
		info.AddValue("triggerCooldown", triggerCooldown);
		info.AddValue("powerMultipler", energyMultipler);
		info.AddValue("eventIdentifier", eventIdentifier);
		info.AddValue("eventDispatchOnlyOnce", eventDispatchOnlyOnce);
		info.AddValue("eventDispatchDelay", eventDispatchDelay);
		info.AddValue("eventTarget", eventTarget);
		info.AddValue("defaultNumericValue", defaultNumericValue);
		info.AddValue("defaultTextualValue", defaultTextualValue);
	}
}

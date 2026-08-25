using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class PhysicsAffectorData : TriggerData
{
	public bool isImpulse;

	public bool isForce;

	public bool isVelocity;

	public bool isAngularVelocity;

	public Vertex3 point;

	public Vertex3 direction;

	public float amount;

	public float duration;

	public bool relative;

	public PhysicsAffectorData()
	{
		dataType = 11u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
		point = new Vertex3(Vector3.zero);
		direction = new Vertex3(Vector2.one);
	}

	public PhysicsAffectorData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		try
		{
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
		}
		catch
		{
			triggerType = 30u;
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
		isImpulse = (bool)info.GetValue("isImpulse", typeof(bool));
		isForce = (bool)info.GetValue("isForce", typeof(bool));
		isVelocity = (bool)info.GetValue("isVelocity", typeof(bool));
		isAngularVelocity = (bool)info.GetValue("isAngularVelocity", typeof(bool));
		isImpulse = (bool)info.GetValue("isImpulse", typeof(bool));
		point = (Vertex3)info.GetValue("point", typeof(Vertex3));
		direction = (Vertex3)info.GetValue("direction", typeof(Vertex3));
		amount = (float)info.GetValue("amount", typeof(float));
		duration = (float)info.GetValue("duration", typeof(float));
		relative = (bool)info.GetValue("relative", typeof(bool));
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
		info.AddValue("isImpulse", isImpulse);
		info.AddValue("isForce", isForce);
		info.AddValue("isVelocity", isVelocity);
		info.AddValue("isAngularVelocity", isAngularVelocity);
		info.AddValue("point", point);
		info.AddValue("direction", direction);
		info.AddValue("amount", amount);
		info.AddValue("duration", duration);
		info.AddValue("relative", isImpulse);
	}
}

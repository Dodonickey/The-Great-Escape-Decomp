using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class ConstraintData : BasicLevelData
{
	public uint constraintType;

	public bool connectToWorld;

	public bool softConnection;

	public float softConnectionStrength;

	public float softConnectionDamp;

	public float rotaryStiffness;

	public bool rotaryLimit;

	public float rotaryLimitMin;

	public float rotaryLimitMax;

	public bool rotarySpring;

	public float rotarySpringStrength;

	public float rotarySpringDamp;

	public bool motorIsStiff;

	public bool linearMotor;

	public bool linearMotorEnabled;

	public float linearMotorMaxForce;

	public float linearMotorRate;

	public uint linearMotorStartIndex;

	public float linearMotorStartPos;

	public bool linearMotorStartDir;

	public bool linearMotorLoop;

	public float waitAtPoints;

	public bool railClosed;

	public int railInterpolationStyle;

	public int railRepeats;

	public bool ropeIsRigid;

	public float ropeMaxLength;

	public float ropeMinLength;

	public bool ropeIsFlexible;

	public float ropeFlexRestLength;

	public float ropeFlexForce;

	public float ropeFlexDamp;

	public bool ropeHasLimits;

	public bool ropeIsCuttable;

	public bool rotaryMotor;

	public bool rotaryMotorEnabled;

	public float rotaryMotorMaxForce;

	public float rotaryMotorRate;

	public float rotaryMotorStartAngle;

	public bool rotaryMotorOneShot;

	public int rotaryMotorLoopStyle;

	public int rotaryMotorRepeats;

	public ConstraintData()
	{
		dataType = 3u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
	}

	public ConstraintData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		constraintType = (uint)info.GetValue("constraintType", typeof(uint));
		connectToWorld = (bool)info.GetValue("connectToWorld", typeof(bool));
		softConnection = (bool)info.GetValue("softConnection", typeof(bool));
		softConnectionStrength = (float)info.GetValue("softConnectionStrength", typeof(float));
		softConnectionDamp = (float)info.GetValue("softConnectionDamp", typeof(float));
		linearMotor = (bool)info.GetValue("LinearMotor", typeof(bool));
		linearMotorEnabled = (bool)info.GetValue("LinearMotorEnabled", typeof(bool));
		linearMotorMaxForce = (float)info.GetValue("LinearMotorMaxForce", typeof(float));
		linearMotorRate = (float)info.GetValue("LinearMotorRate", typeof(float));
		linearMotorStartIndex = (uint)info.GetValue("LinearMotorStartIndex", typeof(uint));
		linearMotorStartPos = (float)info.GetValue("LinearMotorStartPos", typeof(float));
		linearMotorStartDir = (bool)info.GetValue("LinearMotorStartDir", typeof(bool));
		linearMotorLoop = (bool)info.GetValue("LinearMotorLoop", typeof(bool));
		railClosed = (bool)info.GetValue("railOneShot", typeof(bool));
		railInterpolationStyle = (int)info.GetValue("railLoopStyle", typeof(int));
		railRepeats = (int)info.GetValue("railRepeats", typeof(int));
		ropeIsRigid = (bool)info.GetValue("isRigid", typeof(bool));
		ropeMaxLength = (float)info.GetValue("maxLength", typeof(float));
		ropeMinLength = (float)info.GetValue("minLength", typeof(float));
		ropeIsFlexible = (bool)info.GetValue("isFlexible", typeof(bool));
		ropeFlexRestLength = (float)info.GetValue("flexLength", typeof(float));
		ropeFlexForce = (float)info.GetValue("flexForce", typeof(float));
		ropeFlexDamp = (float)info.GetValue("flexDamp", typeof(float));
		ropeHasLimits = (bool)info.GetValue("ropeHasLimits", typeof(bool));
		ropeIsCuttable = (bool)info.GetValue("ropeIsCuttable", typeof(bool));
		rotaryStiffness = (float)info.GetValue("rotaryStiffness", typeof(float));
		rotaryLimit = (bool)info.GetValue("rotaryLimit", typeof(bool));
		rotaryLimitMin = (float)info.GetValue("rotaryLimitMin", typeof(float));
		rotaryLimitMax = (float)info.GetValue("rotaryLimitMax", typeof(float));
		rotarySpring = (bool)info.GetValue("rotarySpring", typeof(bool));
		rotarySpringStrength = (float)info.GetValue("rotarySpringStrength", typeof(float));
		rotarySpringDamp = (float)info.GetValue("rotarySpringDamp", typeof(float));
		rotaryMotor = (bool)info.GetValue("rotaryMotor", typeof(bool));
		rotaryMotorEnabled = (bool)info.GetValue("rotaryMotorEnabled", typeof(bool));
		rotaryMotorMaxForce = (float)info.GetValue("rotaryMotorMaxForce", typeof(float));
		rotaryMotorRate = (float)info.GetValue("rotaryMotorRate", typeof(float));
		rotaryMotorStartAngle = (float)info.GetValue("rotaryMotorStartAngle", typeof(float));
		rotaryMotorOneShot = (bool)info.GetValue("rotaryMotorOneShot", typeof(bool));
		rotaryMotorLoopStyle = (int)info.GetValue("rotaryMotorLoopStyle", typeof(int));
		rotaryMotorRepeats = (int)info.GetValue("rotaryMotorRepeats", typeof(int));
		try
		{
			motorIsStiff = (bool)info.GetValue("motorIsStiff", typeof(bool));
		}
		catch
		{
			motorIsStiff = true;
		}
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            BinaryFormatter binaryFormatter = GELevelSerializer.CreateFormatter();
            binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (ConstraintData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("constraintType", constraintType);
		info.AddValue("connectToWorld", connectToWorld);
		info.AddValue("softConnection", softConnection);
		info.AddValue("softConnectionStrength", softConnectionStrength);
		info.AddValue("softConnectionDamp", softConnectionDamp);
		info.AddValue("LinearMotor", linearMotor);
		info.AddValue("LinearMotorEnabled", linearMotorEnabled);
		info.AddValue("LinearMotorMaxForce", linearMotorMaxForce);
		info.AddValue("LinearMotorRate", linearMotorRate);
		info.AddValue("LinearMotorStartIndex", linearMotorStartIndex);
		info.AddValue("LinearMotorStartPos", linearMotorStartPos);
		info.AddValue("LinearMotorStartDir", linearMotorStartDir);
		info.AddValue("LinearMotorLoop", linearMotorLoop);
		info.AddValue("railOneShot", railClosed);
		info.AddValue("railLoopStyle", railInterpolationStyle);
		info.AddValue("railRepeats", railRepeats);
		info.AddValue("isRigid", ropeIsRigid);
		info.AddValue("maxLength", ropeMaxLength);
		info.AddValue("minLength", ropeMinLength);
		info.AddValue("isFlexible", ropeIsFlexible);
		info.AddValue("flexLength", ropeFlexRestLength);
		info.AddValue("flexForce", ropeFlexForce);
		info.AddValue("flexDamp", ropeFlexDamp);
		info.AddValue("ropeHasLimits", ropeHasLimits);
		info.AddValue("ropeIsCuttable", ropeIsCuttable);
		info.AddValue("rotaryStiffness", rotaryStiffness);
		info.AddValue("rotaryLimit", rotaryLimit);
		info.AddValue("rotaryLimitMin", rotaryLimitMin);
		info.AddValue("rotaryLimitMax", rotaryLimitMax);
		info.AddValue("rotarySpring", rotarySpring);
		info.AddValue("rotarySpringStrength", rotarySpringStrength);
		info.AddValue("rotarySpringDamp", rotarySpringDamp);
		info.AddValue("rotaryMotor", rotaryMotor);
		info.AddValue("rotaryMotorEnabled", rotaryMotorEnabled);
		info.AddValue("rotaryMotorMaxForce", rotaryMotorMaxForce);
		info.AddValue("rotaryMotorRate", rotaryMotorRate);
		info.AddValue("rotaryMotorStartAngle", rotaryMotorStartAngle);
		info.AddValue("rotaryMotorOneShot", rotaryMotorOneShot);
		info.AddValue("rotaryMotorLoopStyle", rotaryMotorLoopStyle);
		info.AddValue("rotaryMotorRepeats", rotaryMotorRepeats);
		info.AddValue("motorIsStiff", motorIsStiff);
	}
}

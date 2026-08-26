using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class ConstraintPointData : BasicLevelData
{
	public int anchorIndex;

	public uint anchorType;

	public float velocityMultipler;

	public float waitAtPoint;

	public int entryEasingType;

	public int exitEasingType;

	public int interpolationType;

	public ConstraintPointData(AnchorType _anchorType)
	{
		dataType = 6u;
		position = new Vertex3(Vector3.zero);
		rotation = new Vertex3(Vector3.zero);
		scale = new Vertex3(Vector3.one);
		anchorType = (uint)_anchorType;
	}

	public ConstraintPointData(SerializationInfo info, StreamingContext ctxt)
	{
		dataType = (uint)info.GetValue("dataType", typeof(uint));
		anchorType = (uint)info.GetValue("anchorType", typeof(uint));
		name = (string)info.GetValue("name", typeof(string));
		id = (uint)info.GetValue("id", typeof(uint));
		active = (bool)info.GetValue("active", typeof(bool));
		position = (Vertex3)info.GetValue("position", typeof(Vertex3));
		rotation = (Vertex3)info.GetValue("rotation", typeof(Vertex3));
		scale = (Vertex3)info.GetValue("scale", typeof(Vertex3));
		anchorIndex = (int)info.GetValue("anchorIndex", typeof(int));
		velocityMultipler = (float)info.GetValue("velocityMultipler", typeof(float));
		waitAtPoint = (float)info.GetValue("waitAtPoint", typeof(float));
		entryEasingType = (int)info.GetValue("entryEasingType", typeof(int));
		exitEasingType = (int)info.GetValue("exitEasingType", typeof(int));
		interpolationType = (int)info.GetValue("interpolationType", typeof(int));
	}

	public override ILevelData DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            BinaryFormatter binaryFormatter = GELevelSerializer.CreateFormatter();
            binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (ConstraintPointData)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("anchorIndex", anchorIndex);
		info.AddValue("anchorType", anchorType);
		info.AddValue("velocityMultipler", velocityMultipler);
		info.AddValue("waitAtPoint", waitAtPoint);
		info.AddValue("entryEasingType", entryEasingType);
		info.AddValue("exitEasingType", exitEasingType);
		info.AddValue("interpolationType", interpolationType);
	}
}

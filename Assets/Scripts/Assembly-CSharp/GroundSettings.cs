using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class GroundSettings : ISerializable
{
	public uint groundType;

	public float elasticity;

	public float friction;

	public float density;

	public Vertex3 surfaceVelocity;

	public float angularDamp;

	public Vertex3 linearDamp;

	public uint effectIdentifier;

	public float effectInterval;

	public uint buffIdentifier;

	public float buffInterval;

	public float minAngle;

	public float minSegment;

	public float maxSegment;

	public int smooth;

	public bool hasRoad;

	public bool hasFill;

	public bool hasBelt;

	public uint beltType;

	public float beltWidth;

	public float beltDepth;

	public Vertex3 beltWeightDirection;

	public float roadScale;

	public float roadDepth;

	public string roadMaterialResourceIdentifier;

	public string fillMaterialResourceIdentifier;

	public float fillScale;

	public Vertex3 parallaxAmount;

	public uint color1;

	public uint color2;

	public GroundSettings(GroundType _type)
	{
		groundType = (uint)_type;
		switch (_type)
		{
		case GroundType.Solid:
			fillMaterialResourceIdentifier = "GrassFill";
			roadMaterialResourceIdentifier = "GrassRoad";
			parallaxAmount = new Vertex3(Vector2.one);
			break;
		case GroundType.Background:
			fillMaterialResourceIdentifier = "WoodPanel";
			roadMaterialResourceIdentifier = string.Empty;
			parallaxAmount = new Vertex3(Vector2.one);
			break;
		case GroundType.Landscape:
			fillMaterialResourceIdentifier = "Landscape1";
			roadMaterialResourceIdentifier = string.Empty;
			parallaxAmount = new Vertex3(new Vector2(0.85f, 1f));
			break;
		}
		roadScale = 1f;
		fillScale = 1f;
		hasRoad = true;
		hasFill = true;
		hasBelt = false;
		beltType = 0u;
		beltWidth = 0f;
		beltDepth = 0f;
		beltWeightDirection = new Vertex3(Vector2.up * 0.5f);
		roadDepth = 75f;
		color1 = 8421504u;
		color2 = 8421504u;
		elasticity = 1f;
		friction = 0.7f;
		density = 1f;
		minAngle = 5f;
		minSegment = 5f;
		maxSegment = 10f;
		smooth = 1;
		surfaceVelocity = new Vertex3(Vector3.zero);
		angularDamp = 0.99f;
		linearDamp = new Vertex3(Vector3.one * 0.995f);
		effectIdentifier = 0u;
	}

	public GroundSettings(SerializationInfo info, StreamingContext ctxt)
	{
		groundType = (uint)info.GetValue("groundType", typeof(uint));
		elasticity = (float)info.GetValue("elasticity", typeof(float));
		friction = (float)info.GetValue("friction", typeof(float));
		density = (float)info.GetValue("density", typeof(float));
		surfaceVelocity = (Vertex3)info.GetValue("surfaceVelocity", typeof(Vertex3));
		angularDamp = (float)info.GetValue("angularDamp", typeof(float));
		linearDamp = (Vertex3)info.GetValue("LinearDamp", typeof(Vertex3));
		effectInterval = (float)info.GetValue("effectInterval", typeof(float));
		effectIdentifier = (uint)info.GetValue("effectIdentifier", typeof(uint));
		buffInterval = (float)info.GetValue("buffInterval", typeof(float));
		buffIdentifier = (uint)info.GetValue("buffIdentifier", typeof(uint));
		roadMaterialResourceIdentifier = (string)info.GetValue("roadMaterialResourceIdentifier", typeof(string));
		fillMaterialResourceIdentifier = (string)info.GetValue("fillMaterialResourceIdentifier", typeof(string));
		minAngle = (float)info.GetValue("minAngle", typeof(float));
		minSegment = (float)info.GetValue("minSegment", typeof(float));
		maxSegment = (float)info.GetValue("maxSegment", typeof(float));
		smooth = (int)info.GetValue("smooth", typeof(int));
		parallaxAmount = (Vertex3)info.GetValue("parallaxAmount", typeof(Vertex3));
		color1 = (uint)info.GetValue("color1", typeof(uint));
		color2 = (uint)info.GetValue("color2", typeof(uint));
		roadScale = (float)info.GetValue("roadScale", typeof(float));
		fillScale = (float)info.GetValue("fillScale", typeof(float));
		hasRoad = (bool)info.GetValue("hasRoad", typeof(bool));
		hasFill = (bool)info.GetValue("hasFill", typeof(bool));
		hasBelt = (bool)info.GetValue("hasBelt", typeof(bool));
		beltType = (uint)info.GetValue("beltType", typeof(uint));
		beltWidth = (float)info.GetValue("beltWidth", typeof(float));
		beltDepth = (float)info.GetValue("beltSize", typeof(float));
		roadDepth = (float)info.GetValue("roadWidth", typeof(float));
		beltWeightDirection = (Vertex3)info.GetValue("beltWeightDirection", typeof(Vertex3));
		if (beltWidth == 0f)
		{
			hasBelt = false;
		}
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("groundType", groundType);
		info.AddValue("elasticity", elasticity);
		info.AddValue("friction", friction);
		info.AddValue("density", density);
		info.AddValue("surfaceVelocity", surfaceVelocity);
		info.AddValue("angularDamp", angularDamp);
		info.AddValue("LinearDamp", linearDamp);
		info.AddValue("effectInterval", effectInterval);
		info.AddValue("effectIdentifier", effectIdentifier);
		info.AddValue("buffInterval", buffInterval);
		info.AddValue("buffIdentifier", buffIdentifier);
		info.AddValue("roadMaterialResourceIdentifier", roadMaterialResourceIdentifier);
		info.AddValue("fillMaterialResourceIdentifier", fillMaterialResourceIdentifier);
		info.AddValue("minAngle", minAngle);
		info.AddValue("minSegment", minSegment);
		info.AddValue("maxSegment", maxSegment);
		info.AddValue("smooth", smooth);
		info.AddValue("parallaxAmount", parallaxAmount);
		info.AddValue("color1", color1);
		info.AddValue("color2", color2);
		info.AddValue("roadScale", roadScale);
		info.AddValue("fillScale", fillScale);
		info.AddValue("hasRoad", hasRoad);
		info.AddValue("hasFill", hasFill);
		info.AddValue("hasBelt", hasBelt);
		info.AddValue("beltType", beltType);
		info.AddValue("beltWidth", beltWidth);
		info.AddValue("beltSize", beltDepth);
		info.AddValue("roadWidth", roadDepth);
		info.AddValue("beltWeightDirection", beltWeightDirection);
	}
}

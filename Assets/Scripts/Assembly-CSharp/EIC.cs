using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class EIC : BasicComponent, ISerializable, IDeserializationCallback
{
	public uint itemType;

	public string identifier;

	public ILevelData data;

	public EIC container;

	public List<EIC> subItems;

	private EIC[] itemsFromDeserialization;

	public Camera camera;

	public TransformC TC;

	public TransformC uiTC;

	public List<IComponent> gameComponents;

	public TouchAreaC TAC;

	public bool isScaleable;

	public bool isScaleUnified;

	public bool isRotateable;

	public bool isRealtimeMovable;

	public bool isDrawable;

	public bool connectionMode;

	public AnchorPointInfo relativeToA;

	public AnchorPointInfo relativeToB;

	public ConnectionSlotType connectionSlotType;

	public IControlledComponent trigger;

	public int horizontalAnchor;

	public bool horizontalIsAbsolute;

	public int verticalAnchor;

	public bool verticalIsAbsolute;

	public float referenceWidth;

	public float referenceHeight;

	public EIC()
	{
		referenceWidth = Screen.width;
		referenceHeight = Screen.height;
	}

	public EIC(SerializationInfo info, StreamingContext ctxt)
	{
		itemType = (uint)info.GetValue("itemType", typeof(uint));
		identifier = (string)info.GetValue("identifier", typeof(string));
		isScaleable = (bool)info.GetValue("isScaleable", typeof(bool));
		isScaleUnified = (bool)info.GetValue("isScaleUnified", typeof(bool));
		isRotateable = (bool)info.GetValue("isRotateable", typeof(bool));
		isRealtimeMovable = (bool)info.GetValue("isRealtimeMovable", typeof(bool));
		isDrawable = (bool)info.GetValue("isDrawable", typeof(bool));
		connectionMode = (bool)info.GetValue("connectionMode", typeof(bool));
		try
		{
			horizontalAnchor = (int)info.GetValue("horizontalAnchor", typeof(int));
			horizontalIsAbsolute = (bool)info.GetValue("horizontalIsAbsolute", typeof(bool));
			verticalAnchor = (int)info.GetValue("verticalAnchor", typeof(int));
			verticalIsAbsolute = (bool)info.GetValue("verticalIsAbsolute", typeof(bool));
			referenceWidth = (float)info.GetValue("referenceWidth", typeof(float));
			referenceHeight = (float)info.GetValue("referenceHeight", typeof(float));
		}
		catch
		{
			referenceWidth = Screen.width;
			referenceHeight = Screen.height;
		}
		data = (BasicLevelData)info.GetValue("data", typeof(BasicLevelData));
		switch ((uint)info.GetValue("dataType", typeof(uint)))
		{
		case 1u:
			data = (CameraData)info.GetValue("data", typeof(CameraData));
			break;
		case 2u:
			data = (ConnectionData)info.GetValue("data", typeof(ConnectionData));
			break;
		case 3u:
			data = (ConstraintData)info.GetValue("data", typeof(ConstraintData));
			break;
		case 4u:
			data = (ControlSchemeData)info.GetValue("data", typeof(ControlSchemeData));
			break;
		case 6u:
			data = (ConstraintPointData)info.GetValue("data", typeof(ConstraintPointData));
			break;
		case 7u:
			data = (ShapeData)info.GetValue("data", typeof(ShapeData));
			break;
		case 8u:
			data = (TriggerData)info.GetValue("data", typeof(TriggerData));
			break;
		default:
			data = (BasicLevelData)info.GetValue("data", typeof(BasicLevelData));
			break;
		}
		itemsFromDeserialization = (EIC[])info.GetValue("items", typeof(EIC[]));
	}

	public EIC DeepCopy()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return (EIC)binaryFormatter.Deserialize(memoryStream);
		}
	}

	public void OnDeserialization(object sender)
	{
		subItems = new List<EIC>(itemsFromDeserialization);
		itemsFromDeserialization = null;
	}

	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("itemType", itemType);
		info.AddValue("identifier", identifier);
		info.AddValue("isScaleable", isScaleable);
		info.AddValue("isScaleUnified", isScaleUnified);
		info.AddValue("isRotateable", isRotateable);
		info.AddValue("isRealtimeMovable", isRealtimeMovable);
		info.AddValue("isDrawable", isDrawable);
		info.AddValue("connectionMode", connectionMode);
		info.AddValue("horizontalAnchor", horizontalAnchor);
		info.AddValue("horizontalIsAbsolute", horizontalIsAbsolute);
		info.AddValue("verticalAnchor", verticalAnchor);
		info.AddValue("verticalIsAbsolute", verticalIsAbsolute);
		info.AddValue("referenceWidth", referenceWidth);
		info.AddValue("referenceHeight", referenceHeight);
		info.AddValue("dataType", data.dataType);
		bool flag = false;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			flag = gEPlugin.GetObjectData(info, data);
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			if (data.dataType == 1)
			{
				info.AddValue("data", (CameraData)data);
			}
			else if (data.dataType == 2)
			{
				info.AddValue("data", (ConnectionData)data);
			}
			else if (data.dataType == 3)
			{
				info.AddValue("data", (ConstraintData)data);
			}
			else if (data.dataType == 4)
			{
				info.AddValue("data", (ControlSchemeData)data);
			}
			else if (data.dataType == 6)
			{
				info.AddValue("data", (ConstraintPointData)data);
			}
			else if (data.dataType == 7)
			{
				info.AddValue("data", (ShapeData)data);
			}
			else if (data.dataType == 8)
			{
				info.AddValue("data", (TriggerData)data);
			}
			else
			{
				info.AddValue("data", (BasicLevelData)data);
			}
		}
		info.AddValue("items", subItems.ToArray());
	}
}

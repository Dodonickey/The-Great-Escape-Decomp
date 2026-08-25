using System;
using UnityEngine;

public class PBPadA
{
	public static ChipmunkC Assemble(EIC _eic)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TriggerData triggerData = _eic.data as TriggerData;
		TransformC transformC = TransformS.AddComponent(entity);
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, false, (ColliderType)25, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, triggerData.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 20f, 5f, 0f, 0f, 999999u, 17895697u, true);
		ChipmunkWrapper.AddBoxShape(chipmunkC.cpBodyPtr, Vector2.right * 15f, 1f, 50f, 17f, 0.5f, 1f, 999999u, 17895697u, false);
		TransformS.SetTransform(transformC, triggerData.position.ToVector2(), triggerData.rotation.ToVector3(), chipmunkC.cpBodyPtr);
		float num = triggerData.rotation.z * ((float)Math.PI / 180f);
		float num2 = num;
		if (num2 > (float)Math.PI)
		{
			num2 -= (float)Math.PI * 2f;
		}
		bool flag = triggerData.rotation.z < 90f || triggerData.rotation.z > 270f;
		IntPtr zero = IntPtr.Zero;
		if (!flag)
		{
			zero = ChipmunkWrapper.AddPivotJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, triggerData.position.ToVector2());
			ChipmunkWrapper.AddRotaryLimitJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, num2 - 1.2217305f, num2);
		}
		else
		{
			zero = ChipmunkWrapper.AddPivotJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, triggerData.position.ToVector2());
			ChipmunkWrapper.AddRotaryLimitJoint(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, num2, num2 + 1.2217305f);
		}
		ChipmunkWrapper.SetConstraintProperties(zero, 0.0001f, float.PositiveInfinity, float.PositiveInfinity);
		IntPtr rotaryMotorPtr = ChipmunkWrapper.AddSimpleMotor(ChipmunkWrapper.GetSpaceStaticBody(), chipmunkC.cpBodyPtr, 0f, 90000000f);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, triggerData, TriggerType.RotaryMotorConstraint, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		GETriggerLogic.AddBeganEventDelegate(gETriggerC, HandlePadEvent);
		GETriggerLogic.AddEndEventDelegate(gETriggerC, HandlePadEvent);
		gETriggerC.autoTrigger = true;
		_eic.trigger = gETriggerC;
		PBSystem.AddPadComponent(chipmunkC, num2, flag, rotaryMotorPtr);
		SpriteC spriteC = SpriteS.AddComponent(transformC, new Frame(42f, 0f, 60f, 150f), PBState.pinballSheet);
		SpriteS.SetDimensions(spriteC, 20f, 50f);
		SpriteS.SetOffset(spriteC, Vector3.right * 15f, 90f);
		return chipmunkC;
	}

	public static void HandlePadEvent(BasicControlledComponent _c)
	{
		Entity entity = EntityManager.m_entities.m_array[_c.entityIndex];
		PBPadC pBPadC = null;
		for (int i = 0; i < entity.components.Count; i++)
		{
			if (entity.components[i].componentType == (ComponentType)70)
			{
				pBPadC = entity.components[i] as PBPadC;
				break;
			}
		}
		if (pBPadC != null)
		{
			pBPadC.isTriggered = _c.triggered;
		}
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		uint uniqueId = GES.GetUniqueId();
		triggerData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, triggerData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = true;
		eIC.isScaleable = false;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		ChipmunkC chipmunkC = Assemble(_eic);
		_eic.gameComponents.Add(chipmunkC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(chipmunkC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
	}

	public static void HandlePropertyChange(EventC _c)
	{
		BasicLevelData basicLevelData = EditorState.m_selection[0].data as BasicLevelData;
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

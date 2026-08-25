using System;
using System.Collections.Generic;
using UnityEngine;

public static class GEDraggableControllerA
{
	public static GETriggerC Assemble(EIC _eic, TriggerData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Controller",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, _data.position.ToVector3());
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, TriggerType.FingerController, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		_data.eventDispatchDelay = 500f;
		_eic.trigger = gETriggerC;
		TouchAreaC touchAreaC = null;
		ChipmunkC chipmunkC = null;
		if (_eic.container != null && _eic.container.identifier == "Block")
		{
			GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
			float num = 999999f;
			float num2 = -999999f;
			float num3 = 999999f;
			float num4 = -999999f;
			for (int i = 0; i < gEBlockC.modifiedShape.Contour.Length; i++)
			{
				VertexList vertexList = gEBlockC.modifiedShape.Contour[i];
				for (int j = 0; j < vertexList.NofVertices; j++)
				{
					num = Mathf.Min(vertexList.Vertex[j].x, num);
					num2 = Mathf.Max(vertexList.Vertex[j].x, num2);
					num3 = Mathf.Min(vertexList.Vertex[j].y, num3);
					num4 = Mathf.Max(vertexList.Vertex[j].y, num4);
				}
			}
			float width = num2 - num;
			float height = num4 - num3;
			if (!GEState.editorMode)
			{
				touchAreaC = TouchAreaS.AddComponent(transformC, "finger", width, height, true, _eic.camera, gETriggerC);
				touchAreaC.scaleByCameraDistance = true;
				TouchAreaS.AddTouchEventListener(touchAreaC, HandleTouches);
				gETriggerC.fingerTAC = touchAreaC;
				gETriggerC.fingerCMC = new List<ChipmunkC>();
				gETriggerC.fingerTouchIndices = new List<int>();
				gETriggerC.fingerBC = gEBlockC;
			}
			TransformS.ParentComponent(transformC, gEBlockC.CMC.TC, Vector3.zero);
			TransformS.SetRotation(transformC, Vector3.zero);
			Polygon polygon = GpcS.ClonePolygon(gEBlockC.modifiedShape);
			polygon = GpcS.ScalePolygon(polygon, -3f);
			PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.forward * -2f, polygon, 6f, DebugDraw.GetColor(255f, 255f, 255f, 255f), ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
		}
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 13u;
		triggerData.active = true;
		triggerData.toggle = false;
		triggerData.triggerOnlyOnce = false;
		triggerData.triggerUntilOutOfEnergy = false;
		triggerData.triggerOnlyOnFullEnergy = false;
		triggerData.autoTrigger = true;
		triggerData.energy = 1f;
		triggerData.energyClips = -1;
		triggerData.energyGain = 0f;
		triggerData.energyConsume = 0f;
		triggerData.gainInterval = 0f;
		triggerData.consumeInterval = 0f;
		triggerData.cooldown = 0f;
		triggerData.eventDispatchDelay = 500f;
		uint uniqueId = GES.GetUniqueId();
		triggerData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, triggerData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = false;
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
		TriggerData data = _eic.data as TriggerData;
		GETriggerC item = Assemble(_eic, data);
		_eic.gameComponents.Add(item);
	}

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		GETriggerC gETriggerC = _c.customComponent as GETriggerC;
		Vector2 screenPos = _c.touchPos[_i];
		Vector2 vector = TouchAreaS.GetTouchWorldPos(_c.camera, screenPos);
		if (_c.touchEvent[_i] == TouchEvent.RollIn)
		{
			return;
		}
		if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			gETriggerC.fingerPrevWorldPos = vector;
			TransformC transformComponent = EntityManager.AddEntityWithTC();
			gETriggerC.fingerCMC.Add(ChipmunkS.AddInactiveComponent(transformComponent, false, ColliderType.Any));
			gETriggerC.fingerTouchIndices.Add(InputManager.m_touches[_c.touchIndex[_i]].fingerId);
			ChipmunkC chipmunkC = gETriggerC.fingerCMC[gETriggerC.fingerCMC.Count - 1];
			ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(false, false, vector, chipmunkC.index, ColliderType.Any));
			IntPtr constraint = ChipmunkWrapper.AddPivotJoint(gETriggerC.fingerBC.CMC.cpBodyPtr, chipmunkC.cpBodyPtr, vector);
			ChipmunkWrapper.SetConstraintProperties(constraint, 0.05f, float.PositiveInfinity, 250000f);
			ChipmunkWrapper.SetVelocity(gETriggerC.fingerBC.CMC.cpBodyPtr, Vector2.zero);
			ChipmunkWrapper.SetAngularVelocity(gETriggerC.fingerBC.CMC.cpBodyPtr, 0f);
			ChipmunkWrapper.ResetForces(gETriggerC.fingerBC.CMC.cpBodyPtr);
			ChipmunkWrapper.SetCustomBodyProperties(gETriggerC.fingerBC.CMC.cpBodyPtr, gETriggerC.fingerBC.linearDamp * 0.5f, gETriggerC.fingerBC.angularDamp * 0.5f, Vector2.zero);
		}
		else if ((_c.touchEvent[_i] == TouchEvent.Drag || _c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.RollOut) && _c.touchStartedInside[_i])
		{
			int num = -1;
			for (int i = 0; i < gETriggerC.fingerTouchIndices.Count; i++)
			{
				if (InputManager.m_touches[_c.touchIndex[_i]].fingerId == gETriggerC.fingerTouchIndices[i])
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				ChipmunkC chipmunkC2 = gETriggerC.fingerCMC[num];
				ChipmunkWrapper.ActivateBody(chipmunkC2.cpBodyPtr);
				Vector2 p = chipmunkC2.ucpBodyStruct.p;
				float a = chipmunkC2.ucpBodyStruct.a;
				Vector2 vector2 = vector - p;
				Vector2 vector3 = p + vector2 * 0.1f;
				Vector2 vector4 = vector3 - p;
				ChipmunkWrapper.SetVelocity(chipmunkC2.cpBodyPtr, vector4 * ChipmunkS.m_chipmunkSlewDelta);
			}
		}
		else
		{
			if ((_c.touchEvent[_i] != TouchEvent.Release && _c.touchEvent[_i] != TouchEvent.ReleaseOutside) || !_c.touchStartedInside[_i])
			{
				return;
			}
			int num2 = -1;
			for (int j = 0; j < gETriggerC.fingerTouchIndices.Count; j++)
			{
				if (InputManager.m_touches[_c.touchIndex[_i]].fingerId == gETriggerC.fingerTouchIndices[j])
				{
					num2 = j;
					break;
				}
			}
			if (num2 != -1)
			{
				ChipmunkC chipmunkC3 = gETriggerC.fingerCMC[num2];
				if (chipmunkC3 != null && chipmunkC3.entityIndex > -1)
				{
					ChipmunkWrapper.SetCustomBodyProperties(gETriggerC.fingerBC.CMC.cpBodyPtr, gETriggerC.fingerBC.linearDamp, gETriggerC.fingerBC.angularDamp, gETriggerC.fingerBC.gravity);
					EntityManager.RemoveEntity(chipmunkC3.entityIndex, true);
				}
				gETriggerC.fingerCMC.RemoveAt(num2);
				gETriggerC.fingerTouchIndices.RemoveAt(num2);
			}
		}
	}

	private static Vector2 RePositionBody(ChipmunkC _sensor, Vector2 _desiredPos, Vector2 _oldPos, Vector2 _prevDesiredPos)
	{
		Vector2 vector = _desiredPos;
		ChipmunkQueryInfo[] array = new ChipmunkQueryInfo[50];
		int num = ChipmunkWrapper.BodyQuery(_sensor.cpBodyPtr, array);
		if (num > 0)
		{
			Vector2 zero = Vector2.zero;
			int num2 = 0;
			while (num > 0 && num2 < 100)
			{
				zero = Vector2.zero;
				for (int i = 0; i < num; i++)
				{
					zero += array[i].pos;
				}
				zero /= (float)num;
				Vector2 vector2 = zero - _oldPos;
				vector -= vector2 * 0.01f;
				ChipmunkWrapper.SetPosition(_sensor.cpBodyPtr, vector);
				ChipmunkWrapper.UpdateBodyPosition(_sensor.cpBodyPtr);
				num = ChipmunkWrapper.BodyQuery(_sensor.cpBodyPtr, array);
				num2++;
			}
			Vector2 vector3 = vector - _oldPos;
		}
		return vector;
	}
}

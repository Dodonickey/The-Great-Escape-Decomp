using UnityEngine;

public static class GEAreaTriggerA
{
	public static GETriggerC Assemble(EIC _eic, TriggerData _data, float _radius)
	{
		_data.shapeType = 0;
		_data.shape = null;
		return Assemble(_eic, _data);
	}

	public static GETriggerC Assemble(EIC _eic, TriggerData _data, float _width, float _height)
	{
		Vector2[] rect = DebugDraw.GetRect(_width, _height, Vector2.zero, false);
		_data.shapeType = 1;
		_data.shape = DebugDraw.Vector2ArrayToPolygon(rect);
		return Assemble(_eic, _data);
	}

	private static GETriggerC Assemble(EIC _eic, TriggerData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		if (GEState.editorMode)
		{
			if (_data.shapeType == 0 && _data.scale.x == _data.scale.y)
			{
				Vector2[] circle = DebugDraw.GetCircle(50f, 36, Vector2.zero);
				DebugDraw.ScaleVectorArray(circle, _data.scale.ToVector2());
				PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.zero, DebugDraw.Vector2ArrayToPolygon(circle), 6f, Color.red, ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
			}
			else
			{
				PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.zero, _data.shape, 6f, Color.red, ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
			}
		}
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, true, (ColliderType)11, 0u, _data.colliderType, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, _data.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		if (_data.shapeType == 0)
		{
			ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 1f, 50f * _data.scale.x, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true);
		}
		else
		{
			ChipmunkWrapper.AddPolyShape(chipmunkC.cpBodyPtr, Vector2.zero, 1f, _data.shape.Contour[0].NofVertices, _data.shape.Contour[0].Vertex, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true);
		}
		GETriggerC gETriggerC = (GETriggerC)(chipmunkC.customComponent = GES.AddTriggerComponent(_eic.camera, _data, chipmunkC));
		gETriggerC.collisionHandler = HandleSensor;
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[4];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		gETriggerC.modifierSlots[3] = new ConnectionSlot(ConnectionSlotType.ColliderType, 3);
		_eic.trigger = gETriggerC;
		if (_eic.container != null && _eic.container.identifier == "Block")
		{
			GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
			if (!gEBlockC.CMC.isStatic && !GEState.editorMode)
			{
				TransformS.ParentComponent(gETriggerC.TC, gEBlockC.CMC.TC);
				gETriggerC.connectedCMC = gEBlockC.CMC;
			}
		}
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 22u;
		triggerData.colliderType = 17895697u;
		triggerData.active = true;
		triggerData.toggle = false;
		triggerData.triggerOnlyOnce = false;
		triggerData.triggerUntilOutOfEnergy = false;
		triggerData.triggerOnlyOnFullEnergy = false;
		triggerData.autoTrigger = false;
		triggerData.energy = 1f;
		triggerData.energyClips = -1;
		triggerData.energyGain = 0f;
		triggerData.energyConsume = 0f;
		triggerData.gainInterval = 0f;
		triggerData.consumeInterval = 0f;
		triggerData.cooldown = 0f;
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
		GETriggerC gETriggerC = Assemble(_eic, data);
		_eic.gameComponents.Add(gETriggerC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gETriggerC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void HandleSensor(GETriggerC trigger, ChipmunkC _collidingCMC, ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		if (!trigger.active || _collidingCMC.isStatic || (trigger.triggerOnlyOnce && trigger.beganTime != 0f))
		{
			return;
		}
		switch (_collisionList)
		{
		case ChipmunkCollisionList.BEGIN:
			trigger.collidingCount++;
			GETriggerLogic.HandleBeginTriggerEvent(trigger);
			break;
		case ChipmunkCollisionList.SEPARATE:
			trigger.collidingCount--;
			if (!trigger.toggle)
			{
				GETriggerLogic.HandleEndTriggerEvent(trigger);
			}
			break;
		}
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		GESensorControllerA.PopulatePropertyBar(_eiC, _propertyBar);
	}
}

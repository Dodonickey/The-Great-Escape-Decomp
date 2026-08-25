using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class FSlingA
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024mapD;

	public static FSlingC Assemble(EIC _eic)
	{
		float maxRange = 40f;
		string[] tags = new string[3]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name,
			_eic.identifier
		};
		Frame frame = new Frame(280f, 512f, 13f, 186f, false, false);
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformC transformC2 = TransformS.AddComponent(entity);
		TransformC transformC3 = TransformS.AddComponent(entity);
		transformC.transform.position = _eic.data.position.ToVector3();
		transformC2.transform.position = _eic.data.position.ToVector3();
		transformC3.transform.position = _eic.data.position.ToVector3();
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC3, true, (ColliderType)20, 0u, 0u, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBoxBody(chipmunkC.isStatic, chipmunkC.isRogue, _eic.data.position.ToVector2(), chipmunkC.index, Vector2.zero, 5f, 10f, 40f, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true, chipmunkC.colliderType));
		GESpritePrefabC gESpritePrefabC = SpritePrefabA.Assemble(transformC.entityIndex, _eic.data.position.ToVector3(), "Milestone", tags, ColliderType.Any, 0u, 0u, 0f - _eic.data.position.z);
		TransformS.ParentComponent(gESpritePrefabC.rootNode.TC, transformC3);
		if (_eic.identifier != "Goal")
		{
			SpritePrefabNode spritePrefabNode = gESpritePrefabC.nodeTable["Goal"] as SpritePrefabNode;
			SpriteS.SetVisibility(spritePrefabNode.SC, false);
		}
		SpritePrefabNode spritePrefabNode2 = gESpritePrefabC.nodeTable["On"] as SpritePrefabNode;
		SpriteS.SetVisibility(spritePrefabNode2.SC, false);
		ChipmunkC chipmunkC2 = ChipmunkS.AddInactiveComponent(transformC, false, (ColliderType)20, 0u, 17895697u, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC2, ChipmunkWrapper.AddBoxBody(chipmunkC2.isStatic, chipmunkC2.isRogue, _eic.data.position.ToVector2(), chipmunkC2.index, Vector2.zero, 20f, 10f, 40f, 0f, 0f, chipmunkC2.colliderGroup, chipmunkC2.colliderLayer, true, chipmunkC2.colliderType));
		ChipmunkC chipmunkC3 = null;
		if (_eic.container != null && _eic.container.identifier == "Block")
		{
			GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
			chipmunkC3 = gEBlockC.CMC;
			TransformS.ParentComponent(transformC3, chipmunkC3.TC, transformC3.transform.position - chipmunkC3.TC.transform.position);
		}
		ChipmunkWrapper.AddPivotJoint2(chipmunkC.cpBodyPtr, chipmunkC2.cpBodyPtr, Vector2.zero, Vector2.zero);
		FSlingC fSlingC = FarmS.AddSlingComponent(entity, chipmunkC2, chipmunkC, _eic.data.position.ToVector2(), maxRange);
		ChipmunkS.SetCustomComponent(chipmunkC2, fSlingC);
		if (_eic.identifier == "Goal")
		{
			fSlingC.isGoal = true;
		}
		if (!GEState.editorMode)
		{
			TouchAreaC touchAreaC = TouchAreaS.AddComponent(transformC, "sling", 100f, 100f, true, Main.camera, fSlingC);
			TouchAreaS.AddTouchEventListener(touchAreaC, HandleSling);
			touchAreaC.scaleByCameraDistance = true;
		}
		fSlingC.connectedCMC = chipmunkC3;
		fSlingC.slingTC = transformC2;
		if (_eic.data.dataType == 8)
		{
			GETriggerC gETriggerC = (fSlingC.triggerC = GES.AddTriggerComponent(_eic.camera, _eic.data as TriggerData, TriggerType.Checkpoint, transformC));
			gETriggerC.inputSlots = new ConnectionSlot[0];
			gETriggerC.outputSlots = new ConnectionSlot[2];
			gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Enter, 0);
			gETriggerC.outputSlots[1] = new ConnectionSlot(ConnectionSlotType.Exit, 1);
			gETriggerC.modifierSlots = new ConnectionSlot[3];
			gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
			gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
			gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
			_eic.trigger = gETriggerC;
		}
		return fSlingC;
	}

	private static void HandleSling(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed)
		{
			return;
		}
		FSlingC fSlingC = _c.customComponent as FSlingC;
		Vector2 screenPos = _c.touchPos[_i];
		Vector3 touchWorldPos = TouchAreaS.GetTouchWorldPos(_c.camera, screenPos);
		if (fSlingC.vehicle == null || !fSlingC.ready || fSlingC.isGoal)
		{
			return;
		}
		bool flag = false;
		if (screenPos.x < 0f || screenPos.x > (float)Screen.width || screenPos.y < 0f || screenPos.y > (float)Screen.height)
		{
			flag = true;
		}
		if (((_c.touchEvent[_i] == TouchEvent.Release || _c.touchEvent[_i] == TouchEvent.ReleaseOutside) && _c.touchStartedInside[_i]) || flag)
		{
			Vector2 vector = fSlingC.vehicle.rootNode.CMC.ucpBodyStruct.p - (Vector2)fSlingC.restPos;
			for (int i = 0; i < fSlingC.vehicle.characters.Count; i++)
			{
				GECharacterC gECharacterC = fSlingC.vehicle.characters[i];
				if ((vector.x < 0f && gECharacterC.SPC.flipX == -1) || (vector.x > 0f && gECharacterC.SPC.flipX == 1))
				{
					GESpritePrefabS.FlipX(gECharacterC.SPC);
					if (gECharacterC.hatSPC != null)
					{
						GESpritePrefabS.FlipX(gECharacterC.hatSPC);
						SpritePrefabNode rootNode = gECharacterC.hatSPC.rootNode;
						Vector3 localPosition = rootNode.TC.transform.localPosition;
						localPosition.x *= -1f;
						rootNode.TC.transform.localPosition = localPosition;
					}
				}
			}
			if (fSlingC.touchCMC != null)
			{
				fSlingC.launchPos = fSlingC.touchCMC.TC.transform.position - fSlingC.rootCMC.TC.transform.position;
				EntityManager.RemoveEntity(fSlingC.touchCMC.entityIndex);
				ChipmunkWrapper.SetCustomBodyProperties(fSlingC.CMC.cpBodyPtr, Vector2.one, 1f, Vector2.zero);
				fSlingC.touchCMC = null;
				fSlingC.launched = true;
				fSlingC.lastLaunch = Main.m_gameTime;
				fSlingC.armed = false;
			}
		}
		else if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			ChipmunkQueryInfo[] array = new ChipmunkQueryInfo[50];
			int num = ChipmunkWrapper.BBQuery(Vector2.one * 50f, fSlingC.restPos, 0u, 17895697u, array);
			List<GECharacterC> list = new List<GECharacterC>();
			for (int j = 0; j < num; j++)
			{
				ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[array[j].componentIndex];
				if (chipmunkC.customComponent != null && chipmunkC.customComponent.componentType == (ComponentType)102)
				{
					GECharacterC gECharacterC2 = chipmunkC.customComponent as GECharacterC;
					if (!list.Contains(gECharacterC2) && gECharacterC2.movementState != MovementState.Stunned && gECharacterC2.movementState != MovementState.Dead)
					{
						list.Add(gECharacterC2);
					}
				}
			}
			if (list.Count > 0 && fSlingC.touchCMC == null)
			{
				string[] tags = new string[2] { "FingerController", "GameEntity" };
				TransformC transformComponent = EntityManager.AddEntityWithTC(tags);
				ChipmunkC chipmunkC2 = ChipmunkS.AddInactiveComponent(transformComponent, true, ColliderType.Any, 0u, 0u, false, true);
				ChipmunkS.ActivateChipmunkComponent(chipmunkC2, ChipmunkWrapper.AddBody(chipmunkC2.isStatic, chipmunkC2.isRogue, touchWorldPos, chipmunkC2.index, chipmunkC2.colliderType));
				fSlingC.touchCMC = chipmunkC2;
				ChipmunkWrapper.AddDampedSpring(fSlingC.touchCMC.cpBodyPtr, fSlingC.CMC.cpBodyPtr, Vector2.zero, Vector2.zero, 0f, 50000f, 500f);
				ChipmunkWrapper.SetCustomBodyProperties(fSlingC.CMC.cpBodyPtr, Vector2.one * 0.5f, 0.75f, Vector2.zero);
				fSlingC.launched = false;
				fSlingC.armed = true;
				Frame frame = new Frame(374f, 583f, 26f, 30f);
				fSlingC.knotSC = SpriteS.AddComponent(fSlingC.rootCMC.TC, frame, FarmState.propBackgroundSheet);
				SpriteS.SetDimensionScale(fSlingC.knotSC, 0.25f);
				SpriteS.SetSortValue(fSlingC.knotSC, 2f);
				SpriteS.SetOffset(fSlingC.knotSC, Vector3.up * -1f + Vector3.forward * -2.5f, 0f);
				Frame frame2 = new Frame(310f, 667f, 201f, 31f);
				fSlingC.slingSC = SpriteS.AddComponent(fSlingC.slingTC, frame2, FarmState.propBackgroundSheet);
				SpriteS.SetDimensions(fSlingC.slingSC, 5f, 5f);
				SpriteS.SetSortValue(fSlingC.knotSC, 1f);
			}
			for (int k = 0; k < list.Count; k++)
			{
				GECharacterLogic.JumpToCart(list[k], fSlingC.vehicle, Vector3.zero, 0f);
			}
		}
		else
		{
			if ((_c.touchEvent[_i] != TouchEvent.Drag && _c.touchEvent[_i] != TouchEvent.Down && _c.touchEvent[_i] != TouchEvent.RollOut) || !_c.touchStartedInside[_i])
			{
				return;
			}
			if (fSlingC.touchCMC != null)
			{
				Vector2 vector2 = touchWorldPos;
				Vector2 vector3 = fSlingC.restPos;
				if ((vector2 - vector3).sqrMagnitude > fSlingC.maxRange * fSlingC.maxRange)
				{
					vector2 = (vector2 - vector3).normalized * fSlingC.maxRange + vector3;
				}
				TransformS.SetPosition(fSlingC.touchCMC.TC, vector2);
			}
			Vector2 vector4 = fSlingC.vehicle.rootNode.CMC.ucpBodyStruct.p - (Vector2)fSlingC.restPos;
			for (int l = 0; l < fSlingC.vehicle.characters.Count; l++)
			{
				GECharacterC gECharacterC3 = fSlingC.vehicle.characters[l];
				if ((vector4.x < 0f && gECharacterC3.SPC.flipX == -1) || (vector4.x > 0f && gECharacterC3.SPC.flipX == 1))
				{
					GESpritePrefabS.FlipX(gECharacterC3.SPC);
					if (gECharacterC3.hatSPC != null)
					{
						GESpritePrefabS.FlipX(gECharacterC3.hatSPC);
						SpritePrefabNode rootNode2 = gECharacterC3.hatSPC.rootNode;
						Vector3 localPosition2 = rootNode2.TC.transform.localPosition;
						localPosition2.x *= -1f;
						rootNode2.TC.transform.localPosition = localPosition2;
					}
				}
			}
		}
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.position.z = 50f;
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 9u;
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
		FSlingC fSlingC = Assemble(_eic);
		_eic.gameComponents.Add(fSlingC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(fSlingC.rootCMC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		BasicLevelData basicLevelData = _eic.data as BasicLevelData;
	}

	public static void HandlePropertyChange(EventC _c)
	{
		BasicLevelData basicLevelData = EditorState.m_selection[0].data as BasicLevelData;
		string identifier = _c.identifier;
		if (identifier != null)
		{
			if (_003C_003Ef__switch_0024mapD == null)
			{
				_003C_003Ef__switch_0024mapD = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024mapD.TryGetValue(identifier, out value))
			{
			}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

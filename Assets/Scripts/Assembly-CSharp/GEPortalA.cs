using System.Collections.Generic;
using UnityEngine;

public static class GEPortalA
{
	private static GEPortalC Assemble(EIC _eic)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		BasicLevelData basicLevelData = _eic.data as BasicLevelData;
		Vector2[] circle = DebugDraw.GetCircle(50f, 36, Vector2.zero);
		DebugDraw.ScaleVectorArray(circle, basicLevelData.scale.ToVector2());
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.zero, DebugDraw.Vector2ArrayToPolygon(circle), 6f, Color.green, ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, true, (ColliderType)6, 0u, 17895697u, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, basicLevelData.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 1f, 50f * basicLevelData.scale.x, 0f, 0f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, true);
		if (_eic.container != null && _eic.container.identifier == "Block")
		{
			GEBlockC gEBlockC = _eic.container.gameComponents[0] as GEBlockC;
			if (!gEBlockC.CMC.isStatic && !GEState.editorMode)
			{
				TransformS.ParentComponent(chipmunkC.TC, gEBlockC.CMC.TC);
			}
		}
		GEPortalC gEPortalC = (GEPortalC)(chipmunkC.customComponent = GES.AddPortalComponent(chipmunkC, true));
		List<EIC> editorItemsWithUniqueId = GES.GetEditorItemsWithUniqueId(basicLevelData.id);
		if (editorItemsWithUniqueId.Count == 2)
		{
			for (int i = 0; i < editorItemsWithUniqueId.Count; i++)
			{
				if (editorItemsWithUniqueId[i] != _eic && editorItemsWithUniqueId[i].gameComponents.Count > 0)
				{
					(gEPortalC.pair = editorItemsWithUniqueId[i].gameComponents[0] as GEPortalC).pair = gEPortalC;
					break;
				}
			}
		}
		return gEPortalC;
	}

	public static List<EIC> CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		List<EIC> list = new List<EIC>();
		uint uniqueId = GES.GetUniqueId();
		BasicLevelData basicLevelData = new BasicLevelData();
		basicLevelData.position = new Vertex3(_pos + Vector3.right * -50f);
		basicLevelData.rotation = new Vertex3(_rot);
		basicLevelData.scale = new Vertex3(_sca);
		basicLevelData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, basicLevelData, Main.camera);
		eIC.isRealtimeMovable = true;
		list.Add(eIC);
		BasicLevelData basicLevelData2 = new BasicLevelData();
		basicLevelData2.position = new Vertex3(_pos + Vector3.right * 50f);
		basicLevelData2.rotation = new Vertex3(_rot);
		basicLevelData2.scale = new Vertex3(_sca);
		basicLevelData2.Init(uniqueId, _identifier + uniqueId);
		eIC = GEItemA.Assemble(_container, _identifier, basicLevelData2, Main.camera);
		eIC.isRealtimeMovable = true;
		list.Add(eIC);
		return list;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		GEPortalC gEPortalC = Assemble(_eic);
		_eic.gameComponents.Add(gEPortalC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gEPortalC.CMC.TC, _eic.TC, Vector3.zero);
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

	public static void HandlePORTALCollisions(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC.colliderType != (ColliderType)6)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		GEPortalC gEPortalC = chipmunkC.customComponent as GEPortalC;
		switch (_collisionList)
		{
		case ChipmunkCollisionList.SEPARATE:
			if (gEPortalC.usingEntityIndex != chipmunkC2.entityIndex)
			{
				gEPortalC.usingEntityIndex = -1;
				gEPortalC.pair.usingEntityIndex = -1;
			}
			break;
		case ChipmunkCollisionList.BEGIN:
			if (chipmunkC2.customComponent == null)
			{
				break;
			}
			switch (chipmunkC2.customComponent.componentType)
			{
			case (ComponentType)100:
			{
				GEBlockC gEBlockC = chipmunkC2.customComponent as GEBlockC;
				if (gEPortalC.usingEntityIndex == -1 && gEPortalC.pair.usingEntityIndex == -1)
				{
					TransformS.SetGlobalPosition(chipmunkC2.TC, gEPortalC.pair.CMC.TC.transform.position, chipmunkC2.cpBodyPtr);
					ChipmunkWrapper.SetVelocity(chipmunkC2.cpBodyPtr, Vector2.zero);
					ChipmunkWrapper.SetAngularVelocity(chipmunkC2.cpBodyPtr, 0f);
					gEPortalC.usingEntityIndex = chipmunkC2.entityIndex;
				}
				break;
			}
			case (ComponentType)113:
			{
				GEVehicleC gEVehicleC = chipmunkC2.customComponent as GEVehicleC;
				if (gEPortalC.usingEntityIndex != -1 || gEPortalC.pair.usingEntityIndex != -1)
				{
					break;
				}
				for (int l = 0; l < gEVehicleC.SPC.nodes.Length; l++)
				{
					SpritePrefabNode spritePrefabNode4 = gEVehicleC.SPC.nodes[l];
					if (spritePrefabNode4.hasPhysics == 1)
					{
						TransformS.SetGlobalPosition(spritePrefabNode4.CMC.TC, gEPortalC.pair.CMC.TC.transform.position, spritePrefabNode4.CMC.cpBodyPtr);
						ChipmunkWrapper.SetVelocity(spritePrefabNode4.CMC.cpBodyPtr, Vector2.zero);
						ChipmunkWrapper.SetAngularVelocity(spritePrefabNode4.CMC.cpBodyPtr, 0f);
					}
				}
				for (int m = 0; m < gEVehicleC.characters.Count; m++)
				{
					for (int n = 0; n < gEVehicleC.characters[m].SPC.nodes.Length; n++)
					{
						SpritePrefabNode spritePrefabNode5 = gEVehicleC.characters[m].SPC.nodes[n];
						if (spritePrefabNode5.hasPhysics == 1 && !spritePrefabNode5.CMC.transformComponentDictates)
						{
							TransformS.SetGlobalPosition(spritePrefabNode5.CMC.TC, gEPortalC.pair.CMC.TC.transform.position + spritePrefabNode5.globalPosition, spritePrefabNode5.CMC.cpBodyPtr);
							ChipmunkWrapper.SetVelocity(spritePrefabNode5.CMC.cpBodyPtr, Vector2.zero);
							ChipmunkWrapper.SetAngularVelocity(spritePrefabNode5.CMC.cpBodyPtr, 0f);
						}
					}
					if (gEVehicleC.characters[m].hatSPC == null)
					{
						continue;
					}
					for (int num = 0; num < gEVehicleC.characters[m].hatSPC.nodes.Length; num++)
					{
						SpritePrefabNode spritePrefabNode6 = gEVehicleC.characters[m].hatSPC.nodes[num];
						if (spritePrefabNode6.hasPhysics == 1 && !spritePrefabNode6.CMC.transformComponentDictates)
						{
							TransformS.SetGlobalPosition(spritePrefabNode6.CMC.TC, gEPortalC.pair.CMC.TC.transform.position, spritePrefabNode6.CMC.cpBodyPtr);
							ChipmunkWrapper.SetVelocity(spritePrefabNode6.CMC.cpBodyPtr, Vector2.zero);
							ChipmunkWrapper.SetAngularVelocity(spritePrefabNode6.CMC.cpBodyPtr, 0f);
						}
					}
				}
				gEPortalC.usingEntityIndex = chipmunkC2.entityIndex;
				break;
			}
			case (ComponentType)102:
			{
				GECharacterC gECharacterC = chipmunkC2.customComponent as GECharacterC;
				if (gEPortalC.usingEntityIndex != -1 || gEPortalC.pair.usingEntityIndex != -1)
				{
					break;
				}
				for (int i = 0; i < gECharacterC.SPC.nodes.Length; i++)
				{
					SpritePrefabNode spritePrefabNode = gECharacterC.SPC.nodes[i];
					if (spritePrefabNode.hasPhysics == 1 && !spritePrefabNode.CMC.transformComponentDictates)
					{
						TransformS.SetGlobalPosition(spritePrefabNode.CMC.TC, gEPortalC.pair.CMC.TC.transform.position, spritePrefabNode.CMC.cpBodyPtr);
						ChipmunkWrapper.SetVelocity(spritePrefabNode.CMC.cpBodyPtr, Vector2.zero);
						ChipmunkWrapper.SetAngularVelocity(spritePrefabNode.CMC.cpBodyPtr, 0f);
					}
				}
				if (gECharacterC.hatSPC != null)
				{
					for (int j = 0; j < gECharacterC.hatSPC.nodes.Length; j++)
					{
						SpritePrefabNode spritePrefabNode2 = gECharacterC.hatSPC.nodes[j];
						if (spritePrefabNode2.hasPhysics == 1 && !spritePrefabNode2.CMC.transformComponentDictates)
						{
							TransformS.SetGlobalPosition(spritePrefabNode2.CMC.TC, gEPortalC.pair.CMC.TC.transform.position, spritePrefabNode2.CMC.cpBodyPtr);
							ChipmunkWrapper.SetVelocity(spritePrefabNode2.CMC.cpBodyPtr, Vector2.zero);
							ChipmunkWrapper.SetAngularVelocity(spritePrefabNode2.CMC.cpBodyPtr, 0f);
						}
					}
				}
				if (gECharacterC.vehicle != null)
				{
					for (int k = 0; k < gECharacterC.vehicle.SPC.nodes.Length; k++)
					{
						SpritePrefabNode spritePrefabNode3 = gECharacterC.vehicle.SPC.nodes[k];
						if (spritePrefabNode3.hasPhysics == 1 && !spritePrefabNode3.CMC.transformComponentDictates)
						{
							TransformS.SetGlobalPosition(spritePrefabNode3.CMC.TC, gEPortalC.pair.CMC.TC.transform.position, spritePrefabNode3.CMC.cpBodyPtr);
							ChipmunkWrapper.SetVelocity(spritePrefabNode3.CMC.cpBodyPtr, Vector2.zero);
							ChipmunkWrapper.SetAngularVelocity(spritePrefabNode3.CMC.cpBodyPtr, 0f);
						}
					}
				}
				gEPortalC.usingEntityIndex = chipmunkC2.entityIndex;
				break;
			}
			}
			break;
		}
	}
}

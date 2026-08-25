using UnityEngine;

public static class GEPhysicsAffectorA
{
	public static GETriggerC Assemble(EIC _eic, PhysicsAffectorData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC tc = TransformS.AddComponent(entity);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _data, TriggerType.PhysicsAffector, tc);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Deactivate, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Destroy, 2);
		if (_eic.container != null)
		{
			Vector2 vector = _eic.data.position.ToVector2() - _eic.container.data.position.ToVector2();
			_data.direction = new Vertex3(vector.normalized);
			_data.amount = vector.magnitude;
		}
		GEPhysicsAffectorC gEPhysicsAffectorC = GES.AddPhysicsAffectorComponent(tc, _data);
		if (_eic.container != null)
		{
			for (int i = 0; i < _eic.container.gameComponents.Count; i++)
			{
				IComponent component = _eic.container.gameComponents[i];
				Entity entity2 = EntityManager.m_entities.m_array[component.entityIndex];
				for (int j = 0; j < entity2.components.Count; j++)
				{
					IComponent component2 = entity2.components[j];
					if (component2.componentType == ComponentType.Chipmunk && !gEPhysicsAffectorC.cmcs.Contains(component2 as ChipmunkC))
					{
						gEPhysicsAffectorC.cmcs.Add(component2 as ChipmunkC);
					}
				}
			}
		}
		_eic.trigger = gETriggerC;
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		return gETriggerC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		PhysicsAffectorData physicsAffectorData = new PhysicsAffectorData();
		physicsAffectorData.position = new Vertex3(_pos);
		physicsAffectorData.rotation = new Vertex3(_rot);
		physicsAffectorData.scale = new Vertex3(_sca);
		physicsAffectorData.active = true;
		physicsAffectorData.duration = 0f;
		physicsAffectorData.relative = true;
		switch (_identifier)
		{
		case "Apply Impulse":
			physicsAffectorData.isImpulse = true;
			break;
		case "Apply Velocity":
			physicsAffectorData.isVelocity = true;
			break;
		case "Apply Angular Velocity":
			physicsAffectorData.isAngularVelocity = true;
			break;
		case "Apply Force":
			physicsAffectorData.isForce = true;
			break;
		}
		uint uniqueId = GES.GetUniqueId();
		physicsAffectorData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, physicsAffectorData, Main.camera);
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
		PhysicsAffectorData data = _eic.data as PhysicsAffectorData;
		GETriggerC gETriggerC = Assemble(_eic, data);
		_eic.gameComponents.Add(gETriggerC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gETriggerC.TC, _eic.TC, Vector3.zero);
		}
	}
}

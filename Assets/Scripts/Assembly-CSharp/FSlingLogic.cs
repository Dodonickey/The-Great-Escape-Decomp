using UnityEngine;

public static class FSlingLogic
{
	private static float m_lastSlingStretchDif;

	private static bool m_slingSoundPlayed;

	public static void HandleVEHICLEtoSLINGCollisions(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		if (GEState.editorMode)
		{
			return;
		}
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC.colliderType != (ColliderType)12)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		FSlingC fSlingC = chipmunkC2.customComponent as FSlingC;
		GEVehicleC gEVehicleC = chipmunkC.customComponent as GEVehicleC;
		if (gEVehicleC.vehicleType == VehicleType.Runner || !(gEVehicleC.health > 0f) || fSlingC == null || fSlingC.vehicle != null || !(fSlingC.lastLaunch + 0.5f < Main.m_gameTime))
		{
			return;
		}
		fSlingC.vehicle = gEVehicleC;
		if (fSlingC.triggerC == null)
		{
			return;
		}
		fSlingC.triggerC.collidingCount++;
		fSlingC.triggerC.outputSlots[0].m_triggered = true;
		GETriggerLogic.HandleBeginTriggerEvent(fSlingC.triggerC);
		GETriggerLogic.HandleEndTriggerEvent(fSlingC.triggerC);
		fSlingC.triggerC.outputSlots[0].m_triggered = false;
		fSlingC.triggerC.collidingCount = 0;
		if (fSlingC.isGoal)
		{
			string[] keys = new string[1] { "GE_vehicle" };
			object[] values = new object[1] { fSlingC.vehicle };
			EventS.Dispatch("F_goal_enter", keys, values, false);
		}
		else
		{
			string[] keys2 = new string[1] { "GE_vehicle" };
			object[] values2 = new object[1] { fSlingC.vehicle };
			EventS.Dispatch("F_sling_enter", keys2, values2, false);
		}
		m_lastSlingStretchDif = 0f;
		m_slingSoundPlayed = false;
		SoundS.PlaySound("SoundCheckpoint", chipmunkC2.TC.transform.gameObject);
		Entity entity = EntityManager.m_entities.m_array[fSlingC.entityIndex];
		GESpritePrefabC gESpritePrefabC = null;
		for (int i = 0; i < entity.components.Count; i++)
		{
			if (entity.components[i].componentType == (ComponentType)116)
			{
				gESpritePrefabC = entity.components[i] as GESpritePrefabC;
				break;
			}
		}
		SpritePrefabNode spritePrefabNode = gESpritePrefabC.nodeTable["On"] as SpritePrefabNode;
		SpritePrefabNode spritePrefabNode2 = gESpritePrefabC.nodeTable["Off"] as SpritePrefabNode;
		SpriteS.SetVisibility(spritePrefabNode2.SC, false);
		SpriteS.SetVisibility(spritePrefabNode.SC, true);
	}

	public static void Update(FSlingC _c)
	{
		if (_c.vehicle == null)
		{
			return;
		}
		_c.restPos = _c.rootCMC.TC.transform.position;
		if (_c.slingSC != null)
		{
			Vector3 vector = _c.vehicle.rootNode.TC.transform.position - (_c.restPos - Vector3.up * 5f);
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			TransformS.SetPosition(_c.slingTC, _c.restPos + vector * 0.5f + Vector3.forward * -1f);
			TransformS.SetRotation(_c.slingTC, Vector3.forward * num);
			SpriteS.SetDimensions(_c.slingSC, vector.magnitude, 3f + (1f - vector.magnitude / _c.maxRange) * 2f);
			if (Mathf.Sign(m_lastSlingStretchDif) != Mathf.Sign(vector.x))
			{
				m_slingSoundPlayed = false;
			}
			if (Mathf.Abs(vector.x) > 10f && Mathf.Abs(m_lastSlingStretchDif - vector.x) > 0.5f && !m_slingSoundPlayed)
			{
				SoundS.PlaySound("SoundSlingStretch", _c.slingTC.transform.gameObject);
				m_slingSoundPlayed = true;
			}
			m_lastSlingStretchDif = vector.x;
		}
		if (_c.launched)
		{
			if (_c.ready)
			{
				float velX = (_c.rootCMC.ucpBodyStruct.p.x - _c.CMC.ucpBodyStruct.p.x) * 10f;
				ChipmunkWrapper.SetXVelocity(_c.vehicle.rootNode.CMC.cpBodyPtr, velX);
				ChipmunkWrapper.SetAngularVelocity(_c.vehicle.rootNode.CMC.cpBodyPtr, 0f);
				for (int i = 0; i < _c.vehicle.SPC.nodes.Length; i++)
				{
					SpritePrefabNode spritePrefabNode = _c.vehicle.SPC.nodes[i];
					if (spritePrefabNode.hasPhysics == 1)
					{
						ChipmunkWrapper.SetXVelocity(spritePrefabNode.CMC.cpBodyPtr, velX);
					}
				}
				for (int j = 0; j < _c.vehicle.characters.Count; j++)
				{
					GECharacterC gECharacterC = _c.vehicle.characters[j];
					for (int k = 0; k < gECharacterC.SPC.nodes.Length; k++)
					{
						SpritePrefabNode spritePrefabNode2 = gECharacterC.SPC.nodes[k];
						if (spritePrefabNode2.hasPhysics == 1)
						{
							ChipmunkWrapper.SetXVelocity(spritePrefabNode2.CMC.cpBodyPtr, velX);
						}
					}
				}
				GEVehicleLogic.SetTireBrakes(_c.vehicle, false);
				_c.armed = false;
				_c.ready = false;
			}
			if ((_c.launchPos.x < -10f && _c.vehicle.rootNode.TC.transform.position.x > _c.restPos.x) || (_c.launchPos.x > 10f && _c.vehicle.rootNode.TC.transform.position.x < _c.restPos.x))
			{
				_c.vehicle = null;
				_c.launched = false;
				SpriteS.RemoveComponent(_c.slingSC);
				_c.slingSC = null;
				SpriteS.RemoveComponent(_c.knotSC);
				_c.knotSC = null;
				if (_c.triggerC != null)
				{
					_c.triggerC.collidingCount++;
					_c.triggerC.outputSlots[1].m_triggered = true;
					GETriggerLogic.HandleBeginTriggerEvent(_c.triggerC);
					GETriggerLogic.HandleEndTriggerEvent(_c.triggerC);
					_c.triggerC.outputSlots[1].m_triggered = false;
					_c.triggerC.collidingCount = 0;
					if (_c.isGoal)
					{
						string[] keys = new string[1] { "GE_vehicle" };
						object[] values = new object[1] { _c.vehicle };
						EventS.Dispatch("F_goal_exit", keys, values, false);
					}
					else
					{
						string[] keys2 = new string[1] { "GE_vehicle" };
						object[] values2 = new object[1] { _c.vehicle };
						EventS.Dispatch("F_sling_exit", keys2, values2, false);
					}
					SoundS.PlaySound("SoundSlingRelease", _c.slingTC.transform.gameObject);
				}
			}
			else if (_c.launchPos.x <= 10f && _c.launchPos.x >= -10f)
			{
				_c.launched = false;
				SpriteS.RemoveComponent(_c.slingSC);
				_c.slingSC = null;
				SpriteS.RemoveComponent(_c.knotSC);
				_c.knotSC = null;
			}
		}
		else if (_c.armed && !_c.launched)
		{
			float num2 = _c.CMC.ucpBodyStruct.p.x - _c.vehicle.rootNode.CMC.ucpBodyStruct.p.x;
			ChipmunkWrapper.SetXVelocity(_c.vehicle.rootNode.CMC.cpBodyPtr, _c.CMC.ucpBodyStruct.v.x + num2);
			ChipmunkWrapper.SetAngularVelocity(_c.vehicle.rootNode.CMC.cpBodyPtr, 0f);
			for (int l = 0; l < _c.vehicle.tires.Length; l++)
			{
				ChipmunkWrapper.SetXVelocity(_c.vehicle.tires[l].CMC.cpBodyPtr, _c.CMC.ucpBodyStruct.v.x + num2);
			}
			for (int m = 0; m < _c.vehicle.characters.Count; m++)
			{
				GECharacterC gECharacterC2 = _c.vehicle.characters[m];
				for (int n = 0; n < gECharacterC2.SPC.nodes.Length; n++)
				{
					SpritePrefabNode spritePrefabNode3 = gECharacterC2.SPC.nodes[n];
					if (spritePrefabNode3.hasPhysics == 1)
					{
						ChipmunkWrapper.SetXVelocity(spritePrefabNode3.CMC.cpBodyPtr, _c.CMC.ucpBodyStruct.v.x + num2);
					}
				}
			}
			GEVehicleLogic.SetTireBrakes(_c.vehicle, false);
		}
		else
		{
			if (_c.armed || _c.launched)
			{
				return;
			}
			if ((_c.vehicle.rootNode.TC.transform.position - _c.restPos).sqrMagnitude > 5f)
			{
				Vector2 vector2 = ((Vector2)_c.restPos - _c.vehicle.rootNode.CMC.ucpBodyStruct.p) * 10f;
				ChipmunkWrapper.SetXVelocity(_c.vehicle.rootNode.CMC.cpBodyPtr, vector2.x);
			}
			if (!_c.ready && _c.isGoal)
			{
				if (Mathf.Abs(_c.vehicle.rootNode.CMC.ucpBodyStruct.v.x - _c.CMC.ucpBodyStruct.v.x) < 2f)
				{
					while (_c.vehicle.characters.Count > 0)
					{
						GECharacterC c = _c.vehicle.characters[0];
						GECharacterLogic.JumpFromCart(c, false, 10f);
						GEVehicleC gEVehicleC = GECharacterLogic.CreateRunner(c, Vector2.up * 2.5f, 10f);
					}
					_c.ready = true;
				}
			}
			else if (!_c.ready)
			{
				_c.ready = true;
			}
		}
	}
}

using System;
using UnityEngine;

public static class GECharacterLogic
{
	private static ChipmunkSegmentQueryInfo lookQueryResult = default(ChipmunkSegmentQueryInfo);

	private static ChipmunkSegmentQueryInfo levelQueryResult1 = default(ChipmunkSegmentQueryInfo);

	private static ChipmunkSegmentQueryInfo levelQueryResult2 = default(ChipmunkSegmentQueryInfo);

	private static ChipmunkPivotJointStruct s1 = default(ChipmunkPivotJointStruct);

	private static ChipmunkPivotJointStruct s2 = default(ChipmunkPivotJointStruct);

	private static ChipmunkQueryInfo[] queryInfo = new ChipmunkQueryInfo[200];

	public static void Update(GECharacterC _c)
	{
		if (_c.vehicle == null)
		{
			return;
		}
		Vector2 currentLookNormal = _c.vehicle.currentLookNormal;
		Vector2 currentLookDir = _c.vehicle.currentLookDir;
		float num = Mathf.Abs(_c.vehicle.currentBalanceDif) * 57.29578f;
		if (!(_c.emotionStateChanged + 0.5f < Main.m_gameTime))
		{
			return;
		}
		if ((_c.vehicle.contactState == ContactState.OnSolid && num > 20f) || (_c.vehicle.contactState == ContactState.OnAir && num > 10f))
		{
			if (_c.emotionState != EmotionState.Terrified)
			{
				ChangeEmotionState(_c, EmotionState.Terrified);
			}
		}
		else if (Mathf.Abs(_c.vehicle.currentBalanceDif) * 57.29578f > 10f)
		{
			if (_c.emotionState != EmotionState.Excited)
			{
				if (_c.emotionState == EmotionState.Terrified)
				{
					ChangeEmotionState(_c, EmotionState.Nausea);
				}
				else
				{
					ChangeEmotionState(_c, EmotionState.Excited);
				}
			}
		}
		else if (_c.health > 0f && _c.emotionState != EmotionState.Neutral)
		{
			if (_c.emotionState == EmotionState.Terrified)
			{
				ChangeEmotionState(_c, EmotionState.Nausea);
			}
			else
			{
				ChangeEmotionState(_c, EmotionState.Neutral);
			}
		}
	}

	public static void BeginCarry(GECharacterC _c, ChipmunkC _component, Vector2 _pos, bool _drag)
	{
	}

	public static void EndCarry(GECharacterC _c, ChipmunkC _component)
	{
	}

	public static void SetCharacterBrakes(GECharacterC _c, bool _brake)
	{
	}

	public static void JumpToCart(GECharacterC _c, GEVehicleC _v, Vector3 _offset, float _sortOffset)
	{
		if (_c.vehicle != null)
		{
			if (_c.vehicle.vehicleType == VehicleType.Runner)
			{
				JumpFromCart(_c, true, 0f);
			}
			else
			{
				JumpFromCart(_c, false, 0f);
			}
		}
		_v.updateCharacterDepth = true;
		for (int i = 0; i < _c.SPC.nodes.Length; i++)
		{
			SpritePrefabNode spritePrefabNode = _c.SPC.nodes[i];
			if (spritePrefabNode.hasPhysics == 1)
			{
				ChipmunkWrapper.SetBodyGroup(spritePrefabNode.CMC.cpBodyPtr, _v.rootNode.CMC.colliderGroup);
				ChipmunkWrapper.SetCustomBodyAngularDamp(spritePrefabNode.CMC.cpBodyPtr, 0.75f);
				ChipmunkWrapper.SetVelocity(spritePrefabNode.CMC.cpBodyPtr, _v.rootNode.CMC.ucpBodyStruct.v);
			}
			if (spritePrefabNode.isArm == 1 || spritePrefabNode.isLeg == 1)
			{
				ChipmunkWrapper.SetBodyLayers(spritePrefabNode.CMC.cpBodyPtr, 0u);
			}
			if (spritePrefabNode.SC != null)
			{
				SpriteS.SetSortValue(spritePrefabNode.SC, 0f - _v.rootNode.CMC.TC.transform.position.z + (0f - spritePrefabNode.globalPosition.z) + _sortOffset);
			}
		}
		if (_c.hatSPC != null)
		{
			SpritePrefabNode spritePrefabNode2 = _c.SPC.nodeTable["LocatorHats"] as SpritePrefabNode;
			for (int j = 0; j < _c.hatSPC.nodes.Length; j++)
			{
				SpritePrefabNode spritePrefabNode3 = _c.hatSPC.nodes[j];
				if (spritePrefabNode3.hasPhysics == 1 && spritePrefabNode3.isSensor == 0)
				{
					ChipmunkWrapper.SetBodyGroup(spritePrefabNode3.CMC.cpBodyPtr, _v.rootNode.CMC.colliderGroup);
					ChipmunkWrapper.SetVelocity(spritePrefabNode3.CMC.cpBodyPtr, _v.rootNode.CMC.ucpBodyStruct.v);
				}
				if ((spritePrefabNode3.isArm == 1 || spritePrefabNode3.isLeg == 1) && spritePrefabNode3.isSensor == 0)
				{
					ChipmunkWrapper.SetBodyLayers(spritePrefabNode3.CMC.cpBodyPtr, 0u);
				}
				if (spritePrefabNode3.SC != null)
				{
					SpriteS.SetSortValue(spritePrefabNode3.SC, 0f - _v.rootNode.CMC.TC.transform.position.z + (0f - spritePrefabNode3.globalPosition.z) + _sortOffset - spritePrefabNode2.globalPosition.z);
				}
			}
		}
		for (int k = 0; k < _v.seats.Count; k++)
		{
			if (_v.seatsTaken[k] == null)
			{
				_v.seatsTaken[k] = _c;
				_offset = _v.seats[k].localPosition;
			}
		}
		_c.vehicle = _v;
		_v.characters.Add(_c);
		if (_v.rootNode.hasSuspension == 1)
		{
			_c.vehicleSpringPtr = ChipmunkWrapper.AddDampedSpring(_c.rootNode.CMC.cpBodyPtr, _v.rootNode.CMC.cpBodyPtr, Vector2.zero, _offset, 0f, _v.rootNode.suspensionStrength, _v.rootNode.suspensionDamp);
			_c.vehicleConnectionPtr = ChipmunkWrapper.AddGrooveJoint(_c.rootNode.CMC.cpBodyPtr, _v.rootNode.CMC.cpBodyPtr, Vector2.up * _v.rootNode.suspensionDepth * 0.5f, Vector2.up * (0f - _v.rootNode.suspensionDepth) * 0.5f, Vector2.zero);
		}
		else
		{
			_c.vehicleConnectionPtr = ChipmunkWrapper.AddPivotJoint2(_c.rootNode.CMC.cpBodyPtr, _v.rootNode.CMC.cpBodyPtr, Vector2.zero, _offset);
		}
		TransformS.SetRotation(_v.rootNode.CMC.TC, Vector3.forward * _c.rootNode.CMC.ucpBodyStruct.a * 57.29578f, _v.rootNode.CMC.cpBodyPtr);
		_c.vehicleRotarySpringPtr = ChipmunkWrapper.AddDampedRotarySpring(ChipmunkWrapper.GetSpaceStaticBody(), _c.rootNode.CMC.cpBodyPtr, 0f, _c.rootNode.rotarySpringStrength, _c.rootNode.rotarySpringDamp);
	}

	public static GEVehicleC CreateRunner(GECharacterC _c, Vector2 _offset, float _sortOffset)
	{
		GEVehicleC gEVehicleC = GERunnerA.Assemble(_c.rootNode.TC.transform.position + Vector3.up * -5f, _c.rootNode.CMC.colliderGroup, GameState.m_playerStates[0], _c.rootNode.CMC.colliderLayer);
		gEVehicleC.hasBrakes = true;
		JumpToCart(_c, gEVehicleC, _offset, _sortOffset);
		return gEVehicleC;
	}

	public static void JumpFromCart(GECharacterC _c, bool _destroyCart, float _sortOffset)
	{
		if (_c.vehicle == null)
		{
			return;
		}
		if (_c.vehicleSpringPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.vehicleSpringPtr);
		}
		if (_c.vehicleConnectionPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.vehicleConnectionPtr);
		}
		if (_c.vehicleRotarySpringPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.vehicleRotarySpringPtr);
		}
		_c.vehicleSpringPtr = IntPtr.Zero;
		_c.vehicleConnectionPtr = IntPtr.Zero;
		_c.vehicleRotarySpringPtr = IntPtr.Zero;
		for (int i = 0; i < _c.vehicle.seats.Count; i++)
		{
			if (_c.vehicle.seatsTaken[i] != null && _c.vehicle.seatsTaken[i] == _c)
			{
				_c.vehicle.seatsTaken[i] = null;
			}
		}
		_c.vehicle.characters.Remove(_c);
		if (_destroyCart)
		{
			EntityManager.RemoveEntity(_c.vehicle.entityIndex);
		}
		_c.vehicle = null;
		for (int j = 0; j < _c.SPC.nodes.Length; j++)
		{
			SpritePrefabNode spritePrefabNode = _c.SPC.nodes[j];
			if (spritePrefabNode.hasPhysics == 1)
			{
				ChipmunkWrapper.SetBodyGroup(spritePrefabNode.CMC.cpBodyPtr, _c.rootNode.CMC.colliderGroup);
				ChipmunkWrapper.SetCustomBodyAngularDamp(spritePrefabNode.CMC.cpBodyPtr, spritePrefabNode.angularDamp);
			}
			if (spritePrefabNode.isArm == 1 || spritePrefabNode.isLeg == 1)
			{
				ChipmunkWrapper.SetBodyLayers(spritePrefabNode.CMC.cpBodyPtr, spritePrefabNode.CMC.colliderLayer);
			}
			if (spritePrefabNode.SC != null)
			{
				SpriteS.SetSortValue(spritePrefabNode.SC, 0f - spritePrefabNode.globalPosition.z + _sortOffset);
			}
		}
		if (_c.hatSPC == null)
		{
			return;
		}
		SpritePrefabNode spritePrefabNode2 = _c.SPC.nodeTable["LocatorHats"] as SpritePrefabNode;
		for (int k = 0; k < _c.hatSPC.nodes.Length; k++)
		{
			SpritePrefabNode spritePrefabNode3 = _c.hatSPC.nodes[k];
			if (spritePrefabNode3.hasPhysics == 1 && spritePrefabNode3.isSensor == 0)
			{
				ChipmunkWrapper.SetBodyGroup(spritePrefabNode3.CMC.cpBodyPtr, _c.rootNode.CMC.colliderGroup);
			}
			if ((spritePrefabNode3.isArm == 1 || spritePrefabNode3.isLeg == 1) && spritePrefabNode3.isSensor == 0)
			{
				ChipmunkWrapper.SetBodyLayers(spritePrefabNode3.CMC.cpBodyPtr, spritePrefabNode3.CMC.colliderLayer);
			}
			if (spritePrefabNode3.SC != null)
			{
				SpriteS.SetSortValue(spritePrefabNode3.SC, 0f - spritePrefabNode3.globalPosition.z + _sortOffset - spritePrefabNode2.globalPosition.z);
			}
		}
	}

	public static void ChangeEmotionState(GECharacterC _c, EmotionState _state)
	{
		_c.emotionState = _state;
		_c.emotionStateChanged = Main.m_gameTime;
		switch (_state)
		{
		case EmotionState.Stunned:
			_c.SPC.animation = _c.SPC.animations["stunned"] as SpritePrefabAnimation;
			GESpritePrefabS.RelaxRotarySprings(_c.SPC);
			break;
		case EmotionState.Neutral:
			_c.SPC.animation = _c.SPC.animations["neutral"] as SpritePrefabAnimation;
			break;
		case EmotionState.Happy:
			_c.SPC.animation = _c.SPC.animations["excited"] as SpritePrefabAnimation;
			break;
		case EmotionState.Terrified:
			_c.SPC.animation = _c.SPC.animations["scared"] as SpritePrefabAnimation;
			break;
		case EmotionState.Excited:
			_c.SPC.animation = _c.SPC.animations["excited"] as SpritePrefabAnimation;
			break;
		case EmotionState.Nausea:
			_c.SPC.animation = _c.SPC.animations["relieved"] as SpritePrefabAnimation;
			break;
		case EmotionState.Pain:
			_c.SPC.animation = _c.SPC.animations["pain"] as SpritePrefabAnimation;
			break;
		}
	}

	public static void SetCharacterLayer(GECharacterC _c, uint _layer)
	{
	}
}

using System;
using UnityEngine;

public static class GEVehicleLogic
{
	private static ChipmunkSegmentQueryInfo lookQueryResult = default(ChipmunkSegmentQueryInfo);

	private static ChipmunkQueryInfo[] queryInfo = new ChipmunkQueryInfo[200];

	public static void Update(GEVehicleC _c)
	{
		ControllerState contollerState = _c.playerState.m_contollerState;
		IControlledComponent controlledComponent = contollerState.components[2];
		IControlledComponent controlledComponent2 = contollerState.components[3];
		IControlledComponent controlledComponent3 = contollerState.components[4];
		IControlledComponent controlledComponent4 = contollerState.components[6];
		IControlledComponent controlledComponent5 = contollerState.components[0];
		IControlledComponent controlledComponent6 = contollerState.components[7];
		IControlledComponent controlledComponent7 = contollerState.components[8];
		if (_c.vehicleType == VehicleType.Runner)
		{
			controlledComponent = null;
			controlledComponent3 = null;
			controlledComponent4 = null;
		}
		bool flag = false;
		bool flag2 = false;
		Vector2 vector = Vector2.one;
		float num = 1f;
		if (_c.touchingColliders.Count > 0)
		{
			_c.contactNormal /= (float)_c.touchingColliders.Count;
		}
		for (int i = 0; i < _c.touchingColliders.Count; i++)
		{
			GEShapeC gEShapeC = null;
			if (_c.touchingColliders[i].customComponent != null && _c.touchingColliders[i].customComponent.componentType == (ComponentType)110)
			{
				gEShapeC = _c.touchingColliders[i].customComponent as GEShapeC;
				GroundSettings groundSettings = gEShapeC.groundSettings;
				Vector2 vector2 = groundSettings.linearDamp.ToVector2();
				vector = ((!(vector.sqrMagnitude < vector2.sqrMagnitude)) ? vector2 : vector);
				num = ((!(num < gEShapeC.groundSettings.angularDamp)) ? groundSettings.angularDamp : num);
				if (groundSettings.effectIdentifier != 0)
				{
					if (_c.vehicleType == VehicleType.Runner)
					{
						for (int j = 0; j < _c.characters.Count; j++)
						{
							GEAffectionC gEAffectionC = null;
							for (int k = 0; k < _c.characters[j].affections.Count; k++)
							{
								if (_c.characters[j].affections[k].identifier == groundSettings.effectIdentifier)
								{
									gEAffectionC = _c.characters[j].affections[k];
								}
							}
							if (gEAffectionC != null)
							{
								gEAffectionC.began = Main.m_gameTime;
								if (gEAffectionC.stack < gEAffectionC.maxStack)
								{
									gEAffectionC.stack++;
								}
							}
							else
							{
								GES.AddAffectionComponent(gEShapeC, _c.characters[j], groundSettings.effectIdentifier);
							}
						}
					}
					else
					{
						GEAffectionC gEAffectionC2 = null;
						for (int l = 0; l < _c.affections.Count; l++)
						{
							if (_c.affections[l].identifier == groundSettings.effectIdentifier)
							{
								gEAffectionC2 = _c.affections[l];
								break;
							}
						}
						if (gEAffectionC2 != null)
						{
							gEAffectionC2.began = Main.m_gameTime;
							if (gEAffectionC2.stack < gEAffectionC2.maxStack)
							{
								gEAffectionC2.stack++;
							}
						}
						else
						{
							GES.AddAffectionComponent(gEShapeC, _c, groundSettings.effectIdentifier);
						}
					}
				}
			}
			switch (_c.touchingColliders[i].colliderType)
			{
			case (ColliderType)5:
				flag = true;
				break;
			case (ColliderType)9:
				flag2 = true;
				break;
			}
		}
		ContactState contactState = ContactState.OnAir;
		if (flag)
		{
			contactState = ContactState.OnLiquid;
		}
		else if (flag2)
		{
			contactState = ContactState.OnSolid;
			if (_c.lastJump + 0.25f < Main.m_gameTime)
			{
				_c.jumpPower = 1f;
				_c.lastContact = Main.m_gameTime;
			}
			if (_c.contactState != ContactState.OnSolid)
			{
				_c.firstContact = Main.m_gameTime;
				GECreatureLogic.ChangeMovementState(_c, MovementState.Landing);
			}
		}
		if (_c.contactState == ContactState.OnAir && _c.lastContact + 0.15f < Main.m_gameTime)
		{
			_c.jumpPower = 0f;
			if (_c.movementState != MovementState.Jumping && _c.movementState != MovementState.Falling)
			{
				GECreatureLogic.ChangeMovementState(_c, MovementState.Falling);
			}
		}
		if (_c.contactState != contactState)
		{
			GECreatureLogic.ChangeContactState(_c, contactState);
		}
		if (_c.driveSoundLoop != null && _c.health > 0f && _c.vehicleType != VehicleType.Runner)
		{
			if (contactState == ContactState.OnSolid)
			{
				if (!_c.driveSoundLoop.m_isPlaying)
				{
					_c.driveSoundLoop.Play(0f);
				}
				else
				{
					float val = Mathf.Abs(_c.tires[0].CMC.ucpBodyStruct.w);
					float target = ToolBox.getPositionBetween(val, 5f, 50f) * 0.8f;
					float num2 = ToolBox.getPositionBetween(val, 0f, 50f) * 0.5f;
					SoundS.setVolumeTweenSpeed(_c.driveSoundLoop.m_soundId, 0.1f);
					SoundS.setVolumeTarget(_c.driveSoundLoop.m_soundId, target, false);
					SoundS.setPitch(_c.driveSoundLoop.m_soundId, 0.75f + num2);
				}
			}
			else if (_c.driveSoundLoop.m_isPlaying)
			{
				SoundS.setVolumeTweenSpeed(_c.driveSoundLoop.m_soundId, 0.1f);
				SoundS.setVolumeTarget(_c.driveSoundLoop.m_soundId, 0f, false);
			}
		}
		for (int m = 0; m < _c.tires.Length; m++)
		{
			if (_c.tireEffects[m] != null && _c.tireEffectTimes[m] < Main.m_gameTime - 0.25f)
			{
				EntityManager.RemoveEntity(_c.tireEffects[m]);
				_c.tireEffects[m] = null;
				_c.tireEffectTimes[m] = 0f;
			}
		}
		_c.backBlocked = false;
		_c.frontBlocked = false;
		int num3 = ChipmunkWrapper.BBQuery(Vector2.one * 40f, _c.rootNode.CMC.ucpBodyStruct.p + Vector2.up * 20f, _c.rootNode.CMC.colliderGroup, 17895697u, queryInfo);
		Vector2 pos = Vector2.zero;
		if (num3 > 0)
		{
			ChipmunkC chipmunkC = null;
			ChipmunkC chipmunkC2 = null;
			float num4 = 9999999f;
			float num5 = 9999999f;
			ChipmunkSegmentQueryInfo result = default(ChipmunkSegmentQueryInfo);
			for (int n = 0; n < num3; n++)
			{
				ChipmunkQueryInfo chipmunkQueryInfo = queryInfo[n];
				ChipmunkC chipmunkC3 = ChipmunkS.m_components.m_array[chipmunkQueryInfo.componentIndex];
				if (_c.characters.Count <= 0)
				{
					continue;
				}
				Vector2 start = _c.rootNode.CMC.ucpBodyStruct.p + new Vector2(-10 * _c.characters[0].SPC.flipX, 10f);
				Vector2 pos2 = chipmunkQueryInfo.pos;
				ChipmunkWrapper.SegmentQuery(start, pos2, _c.rootNode.CMC.colliderGroup, 17895697u, ref result);
				if (!((_c.rootNode.CMC.ucpBodyStruct.p.x - result.p.x) * (float)(-_c.characters[0].SPC.flipX) > 0f) || result.unityComponentIndex != chipmunkC3.index || !(result.d > 0f) || chipmunkC3.customComponent == null)
				{
					continue;
				}
				if (chipmunkC3.customComponent.componentType == (ComponentType)100)
				{
					if (result.d < num4)
					{
						pos = chipmunkQueryInfo.pos;
						chipmunkC = chipmunkC3;
						num4 = result.d;
					}
				}
				else if (chipmunkC3.customComponent.componentType == (ComponentType)113 && result.d < num5)
				{
					chipmunkC2 = chipmunkC3;
					num5 = result.d;
				}
			}
			ChipmunkC chipmunkC4 = null;
			if (chipmunkC != null)
			{
				chipmunkC4 = chipmunkC;
			}
			else if (chipmunkC2 != null)
			{
				chipmunkC4 = chipmunkC2;
			}
			if (_c.focusCMC != chipmunkC4 && _c.focusCMC != null && _c.focusCMC.entityIndex > -1)
			{
				PrefabS.ColorizeByTransformComponent(_c.focusCMC.TC, Color.grey, false, false);
			}
			if (chipmunkC4 != null)
			{
				_c.focusCMC = chipmunkC4;
				PrefabS.ColorizeByTransformComponent(_c.focusCMC.TC, DebugDraw.GetColor(160f, 160f, 160f), false, false);
			}
		}
		else if (_c.focusCMC != null)
		{
			if (_c.focusCMC.entityIndex > -1)
			{
				PrefabS.ColorizeByTransformComponent(_c.focusCMC.TC, Color.grey, false, false);
			}
			_c.focusCMC = null;
		}
		if (controlledComponent7 != null && controlledComponent7.began)
		{
			if (_c.focusCMC != null)
			{
				ComponentType componentType = _c.focusCMC.customComponent.componentType;
				IComponent customComponent = _c.focusCMC.customComponent;
				switch (componentType)
				{
				case (ComponentType)100:
					if (_c.carrying)
					{
						EndCarry(_c, _c.carriedCMC);
					}
					else
					{
						BeginCarry(_c, _c.focusCMC, pos, true);
					}
					break;
				case (ComponentType)113:
				{
					for (int num6 = 0; num6 < _c.characters.Count; num6++)
					{
						GECharacterLogic.JumpToCart(_c.characters[num6], customComponent as GEVehicleC, Vector3.zero, 0f);
					}
					controlledComponent7.began = false;
					break;
				}
				}
			}
			else if (_c.carriedCMC != null)
			{
				EndCarry(_c, _c.carriedCMC);
			}
			else if (_c.vehicleType != VehicleType.Runner)
			{
				for (int num7 = _c.characters.Count - 1; num7 > -1; num7--)
				{
					GECharacterC c = _c.characters[num7];
					GECharacterLogic.JumpFromCart(c, false, 0f);
					GECharacterLogic.CreateRunner(c, Vector2.up * 5f, 0f);
				}
			}
		}
		if (_c.carrying && _c.currentLayer != _c.carriedCMC.colliderLayer)
		{
			SetCartLayer(_c, _c.carriedCMC.colliderLayer);
		}
		else if (!_c.carrying)
		{
			ChipmunkSegmentQueryInfo result2 = default(ChipmunkSegmentQueryInfo);
			Vector2 zero = Vector2.zero;
			for (int num8 = 0; num8 < _c.tires.Length; num8++)
			{
				zero += _c.tires[num8].CMC.ucpBodyStruct.p;
			}
			for (int num9 = 0; num9 < _c.crawlers.Length; num9++)
			{
				zero += _c.crawlers[num9].CMC.ucpBodyStruct.p;
			}
			zero /= (float)(_c.tires.Length + _c.crawlers.Length);
			Vector2 end = zero + Vector2.up * -100f;
			if (!_c.backBlocked && !_c.frontBlocked)
			{
				ChipmunkWrapper.SegmentQuery(zero, end, _c.currentGroup, GEState.layer_all, ref result2);
			}
			else if (!_c.backBlocked && _c.frontBlocked)
			{
				ChipmunkWrapper.SegmentQuery(zero, end, _c.currentGroup, GEState.layer_back, ref result2);
			}
			else if (!_c.frontBlocked && _c.backBlocked)
			{
				ChipmunkWrapper.SegmentQuery(zero, end, _c.currentGroup, GEState.layer_front, ref result2);
			}
			if (_c.contactState == ContactState.OnAir && result2.d > 5f && result2.unityComponentIndex != -1)
			{
				ChipmunkC chipmunkC5 = ChipmunkS.m_components.m_array[result2.unityComponentIndex];
				if (_c.frontBlocked && _c.backBlocked)
				{
					SetCartLayer(_c, GEState.layer_middle);
				}
				else if (chipmunkC5.colliderLayer == GEState.layer_front && !_c.frontBlocked)
				{
					SetCartLayer(_c, GEState.layer_front);
				}
				else if (chipmunkC5.colliderLayer == GEState.layer_back && !_c.backBlocked)
				{
					SetCartLayer(_c, GEState.layer_back);
				}
				else
				{
					SetCartLayer(_c, GEState.layer_middle);
				}
			}
			if (controlledComponent5 != null && ((_c.rootNode.CMC.ucpBodyStruct.v.x < -10f && _c.SPC.flipX == 1 && controlledComponent5.output.vector.x < 0f) || (_c.rootNode.CMC.ucpBodyStruct.v.x > 10f && _c.SPC.flipX == -1 && controlledComponent5.output.vector.x > 0f)))
			{
				_c.SPC.flipX *= -1;
				for (int num10 = 0; num10 < _c.characters.Count; num10++)
				{
					GESpritePrefabS.FlipX(_c.characters[num10].SPC);
					if (_c.characters[num10].hatSPC != null)
					{
						GESpritePrefabS.FlipX(_c.characters[num10].hatSPC);
						SpritePrefabNode rootNode = _c.characters[num10].hatSPC.rootNode;
						Vector3 localPosition = rootNode.TC.transform.localPosition;
						localPosition.x *= -1f;
						rootNode.TC.transform.localPosition = localPosition;
					}
					if (_c.rootNode.hasSuspension == 1)
					{
						ChipmunkDampedSpringStruct springRef = default(ChipmunkDampedSpringStruct);
						ChipmunkWrapper.GetDampedSpringProperties(_c.characters[num10].vehicleSpringPtr, ref springRef);
						springRef.b.x *= -1f;
						ChipmunkWrapper.RemoveConstraint(_c.characters[num10].vehicleSpringPtr);
						ChipmunkWrapper.RemoveConstraint(_c.characters[num10].vehicleConnectionPtr);
						_c.characters[num10].vehicleSpringPtr = ChipmunkWrapper.AddDampedSpring(_c.characters[num10].rootNode.CMC.cpBodyPtr, _c.rootNode.CMC.cpBodyPtr, Vector2.zero, springRef.b, 0f, _c.rootNode.suspensionStrength, _c.rootNode.suspensionDamp);
						springRef.b.y = 1f;
						_c.characters[num10].vehicleConnectionPtr = ChipmunkWrapper.AddGrooveJoint(_c.characters[num10].rootNode.CMC.cpBodyPtr, _c.rootNode.CMC.cpBodyPtr, springRef.b * _c.rootNode.suspensionDepth * 0.5f, springRef.b * (0f - _c.rootNode.suspensionDepth) * 0.5f, Vector2.zero);
					}
					else
					{
						ChipmunkPivotJointStruct jointRef = default(ChipmunkPivotJointStruct);
						ChipmunkWrapper.GetPivotJointProperties(_c.characters[num10].vehicleConnectionPtr, ref jointRef);
						jointRef.b.x *= -1f;
						ChipmunkWrapper.SetPivotJointOffsetB(_c.characters[num10].vehicleConnectionPtr, jointRef.b);
					}
				}
			}
		}
		Vector2 vector3 = _c.rootNode.TC.transform.position;
		Vector2 vector4 = vector3;
		if (controlledComponent4 != null)
		{
			if (controlledComponent4.triggered)
			{
				if (controlledComponent4.energy > 0f)
				{
					Vector2 rot = _c.rootNode.CMC.ucpBodyStruct.rot;
					vector4 += new Vector2(0f - rot.y, rot.x) * -200f;
				}
			}
			else
			{
				vector4 += _c.rootNode.CMC.ucpBodyStruct.v * 0.5f;
			}
		}
		else
		{
			vector4 += _c.rootNode.CMC.ucpBodyStruct.v * 0.5f;
		}
		ChipmunkWrapper.SegmentQuery(vector3, vector4, _c.currentGroup, _c.currentLayer, ref lookQueryResult);
		Vector2 vector5 = new Vector2(0f - _c.rootNode.CMC.ucpBodyStruct.rot.y, _c.rootNode.CMC.ucpBodyStruct.rot.x);
		if (lookQueryResult.p == Vector2.zero || lookQueryResult.d > 100f)
		{
			vector4 = vector3 + _c.rootNode.CMC.ucpBodyStruct.v * 1.5f + vector5 * -200f;
			ChipmunkWrapper.SegmentQuery(vector3, vector4, _c.currentGroup, _c.currentLayer, ref lookQueryResult);
		}
		float currentBalance = _c.currentBalance;
		float num11 = (float)Math.PI;
		float num12 = (float)Math.PI * 2f;
		int num13 = 0;
		if (lookQueryResult.p != Vector2.zero && lookQueryResult.d < 100f)
		{
			currentBalance = Mathf.Atan2(lookQueryResult.n.x, lookQueryResult.n.y);
			while (currentBalance + _c.rootNode.CMC.ucpBodyStruct.a > num11)
			{
				currentBalance -= num12;
				num13++;
			}
			while (currentBalance + _c.rootNode.CMC.ucpBodyStruct.a < 0f - num11)
			{
				currentBalance += num12;
				num13--;
			}
			float num14 = _c.currentBalance - currentBalance;
			if (_c.rootNode.minRotaryLimit != 0f && _c.rootNode.maxRotaryLimit != 0f && (num14 < _c.rootNode.minRotaryLimit * ((float)Math.PI / 180f) || num14 > _c.rootNode.maxRotaryLimit * ((float)Math.PI / 180f)))
			{
				vector4 = vector3 + Vector2.up * -200f;
				ChipmunkWrapper.SegmentQuery(vector3, vector4, _c.currentGroup, _c.currentLayer, ref lookQueryResult);
				currentBalance = Mathf.Atan2(lookQueryResult.n.x, lookQueryResult.n.y);
				num13 = 0;
				while (currentBalance + _c.rootNode.CMC.ucpBodyStruct.a > num11)
				{
					currentBalance -= num12;
					num13++;
				}
				while (currentBalance + _c.rootNode.CMC.ucpBodyStruct.a < 0f - num11)
				{
					currentBalance += num12;
					num13--;
				}
			}
			_c.currentBalance = currentBalance;
			_c.currentBalanceDif = num14;
			_c.currentLookDir = (vector4 - vector3).normalized;
			_c.currentLookNormal = lookQueryResult.n;
		}
		else
		{
			while (_c.currentBalance > num11)
			{
				_c.currentBalance -= num12;
				num13++;
			}
			while (_c.currentBalance < 0f - num11)
			{
				_c.currentBalance += num12;
				num13--;
			}
			_c.currentBalance *= 0.8f;
			_c.currentBalance += (float)num13 * num12;
			_c.currentBalanceDif = 0f;
			_c.currentLookDir = (vector4 - vector3).normalized;
			_c.currentLookNormal = _c.currentLookDir;
		}
		float restAngle = _c.currentBalance - _c.SPC.rootNode.globalRotation.z * ((float)Math.PI / 180f);
		float stiffness = _c.rootNode.rotarySpringStrength;
		float damping = _c.rootNode.rotarySpringDamp;
		if (_c.characters.Count == 0)
		{
			stiffness = 0f;
			damping = 0f;
		}
		if (_c.balanceSpring != IntPtr.Zero)
		{
			ChipmunkWrapper.SetDampedRotarySpringProperties(_c.balanceSpring, stiffness, damping, restAngle);
		}
		float num15 = (_c.characterBalanceAngle = Mathf.Sin(_c.currentBalance + (float)num13 * num12));
		if (_c.characters.Count > 0)
		{
			if (_c.vehicleType != VehicleType.Runner)
			{
				for (int num16 = 0; num16 < _c.characters.Count; num16++)
				{
					GECharacterC gECharacterC = _c.characters[num16];
					if (gECharacterC.vehicleRotarySpringPtr != IntPtr.Zero)
					{
						ChipmunkWrapper.SetDampedRotarySpringProperties(gECharacterC.vehicleRotarySpringPtr, gECharacterC.rootNode.rotarySpringStrength, gECharacterC.rootNode.rotarySpringDamp, restAngle);
					}
					gECharacterC.balanceAngle = num15;
					if (gECharacterC.movementState != MovementState.Stunned)
					{
						SpritePrefabNode spritePrefabNode = gECharacterC.SPC.nodeTable["Head"] as SpritePrefabNode;
						SpritePrefabNode spritePrefabNode2 = gECharacterC.SPC.nodeTable["Torso"] as SpritePrefabNode;
						ChipmunkWrapper.SetDampedRotarySpringProperties(spritePrefabNode2.rotarySpring, spritePrefabNode2.rotarySpringStrength, spritePrefabNode2.rotarySpringDamp, 0f - num15);
						ChipmunkWrapper.SetDampedRotarySpringProperties(spritePrefabNode.rotarySpring, spritePrefabNode.rotarySpringStrength, spritePrefabNode.rotarySpringDamp, num15 * 0.5f);
					}
				}
			}
			if (controlledComponent2 != null)
			{
				if (controlledComponent2.began && _c.jumpPower == 1f)
				{
					if (_c.rootNode.CMC.ucpBodyStruct.v.y < 0f)
					{
						for (int num17 = 0; num17 < _c.SPC.nodes.Length; num17++)
						{
							SpritePrefabNode spritePrefabNode3 = _c.SPC.nodes[num17];
							if (spritePrefabNode3.hasPhysics == 1)
							{
								ChipmunkWrapper.SetYVelocity(spritePrefabNode3.CMC.cpBodyPtr, 0f);
							}
						}
						for (int num18 = 0; num18 < _c.characters.Count; num18++)
						{
							GECharacterC gECharacterC2 = _c.characters[num18];
							for (int num19 = 0; num19 < gECharacterC2.SPC.nodes.Length; num19++)
							{
								SpritePrefabNode spritePrefabNode4 = gECharacterC2.SPC.nodes[num19];
								if (spritePrefabNode4.hasPhysics == 1)
								{
									ChipmunkWrapper.SetYVelocity(spritePrefabNode4.CMC.cpBodyPtr, 0f);
								}
							}
						}
						if (_c.carrying)
						{
							ChipmunkWrapper.SetYVelocity(_c.carriedCMC.cpBodyPtr, 0f);
						}
					}
					_c.lastJump = Main.m_gameTime;
					GECreatureLogic.ChangeMovementState(_c, MovementState.Jumping);
				}
				float num20 = 1400f;
				if (_c.vehicleType == VehicleType.Runner)
				{
					num20 = 900f;
				}
				float num21 = num20 * Mathf.Cos(Mathf.Atan2(_c.contactNormal.x, 0f - _c.contactNormal.y));
				if (controlledComponent2.triggered && _c.lastJump + 0.25f > Main.m_gameTime)
				{
					if (_c.jumpPower > 0f)
					{
						float num22 = num21 * _c.jumpPower;
						if (_c.lastJump + 0.25f > Main.m_gameTime)
						{
							ChipmunkWrapper.ApplyImpulse(_c.rootNode.CMC.cpBodyPtr, Vector2.up * num22, Vector2.zero, true);
							for (int num23 = 0; num23 < _c.tires.Length; num23++)
							{
								ChipmunkWrapper.ApplyImpulse(_c.tires[num23].CMC.cpBodyPtr, Vector2.up * num22 * 0.2f, Vector2.zero, true);
							}
						}
						_c.jumpPower -= 0.2f;
						_c.jumpPower = Mathf.Max(_c.jumpPower, 0f);
					}
				}
				else if (_c.jumpPower < 1f)
				{
					_c.jumpPower = 0f;
				}
			}
			if (controlledComponent3 != null)
			{
				float num24 = 400f;
				if (controlledComponent3.triggered && controlledComponent3.energy > 0f)
				{
					ChipmunkWrapper.ApplyImpulse(_c.rootNode.CMC.cpBodyPtr, Vector2.up * num24, Vector2.up * 10f, true);
				}
			}
			if (controlledComponent4 != null)
			{
				if (controlledComponent4.began && controlledComponent4.energy > 0f)
				{
					for (int num25 = 0; num25 < _c.tires.Length; num25++)
					{
						SpritePrefabNode spritePrefabNode5 = _c.tires[num25];
					}
				}
				else if (controlledComponent4.triggered)
				{
					if (controlledComponent4.energy > 0f)
					{
						float y = Mathf.Cos(_c.currentBalance);
						float x = Mathf.Sin(_c.currentBalance);
						ChipmunkWrapper.SetCustomBodyGravity(gravity: new Vector2(x, y) * -450f, bodyPtr: _c.rootNode.CMC.cpBodyPtr);
						for (int num26 = 0; num26 < _c.tires.Length; num26++)
						{
							SpritePrefabNode spritePrefabNode6 = _c.tires[num26];
							ChipmunkWrapper.SetCustomBodyGravity(spritePrefabNode6.CMC.cpBodyPtr, new Vector2(x, y) * -450f);
						}
						for (int num27 = 0; num27 < _c.characters.Count; num27++)
						{
							GECharacterC gECharacterC3 = _c.characters[num27];
							for (int num28 = 0; num28 < gECharacterC3.SPC.nodes.Length; num28++)
							{
								SpritePrefabNode spritePrefabNode7 = gECharacterC3.SPC.nodes[num28];
								if (spritePrefabNode7.hasPhysics == 1)
								{
									ChipmunkWrapper.SetCustomBodyGravity(spritePrefabNode7.CMC.cpBodyPtr, new Vector2(x, y) * -450f);
								}
							}
						}
					}
				}
				else if (controlledComponent4.end || controlledComponent4.energy == 0f)
				{
					ChipmunkWrapper.SetCustomBodyGravity(_c.rootNode.CMC.cpBodyPtr, Vector2.up * -450f);
					for (int num29 = 0; num29 < _c.tires.Length; num29++)
					{
						SpritePrefabNode spritePrefabNode8 = _c.tires[num29];
						ChipmunkWrapper.SetCustomBodyGravity(spritePrefabNode8.CMC.cpBodyPtr, Vector2.up * -450f);
					}
					for (int num30 = 0; num30 < _c.characters.Count; num30++)
					{
						GECharacterC gECharacterC4 = _c.characters[num30];
						for (int num31 = 0; num31 < gECharacterC4.SPC.nodes.Length; num31++)
						{
							SpritePrefabNode spritePrefabNode9 = gECharacterC4.SPC.nodes[num31];
							if (spritePrefabNode9.hasPhysics == 1)
							{
								ChipmunkWrapper.SetCustomBodyGravity(spritePrefabNode9.CMC.cpBodyPtr, Vector2.up * -450f);
							}
						}
					}
				}
			}
			if (controlledComponent != null)
			{
				float num32 = 6000f;
				Vector2 right = Vector2.right;
				ChipmunkWrapper.ResetForces(_c.rootNode.CMC.cpBodyPtr);
				if (controlledComponent.triggered)
				{
					if (controlledComponent.energy > 0f)
					{
						if (_c.SPC.flipX == 1)
						{
							if (_c.movementState == MovementState.Landing || _c.movementState == MovementState.Idling)
							{
								_c.movementState = MovementState.Moving;
							}
							if (_c.tires.Length > 0)
							{
								if (_c.braking)
								{
									SetTireBrakes(_c, false);
								}
								if (_c.contactState == ContactState.OnAir && _c.rootNode.CMC.ucpBodyStruct.v.x < 50f)
								{
									ChipmunkWrapper.ApplyForce(_c.rootNode.CMC.cpBodyPtr, right * num32, Vector2.zero, true);
								}
								for (int num33 = 0; num33 < _c.tires.Length; num33++)
								{
									SpritePrefabNode spritePrefabNode10 = _c.tires[num33];
									float num34 = 1f - Mathf.Max(Mathf.Min(spritePrefabNode10.CMC.ucpBodyStruct.w / num12 / spritePrefabNode10.motorRate, 1f), 0f);
									if (spritePrefabNode10.CMC.ucpBodyStruct.w > 0f)
									{
										num34 = 1f;
									}
									ChipmunkWrapper.SetMotorProperties(spritePrefabNode10.motor, spritePrefabNode10.motorRate * num12, spritePrefabNode10.motorStrength * num34);
								}
							}
							else if (_c.crawlers.Length <= 0)
							{
							}
						}
						else if (_c.SPC.flipX == -1)
						{
							if (_c.movementState == MovementState.Landing || _c.movementState == MovementState.Idling)
							{
								_c.movementState = MovementState.Moving;
							}
							if (_c.tires.Length > 0)
							{
								if (_c.braking)
								{
									SetTireBrakes(_c, false);
								}
								if (_c.contactState == ContactState.OnAir && _c.rootNode.CMC.ucpBodyStruct.v.x > -50f)
								{
									ChipmunkWrapper.ApplyForce(_c.rootNode.CMC.cpBodyPtr, right * (0f - num32), Vector2.zero, true);
								}
								for (int num35 = 0; num35 < _c.tires.Length; num35++)
								{
									SpritePrefabNode spritePrefabNode11 = _c.tires[num35];
									float num36 = 1f - Mathf.Max(Mathf.Min(spritePrefabNode11.CMC.ucpBodyStruct.w / num12 / spritePrefabNode11.motorRate, 1f), 0f);
									if (spritePrefabNode11.CMC.ucpBodyStruct.w < 0f)
									{
										num36 = 1f;
									}
									ChipmunkWrapper.SetMotorProperties(spritePrefabNode11.motor, (0f - spritePrefabNode11.motorRate) * num12, spritePrefabNode11.motorStrength * num36);
								}
							}
							else if (_c.crawlers.Length <= 0)
							{
							}
						}
					}
				}
				else if (controlledComponent.end)
				{
					for (int num37 = 0; num37 < _c.tires.Length; num37++)
					{
						SpritePrefabNode spritePrefabNode12 = _c.tires[num37];
						ChipmunkWrapper.SetMotorProperties(spritePrefabNode12.motor, 0f, 0f);
					}
				}
			}
			if (controlledComponent5 != null)
			{
				float num38 = 8000f;
				if (_c.vehicleType == VehicleType.Runner)
				{
					num38 = 4000f;
				}
				Vector2 right2 = Vector2.right;
				ChipmunkWrapper.ResetForces(_c.rootNode.CMC.cpBodyPtr);
				if (controlledComponent5.triggered)
				{
					if (controlledComponent5.energy > 0f)
					{
						if (controlledComponent5.output.vector.x > 0f)
						{
							if (_c.movementState == MovementState.Landing || _c.movementState == MovementState.Idling)
							{
								_c.movementState = MovementState.Moving;
							}
							if (_c.tires.Length > 0)
							{
								if (_c.braking)
								{
									SetTireBrakes(_c, false);
								}
								for (int num39 = 0; num39 < _c.tires.Length; num39++)
								{
									SpritePrefabNode spritePrefabNode13 = _c.tires[num39];
									float num40 = 1f - Mathf.Max(Mathf.Min(spritePrefabNode13.CMC.ucpBodyStruct.w / num12 / spritePrefabNode13.motorRate, 1f), 0f);
									if (spritePrefabNode13.CMC.ucpBodyStruct.w > 0f)
									{
										num40 = 1f;
									}
									ChipmunkWrapper.SetMotorProperties(spritePrefabNode13.motor, spritePrefabNode13.motorRate * num12, spritePrefabNode13.motorStrength * num40);
								}
							}
							else if (_c.crawlers.Length > 0)
							{
								if (_c.braking)
								{
									SetCrawlerBrakes(_c, false);
								}
								else if (_c.currentBrakeAmount < 1f)
								{
									_c.currentBrakeAmount = Mathf.Min(_c.currentBrakeAmount + 0.1f, 1f);
								}
								for (int num41 = 0; num41 < _c.crawlers.Length; num41++)
								{
									SpritePrefabNode spritePrefabNode14 = _c.crawlers[num41];
									ChipmunkWrapper.SetBodySurfaceVelocity(spritePrefabNode14.CMC.cpBodyPtr, right2 * spritePrefabNode14.motorRate * _c.currentBrakeAmount);
								}
							}
							if (_c.contactState == ContactState.OnAir && _c.rootNode.CMC.ucpBodyStruct.v.x < 50f)
							{
								ChipmunkWrapper.ApplyForce(_c.rootNode.CMC.cpBodyPtr, right2 * num38, Vector2.zero, true);
							}
						}
						else if (controlledComponent5.output.vector.x < 0f)
						{
							if (_c.movementState == MovementState.Landing || _c.movementState == MovementState.Idling)
							{
								_c.movementState = MovementState.Moving;
							}
							if (_c.tires.Length > 0)
							{
								if (_c.braking)
								{
									SetTireBrakes(_c, false);
								}
								for (int num42 = 0; num42 < _c.tires.Length; num42++)
								{
									SpritePrefabNode spritePrefabNode15 = _c.tires[num42];
									float num43 = 1f - Mathf.Max(Mathf.Min(spritePrefabNode15.CMC.ucpBodyStruct.w / num12 / spritePrefabNode15.motorRate, 1f), 0f);
									if (spritePrefabNode15.CMC.ucpBodyStruct.w < 0f)
									{
										num43 = 1f;
									}
									ChipmunkWrapper.SetMotorProperties(spritePrefabNode15.motor, (0f - spritePrefabNode15.motorRate) * num12, spritePrefabNode15.motorStrength * num43);
								}
							}
							else if (_c.crawlers.Length > 0)
							{
								if (_c.braking)
								{
									SetCrawlerBrakes(_c, false);
								}
								else if (_c.currentBrakeAmount < 1f)
								{
									_c.currentBrakeAmount = Mathf.Min(_c.currentBrakeAmount + 0.1f, 1f);
								}
								for (int num44 = 0; num44 < _c.crawlers.Length; num44++)
								{
									SpritePrefabNode spritePrefabNode16 = _c.crawlers[num44];
									ChipmunkWrapper.SetBodySurfaceVelocity(spritePrefabNode16.CMC.cpBodyPtr, -right2 * spritePrefabNode16.motorRate * _c.currentBrakeAmount);
								}
							}
							if (_c.contactState == ContactState.OnAir && _c.rootNode.CMC.ucpBodyStruct.v.x > -50f)
							{
								ChipmunkWrapper.ApplyForce(_c.rootNode.CMC.cpBodyPtr, right2 * (0f - num38), Vector2.zero, true);
							}
						}
					}
				}
				else if (controlledComponent5.end)
				{
					if (_c.hasBrakes)
					{
						if (!_c.braking)
						{
							if (_c.tires.Length > 0)
							{
								SetTireBrakes(_c, true);
							}
							else if (_c.crawlers.Length > 0)
							{
								SetCrawlerBrakes(_c, true);
							}
						}
					}
					else
					{
						for (int num45 = 0; num45 < _c.tires.Length; num45++)
						{
							SpritePrefabNode spritePrefabNode17 = _c.tires[num45];
							ChipmunkWrapper.SetMotorProperties(spritePrefabNode17.motor, 0f, 0f);
						}
					}
				}
				else if (!controlledComponent5.triggered && _c.braking && _c.currentBrakeAmount > 0f)
				{
					_c.currentBrakeAmount = Mathf.Max(_c.currentBrakeAmount - 0.1f, 0f);
					for (int num46 = 0; num46 < _c.crawlers.Length; num46++)
					{
						SpritePrefabNode spritePrefabNode18 = _c.crawlers[num46];
						ChipmunkWrapper.SetBodySurfaceVelocity(spritePrefabNode18.CMC.cpBodyPtr, _c.characters[0].SPC.flipX * right2 * spritePrefabNode18.motorRate * _c.currentBrakeAmount);
					}
				}
			}
		}
		Vector3 position = _c.rootNode.CMC.TC.transform.position;
		float num47 = _c.desiredZ - position.z;
		if (!(Mathf.Abs(num47) > 0.1f) && !_c.updateCharacterDepth)
		{
			return;
		}
		_c.updateCharacterDepth = false;
		position.z += num47 * 0.1f;
		_c.rootNode.CMC.TC.transform.position = position;
		for (int num48 = 0; num48 < _c.SPC.nodes.Length; num48++)
		{
			SpritePrefabNode spritePrefabNode19 = _c.SPC.nodes[num48];
			Vector3 position2 = spritePrefabNode19.TC.transform.position;
			position2.z = position.z;
			spritePrefabNode19.TC.transform.position = position2;
		}
		for (int num49 = 0; num49 < _c.characters.Count; num49++)
		{
			for (int num50 = 0; num50 < _c.characters[num49].SPC.nodes.Length; num50++)
			{
				SpritePrefabNode spritePrefabNode20 = _c.characters[num49].SPC.nodes[num50];
				Vector3 position3 = spritePrefabNode20.TC.transform.position;
				position3.z = position.z;
				spritePrefabNode20.TC.transform.position = position3;
			}
			if (_c.characters[num49].hatSPC == null)
			{
				continue;
			}
			for (int num51 = 0; num51 < _c.characters[num49].hatSPC.nodes.Length; num51++)
			{
				SpritePrefabNode spritePrefabNode21 = _c.characters[num49].hatSPC.nodes[num51];
				if (spritePrefabNode21.hasPhysics == 1)
				{
					Vector3 position4 = spritePrefabNode21.TC.transform.position;
					position4.z = position.z;
					spritePrefabNode21.TC.transform.position = position4;
				}
			}
		}
	}

	public static void SetTireBrakes(GEVehicleC _v, bool _brake)
	{
		if (_brake)
		{
			_v.braking = true;
			for (int i = 0; i < _v.tires.Length; i++)
			{
				SpritePrefabNode spritePrefabNode = _v.tires[i];
				ChipmunkWrapper.ResetForces(spritePrefabNode.CMC.cpBodyPtr);
				if (spritePrefabNode.motor != IntPtr.Zero)
				{
					ChipmunkWrapper.SetMotorProperties(spritePrefabNode.motor, 0f, spritePrefabNode.motorStrength);
				}
			}
			return;
		}
		_v.braking = false;
		for (int j = 0; j < _v.tires.Length; j++)
		{
			SpritePrefabNode spritePrefabNode2 = _v.tires[j];
			ChipmunkWrapper.ResetForces(spritePrefabNode2.CMC.cpBodyPtr);
			if (spritePrefabNode2.motor != IntPtr.Zero)
			{
				ChipmunkWrapper.SetMotorProperties(spritePrefabNode2.motor, 0f, 0f);
			}
		}
	}

	public static void SetCrawlerBrakes(GEVehicleC _v, bool _brake)
	{
		if (_brake)
		{
			_v.braking = true;
		}
		else
		{
			_v.braking = false;
		}
	}

	public static void SetCartLayer(GEVehicleC _c, uint _layer)
	{
		if (_c.currentLayer == _layer)
		{
			return;
		}
		if (_layer == GEState.layer_front)
		{
			_c.desiredZ = 12.5f;
		}
		else if (_layer == GEState.layer_back)
		{
			_c.desiredZ = 62.5f;
		}
		else
		{
			_c.desiredZ = 37.5f;
		}
		if (_c.vehicleType == VehicleType.Runner)
		{
			_c.desiredZ -= 10f;
		}
		for (int i = 0; i < _c.SPC.nodes.Length; i++)
		{
			if (_c.SPC.nodes[i].hasPhysics == 1)
			{
				ChipmunkWrapper.SetBodyLayers(_c.SPC.nodes[i].CMC.cpBodyPtr, _layer);
			}
		}
		for (int j = 0; j < _c.characters.Count; j++)
		{
			for (int k = 0; k < _c.characters[j].SPC.nodes.Length; k++)
			{
				if (_c.characters[j].SPC.nodes[k].hasPhysics == 1)
				{
					ChipmunkWrapper.SetBodyLayers(_c.characters[j].SPC.nodes[k].CMC.cpBodyPtr, _layer);
				}
			}
			if (_c.characters[j].hatSPC == null)
			{
				continue;
			}
			for (int l = 0; l < _c.characters[j].hatSPC.nodes.Length; l++)
			{
				if (_c.characters[j].hatSPC.nodes[l].hasPhysics == 1)
				{
					ChipmunkWrapper.SetBodyLayers(_c.characters[j].hatSPC.nodes[l].CMC.cpBodyPtr, _layer);
				}
			}
		}
		_c.currentLayer = _layer;
	}

	public static void BeginCarry(GEVehicleC _c, ChipmunkC _component, Vector2 _pos, bool _drag)
	{
		ChipmunkSegmentQueryInfo result = default(ChipmunkSegmentQueryInfo);
		Vector2 p = _c.rootNode.CMC.ucpBodyStruct.p;
		ChipmunkWrapper.SegmentQuery(p, _pos, _c.rootNode.CMC.colliderGroup, 17895697u, ref result);
		if (result.unityComponentIndex != _component.index)
		{
			return;
		}
		_c.carrying = true;
		_c.dragging = _drag;
		_c.carriedCMC = _component;
		if (_c.vehicleType != VehicleType.Runner || _c.characters.Count <= 0)
		{
			return;
		}
		GECharacterC gECharacterC = _c.characters[0];
		for (int i = 0; i < gECharacterC.arms.Length; i++)
		{
			gECharacterC.armSprings[i] = ChipmunkWrapper.AddPivotJoint(gECharacterC.arms[i].CMC.cpBodyPtr, _c.carriedCMC.cpBodyPtr, result.p + Vector2.up * -5f);
			ChipmunkWrapper.SetPivotJointOffsetA(gECharacterC.armSprings[i], Vector2.up * -5f);
			if (!_drag)
			{
				ChipmunkWrapper.SetBodyGroup(_component.cpBodyPtr, gECharacterC.arms[i].CMC.colliderGroup);
			}
			else
			{
				ChipmunkWrapper.SetBodyLayers(gECharacterC.arms[i].CMC.cpBodyPtr, 0u);
			}
		}
	}

	public static void EndCarry(GEVehicleC _c, ChipmunkC _component)
	{
		if (_c.vehicleType != VehicleType.Runner || _c.characters.Count <= 0)
		{
			return;
		}
		GECharacterC gECharacterC = _c.characters[0];
		for (int i = 0; i < gECharacterC.arms.Length; i++)
		{
			if (gECharacterC.armSprings[i] != IntPtr.Zero)
			{
				if (!_c.dragging)
				{
					ChipmunkWrapper.SetBodyGroup(_component.cpBodyPtr, _component.colliderGroup);
				}
				else
				{
					ChipmunkWrapper.SetBodyLayers(gECharacterC.arms[i].CMC.cpBodyPtr, gECharacterC.arms[i].CMC.colliderLayer);
				}
				ChipmunkWrapper.RemoveConstraint(gECharacterC.armSprings[i]);
				gECharacterC.armSprings[i] = IntPtr.Zero;
			}
		}
		_c.carrying = false;
		_c.dragging = false;
		_c.carriedCMC = null;
	}
}

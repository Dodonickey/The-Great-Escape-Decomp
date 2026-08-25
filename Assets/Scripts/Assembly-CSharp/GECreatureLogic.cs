using System;
using UnityEngine;

public static class GECreatureLogic
{
	public static AffectionInfo[] m_tickAffections = new AffectionInfo[10];

	public static AffectionInfo[] m_beganAffections = new AffectionInfo[10];

	public static AffectionInfo[] m_endAffections = new AffectionInfo[10];

	public static void ChangeContactState(GECreatureC _c, ContactState _state)
	{
		ContactState contactState = _c.contactState;
		_c.contactState = _state;
		_c.contactStateChanged = Main.m_gameTime;
	}

	public static void ChangeMovementState(GECreatureC _c, MovementState _state)
	{
		MovementState movementState = _c.movementState;
		_c.movementState = _state;
		_c.movementStateChanged = Main.m_gameTime;
		if (_c.creatureType == CreatureType.Biped)
		{
			switch (_state)
			{
			case MovementState.Moving:
				_c.SPC.animation = _c.SPC.animations["run"] as SpritePrefabAnimation;
				break;
			case MovementState.Idling:
				_c.SPC.animation = _c.SPC.animations["stand"] as SpritePrefabAnimation;
				break;
			}
		}
	}

	public static void KillCreature(GEAffectionC _affection, int _killingEffect)
	{
		GECreatureC affected = _affection.affected;
		if (affected.creatureType == CreatureType.Biped)
		{
			GECharacterC gECharacterC = affected as GECharacterC;
			if (gECharacterC.vehicle != null)
			{
				if (gECharacterC.vehicle.vehicleType == VehicleType.Runner)
				{
					GECharacterLogic.JumpFromCart(gECharacterC, true, 10f);
				}
				else
				{
					GECharacterLogic.JumpFromCart(gECharacterC, false, 150f);
					GESpritePrefabS.RelaxRotarySprings(gECharacterC.SPC);
				}
			}
			if (gECharacterC.movementState != MovementState.Dead)
			{
				ChangeMovementState(gECharacterC, MovementState.Dead);
			}
			if (gECharacterC.emotionState != EmotionState.Stunned)
			{
				GECharacterLogic.ChangeEmotionState(gECharacterC, EmotionState.Stunned);
			}
			string[] keys = new string[3] { "GE_killed", "GE_killer", "GE_killingEffect" };
			object[] values = new object[3] { _affection.affected, _affection.source, _killingEffect };
			EventS.Dispatch("GE_dead", keys, values, true);
			FStunStarA.m_starCount = 0;
			for (int i = 0; i < gECharacterC.heads.Length; i++)
			{
				FStunStarA.Assemble(gECharacterC.heads[i].TC, 0.5f);
			}
		}
		else
		{
			if (affected.creatureType != CreatureType.Vehicle)
			{
				return;
			}
			GEVehicleC gEVehicleC = affected as GEVehicleC;
			while (gEVehicleC.characters.Count > 0)
			{
				int index = gEVehicleC.characters.Count - 1;
				GECharacterC gECharacterC2 = gEVehicleC.characters[index];
				GECharacterLogic.JumpFromCart(gECharacterC2, false, 160f);
				GESpritePrefabS.RelaxRotarySprings(gECharacterC2.SPC);
				if (gECharacterC2.movementState != MovementState.Dead)
				{
					ChangeMovementState(gECharacterC2, MovementState.Dead);
				}
				if (gECharacterC2.emotionState != EmotionState.Stunned)
				{
					GECharacterLogic.ChangeEmotionState(gECharacterC2, EmotionState.Stunned);
				}
				FStunStarA.m_starCount = 0;
				for (int j = 0; j < gECharacterC2.heads.Length; j++)
				{
					FStunStarA.Assemble(gECharacterC2.heads[j].TC, 0.5f);
				}
			}
			if (gEVehicleC.vehicleType != VehicleType.Runner)
			{
				gEVehicleC.balanceSpring = IntPtr.Zero;
				gEVehicleC.SPC.rootNode.dampedSpring = IntPtr.Zero;
				gEVehicleC.SPC.rootNode.rotarySpring = IntPtr.Zero;
				gEVehicleC.SPC.rootNode.pivotJoint = IntPtr.Zero;
				gEVehicleC.SPC.rootNode.grooveJoint = IntPtr.Zero;
				gEVehicleC.SPC.rootNode.rotaryLimitJoint = IntPtr.Zero;
				ChipmunkWrapper.RemoveConstraintsFromBody(gEVehicleC.SPC.rootNode.CMC.cpBodyPtr);
				for (int k = 0; k < gEVehicleC.SPC.nodes.Length; k++)
				{
					if (gEVehicleC.SPC.nodes[k].isProp == 1 && gEVehicleC.SPC.nodes[k].CMC != null)
					{
						ChipmunkWrapper.SetBodyLayers(gEVehicleC.SPC.nodes[k].CMC.cpBodyPtr, gEVehicleC.SPC.rootNode.CMC.colliderLayer);
					}
				}
			}
			if (gEVehicleC.movementState != MovementState.Dead)
			{
				ChangeMovementState(gEVehicleC, MovementState.Dead);
			}
			if (gEVehicleC.vehicleType == VehicleType.Runner)
			{
				EntityManager.RemoveEntity(gEVehicleC.entityIndex);
			}
			string[] keys2 = new string[3] { "GE_killed", "GE_killer", "GE_killingEffect" };
			object[] values2 = new object[3] { _affection.affected, _affection.source, _killingEffect };
			EventS.Dispatch("GE_dead", keys2, values2, true);
		}
	}

	public static void AddInstantAffection(IComponent _source, GECreatureC _affected, GEEffectType _effectType, int _amount)
	{
		GEAffectionC gEAffectionC = GES.AddAffectionComponent(_source, _affected, 0u);
		gEAffectionC.stack = 1;
		gEAffectionC.beganEffect.effectActive[(int)_effectType] = true;
		gEAffectionC.beganEffect.effects[(int)_effectType] = _amount;
		UpdateAffection(gEAffectionC);
	}

	public static int ApplyAffection(GEAffectionC _a, int phase)
	{
		GEEffect gEEffect = null;
		switch (phase)
		{
		case 0:
			gEEffect = _a.beganEffect;
			break;
		case 1:
			gEEffect = _a.tickEffect;
			break;
		case 2:
			gEEffect = _a.endEffect;
			break;
		}
		if (gEEffect != null)
		{
			GECreatureC affected = _a.affected;
			for (int i = 0; i < gEEffect.effects.Length; i++)
			{
				if (!gEEffect.effectActive[i])
				{
					continue;
				}
				if (_a.affectionType == GEAffectionType.Damaging)
				{
					if (i < 7)
					{
						affected.health -= Mathf.Max(0, gEEffect.effects[i] * _a.stack - affected.defensiveAttributes.effects[i]);
						if (affected.health <= 0f)
						{
							return i;
						}
					}
					else
					{
						affected.defensiveAttributes.effects[i] -= Mathf.Max(0, gEEffect.effects[i] * _a.stack - affected.defensiveAttributes.effects[i]);
					}
				}
				else if (_a.affectionType == GEAffectionType.Healing)
				{
					if (i >= 7)
					{
						return -1;
					}
					affected.health = Mathf.Min(affected.maxHealth, affected.health + (float)(gEEffect.effects[i] * _a.stack));
				}
				else if (_a.affectionType == GEAffectionType.Defensive)
				{
					affected.defensiveAttributes.effects[i] += gEEffect.effects[i] * _a.stack;
				}
			}
		}
		return -1;
	}

	public static bool UpdateAffection(GEAffectionC _c)
	{
		if (_c.affected.health <= 0f)
		{
			return true;
		}
		if (!_c.hasBegan)
		{
			_c.hasBegan = true;
			_c.lastTick = Main.m_gameTime;
			int num = ApplyAffection(_c, 0);
			if (num > -1)
			{
				ApplyAffection(_c, 2);
				KillCreature(_c, num);
				return true;
			}
		}
		if (_c.began + _c.duration > Main.m_gameTime)
		{
			if (_c.lastTick + _c.tickInterval < Main.m_gameTime)
			{
				int num2 = ApplyAffection(_c, 1);
				if (num2 > -1)
				{
					ApplyAffection(_c, 2);
					KillCreature(_c, num2);
					return true;
				}
				_c.lastTick += _c.tickInterval;
			}
		}
		else
		{
			int num3 = ApplyAffection(_c, 2);
			if (num3 <= -1)
			{
				return true;
			}
			KillCreature(_c, num3);
		}
		return false;
	}
}

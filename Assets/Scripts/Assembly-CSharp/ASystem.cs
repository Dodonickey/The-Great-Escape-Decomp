using System;
using UnityEngine;

public static class ASystem
{
	public static GenericArray<AShipC> m_shipComponents;

	public static GenericArray<AEmitterC> m_emitterComponents;

	public static GenericArray<ABulletC> m_bulletComponents;

	public static GenericArray<AAsteroidC> m_asteroidComponents;

	private static int shipCount = 20;

	private static int emitterCount = 10;

	private static int bulletCount = 25;

	private static int asteroidCount = 100;

	private static int curFrame;

	private static int frameWait;

	private static int shipInvisilibty;

	private static float scale;

	private static int firingTimer;

	private static int shhp;

	private static bool continuousCollider;

	public static void Initialize()
	{
		m_shipComponents = new GenericArray<AShipC>(shipCount);
		for (int i = 0; i < shipCount; i++)
		{
			m_shipComponents.m_array[i] = new AShipC();
			m_shipComponents.m_array[i].index = i;
			m_shipComponents.m_array[i].componentType = (ComponentType)30;
		}
		m_emitterComponents = new GenericArray<AEmitterC>(emitterCount);
		for (int j = 0; j < emitterCount; j++)
		{
			m_emitterComponents.m_array[j] = new AEmitterC();
			m_emitterComponents.m_array[j].index = j;
			m_emitterComponents.m_array[j].componentType = (ComponentType)33;
		}
		m_bulletComponents = new GenericArray<ABulletC>(bulletCount);
		for (int k = 0; k < bulletCount; k++)
		{
			m_bulletComponents.m_array[k] = new ABulletC();
			m_bulletComponents.m_array[k].index = k;
			m_bulletComponents.m_array[k].componentType = (ComponentType)32;
		}
		m_asteroidComponents = new GenericArray<AAsteroidC>(asteroidCount);
		for (int l = 0; l < asteroidCount; l++)
		{
			m_asteroidComponents.m_array[l] = new AAsteroidC();
			m_asteroidComponents.m_array[l].index = l;
			m_asteroidComponents.m_array[l].componentType = (ComponentType)31;
		}
		ChipmunkS.AddCollisionInterest(true, false, false, (ColliderType)15, (ColliderType)16, HandleASTEROIDtoBULLETCollisions);
		curFrame = 0;
		scale = 1f;
	}

	private static void HandleASTEROIDtoBULLETCollisions(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		if (GEState.editorMode)
		{
			return;
		}
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC != null && chipmunkC2 != null)
		{
			if (chipmunkC.colliderType != (ColliderType)15)
			{
				chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
				chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
			}
			Vector2 worldPos = ChipmunkWrapper.GetWorldPos(chipmunkC.cpBodyPtr, Vector2.zero);
			EntityManager.RemoveEntity(chipmunkC.entityIndex, false);
			EntityManager.RemoveEntity(chipmunkC2.entityIndex, false);
			AAsteroidC aAsteroidC = chipmunkC.customComponent as AAsteroidC;
			splitAsteroid(worldPos, aAsteroidC.size / 2);
		}
	}

	public static void splitAsteroid(Vector2 _pos, float size)
	{
		if (size <= 5f)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			switch (i)
			{
			case 0:
			case 1:
				_pos.x += 30f;
				break;
			case 2:
			case 3:
				_pos.x -= 30f;
				break;
			}
			switch (i)
			{
			case 0:
			case 2:
				_pos.y += 30f;
				break;
			case 1:
			case 3:
				_pos.y -= 30f;
				break;
			}
			AAsteroidC aAsteroidC = AAsteroidA.Assemble(_pos, (int)size, 20);
			Vector2 j = new Vector2(-200f, -200f);
			if (i == 1 || i == 3)
			{
				j.x = 200f;
			}
			if (i == 3 || i == 2)
			{
				j.y = 200f;
			}
			ChipmunkWrapper.ApplyImpulse(aAsteroidC.CMC.cpBodyPtr, j, Vector2.zero, true);
		}
	}

	public static AAsteroidC AddAsteroidComponent(Entity _e, ChipmunkC _CMC)
	{
		int num = m_asteroidComponents.AddItem();
		AAsteroidC aAsteroidC = m_asteroidComponents.m_array[num];
		aAsteroidC.active = true;
		aAsteroidC.entityIndex = _e.index;
		aAsteroidC.CMC = _CMC;
		aAsteroidC.size = 0;
		_e.components.Add(aAsteroidC);
		return aAsteroidC;
	}

	public static AShipC AddShipComponent(Entity _e, ChipmunkC _cmc, ShipData _data)
	{
		int num = m_shipComponents.AddItem();
		AShipC aShipC = m_shipComponents.m_array[num];
		aShipC.active = true;
		aShipC.entityIndex = _e.index;
		aShipC.data = _data;
		aShipC.playerState = GameState.m_playerStates[_data.plrIdx];
		aShipC.plrIdx = _data.plrIdx;
		aShipC.CMC = _cmc;
		aShipC.angle = 0f;
		aShipC.def = new GEControlledValue();
		aShipC.input = new GEControlledValue();
		aShipC.modifier = new GEControlledValue();
		aShipC.output = new GEControlledValue();
		aShipC.update = true;
		aShipC.camera = Main.camera;
		aShipC.update = true;
		aShipC.inputSlots = new ConnectionSlot[0];
		aShipC.outputSlots = new ConnectionSlot[0];
		aShipC.modifierSlots = new ConnectionSlot[0];
		aShipC.lastGain = 0f;
		aShipC.lastConsume = 0f;
		aShipC.lastReload = 0f;
		_e.components.Add(aShipC);
		return aShipC;
	}

	public static void RemoveAsteroidComponent(IComponent _c)
	{
		AAsteroidC aAsteroidC = _c as AAsteroidC;
		aAsteroidC.active = false;
		aAsteroidC.CMC = null;
		aAsteroidC.PC = null;
		aAsteroidC.TC = null;
		aAsteroidC.size = 0;
		m_asteroidComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[aAsteroidC.entityIndex].components.Remove(_c);
	}

	public static void RemoveShipComponent(IComponent _c)
	{
		AShipC aShipC = _c as AShipC;
		aShipC.active = false;
		aShipC.CMC = null;
		aShipC.angle = 0f;
		m_shipComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[aShipC.entityIndex].components.Remove(_c);
	}

	public static AEmitterC AddEmitterComponent(Entity _e, BasicLevelData _data)
	{
		int num = m_emitterComponents.AddItem();
		AEmitterC aEmitterC = m_emitterComponents.m_array[num];
		aEmitterC.active = true;
		aEmitterC.entityIndex = _e.index;
		aEmitterC.data = _data;
		_e.components.Add(aEmitterC);
		return aEmitterC;
	}

	public static void RemoveEmitterComponent(IComponent _c)
	{
		AEmitterC aEmitterC = _c as AEmitterC;
		aEmitterC.active = false;
		aEmitterC.asteroids = null;
		aEmitterC.asteroidSpeed = 0;
		aEmitterC.numAsteroids = 0;
		m_emitterComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[aEmitterC.entityIndex].components.Remove(_c);
	}

	public static ABulletC AddBulletComponent(Entity _e)
	{
		int num = m_bulletComponents.AddItem();
		ABulletC aBulletC = m_bulletComponents.m_array[num];
		aBulletC.active = true;
		aBulletC.entityIndex = _e.index;
		_e.components.Add(aBulletC);
		return aBulletC;
	}

	public static void RemoveBulletComponent(IComponent _c)
	{
		ABulletC aBulletC = _c as ABulletC;
		aBulletC.active = false;
		aBulletC.CMC = null;
		aBulletC.prefab = null;
		aBulletC.TC = null;
		aBulletC.timer = 0;
		m_bulletComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[aBulletC.entityIndex].components.Remove(_c);
	}

	public static IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		int aliveCount = m_shipComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			AShipC aShipC = m_shipComponents.m_array[m_shipComponents.m_aliveIndices[i]];
			if (aShipC.data.id == _id)
			{
				return aShipC;
			}
		}
		return null;
	}

	public static void Update()
	{
		if (GEState.editorMode)
		{
			return;
		}
		int aliveCount = m_shipComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			AShipC aShipC = m_shipComponents.m_array[m_shipComponents.m_aliveIndices[i]];
			if (!aShipC.active)
			{
				continue;
			}
			ShipData data = aShipC.data;
			if (shipInvisilibty > 0)
			{
				if (shipInvisilibty % 10 == 0 && shipInvisilibty <= 120)
				{
					if (scale == 1f)
					{
						scale = 0f;
					}
					else
					{
						scale = 1f;
					}
					TransformS.SetScale(aShipC.TC, scale);
				}
				shipInvisilibty--;
			}
			else if (scale == 0f)
			{
				TransformS.SetScale(aShipC.TC, 1f);
				scale = 1f;
			}
			if (ChipmunkS.IsBodyColliding(aShipC.CMC.cpBodyPtr) && shipInvisilibty == 0 && !continuousCollider)
			{
				continuousCollider = true;
				shhp++;
				if (aShipC.data.health == shhp)
				{
					Vector2 worldPos = ChipmunkWrapper.GetWorldPos(aShipC.CMC.cpBodyPtr, Vector2.zero);
					ChipmunkWrapper.ResetForces(aShipC.CMC.cpBodyPtr);
					ChipmunkWrapper.SetVelocity(aShipC.CMC.cpBodyPtr, Vector2.zero);
					TransformS.SetScale(aShipC.TC, 0f);
					shhp = 0;
					shipInvisilibty = 150;
					GameObject obj = UnityEngine.Object.Instantiate(position: new Vector3(worldPos.x, worldPos.y, 40f), original: ResourceManager.GetGameObject("Explosion"), rotation: Quaternion.identity) as GameObject;
					UnityEngine.Object.Destroy(obj, 2f);
				}
			}
			else if (shipInvisilibty == 0 && scale == 1f)
			{
				continuousCollider = false;
			}
			ControllerState contollerState = aShipC.playerState.m_contollerState;
			IControlledComponent controlledComponent = contollerState.components[0];
			IControlledComponent controlledComponent2 = contollerState.components[8];
			IControlledComponent controlledComponent3 = contollerState.components[9];
			int num = 0;
			if (aShipC.plrIdx == 0)
			{
				if (controlledComponent3 != null)
				{
					Debug.Log(controlledComponent3.output.vector.x / 500f + " <> " + controlledComponent3.output.vector.y / 500f);
					Vector2 vector = new Vector2(controlledComponent3.output.vector.y, 0f - controlledComponent3.output.vector.x);
					if (vector.sqrMagnitude > 1f)
					{
						vector.Normalize();
					}
					ChipmunkWrapper.SetVelocity(aShipC.CMC.cpBodyPtr, vector * 500f);
				}
				if (controlledComponent != null)
				{
					aShipC.acceleration = controlledComponent.output.vector;
					aShipC.angle = Mathf.Atan2(aShipC.acceleration.y, aShipC.acceleration.x) - (float)Math.PI / 2f;
					num = ((aShipC.acceleration != Vector2.zero) ? 1 : 0);
				}
				if (controlledComponent2 != null)
				{
					if (controlledComponent2.triggered)
					{
						if (firingTimer <= 0 && m_bulletComponents.m_aliveCount < bulletCount)
						{
							firingTimer = data.firingDelay + 1;
							ABulletA.Assemble(aShipC);
						}
						EventS.Dispatch("ShipEvent", null, null, false);
						firingTimer--;
					}
					else if (controlledComponent2.end)
					{
						firingTimer = 0;
					}
				}
			}
			if (num == 1)
			{
				aShipC.prefab.p_gameObject.animation.Stop();
				frameWait++;
				if (frameWait == data.frameSpeed)
				{
					frameWait = 0;
				}
				aShipC.prefab.p_gameObject.animation.Play("flames");
				aShipC.prefab.p_gameObject.animation["flames"].speed = 0f;
				float frameRate = aShipC.prefab.p_gameObject.animation.GetClip("flames").frameRate;
				aShipC.prefab.p_gameObject.animation["flames"].time = (float)curFrame / frameRate;
				if (frameWait == 0)
				{
					curFrame++;
				}
				if (curFrame > 4)
				{
					curFrame = 1;
				}
			}
			else
			{
				aShipC.prefab.p_gameObject.animation.Stop();
				aShipC.prefab.p_gameObject.animation.Play("idle");
				aShipC.prefab.p_gameObject.animation["idle"].speed = 0f;
				float frameRate2 = aShipC.prefab.p_gameObject.animation.GetClip("idle").frameRate;
				aShipC.prefab.p_gameObject.animation["idle"].time = 1f;
			}
			if (aShipC.CMC != null)
			{
				if (aShipC.acceleration != Vector2.zero)
				{
					ChipmunkWrapper.SetAngularVelocity(aShipC.CMC.cpBodyPtr, 0f);
					ChipmunkWrapper.ApplyImpulse(aShipC.CMC.cpBodyPtr, aShipC.acceleration * (aShipC.data.accSpeed / 6), Vector2.zero, true);
					ChipmunkWrapper.SetAngle(aShipC.CMC.cpBodyPtr, aShipC.angle);
				}
				Vector2 worldPos2 = ChipmunkWrapper.GetWorldPos(aShipC.CMC.cpBodyPtr, Vector2.zero);
				ChipmunkWrapper.SetPosition(aShipC.CMC.cpBodyPtr, worldPos2);
			}
		}
		aliveCount = m_asteroidComponents.m_aliveCount;
		for (int j = 0; j < aliveCount; j++)
		{
			AAsteroidC aAsteroidC = m_asteroidComponents.m_array[m_asteroidComponents.m_aliveIndices[j]];
			Vector2 worldPos3 = ChipmunkWrapper.GetWorldPos(aAsteroidC.CMC.cpBodyPtr, Vector2.zero);
			ChipmunkWrapper.SetPosition(aAsteroidC.CMC.cpBodyPtr, worldPos3);
		}
		aliveCount = m_bulletComponents.m_aliveCount;
		for (int k = 0; k < aliveCount; k++)
		{
			ABulletC aBulletC = m_bulletComponents.m_array[m_bulletComponents.m_aliveIndices[k]];
			if (aBulletC.active)
			{
				aBulletC.timer--;
				if (aBulletC.timer % 2 == 0)
				{
					aBulletC.curFrame++;
				}
				aBulletC.curFrame %= 3;
				SpriteS.SetFrame(AState.tss, aBulletC.sprite, new Frame(32 * aBulletC.curFrame, 0f, 32f, 32f));
				if (aBulletC.timer == 0 || ChipmunkS.IsBodyColliding(aBulletC.CMC.cpBodyPtr))
				{
					EntityManager.RemoveEntity(aBulletC.entityIndex, false);
				}
			}
		}
	}

	public static void OnShipEvent(EventC _e)
	{
		switch (_e.properties["event"] as string)
		{
		case "destroy":
			Debug.Log(_e.properties["debug"]);
			break;
		default:
			Debug.Log("I HAVE NO IDEA WHAT WENT WRONG");
			break;
		}
	}
}

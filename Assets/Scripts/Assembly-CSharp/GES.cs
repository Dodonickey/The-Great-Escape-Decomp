using System;
using System.Collections.Generic;
using UnityEngine;

public static class GES
{
	private static int m_characterCount = 10;

	private static int m_blockCount = 100;

	private static int m_connectionCount = 100;

	private static int m_controlSchemeCount = 4;

	private static int m_constraintCount = 100;

	private static int m_editorItemCount = 500;

	private static int m_explosiveCount = 20;

	private static int m_portalCount = 20;

	private static int m_shapeCount = 20;

	private static int m_transformGizmoCount = 5;

	private static int m_triggerCount = 100;

	private static int m_vehicleCount = 10;

	private static int m_affectionCount = 100;

	private static int m_physicsAffectorCount = 50;

	public static GenericArray<GECharacterC> m_characterComponents;

	public static GenericArray<GEBlockC> m_blockComponents;

	public static GenericArray<GEConnectionC> m_connectionComponents;

	public static GenericArray<GEControlSchemeC> m_controlSchemeComponents;

	public static GenericArray<GEConstraintC> m_constraintComponents;

	public static GenericArray<EIC> m_editorItemComponents;

	public static GenericArray<GEPortalC> m_portalComponents;

	public static GenericArray<GEShapeC> m_shapeComponents;

	public static GenericArray<GETransformGizmoC> m_transformGizmoComponents;

	public static GenericArray<GETriggerC> m_triggerComponents;

	public static GenericArray<GEVehicleC> m_vehicleComponents;

	public static GenericArray<GEAffectionC> m_affectionComponents;

	public static GenericArray<GEPhysicsAffectorC> m_physicsAffectorComponents;

	public static List<int> m_affectionsRemoveList = new List<int>();

	public static uint m_uniqueId = 0u;

	private static Vector3 m_lastCameraPos = Vector3.zero;

	private static ChipmunkQueryInfo[] m_blockShapeQuery = new ChipmunkQueryInfo[100];

	public static void Initialize()
	{
		m_characterComponents = new GenericArray<GECharacterC>(m_characterCount);
		m_blockComponents = new GenericArray<GEBlockC>(m_blockCount);
		m_connectionComponents = new GenericArray<GEConnectionC>(m_connectionCount);
		m_controlSchemeComponents = new GenericArray<GEControlSchemeC>(m_controlSchemeCount);
		m_constraintComponents = new GenericArray<GEConstraintC>(m_constraintCount);
		m_editorItemComponents = new GenericArray<EIC>(m_editorItemCount);
		m_portalComponents = new GenericArray<GEPortalC>(m_portalCount);
		m_shapeComponents = new GenericArray<GEShapeC>(m_shapeCount);
		m_transformGizmoComponents = new GenericArray<GETransformGizmoC>(m_transformGizmoCount);
		m_triggerComponents = new GenericArray<GETriggerC>(m_triggerCount);
		m_vehicleComponents = new GenericArray<GEVehicleC>(m_vehicleCount);
		m_affectionComponents = new GenericArray<GEAffectionC>(m_affectionCount);
		m_physicsAffectorComponents = new GenericArray<GEPhysicsAffectorC>(m_physicsAffectorCount);
		for (int i = 0; i < m_characterCount; i++)
		{
			m_characterComponents.m_array[i] = new GECharacterC();
			m_characterComponents.m_array[i].entityIndex = -1;
			m_characterComponents.m_array[i].index = i;
			m_characterComponents.m_array[i].componentType = (ComponentType)102;
			m_characterComponents.m_array[i].affections = new List<GEAffectionC>();
		}
		for (int j = 0; j < m_blockCount; j++)
		{
			m_blockComponents.m_array[j] = new GEBlockC();
			m_blockComponents.m_array[j].entityIndex = -1;
			m_blockComponents.m_array[j].index = j;
			m_blockComponents.m_array[j].componentType = (ComponentType)100;
		}
		for (int k = 0; k < m_editorItemCount; k++)
		{
			m_editorItemComponents.m_array[k] = new EIC();
			m_editorItemComponents.m_array[k].entityIndex = -1;
			m_editorItemComponents.m_array[k].index = k;
			m_editorItemComponents.m_array[k].componentType = (ComponentType)106;
		}
		for (int l = 0; l < m_connectionCount; l++)
		{
			m_connectionComponents.m_array[l] = new GEConnectionC();
			m_connectionComponents.m_array[l].entityIndex = -1;
			m_connectionComponents.m_array[l].index = l;
			m_connectionComponents.m_array[l].componentType = (ComponentType)103;
		}
		for (int m = 0; m < m_controlSchemeCount; m++)
		{
			m_controlSchemeComponents.m_array[m] = new GEControlSchemeC();
			m_controlSchemeComponents.m_array[m].entityIndex = -1;
			m_controlSchemeComponents.m_array[m].index = m;
			m_controlSchemeComponents.m_array[m].componentType = (ComponentType)104;
		}
		for (int n = 0; n < m_constraintCount; n++)
		{
			m_constraintComponents.m_array[n] = new GEConstraintC();
			m_constraintComponents.m_array[n].entityIndex = -1;
			m_constraintComponents.m_array[n].index = n;
			m_constraintComponents.m_array[n].componentType = (ComponentType)105;
		}
		for (int num = 0; num < m_portalCount; num++)
		{
			m_portalComponents.m_array[num] = new GEPortalC();
			m_portalComponents.m_array[num].entityIndex = -1;
			m_portalComponents.m_array[num].index = num;
			m_portalComponents.m_array[num].componentType = (ComponentType)108;
		}
		for (int num2 = 0; num2 < m_shapeCount; num2++)
		{
			m_shapeComponents.m_array[num2] = new GEShapeC();
			m_shapeComponents.m_array[num2].entityIndex = -1;
			m_shapeComponents.m_array[num2].index = num2;
			m_shapeComponents.m_array[num2].componentType = (ComponentType)110;
		}
		for (int num3 = 0; num3 < m_triggerCount; num3++)
		{
			m_triggerComponents.m_array[num3] = new GETriggerC();
			m_triggerComponents.m_array[num3].entityIndex = -1;
			m_triggerComponents.m_array[num3].index = num3;
			m_triggerComponents.m_array[num3].componentType = (ComponentType)112;
		}
		for (int num4 = 0; num4 < m_transformGizmoCount; num4++)
		{
			m_transformGizmoComponents.m_array[num4] = new GETransformGizmoC();
			m_transformGizmoComponents.m_array[num4].entityIndex = -1;
			m_transformGizmoComponents.m_array[num4].index = num4;
			m_transformGizmoComponents.m_array[num4].componentType = (ComponentType)111;
		}
		for (int num5 = 0; num5 < m_vehicleCount; num5++)
		{
			m_vehicleComponents.m_array[num5] = new GEVehicleC();
			m_vehicleComponents.m_array[num5].entityIndex = -1;
			m_vehicleComponents.m_array[num5].index = num5;
			m_vehicleComponents.m_array[num5].componentType = (ComponentType)113;
			m_vehicleComponents.m_array[num5].affections = new List<GEAffectionC>();
		}
		for (int num6 = 0; num6 < m_affectionCount; num6++)
		{
			m_affectionComponents.m_array[num6] = new GEAffectionC();
			m_affectionComponents.m_array[num6].entityIndex = -1;
			m_affectionComponents.m_array[num6].index = num6;
			m_affectionComponents.m_array[num6].componentType = (ComponentType)114;
		}
		for (int num7 = 0; num7 < m_physicsAffectorCount; num7++)
		{
			m_physicsAffectorComponents.m_array[num7] = new GEPhysicsAffectorC();
			m_physicsAffectorComponents.m_array[num7].entityIndex = -1;
			m_physicsAffectorComponents.m_array[num7].index = num7;
			m_physicsAffectorComponents.m_array[num7].componentType = (ComponentType)117;
			m_physicsAffectorComponents.m_array[num7].cmcs = new List<ChipmunkC>();
		}
		ChipmunkS.AddCollisionInterest(true, false, true, (ColliderType)11, ColliderType.Any, HandleTRIGGERCollisions);
		ChipmunkS.AddCollisionInterest(true, true, true, (ColliderType)10, (ColliderType)9, HandleTIRECollisions);
		ChipmunkS.AddCollisionInterest(true, false, false, (ColliderType)12, (ColliderType)9, HandleVEHICLECollisions);
		ChipmunkS.AddCollisionInterest(true, false, false, (ColliderType)3, (ColliderType)9, HandleCHARACTERCollisions);
		ChipmunkS.AddCollisionInterest(true, true, true, (ColliderType)6, ColliderType.Any, GEPortalA.HandlePORTALCollisions);
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			gEPlugin.Initialize();
		}
	}

	private static void HandleTRIGGERCollisions(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC.colliderType != (ColliderType)11)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		GETriggerC gETriggerC = chipmunkC.customComponent as GETriggerC;
		bool flag = false;
		int count = gETriggerC.listenedColliderTypes.Count;
		for (int i = 0; i < count; i++)
		{
			if (gETriggerC.listenedColliderTypes[i] == chipmunkC2.colliderType)
			{
				flag = true;
				break;
			}
		}
		if ((gETriggerC.listenedColliderTypes.Count == 0 || flag) && gETriggerC.collisionHandler != null && chipmunkC2 != gETriggerC.connectedCMC)
		{
			gETriggerC.collisionHandler(gETriggerC, chipmunkC2, _collisionPair, _collisionList);
		}
	}

	private static void HandleFINGERCONTROLLEDCollisions(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC.colliderType != (ColliderType)2)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		GETriggerC gETriggerC = chipmunkC.customComponent as GETriggerC;
		gETriggerC.fingerCollisions.Add(_collisionPair);
	}

	private static void HandleTIRECollisions(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC item = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC.colliderType != (ColliderType)10)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			item = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		GEVehicleC gEVehicleC = chipmunkC.customComponent as GEVehicleC;
		switch (_collisionList)
		{
		case ChipmunkCollisionList.BEGIN:
		{
			gEVehicleC.touchingColliders.Add(item);
			float rot = Mathf.Atan2(_collisionPair.normal.y, _collisionPair.normal.x) * 57.29578f + 90f;
			if (_collisionPair.impulse.sqrMagnitude > 1000000f)
			{
			}
			for (int j = 0; j < gEVehicleC.tires.Length; j++)
			{
				if (gEVehicleC.tires[j].CMC != chipmunkC)
				{
					continue;
				}
				if (gEVehicleC.tireEffects[j] == null && _collisionPair.impulse.sqrMagnitude > 1000000f && gEVehicleC.SPC.rootNode.CMC.ucpBodyStruct.v.sqrMagnitude > 40000f)
				{
					Entity entity = EntityManager.AddEntity("effect");
					TransformC transformC = TransformS.AddComponent(entity);
					transformC.forceRotation = true;
					TransformS.ParentComponent(transformC, gEVehicleC.tires[j].TC, Vector3.zero);
					Frame frame = new Frame(0f, 0f, 16f, 16f);
					bool flipX = true;
					if (gEVehicleC.SPC.rootNode.CMC.ucpBodyStruct.v.x < 0f)
					{
						flipX = false;
					}
					frame.flipX = flipX;
					SpriteC s = SpriteS.AddComponent(transformC, frame, FarmState.effectSheet);
					SpriteS.SetOffset(s, _collisionPair.pos - (Vector2)gEVehicleC.tires[j].TC.transform.position, rot);
					GameObject gameObject = UnityEngine.Object.Instantiate(FarmState.tireSparks) as GameObject;
					gameObject.transform.parent = gEVehicleC.tires[j].TC.transform;
					gameObject.transform.localPosition = Vector3.zero;
					UnityEngine.Object.Destroy(gameObject, 0.5f);
					gEVehicleC.tireEffects[j] = entity;
					gEVehicleC.tireEffectTimes[j] = Main.m_gameTime;
				}
				break;
			}
			break;
		}
		case ChipmunkCollisionList.PERSIST:
		{
			float rot2 = Mathf.Atan2(_collisionPair.normal.y, _collisionPair.normal.x) * 57.29578f + 90f;
			if (_collisionPair.impulse.sqrMagnitude > 250000f)
			{
			}
			for (int k = 0; k < gEVehicleC.tires.Length; k++)
			{
				if (gEVehicleC.tires[k].CMC != chipmunkC)
				{
					continue;
				}
				if (gEVehicleC.tireEffects[k] == null && _collisionPair.impulse.sqrMagnitude > 1000000f && gEVehicleC.SPC.rootNode.CMC.ucpBodyStruct.v.sqrMagnitude > 40000f)
				{
					Entity entity2 = EntityManager.AddEntity("effect");
					TransformC transformC2 = TransformS.AddComponent(entity2);
					transformC2.forceRotation = true;
					TransformS.ParentComponent(transformC2, gEVehicleC.tires[k].TC, Vector3.zero);
					Frame frame2 = new Frame(0f, 0f, 16f, 16f);
					bool flipX2 = true;
					if (gEVehicleC.SPC.rootNode.CMC.ucpBodyStruct.v.x < 0f)
					{
						flipX2 = false;
					}
					frame2.flipX = flipX2;
					SpriteC s2 = SpriteS.AddComponent(transformC2, frame2, FarmState.effectSheet);
					SpriteS.SetOffset(s2, _collisionPair.pos - (Vector2)gEVehicleC.tires[k].TC.transform.position, rot2);
					GameObject gameObject2 = UnityEngine.Object.Instantiate(FarmState.tireSparks) as GameObject;
					gameObject2.transform.parent = gEVehicleC.tires[k].TC.transform;
					gameObject2.transform.localPosition = Vector3.zero;
					UnityEngine.Object.Destroy(gameObject2, 0.5f);
					gEVehicleC.tireEffects[k] = entity2;
					gEVehicleC.tireEffectTimes[k] = Main.m_gameTime;
				}
				break;
			}
			break;
		}
		case ChipmunkCollisionList.SEPARATE:
		{
			gEVehicleC.touchingColliders.Remove(item);
			for (int i = 0; i < gEVehicleC.tires.Length; i++)
			{
				if (gEVehicleC.tires[i].CMC == chipmunkC)
				{
					if (gEVehicleC.tireEffects[i] != null)
					{
						EntityManager.RemoveEntity(gEVehicleC.tireEffects[i]);
						gEVehicleC.tireEffects[i] = null;
						gEVehicleC.tireEffectTimes[i] = 0f;
					}
					break;
				}
			}
			break;
		}
		}
		if (gEVehicleC.vehicleType == VehicleType.Runner)
		{
			Vector2 vector = _collisionPair.pos - chipmunkC.ucpBodyStruct.p;
			Vector2 vector2 = new Vector2(chipmunkC.ucpBodyStruct.rot.y, 0f - chipmunkC.ucpBodyStruct.rot.x);
			if (Vector2.Dot(vector.normalized, vector2) < 0.7f)
			{
				_collisionPair.normal = vector2;
			}
		}
		gEVehicleC.contactNormal += _collisionPair.normal;
	}

	private static void HandleVEHICLECollisions(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC.colliderType != (ColliderType)12)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		GEVehicleC gEVehicleC = chipmunkC.customComponent as GEVehicleC;
		if (chipmunkC2.colliderType != (ColliderType)9)
		{
			return;
		}
		int num = (int)(_collisionPair.impulse.magnitude * 0.1f);
		if (gEVehicleC.vehicleType == VehicleType.Runner)
		{
			float x = Mathf.Cos(gEVehicleC.rootNode.CMC.ucpBodyStruct.a + (float)Math.PI / 2f);
			float y = Mathf.Sin(gEVehicleC.rootNode.CMC.ucpBodyStruct.a + (float)Math.PI / 2f);
			Vector2 lhs = new Vector2(x, y);
			float num2 = Vector2.Dot(lhs, -_collisionPair.normal);
			float num3 = 1f;
			if (num2 > 0.6f)
			{
				num3 = 0.25f;
			}
			for (int i = 0; i < gEVehicleC.characters.Count; i++)
			{
				GECreatureLogic.AddInstantAffection(chipmunkC2.customComponent, gEVehicleC.characters[i], GEEffectType.Physical, Mathf.RoundToInt((float)num * num3));
			}
			return;
		}
		bool flag = gEVehicleC.health > 0f;
		float x2 = Mathf.Cos(gEVehicleC.rootNode.CMC.ucpBodyStruct.a + (float)Math.PI / 2f);
		float y2 = Mathf.Sin(gEVehicleC.rootNode.CMC.ucpBodyStruct.a + (float)Math.PI / 2f);
		Vector2 lhs2 = new Vector2(x2, y2);
		float num4 = Vector2.Dot(lhs2, -_collisionPair.normal);
		float num5 = 1f;
		if (num4 > 0.6f)
		{
			num5 = 0.25f;
		}
		GECreatureLogic.AddInstantAffection(chipmunkC2.customComponent, gEVehicleC, GEEffectType.Physical, Mathf.RoundToInt((float)num * num5));
		if (flag && gEVehicleC.health <= 0f)
		{
			FImpactStarA.Assemble(new Vector3(_collisionPair.pos.x, _collisionPair.pos.y, gEVehicleC.SPC.rootNode.TC.transform.position.z) + (Vector3)_collisionPair.normal * -5f, (Vector3.up * 2f + (Vector3)_collisionPair.normal * -1f).normalized, 0, 1, 1f);
			SoundS.PlaySound("SoundCartBreak", gEVehicleC.rootNode.CMC.TC.transform.gameObject);
			gEVehicleC.driveSoundLoop.Stop();
		}
		else if (flag)
		{
			FImpactStarA.Assemble(new Vector3(_collisionPair.pos.x, _collisionPair.pos.y, gEVehicleC.SPC.rootNode.TC.transform.position.z) + (Vector3)_collisionPair.normal * -5f, (Vector3.up * 2f + (Vector3)_collisionPair.normal * -1f).normalized, 3, 1, 1f);
		}
		else if (!flag && _collisionPair.impulse.sqrMagnitude > 62500f)
		{
			float angle = Mathf.Atan2(_collisionPair.normal.y, _collisionPair.normal.x) * 57.29578f + 90f;
			FImpactDustA.Assemble(new Vector3(_collisionPair.pos.x, _collisionPair.pos.y, gEVehicleC.SPC.rootNode.TC.transform.position.z) + (Vector3)_collisionPair.normal * -5f, angle, 2, 0.5f);
		}
	}

	private static void HandleCHARACTERCollisions(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		if (chipmunkC.colliderType != (ColliderType)3)
		{
			chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
			chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		}
		GECharacterC gECharacterC = chipmunkC.customComponent as GECharacterC;
		if (chipmunkC2.colliderType != (ColliderType)9)
		{
			return;
		}
		if (gECharacterC.vehicle != null)
		{
			if (gECharacterC.vehicle.vehicleType != VehicleType.Runner)
			{
				bool flag = gECharacterC.health > 0f;
				int amount = (int)(_collisionPair.impulse.magnitude * 0.1f);
				GECreatureLogic.AddInstantAffection(chipmunkC2.customComponent, gECharacterC, GEEffectType.Physical, amount);
				if (flag && gECharacterC.health <= 0f)
				{
					FImpactStarA.Assemble(new Vector3(_collisionPair.pos.x, _collisionPair.pos.y, gECharacterC.SPC.rootNode.TC.transform.position.z) + (Vector3)_collisionPair.normal * -5f, (Vector3.up * 2f + (Vector3)_collisionPair.normal * -1f).normalized, 0, 1, 1f);
				}
				else if (flag)
				{
					FImpactStarA.Assemble(new Vector3(_collisionPair.pos.x, _collisionPair.pos.y, gECharacterC.SPC.rootNode.TC.transform.position.z) + (Vector3)_collisionPair.normal * -5f, (Vector3.up * 2f + (Vector3)_collisionPair.normal * -1f).normalized, 3, 1, 1f);
				}
			}
		}
		else if (_collisionPair.impulse.sqrMagnitude > 250000f)
		{
			float angle = Mathf.Atan2(_collisionPair.normal.y, _collisionPair.normal.x) * 57.29578f + 90f;
			FImpactDustA.Assemble(new Vector3(_collisionPair.pos.x, _collisionPair.pos.y, gECharacterC.SPC.rootNode.TC.transform.position.z) + (Vector3)_collisionPair.normal * -5f, angle, 3, 0.25f);
		}
	}

	public static uint GetUniqueId()
	{
		m_uniqueId++;
		return m_uniqueId;
	}

	public static GEPhysicsAffectorC AddPhysicsAffectorComponent(TransformC _tc, PhysicsAffectorData _data)
	{
		int num = m_physicsAffectorComponents.AddItem();
		GEPhysicsAffectorC gEPhysicsAffectorC = m_physicsAffectorComponents.m_array[num];
		gEPhysicsAffectorC.active = true;
		gEPhysicsAffectorC.entityIndex = _tc.entityIndex;
		gEPhysicsAffectorC.amount = _data.amount;
		gEPhysicsAffectorC.direction = _data.direction.ToVector2();
		if (_data.duration > 0f)
		{
			gEPhysicsAffectorC.affectUntil = Main.m_gameTime + _data.duration;
		}
		else if (_data.duration == 0f)
		{
			gEPhysicsAffectorC.affectUntil = 0f;
		}
		else
		{
			gEPhysicsAffectorC.affectUntil = -1f;
		}
		gEPhysicsAffectorC.isAngularVelocity = _data.isAngularVelocity;
		gEPhysicsAffectorC.isForce = _data.isForce;
		gEPhysicsAffectorC.isImpulse = _data.isImpulse;
		gEPhysicsAffectorC.isVelocity = _data.isVelocity;
		gEPhysicsAffectorC.point = _data.point.ToVector2();
		gEPhysicsAffectorC.relative = _data.relative;
		EntityManager.m_entities.m_array[gEPhysicsAffectorC.entityIndex].components.Add(gEPhysicsAffectorC);
		return gEPhysicsAffectorC;
	}

	public static void RemovePhysicsAffectorComponent(IComponent _c)
	{
		GEPhysicsAffectorC gEPhysicsAffectorC = _c as GEPhysicsAffectorC;
		gEPhysicsAffectorC.active = false;
		gEPhysicsAffectorC.cmcs.Clear();
		m_physicsAffectorComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[gEPhysicsAffectorC.entityIndex].components.Remove(_c);
	}

	public static GEPortalC AddPortalComponent(ChipmunkC _cmc, bool _automatic)
	{
		int num = m_portalComponents.AddItem();
		GEPortalC gEPortalC = m_portalComponents.m_array[num];
		gEPortalC.active = true;
		gEPortalC.entityIndex = _cmc.entityIndex;
		gEPortalC.CMC = _cmc;
		gEPortalC.automatic = _automatic;
		gEPortalC.pair = null;
		gEPortalC.reserved = false;
		EntityManager.m_entities.m_array[gEPortalC.entityIndex].components.Add(gEPortalC);
		return gEPortalC;
	}

	public static void RemovePortalComponent(IComponent _c)
	{
		GEPortalC gEPortalC = _c as GEPortalC;
		gEPortalC.active = false;
		gEPortalC.CMC = null;
		gEPortalC.pair = null;
		gEPortalC.usingEntityIndex = -1;
		m_portalComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[gEPortalC.entityIndex].components.Remove(_c);
	}

	public static GEControlSchemeC AddControlSchemeComponent(Camera _camera, TransformC _tc, uint _id, PlayerState _playerState)
	{
		int num = m_controlSchemeComponents.AddItem();
		GEControlSchemeC gEControlSchemeC = m_controlSchemeComponents.m_array[num];
		gEControlSchemeC.active = true;
		gEControlSchemeC.entityIndex = _tc.entityIndex;
		gEControlSchemeC.id = _id;
		gEControlSchemeC.TC = _tc;
		gEControlSchemeC.camera = _camera;
		gEControlSchemeC.playerState = _playerState;
		gEControlSchemeC.def = new GEControlledValue();
		gEControlSchemeC.input = new GEControlledValue();
		gEControlSchemeC.modifier = new GEControlledValue();
		gEControlSchemeC.output = new GEControlledValue();
		gEControlSchemeC.inputSlots = new ConnectionSlot[0];
		gEControlSchemeC.outputSlots = new ConnectionSlot[0];
		gEControlSchemeC.modifierSlots = new ConnectionSlot[0];
		gEControlSchemeC.beganDelegatedCount = 0;
		EntityManager.m_entities.m_array[gEControlSchemeC.entityIndex].components.Add(gEControlSchemeC);
		return gEControlSchemeC;
	}

	public static void RemoveControlSchemeComponent(GEControlSchemeC _c)
	{
		_c.active = false;
		for (int i = 0; i < _c.playerState.m_contollerState.components.Length; i++)
		{
			_c.playerState.m_contollerState.components[i] = null;
		}
		if (_c.BeganEventDelegate != null)
		{
			Delegate[] invocationList = _c.BeganEventDelegate.GetInvocationList();
			Delegate[] array = invocationList;
			foreach (Delegate obj in array)
			{
				_c.BeganEventDelegate = (TriggerEventDelegate)Delegate.Remove(_c.BeganEventDelegate, (TriggerEventDelegate)obj);
			}
			_c.beganDelegatedCount = 0;
		}
		if (_c.EndEventDelegate != null)
		{
			Delegate[] invocationList2 = _c.EndEventDelegate.GetInvocationList();
			Delegate[] array2 = invocationList2;
			foreach (Delegate obj2 in array2)
			{
				_c.EndEventDelegate = (TriggerEventDelegate)Delegate.Remove(_c.EndEventDelegate, (TriggerEventDelegate)obj2);
			}
			_c.endDelegatedCount = 0;
		}
		GameState.RemovePlayer(_c.playerState);
		_c.playerState = null;
		m_controlSchemeComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static GEVehicleC AddVehicleComponent(GESpritePrefabC _SPC, SpritePrefabNode[] _tires, SpritePrefabNode[] _crawlers, PlayerState _playerState)
	{
		int num = m_vehicleComponents.AddItem();
		GEVehicleC gEVehicleC = m_vehicleComponents.m_array[num];
		gEVehicleC.active = true;
		gEVehicleC.entityIndex = _SPC.entityIndex;
		gEVehicleC.SPC = _SPC;
		gEVehicleC.rootNode = _SPC.rootNode;
		gEVehicleC.tires = _tires;
		gEVehicleC.tireEffects = new Entity[_tires.Length];
		gEVehicleC.tireEffectTimes = new float[_tires.Length];
		gEVehicleC.tireConstraints = new IntPtr[_tires.Length];
		gEVehicleC.crawlers = _crawlers;
		gEVehicleC.contactState = ContactState.OnAir;
		gEVehicleC.flyPower = 1f;
		gEVehicleC.jumpPower = 1f;
		gEVehicleC.currentGroup = gEVehicleC.rootNode.CMC.colliderGroup;
		gEVehicleC.currentLayer = 0u;
		gEVehicleC.characters = new List<GECharacterC>();
		gEVehicleC.seats = new List<SpritePrefabNode>();
		gEVehicleC.seatsTaken = new List<GECreatureC>();
		gEVehicleC.touchingColliders = new List<ChipmunkC>();
		gEVehicleC.currentBalance = 0f;
		gEVehicleC.playerState = _playerState;
		gEVehicleC.driveSoundLoop = null;
		gEVehicleC.braking = false;
		gEVehicleC.characters = new List<GECharacterC>();
		gEVehicleC.defensiveAttributes = new GEEffect();
		EntityManager.m_entities.m_array[gEVehicleC.rootNode.CMC.entityIndex].components.Add(gEVehicleC);
		return gEVehicleC;
	}

	public static void RemoveVehicleComponent(IComponent _c)
	{
		GEVehicleC gEVehicleC = _c as GEVehicleC;
		gEVehicleC.active = false;
		gEVehicleC.rootNode = null;
		gEVehicleC.tireConstraints = null;
		gEVehicleC.tires = null;
		gEVehicleC.crawlers = null;
		gEVehicleC.braking = false;
		gEVehicleC.currentLayer = 0u;
		gEVehicleC.currentGroup = 0u;
		gEVehicleC.playerState = null;
		gEVehicleC.tireEffects = null;
		gEVehicleC.tireEffectTimes = null;
		gEVehicleC.characters = null;
		gEVehicleC.affections.Clear();
		gEVehicleC.seats = null;
		gEVehicleC.seatsTaken = null;
		gEVehicleC.driveSoundLoop = null;
		m_vehicleComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[gEVehicleC.entityIndex].components.Remove(_c);
	}

	public static GECharacterC AddBipedCharacterComponent(GESpritePrefabC _spritePrefabC, SpritePrefabNode[] _eyes, SpritePrefabNode[] _legs, SpritePrefabNode[] _arms, SpritePrefabNode[] _heads, CreatureType _characterType)
	{
		int num = m_characterComponents.AddItem();
		GECharacterC gECharacterC = m_characterComponents.m_array[num];
		gECharacterC.active = true;
		gECharacterC.entityIndex = _spritePrefabC.entityIndex;
		gECharacterC.vehicle = null;
		gECharacterC.rootNode = _spritePrefabC.rootNode;
		gECharacterC.creatureType = _characterType;
		gECharacterC.movementState = MovementState.Idling;
		gECharacterC.SPC = _spritePrefabC;
		gECharacterC.eyes = _eyes;
		gECharacterC.heads = _heads;
		gECharacterC.arms = _arms;
		gECharacterC.legs = _legs;
		gECharacterC.contactState = ContactState.OnAir;
		gECharacterC.armSprings = new IntPtr[_arms.Length];
		gECharacterC.legSprings = new IntPtr[_legs.Length];
		gECharacterC.headSprings = new IntPtr[_heads.Length];
		gECharacterC.balanceAngle = 0f;
		gECharacterC.vehicleRotarySpringPtr = IntPtr.Zero;
		gECharacterC.vehicleConnectionPtr = IntPtr.Zero;
		gECharacterC.vehicleSpringPtr = IntPtr.Zero;
		gECharacterC.hatSPC = null;
		gECharacterC.defensiveAttributes = new GEEffect();
		EntityManager.m_entities.m_array[gECharacterC.entityIndex].components.Add(gECharacterC);
		return gECharacterC;
	}

	public static void RemoveBipedCharacterComponent(IComponent _c)
	{
		GECharacterC gECharacterC = _c as GECharacterC;
		gECharacterC.active = false;
		gECharacterC.rootNode = null;
		gECharacterC.vehicleRotarySpringPtr = IntPtr.Zero;
		gECharacterC.vehicleConnectionPtr = IntPtr.Zero;
		gECharacterC.vehicleSpringPtr = IntPtr.Zero;
		gECharacterC.SPC = null;
		gECharacterC.eyes = null;
		gECharacterC.vehicle = null;
		gECharacterC.affections.Clear();
		m_characterComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[gECharacterC.entityIndex].components.Remove(_c);
	}

	public static GEBlockC AddBlockComponent(Entity _e, ChipmunkC _cmc, ShapeData _shapeData, Polygon _modified, float _area)
	{
		int num = m_blockComponents.AddItem();
		GEBlockC gEBlockC = m_blockComponents.m_array[num];
		gEBlockC.active = true;
		gEBlockC.entityIndex = _e.index;
		gEBlockC.created = Main.m_gameTime;
		gEBlockC.CMC = _cmc;
		gEBlockC.originalShape = _shapeData.polygon;
		gEBlockC.modifiedShape = _modified;
		gEBlockC.area = _area;
		gEBlockC.isBreakable = false;
		gEBlockC.breakingImpulse = 0f;
		gEBlockC.breakEvent = 0;
		gEBlockC.breakEventScale = 1f;
		gEBlockC.CMC.customComponent = gEBlockC;
		_e.components.Add(gEBlockC);
		return gEBlockC;
	}

	public static GEBlockC AddBlockComponent(Entity _e, ChipmunkC _cmc, Polygon _modified, Polygon _original, float _area, bool _isBreakable, float _breakingImpulse, int _breakEvent, float _breakEventScale)
	{
		int num = m_blockComponents.AddItem();
		GEBlockC gEBlockC = m_blockComponents.m_array[num];
		gEBlockC.active = true;
		gEBlockC.entityIndex = _e.index;
		gEBlockC.created = Main.m_gameTime;
		gEBlockC.CMC = _cmc;
		gEBlockC.originalShape = _original;
		gEBlockC.modifiedShape = _modified;
		gEBlockC.area = _area;
		gEBlockC.isBreakable = _isBreakable;
		gEBlockC.breakingImpulse = _breakingImpulse;
		gEBlockC.breakEvent = _breakEvent;
		gEBlockC.breakEventScale = _breakEventScale;
		gEBlockC.CMC.customComponent = gEBlockC;
		_e.components.Add(gEBlockC);
		return gEBlockC;
	}

	public static void RemoveBlockComponent(GEBlockC _c)
	{
		_c.active = false;
		_c.CMC = null;
		_c.originalShape = null;
		_c.area = 0f;
		_c.created = 0f;
		_c.isBreakable = false;
		_c.breakingImpulse = 0f;
		_c.breakEvent = 0;
		_c.breakEventScale = 1f;
		_c.isOneway = false;
		_c.isPowerLane = false;
		_c.powerLaneShape = IntPtr.Zero;
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		m_blockComponents.RemoveItem(_c.index);
		_c.entityIndex = -1;
	}

	public static GEShapeC AddShapeComponent(Entity _e, TransformC _tc, GpcC _gpc, ShapeData _shapeData)
	{
		int num = m_shapeComponents.AddItem();
		GEShapeC gEShapeC = m_shapeComponents.m_array[num];
		gEShapeC.active = true;
		gEShapeC.entityIndex = _e.index;
		gEShapeC.TC = _tc;
		gEShapeC.GPC = _gpc;
		gEShapeC.groundSettings = _shapeData.groundSettings;
		gEShapeC.order = _shapeData.position.y;
		_e.components.Add(gEShapeC);
		return gEShapeC;
	}

	public static void RemoveShapeComponent(GEShapeC _c)
	{
		_c.active = false;
		_c.CMC = null;
		_c.GPC = null;
		_c.order = 0f;
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		m_shapeComponents.RemoveItem(_c.index);
		_c.entityIndex = -1;
	}

	public static EIC AddEditorItemContainerComponent(Entity _entity, EIC _container, string _identifier, ILevelData _data, EditorItemType _type, TransformC _tc, TransformC _uiTC)
	{
		int num = m_editorItemComponents.AddItem();
		EIC eIC = m_editorItemComponents.m_array[num];
		eIC.entityIndex = _entity.index;
		eIC.active = true;
		eIC.TC = _tc;
		eIC.uiTC = _uiTC;
		eIC.data = _data;
		eIC.itemType = (uint)_type;
		eIC.identifier = _identifier;
		eIC.subItems = new List<EIC>();
		eIC.gameComponents = new List<IComponent>();
		if (_container != null)
		{
			eIC.container = _container;
			eIC.container.subItems.Add(eIC);
			TransformS.ParentComponent(eIC.TC, eIC.container.TC);
		}
		EntityManager.m_entities.m_array[eIC.entityIndex].components.Add(eIC);
		return eIC;
	}

	public static EIC AddEditorItemComponent(EIC _container, string _identifier, ILevelData _data, EditorItemType _type, TransformC _tc, TransformC _uiTC, TouchAreaC _tac)
	{
		int num = m_editorItemComponents.AddItem();
		EIC eIC = m_editorItemComponents.m_array[num];
		eIC.entityIndex = _tc.entityIndex;
		eIC.active = true;
		eIC.TC = _tc;
		eIC.uiTC = _uiTC;
		eIC.TAC = _tac;
		eIC.TAC.customComponent = eIC;
		eIC.data = _data;
		eIC.itemType = (uint)_type;
		eIC.identifier = _identifier;
		eIC.subItems = new List<EIC>();
		eIC.gameComponents = new List<IComponent>();
		eIC.isScaleable = false;
		eIC.isScaleUnified = true;
		eIC.isRotateable = false;
		eIC.isRealtimeMovable = true;
		eIC.connectionMode = false;
		if (_container != null)
		{
			eIC.container = _container;
			eIC.container.subItems.Add(eIC);
			TransformS.ParentComponent(eIC.TC, eIC.container.TC);
		}
		EntityManager.m_entities.m_array[eIC.entityIndex].components.Add(eIC);
		return eIC;
	}

	public static EIC AddEditorItemHandleComponent(EIC _container, string _identifier, ILevelData _data, EditorItemType _type, ConnectionSlotType _connectionType, TransformC _tc, TouchAreaC _tac)
	{
		int num = m_editorItemComponents.AddItem();
		EIC eIC = m_editorItemComponents.m_array[num];
		eIC.entityIndex = _tac.entityIndex;
		eIC.active = true;
		eIC.TAC = _tac;
		eIC.TAC.customComponent = eIC;
		eIC.TC = _tc;
		eIC.data = _data;
		eIC.itemType = (uint)_type;
		eIC.identifier = _identifier;
		eIC.subItems = new List<EIC>();
		eIC.connectionSlotType = _connectionType;
		if (_container != null)
		{
			eIC.container = _container;
			eIC.container.subItems.Add(eIC);
			TransformS.ParentComponent(eIC.TC, eIC.container.uiTC);
			eIC.camera = eIC.container.camera;
		}
		EntityManager.m_entities.m_array[eIC.entityIndex].components.Add(eIC);
		return eIC;
	}

	public static EIC AddEditorItemHandleComponent(EIC _container, string _identifier, ILevelData _data, EditorItemType _type, AnchorPointInfo _relativeToA, AnchorPointInfo _relativeToB, TouchAreaC _tac)
	{
		int num = m_editorItemComponents.AddItem();
		EIC eIC = m_editorItemComponents.m_array[num];
		eIC.entityIndex = _tac.entityIndex;
		eIC.active = true;
		eIC.data = _data;
		eIC.data.Init(_container.data.id, _identifier);
		eIC.itemType = (uint)_type;
		eIC.identifier = _identifier;
		eIC.subItems = new List<EIC>();
		eIC.uiTC = _tac.TC;
		eIC.TAC = _tac;
		eIC.TAC.customComponent = eIC;
		eIC.relativeToA = _relativeToA;
		eIC.relativeToB = _relativeToB;
		eIC.container = _container;
		eIC.container.subItems.Add(eIC);
		eIC.camera = eIC.container.camera;
		EntityManager.m_entities.m_array[eIC.entityIndex].components.Add(eIC);
		return eIC;
	}

	public static void RemoveEditorItemComponent(EIC _c)
	{
		_c.active = false;
		_c.gameComponents = null;
		_c.identifier = string.Empty;
		_c.TAC = null;
		_c.isScaleable = false;
		_c.isScaleUnified = true;
		_c.isRotateable = false;
		_c.isRealtimeMovable = false;
		_c.isDrawable = false;
		_c.data = null;
		_c.uiTC = null;
		_c.TC = null;
		_c.trigger = null;
		_c.horizontalAnchor = 0;
		_c.verticalAnchor = 0;
		_c.verticalIsAbsolute = false;
		_c.horizontalIsAbsolute = false;
		_c.referenceHeight = Screen.height;
		_c.referenceWidth = Screen.width;
		_c.connectionMode = false;
		if (_c.itemType == 0)
		{
			(LevelManager.m_currentLevel as GELevel).items.Remove(_c);
			(LevelManager.m_currentLevel as GELevel).connections.Remove(_c);
		}
		if (_c.container != null)
		{
			_c.container.subItems.Remove(_c);
			_c.container = null;
		}
		_c.itemType = 1u;
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		m_editorItemComponents.RemoveItem(_c.index);
		_c.entityIndex = -1;
	}

	public static GEAffectionC AddAffectionComponent(IComponent _source, GECreatureC _affected, uint _identifier)
	{
		int num = m_affectionComponents.AddItem();
		GEAffectionC gEAffectionC = m_affectionComponents.m_array[num];
		gEAffectionC.active = true;
		gEAffectionC.entityIndex = _affected.entityIndex;
		AffectionInfo affectionInfo = GECreatureLogic.m_beganAffections[_identifier];
		AffectionInfo affectionInfo2 = GECreatureLogic.m_tickAffections[_identifier];
		AffectionInfo affectionInfo3 = GECreatureLogic.m_endAffections[_identifier];
		gEAffectionC.began = Main.m_gameTime;
		if (affectionInfo != null && affectionInfo.effect != null)
		{
			gEAffectionC.beganEffect = affectionInfo.effect;
		}
		else
		{
			gEAffectionC.beganEffect = null;
		}
		if (affectionInfo2 != null && affectionInfo2.effect != null)
		{
			gEAffectionC.maxStack = affectionInfo2.maxStack;
			gEAffectionC.tickInterval = affectionInfo2.interval;
			gEAffectionC.duration = affectionInfo2.duration;
			gEAffectionC.tickEffect = affectionInfo2.effect;
		}
		else
		{
			gEAffectionC.maxStack = 1;
			gEAffectionC.tickInterval = 0f;
			gEAffectionC.duration = 0f;
			gEAffectionC.tickEffect = null;
		}
		if (affectionInfo3 != null && affectionInfo3.effect != null)
		{
			gEAffectionC.endEffect = affectionInfo3.effect;
		}
		else
		{
			gEAffectionC.endEffect = null;
		}
		gEAffectionC.identifier = _identifier;
		gEAffectionC.source = _source;
		gEAffectionC.affected = _affected;
		gEAffectionC.affected.affections.Add(gEAffectionC);
		EntityManager.m_entities.m_array[gEAffectionC.entityIndex].components.Add(gEAffectionC);
		return gEAffectionC;
	}

	public static void RemoveAffectionComponent(GEAffectionC _c)
	{
		_c.active = false;
		_c.source = null;
		if (_c.affected != null && _c.affected.affections != null)
		{
			_c.affected.affections.Remove(_c);
		}
		_c.affected = null;
		_c.hasEnded = false;
		_c.hasBegan = false;
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		m_affectionComponents.RemoveItem(_c.index);
		_c.entityIndex = -1;
	}

	public static GEConnectionC AddConnectionComponent(int _entityIndex, ConnectionSlot _startSlot, ConnectionSlot _endSlot, BasicControlledComponent _controller, BasicControlledComponent _controllee)
	{
		int num = m_connectionComponents.AddItem();
		GEConnectionC gEConnectionC = m_connectionComponents.m_array[num];
		gEConnectionC.active = true;
		gEConnectionC.entityIndex = _entityIndex;
		bool flag = false;
		switch (_endSlot.m_connectionSlotType)
		{
		case ConnectionSlotType.ColliderType:
			if (_controllee.componentType == (ComponentType)112)
			{
				GETriggerC gETriggerC = _controllee as GETriggerC;
				foreach (IComponent item in EntityManager.GetComponentsByEntityIndex(ComponentType.Chipmunk, _controller.entityIndex))
				{
					ChipmunkC chipmunkC = null;
					if (item.componentType == ComponentType.Chipmunk)
					{
						chipmunkC = item as ChipmunkC;
					}
					if (chipmunkC != null)
					{
						gETriggerC.listenedColliderTypes.Add(chipmunkC.colliderType);
					}
				}
			}
			flag = true;
			break;
		case ConnectionSlotType.Activate:
		case ConnectionSlotType.Deactivate:
		case ConnectionSlotType.Destroy:
		case ConnectionSlotType.Modifier:
			flag = true;
			break;
		}
		if (!flag)
		{
			gEConnectionC.connectionType = ConnectionType.Normal;
			for (int i = 0; i < _controllee.inputSlots.Length; i++)
			{
				if (_controllee.inputSlots[i].m_connectionSlotType == _endSlot.m_connectionSlotType)
				{
					_controllee.inputSlots[i].m_connections.Add(gEConnectionC);
				}
			}
		}
		else
		{
			gEConnectionC.connectionType = ConnectionType.Modifier;
			for (int j = 0; j < _controllee.modifierSlots.Length; j++)
			{
				if (_controllee.modifierSlots[j].m_connectionSlotType == _endSlot.m_connectionSlotType)
				{
					_controllee.modifierSlots[j].m_connections.Add(gEConnectionC);
				}
			}
		}
		for (int k = 0; k < _controller.outputSlots.Length; k++)
		{
			if (_controller.outputSlots[k].m_connectionSlotType == _startSlot.m_connectionSlotType)
			{
				_controller.outputSlots[k].m_connections.Add(gEConnectionC);
			}
		}
		if (_controllee.triggerType == TriggerType.ControlScheme)
		{
			GEControlSchemeC gEControlSchemeC = _controllee as GEControlSchemeC;
			gEControlSchemeC.playerState.m_contollerState.components[_endSlot.m_index] = _controller;
		}
		gEConnectionC.startSlot = _startSlot;
		gEConnectionC.endSlot = _endSlot;
		gEConnectionC.controller = _controller;
		gEConnectionC.controllee = _controllee;
		gEConnectionC.depth = -1;
		EntityManager.m_entities.m_array[gEConnectionC.entityIndex].components.Add(gEConnectionC);
		return gEConnectionC;
	}

	public static void RemoveConnectionComponent(GEConnectionC _c)
	{
		_c.active = false;
		_c.startSlot = null;
		_c.endSlot = null;
		_c.container = null;
		_c.controller = null;
		_c.controllee = null;
		m_connectionComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static EIC GetEditorItemContainerWithUniqueId(uint _id)
	{
		int aliveCount = m_editorItemComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EIC eIC = m_editorItemComponents.m_array[m_editorItemComponents.m_aliveIndices[i]];
			if (eIC.itemType == 0 && eIC.data.id == _id)
			{
				return eIC;
			}
		}
		return null;
	}

	public static List<EIC> GetEditorItemsWithUniqueId(uint _id)
	{
		List<EIC> list = new List<EIC>();
		int aliveCount = m_editorItemComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EIC eIC = m_editorItemComponents.m_array[m_editorItemComponents.m_aliveIndices[i]];
			if (eIC.data != null && eIC.data.id == _id)
			{
				list.Add(eIC);
			}
		}
		return list;
	}

	public static IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		int aliveCount = m_triggerComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GETriggerC gETriggerC = m_triggerComponents.m_array[m_triggerComponents.m_aliveIndices[i]];
			if (gETriggerC.id == _id)
			{
				return gETriggerC;
			}
		}
		aliveCount = m_constraintComponents.m_aliveCount;
		for (int j = 0; j < aliveCount; j++)
		{
			GEConstraintC gEConstraintC = m_constraintComponents.m_array[m_constraintComponents.m_aliveIndices[j]];
			if (gEConstraintC.id == _id)
			{
				return gEConstraintC;
			}
		}
		aliveCount = m_controlSchemeComponents.m_aliveCount;
		for (int k = 0; k < aliveCount; k++)
		{
			GEControlSchemeC gEControlSchemeC = m_controlSchemeComponents.m_array[m_controlSchemeComponents.m_aliveIndices[k]];
			if (gEControlSchemeC.id == _id)
			{
				return gEControlSchemeC;
			}
		}
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			IControlledComponent controlledComponentWithUniqueId = gEPlugin.GetControlledComponentWithUniqueId(_id);
			if (controlledComponentWithUniqueId != null)
			{
				return controlledComponentWithUniqueId;
			}
		}
		return null;
	}

	public static GEConstraintC AddConstraintComponent(ConstraintData _data, TransformC _tc, AnchorPointInfo[] _anchorPoints)
	{
		int num = m_constraintComponents.AddItem();
		GEConstraintC gEConstraintC = m_constraintComponents.m_array[num];
		gEConstraintC.active = true;
		gEConstraintC.entityIndex = _tc.entityIndex;
		gEConstraintC.TC = _tc;
		if (_data != null)
		{
			gEConstraintC.id = _data.id;
			gEConstraintC.linearMotor = _data.linearMotor;
			gEConstraintC.linearMotorEnabled = _data.linearMotorEnabled;
			gEConstraintC.linearMotorRate = _data.linearMotorRate;
			gEConstraintC.rotaryMotorEnabled = _data.rotaryMotorEnabled;
			gEConstraintC.rotaryMotorRate = _data.rotaryMotorRate;
			gEConstraintC.rotaryMotorMaxForce = _data.rotaryMotorMaxForce;
			gEConstraintC.currentRailPos = _data.linearMotorStartPos;
			gEConstraintC.currentIndex = (int)_data.linearMotorStartIndex;
			gEConstraintC.currentRepeats = 0;
			gEConstraintC.maxRepeats = _data.railRepeats;
			gEConstraintC.railTweenStyle = (TweenStyle)_data.railInterpolationStyle;
			gEConstraintC.connectedToWorld = _data.connectToWorld;
		}
		gEConstraintC.def = new GEControlledValue();
		gEConstraintC.input = new GEControlledValue();
		gEConstraintC.modifier = new GEControlledValue();
		gEConstraintC.output = new GEControlledValue();
		gEConstraintC.inputSlots = new ConnectionSlot[0];
		gEConstraintC.outputSlots = new ConnectionSlot[0];
		gEConstraintC.modifierSlots = new ConnectionSlot[0];
		gEConstraintC.anchorPoints = _anchorPoints;
		gEConstraintC.railedPivotJointPtr = IntPtr.Zero;
		gEConstraintC.railedSlideJointAPtr = IntPtr.Zero;
		gEConstraintC.railedSlideJointBPtr = IntPtr.Zero;
		gEConstraintC.railedDampedSpringAPtr = IntPtr.Zero;
		gEConstraintC.railedDampedSpringBPtr = IntPtr.Zero;
		gEConstraintC.railedSlideJointATC = null;
		gEConstraintC.railedSlideJointBTC = null;
		gEConstraintC.pivotOffset = Vector3.zero;
		gEConstraintC.beganDelegatedCount = 0;
		EntityManager.m_entities.m_array[gEConstraintC.entityIndex].components.Add(gEConstraintC);
		return gEConstraintC;
	}

	public static void RemoveConstraintComponent(GEConstraintC _c)
	{
		_c.active = false;
		_c.TC = null;
		_c.CMC = null;
		_c.connectJointPtr = IntPtr.Zero;
		_c.railJointPtr = IntPtr.Zero;
		_c.rotaryLimitJointPtr = IntPtr.Zero;
		_c.rotaryMotorPtr = IntPtr.Zero;
		_c.rotarySpringPtr = IntPtr.Zero;
		_c.rotaryStiffnessPtr = IntPtr.Zero;
		_c.slideJointPtr = IntPtr.Zero;
		_c.motorIsStiff = true;
		_c.connectedBodies = null;
		_c.connectedBodyLocalAnchors = null;
		_c.ropeCMCs = null;
		_c.ropeLength = 0f;
		_c.connectedBodies = null;
		_c.anchorPoints = null;
		_c.currentRailPos = 0f;
		_c.currentIndex = 0;
		_c.loopStyle = 0;
		_c.rail = null;
		_c.railInterpolationStyle = 0;
		_c.railClosed = false;
		_c.triggered = false;
		_c.began = false;
		_c.end = false;
		_c.beganTime = 0f;
		_c.endTime = 0f;
		_c.toggle = false;
		_c.input.Zero();
		_c.def.Zero();
		_c.output.Zero();
		_c.collidingCount = 0;
		_c.ropeCutTime = -1f;
		_c.pivotOffset = Vector3.zero;
		if (_c.BeganEventDelegate != null)
		{
			Delegate[] invocationList = _c.BeganEventDelegate.GetInvocationList();
			Delegate[] array = invocationList;
			foreach (Delegate obj in array)
			{
				_c.BeganEventDelegate = (TriggerEventDelegate)Delegate.Remove(_c.BeganEventDelegate, (TriggerEventDelegate)obj);
			}
			_c.beganDelegatedCount = 0;
		}
		m_constraintComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static GETriggerC AddTriggerComponent(Camera _camera, TriggerData _data, ChipmunkC _cmc)
	{
		int num = m_triggerComponents.AddItem();
		GETriggerC gETriggerC = m_triggerComponents.m_array[num];
		gETriggerC.active = true;
		gETriggerC.entityIndex = _cmc.entityIndex;
		gETriggerC.id = _data.id;
		gETriggerC.triggerType = (TriggerType)_data.triggerType;
		gETriggerC.CMC = _cmc;
		gETriggerC.TC = _cmc.TC;
		gETriggerC.def = new GEControlledValue();
		gETriggerC.input = new GEControlledValue();
		gETriggerC.modifier = new GEControlledValue();
		gETriggerC.output = new GEControlledValue();
		gETriggerC.update = true;
		gETriggerC.camera = _camera;
		gETriggerC.update = true;
		gETriggerC.triggerCount = 0;
		gETriggerC.collidingCount = 0;
		gETriggerC.listenedColliderTypes = new List<ColliderType>();
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[0];
		gETriggerC.modifierSlots = new ConnectionSlot[0];
		gETriggerC.dispatched = false;
		gETriggerC.dispatchOnlyOnce = _data.eventDispatchOnlyOnce;
		gETriggerC.data = _data;
		gETriggerC.toggle = _data.toggle;
		gETriggerC.triggerOnlyOnce = _data.triggerOnlyOnce;
		gETriggerC.triggerOnlyOnFullEnergy = _data.triggerOnlyOnFullEnergy;
		gETriggerC.triggerUntilOutOfEnergy = _data.triggerUntilOutOfEnergy;
		gETriggerC.autoTrigger = _data.autoTrigger;
		gETriggerC.triggerCooldown = _data.triggerCooldown;
		gETriggerC.energy = _data.energy;
		gETriggerC.energyConsume = _data.energyConsume;
		gETriggerC.energyConsumeInterval = _data.consumeInterval;
		gETriggerC.energyGain = _data.energyGain;
		gETriggerC.energyGainInterval = _data.gainInterval;
		gETriggerC.gainCooldown = _data.cooldown;
		gETriggerC.energyClips = _data.energyClips;
		gETriggerC.reloadCooldown = _data.reloadCooldown;
		if (_data.defaultNumericValue != null)
		{
			gETriggerC.def.vector = _data.defaultNumericValue.ToVector3();
		}
		else
		{
			gETriggerC.def.vector = Vector3.one;
		}
		gETriggerC.def.text = _data.defaultTextualValue;
		gETriggerC.actionType = _data.action;
		gETriggerC.lastGain = 0f;
		gETriggerC.lastConsume = 0f;
		gETriggerC.lastReload = 0f;
		gETriggerC.beganDelegatedCount = 0;
		EntityManager.m_entities.m_array[_cmc.entityIndex].components.Add(gETriggerC);
		return gETriggerC;
	}

	public static GETriggerC AddTriggerComponent(Camera _camera, TriggerData _data, TriggerType _type, TransformC _tc)
	{
		int num = m_triggerComponents.AddItem();
		GETriggerC gETriggerC = m_triggerComponents.m_array[num];
		gETriggerC.active = true;
		gETriggerC.entityIndex = _tc.entityIndex;
		gETriggerC.id = _data.id;
		gETriggerC.triggerType = _type;
		gETriggerC.TC = _tc;
		gETriggerC.def = new GEControlledValue();
		gETriggerC.input = new GEControlledValue();
		gETriggerC.modifier = new GEControlledValue();
		gETriggerC.output = new GEControlledValue();
		gETriggerC.camera = _camera;
		gETriggerC.dispatched = false;
		gETriggerC.dispatchOnlyOnce = true;
		gETriggerC.update = true;
		gETriggerC.triggerCount = 0;
		gETriggerC.collidingCount = 0;
		gETriggerC.listenedColliderTypes = new List<ColliderType>();
		gETriggerC.inputSlots = new ConnectionSlot[0];
		gETriggerC.outputSlots = new ConnectionSlot[0];
		gETriggerC.modifierSlots = new ConnectionSlot[0];
		gETriggerC.dispatched = false;
		gETriggerC.dispatchOnlyOnce = _data.eventDispatchOnlyOnce;
		gETriggerC.toggle = _data.toggle;
		gETriggerC.triggerOnlyOnce = _data.triggerOnlyOnce;
		gETriggerC.triggerOnlyOnFullEnergy = _data.triggerOnlyOnFullEnergy;
		gETriggerC.triggerUntilOutOfEnergy = _data.triggerUntilOutOfEnergy;
		gETriggerC.autoTrigger = _data.autoTrigger;
		gETriggerC.triggerCooldown = _data.triggerCooldown;
		gETriggerC.energy = _data.energy;
		gETriggerC.energyConsume = _data.energyConsume;
		gETriggerC.energyConsumeInterval = _data.consumeInterval;
		gETriggerC.energyGain = _data.energyGain;
		gETriggerC.energyGainInterval = _data.gainInterval;
		gETriggerC.gainCooldown = _data.cooldown;
		gETriggerC.energyClips = _data.energyClips;
		gETriggerC.reloadCooldown = _data.reloadCooldown;
		if (_data.defaultNumericValue != null)
		{
			gETriggerC.def.vector = _data.defaultNumericValue.ToVector3();
		}
		else
		{
			gETriggerC.def.vector = Vector3.one;
		}
		gETriggerC.def.text = _data.defaultTextualValue;
		gETriggerC.actionType = _data.action;
		gETriggerC.lastGain = 0f;
		gETriggerC.lastConsume = 0f;
		gETriggerC.lastReload = 0f;
		gETriggerC.beganDelegatedCount = 0;
		gETriggerC.data = _data;
		EntityManager.m_entities.m_array[_tc.entityIndex].components.Add(gETriggerC);
		return gETriggerC;
	}

	public static void RemoveTriggerComponent(GETriggerC _c)
	{
		_c.active = false;
		_c.CMC = null;
		_c.EC = null;
		_c.TC = null;
		_c.tileTC = null;
		_c.fingerTAC = null;
		_c.fingerCMC = null;
		_c.fingerTouchIndices = null;
		_c.fingerTouchCMC = null;
		_c.fingerSensorCMC = null;
		_c.dragging = false;
		_c.fingerCollisions = null;
		_c.fingerBC = null;
		_c.collisionHandler = null;
		_c.camera = null;
		_c.connectedCMC = null;
		_c.actionType = 0;
		_c.triggered = false;
		_c.began = false;
		_c.end = false;
		_c.beganTime = 0f;
		_c.endTime = 0f;
		_c.toggle = false;
		_c.input.Zero();
		_c.def.Zero();
		_c.output.Zero();
		_c.collidingCount = 0;
		_c.debug = null;
		_c.debugTC = null;
		_c.listenedColliderTypes = null;
		if (_c.BeganEventDelegate != null)
		{
			Delegate[] invocationList = _c.BeganEventDelegate.GetInvocationList();
			Delegate[] array = invocationList;
			foreach (Delegate obj in array)
			{
				_c.BeganEventDelegate = (TriggerEventDelegate)Delegate.Remove(_c.BeganEventDelegate, (TriggerEventDelegate)obj);
			}
			_c.beganDelegatedCount = 0;
			_c.BeganEventDelegate = null;
		}
		if (_c.EndEventDelegate != null)
		{
			Delegate[] invocationList2 = _c.EndEventDelegate.GetInvocationList();
			Delegate[] array2 = invocationList2;
			foreach (Delegate obj2 in array2)
			{
				_c.EndEventDelegate = (TriggerEventDelegate)Delegate.Remove(_c.EndEventDelegate, (TriggerEventDelegate)obj2);
			}
			_c.endDelegatedCount = 0;
			_c.EndEventDelegate = null;
		}
		m_triggerComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static GETransformGizmoC AddTransformGizmoComponent(TransformC _gizmoTC)
	{
		int num = m_transformGizmoComponents.AddItem();
		GETransformGizmoC gETransformGizmoC = m_transformGizmoComponents.m_array[num];
		gETransformGizmoC.entityIndex = _gizmoTC.entityIndex;
		gETransformGizmoC.active = true;
		gETransformGizmoC.gizmoTC = _gizmoTC;
		EntityManager.m_entities.m_array[gETransformGizmoC.entityIndex].components.Add(gETransformGizmoC);
		return gETransformGizmoC;
	}

	public static void RemoveTransformGizmoComponent(GETransformGizmoC _c)
	{
		_c.active = false;
		_c.gizmoTC = null;
		_c.moveTAC = null;
		_c.readyToMove = false;
		_c.originalRotation.Clear();
		_c.originalScale.Clear();
		_c.originalPosition.Clear();
		_c.rotateStart = Vector3.zero;
		m_transformGizmoComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static void BuildRailAnchorHandles(EIC _eic)
	{
		EntityManager.RemoveEntitiesByTag(LevelManager.m_currentLevel.name + ":EditorHandle", true);
		string[] tags = new string[3]
		{
			LevelManager.m_currentLevel.name + ":EditorHandle",
			LevelManager.m_currentLevel.name,
			LevelManager.m_currentLevel.name + ":RailAnchorHandle"
		};
		if (_eic.gameComponents.Count <= 0)
		{
			return;
		}
		GEConstraintC gEConstraintC = _eic.gameComponents[0] as GEConstraintC;
		AnchorPointInfo[] anchorPoints = gEConstraintC.anchorPoints;
		if (anchorPoints.Length <= 1)
		{
			return;
		}
		AnchorPointInfo anchorPointInfo = anchorPoints[0];
		Vector3 zero = Vector3.zero;
		ConstraintData constraintData = _eic.data as ConstraintData;
		if (!constraintData.railClosed)
		{
			zero = anchorPoints[0].position + (anchorPoints[0].position - anchorPoints[1].position).normalized * 20f;
			EIC eIC = GERailPointHandleA.Assemble(_eic.camera, null, _eic, anchorPoints[0], anchorPoints[0], zero, "EditorHandle", tags);
			(eIC.data as ConstraintPointData).anchorIndex = 0;
		}
		for (int i = 1; i < anchorPoints.Length; i++)
		{
			AnchorPointInfo anchorPointInfo2 = anchorPoints[i];
			zero = (anchorPointInfo.position + anchorPointInfo2.position) * 0.5f;
			EIC eIC = GERailPointHandleA.Assemble(_eic.camera, null, _eic, anchorPointInfo, anchorPointInfo2, zero, "EditorHandle", tags);
			(eIC.data as ConstraintPointData).anchorIndex = i;
			if (i == anchorPoints.Length - 1 && anchorPoints.Length > 2)
			{
				if (constraintData.railClosed)
				{
					anchorPointInfo = anchorPointInfo2;
					anchorPointInfo2 = anchorPoints[0];
					zero = (anchorPointInfo.position + anchorPointInfo2.position) * 0.5f;
					eIC = GERailPointHandleA.Assemble(_eic.camera, null, _eic, anchorPointInfo, anchorPointInfo2, zero, "EditorHandle", tags);
					(eIC.data as ConstraintPointData).anchorIndex = i + 1;
				}
				else
				{
					anchorPointInfo = anchorPoints[i - 1];
					zero = anchorPointInfo2.position + (anchorPointInfo2.position - anchorPointInfo.position).normalized * 20f;
					eIC = GERailPointHandleA.Assemble(_eic.camera, null, _eic, anchorPointInfo, anchorPointInfo2, zero, "EditorHandle", tags);
					(eIC.data as ConstraintPointData).anchorIndex = i + 1;
				}
			}
			anchorPointInfo = anchorPointInfo2;
		}
		if (anchorPoints.Length == 1)
		{
			AnchorPointInfo anchorPointInfo2 = anchorPoints[0];
			anchorPointInfo.position = anchorPointInfo2.position + Vector3.up * 50f;
			zero = anchorPointInfo2.position + Vector3.up * 100f;
			EIC eIC = GERailPointHandleA.Assemble(_eic.camera, null, _eic, anchorPointInfo, anchorPointInfo2, zero, "EditorHandle", tags);
			(eIC.data as ConstraintPointData).anchorIndex = 1;
		}
	}

	public static void SetContainerPosition(EIC _container, bool _traverseParents)
	{
		if (_container == null)
		{
			return;
		}
		if (_container.itemType == 0)
		{
			int count = _container.subItems.Count;
			if (count > 0)
			{
				Vector3 zero = Vector3.zero;
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					if (_container.subItems[i].itemType == 2 || _container.subItems[i].itemType == 1)
					{
						zero += _container.subItems[i].TC.transform.position;
						num++;
					}
				}
				if (num > 0)
				{
					zero /= (float)num;
				}
				Vector3 position = zero;
				if (_container.camera == Main.camera)
				{
					Vector3 position2 = Main.camera.transform.position;
					position2.z = 0f;
					position = Main.camera.WorldToScreenPoint(zero) - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
					position.z = 0f;
				}
				TransformS.SetGlobalPositionWithoutChildren(_container.TC, zero);
				TransformS.SetGlobalPositionWithoutChildren(_container.uiTC, position);
				_container.data.position = new Vertex3(zero);
			}
		}
		if (_traverseParents && _container.container != null)
		{
			SetContainerPosition(_container.container, true);
		}
	}

	public static EIC DublicateEditorItem(EIC _eic, Vector3 _offset)
	{
		List<EIC> list = new List<EIC>();
		list.Add(_eic);
		return DublicateEditorItem(_eic, _offset, list);
	}

	public static EIC DublicateEditorItem(EIC _eic, Vector3 _offset, List<EIC> _processed)
	{
		EIC eIC = ConstructCopy(_eic, _offset, -1);
		uint id = eIC.data.id;
		if (eIC != null)
		{
			UIC level = GEOutlinerA.m_level;
			if (_eic.container != null)
			{
				_eic.container.subItems.Add(eIC);
				eIC.container = _eic.container;
				TransformS.ParentComponent(eIC.TC, _eic.container.TC);
				TransformS.ParentComponent(eIC.uiTC, _eic.container.TC);
				level = UIS.GetUIComponentByIdentifier(eIC.container.index);
				EditorState.AddItemToOutliner(level, eIC, Mathf.RoundToInt(level.intent + 1f));
			}
			else
			{
				GELevel gELevel = LevelManager.m_currentLevel as GELevel;
				gELevel.items.Add(eIC);
				EditorState.AddItemToOutliner(level, eIC, 1);
			}
			uint id2 = _eic.data.id;
			List<EIC> editorItemsWithUniqueId = GetEditorItemsWithUniqueId(id2);
			Debug.Log(id2 + ": " + editorItemsWithUniqueId.Count);
			for (int i = 0; i < editorItemsWithUniqueId.Count; i++)
			{
				if (!_processed.Contains(editorItemsWithUniqueId[i]))
				{
					EIC eIC2 = ConstructCopy(editorItemsWithUniqueId[i], _offset, (int)id);
					_processed.Add(editorItemsWithUniqueId[i]);
					level = GEOutlinerA.m_level;
					if (editorItemsWithUniqueId[i].container != null)
					{
						editorItemsWithUniqueId[i].container.subItems.Add(eIC2);
						eIC2.container = editorItemsWithUniqueId[i].container;
						TransformS.ParentComponent(eIC2.TC, editorItemsWithUniqueId[i].container.TC);
						TransformS.ParentComponent(eIC2.uiTC, editorItemsWithUniqueId[i].container.TC);
						level = UIS.GetUIComponentByIdentifier(eIC2.container.index);
						EditorState.AddItemToOutliner(level, eIC2, Mathf.RoundToInt(level.intent + 1f));
					}
					else
					{
						GELevel gELevel2 = LevelManager.m_currentLevel as GELevel;
						gELevel2.items.Add(eIC2);
						EditorState.AddItemToOutliner(level, eIC2, 1);
					}
				}
			}
			return eIC;
		}
		EditorState.FillEditorItemHierarchy(eIC);
		return null;
	}

	public static EIC ConstructCopy(EIC _original, Vector3 _offset, int _id)
	{
		ILevelData levelData = _original.data.DeepCopy();
		uint num = 0u;
		num = ((_id != -1) ? ((uint)_id) : GetUniqueId());
		levelData.Init(num, _original.identifier + num);
		Vector3 v = levelData.position.ToVector3() + _offset;
		levelData.position = new Vertex3(v);
		EIC eIC = null;
		if (_original.itemType == 1)
		{
			eIC = GEItemA.Assemble(null, _original.identifier, levelData, _original.camera);
		}
		else if (_original.itemType == 2)
		{
			eIC = GEAnchorA.Assemble(null, _original.identifier, levelData, _original.camera);
		}
		else if (_original.itemType == 0)
		{
			eIC = GEContainerA.Assemble(null, _original.identifier, levelData);
		}
		eIC.camera = _original.camera;
		eIC.isDrawable = _original.isDrawable;
		eIC.isRealtimeMovable = _original.isRealtimeMovable;
		eIC.isRotateable = _original.isRotateable;
		eIC.isScaleable = _original.isScaleable;
		eIC.isScaleUnified = _original.isScaleUnified;
		for (int i = 0; i < _original.subItems.Count; i++)
		{
			if (_original.subItems[i].itemType == 1 || _original.subItems[i].itemType == 2)
			{
				num = ((_original.data.id != _original.subItems[i].data.id) ? GetUniqueId() : eIC.data.id);
				ILevelData levelData2 = _original.subItems[i].data.DeepCopy();
				levelData2.Init(num, _original.subItems[i].identifier + num);
				v = levelData2.position.ToVector3() + _offset;
				levelData2.position = new Vertex3(v);
				EIC eIC2 = null;
				eIC2 = ((_original.subItems[i].itemType != 1) ? GEAnchorA.Assemble(eIC, _original.subItems[i].identifier, levelData2, _original.camera) : GEItemA.Assemble(eIC, _original.subItems[i].identifier, levelData2, _original.camera));
				eIC2.camera = _original.subItems[i].camera;
				eIC2.isDrawable = _original.subItems[i].isDrawable;
				eIC2.isRealtimeMovable = _original.subItems[i].isRealtimeMovable;
				eIC2.isRotateable = _original.subItems[i].isRotateable;
				eIC2.isScaleable = _original.subItems[i].isScaleable;
				eIC2.isScaleUnified = _original.subItems[i].isScaleUnified;
				TransformS.SetGlobalPosition(eIC2.TC, levelData2.position.ToVector3());
				TransformS.SetGlobalPosition(eIC2.uiTC, levelData2.position.ToVector3());
				TransformS.SetRotation(eIC2.TC, levelData2.rotation.ToVector3());
				TransformS.SetScale(eIC2.TC, levelData2.scale.ToVector3());
			}
			else if (_original.subItems[i].itemType == 0)
			{
				EIC eIC3 = DublicateEditorItem(_original.subItems[i], _offset);
				eIC.subItems.Add(eIC3);
				eIC3.container = eIC;
				TransformS.ParentComponent(eIC3.TC, eIC.TC);
			}
		}
		return eIC;
	}

	public static void Update()
	{
		int num = 0;
		if (GEState.generateShapes)
		{
			GELevelGenerator.CreateShapes();
		}
		num = m_shapeComponents.m_aliveCount;
		for (int i = 0; i < num; i++)
		{
			GEShapeC gEShapeC = m_shapeComponents.m_array[m_shapeComponents.m_aliveIndices[i]];
			if (gEShapeC.active && gEShapeC.groundSettings.groundType == 1)
			{
				Material material = ResourceManager.GetMaterial(gEShapeC.groundSettings.fillMaterialResourceIdentifier);
				if (material == null)
				{
					material = ResourceManager.GetMaterial("Solid");
				}
				Material material2 = material;
				Vector3 position = Main.camera.transform.position;
				Vector3 eulerAngles = Main.camera.transform.rotation.eulerAngles;
				float num2 = position.x - gEShapeC.TC.transform.position.x;
				float num3 = position.y - gEShapeC.TC.transform.position.y;
				float num4 = position.z - gEShapeC.TC.transform.position.z;
				float num5 = Main.camera.fieldOfView * ((float)Math.PI / 180f);
				float num6 = (0f - num4) * Mathf.Tan(num5 * 0.5f) * 2f;
				float num7 = (float)Screen.height / num6;
				material2.mainTextureOffset = new Vector2((0f - num2) * gEShapeC.groundSettings.parallaxAmount.x, (0f - num3) * gEShapeC.groundSettings.parallaxAmount.y) / num6 + Vector2.one * 0.5f;
				material2.mainTextureScale = Vector2.one * num7;
				m_lastCameraPos = position;
			}
		}
		num = m_editorItemComponents.m_aliveCount;
		for (int j = 0; j < num; j++)
		{
			EIC eIC = m_editorItemComponents.m_array[m_editorItemComponents.m_aliveIndices[j]];
			if (!GEState.editorMode || (eIC.itemType != 2 && eIC.itemType != 1))
			{
				continue;
			}
			if (eIC.camera == Main.camera)
			{
				Vector3 position2 = eIC.TC.transform.position;
				Vector3 position3 = Main.camera.transform.position;
				position3.z = 0f;
				Vector3 position4 = Main.camera.WorldToScreenPoint(position2) - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
				position4.z = 0f;
				TransformS.SetGlobalPosition(eIC.uiTC, position4);
				eIC.data.position = new Vertex3(position2);
				eIC.data.rotation = new Vertex3(eIC.TC.transform.eulerAngles);
			}
			else
			{
				Vector3 position5 = eIC.TC.transform.position;
				position5.z = -200f;
				TransformS.SetGlobalPosition(eIC.uiTC, position5);
				eIC.data.position = new Vertex3(position5);
				eIC.data.rotation = new Vertex3(eIC.TC.transform.eulerAngles);
			}
			if (eIC.itemType == 4)
			{
				Vector3 zero = Vector3.zero;
				zero = (eIC.relativeToB.position + eIC.relativeToA.position) * 0.5f;
				TransformS.SetPosition(eIC.TAC.TC, zero);
			}
			if (!(eIC.identifier == "Rail Motor") || eIC.container == null || !(eIC.container.identifier == "Rail") || eIC.gameComponents.Count <= 0)
			{
				continue;
			}
			GEConstraintC gEConstraintC = eIC.gameComponents[0] as GEConstraintC;
			gEConstraintC.currentIndex = Mathf.Min(gEConstraintC.currentIndex, gEConstraintC.rail.anchorPoints.Length - 1);
			if (gEConstraintC.rail.anchorPoints.Length <= 1)
			{
				continue;
			}
			Vector2 vector = gEConstraintC.rail.anchorPoints[gEConstraintC.currentIndex].position;
			int num8 = gEConstraintC.currentIndex + 1;
			if (num8 == gEConstraintC.rail.anchorPoints.Length)
			{
				num8 = 0;
			}
			Vector2 vector2 = (Vector2)gEConstraintC.rail.anchorPoints[num8].position - vector;
			Vector3 vector3 = Vector3.zero;
			if (gEConstraintC.rail.railInterpolationStyle == 1)
			{
				vector3.x = TweenS.tween(gEConstraintC.railTweenStyle, gEConstraintC.currentRailPos, 1f, vector.x, vector2.x);
				vector3.y = TweenS.tween(gEConstraintC.railTweenStyle, gEConstraintC.currentRailPos, 1f, vector.y, vector2.y);
			}
			else if (gEConstraintC.rail.railInterpolationStyle == 0)
			{
				int currentIndex = gEConstraintC.currentIndex;
				Vector3 position6;
				Vector3 position7;
				Vector3 vector4;
				Vector3 vector5;
				if (currentIndex == 0)
				{
					position6 = gEConstraintC.rail.anchorPoints[currentIndex].position;
					position7 = gEConstraintC.rail.anchorPoints[currentIndex + 1].position;
					vector4 = ((currentIndex + 2 < gEConstraintC.rail.anchorPoints.Length) ? gEConstraintC.rail.anchorPoints[currentIndex + 2].position : (position7 + (position7 - position6)));
					vector5 = ((!gEConstraintC.rail.railClosed) ? (position6 - (position7 - position6)) : gEConstraintC.rail.anchorPoints[gEConstraintC.rail.anchorPoints.Length - 1].position);
				}
				else if (currentIndex == gEConstraintC.rail.anchorPoints.Length - 1)
				{
					vector5 = gEConstraintC.rail.anchorPoints[currentIndex - 1].position;
					position6 = gEConstraintC.rail.anchorPoints[currentIndex].position;
					position7 = gEConstraintC.rail.anchorPoints[0].position;
					vector4 = gEConstraintC.rail.anchorPoints[1].position;
				}
				else
				{
					vector5 = gEConstraintC.rail.anchorPoints[currentIndex - 1].position;
					position6 = gEConstraintC.rail.anchorPoints[currentIndex].position;
					position7 = gEConstraintC.rail.anchorPoints[currentIndex + 1].position;
					vector4 = ((currentIndex + 2 < gEConstraintC.rail.anchorPoints.Length) ? gEConstraintC.rail.anchorPoints[currentIndex + 2].position : ((!gEConstraintC.rail.railClosed) ? (position7 + (position7 - position6)) : gEConstraintC.rail.anchorPoints[0].position));
				}
				vector3 = ToolBox.PointOnSplineSegment(vector5, position6, position7, vector4, gEConstraintC.currentRailPos);
			}
			eIC.data.position = new Vertex3(vector3);
			TransformS.SetGlobalPosition(eIC.TC, vector3);
		}
		if (GEState.connectionTC != null)
		{
			DebugDraw.Clear(Main.camera, GEState.connectionTC);
		}
		num = m_connectionComponents.m_aliveCount;
		for (int k = 0; k < num; k++)
		{
			GEConnectionC gEConnectionC = m_connectionComponents.m_array[m_connectionComponents.m_aliveIndices[k]];
			if (gEConnectionC.active)
			{
				GEConnectionLogic.Update(gEConnectionC);
			}
		}
		num = m_constraintComponents.m_aliveCount;
		for (int l = 0; l < num; l++)
		{
			GEConstraintC gEConstraintC2 = m_constraintComponents.m_array[m_constraintComponents.m_aliveIndices[l]];
			if (gEConstraintC2.active)
			{
				GEConstraintLogic.Update(gEConstraintC2);
			}
		}
		while (GEConnectionLogic.m_removeList.Count > 0)
		{
			int index = GEConnectionLogic.m_removeList.Count - 1;
			GEConnectionLogic.RemoveConnectionsByAnchoredId(GEConnectionLogic.m_removeList[index], ConnectionSlotType.Any);
			GEConnectionLogic.m_removeList.RemoveAt(index);
		}
		num = m_blockComponents.m_aliveCount;
		for (int m = 0; m < num; m++)
		{
		}
		num = m_transformGizmoComponents.m_aliveCount;
		for (int n = 0; n < num; n++)
		{
			GETransformGizmoC gETransformGizmoC = m_transformGizmoComponents.m_array[m_transformGizmoComponents.m_aliveIndices[n]];
			if (!gETransformGizmoC.active)
			{
				continue;
			}
			Vector3 zero2 = Vector3.zero;
			int count = EditorState.m_selection.Count;
			if (count > 0)
			{
				for (int num9 = 0; num9 < count; num9++)
				{
					zero2 += EditorState.m_selection[num9].uiTC.transform.position;
				}
				zero2 /= (float)count;
				TransformS.SetGlobalPosition(gETransformGizmoC.gizmoTC, zero2);
			}
		}
		num = m_vehicleComponents.m_aliveCount;
		if (!GEState.editorMode)
		{
			for (int num10 = 0; num10 < num; num10++)
			{
				GEVehicleC gEVehicleC = m_vehicleComponents.m_array[m_vehicleComponents.m_aliveIndices[num10]];
				if (gEVehicleC.active)
				{
					GEVehicleLogic.Update(gEVehicleC);
				}
			}
		}
		num = m_characterComponents.m_aliveCount;
		for (int num11 = 0; num11 < num; num11++)
		{
			GECharacterC gECharacterC = m_characterComponents.m_array[m_characterComponents.m_aliveIndices[num11]];
			if (gECharacterC.active)
			{
				GECharacterLogic.Update(gECharacterC);
			}
		}
		num = m_affectionComponents.m_aliveCount;
		for (int num12 = num - 1; num12 > -1; num12--)
		{
			GEAffectionC gEAffectionC = m_affectionComponents.m_array[m_affectionComponents.m_aliveIndices[num12]];
			bool flag = false;
			if (gEAffectionC.active)
			{
				flag = GECreatureLogic.UpdateAffection(gEAffectionC);
			}
			if (flag)
			{
				m_affectionsRemoveList.Add(gEAffectionC.index);
			}
		}
		while (m_affectionsRemoveList.Count > 0)
		{
			int index2 = m_affectionsRemoveList.Count - 1;
			RemoveAffectionComponent(m_affectionComponents.m_array[m_affectionsRemoveList[index2]]);
			m_affectionsRemoveList.RemoveAt(index2);
		}
		num = m_physicsAffectorComponents.m_aliveCount;
		for (int num13 = num - 1; num13 > -1; num13--)
		{
			GEPhysicsAffectorC gEPhysicsAffectorC = m_physicsAffectorComponents.m_array[m_physicsAffectorComponents.m_aliveIndices[num13]];
			if (gEPhysicsAffectorC.active)
			{
				GEPhysicsAffectorLogic.UpdatePhysicsAffector(gEPhysicsAffectorC);
			}
		}
		if (GEState.editorMode)
		{
			return;
		}
		num = m_triggerComponents.m_aliveCount;
		for (int num14 = 0; num14 < num; num14++)
		{
			GETriggerC gETriggerC = m_triggerComponents.m_array[m_triggerComponents.m_aliveIndices[num14]];
			if (gETriggerC.active)
			{
				GETriggerLogic.Update(gETriggerC);
			}
		}
	}
}

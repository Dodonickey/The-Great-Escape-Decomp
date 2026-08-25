using System;
using System.Collections;
using UnityEngine;

public static class GESpritePrefabS
{
	private static int m_spritePrefabCount = 50;

	private static GenericArray<GESpritePrefabC> m_spritePrefabComponents;

	private static int m_lastFrame;

	private static int skip = 1;

	public static void Initialize(int _count)
	{
		m_spritePrefabCount = _count;
		m_spritePrefabComponents = new GenericArray<GESpritePrefabC>(m_spritePrefabCount);
		for (int i = 0; i < m_spritePrefabCount; i++)
		{
			m_spritePrefabComponents.m_array[i] = new GESpritePrefabC();
			m_spritePrefabComponents.m_array[i].entityIndex = -1;
			m_spritePrefabComponents.m_array[i].index = i;
			m_spritePrefabComponents.m_array[i].componentType = (ComponentType)116;
		}
	}

	public static GESpritePrefabC AddComponent(SpritePrefabNode _rootNode, SpritePrefabNode[] _nodes, Hashtable _nodeTable)
	{
		int num = m_spritePrefabComponents.AddItem();
		GESpritePrefabC gESpritePrefabC = m_spritePrefabComponents.m_array[num];
		gESpritePrefabC.entityIndex = _rootNode.TC.entityIndex;
		gESpritePrefabC.active = true;
		gESpritePrefabC.rootNode = _rootNode;
		gESpritePrefabC.nodes = _nodes;
		gESpritePrefabC.nodeTable = _nodeTable;
		gESpritePrefabC.currentFrame = 0;
		gESpritePrefabC.flipX = 1;
		EntityManager.m_entities.m_array[gESpritePrefabC.entityIndex].components.Add(gESpritePrefabC);
		return gESpritePrefabC;
	}

	public static void RemoveComponent(GESpritePrefabC _c)
	{
		_c.active = false;
		_c.animation = null;
		_c.nodes = null;
		_c.nodeTable = null;
		_c.rootNode = null;
		_c.animations = null;
		_c.flipX = 1;
		_c.customComponent = null;
		_c.animatePhysics = true;
		m_spritePrefabComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static void RelaxRotarySprings(GESpritePrefabC _c)
	{
		for (int i = 0; i < _c.nodes.Length; i++)
		{
			if (_c.nodes[i].hasPhysics == 1 && _c.nodes[i].hasRotarySpring == 1 && _c.nodes[i].rotarySpring != IntPtr.Zero)
			{
				ChipmunkWrapper.SetDampedRotarySpringProperties(_c.nodes[i].rotarySpring, 0f, 50f, 0f);
			}
		}
		_c.animatePhysics = false;
	}

	public static void UnrelaxRotarySprings(GESpritePrefabC _c)
	{
		for (int i = 0; i < _c.nodes.Length; i++)
		{
			if (_c.nodes[i].hasPhysics == 1 && _c.nodes[i].hasRotarySpring == 1 && _c.nodes[i].rotarySpring != IntPtr.Zero)
			{
				ChipmunkWrapper.SetDampedRotarySpringProperties(_c.nodes[i].rotarySpring, _c.nodes[i].rotarySpringStrength, _c.nodes[i].rotarySpringDamp, _c.nodes[i].globalRotation.z * ((float)Math.PI / 180f));
			}
		}
		_c.animatePhysics = true;
	}

	public static void Update()
	{
		if (skip == 1)
		{
			int aliveCount = m_spritePrefabComponents.m_aliveCount;
			for (int i = 0; i < aliveCount; i++)
			{
				GESpritePrefabC gESpritePrefabC = m_spritePrefabComponents.m_array[m_spritePrefabComponents.m_aliveIndices[i]];
				GECharacterC gECharacterC = null;
				if (gESpritePrefabC.customComponent != null && gESpritePrefabC.customComponent.componentType == (ComponentType)102)
				{
					gECharacterC = gESpritePrefabC.customComponent as GECharacterC;
				}
				if (!gESpritePrefabC.active || gESpritePrefabC.animation == null)
				{
					continue;
				}
				for (int j = 0; j < gESpritePrefabC.nodes.Length; j++)
				{
					int num = gESpritePrefabC.currentFrame * gESpritePrefabC.animation.nodeCount + j;
					SpritePrefabNode spritePrefabNode = gESpritePrefabC.nodes[j];
					Vector3 vector = gESpritePrefabC.animation.positions[num];
					vector.x *= gESpritePrefabC.flipX;
					if (spritePrefabNode.parentIndex != -1)
					{
						vector -= gESpritePrefabC.nodes[spritePrefabNode.parentIndex].localCenter;
					}
					if (spritePrefabNode.isEye == 1 && gECharacterC != null && gECharacterC.vehicle != null)
					{
						vector.x += gECharacterC.vehicle.currentLookDir.x;
						vector.y += gECharacterC.vehicle.currentLookDir.y * 0.5f;
					}
					Vector3 vector2 = gESpritePrefabC.animation.rotations[num];
					Vector3 scale = gESpritePrefabC.animation.scales[num];
					if (gESpritePrefabC.animation.visibilities[num])
					{
						if (spritePrefabNode.SC != null && !spritePrefabNode.SC.visible)
						{
							SpriteS.SetVisibility(spritePrefabNode.SC, true);
						}
						TransformS.SetScale(spritePrefabNode.TC, scale);
						if (gESpritePrefabC.animatePhysics)
						{
							if (spritePrefabNode.hasRotarySpring == 1 && spritePrefabNode.rotarySpring != IntPtr.Zero)
							{
								ChipmunkWrapper.SetDampedRotarySpringProperties(spritePrefabNode.rotarySpring, spritePrefabNode.rotarySpringStrength, spritePrefabNode.rotarySpringDamp, (0f - vector2.z) * ((float)Math.PI / 180f) * (float)gESpritePrefabC.flipX);
							}
							if (spritePrefabNode.hasPhysics == 1 && spritePrefabNode.pivotJoint != IntPtr.Zero)
							{
								ChipmunkWrapper.SetPivotJointOffsetA(spritePrefabNode.pivotJoint, vector);
							}
						}
						if (spritePrefabNode.hasPhysics == 0)
						{
							TransformS.SetPosition(spritePrefabNode.TC, vector);
						}
					}
					else if (spritePrefabNode.SC != null && spritePrefabNode.SC.visible)
					{
						SpriteS.SetVisibility(spritePrefabNode.SC, false);
					}
				}
				gESpritePrefabC.currentFrame++;
				if (gESpritePrefabC.currentFrame >= gESpritePrefabC.animation.frames)
				{
					gESpritePrefabC.currentFrame = 0;
				}
			}
		}
		skip *= -1;
	}

	public static void FlipX(GESpritePrefabC _c)
	{
		_c.flipX *= -1;
		for (int i = 0; i < _c.nodes.Length; i++)
		{
			SpritePrefabNode spritePrefabNode = _c.nodes[i];
			spritePrefabNode.localCenter.x *= -1f;
			spritePrefabNode.localPosition.x *= -1f;
			spritePrefabNode.localRotation.z *= -1f;
			spritePrefabNode.globalCenter.x *= -1f;
			spritePrefabNode.globalPosition.x *= -1f;
			spritePrefabNode.globalRotation.z *= -1f;
			float minRotaryLimit = spritePrefabNode.minRotaryLimit;
			spritePrefabNode.minRotaryLimit = spritePrefabNode.maxRotaryLimit * -1f;
			spritePrefabNode.maxRotaryLimit = minRotaryLimit * -1f;
			if (spritePrefabNode.SC != null)
			{
				if (_c.flipX == -1)
				{
					SpriteS.SetFlip(spritePrefabNode.SC, true, false);
				}
				else
				{
					SpriteS.SetFlip(spritePrefabNode.SC, false, false);
				}
			}
			if (spritePrefabNode.parentIndex != -1)
			{
				SpritePrefabNode spritePrefabNode2 = _c.nodes[_c.nodes[i].parentIndex];
				if (spritePrefabNode.pivotJoint != IntPtr.Zero)
				{
					ChipmunkWrapper.SetPivotJointOffsetA(spritePrefabNode.pivotJoint, (Vector2)spritePrefabNode.localPosition - (Vector2)spritePrefabNode2.localCenter);
					ChipmunkWrapper.SetPivotJointOffsetB(spritePrefabNode.pivotJoint, -(Vector2)spritePrefabNode.localCenter);
				}
				else
				{
					TransformS.SetPosition(spritePrefabNode.TC, spritePrefabNode.localPosition - spritePrefabNode2.localCenter);
					SpriteS.SetOffset(spritePrefabNode.SC, spritePrefabNode.localCenter, 0f);
				}
				if (spritePrefabNode.rotarySpring != IntPtr.Zero)
				{
					ChipmunkWrapper.SetDampedRotarySpringProperties(spritePrefabNode.rotarySpring, spritePrefabNode.rotarySpringStrength, spritePrefabNode.rotarySpringDamp, (0f - spritePrefabNode.globalRotation.z) * ((float)Math.PI / 180f));
				}
				else
				{
					TransformS.SetRotation(spritePrefabNode.TC, spritePrefabNode.localRotation);
				}
				if (spritePrefabNode.rotaryLimitJoint != IntPtr.Zero)
				{
					ChipmunkWrapper.SetRotaryLimitJointProperties(spritePrefabNode.rotaryLimitJoint, spritePrefabNode.minRotaryLimit * ((float)Math.PI / 180f), spritePrefabNode.maxRotaryLimit * ((float)Math.PI / 180f));
				}
			}
		}
	}
}

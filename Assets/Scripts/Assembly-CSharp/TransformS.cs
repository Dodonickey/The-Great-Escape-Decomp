using System;
using System.Collections.Generic;
using UnityEngine;

public static class TransformS
{
	public static GenericArray<TransformC> m_components;

	public static GameObject m_transformHelper = new GameObject("TransformComponent");

	private static TransformC c;

	private static TransformC p;

	public static void Initialize(int _maxComponentCount)
	{
		m_components = new GenericArray<TransformC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			m_components.m_array[i] = new TransformC();
			GameObject gameObject = UnityEngine.Object.Instantiate(m_transformHelper) as GameObject;
			m_components.m_array[i].transform = gameObject.transform;
			m_components.m_array[i].transform.gameObject.active = false;
			m_components.m_array[i].transform.gameObject.active = false;
			m_components.m_array[i].componentType = ComponentType.Transform;
			m_components.m_array[i].index = i;
			ResetComponent(m_components.m_array[i]);
		}
	}

	public static void ResetComponent(TransformC c)
	{
		c.forceRotation = false;
		c.forceScale = false;
		c.parent = null;
		c.childs = new List<TransformC>();
		c.active = false;
		c.updatedPosition = false;
		c.updatedRotation = false;
		c.updatedScale = false;
		c.updatePosition = true;
		c.updateRotation = true;
		c.updateScale = true;
		c.transform.localScale = Vector3.one;
		c.transform.position = Vector3.zero;
		c.transform.localRotation = Quaternion.identity;
		c.forcedRotation = Quaternion.identity;
		c.forcedScale = Vector3.one;
		c.level = 0;
		c.parentedToPhysics = false;
		c.delta = Vector3.zero;
		c.lastPos = Vector3.zero;
	}

	public static TransformC AddComponent(Entity _entity)
	{
		int num = m_components.AddItem();
		TransformC transformC = m_components.m_array[num];
		ResetComponent(transformC);
		transformC.entityIndex = _entity.index;
		transformC.active = true;
		_entity.components.Add(transformC);
		transformC.transform.gameObject.active = true;
		return transformC;
	}

	public static TransformC AddComponent(int _entityIndex)
	{
		return AddComponent(EntityManager.m_entities.m_array[_entityIndex]);
	}

	public static void RemoveComponent(TransformC _c)
	{
		if (_c.parent != null)
		{
			_c.transform.parent = null;
			_c.parent.childs.Remove(_c);
			_c.parent = null;
		}
		while (_c.childs.Count > 0)
		{
			int index = _c.childs.Count - 1;
			_c.childs[index].parent = null;
			_c.childs[index].transform.parent = null;
			_c.childs.RemoveAt(index);
		}
		_c.transform.position = Vector3.zero;
		_c.transform.rotation = Quaternion.Euler(Vector3.zero);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		m_components.RemoveItem(_c.index);
		_c.entityIndex = -1;
		_c.transform.gameObject.active = false;
	}

	private static void UpdateChildHierarchyLevel(TransformC _parent)
	{
		for (int i = 0; i < _parent.childs.Count; i++)
		{
			_parent.childs[i].level = _parent.level + 1;
			UpdateChildHierarchyLevel(_parent.childs[i]);
		}
	}

	public static void ParentComponent(TransformC _c, TransformC _parent)
	{
		ParentComponent(_c, _parent, _c.transform.position - _parent.transform.position);
	}

	public static void ParentComponent(TransformC _c, TransformC _parent, Vector3 _childLocalPos)
	{
		if (_c.transform.parent != null)
		{
			UnparentComponent(_c);
		}
		_c.transform.parent = _parent.transform;
		_parent.childs.Add(_c);
		_c.parent = _parent;
		_c.updatePosition = true;
		_c.updateRotation = true;
		_c.updateScale = true;
		UpdateChildHierarchyLevel(_parent);
		SetPosition(_c, _childLocalPos);
		LoosenPhysicsConnections(_c);
	}

	public static void UnparentComponent(TransformC _c)
	{
		_c.parent.childs.Remove(_c);
		_c.parent = null;
		_c.transform.parent = null;
		_c.level = 0;
		_c.updatePosition = true;
		_c.updateRotation = true;
		_c.updateScale = true;
		UpdateChildHierarchyLevel(_c);
	}

	public static void LoosenPhysicsConnections(TransformC _c)
	{
		for (int i = 0; i < _c.childs.Count; i++)
		{
			LoosenPhysicsConnections(_c.childs[i]);
		}
		_c.lastPos = _c.transform.localPosition;
	}

	public static TransformC GetRootTransformComponent(TransformC _tc)
	{
		if (_tc.parent != null)
		{
			return GetRootTransformComponent(_tc.parent);
		}
		return _tc;
	}

	public static TransformC GetParentTransformComponent(TransformC _tc)
	{
		if (_tc.parent != null)
		{
			return _tc.parent;
		}
		return _tc;
	}

	public static void SetTransform(TransformC _c, Vector3 _position, Vector3 _rotation)
	{
		SetTransform(_c, _position, _rotation, IntPtr.Zero);
	}

	public static void SetTransform(TransformC _c, Vector3 _position, Vector3 _rotation, IntPtr _cpBodyPtr)
	{
		SetPosition(_c, _position, _cpBodyPtr);
		SetRotation(_c, _rotation, _cpBodyPtr);
	}

	public static void SetPosition(TransformC _c, Vector3 _position)
	{
		SetPosition(_c, _position, IntPtr.Zero);
	}

	public static void SetPosition(TransformC _c, Vector3 _position, IntPtr _cpBodyPtr)
	{
		_c.transform.localPosition = _position;
		_c.updatePosition = true;
		if (_cpBodyPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.SetPosition(_cpBodyPtr, _c.transform.position);
		}
	}

	public static void SetGlobalPositionWithoutChildren(TransformC _c, Vector3 _position)
	{
		SetGlobalPositionWithoutChildren(_c, _position, IntPtr.Zero);
	}

	public static void SetGlobalPositionWithoutChildren(TransformC _c, Vector3 _position, IntPtr _cpBodyPtr)
	{
		for (int i = 0; i < _c.childs.Count; i++)
		{
			_c.childs[i].transform.parent = null;
		}
		_c.transform.position = _position;
		_c.updatePosition = true;
		for (int j = 0; j < _c.childs.Count; j++)
		{
			_c.childs[j].transform.parent = _c.transform;
		}
		if (_cpBodyPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.SetPosition(_cpBodyPtr, _position);
		}
	}

	public static void SetGlobalPosition(TransformC _c, Vector3 _position)
	{
		SetGlobalPosition(_c, _position, IntPtr.Zero);
	}

	public static void SetGlobalPosition(TransformC _c, Vector3 _position, IntPtr _cpBodyPtr)
	{
		_c.transform.position = _position;
		_c.updatePosition = true;
		if (_cpBodyPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.SetPosition(_cpBodyPtr, _position);
		}
	}

	public static void Move(TransformC _c, Vector3 _step)
	{
		_c.transform.localPosition += _step;
		_c.updatePosition = true;
	}

	public static void GlobalMove(TransformC _c, Vector3 _step)
	{
		_c.transform.position += _step;
		_c.updatePosition = true;
	}

	public static void SetRotation(TransformC _c, Vector3 _rotation)
	{
		SetRotation(_c, _rotation, IntPtr.Zero);
	}

	public static void SetRotation(TransformC _c, Vector3 _rotation, IntPtr _cpBodyPtr)
	{
		if (_c.forceRotation)
		{
			_c.forcedRotation = Quaternion.Euler(_rotation);
		}
		else
		{
			_c.transform.localRotation = Quaternion.Euler(_rotation);
		}
		_c.updateRotation = true;
		if (_cpBodyPtr != IntPtr.Zero)
		{
			float num = _c.transform.rotation.eulerAngles.z;
			if (num > 180f)
			{
				num -= 360f;
			}
			ChipmunkWrapper.SetAngle(_cpBodyPtr, num * ((float)Math.PI / 180f));
		}
	}

	public static void SetGlobalRotation(TransformC _c, Vector3 _rotation)
	{
		SetGlobalRotation(_c, _rotation, IntPtr.Zero);
	}

	public static void SetGlobalRotation(TransformC _c, Vector3 _rotation, IntPtr _cpBodyPtr)
	{
		_c.transform.rotation = Quaternion.Euler(_rotation);
		_c.updateRotation = true;
		if (_cpBodyPtr != IntPtr.Zero)
		{
			float num = _c.transform.rotation.eulerAngles.z;
			if (num > 180f)
			{
				num -= 360f;
			}
			ChipmunkWrapper.SetAngle(_cpBodyPtr, num * ((float)Math.PI / 180f));
		}
	}

	public static void SetGlobalRotationWithoutChildren(TransformC _c, Vector3 _rotation)
	{
		SetGlobalRotationWithoutChildren(_c, _rotation, IntPtr.Zero);
	}

	public static void SetGlobalRotationWithoutChildren(TransformC _c, Vector3 _rotation, IntPtr _cpBodyPtr)
	{
		List<Vector3> list = new List<Vector3>();
		List<Quaternion> list2 = new List<Quaternion>();
		for (int i = 0; i < _c.childs.Count; i++)
		{
			list.Add(_c.childs[i].transform.position);
			list2.Add(_c.childs[i].transform.rotation);
			_c.childs[i].transform.parent = null;
		}
		_c.transform.rotation = Quaternion.Euler(_rotation);
		_c.updateRotation = true;
		for (int j = 0; j < _c.childs.Count; j++)
		{
			_c.childs[j].transform.parent = _c.transform;
			_c.childs[j].transform.position = list[j];
			_c.childs[j].transform.rotation = list2[j];
		}
		if (_cpBodyPtr != IntPtr.Zero)
		{
			float num = _c.transform.rotation.eulerAngles.z;
			if (num > 180f)
			{
				num -= 360f;
			}
			ChipmunkWrapper.SetAngle(_cpBodyPtr, num * ((float)Math.PI / 180f));
		}
	}

	public static Vector3 Rotate(TransformC _c, Vector3 _rotation)
	{
		if (_c.forceRotation)
		{
			_c.forcedRotation.eulerAngles += _rotation;
		}
		else
		{
			_c.transform.Rotate(_rotation);
		}
		_c.updateRotation = true;
		if (_c.forceRotation)
		{
			return _c.forcedRotation.eulerAngles;
		}
		return _c.transform.rotation.eulerAngles;
	}

	public static void SetScale(TransformC _c, Vector3 _scale)
	{
		if (_c.forceScale)
		{
			_c.forcedScale = _scale;
		}
		else
		{
			_c.transform.localScale = _scale;
		}
		_c.updateScale = true;
	}

	public static void SetScale(TransformC _c, float _scale)
	{
		if (_c.forceScale)
		{
			_c.forcedScale = Vector3.one * _scale;
		}
		else
		{
			_c.transform.localScale = Vector3.one * _scale;
		}
		_c.updateScale = true;
	}

	public static void Scale(TransformC _c, float _scale)
	{
		if (_c.forceScale)
		{
			_c.forcedScale *= _scale;
		}
		else
		{
			_c.transform.localScale *= _scale;
		}
		_c.updateScale = true;
	}

	public static void Scale(TransformC _c, Vector3 _scale)
	{
		if (_c.forceScale)
		{
			_c.forcedScale.x *= _scale.x;
			_c.forcedScale.y *= _scale.y;
			_c.forcedScale.z *= _scale.z;
		}
		else
		{
			Vector3 localScale = _c.transform.localScale;
			localScale.x *= _scale.x;
			localScale.y *= _scale.y;
			localScale.z *= _scale.z;
			_c.transform.localScale = localScale;
		}
		_c.updateScale = true;
	}

	public static void Update()
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			c = m_components.m_array[m_components.m_aliveIndices[i]];
			if (!c.active)
			{
				continue;
			}
			if (!c.updatePosition && c.updatedPosition)
			{
				c.updatedPosition = false;
			}
			if (!c.updateRotation && c.updatedRotation)
			{
				c.updatedRotation = false;
			}
			if (!c.updateScale && c.updatedScale)
			{
				c.updatedScale = false;
			}
			if (c.parent != null)
			{
				p = c.parent;
				if (p.updatedPosition)
				{
					c.updatePosition = true;
				}
				if (p.updatedRotation)
				{
					c.updatePosition = true;
					if (!c.forceRotation)
					{
						c.updateRotation = true;
					}
					else
					{
						c.transform.rotation = c.forcedRotation;
					}
				}
				if (p.updatedScale)
				{
					c.updatePosition = true;
					if (!c.forceScale)
					{
						c.updateScale = true;
					}
					else
					{
						c.transform.localScale = c.forcedScale;
					}
				}
				if (c.parentedToPhysics)
				{
					c.updatedPosition = true;
					c.delta = c.transform.localPosition - c.lastPos;
					SetPosition(c, c.delta);
					c.lastPos = c.transform.localPosition;
				}
			}
			if (c.updateRotation)
			{
				if (c.forceRotation)
				{
					c.transform.rotation = c.forcedRotation;
				}
				c.updateRotation = false;
				c.updatedRotation = true;
			}
			if (c.updateScale)
			{
				if (c.forceScale)
				{
					c.transform.localScale = c.forcedScale;
				}
				c.updateScale = false;
				c.updatedScale = true;
			}
			if (c.updatePosition)
			{
				c.updatePosition = false;
				c.updatedPosition = true;
			}
		}
	}
}

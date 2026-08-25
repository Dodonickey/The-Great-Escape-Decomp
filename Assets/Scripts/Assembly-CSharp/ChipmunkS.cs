using System;
using UnityEngine;

public static class ChipmunkS
{
	public static uint m_groundColliderGroup = 999999u;

	public static float m_chipmunkSlewDelta = 60f;

	public static GenericArray<ChipmunkC> m_components;

	public static CollisionInterestPair[][] m_collisionInterestArray;

	public static void Initialize(int _maxComponentCount, int _maxCollisionInterestPairs)
	{
		m_components = new GenericArray<ChipmunkC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			m_components.m_array[i] = new ChipmunkC();
			m_components.m_array[i].index = i;
			m_components.m_array[i].entityIndex = -1;
			m_components.m_array[i].componentType = ComponentType.Chipmunk;
			m_components.m_array[i].ucpBodyStruct = default(ChipmunkBodyStruct);
		}
		m_collisionInterestArray = new CollisionInterestPair[_maxCollisionInterestPairs][];
		for (int j = 0; j < m_collisionInterestArray.Length; j++)
		{
			m_collisionInterestArray[j] = new CollisionInterestPair[_maxCollisionInterestPairs];
		}
	}

	public static ChipmunkC AddInactiveComponent(TransformC _transformComponent, bool _transformComponentDictates, ColliderType _colliderType)
	{
		return AddInactiveComponent(_transformComponent, _transformComponentDictates, _colliderType, 0u, 1118481u, false, false);
	}

	public static ChipmunkC AddInactiveComponent(TransformC _transformComponent, bool _transformComponentDictates, ColliderType _colliderType, uint _colliderGroup, uint _colliderLayer)
	{
		return AddInactiveComponent(_transformComponent, _transformComponentDictates, _colliderType, _colliderGroup, _colliderLayer, false, false);
	}

	public static ChipmunkC AddInactiveComponent(TransformC _transformComponent, bool _transformComponentDictates, ColliderType _colliderType, bool _isStatic, bool _isRogue)
	{
		return AddInactiveComponent(_transformComponent, _transformComponentDictates, _colliderType, 0u, 1118481u, _isStatic, _isRogue);
	}

	public static ChipmunkC AddInactiveComponent(TransformC _transformComponent, bool _transformComponentDictates, ColliderType _colliderType, uint _colliderGroup, uint _colliderLayer, bool _isStatic, bool _isRogue)
	{
		int num = m_components.AddItem();
		ChipmunkC chipmunkC = m_components.m_array[num];
		chipmunkC.entityIndex = _transformComponent.entityIndex;
		chipmunkC.active = true;
		chipmunkC.cpBodyPtr = IntPtr.Zero;
		chipmunkC.ucpBodyStruct = default(ChipmunkBodyStruct);
		chipmunkC.TC = _transformComponent;
		chipmunkC.transformComponentDictates = _transformComponentDictates;
		chipmunkC.dictateAngle = true;
		chipmunkC.dictatePosition = true;
		chipmunkC.active = false;
		chipmunkC.colliderType = _colliderType;
		chipmunkC.isStatic = _isStatic;
		chipmunkC.isRogue = _isRogue;
		chipmunkC.colliderGroup = _colliderGroup;
		chipmunkC.colliderLayer = _colliderLayer;
		EntityManager.m_entities.m_array[chipmunkC.entityIndex].components.Add(chipmunkC);
		return chipmunkC;
	}

	public static IntPtr ActivateChipmunkComponent(ChipmunkC _chipmunkComponent, IntPtr _bodyPtr)
	{
		_chipmunkComponent.cpBodyPtr = _bodyPtr;
		ChipmunkWrapper.SetAngle(_bodyPtr, _chipmunkComponent.TC.transform.eulerAngles.z * ((float)Math.PI / 180f));
		ChipmunkWrapper.GetBodyValues(_bodyPtr, ref _chipmunkComponent.ucpBodyStruct);
		TransformS.SetGlobalPosition(_chipmunkComponent.TC, new Vector3(_chipmunkComponent.ucpBodyStruct.p.x, _chipmunkComponent.ucpBodyStruct.p.y, _chipmunkComponent.TC.transform.position.z));
		_chipmunkComponent.active = true;
		return _bodyPtr;
	}

	public static void SetCustomComponent(ChipmunkC _cmc, IComponent _customComponent)
	{
		_cmc.customComponent = _customComponent;
	}

	public static void RemoveComponent(ChipmunkC _c)
	{
		_c.TC = null;
		_c.customComponent = null;
		if (_c.cpBodyPtr == IntPtr.Zero)
		{
			Debug.LogError("Removing chipmunk component with zero chipmunk body IntPtr, have you assigned chipmunk body to it with ActivateChipmunkComponent?");
		}
		ChipmunkWrapper.RemoveBody(_c.cpBodyPtr);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		m_components.RemoveItem(_c.index);
		_c.entityIndex = -1;
		_c.colliderGroup = 0u;
		_c.colliderLayer = 17895697u;
	}

	public static void AddCollisionInterest(bool _begin, bool _persist, bool _separate, ColliderType _colliderA, ColliderType _colliderB, CollisionEventDelegate _collisionDelegate)
	{
		CollisionInterestPair collisionInterestPair = new CollisionInterestPair(_colliderA, _colliderB, _collisionDelegate);
		m_collisionInterestArray[(int)_colliderA][(int)_colliderB] = collisionInterestPair;
		m_collisionInterestArray[(int)_colliderB][(int)_colliderA] = collisionInterestPair;
		if (_colliderA == ColliderType.Any)
		{
			for (int i = 0; i < m_collisionInterestArray.Length; i++)
			{
				m_collisionInterestArray[i][(int)_colliderB] = collisionInterestPair;
			}
		}
		if (_colliderB == ColliderType.Any)
		{
			for (int j = 0; j < m_collisionInterestArray.Length; j++)
			{
				m_collisionInterestArray[(int)_colliderA][j] = collisionInterestPair;
			}
		}
		if (_begin)
		{
			ChipmunkWrapper.AddCollisionInterestPair(ChipmunkCollisionList.BEGIN, _colliderA, _colliderB);
		}
		if (_persist)
		{
			ChipmunkWrapper.AddCollisionInterestPair(ChipmunkCollisionList.PERSIST, _colliderA, _colliderB);
		}
		if (_separate)
		{
			ChipmunkWrapper.AddCollisionInterestPair(ChipmunkCollisionList.SEPARATE, _colliderA, _colliderB);
		}
	}

	public static void ClearCollisionInterests()
	{
		for (int i = 0; i < m_collisionInterestArray.Length; i++)
		{
			for (int j = 0; j < m_collisionInterestArray[i].Length; j++)
			{
				m_collisionInterestArray[i][j] = null;
			}
		}
		ChipmunkWrapper.ClearCollisionInterestPairs();
	}

	public static void CreateSegmentShapesFromPolygon(ChipmunkC _cmc, Polygon _polygon, uint _layer)
	{
		CreateSegmentShapesFromPolygon(_cmc, _polygon, _layer, 0.5f, 0.5f, 0f);
	}

	public static void CreateSegmentShapesFromPolygon(ChipmunkC _cmc, Polygon _polygon, uint _layer, float _segmentWidth)
	{
		CreateSegmentShapesFromPolygon(_cmc, _polygon, _layer, 0.5f, 0.5f, _segmentWidth);
	}

	public static void CreateSegmentShapesFromPolygon(ChipmunkC _cmc, Polygon _polygon, uint _layer, float _restitution, float _friction, float _segmentWidth)
	{
		CreateSegmentShapesFromPolygon(_cmc, _polygon, _layer, _restitution, _friction, _segmentWidth, Vector2.zero, true);
	}

	public static void CreateSegmentShapesFromPolygon(ChipmunkC _cmc, Polygon _polygon, uint _layer, float _restitution, float _friction, float _segmentWidth, Vector2 _offset, bool _removeShapesFromBody)
	{
		if (_polygon == null || _polygon.Contour == null)
		{
			return;
		}
		if (_removeShapesFromBody)
		{
			ChipmunkWrapper.RemoveShapesFromBody(_cmc.cpBodyPtr);
		}
		if (_segmentWidth != 0f)
		{
			_polygon = GpcS.ScalePolygon(_polygon, 0f - _segmentWidth);
		}
		for (int i = 0; i < _polygon.NofContours; i++)
		{
			GraphicsPath graphicsPath = _polygon.Contour[i].ToGraphicsPath();
			Vector2[] pathPoints = graphicsPath.PathPoints;
			for (int j = 0; j < pathPoints.Length; j++)
			{
				Vector2 b = pathPoints[j] - _offset;
				ChipmunkWrapper.AddSegmentShape(a: (j + 1 >= pathPoints.Length) ? (pathPoints[0] - _offset) : (pathPoints[j + 1] - _offset), bodyPtr: _cmc.cpBodyPtr, b: b, radius: _segmentWidth, restitution: _restitution, friction: _friction, collisionGroup: m_groundColliderGroup, layers: _layer, sensor: false);
			}
		}
	}

	public static void CreateSegmentShapesFromVectorArray(ChipmunkC _cmc, Vector2[] _vectorArray, uint _layer, float _restitution, float _friction, float _segmentWidth, Vector2 _offset, bool _removeShapesFromBody)
	{
		if (_vectorArray != null)
		{
			if (_removeShapesFromBody)
			{
				ChipmunkWrapper.RemoveShapesFromBody(_cmc.cpBodyPtr);
			}
			for (int i = 0; i < _vectorArray.Length - 1; i++)
			{
				Vector2 b = _vectorArray[i] - _offset;
				Vector2 a = _vectorArray[i + 1] - _offset;
				ChipmunkWrapper.AddSegmentShape(_cmc.cpBodyPtr, a, b, _segmentWidth, _restitution, _friction, 0u, _layer, false);
			}
		}
	}

	public static void CreatePolyShapesFromPolygon(ChipmunkC _cmc, Polygon _polygon, float _mass, uint _group, uint _layer, bool _sensor, bool _removeOld)
	{
		CreatePolyShapesFromPolygon(_cmc, _polygon, _mass, _group, _layer, 0.5f, 0.5f, _sensor, _removeOld);
	}

	public static void CreatePolyShapesFromPolygon(ChipmunkC _cmc, Polygon _polygon, float _mass, uint _group, uint _layer, float _restitution, float _friction, bool _sensor, bool _removeOld)
	{
		if (_removeOld)
		{
			ChipmunkWrapper.RemoveShapesFromBody(_cmc.cpBodyPtr);
		}
		Tristrip tristrip = _polygon.ToTristrip();
		for (int i = 0; i < tristrip.NofStrips; i++)
		{
			VertexList vertexList = tristrip.Strip[i];
			int num = -1;
			for (int j = 0; j < vertexList.NofVertices - 2; j++)
			{
				Vector2[] array = new Vector2[3];
				if (num == -1)
				{
					array[0] = vertexList.Vertex[j];
					array[1] = vertexList.Vertex[j + 2];
					array[2] = vertexList.Vertex[j + 1];
					num *= -1;
				}
				else
				{
					array[0] = vertexList.Vertex[j];
					array[1] = vertexList.Vertex[j + 1];
					array[2] = vertexList.Vertex[j + 2];
					num *= -1;
				}
				IntPtr intPtr = ChipmunkWrapper.AddPolyShape(_cmc.cpBodyPtr, Vector2.zero, _mass / (float)tristrip.NofStrips, 3, array, _restitution, _friction, _group, _layer, _sensor);
			}
		}
	}

	public static bool IsBodyColliding(IntPtr _bodyPtr)
	{
		int collisionList = ChipmunkWrapper.GetCollisionList(ChipmunkCollisionList.PERSIST, ChipmunkWrapper.persistList);
		for (int i = 0; i < collisionList; i++)
		{
			ChipmunkCollisionPair chipmunkCollisionPair = ChipmunkWrapper.persistList[i];
			if (m_components.m_array[chipmunkCollisionPair.componentIndexA].cpBodyPtr == _bodyPtr)
			{
				return true;
			}
			if (m_components.m_array[chipmunkCollisionPair.componentIndexB].cpBodyPtr == _bodyPtr)
			{
				return true;
			}
		}
		return false;
	}

	public static ChipmunkC GetCollidingBody(IntPtr _bodyPtr)
	{
		int collisionList = ChipmunkWrapper.GetCollisionList(ChipmunkCollisionList.PERSIST, ChipmunkWrapper.persistList);
		for (int i = 0; i < collisionList; i++)
		{
			ChipmunkCollisionPair chipmunkCollisionPair = ChipmunkWrapper.persistList[i];
			if (m_components.m_array[chipmunkCollisionPair.componentIndexA].cpBodyPtr == _bodyPtr)
			{
				return m_components.m_array[chipmunkCollisionPair.componentIndexB];
			}
			if (m_components.m_array[chipmunkCollisionPair.componentIndexB].cpBodyPtr == _bodyPtr)
			{
				return m_components.m_array[chipmunkCollisionPair.componentIndexA];
			}
		}
		return null;
	}

	public static void SetColliderType(ChipmunkC _c, ColliderType _colliderType)
	{
		_c.colliderType = _colliderType;
		ChipmunkWrapper.SetBodyColliderType(_c.cpBodyPtr, _colliderType);
	}

	public static void Update(float _dt)
	{
		ChipmunkWrapper.UpdateWorld(_dt);
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			ChipmunkC chipmunkC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (!chipmunkC.active)
			{
				continue;
			}
			Vector2 p = chipmunkC.ucpBodyStruct.p;
			ChipmunkWrapper.GetBodyValues(chipmunkC.cpBodyPtr, ref chipmunkC.ucpBodyStruct);
			if (chipmunkC.transformComponentDictates)
			{
				if (chipmunkC.isStatic)
				{
					continue;
				}
				if (chipmunkC.dictatePosition)
				{
					if (chipmunkC.isRogue)
					{
						Vector2 vector = (Vector2)chipmunkC.TC.transform.position - chipmunkC.ucpBodyStruct.p;
						ChipmunkWrapper.SetVelocity(chipmunkC.cpBodyPtr, vector * m_chipmunkSlewDelta);
						ChipmunkWrapper.UpdateBodyPosition(chipmunkC.cpBodyPtr);
					}
					else
					{
						Vector2 vector2 = (Vector2)chipmunkC.TC.transform.position - chipmunkC.ucpBodyStruct.p;
						ChipmunkWrapper.SetVelocity(chipmunkC.cpBodyPtr, vector2 * m_chipmunkSlewDelta);
					}
				}
				if (chipmunkC.dictateAngle)
				{
					float num = 0f;
					num = ((!chipmunkC.TC.forceRotation) ? chipmunkC.TC.transform.eulerAngles.z : chipmunkC.TC.forcedRotation.eulerAngles.z);
					if (num > 180f)
					{
						num -= 360f;
					}
					ChipmunkWrapper.SetAngle(chipmunkC.cpBodyPtr, num * ((float)Math.PI / 180f));
				}
			}
			else
			{
				if (chipmunkC.isStatic)
				{
					continue;
				}
				if (chipmunkC.dictatePosition)
				{
					Vector3 position;
					if (chipmunkC.isRogue)
					{
						Vector2 vector3 = chipmunkC.ucpBodyStruct.p - p;
						position = new Vector3(chipmunkC.ucpBodyStruct.p.x + vector3.x, chipmunkC.ucpBodyStruct.p.y + vector3.y, chipmunkC.TC.transform.position.z);
					}
					else
					{
						position = new Vector3(chipmunkC.ucpBodyStruct.p.x, chipmunkC.ucpBodyStruct.p.y, chipmunkC.TC.transform.position.z);
					}
					if (chipmunkC.TC != null && position.x > -999999f)
					{
						TransformS.SetGlobalPosition(chipmunkC.TC, position);
					}
				}
				else
				{
					Vector2 vector4 = (Vector2)chipmunkC.TC.transform.position - chipmunkC.ucpBodyStruct.p;
					ChipmunkWrapper.SetVelocity(chipmunkC.cpBodyPtr, vector4 * m_chipmunkSlewDelta);
				}
				if (chipmunkC.dictateAngle)
				{
					Vector3 rotation = new Vector3(0f, 0f, chipmunkC.ucpBodyStruct.a * 57.29578f);
					if (chipmunkC.TC != null && rotation.z > -999999f)
					{
						TransformS.SetGlobalRotation(chipmunkC.TC, rotation);
					}
					continue;
				}
				float num2 = 0f;
				num2 = ((!chipmunkC.TC.forceRotation) ? chipmunkC.TC.transform.eulerAngles.z : chipmunkC.TC.forcedRotation.eulerAngles.z);
				if (num2 > 180f)
				{
					num2 -= 360f;
				}
				num2 *= (float)Math.PI / 180f;
				float num3 = num2 - chipmunkC.ucpBodyStruct.a;
				ChipmunkWrapper.SetAngularVelocity(chipmunkC.cpBodyPtr, num3 * m_chipmunkSlewDelta);
			}
		}
		int collisionInterestList = ChipmunkWrapper.GetCollisionInterestList(ChipmunkCollisionList.BEGIN, ChipmunkWrapper.beginList);
		int collisionInterestList2 = ChipmunkWrapper.GetCollisionInterestList(ChipmunkCollisionList.PERSIST, ChipmunkWrapper.persistList);
		int collisionInterestList3 = ChipmunkWrapper.GetCollisionInterestList(ChipmunkCollisionList.SEPARATE, ChipmunkWrapper.separateList);
		for (int j = 0; j < collisionInterestList; j++)
		{
			ChipmunkCollisionPair collisionPair = ChipmunkWrapper.beginList[j];
			ChipmunkC chipmunkC2 = m_components.m_array[collisionPair.componentIndexA];
			ChipmunkC chipmunkC3 = m_components.m_array[collisionPair.componentIndexB];
			if (m_collisionInterestArray[(int)chipmunkC2.colliderType][(int)chipmunkC3.colliderType] != null)
			{
				m_collisionInterestArray[(int)chipmunkC2.colliderType][(int)chipmunkC3.colliderType].collisionDelegate(collisionPair, ChipmunkCollisionList.BEGIN);
			}
			else
			{
				m_collisionInterestArray[(int)chipmunkC3.colliderType][(int)chipmunkC2.colliderType].collisionDelegate(collisionPair, ChipmunkCollisionList.BEGIN);
			}
		}
		for (int k = 0; k < collisionInterestList2; k++)
		{
			ChipmunkCollisionPair collisionPair2 = ChipmunkWrapper.persistList[k];
			if (collisionPair2.componentIndexA > -1 && collisionPair2.componentIndexB > -1)
			{
				ChipmunkC chipmunkC4 = m_components.m_array[collisionPair2.componentIndexA];
				ChipmunkC chipmunkC5 = m_components.m_array[collisionPair2.componentIndexB];
				if (m_collisionInterestArray[(int)chipmunkC4.colliderType][(int)chipmunkC5.colliderType] != null)
				{
					m_collisionInterestArray[(int)chipmunkC4.colliderType][(int)chipmunkC5.colliderType].collisionDelegate(collisionPair2, ChipmunkCollisionList.PERSIST);
				}
				else
				{
					m_collisionInterestArray[(int)chipmunkC5.colliderType][(int)chipmunkC4.colliderType].collisionDelegate(collisionPair2, ChipmunkCollisionList.PERSIST);
				}
			}
		}
		for (int l = 0; l < collisionInterestList3; l++)
		{
			ChipmunkCollisionPair collisionPair3 = ChipmunkWrapper.separateList[l];
			ChipmunkC chipmunkC6 = m_components.m_array[collisionPair3.componentIndexA];
			ChipmunkC chipmunkC7 = m_components.m_array[collisionPair3.componentIndexB];
			if (m_collisionInterestArray[(int)chipmunkC6.colliderType][(int)chipmunkC7.colliderType] != null)
			{
				m_collisionInterestArray[(int)chipmunkC6.colliderType][(int)chipmunkC7.colliderType].collisionDelegate(collisionPair3, ChipmunkCollisionList.SEPARATE);
			}
			else
			{
				m_collisionInterestArray[(int)chipmunkC7.colliderType][(int)chipmunkC6.colliderType].collisionDelegate(collisionPair3, ChipmunkCollisionList.SEPARATE);
			}
		}
	}
}

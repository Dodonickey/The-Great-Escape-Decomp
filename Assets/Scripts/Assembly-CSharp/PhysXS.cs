public static class PhysXS
{
	public static GenericArray<PhysXC> m_components;

	public static void Initialize(int _maxComponentCount)
	{
		m_components = new GenericArray<PhysXC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			m_components.m_array[i] = new PhysXC();
			m_components.m_array[i].index = i;
			m_components.m_array[i].entityIndex = -1;
			m_components.m_array[i].componentType = ComponentType.PhysX;
		}
	}

	public static PhysXC AddComponent(TransformC _transformComponent, bool _transformComponentDictates, ColliderType _colliderType, uint _colliderGroup, uint _colliderLayer, bool _isStatic, bool _isRogue)
	{
		int num = m_components.AddItem();
		PhysXC physXC = m_components.m_array[num];
		physXC.entityIndex = _transformComponent.entityIndex;
		physXC.active = true;
		physXC.TC = _transformComponent;
		physXC.transformComponentDictates = _transformComponentDictates;
		physXC.dictateAngle = true;
		physXC.dictatePosition = true;
		physXC.active = false;
		physXC.colliderType = _colliderType;
		physXC.isStatic = _isStatic;
		physXC.isRogue = _isRogue;
		physXC.colliderGroup = _colliderGroup;
		physXC.colliderLayer = _colliderLayer;
		EntityManager.m_entities.m_array[physXC.entityIndex].components.Add(physXC);
		return physXC;
	}

	public static void SetCustomComponent(ChipmunkC _cmc, IComponent _customComponent)
	{
		_cmc.customComponent = _customComponent;
	}

	public static void RemoveComponent(PhysXC _c)
	{
		_c.TC = null;
		_c.customComponent = null;
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		m_components.RemoveItem(_c.index);
		_c.entityIndex = -1;
		_c.colliderGroup = 0u;
		_c.colliderLayer = 17895697u;
	}
}

using System.Collections.Generic;

public static class EntityManager
{
	public static GenericArray<Entity> m_entities;

	private static List<Entity> m_removeList;

	private static List<Tag> m_tags;

	public static void Initialize(int _maxEntityCount)
	{
		m_entities = new GenericArray<Entity>(_maxEntityCount);
		m_removeList = new List<Entity>();
		m_tags = new List<Tag>();
		for (int i = 0; i < _maxEntityCount; i++)
		{
			m_entities.m_array[i] = new Entity();
			m_entities.m_array[i].index = i;
			m_entities.m_array[i].components = new List<IComponent>();
			m_entities.m_array[i].persistent = false;
		}
	}

	public static Entity AddEntity()
	{
		string[] tags = new string[0];
		return AddEntity(tags);
	}

	public static Entity AddEntity(string _tag)
	{
		string[] tags = new string[1] { _tag };
		return AddEntity(tags);
	}

	public static Entity AddEntity(string[] _tags)
	{
		int num = m_entities.AddItem();
		Entity entity = m_entities.m_array[num];
		entity.persistent = false;
		if (_tags != null)
		{
			for (int i = 0; i < _tags.Length; i++)
			{
				AddTagForEntity(entity, _tags[i]);
			}
		}
		return entity;
	}

	public static TransformC AddEntityWithTC()
	{
		string[] tags = new string[0];
		return AddEntityWithTC(tags);
	}

	public static TransformC AddEntityWithTC(string _tag)
	{
		string[] tags = new string[1] { _tag };
		return AddEntityWithTC(tags);
	}

	public static TransformC AddEntityWithTC(string[] _tags)
	{
		Entity entity = AddEntity(_tags);
		return TransformS.AddComponent(entity);
	}

	public static void RemoveEntity(Entity _e)
	{
		RemoveEntity(_e, true);
	}

	public static void RemoveEntity(int _index)
	{
		RemoveEntity(_index, false);
	}

	public static void RemoveEntity(int _index, bool _removeImmediately)
	{
		Entity e = m_entities.m_array[_index];
		RemoveEntity(e, true, _removeImmediately);
	}

	public static void RemoveEntitiesByTransformComponentHierarchy(TransformC _tc, bool _removeParents)
	{
		RemoveEntitiesByTransformComponentHierarchy(_tc, _removeParents, false);
	}

	public static void RemoveEntitiesByTransformComponentHierarchy(TransformC _tc, bool _removeParents, bool _removeImmediately)
	{
		if (_tc != null)
		{
			if (_removeParents)
			{
				_tc = TransformS.GetRootTransformComponent(_tc);
			}
			while (_tc.childs.Count > 0)
			{
				int index = _tc.childs.Count - 1;
				RemoveEntitiesByTransformComponentHierarchy(_tc.childs[index], false, _removeImmediately);
				_tc.childs.RemoveAt(index);
			}
			Entity e = m_entities.m_array[_tc.entityIndex];
			RemoveEntity(e, true, _removeImmediately);
		}
	}

	public static void RemoveEntityByTransformComponent(TransformC _tc, bool _removeChildren)
	{
		if (_removeChildren)
		{
			while (_tc.childs.Count > 0)
			{
				RemoveEntitiesByTransformComponentHierarchy(_tc.childs[0], false);
				_tc.childs.Remove(_tc.childs[0]);
			}
		}
		Entity e = m_entities.m_array[_tc.entityIndex];
		RemoveEntity(e);
	}

	public static void RemoveEntity(Entity _e, bool _removeFromList)
	{
		RemoveEntity(_e, _removeFromList, false);
	}

	public static void RemoveEntity(Entity _e, bool _removeFromList, bool _removeImmediately)
	{
		if (!_removeImmediately)
		{
			m_removeList.Add(_e);
			return;
		}
		RemoveAllTagsFromEntity(_e);
		List<IComponent> list = new List<IComponent>();
		for (int i = 0; i < _e.components.Count; i++)
		{
			list.Add(_e.components[i]);
		}
		for (int j = 0; j < list.Count; j++)
		{
			IComponent component = list[j];
			switch (component.componentType)
			{
			case ComponentType.CameraBorder:
				CameraS.RemoveBorderComponent(component as CameraBorderC);
				break;
			case ComponentType.CameraTarget:
				CameraS.RemoveTargetComponent(component as CameraTargetC);
				break;
			case ComponentType.Chipmunk:
				ChipmunkS.RemoveComponent(component as ChipmunkC);
				break;
			case ComponentType.Event:
				EventS.RemoveComponent(component as EventC);
				break;
			case ComponentType.Gpc:
				GpcS.RemoveComponent(component as GpcC);
				break;
			case ComponentType.Prefab:
				PrefabS.RemoveComponent(component as PrefabC);
				break;
			case ComponentType.Sound:
				SoundS.RemoveComponent(component as SoundC);
				break;
			case ComponentType.Sprite:
				SpriteS.RemoveComponent(component as SpriteC);
				break;
			case ComponentType.Text:
				TextS.RemoveComponent(component as TextC);
				break;
			case ComponentType.TouchArea:
				TouchAreaS.RemoveComponent(component as TouchAreaC);
				break;
			case ComponentType.Transform:
				TransformS.RemoveComponent(component as TransformC);
				break;
			case ComponentType.Tween:
				TweenS.RemoveComponent(component as TweenC);
				break;
			case ComponentType.UI:
				UIS.RemoveComponent(component as UIC);
				break;
			default:
				Main.m_currentGame.RemoveComponent(component);
				break;
			}
		}
		list = null;
		if (_removeFromList)
		{
			_e.persistent = false;
			m_entities.RemoveItem(_e.index);
		}
	}

	public static void RemoveAllEntities()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < m_entities.m_aliveCount; i++)
		{
			Entity entity = m_entities.m_array[m_entities.m_aliveIndices[i]];
			if (!entity.persistent)
			{
				RemoveEntity(entity, false, true);
				list.Add(entity.index);
			}
		}
		while (list.Count > 0)
		{
			m_entities.RemoveItem(list[0]);
			list.RemoveAt(0);
		}
	}

	public static void RemoveEntitiesByTag(string _tag)
	{
		RemoveEntitiesByTag(_tag, false);
	}

	public static void RemoveEntitiesByTag(string _tag, bool _removeImmediately)
	{
		List<Entity> list = new List<Entity>();
		for (int num = m_tags.Count - 1; num > -1; num--)
		{
			if (m_tags[num].tag == _tag)
			{
				list.Add(m_tags[num].entity);
			}
		}
		while (list.Count > 0)
		{
			int index = list.Count - 1;
			RemoveEntity(list[index], true, _removeImmediately);
			list.RemoveAt(index);
		}
	}

	public static Entity GetEntityByIndex(int _entityIndex)
	{
		return m_entities.m_array[_entityIndex];
	}

	public static List<Entity> GetEntitiesByTag(string _tag)
	{
		List<Entity> list = new List<Entity>();
		for (int i = 0; i < m_tags.Count; i++)
		{
			if (m_tags[i].tag == _tag)
			{
				list.Add(m_tags[i].entity);
			}
		}
		return list;
	}

	public static List<IComponent> GetComponentsByEntityIndex(ComponentType _componentType, int _index)
	{
		List<IComponent> list = new List<IComponent>();
		Entity entity = m_entities.m_array[_index];
		for (int i = 0; i < entity.components.Count; i++)
		{
			if (entity.components[i].componentType == _componentType)
			{
				list.Add(entity.components[i]);
			}
		}
		return list;
	}

	public static List<IComponent> GetComponentsByType(ComponentType _componentType)
	{
		List<IComponent> list = new List<IComponent>();
		for (int i = 0; i < m_entities.m_aliveCount; i++)
		{
			Entity entity = m_entities.m_array[m_entities.m_aliveIndices[i]];
			for (int j = 0; j < entity.components.Count; j++)
			{
				if (entity.components[j].componentType == _componentType)
				{
					list.Add(entity.components[j]);
				}
			}
		}
		return list;
	}

	public static void AddTagForEntity(Entity _entity, string _tag)
	{
		Tag item = new Tag(_tag, _entity);
		m_tags.Add(item);
	}

	public static void RemoveTagFromEntity(Entity _entity, string _tag)
	{
		for (int num = m_tags.Count - 1; num > -1; num--)
		{
			if (m_tags[num].entity == _entity && m_tags[num].tag == _tag)
			{
				m_tags.RemoveAt(num);
				break;
			}
		}
	}

	public static void RemoveAllTagsFromEntity(Entity _entity)
	{
		for (int num = m_tags.Count - 1; num > -1; num--)
		{
			if (m_tags[num].entity == _entity)
			{
				m_tags.RemoveAt(num);
			}
		}
	}

	public static void SetVisibilityOfEntity(int _entityIndex, bool _visible)
	{
		Entity entity = m_entities.m_array[_entityIndex];
		for (int i = 0; i < entity.components.Count; i++)
		{
			if (entity.components[i].componentType == ComponentType.Sprite)
			{
				SpriteC spriteC = entity.components[i] as SpriteC;
				if (spriteC.isVisible)
				{
					SpriteS.SetVisibility(spriteC, _visible, false);
				}
			}
			else if (entity.components[i].componentType == ComponentType.Prefab)
			{
				PrefabC prefabC = entity.components[i] as PrefabC;
				if (prefabC.isVisible)
				{
					PrefabS.SetVisibility(prefabC, _visible, false);
				}
			}
		}
	}

	public static void SetActivityOfEntity(int _entityIndex, bool _active, bool _affectSpriteAndPrefabVisibility)
	{
		Entity entity = m_entities.m_array[_entityIndex];
		for (int i = 0; i < entity.components.Count; i++)
		{
			entity.components[i].active = _active;
			if (!_affectSpriteAndPrefabVisibility)
			{
				continue;
			}
			if (entity.components[i].componentType == ComponentType.Sprite)
			{
				SpriteC spriteC = entity.components[i] as SpriteC;
				if (spriteC.isVisible)
				{
					SpriteS.SetVisibility(spriteC, _active, false);
				}
			}
			else if (entity.components[i].componentType == ComponentType.Prefab)
			{
				PrefabC prefabC = entity.components[i] as PrefabC;
				if (prefabC.isVisible)
				{
					PrefabS.SetVisibility(prefabC, _active, false);
				}
			}
		}
	}

	public static void SetActivityOfAllEntities(bool _active, bool _affectSpriteAndPrefabVisibility)
	{
		int aliveCount = m_entities.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			Entity entity = m_entities.m_array[m_entities.m_aliveIndices[i]];
			for (int j = 0; j < entity.components.Count; j++)
			{
				entity.components[j].active = _active;
				if (!_affectSpriteAndPrefabVisibility)
				{
					continue;
				}
				if (entity.components[j].componentType == ComponentType.Sprite)
				{
					SpriteC spriteC = entity.components[j] as SpriteC;
					if (spriteC.isVisible)
					{
						SpriteS.SetVisibility(spriteC, _active, false);
					}
				}
				else if (entity.components[j].componentType == ComponentType.Prefab)
				{
					PrefabC prefabC = entity.components[j] as PrefabC;
					if (prefabC.isVisible)
					{
						PrefabS.SetVisibility(prefabC, _active, false);
					}
				}
			}
		}
	}

	public static void SetActivityOfEntitiesWithTag(string _tag, bool _active, bool _affectSpriteAndPrefabVisibility)
	{
		for (int i = 0; i < m_tags.Count; i++)
		{
			if (!(m_tags[i].tag == _tag))
			{
				continue;
			}
			Entity entity = m_tags[i].entity;
			for (int j = 0; j < entity.components.Count; j++)
			{
				entity.components[j].active = _active;
				if (!_affectSpriteAndPrefabVisibility)
				{
					continue;
				}
				if (entity.components[j].componentType == ComponentType.Sprite)
				{
					SpriteC spriteC = entity.components[j] as SpriteC;
					if (spriteC.isVisible)
					{
						SpriteS.SetVisibility(spriteC, _active, false);
					}
				}
				else if (entity.components[j].componentType == ComponentType.Prefab)
				{
					PrefabC prefabC = entity.components[j] as PrefabC;
					if (prefabC.isVisible)
					{
						PrefabS.SetVisibility(prefabC, _active, false);
					}
				}
			}
		}
	}

	public static void Update()
	{
		while (m_removeList.Count > 0)
		{
			int index = m_removeList.Count - 1;
			RemoveEntity(m_removeList[index], true, true);
			m_removeList.RemoveAt(index);
		}
	}
}

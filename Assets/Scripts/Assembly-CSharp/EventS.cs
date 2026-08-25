using System;
using System.Collections;
using System.Collections.Generic;

public static class EventS
{
	private static GenericArray<EventC> m_components;

	private static List<EventC> removeList;

	private static List<EventC> delegateList;

	public static void Initialize(int _maxComponentCount)
	{
		m_components = new GenericArray<EventC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			m_components.m_array[i] = new EventC();
			m_components.m_array[i].entityIndex = -1;
			m_components.m_array[i].index = i;
			m_components.m_array[i].componentType = ComponentType.Event;
			m_components.m_array[i].properties = new Hashtable();
			m_components.m_array[i].eventDelegate = DelegatedEventDebugMethod;
		}
		removeList = new List<EventC>();
		delegateList = new List<EventC>();
	}

	public static EventC AddComponent(int _entityIndex, string _identifier, EventDelegate _eventHandler, float _delay, bool _dispatchAutomaticly, bool _removeAfterDelegate, bool _delegateAtDestroy, bool _delegateOnlyOnce)
	{
		int num = m_components.AddItem();
		EventC eventC = m_components.m_array[num];
		eventC.entityIndex = _entityIndex;
		eventC.active = true;
		eventC.identifier = _identifier;
		eventC.delay = _delay;
		eventC.dispatched = _dispatchAutomaticly;
		eventC.startTime = Main.m_gameTime;
		eventC.count = 0;
		eventC.removeAfterDelegate = _removeAfterDelegate;
		eventC.delegateAtRemove = _delegateAtDestroy;
		eventC.delegateOnlyOnce = _delegateOnlyOnce;
		AddEventListener(eventC, _eventHandler);
		if (_entityIndex != -1)
		{
			EntityManager.m_entities.m_array[eventC.entityIndex].components.Add(eventC);
		}
		return eventC;
	}

	public static void RemoveComponent(EventC _c)
	{
		_c.active = false;
		_c.properties = new Hashtable();
		if (_c.delegateAtRemove && (!_c.delegateOnlyOnce || _c.count == 0))
		{
			_c.eventDelegate(_c);
		}
		if (_c.eventDelegate != null)
		{
			Delegate[] invocationList = _c.eventDelegate.GetInvocationList();
			Delegate[] array = invocationList;
			foreach (Delegate obj in array)
			{
				_c.eventDelegate = (EventDelegate)Delegate.Remove(_c.eventDelegate, (EventDelegate)obj);
			}
		}
		_c.delegatedCount = 0;
		_c.count = 0;
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		m_components.RemoveItem(_c.index);
		_c.entityIndex = -1;
	}

	public static EventC FindEventComponent(string _identifier)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EventC eventC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (eventC.identifier == _identifier)
			{
				return eventC;
			}
		}
		return null;
	}

	public static EventC FindEventComponent(int _entityIndex)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EventC eventC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (eventC.entityIndex == _entityIndex)
			{
				return eventC;
			}
		}
		return null;
	}

	public static void AddProperty(EventC _c, string _key, object _value)
	{
		_c.properties.Add(_key, _value);
	}

	public static void AddEventListener(EventC _c, EventDelegate _eventHandler)
	{
		if (_c.delegatedCount == 0)
		{
			_c.eventDelegate = _eventHandler;
		}
		else
		{
			_c.eventDelegate = (EventDelegate)Delegate.Combine(_c.eventDelegate, _eventHandler);
		}
		_c.delegatedCount++;
	}

	public static void AddEventListener(string identifier, EventDelegate _eventHandler, float _delay, bool _dispatchAutomaticly, bool _removeAfterDelegate, bool _delegateAtDestroy, bool _delegateOnlyOnce)
	{
		EventC eventC = AddComponent(-1, identifier, _eventHandler, _delay, _dispatchAutomaticly, _removeAfterDelegate, _delegateAtDestroy, _delegateOnlyOnce);
	}

	public static void RemoveEventListener(EventC _c, EventDelegate _eventHandler)
	{
		if (_c.count > 0)
		{
			_c.eventDelegate = (EventDelegate)Delegate.Remove(_c.eventDelegate, _eventHandler);
			_c.delegatedCount--;
		}
		if (_c.delegatedCount == 0)
		{
			_c.eventDelegate = DelegatedEventDebugMethod;
		}
	}

	private static void DelegatedEventDebugMethod(EventC _c)
	{
		Debug.Log(_c.identifier + ":\n");
		foreach (string key in _c.properties.Keys)
		{
			Debug.Log(string.Concat(_c.properties[key], "\n"));
		}
	}

	public static void Dispatch(EventC _c, bool _remove)
	{
		_c.startTime = Main.m_gameTime;
		_c.dispatched = true;
		_c.removeAfterDelegate = _remove;
	}

	public static void Dispatch(string _identifier, string[] _keys, object[] _values, bool _remove)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EventC eventC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (!eventC.active || !(eventC.identifier == _identifier))
			{
				continue;
			}
			if (_keys != null && _values != null)
			{
				for (int j = 0; j < _keys.Length; j++)
				{
					if (!eventC.properties.ContainsKey(_keys[j]))
					{
						AddProperty(eventC, _keys[j], _values[j]);
					}
					else
					{
						eventC.properties[_keys[j]] = _values[j];
					}
				}
			}
			Dispatch(eventC, _remove);
		}
	}

	public static void Update()
	{
		int aliveCount = m_components.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EventC eventC = m_components.m_array[m_components.m_aliveIndices[i]];
			if (eventC.active && eventC.dispatched && eventC.startTime + eventC.delay <= Main.m_gameTime && (!eventC.delegateOnlyOnce || eventC.count == 0))
			{
				eventC.count++;
				if (eventC.delegateOnlyOnce || eventC.delay == 0f)
				{
					eventC.dispatched = false;
				}
				else
				{
					eventC.startTime = Main.m_gameTime;
				}
				if (eventC.removeAfterDelegate)
				{
					removeList.Add(eventC);
				}
				delegateList.Add(eventC);
			}
		}
		while (delegateList.Count > 0)
		{
			int index = delegateList.Count - 1;
			if (delegateList[index] != null && delegateList[index].eventDelegate != null)
			{
				delegateList[index].eventDelegate(delegateList[index]);
			}
			delegateList.RemoveAt(index);
		}
		while (removeList.Count > 0)
		{
			int index2 = removeList.Count - 1;
			if (removeList[index2].entityIndex != -1)
			{
				RemoveComponent(removeList[index2]);
			}
			removeList.RemoveAt(index2);
		}
	}
}

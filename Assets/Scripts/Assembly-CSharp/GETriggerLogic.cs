using System;
using UnityEngine;

public static class GETriggerLogic
{
	public static void AddBeganEventDelegate(BasicControlledComponent _c, TriggerEventDelegate _eventHandler)
	{
		if (_c.beganDelegatedCount == 0)
		{
			_c.BeganEventDelegate = _eventHandler;
		}
		else
		{
			_c.BeganEventDelegate = (TriggerEventDelegate)Delegate.Combine(_c.BeganEventDelegate, _eventHandler);
		}
		_c.beganDelegatedCount++;
	}

	public static void RemoveBeganEventDelegate(BasicControlledComponent _c, TriggerEventDelegate _eventHandler)
	{
		if (_c.beganDelegatedCount > 0)
		{
			_c.BeganEventDelegate = (TriggerEventDelegate)Delegate.Remove(_c.BeganEventDelegate, _eventHandler);
			_c.beganDelegatedCount--;
		}
	}

	public static void AddEndEventDelegate(BasicControlledComponent _c, TriggerEventDelegate _eventHandler)
	{
		if (_c.endDelegatedCount == 0)
		{
			_c.EndEventDelegate = _eventHandler;
		}
		else
		{
			_c.EndEventDelegate = (TriggerEventDelegate)Delegate.Combine(_c.EndEventDelegate, _eventHandler);
		}
		_c.endDelegatedCount++;
	}

	public static void RemoveEndEventDelegate(BasicControlledComponent _c, TriggerEventDelegate _eventHandler)
	{
		if (_c.endDelegatedCount > 0)
		{
			_c.EndEventDelegate = (TriggerEventDelegate)Delegate.Remove(_c.EndEventDelegate, _eventHandler);
			_c.endDelegatedCount--;
		}
	}

	public static void DefaultBeganTriggerHandler(BasicControlledComponent _c)
	{
	}

	public static void DefaultEndTriggerHandler(BasicControlledComponent _c)
	{
	}

	public static void HandleBeginTriggerEvent(BasicControlledComponent _c)
	{
		if (!_c.active)
		{
			return;
		}
		bool flag = false;
		int num = 0;
		_c.input.vector = Vector3.zero;
		for (int i = 0; i < _c.inputSlots.Length; i++)
		{
			_c.inputSlots[i].m_value.vector = Vector3.zero;
			for (int j = 0; j < _c.inputSlots[i].m_connections.Count; j++)
			{
				if (_c.inputSlots[i].m_connections[j].controller.triggered && _c.inputSlots[i].m_connections[j].startSlot.m_connectionSlotType == ConnectionSlotType.Output)
				{
					_c.inputSlots[i].m_connections[j].startSlot.m_triggered = true;
				}
				if (_c.inputSlots[i].m_connections[j].startSlot.m_triggered)
				{
					flag = true;
					_c.inputSlots[i].m_triggered = true;
					_c.inputSlots[i].m_value.vector += _c.inputSlots[i].m_connections[j].startSlot.m_value.vector;
					_c.input.vector += _c.inputSlots[i].m_connections[j].startSlot.m_value.vector;
				}
			}
			num += _c.inputSlots[i].m_connections.Count;
		}
		if (num == 0)
		{
			_c.input.vector = _c.def.vector;
			flag = true;
		}
		if ((_c.collidingCount > 0 || _c.autoTrigger) && (!_c.triggerOnlyOnce || _c.triggerCount == 0) && (!_c.triggerOnlyOnFullEnergy || _c.energy == 1f))
		{
			if (!_c.toggle)
			{
				if (!_c.triggered && _c.energy > 0f && _c.endTime + _c.triggerCooldown < Main.m_gameTime)
				{
					if (flag)
					{
						_c.triggerCount++;
						_c.update = true;
						_c.triggered = true;
						_c.began = true;
						_c.beganTime = Main.m_gameTime;
						_c.lastConsume = Main.m_gameTime - _c.energyConsumeInterval;
						if (_c.BeganEventDelegate != null)
						{
							_c.BeganEventDelegate(_c);
						}
					}
				}
				else if (_c.triggered && _c.energy > 0f && _c.endTime + _c.triggerCooldown < Main.m_gameTime && flag)
				{
					_c.triggerCount++;
					_c.update = true;
				}
			}
			else
			{
				if (_c.triggered && flag && !_c.triggerUntilOutOfEnergy)
				{
					HandleEndTriggerEvent(_c);
					return;
				}
				if (flag && _c.energy > 0f && (!_c.triggerOnlyOnFullEnergy || _c.energy == 1f) && _c.endTime + _c.triggerCooldown < Main.m_gameTime)
				{
					_c.triggerCount++;
					_c.update = true;
					_c.triggered = true;
					_c.began = true;
					_c.beganTime = Main.m_gameTime;
					_c.lastConsume = Main.m_gameTime - _c.energyConsumeInterval;
					if (_c.BeganEventDelegate != null)
					{
						_c.BeganEventDelegate(_c);
					}
				}
			}
		}
		if (_c.triggered)
		{
			if (_c.modifierType != ModifierType.None)
			{
				_c.modifier.vector = Vector3.zero;
				for (int k = 0; k < _c.modifierSlots.Length; k++)
				{
					if (_c.modifierSlots[k].m_connectionSlotType != ConnectionSlotType.Modifier)
					{
						continue;
					}
					for (int l = 0; l < _c.modifierSlots[k].m_connections.Count; l++)
					{
						if (_c.modifierSlots[k].m_connections[l].controller.triggered)
						{
							_c.modifier.vector += _c.modifierSlots[k].m_connections[l].startSlot.m_value.vector;
						}
					}
				}
				_c.output = _c.input.Modify(_c.modifier, _c.modifierType);
			}
			else
			{
				_c.output = _c.input;
			}
			for (int m = 0; m < _c.outputSlots.Length; m++)
			{
				if (_c.outputSlots[m].m_connectionSlotType == ConnectionSlotType.Output)
				{
					_c.outputSlots[m].m_triggered = true;
					_c.outputSlots[m].m_value.vector = _c.output.vector;
				}
				if (!_c.outputSlots[m].m_triggered)
				{
					continue;
				}
				for (int n = 0; n < _c.outputSlots[m].m_connections.Count; n++)
				{
					GEConnectionC gEConnectionC = _c.outputSlots[m].m_connections[n];
					if (gEConnectionC.active)
					{
						if (gEConnectionC.connectionType == ConnectionType.Normal)
						{
							HandleBeginTriggerEvent(gEConnectionC.controllee);
						}
						else
						{
							HandleModifierEvent(gEConnectionC.controllee, gEConnectionC);
						}
					}
				}
			}
		}
		else
		{
			_c.input.vector = Vector3.zero;
			_c.output.vector = Vector3.zero;
		}
	}

	public static void HandleEndTriggerEvent(BasicControlledComponent _c)
	{
		if (!_c.active)
		{
			return;
		}
		bool flag = false;
		int num = 0;
		_c.input.vector = Vector3.zero;
		for (int i = 0; i < _c.inputSlots.Length; i++)
		{
			_c.inputSlots[i].m_triggered = false;
			_c.inputSlots[i].m_value.vector = Vector3.zero;
			for (int j = 0; j < _c.inputSlots[i].m_connections.Count; j++)
			{
				if (_c.inputSlots[i].m_connections[j].controller.triggered && _c.inputSlots[i].m_connections[j].startSlot.m_connectionSlotType == ConnectionSlotType.Output)
				{
					_c.inputSlots[i].m_connections[j].startSlot.m_triggered = true;
				}
				if (_c.inputSlots[i].m_connections[j].startSlot.m_triggered)
				{
					flag = true;
					_c.inputSlots[i].m_triggered = true;
					_c.inputSlots[i].m_value.vector += _c.inputSlots[i].m_connections[j].startSlot.m_value.vector;
					_c.input.vector += _c.inputSlots[i].m_connections[j].startSlot.m_value.vector;
				}
			}
			num += _c.inputSlots[i].m_connections.Count;
		}
		if (num == 0 && _c.autoTrigger)
		{
			flag = true;
		}
		if ((!_c.triggerUntilOutOfEnergy || _c.energy == 0f) && (!flag || _c.collidingCount == 0))
		{
			_c.update = true;
			_c.triggered = false;
			_c.end = true;
			_c.endTime = Main.m_gameTime;
			_c.lastGain = Main.m_gameTime + _c.gainCooldown;
			if (_c.EndEventDelegate != null)
			{
				_c.EndEventDelegate(_c);
			}
		}
		if (_c.modifierType != ModifierType.None)
		{
			_c.modifier.vector = Vector3.zero;
			for (int k = 0; k < _c.modifierSlots.Length; k++)
			{
				if (_c.modifierSlots[k].m_connectionSlotType != ConnectionSlotType.Modifier)
				{
					continue;
				}
				for (int l = 0; l < _c.modifierSlots[k].m_connections.Count; l++)
				{
					if (_c.modifierSlots[k].m_connections[l].startSlot.m_triggered)
					{
						_c.modifier.vector += _c.modifierSlots[k].m_connections[l].startSlot.m_value.vector;
					}
				}
			}
			_c.output = _c.input.Modify(_c.modifier, _c.modifierType);
		}
		else
		{
			_c.output = _c.input;
		}
		if (_c.triggered)
		{
			for (int m = 0; m < _c.outputSlots.Length; m++)
			{
				if (_c.outputSlots[m].m_connectionSlotType == ConnectionSlotType.Output)
				{
					_c.outputSlots[m].m_triggered = true;
					_c.outputSlots[m].m_value.vector = _c.output.vector;
				}
				for (int n = 0; n < _c.outputSlots[m].m_connections.Count; n++)
				{
					GEConnectionC gEConnectionC = _c.outputSlots[m].m_connections[n];
					if (gEConnectionC.connectionType == ConnectionType.Normal)
					{
						HandleBeginTriggerEvent(gEConnectionC.controllee);
					}
					else
					{
						HandleModifierEvent(gEConnectionC.controllee, gEConnectionC);
					}
				}
			}
			return;
		}
		_c.input.vector = Vector3.zero;
		_c.output.vector = Vector3.zero;
		for (int num2 = 0; num2 < _c.outputSlots.Length; num2++)
		{
			if (_c.outputSlots[num2].m_connectionSlotType == ConnectionSlotType.Output)
			{
				_c.outputSlots[num2].m_triggered = false;
				_c.outputSlots[num2].m_value.vector = Vector3.zero;
			}
			for (int num3 = 0; num3 < _c.outputSlots[num2].m_connections.Count; num3++)
			{
				GEConnectionC gEConnectionC2 = _c.outputSlots[num2].m_connections[num3];
				if (gEConnectionC2.connectionType == ConnectionType.Normal)
				{
					HandleEndTriggerEvent(gEConnectionC2.controllee);
				}
			}
		}
	}

	public static void HandleModifierEvent(BasicControlledComponent _controllee, GEConnectionC _connection)
	{
		if (GEState.editorMode)
		{
			return;
		}
		if (_connection.endSlot.m_connectionSlotType == ConnectionSlotType.Activate)
		{
			if (_connection.controller.began && !_controllee.active)
			{
				GEConnectionLogic.SetActivityOfControlledComponent(_controllee, true);
			}
			else if (_connection.controller.end && _controllee.active)
			{
				GEConnectionLogic.SetActivityOfControlledComponent(_controllee, false);
			}
		}
		else if (_connection.endSlot.m_connectionSlotType == ConnectionSlotType.Deactivate)
		{
			if (_connection.controller.began && _controllee.active)
			{
				GEConnectionLogic.SetActivityOfControlledComponent(_controllee, false);
			}
			else if (_connection.controller.end && !_controllee.active)
			{
				GEConnectionLogic.SetActivityOfControlledComponent(_controllee, true);
			}
		}
		else if (_connection.endSlot.m_connectionSlotType == ConnectionSlotType.Destroy && _connection.controller.triggered)
		{
			GEConnectionLogic.DestroyControlledComponent(_controllee);
		}
	}

	public static void Update(GETriggerC _c)
	{
		if (_c.triggerType == TriggerType.TiltController)
		{
			_c.def.vector = Input.acceleration;
		}
		if (_c.update)
		{
			if (!_c.triggered)
			{
				bool flag = false;
				int num = 0;
				for (int i = 0; i < _c.inputSlots.Length; i++)
				{
					for (int j = 0; j < _c.inputSlots[i].m_connections.Count; j++)
					{
						if (_c.inputSlots[i].m_connections[j].controller.triggered)
						{
							flag = true;
						}
					}
					num += _c.inputSlots[i].m_connections.Count;
				}
				if (num == 0 && _c.autoTrigger)
				{
					flag = true;
				}
				if (flag)
				{
					HandleBeginTriggerEvent(_c);
				}
			}
			_c.update = false;
			_c.began = false;
			_c.end = false;
		}
		if (_c.triggerType == TriggerType.ButtonTrigger)
		{
			Vector3 vector = _c.CMC.TC.transform.position - _c.tileTC.transform.position;
			if (!_c.triggered)
			{
				if (vector.sqrMagnitude > 25f)
				{
					_c.collidingCount++;
					HandleBeginTriggerEvent(_c);
				}
			}
			else if (_c.triggered && vector.sqrMagnitude < 25f && !_c.toggle)
			{
				_c.collidingCount--;
				HandleEndTriggerEvent(_c);
			}
		}
		float gameTime = Main.m_gameTime;
		if (_c.triggered)
		{
			if (_c.lastConsume + _c.energyConsumeInterval < gameTime)
			{
				_c.energy = Mathf.Min(Mathf.Max(_c.energy - _c.energyConsume, 0f), 1f);
				_c.lastConsume += _c.energyConsumeInterval;
				if (_c.energy == 0f)
				{
					HandleEndTriggerEvent(_c);
					if (_c.energyClips > 0)
					{
						_c.reloading = true;
						_c.energyClips--;
						_c.lastReload = gameTime;
					}
					else if (_c.energyClips == -1)
					{
						_c.reloading = true;
						_c.lastReload = gameTime;
					}
				}
			}
		}
		else if (_c.reloading)
		{
			if (_c.lastReload + _c.reloadCooldown < gameTime && _c.energy == 0f && _c.reloading)
			{
				_c.energy = 1f;
				_c.reloading = false;
				_c.lastGain = Main.m_gameTime + _c.gainCooldown;
				HandleBeginTriggerEvent(_c);
			}
		}
		else if (!_c.toggle && _c.lastConsume + _c.gainCooldown < gameTime && _c.lastGain + _c.energyGainInterval < gameTime)
		{
			_c.energy = Mathf.Min(Mathf.Max(_c.energy + _c.energyGain, 0f), 1f);
			_c.lastGain += _c.energyGainInterval;
			if (_c.energy > 0f && (!_c.triggerOnlyOnFullEnergy || _c.energy == 1f) && _c.collidingCount > 0)
			{
				HandleBeginTriggerEvent(_c);
			}
		}
		if (_c.debug != null)
		{
			TextS.ChangeText(_c.debug, "t: " + _c.triggered + ", e: " + _c.energy + ", c: " + _c.energyClips + ", v: " + _c.output.vector);
		}
	}
}

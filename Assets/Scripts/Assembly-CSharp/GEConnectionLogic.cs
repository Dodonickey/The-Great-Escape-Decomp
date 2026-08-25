using System;
using System.Collections.Generic;
using UnityEngine;

public static class GEConnectionLogic
{
	public static EIC m_connectionStart;

	public static List<uint> m_removeList = new List<uint>();

	public static void RemoveOutputConnectionsByAnchoredId(uint _id, ConnectionSlotType _connectionSlotType)
	{
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (gEConnectionC.controller.id != _id || (_connectionSlotType != ConnectionSlotType.Any && _connectionSlotType != gEConnectionC.startSlot.m_connectionSlotType))
			{
				continue;
			}
			if (gEConnectionC.controllee.triggerType == TriggerType.ControlScheme)
			{
				GEControlSchemeC gEControlSchemeC = gEConnectionC.controllee as GEControlSchemeC;
				gEControlSchemeC.playerState.m_contollerState.components[gEConnectionC.endSlot.m_index] = null;
			}
			bool flag = false;
			for (int j = 0; j < gEConnectionC.controllee.inputSlots.Length; j++)
			{
				ConnectionSlot connectionSlot = gEConnectionC.controllee.inputSlots[j];
				for (int k = 0; k < connectionSlot.m_connections.Count; k++)
				{
					if (connectionSlot.m_connections[k] == gEConnectionC)
					{
						connectionSlot.m_connections.RemoveAt(k);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			(LevelManager.m_currentLevel as GELevel).items.Remove(gEConnectionC.container);
			(LevelManager.m_currentLevel as GELevel).connections.Remove(gEConnectionC.container);
			EntityManager.RemoveEntity(gEConnectionC.entityIndex);
		}
		EntityManager.Update();
	}

	public static void RemoveModifierConnectionsByAnchoredId(uint _id, ConnectionSlotType _connectionSlotType)
	{
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (gEConnectionC.controllee.id != _id || (_connectionSlotType != ConnectionSlotType.Any && _connectionSlotType != gEConnectionC.endSlot.m_connectionSlotType))
			{
				continue;
			}
			if (gEConnectionC.controllee.triggerType == TriggerType.ControlScheme)
			{
				GEControlSchemeC gEControlSchemeC = gEConnectionC.controllee as GEControlSchemeC;
				gEControlSchemeC.playerState.m_contollerState.components[gEConnectionC.endSlot.m_index] = null;
			}
			bool flag = false;
			for (int j = 0; j < gEConnectionC.controller.outputSlots.Length; j++)
			{
				ConnectionSlot connectionSlot = gEConnectionC.controller.outputSlots[j];
				for (int k = 0; k < connectionSlot.m_connections.Count; k++)
				{
					if (connectionSlot.m_connections[k] == gEConnectionC)
					{
						connectionSlot.m_connections.RemoveAt(k);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			(LevelManager.m_currentLevel as GELevel).items.Remove(gEConnectionC.container);
			(LevelManager.m_currentLevel as GELevel).connections.Remove(gEConnectionC.container);
			EntityManager.RemoveEntity(gEConnectionC.entityIndex);
		}
		EntityManager.Update();
	}

	public static void RemoveInputConnectionsByAnchoredId(uint _id, ConnectionSlotType _connectionSlotType)
	{
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (gEConnectionC.controllee.id != _id || (_connectionSlotType != ConnectionSlotType.Any && _connectionSlotType != gEConnectionC.endSlot.m_connectionSlotType))
			{
				continue;
			}
			if (gEConnectionC.controllee.triggerType == TriggerType.ControlScheme)
			{
				GEControlSchemeC gEControlSchemeC = gEConnectionC.controllee as GEControlSchemeC;
				gEControlSchemeC.playerState.m_contollerState.components[gEConnectionC.endSlot.m_index] = null;
			}
			bool flag = false;
			for (int j = 0; j < gEConnectionC.controller.outputSlots.Length; j++)
			{
				ConnectionSlot connectionSlot = gEConnectionC.controller.outputSlots[j];
				for (int k = 0; k < connectionSlot.m_connections.Count; k++)
				{
					if (connectionSlot.m_connections[k] == gEConnectionC)
					{
						connectionSlot.m_connections.RemoveAt(k);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			(LevelManager.m_currentLevel as GELevel).items.Remove(gEConnectionC.container);
			(LevelManager.m_currentLevel as GELevel).connections.Remove(gEConnectionC.container);
			EntityManager.RemoveEntity(gEConnectionC.entityIndex);
		}
		EntityManager.Update();
	}

	public static void RemoveConnectionsByAnchoredId(uint _id, ConnectionSlotType _connectionType)
	{
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if ((gEConnectionC.controller.id != _id && gEConnectionC.controllee.id != _id) || (_connectionType != ConnectionSlotType.Any && _connectionType != gEConnectionC.startSlot.m_connectionSlotType && _connectionType != gEConnectionC.endSlot.m_connectionSlotType))
			{
				continue;
			}
			if (gEConnectionC.controllee.triggerType == TriggerType.ControlScheme)
			{
				GEControlSchemeC gEControlSchemeC = gEConnectionC.controllee as GEControlSchemeC;
				gEControlSchemeC.playerState.m_contollerState.components[gEConnectionC.endSlot.m_index] = null;
			}
			if (gEConnectionC.controller.id == _id)
			{
				bool flag = false;
				for (int j = 0; j < gEConnectionC.controllee.inputSlots.Length; j++)
				{
					ConnectionSlot connectionSlot = gEConnectionC.controllee.inputSlots[j];
					for (int k = 0; k < connectionSlot.m_connections.Count; k++)
					{
						if (connectionSlot.m_connections[k] == gEConnectionC)
						{
							connectionSlot.m_connections.RemoveAt(k);
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			else
			{
				bool flag2 = false;
				for (int l = 0; l < gEConnectionC.controller.outputSlots.Length; l++)
				{
					ConnectionSlot connectionSlot2 = gEConnectionC.controller.outputSlots[l];
					for (int m = 0; m < connectionSlot2.m_connections.Count; m++)
					{
						if (connectionSlot2.m_connections[m] == gEConnectionC)
						{
							connectionSlot2.m_connections.RemoveAt(m);
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						break;
					}
				}
			}
			(LevelManager.m_currentLevel as GELevel).items.Remove(gEConnectionC.container);
			(LevelManager.m_currentLevel as GELevel).connections.Remove(gEConnectionC.container);
			EntityManager.RemoveEntity(gEConnectionC.entityIndex);
		}
		EntityManager.Update();
	}

	public static bool AreConnectionsLooping(uint _start, uint _end)
	{
		GEConnectionC[] controlledConnections = GetControlledConnections(_end);
		foreach (GEConnectionC gEConnectionC in controlledConnections)
		{
			if (gEConnectionC.controllee.id == _start && gEConnectionC.connectionType != ConnectionType.Modifier)
			{
				return true;
			}
			if (AreConnectionsLooping(_start, gEConnectionC.controllee.id))
			{
				return true;
			}
		}
		return false;
	}

	public static GEConnectionC GetConnection(uint _start, uint _end, ConnectionSlotType _startType, ConnectionSlotType _endType)
	{
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (gEConnectionC.controller.id == _start && gEConnectionC.controllee.id == _end && gEConnectionC.startSlot.m_connectionSlotType == _startType && gEConnectionC.endSlot.m_connectionSlotType == _endType)
			{
				return gEConnectionC;
			}
		}
		return null;
	}

	public static GEConnectionC[] GetControlledConnections(uint _controller)
	{
		List<GEConnectionC> list = new List<GEConnectionC>();
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (gEConnectionC.controller.id == _controller)
			{
				list.Add(gEConnectionC);
			}
		}
		return list.ToArray();
	}

	public static GEConnectionC[] GetConnections(uint _id)
	{
		List<GEConnectionC> list = new List<GEConnectionC>();
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (gEConnectionC.controller.id == _id || gEConnectionC.controllee.id == _id)
			{
				list.Add(gEConnectionC);
			}
		}
		return list.ToArray();
	}

	public static GEConnectionC[] GetOutputConnections(uint _id)
	{
		List<GEConnectionC> list = new List<GEConnectionC>();
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (gEConnectionC.controller.id == _id)
			{
				list.Add(gEConnectionC);
			}
		}
		return list.ToArray();
	}

	public static GEConnectionC[] GetInputConnections(uint _id)
	{
		List<GEConnectionC> list = new List<GEConnectionC>();
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (gEConnectionC.controllee.id == _id)
			{
				list.Add(gEConnectionC);
			}
		}
		return list.ToArray();
	}

	public static void CreateInputAnchors(EIC _eic)
	{
		int aliveCount = GES.m_editorItemComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EIC eIC = GES.m_editorItemComponents.m_array[GES.m_editorItemComponents.m_aliveIndices[i]];
			if (eIC.active && eIC != _eic && eIC.itemType == 1)
			{
				CreateInputAnchor(eIC);
			}
		}
	}

	public static void CreateOutputAnchors(EIC _eic)
	{
		int aliveCount = GES.m_editorItemComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EIC eIC = GES.m_editorItemComponents.m_array[GES.m_editorItemComponents.m_aliveIndices[i]];
			if (eIC.active && eIC != _eic && eIC.itemType == 1)
			{
				CreateOutputAnchor(eIC);
			}
		}
	}

	public static void CreateModifierAnchors(EIC _eic)
	{
		int aliveCount = GES.m_editorItemComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			EIC eIC = GES.m_editorItemComponents.m_array[GES.m_editorItemComponents.m_aliveIndices[i]];
			if (eIC.active && eIC != _eic && eIC.itemType == 1)
			{
				CreateModifierAnchor(eIC);
			}
		}
	}

	public static void CreateInputAnchor(EIC _eic)
	{
		string[] tags = new string[3]
		{
			"EditorHandle",
			LevelManager.m_currentLevel.name + ":EditorHandle",
			LevelManager.m_currentLevel.name
		};
		IControlledComponent trigger = _eic.trigger;
		if (trigger != null)
		{
			int num = trigger.inputSlots.Length;
			float num2 = 20f;
			float num3 = 60f;
			float num4 = num2 * (float)(num - 1) * -0.5f - 90f;
			for (int i = 0; i < trigger.inputSlots.Length; i++)
			{
				float num5 = (num4 + num2 * (float)i) * ((float)Math.PI / 180f);
				float x = Mathf.Sin(num5) * num3;
				float y = Mathf.Cos(num5) * num3;
				EIC eIC = GEConnectionHandleA.Assemble(GEConnectionHandleA.HandleInputAnchor, _eic, trigger.inputSlots[i].m_connectionSlotType, num5 * 57.29578f, new Vector3(x, y, 0f), Vector3.zero, Vector3.one, "EditorHandle", tags);
			}
		}
	}

	public static void CreateModifierAnchor(EIC _eic)
	{
		string[] tags = new string[3]
		{
			"EditorHandle",
			LevelManager.m_currentLevel.name + ":EditorHandle",
			LevelManager.m_currentLevel.name
		};
		IControlledComponent trigger = _eic.trigger;
		if (trigger != null)
		{
			int num = trigger.modifierSlots.Length;
			float num2 = 20f;
			float num3 = 60f;
			float num4 = num2 * (float)(num - 1) * -0.5f;
			for (int i = 0; i < trigger.modifierSlots.Length; i++)
			{
				float num5 = (num4 + num2 * (float)i) * ((float)Math.PI / 180f);
				float x = Mathf.Sin(num5) * num3;
				float y = Mathf.Cos(num5) * num3;
				EIC eIC = GEConnectionHandleA.Assemble(GEConnectionHandleA.HandleModifierAnchor, _eic, trigger.modifierSlots[i].m_connectionSlotType, num5 * 57.29578f, new Vector3(x, y, 0f), Vector3.zero, Vector3.one, "EditorHandle", tags);
			}
		}
	}

	public static void CreateOutputAnchor(EIC _eic)
	{
		string[] tags = new string[3]
		{
			"EditorHandle",
			LevelManager.m_currentLevel.name + ":EditorHandle",
			LevelManager.m_currentLevel.name
		};
		IControlledComponent trigger = _eic.trigger;
		if (trigger != null)
		{
			int num = trigger.outputSlots.Length;
			float num2 = 20f;
			float num3 = 60f;
			float num4 = num2 * (float)(num - 1) * -0.5f + 90f;
			for (int i = 0; i < trigger.outputSlots.Length; i++)
			{
				float num5 = (num4 + num2 * (float)i) * ((float)Math.PI / 180f);
				float x = Mathf.Sin(num5) * num3;
				float y = Mathf.Cos(num5) * num3;
				EIC eIC = GEConnectionHandleA.Assemble(GEConnectionHandleA.HandleOutputAnchor, _eic, trigger.outputSlots[i].m_connectionSlotType, num5 * 57.29578f, new Vector3(x, y, 0f), Vector3.zero, Vector3.one, "EditorHandle", tags);
			}
		}
	}

	public static void MarkDepthValuesForConnections()
	{
		int aliveCount = GES.m_connectionComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			GEConnectionC gEConnectionC = GES.m_connectionComponents.m_array[GES.m_connectionComponents.m_aliveIndices[i]];
			if (!gEConnectionC.active)
			{
				continue;
			}
			bool flag = true;
			for (int j = 0; j < gEConnectionC.controllee.outputSlots.Length; j++)
			{
				if (gEConnectionC.controllee.outputSlots[j].m_connections.Count == 0)
				{
					flag = false;
				}
			}
			if (flag)
			{
				GEConnectionC[] inputConnections = GetInputConnections(gEConnectionC.controllee.id);
				for (int k = 0; k < inputConnections.Length; k++)
				{
					MarkDepthForConnectionTree(inputConnections[k], 0);
				}
			}
		}
	}

	private static void MarkDepthForConnectionTree(GEConnectionC _c, int _deptMarking)
	{
		if (_c.depth < _deptMarking)
		{
			_c.depth = _deptMarking;
			GEConnectionC[] inputConnections = GetInputConnections(_c.controller.id);
			for (int i = 0; i < inputConnections.Length; i++)
			{
				MarkDepthForConnectionTree(inputConnections[i], _deptMarking + 1);
			}
		}
	}

	public static void SetActivityOfControlledComponent(BasicControlledComponent _c, bool _active)
	{
		EntityManager.SetActivityOfEntity(_c.entityIndex, _active, true);
	}

	public static void DestroyControlledComponent(BasicControlledComponent _c)
	{
		if (_c.triggerType == TriggerType.RopeConstraint || _c.triggerType == TriggerType.BoltConstraint)
		{
			GEConstraintLogic.RemoveChipmunkConstraints(_c as GEConstraintC);
		}
		m_removeList.Add(_c.id);
		EntityManager.RemoveEntity(_c.entityIndex);
	}

	public static void Update(GEConnectionC _c)
	{
		if (GEState.connectionTC != null)
		{
			if (_c.depth > -1)
			{
				_c.depth = -1;
			}
			Vector3 vector = _c.controller.position;
			Vector3 vector2 = _c.controllee.position;
			if (_c.controller.camera == Main.camera)
			{
				Vector3 position = vector;
				vector = Main.camera.WorldToScreenPoint(position) - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
				vector.z = -200f;
			}
			if (_c.controllee.camera == Main.camera)
			{
				Vector3 position2 = vector2;
				vector2 = Main.camera.WorldToScreenPoint(position2) - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
				vector2.z = -200f;
			}
			float num = 20f;
			float num2 = 60f;
			if (_c.connectionType == ConnectionType.Modifier)
			{
				int num3 = _c.controller.outputSlots.Length;
				float num4 = num * (float)(num3 - 1) * -0.5f + 90f;
				float f = (num4 + num * (float)_c.startSlot.m_index) * ((float)Math.PI / 180f);
				float x = Mathf.Sin(f) * num2;
				float y = Mathf.Cos(f) * num2;
				int num5 = _c.controllee.modifierSlots.Length;
				float num6 = num * (float)(num5 - 1) * -0.5f;
				float f2 = (num6 + num * (float)_c.endSlot.m_index) * ((float)Math.PI / 180f);
				float x2 = Mathf.Sin(f2) * num2;
				float y2 = Mathf.Cos(f2) * num2;
				DebugDraw.CreateCircle(Main.uiCamera, GEState.connectionTC, vector2 + new Vector3(x2, y2, 0f), 5f, false);
				DebugDraw.CreateLine(Main.uiCamera, GEState.connectionTC, vector2, vector2 + new Vector3(x2, y2, 0f));
				DebugDraw.CreateLine(Main.uiCamera, GEState.connectionTC, vector + new Vector3(x, y, 0f), vector2 + new Vector3(x2, y2, 0f));
				DebugDraw.CreateCircle(Main.uiCamera, GEState.connectionTC, vector + new Vector3(x, y, 0f), 5f, false);
				DebugDraw.CreateLine(Main.uiCamera, GEState.connectionTC, vector, vector + new Vector3(x, y, 0f));
			}
			else
			{
				int num7 = _c.controller.outputSlots.Length;
				float num8 = num * (float)(num7 - 1) * -0.5f + 90f;
				float f3 = (num8 + num * (float)_c.startSlot.m_index) * ((float)Math.PI / 180f);
				float x3 = Mathf.Sin(f3) * num2;
				float y3 = Mathf.Cos(f3) * num2;
				int num9 = _c.controllee.inputSlots.Length;
				float num10 = num * (float)(num9 - 1) * -0.5f - 90f;
				float f4 = (num10 + num * (float)_c.endSlot.m_index) * ((float)Math.PI / 180f);
				float x4 = Mathf.Sin(f4) * num2;
				float y4 = Mathf.Cos(f4) * num2;
				DebugDraw.CreateCircle(Main.uiCamera, GEState.connectionTC, vector2 + new Vector3(x4, y4, 0f), 5f, false);
				DebugDraw.CreateLine(Main.uiCamera, GEState.connectionTC, vector2, vector2 + new Vector3(x4, y4, 0f));
				DebugDraw.CreateLine(Main.uiCamera, GEState.connectionTC, vector + new Vector3(x3, y3, 0f), vector2 + new Vector3(x4, y4, 0f));
				DebugDraw.CreateCircle(Main.uiCamera, GEState.connectionTC, vector + new Vector3(x3, y3, 0f), 5f, false);
				DebugDraw.CreateLine(Main.uiCamera, GEState.connectionTC, vector, vector + new Vector3(x3, y3, 0f));
			}
		}
	}
}

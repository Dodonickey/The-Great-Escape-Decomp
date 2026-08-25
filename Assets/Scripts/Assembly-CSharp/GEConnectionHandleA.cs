using UnityEngine;

public static class GEConnectionHandleA
{
	public static EIC Assemble(TouchEventDelegate _customTouchEventHandler, EIC _container, ConnectionSlotType _connectionType, float _labelAngle, Vector3 _pos, Vector3 _rot, Vector3 _scale, string _identifier, string[] _tags)
	{
		Entity entity = EntityManager.AddEntity(_tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TouchAreaC touchAreaC = TouchAreaS.AddComponent(transformC, _identifier, 10f, true, Main.uiCamera, null);
		TouchAreaS.AddTouchEventListener(touchAreaC, _customTouchEventHandler);
		float num = -90f;
		Align hAlign = Align.Right;
		if (_labelAngle > 0f)
		{
			hAlign = Align.Left;
			num = 90f;
		}
		TextS.SetStyle("body");
		TextC textC = TextS.AddSingleLineComponent(transformC, _connectionType.ToString(), 1f, hAlign, Align.Bottom);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(255f, 255f, 255f), false, false);
		TransformS.SetRotation(textC.contentTC, Vector3.forward * (0f - _labelAngle + num));
		Vector2[] circle = DebugDraw.GetCircle(10f, 36, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC, Vector3.zero, circle, 4f, new Color(0.9f, 1f, 0.9f), ResourceManager.GetMaterial("Line4"), Main.uiCamera, Position.Center, true);
		TransformS.SetPosition(transformC, _container.uiTC.transform.position + _pos);
		return GES.AddEditorItemHandleComponent(_container, _identifier, null, EditorItemType.ConnectionHandle, _connectionType, transformC, touchAreaC);
	}

	public static void HandleInputAnchor(TouchAreaC _c, int _i, bool _consumed)
	{
		EIC eIC = _c.customComponent as EIC;
		if (!_consumed)
		{
			if (_c.touchEvent[_i] == TouchEvent.Began)
			{
				GEConnectionLogic.RemoveInputConnectionsByAnchoredId(eIC.container.data.id, eIC.connectionSlotType);
				GEConnectionLogic.CreateOutputAnchors(eIC.container.subItems[0]);
				GEConnectionLogic.m_connectionStart = eIC;
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", false, false);
			}
			else if (_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				if (_c.camera == Main.uiCamera)
				{
					DebugDraw.CreateLine(_c.camera, GEState.drawTC, _c.TC.transform.position, -TouchAreaS.GetTouchWorldPos(_c.camera, _c.touchPos[_i]));
				}
				else
				{
					DebugDraw.CreateLine(_c.camera, GEState.drawTC, _c.TC.transform.position, TouchAreaS.GetTouchWorldPos(_c.camera, _c.touchPos[_i]));
				}
			}
			else if (!_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				DebugDraw.CreateLine(_c.camera, GEState.drawTC, GEConnectionLogic.m_connectionStart.TAC.TC.transform.position, _c.TC.transform.position);
			}
			else if (_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Release || _c.touchEvent[_i] == TouchEvent.ReleaseOutside))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				GEConnectionLogic.m_connectionStart = null;
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", true, false);
				EntityManager.RemoveEntitiesByTag("EditorHandle");
				EditorState.UpdateSelection();
			}
		}
		else if (!_c.touchStartedInside[_i] && _c.touchEvent[_i] == TouchEvent.Release)
		{
			if (eIC.container != null && GEConnectionLogic.m_connectionStart != null && GEConnectionLogic.m_connectionStart.container != null && !GEConnectionLogic.AreConnectionsLooping(GEConnectionLogic.m_connectionStart.container.data.id, eIC.container.data.id))
			{
				ConnectionData connectionData = new ConnectionData();
				connectionData.id = GES.GetUniqueId();
				connectionData.name = "Connection";
				connectionData.startId = GEConnectionLogic.m_connectionStart.container.data.id;
				connectionData.endId = eIC.container.data.id;
				connectionData.startType = (uint)GEConnectionLogic.m_connectionStart.connectionSlotType;
				connectionData.endType = (uint)eIC.connectionSlotType;
				EIC eIC2 = GEConnectionA.Assemble(connectionData);
				EditorState.FillEditorItemHierarchy(eIC2);
				(LevelManager.m_currentLevel as GELevel).connections.Add(eIC2);
			}
			EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", true, false);
		}
	}

	public static void HandleModifierAnchor(TouchAreaC _c, int _i, bool _consumed)
	{
		EIC eIC = _c.customComponent as EIC;
		if (!_consumed)
		{
			if (_c.touchEvent[_i] == TouchEvent.Began)
			{
				GEConnectionLogic.RemoveModifierConnectionsByAnchoredId(eIC.container.data.id, eIC.connectionSlotType);
				GEConnectionLogic.CreateOutputAnchors(eIC.container.subItems[0]);
				GEConnectionLogic.m_connectionStart = eIC;
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", false, false);
			}
			else if (_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				if (_c.camera == Main.uiCamera)
				{
					DebugDraw.CreateLine(_c.camera, GEState.drawTC, _c.TC.transform.position, -TouchAreaS.GetTouchWorldPos(_c.camera, _c.touchPos[_i]));
				}
				else
				{
					DebugDraw.CreateLine(_c.camera, GEState.drawTC, _c.TC.transform.position, TouchAreaS.GetTouchWorldPos(_c.camera, _c.touchPos[_i]));
				}
			}
			else if (!_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				DebugDraw.CreateLine(_c.camera, GEState.drawTC, GEConnectionLogic.m_connectionStart.TAC.TC.transform.position, _c.TC.transform.position);
			}
			else if (_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Release || _c.touchEvent[_i] == TouchEvent.ReleaseOutside))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				GEConnectionLogic.m_connectionStart = null;
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", true, false);
				EntityManager.RemoveEntitiesByTag("EditorHandle");
				EditorState.UpdateSelection();
			}
		}
		else if (!_c.touchStartedInside[_i] && _c.touchEvent[_i] == TouchEvent.Release)
		{
			if (eIC.container != null && GEConnectionLogic.m_connectionStart != null && GEConnectionLogic.m_connectionStart.container != null && !GEConnectionLogic.AreConnectionsLooping(GEConnectionLogic.m_connectionStart.container.data.id, eIC.container.data.id))
			{
				ConnectionData connectionData = new ConnectionData();
				connectionData.id = GES.GetUniqueId();
				connectionData.name = "Connection";
				connectionData.startId = GEConnectionLogic.m_connectionStart.container.data.id;
				connectionData.endId = eIC.container.data.id;
				connectionData.startType = (uint)GEConnectionLogic.m_connectionStart.connectionSlotType;
				connectionData.endType = (uint)eIC.connectionSlotType;
				EIC eIC2 = GEConnectionA.Assemble(connectionData);
				EditorState.FillEditorItemHierarchy(eIC2);
				(LevelManager.m_currentLevel as GELevel).connections.Add(eIC2);
			}
			EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", true, false);
		}
	}

	public static void HandleOutputAnchor(TouchAreaC _c, int _i, bool _consumed)
	{
		EIC eIC = _c.customComponent as EIC;
		if (!_consumed)
		{
			if (_c.touchEvent[_i] == TouchEvent.Began)
			{
				GEConnectionLogic.RemoveOutputConnectionsByAnchoredId(eIC.container.data.id, eIC.connectionSlotType);
				GEConnectionLogic.CreateInputAnchors(eIC.container.subItems[0]);
				GEConnectionLogic.CreateModifierAnchors(eIC.container.subItems[0]);
				GEConnectionLogic.m_connectionStart = eIC;
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", false, false);
			}
			else if (_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				if (_c.camera == Main.uiCamera)
				{
					DebugDraw.CreateLine(_c.camera, GEState.drawTC, _c.TC.transform.position, -TouchAreaS.GetTouchWorldPos(_c.camera, _c.touchPos[_i]));
				}
				else
				{
					DebugDraw.CreateLine(_c.camera, GEState.drawTC, _c.TC.transform.position, TouchAreaS.GetTouchWorldPos(_c.camera, _c.touchPos[_i]));
				}
			}
			else if (!_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				DebugDraw.CreateLine(_c.camera, GEState.drawTC, GEConnectionLogic.m_connectionStart.TAC.TC.transform.position, _c.TC.transform.position);
			}
			else if (_c.touchStartedInside[_i] && (_c.touchEvent[_i] == TouchEvent.Release || _c.touchEvent[_i] == TouchEvent.ReleaseOutside))
			{
				DebugDraw.Clear(_c.camera, GEState.drawTC);
				GEConnectionLogic.m_connectionStart = null;
				EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", true, false);
				EntityManager.RemoveEntitiesByTag("EditorHandle");
				EditorState.UpdateSelection();
			}
		}
		else if (!_c.touchStartedInside[_i] && _c.touchEvent[_i] == TouchEvent.Release)
		{
			if (eIC.container != null && GEConnectionLogic.m_connectionStart != null && GEConnectionLogic.m_connectionStart.container != null && !GEConnectionLogic.AreConnectionsLooping(eIC.container.data.id, GEConnectionLogic.m_connectionStart.container.data.id))
			{
				ConnectionData connectionData = new ConnectionData();
				connectionData.id = GES.GetUniqueId();
				connectionData.name = "Connection";
				connectionData.startId = eIC.container.data.id;
				connectionData.endId = GEConnectionLogic.m_connectionStart.container.data.id;
				connectionData.startType = (uint)eIC.connectionSlotType;
				connectionData.endType = (uint)GEConnectionLogic.m_connectionStart.connectionSlotType;
				EIC eIC2 = GEConnectionA.Assemble(connectionData);
				EditorState.FillEditorItemHierarchy(eIC2);
				(LevelManager.m_currentLevel as GELevel).connections.Add(eIC2);
			}
			EntityManager.SetActivityOfEntitiesWithTag(LevelManager.m_currentLevel.name + ":RailAnchorHandle", true, false);
		}
	}
}

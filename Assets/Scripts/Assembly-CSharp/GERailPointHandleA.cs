using UnityEngine;

public static class GERailPointHandleA
{
	public static EIC Assemble(Camera _camera, TouchEventDelegate _customTouchEventHandler, EIC _eic, AnchorPointInfo _relativeToA, AnchorPointInfo _relativeToB, Vector3 _pos, string _identifier, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		TouchAreaC touchAreaC = TouchAreaS.AddComponent(transformC, _identifier, 10f, true, Main.camera, null);
		touchAreaC.scaleByCameraDistance = true;
		if (_customTouchEventHandler == null)
		{
			TouchAreaS.AddTouchEventListener(touchAreaC, HandleConstraintHandle);
		}
		else
		{
			TouchAreaS.AddTouchEventListener(touchAreaC, _customTouchEventHandler);
		}
		ConstraintPointData constraintPointData = new ConstraintPointData(AnchorType.RailPoint);
		constraintPointData.position = new Vertex3(_pos);
		constraintPointData.rotation = new Vertex3(Vector3.zero);
		constraintPointData.scale = new Vertex3(Vector3.one);
		constraintPointData.anchorIndex = _relativeToB.anchorIndex;
		constraintPointData.entryEasingType = _relativeToB.entryEasingType;
		constraintPointData.exitEasingType = _relativeToB.exitEasingType;
		constraintPointData.interpolationType = _relativeToB.interpolationType;
		constraintPointData.velocityMultipler = _relativeToB.velocityMultipler;
		constraintPointData.waitAtPoint = _relativeToB.waitAtPoint;
		DebugDraw.CreateLine(Main.camera, transformC, 10f, Vector2.zero, 0f);
		DebugDraw.CreateLine(Main.camera, transformC, 10f, Vector2.zero, 90f);
		SpriteS.ConvertSpritesToPrefabComponent(transformC, true);
		TransformS.SetPosition(transformC, _pos);
		return GES.AddEditorItemHandleComponent(_eic, _identifier, constraintPointData, EditorItemType.RailPointHandle, _relativeToA, _relativeToB, touchAreaC);
	}

	public static void HandleConstraintHandle(TouchAreaC _c, int _i, bool _consumed)
	{
		EIC eIC = _c.customComponent as EIC;
		if (_c.touchEvent[_i] != TouchEvent.Began)
		{
			return;
		}
		EntityManager.SetActivityOfEntitiesWithTag("drawModeMenuButton", false, true);
		EntityManager.SetActivityOfEntitiesWithTag("drawButtons", false, true);
		TLTouch tLTouch = InputManager.m_touches[_c.touchIndex[_i]];
		Vector2 position = tLTouch.position;
		Vector3 touchWorldPos = TouchAreaS.GetTouchWorldPos(_c.camera, _c.touchPos[_i]);
		ConstraintPointData constraintPointData = eIC.data as ConstraintPointData;
		int anchorIndex = constraintPointData.anchorIndex;
		for (int i = 0; i < eIC.container.subItems.Count; i++)
		{
			EIC eIC2 = eIC.container.subItems[i];
			if (eIC2.identifier == "RailPoint" && (eIC2.itemType == 1 || eIC2.itemType == 2))
			{
				ConstraintPointData constraintPointData2 = eIC2.data as ConstraintPointData;
				if (constraintPointData2.anchorIndex >= anchorIndex)
				{
					constraintPointData2.anchorIndex++;
				}
			}
		}
		ConstraintPointData constraintPointData3 = new ConstraintPointData(AnchorType.RailPoint);
		constraintPointData3.position = eIC.data.position;
		constraintPointData3.rotation = new Vertex3(Vector3.zero);
		constraintPointData3.scale = new Vertex3(Vector3.one);
		constraintPointData3.anchorIndex = (eIC.data as ConstraintPointData).anchorIndex;
		constraintPointData3.entryEasingType = 0;
		constraintPointData3.exitEasingType = 0;
		constraintPointData3.interpolationType = 0;
		constraintPointData3.velocityMultipler = 1f;
		constraintPointData3.waitAtPoint = 0f;
		constraintPointData3.Init(eIC.data.id, "RailPoint" + eIC.data.id);
		EIC eIC3 = GEItemA.Assemble(eIC.container, "RailPoint", constraintPointData3, Main.camera);
		eIC3.camera = Main.camera;
		eIC3.isRealtimeMovable = true;
		eIC.container.subItems.Remove(eIC3);
		eIC.container.subItems.Insert(constraintPointData3.anchorIndex, eIC3);
		EditorState.m_selection.Clear();
		EditorState.m_selection.Add(eIC3);
		EditorState.UpdateSelection();
		if (EditorState.m_gizmo != null)
		{
			TouchAreaS.ReleaseTouches(_c);
			TouchAreaS.ForceTouch(EditorState.m_gizmo.moveTAC, tLTouch, _i, TouchEvent.Began, false);
			EditorState.m_gizmo.touchOffset = Vector3.zero;
		}
		EntityManager.RemoveEntity(eIC.entityIndex);
	}
}

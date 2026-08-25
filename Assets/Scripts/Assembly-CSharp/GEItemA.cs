using UnityEngine;

public static class GEItemA
{
	public static EIC Assemble(EIC _container, string _identifier, ILevelData _data, Camera _camera)
	{
		string[] tags = new string[3]
		{
			LevelManager.m_currentLevel.name,
			LevelManager.m_currentLevel.name + ":EditorItem",
			"EditorItem"
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformC transformC2 = TransformS.AddComponent(entity);
		TouchAreaC touchAreaC = TouchAreaS.AddComponent(transformC2, _identifier, 20f, true, Main.uiCamera, null);
		TouchAreaS.AddTouchEventListener(touchAreaC, HandleEditorItemTouches);
		TextS.SetStyle("body");
		TextC textC = TextS.AddSingleLineComponent(transformC2, _data.name, 1f, Align.Center, Align.Top);
		TransformS.SetPosition(textC.contentTC, Vector3.up * -15f);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(255f, 255f, 255f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, true);
		Vector2[] circle = DebugDraw.GetCircle(10f, 36, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC2, Vector3.zero, circle, 4f, new Color(0.8f, 1f, 1f), ResourceManager.GetMaterial("Line4"), Main.uiCamera, Position.Center, true);
		TransformS.SetGlobalPosition(transformC, _data.position.ToVector3());
		TransformS.SetRotation(transformC, _data.rotation.ToVector3());
		TransformS.SetScale(transformC, _data.scale.ToVector3());
		if (_camera == Main.camera)
		{
			Vector3 position = Main.camera.transform.position;
			position.z = 0f;
			Vector3 position2 = Main.camera.WorldToScreenPoint(_data.position.ToVector3()) - new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
			position2.z = 0f;
			TransformS.SetGlobalPosition(transformC2, position2);
		}
		else
		{
			TransformS.SetGlobalPosition(transformC2, _data.position.ToVector3());
		}
		EIC eIC = GES.AddEditorItemComponent(_container, _identifier, _data, EditorItemType.PersistentItem, transformC, transformC2, touchAreaC);
		eIC.camera = _camera;
		return eIC;
	}

	public static void HandleEditorItemTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed || EditorState.m_isSelectionLocked)
		{
			return;
		}
		EIC eIC = _c.customComponent as EIC;
		if (_c.touchEvent[_i] != TouchEvent.Began)
		{
			return;
		}
		EditorState.SelectEditorItem(eIC.index);
		if (EditorState.m_gizmo == null || EditorState.m_selection.Count != 1 || EditorState.m_selection[0] != eIC || GEState.m_addDown || GEState.m_subDown)
		{
			return;
		}
		TLTouch t = InputManager.m_touches[_c.touchIndex[_i]];
		TouchAreaS.ReleaseTouches(eIC.TAC);
		TouchAreaS.ForceTouch(EditorState.m_gizmo.moveTAC, t, _i, TouchEvent.Began, false);
		if (!(eIC.identifier == "RailPoint") || !(eIC.container.identifier == "Rail"))
		{
			return;
		}
		for (int i = 0; i < eIC.container.subItems.Count; i++)
		{
			EIC eIC2 = eIC.container.subItems[i];
			if (Input.GetKey(KeyCode.LeftControl) && eIC2.identifier == "Rail Motor")
			{
				GEConstraintC gEConstraintC = eIC2.gameComponents[0] as GEConstraintC;
				ConstraintPointData constraintPointData = eIC.data as ConstraintPointData;
				ConstraintData constraintData = eIC2.data as ConstraintData;
				gEConstraintC.currentIndex = constraintPointData.anchorIndex;
				gEConstraintC.currentRailPos = 0f;
				constraintData.linearMotorStartIndex = (uint)constraintPointData.anchorIndex;
			}
		}
	}
}

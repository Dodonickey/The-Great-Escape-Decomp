using UnityEngine;

public static class GEAnchorA
{
	public static EIC Assemble(EIC _container, string _identifier, ILevelData _data, Camera _camera)
	{
		Entity entity = EntityManager.AddEntity(new string[3]
		{
			LevelManager.m_currentLevel.name,
			LevelManager.m_currentLevel.name + ":EditorItem",
			"EditorItem"
		});
		TransformC transformC = TransformS.AddComponent(entity);
		TransformC transformC2 = TransformS.AddComponent(entity);
		TouchAreaC touchAreaC = TouchAreaS.AddComponent(transformC, _identifier, 20f, true, Main.uiCamera, null);
		TouchAreaS.AddTouchEventListener(touchAreaC, GEItemA.HandleEditorItemTouches);
		TextS.SetStyle("body");
		TextC textC = TextS.AddSingleLineComponent(transformC2, _data.name, 1f, Align.Center, Align.Top);
		TransformS.SetPosition(textC.contentTC, Vector3.up * -15f);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(255f, 255f, 255f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, true);
		Vector2[] circle = DebugDraw.GetCircle(10f, 36, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(transformC2, Vector3.zero, circle, 4f, new Color(0.9f, 1f, 0.9f), ResourceManager.GetMaterial("Line4"), Main.uiCamera, Position.Center, true);
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
		EIC eIC = GES.AddEditorItemComponent(_container, _identifier, _data, EditorItemType.PersistentAnchor, transformC, transformC2, touchAreaC);
		eIC.camera = _camera;
		return eIC;
	}
}

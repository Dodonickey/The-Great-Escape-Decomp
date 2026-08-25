using UnityEngine;

public class PBBallA
{
	public static ChipmunkC Assemble(EIC _eic)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		BasicLevelData basicLevelData = _eic.data as BasicLevelData;
		TransformC transformC = TransformS.AddComponent(entity);
		TransformC transformC2 = TransformS.AddComponent(entity);
		TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
		transformC2.forceRotation = true;
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, false, (ColliderType)25, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, basicLevelData.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 10f, 8f, 0.25f, 0.5f, 0u, 17895697u, false);
		ChipmunkWrapper.SetCustomBodyGravity(chipmunkC.cpBodyPtr, Vector2.up * -350f);
		SpriteC c = SpriteS.AddComponent(transformC2, new Frame(342f, 0f, 144f, 144f), PBState.pinballSheet);
		SpriteS.SetDimensions(c, 16f, 16f);
		return chipmunkC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		BasicLevelData basicLevelData = new BasicLevelData();
		basicLevelData.position = new Vertex3(_pos);
		basicLevelData.rotation = new Vertex3(_rot);
		basicLevelData.scale = new Vertex3(_sca);
		uint uniqueId = GES.GetUniqueId();
		basicLevelData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, basicLevelData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = false;
		eIC.isScaleable = false;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		ChipmunkC chipmunkC = Assemble(_eic);
		_eic.gameComponents.Add(chipmunkC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(chipmunkC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
	}

	public static void HandlePropertyChange(EventC _c)
	{
		BasicLevelData basicLevelData = EditorState.m_selection[0].data as BasicLevelData;
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

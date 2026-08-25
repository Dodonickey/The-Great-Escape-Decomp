using UnityEngine;

public class PBRoundBumperA
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
		TransformC transformComponent = TransformS.AddComponent(entity);
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformComponent, true, (ColliderType)26, true, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, basicLevelData.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 100f, 15f, 0.5f, 1f, 0u, 17895697u, false);
		SpriteC c = SpriteS.AddComponent(transformComponent, new Frame(101f, 0f, 242f, 242f), PBState.pinballSheet);
		SpriteS.SetDimensions(c, 33f, 33f);
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

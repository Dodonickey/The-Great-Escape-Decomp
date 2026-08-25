using UnityEngine;

public static class FLevelItemA
{
	public static TransformC Assemble(EIC _eic)
	{
		PropData propData = _eic.data as PropData;
		ColliderType colliderType = (ColliderType)8;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Prop",
			LevelManager.m_currentLevel.name
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		PrefabC prefabC = null;
		GESpritePrefabC gESpritePrefabC = null;
		if (propData.assetIdentifier != string.Empty)
		{
			if (propData.isPrefab)
			{
				prefabC = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject(propData.assetIdentifier));
			}
			else if (propData.isSpritePrefab)
			{
				gESpritePrefabC = ((!(_eic.data.position.z > 50f)) ? SpritePrefabA.Assemble(transformC.entityIndex, propData.position.ToVector3(), propData.assetIdentifier, tags, colliderType, 0u, 0u, 0f - _eic.data.position.z, FarmState.propForegroundSheet) : SpritePrefabA.Assemble(transformC.entityIndex, propData.position.ToVector3(), propData.assetIdentifier, tags, colliderType, 0u, 0u, 0f - _eic.data.position.z, FarmState.propBackgroundSheet));
			}
		}
		if (gESpritePrefabC != null)
		{
			TransformS.ParentComponent(gESpritePrefabC.rootNode.TC, transformC, gESpritePrefabC.rootNode.globalPosition + gESpritePrefabC.rootNode.localCenter);
		}
		TransformS.SetGlobalPosition(transformC, propData.position.ToVector3());
		TransformS.SetRotation(transformC, propData.rotation.ToVector3());
		TransformS.SetScale(transformC, propData.scale.ToVector2());
		return transformC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		PropData propData = new PropData();
		propData.position = new Vertex3(_pos);
		propData.position.z = 90f;
		propData.rotation = new Vertex3(_rot);
		propData.scale = new Vertex3(_sca);
		switch (_identifier)
		{
		case "Plant1":
		case "Plant2":
		case "Plant3":
		case "Tree1":
			propData.isPrefab = true;
			break;
		default:
			propData.isSpritePrefab = true;
			break;
		}
		propData.assetIdentifier = _identifier;
		uint uniqueId = GES.GetUniqueId();
		propData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, propData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = true;
		eIC.isScaleable = true;
		eIC.isScaleUnified = true;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		TransformC transformC = Assemble(_eic);
		_eic.gameComponents.Add(transformC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(transformC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		PropData propData = _eiC.data as PropData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = NumericFieldA.Assemble(Main.uiCamera, "Z", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -200f, 200f, _eiC.data.position.z, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Location", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(RadioButtonA.Assemble(Main.uiCamera, "Background", HandlePropertyChange, null, true, Align.Right, 1f, propData.location == 2, 2, 102, tags), _propertyBar, true);
		UIS.AddToCanvasGrid(RadioButtonA.Assemble(Main.uiCamera, "Front", HandlePropertyChange, null, true, Align.Right, 1f, propData.location == 6, 6, 102, tags), _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Tweak", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandlePropertyChange(EventC _c)
	{
		PropData propData = EditorState.m_selection[0].data as PropData;
		switch (_c.identifier)
		{
		case "Background":
		{
			Vertex3 position = propData.position;
			position.z = 90f;
			propData.position = position;
			propData.location = int.Parse(_c.properties["value"].ToString());
			break;
		}
		case "Front":
		{
			Vertex3 position = propData.position;
			position.z = 10f;
			propData.position = position;
			propData.location = int.Parse(_c.properties["value"].ToString());
			break;
		}
		case "Z":
		{
			Vertex3 position = propData.position;
			position.z = (float)_c.properties["value"];
			propData.position = position;
			break;
		}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

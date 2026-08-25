using UnityEngine;

public static class GEVoxelShapeA
{
	public static GEVoxelShapeC Assemble(EIC _eic)
	{
		VoxelData voxelData = _eic.data as VoxelData;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, voxelData.position.ToVector3());
		GEVoxelShapeC gEVoxelShapeC = GEVoxelShapeS.AddComponent(transformC, Vector3.zero, voxelData);
		Vector2 position = voxelData.position.ToVector2();
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, voxelData.isStatic, (ColliderType)9, voxelData.colliderGroup, voxelData.colliderLayer, voxelData.isStatic, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBodyWithCustomProperties(chipmunkC.isStatic, chipmunkC.isRogue, position, chipmunkC.index, chipmunkC.colliderType, voxelData.groundSettings.linearDamp.ToVector2(), voxelData.groundSettings.angularDamp, voxelData.gravity.ToVector2()));
		gEVoxelShapeC.CMC = chipmunkC;
		return gEVoxelShapeC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		VoxelData voxelData = new VoxelData();
		voxelData.rotation = new Vertex3(_rot);
		voxelData.scale = new Vertex3(_sca);
		voxelData.position = new Vertex3(_pos);
		voxelData.groundSettings = new GroundSettings(GroundType.Solid);
		voxelData.groundSettings.elasticity = 1f;
		voxelData.groundSettings.friction = 1f;
		voxelData.convex = true;
		voxelData.separate = false;
		voxelData.gravity = new Vertex3(Vector2.up * -450f);
		voxelData.linearDamp = new Vertex3(Vector2.one * 0.995f);
		voxelData.angularDamp = 0.99f;
		voxelData.isStatic = true;
		voxelData.colliderGroup = 0u;
		voxelData.colliderLayer = GEState.layer_all;
		uint uniqueId = GES.GetUniqueId();
		voxelData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, voxelData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = true;
		eIC.TC.forceRotation = true;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		EIC eIC = GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
		eIC.TC.forceRotation = true;
		return eIC;
	}

	public static void FillEditorItem(EIC _eic)
	{
		GEVoxelShapeC gEVoxelShapeC = Assemble(_eic);
		_eic.gameComponents.Add(gEVoxelShapeC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gEVoxelShapeC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		VoxelData voxelData = _eiC.data as VoxelData;
		GroundSettings groundSettings = voxelData.groundSettings;
		Camera canvasCamera = _propertyBar.canvasCamera;
		if (voxelData.groundSettings.groundType == 3)
		{
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Grounds", tags), _propertyBar, true);
			for (int i = 0; i < GEState.groundMats.Count; i++)
			{
				GEMat gEMat = GEState.groundMats[i];
				UIC component = RadioButtonA.Assemble(canvasCamera, gEMat.name, HandleShapePropertyChange, null, true, Align.Bottom, 1f, false, i, 1000, tags);
				UIS.AddToCanvasGrid(component, _propertyBar, i % 4 == 0);
			}
			UIS.MoveCursor(_propertyBar, 0f, -15f);
		}
		else if (voxelData.groundSettings.groundType == 0)
		{
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Background", tags), _propertyBar, true);
			for (int j = 0; j < GEState.backgroundMats.Count; j++)
			{
				GEMat gEMat2 = GEState.backgroundMats[j];
				UIC component2 = RadioButtonA.Assemble(canvasCamera, gEMat2.name, HandleShapePropertyChange, null, true, Align.Bottom, 1f, false, j, 1000, tags);
				UIS.AddToCanvasGrid(component2, _propertyBar, j % 4 == 0);
			}
			UIS.MoveCursor(_propertyBar, 0f, -15f);
		}
		else if (voxelData.groundSettings.groundType == 1)
		{
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Landscape", tags), _propertyBar, true);
			for (int k = 0; k < GEState.landscapeMats.Count; k++)
			{
				GEMat gEMat3 = GEState.landscapeMats[k];
				UIC component3 = RadioButtonA.Assemble(canvasCamera, gEMat3.name, HandleShapePropertyChange, null, true, Align.Bottom, 1f, false, k, 1000, tags);
				UIS.AddToCanvasGrid(component3, _propertyBar, k % 4 == 0);
			}
			UIS.MoveCursor(_propertyBar, 0f, -15f);
		}
	}

	public static void HandleShapePropertyChange(EventC _c)
	{
		VoxelData voxelData = EditorState.m_selection[0].data as VoxelData;
		int valueFromRadioButtonGroup = UIS.GetValueFromRadioButtonGroup(1000);
		if (valueFromRadioButtonGroup > -1)
		{
			GEMat gEMat = null;
			if (voxelData.groundSettings.groundType == 3)
			{
				gEMat = GEState.groundMats[valueFromRadioButtonGroup];
			}
			else if (voxelData.groundSettings.groundType == 0)
			{
				gEMat = GEState.backgroundMats[valueFromRadioButtonGroup];
			}
			else if (voxelData.groundSettings.groundType == 1)
			{
				gEMat = GEState.landscapeMats[valueFromRadioButtonGroup];
			}
			voxelData.groundSettings.fillMaterialResourceIdentifier = gEMat.fill;
			voxelData.groundSettings.roadMaterialResourceIdentifier = gEMat.road;
			voxelData.groundSettings.fillScale = gEMat.fillScale;
			voxelData.groundSettings.roadScale = gEMat.roadScale;
			voxelData.groundSettings.beltWidth = gEMat.beltWidth;
			voxelData.groundSettings.beltDepth = gEMat.beltDepth;
			voxelData.groundSettings.smooth = gEMat.smooth;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

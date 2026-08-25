using UnityEngine;

public static class GEShapeA
{
	public static GEShapeC Assemble(Camera _camera, ShapeData _shapeData, Polygon _modifiedPoly)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		GpcC gpcC = GpcS.AddComponent(transformC);
		gpcC.originalPolygon = _shapeData.polygon;
		gpcC.modifiedPolygon = _modifiedPoly;
		return GES.AddShapeComponent(entity, transformC, gpcC, _shapeData);
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		ShapeData shapeData = new ShapeData();
		shapeData.rotation = new Vertex3(_rot);
		shapeData.scale = new Vertex3(_sca);
		switch (_identifier)
		{
		case "Ground":
			shapeData.groundSettings = new GroundSettings(GroundType.Solid);
			shapeData.position = new Vertex3(_pos);
			break;
		case "Background":
			shapeData.groundSettings = new GroundSettings(GroundType.Background);
			shapeData.position = new Vertex3(_pos);
			shapeData.position.z = GEState.defaultBackgroundDepth;
			break;
		case "Landscape":
			shapeData.groundSettings = new GroundSettings(GroundType.Landscape);
			shapeData.position = new Vertex3(_pos);
			shapeData.position.z = GEState.defaultBackgroundDepth;
			break;
		}
		shapeData.tiled = true;
		shapeData.tileSize = 200;
		Vector2[] rect = DebugDraw.GetRect(100f, 50f, Vector2.zero, false);
		shapeData.polygon = new Polygon();
		shapeData.polygon.AddContour(new VertexList(rect), false);
		uint uniqueId = GES.GetUniqueId();
		shapeData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, shapeData, Main.camera);
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
		ShapeData shapeData = _eic.data as ShapeData;
		Polygon polygon = GpcS.CleanPolygon(shapeData.polygon, shapeData.groundSettings.minSegment, shapeData.groundSettings.minAngle, shapeData.groundSettings.maxSegment, shapeData.convex);
		polygon = GpcS.SmoothPolygon(polygon, shapeData.groundSettings.smooth);
		GEShapeC gEShapeC = Assemble(_eic.camera, shapeData, polygon);
		gEShapeC.GPC.tileWidth = (_eic.data as ShapeData).tileSize;
		gEShapeC.GPC.tileHeight = (_eic.data as ShapeData).tileSize;
		_eic.gameComponents.Add(gEShapeC);
		TransformS.ParentComponent(gEShapeC.TC, _eic.TC, Vector3.zero);
		GEState.generateShapes = true;
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		ShapeData shapeData = _eiC.data as ShapeData;
		GroundSettings groundSettings = shapeData.groundSettings;
		Camera canvasCamera = _propertyBar.canvasCamera;
		if (shapeData.groundSettings.groundType == 3)
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
		else if (shapeData.groundSettings.groundType == 0)
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
		else if (shapeData.groundSettings.groundType == 1)
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
		UIC component4 = NumericFieldA.Assemble(canvasCamera, "Min Seg Len", HandleShapePropertyChange, null, true, Align.Left, 80f, 1f, false, 1f, 5f, groundSettings.minSegment, tags);
		UIC component5 = NumericFieldA.Assemble(canvasCamera, "Max Seg Len", HandleShapePropertyChange, null, true, Align.Left, 80f, 1f, false, 10f, 200f, groundSettings.maxSegment, tags);
		UIC component6 = NumericFieldA.Assemble(canvasCamera, "Min Angle", HandleShapePropertyChange, null, true, Align.Left, 80f, 1f, false, 1f, 45f, groundSettings.minAngle, tags);
		UIC component7 = NumericFieldA.Assemble(canvasCamera, "Smooth Mult", HandleShapePropertyChange, null, true, Align.Left, 80f, 1f, true, 0f, 5f, groundSettings.smooth, tags);
		UIC component8 = null;
		UIC component9 = null;
		UIC component10 = null;
		UIC component11 = null;
		UIC component12 = null;
		if (shapeData.groundSettings.groundType == 3)
		{
			component8 = CheckBoxA.Assemble(canvasCamera, "Bewel", HandleShapePropertyChange, null, true, Align.Right, 1f, groundSettings.hasBelt, tags);
			component9 = RadioButtonA.Assemble(canvasCamera, "Default", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.beltType == 0, 0, 103, tags);
			component10 = NumericFieldA.Assemble(canvasCamera, "Belt Width", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, 1f, 50f, groundSettings.beltWidth, tags);
			component11 = NumericFieldA.Assemble(canvasCamera, "Belt Weight X", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, -1f, 1f, groundSettings.beltWeightDirection.x, tags);
			component12 = NumericFieldA.Assemble(canvasCamera, "Belt Weight Y", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, -1f, 1f, groundSettings.beltWeightDirection.y, tags);
		}
		UIC component13 = NumericFieldA.Assemble(canvasCamera, "Fill Scale", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, 0.1f, 10f, groundSettings.fillScale, tags);
		UIC component14 = NumericFieldA.Assemble(canvasCamera, "Top R", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, (groundSettings.color2 >> 16) & 0xFF, tags);
		UIC component15 = NumericFieldA.Assemble(canvasCamera, "Top G", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, (groundSettings.color2 >> 8) & 0xFF, tags);
		UIC component16 = NumericFieldA.Assemble(canvasCamera, "Top B", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, groundSettings.color2 & 0xFF, tags);
		UIC component17 = NumericFieldA.Assemble(canvasCamera, "Bottom R", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, (groundSettings.color1 >> 16) & 0xFF, tags);
		UIC component18 = NumericFieldA.Assemble(canvasCamera, "Bottom G", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, (groundSettings.color1 >> 8) & 0xFF, tags);
		UIC component19 = NumericFieldA.Assemble(canvasCamera, "Bottom B", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, groundSettings.color1 & 0xFF, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Cleanup", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component4, _propertyBar, true);
		UIS.AddToCanvasGrid(component5, _propertyBar, false);
		UIS.AddToCanvasGrid(component6, _propertyBar, true);
		UIS.AddToCanvasGrid(component7, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		if (shapeData.groundSettings.groundType == 3)
		{
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Belt", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(component8, _propertyBar, true);
			UIS.AddToCanvasGrid(component9, _propertyBar, true);
			UIS.AddToCanvasGrid(component10, _propertyBar, true);
			UIS.AddToCanvasGrid(component11, _propertyBar, false);
			UIS.AddToCanvasGrid(component12, _propertyBar, false);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
		}
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Material", tags), _propertyBar, true);
		if (shapeData.groundSettings.groundType == 1)
		{
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "Mountain", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.fillMaterialResourceIdentifier == "landscape_mountain", 0, 102, tags), _propertyBar, true);
		}
		if (shapeData.groundSettings.groundType == 0)
		{
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "Solid", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.fillMaterialResourceIdentifier == "background_solid", 1, 102, tags), _propertyBar, true);
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "Foliage", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.fillMaterialResourceIdentifier == "background_foliage", 2, 102, tags), _propertyBar, false);
		}
		else
		{
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "Grass", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.roadMaterialResourceIdentifier == "belt_grass", 3, 102, tags), _propertyBar, true);
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "Mud", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.roadMaterialResourceIdentifier == "belt_mud", 4, 102, tags), _propertyBar, false);
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "Rock", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.roadMaterialResourceIdentifier == "belt_rock", 5, 102, tags), _propertyBar, false);
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "Snow", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.roadMaterialResourceIdentifier == "belt_snow", 6, 102, tags), _propertyBar, false);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
		}
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(component13, _propertyBar, true);
		UIS.AddToCanvasGrid(component14, _propertyBar, true);
		UIS.AddToCanvasGrid(component15, _propertyBar, false);
		UIS.AddToCanvasGrid(component16, _propertyBar, false);
		UIS.AddToCanvasGrid(component17, _propertyBar, true);
		UIS.AddToCanvasGrid(component18, _propertyBar, false);
		UIS.AddToCanvasGrid(component19, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		if (shapeData.groundSettings.groundType == 3 || shapeData.groundSettings.groundType == 2)
		{
			UIC component20 = NumericFieldA.Assemble(canvasCamera, "Density", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, 0.1f, 10f, groundSettings.density, tags);
			UIC component21 = NumericFieldA.Assemble(canvasCamera, "Elasticity", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, groundSettings.elasticity, tags);
			UIC component22 = NumericFieldA.Assemble(canvasCamera, "Friction", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 5f, groundSettings.friction, tags);
			UIC component23 = NumericFieldA.Assemble(canvasCamera, "Angular Damp", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, groundSettings.angularDamp, tags);
			UIC component24 = NumericFieldA.Assemble(canvasCamera, "Linear Damp X", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, -0f, 1f, groundSettings.linearDamp.x, tags);
			UIC component25 = NumericFieldA.Assemble(canvasCamera, "Linear Damp Y", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, -0f, 1f, groundSettings.linearDamp.y, tags);
			UIC component26 = NumericFieldA.Assemble(canvasCamera, "Surface Vel X", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, -100f, 100f, groundSettings.surfaceVelocity.x, tags);
			UIC component27 = NumericFieldA.Assemble(canvasCamera, "Surface Vel Y", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, -100f, 100f, groundSettings.surfaceVelocity.y, tags);
			UIS.AddToCanvasGrid(component20, _propertyBar, true);
			UIS.AddToCanvasGrid(component21, _propertyBar, false);
			UIS.AddToCanvasGrid(component22, _propertyBar, false);
			UIS.AddToCanvasGrid(component23, _propertyBar, true);
			UIS.AddToCanvasGrid(component24, _propertyBar, false);
			UIS.AddToCanvasGrid(component25, _propertyBar, false);
			UIS.AddToCanvasGrid(component26, _propertyBar, true);
			UIS.AddToCanvasGrid(component27, _propertyBar, false);
			UIS.MoveCursor(_propertyBar, 0f, -15f);
			UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Effect", tags), _propertyBar, true);
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "ENone", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.effectIdentifier == 0, 0, 103, tags), _propertyBar, true);
			UIS.AddToCanvasGrid(RadioButtonA.Assemble(canvasCamera, "EDamaging", HandleShapePropertyChange, null, true, Align.Bottom, 1f, groundSettings.effectIdentifier == 1, 1, 103, tags), _propertyBar, false);
			UIC component28 = NumericFieldA.Assemble(canvasCamera, "Effect Interval", HandleShapePropertyChange, null, true, Align.Left, 50f, 1f, false, 0f, 1f, groundSettings.effectInterval, tags);
			UIS.AddToCanvasGrid(component28, _propertyBar, true);
		}
	}

	public static void HandleShapePropertyChange(EventC _c)
	{
		ShapeData shapeData = EditorState.m_selection[0].data as ShapeData;
		int num = ((int)shapeData.groundSettings.color2 >> 16) & 0xFF;
		int num2 = ((int)shapeData.groundSettings.color2 >> 8) & 0xFF;
		int num3 = (int)(shapeData.groundSettings.color2 & 0xFF);
		int num4 = ((int)shapeData.groundSettings.color1 >> 16) & 0xFF;
		int num5 = ((int)shapeData.groundSettings.color1 >> 8) & 0xFF;
		int num6 = (int)(shapeData.groundSettings.color1 & 0xFF);
		int valueFromRadioButtonGroup = UIS.GetValueFromRadioButtonGroup(1000);
		if (valueFromRadioButtonGroup > -1)
		{
			GEMat gEMat = null;
			if (shapeData.groundSettings.groundType == 3)
			{
				gEMat = GEState.groundMats[valueFromRadioButtonGroup];
			}
			else if (shapeData.groundSettings.groundType == 0)
			{
				gEMat = GEState.backgroundMats[valueFromRadioButtonGroup];
			}
			else if (shapeData.groundSettings.groundType == 1)
			{
				gEMat = GEState.landscapeMats[valueFromRadioButtonGroup];
			}
			shapeData.groundSettings.fillMaterialResourceIdentifier = gEMat.fill;
			shapeData.groundSettings.roadMaterialResourceIdentifier = gEMat.road;
			shapeData.groundSettings.fillScale = gEMat.fillScale;
			shapeData.groundSettings.roadScale = gEMat.roadScale;
			shapeData.groundSettings.beltWidth = gEMat.beltWidth;
			shapeData.groundSettings.beltDepth = gEMat.beltDepth;
			shapeData.groundSettings.smooth = gEMat.smooth;
			shapeData.groundSettings.hasRoad = gEMat.hasRoad;
			shapeData.groundSettings.hasFill = gEMat.hasFill;
			shapeData.groundSettings.hasBelt = gEMat.hasBelt;
		}
		switch (_c.identifier)
		{
		case "Bewel":
			shapeData.groundSettings.hasBelt = (bool)_c.properties["checked"];
			break;
		case "Default":
			shapeData.groundSettings.beltType = 0u;
			break;
		case "Belt Width":
			shapeData.groundSettings.beltWidth = (float)_c.properties["value"];
			break;
		case "Belt Weight X":
			shapeData.groundSettings.beltWeightDirection.x = (float)_c.properties["value"];
			break;
		case "Belt Weight Y":
			shapeData.groundSettings.beltWeightDirection.y = (float)_c.properties["value"];
			break;
		case "Top R":
			shapeData.groundSettings.color2 = (uint)((Mathf.RoundToInt((float)_c.properties["value"]) << 16) | (num2 << 8) | num3);
			break;
		case "Top G":
			shapeData.groundSettings.color2 = (uint)((num << 16) | (Mathf.RoundToInt((float)_c.properties["value"]) << 8) | num3);
			break;
		case "Top B":
			shapeData.groundSettings.color2 = (uint)((num << 16) | (num2 << 8) | Mathf.RoundToInt((float)_c.properties["value"]));
			break;
		case "Bottom R":
			shapeData.groundSettings.color1 = (uint)((Mathf.RoundToInt((float)_c.properties["value"]) << 16) | (num5 << 8) | num6);
			break;
		case "Bottom G":
			shapeData.groundSettings.color1 = (uint)((num4 << 16) | (Mathf.RoundToInt((float)_c.properties["value"]) << 8) | num6);
			break;
		case "Bottom B":
			shapeData.groundSettings.color1 = (uint)((num4 << 16) | (num5 << 8) | Mathf.RoundToInt((float)_c.properties["value"]));
			break;
		case "Min Seg Len":
			shapeData.groundSettings.minSegment = (float)_c.properties["value"];
			break;
		case "Max Seg Len":
			shapeData.groundSettings.maxSegment = (float)_c.properties["value"];
			break;
		case "Min Angle":
			shapeData.groundSettings.minAngle = (float)_c.properties["value"];
			break;
		case "Smooth Mult":
			shapeData.groundSettings.smooth = int.Parse(_c.properties["value"].ToString());
			break;
		case "Density":
			shapeData.groundSettings.density = (float)_c.properties["value"];
			break;
		case "Elasticity":
			shapeData.groundSettings.elasticity = (float)_c.properties["value"];
			break;
		case "Friction":
			shapeData.groundSettings.friction = (float)_c.properties["value"];
			break;
		case "Angular Damp":
			shapeData.groundSettings.angularDamp = (float)_c.properties["value"];
			break;
		case "Linear Damp X":
			shapeData.groundSettings.linearDamp.x = (float)_c.properties["value"];
			break;
		case "Linear Damp Y":
			shapeData.groundSettings.linearDamp.y = (float)_c.properties["value"];
			break;
		case "Surface Vel X":
			shapeData.groundSettings.surfaceVelocity.x = (float)_c.properties["value"];
			break;
		case "Surface Vel Y":
			shapeData.groundSettings.surfaceVelocity.y = (float)_c.properties["value"];
			break;
		case "Effect Interval":
			shapeData.groundSettings.effectInterval = (float)_c.properties["value"];
			break;
		case "Buff Interval":
			shapeData.groundSettings.buffInterval = (float)_c.properties["value"];
			break;
		case "ENone":
		case "EDamaging":
		case "EHealing":
		{
			uint valueFromRadioButtonGroup3 = (uint)UIS.GetValueFromRadioButtonGroup(103);
			shapeData.groundSettings.effectIdentifier = valueFromRadioButtonGroup3;
			break;
		}
		case "BNone":
		case "BDamaging":
		case "BHealing":
		{
			uint valueFromRadioButtonGroup2 = (uint)UIS.GetValueFromRadioButtonGroup(104);
			shapeData.groundSettings.buffIdentifier = valueFromRadioButtonGroup2;
			break;
		}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

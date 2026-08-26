using System.Collections.Generic;
using UnityEngine;

public static class GELevelGenerator
{
	public static uint[] m_idConversionArray;

	public static ILevel GenerateLevel(ILevel _level)
	{
		m_idConversionArray = new uint[10000];
		GELevel gELevel = (GELevel)(LevelManager.m_currentLevel = new GELevel());
		if (_level == null)
		{
			gELevel.name = "MyLevel";
			gELevel.projectCode = Main.m_currentGame.m_projectCode;
			gELevel.levelIndex = LevelManager.m_currentLevelIndex;
			gELevel.chapterIndex = LevelManager.m_currentChapterIndex;
			if (GEState.editorMode)
			{
				GEEditorCanvasA.Assemble();
			}
			gELevel.items = new List<EIC>();
			gELevel.connections = new List<EIC>();
			gELevel.requiredResources = new List<string>();
		}
		else
		{
			GELevel gELevel2 = _level as GELevel;
			gELevel.name = gELevel2.name;
			gELevel.chapterIndex = gELevel2.chapterIndex;
			gELevel.levelIndex = gELevel2.levelIndex;
			if (gELevel.chapterIndex != 0)
			{
				LevelManager.m_currentChapterIndex = gELevel.chapterIndex;
			}
			if (gELevel.levelIndex != 0)
			{
				LevelManager.m_currentLevelIndex = gELevel.levelIndex;
			}
			gELevel.projectCode = Main.m_currentGame.m_projectCode;
			gELevel.requiredResources = new List<string>();
			if (GEState.editorMode)
			{
				GEEditorCanvasA.Assemble();
			}
			if (gELevel2.requiredResources != null)
			{
				for (int i = 0; i < gELevel2.requiredResources.Count; i++)
				{
					ResourceManager.LoadResource(gELevel2.requiredResources[i]);
				}
			}
			gELevel.items = new List<EIC>();
			gELevel.connections = new List<EIC>();
			if (gELevel2.items != null)
			{
				for (int j = 0; j < gELevel2.items.Count; j++)
				{
					CreateEditorItem(gELevel.items, gELevel2.items[j], null);
				}
			}
			if (gELevel2.connections != null)
			{
				for (int k = 0; k < gELevel2.connections.Count; k++)
				{
					CreateEditorItem(gELevel.connections, gELevel2.connections[k], null);
				}
			}
		}
		if (GEState.generateShapes)
		{
			CreateShapes();
		}
		if (!GEState.editorMode)
		{
			EntityManager.RemoveEntitiesByTag("EditorItem", false);
		}
		Main.m_currentGame.GetCurrentScene().DestroyLoadingScreen();
		return gELevel;
	}

	public static void CreateEditorItem(List<EIC> _itemList, EIC _item, EIC _container)
	{
		EIC eIC = EditorState.CreateLoadedEditorItem(_container, _item);
		if (eIC == null)
		{
			return;
		}
		if (_item.data.position != null && eIC.TC != null)
		{
			eIC.TC.transform.position = _item.data.position.ToVector3();
			eIC.TC.transform.rotation = Quaternion.Euler(_item.data.rotation.ToVector3());
		}
		for (int i = 0; i < _item.subItems.Count; i++)
		{
			CreateEditorItem(_itemList, _item.subItems[i], eIC);
		}
		if (_container == null)
		{
			_itemList.Add(eIC);
			EditorState.FillEditorItemHierarchy(eIC);
		}
		if (!(eIC.camera == Main.uiCamera))
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		if (eIC.horizontalAnchor == 0)
		{
			if (eIC.horizontalIsAbsolute)
			{
				zero.x = eIC.data.position.x;
			}
			else
			{
				zero.x = eIC.data.position.x / eIC.referenceWidth * (float)Screen.width;
			}
		}
		else if (eIC.horizontalAnchor == 1)
		{
			if (eIC.horizontalIsAbsolute)
			{
				zero.x = eIC.data.position.x + eIC.referenceWidth * 0.5f + (float)Screen.width * -0.5f;
			}
			else
			{
				zero.x = (eIC.data.position.x + eIC.referenceWidth * 0.5f) / eIC.referenceWidth * (float)Screen.width + (float)Screen.width * -0.5f;
			}
		}
		else if (eIC.horizontalAnchor == 2)
		{
			if (eIC.horizontalIsAbsolute)
			{
				zero.x = eIC.data.position.x - eIC.referenceWidth * 0.5f + (float)Screen.width * 0.5f;
			}
			else
			{
				zero.x = (eIC.data.position.x - eIC.referenceWidth * 0.5f) / eIC.referenceWidth * (float)Screen.width + (float)Screen.width * 0.5f;
			}
		}
		if (eIC.verticalAnchor == 0)
		{
			if (eIC.verticalIsAbsolute)
			{
				zero.y = eIC.data.position.y;
			}
			else
			{
				zero.y = eIC.data.position.y / eIC.referenceHeight * (float)Screen.height;
			}
		}
		else if (eIC.verticalAnchor == 1)
		{
			if (eIC.verticalIsAbsolute)
			{
				zero.y = eIC.data.position.y - eIC.referenceHeight * 0.5f + (float)Screen.height * 0.5f;
			}
			else
			{
				zero.y = (eIC.data.position.y - eIC.referenceHeight * 0.5f) / eIC.referenceHeight * (float)Screen.height + (float)Screen.height * 0.5f;
			}
		}
		else if (eIC.verticalAnchor == 2)
		{
			if (eIC.verticalIsAbsolute)
			{
				zero.y = eIC.data.position.y + eIC.referenceHeight * 0.5f + (float)Screen.height * -0.5f;
			}
			else
			{
				zero.y = (eIC.data.position.y + eIC.referenceHeight * 0.5f) / eIC.referenceHeight * (float)Screen.height + (float)Screen.height * -0.5f;
			}
		}
		zero.z = -200f;
		eIC.data.position = new Vertex3(zero);
		if (eIC.TC != null)
		{
			eIC.TC.transform.position = zero;
		}
		EditorState.ResetEditorItem(eIC);
		eIC.referenceHeight = Screen.height;
		eIC.referenceWidth = Screen.width;
	}

	private static void GetOrderedIndices(List<int> _list, EIC _item)
	{
		for (int num = _item.subItems.Count - 1; num > -1; num--)
		{
			GetOrderedIndices(_list, _item.subItems[num]);
			if ((_item.subItems[num].identifier == "Ground" || _item.subItems[num].identifier == "Background" || _item.subItems[num].identifier == "Landscape") && _item.subItems[num].gameComponents.Count > 0)
			{
				_list.Add(_item.subItems[num].gameComponents[0].index);
			}
		}
	}

	public static void CreateShapes()
	{
		GEState.generateShapes = false;
		bool flag = false;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			bool flag2 = gEPlugin.CreateShapes();
			if (!flag)
			{
				flag = gEPlugin.CreateShapes();
			}
		}
		if (flag)
		{
			return;
		}
		for (int j = 0; j < GES.m_shapeComponents.m_aliveCount; j++)
		{
			GEShapeC gEShapeC = GES.m_shapeComponents.m_array[GES.m_shapeComponents.m_aliveIndices[j]];
			if (gEShapeC.active)
			{
				GroundSettings groundSettings = gEShapeC.groundSettings;
				gEShapeC.GPC.modifiedPolygon = GpcS.CleanPolygon(gEShapeC.GPC.originalPolygon, groundSettings.minSegment, groundSettings.minAngle, groundSettings.maxSegment, false);
				gEShapeC.GPC.modifiedPolygon = GpcS.SmoothPolygon(gEShapeC.GPC.modifiedPolygon, groundSettings.smooth);
				gEShapeC.GPC.modifiedPolygon = DebugDraw.TransformPolygon(gEShapeC.GPC.modifiedPolygon, gEShapeC.TC);
			}
		}
		List<int> list = new List<int>();
		GELevel gELevel = LevelManager.m_currentLevel as GELevel;
		for (int num = gELevel.items.Count - 1; num > -1; num--)
		{
			GetOrderedIndices(list, gELevel.items[num]);
			if ((gELevel.items[num].identifier == "Ground" || gELevel.items[num].identifier == "Background" || gELevel.items[num].identifier == "Landscape") && gELevel.items[num].gameComponents.Count > 0)
			{
				list.Add(gELevel.items[num].gameComponents[0].index);
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			GEShapeC gEShapeC2 = GES.m_shapeComponents.m_array[list[k]];
			float _distance = 0f;
			PrefabS.RemoveComponentsByEntityIndex(gEShapeC2.entityIndex);
			List<IComponent> componentsByEntityIndex = EntityManager.GetComponentsByEntityIndex(ComponentType.Chipmunk, gEShapeC2.entityIndex);
			while (componentsByEntityIndex.Count > 0)
			{
				int index = componentsByEntityIndex.Count - 1;
				ChipmunkS.RemoveComponent(componentsByEntityIndex[index] as ChipmunkC);
				componentsByEntityIndex.RemoveAt(index);
			}
			for (int l = k + 1; l < list.Count; l++)
			{
				GEShapeC gEShapeC3 = GES.m_shapeComponents.m_array[list[l]];
				if (gEShapeC2.GPC.modifiedPolygon.NofContours > 0 && ((gEShapeC2.groundSettings.groundType != 0 && gEShapeC2.groundSettings.groundType != 1) || gEShapeC2.TC.transform.position.z == GEState.defaultBackgroundDepth) && ((gEShapeC3.groundSettings.groundType != 0 && gEShapeC3.groundSettings.groundType != 1) || gEShapeC3.TC.transform.position.z == GEState.defaultBackgroundDepth))
				{
					gEShapeC2.GPC.modifiedPolygon = gEShapeC2.GPC.modifiedPolygon.Clip(GpcOperation.Difference, gEShapeC3.GPC.modifiedPolygon);
				}
			}
			gEShapeC2.GPC.modifiedPolygon = DebugDraw.TransformPolygon(gEShapeC2.GPC.modifiedPolygon, -gEShapeC2.TC.transform.position, 0f - gEShapeC2.TC.transform.eulerAngles.z);
			if (gEShapeC2.GPC.modifiedPolygon == null || gEShapeC2.GPC.modifiedPolygon.NofContours <= 0)
			{
				continue;
			}
			float num2 = 0f;
			Vector2 zero = Vector2.zero;
			List<float> list2 = new List<float>();
			for (int m = 0; m < gEShapeC2.GPC.originalPolygon.NofContours; m++)
			{
				Vector2 zero2 = Vector2.zero;
				float num3 = 0f;
				int nofVertices = gEShapeC2.GPC.originalPolygon.Contour[m].NofVertices;
				for (int n = 0; n < nofVertices; n++)
				{
					Vector2 vector = gEShapeC2.GPC.originalPolygon.Contour[m].Vertex[n];
					int num4 = n + 1;
					if (num4 == nofVertices)
					{
						num4 = 0;
					}
					Vector2 vector2 = gEShapeC2.GPC.originalPolygon.Contour[m].Vertex[num4];
					num3 += vector.x * vector2.y - vector2.x * vector.y;
					zero2.x += (vector.x + vector2.x) * (vector.x * vector2.y - vector2.x * vector.y);
					zero2.y += (vector.y + vector2.y) * (vector.x * vector2.y - vector2.x * vector.y);
				}
				num3 *= -0.5f;
				zero2 *= 1f / (6f * (0f - num3));
				if (num3 > 25f)
				{
					num2 += num3;
					zero += zero2 * num3;
				}
				list2.Add(num3);
			}
			zero /= num2;
			for (int num5 = 0; num5 < gEShapeC2.GPC.originalPolygon.NofContours; num5++)
			{
				for (int num6 = 0; num6 < gEShapeC2.GPC.originalPolygon.Contour[num5].Vertex.Length; num6++)
				{
					gEShapeC2.GPC.originalPolygon.Contour[num5].Vertex[num6] -= zero;
				}
			}
			List<IComponent> componentsByEntityIndex2 = EntityManager.GetComponentsByEntityIndex((ComponentType)106, gEShapeC2.TC.parent.entityIndex);
			EIC eIC = componentsByEntityIndex2[0] as EIC;
			TransformS.m_transformHelper.transform.rotation = Quaternion.identity;
			TransformS.m_transformHelper.transform.position = zero;
			TransformS.m_transformHelper.transform.RotateAround(Vector3.zero, Vector3.forward, eIC.data.rotation.z);
			Vector2 vector3 = TransformS.m_transformHelper.transform.position;
			Vector2 vector4 = eIC.data.position.ToVector2();
			Vector2 vector5 = vector4 + vector3;
			TransformS.SetGlobalPositionWithoutChildren(eIC.TC, new Vector3(vector5.x, vector5.y, eIC.data.position.z));
			eIC.data.position = new Vertex3(new Vector3(vector5.x, vector5.y, eIC.data.position.z));
			ColliderType colliderType = ColliderType.Any;
			if (gEShapeC2.groundSettings.groundType != 0 && gEShapeC2.groundSettings.groundType != 1)
			{
				if (gEShapeC2.groundSettings.groundType == 2)
				{
					colliderType = (ColliderType)5;
				}
				else if (gEShapeC2.groundSettings.groundType == 3)
				{
					colliderType = (ColliderType)9;
				}
				TransformS.SetGlobalRotation(gEShapeC2.TC, eIC.data.rotation.ToVector3());
				ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(gEShapeC2.TC, false, colliderType, true, false);
				ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, vector5 - vector3, chipmunkC.index, chipmunkC.colliderType));
				ChipmunkS.CreateSegmentShapesFromPolygon(chipmunkC, gEShapeC2.GPC.modifiedPolygon, GEState.layer_solid, gEShapeC2.groundSettings.elasticity, gEShapeC2.groundSettings.friction, 5f);
				chipmunkC.customComponent = gEShapeC2;
				Vector2 vector6 = gEShapeC2.groundSettings.surfaceVelocity.ToVector2();
				if (vector6 != Vector2.zero)
				{
					ChipmunkWrapper.SetBodySurfaceVelocity(chipmunkC.cpBodyPtr, vector6);
				}
			}
			TransformS.SetPosition(gEShapeC2.TC, -zero);
			gEShapeC2.GPC.tileWidth = 300;
			gEShapeC2.GPC.tileHeight = 300;
			GpcS.SplitPolygonToTiles(gEShapeC2.GPC, gEShapeC2.GPC.tileWidth, gEShapeC2.GPC.tileHeight);
			List<List<Vector2>> list3 = new List<List<Vector2>>();
			for (int num7 = 0; num7 < gEShapeC2.GPC.tiles.Length; num7++)
			{
				if (gEShapeC2.groundSettings.groundType == 3)
				{
					List<List<Vector2>> collection = SplitShape(gEShapeC2, num7);
					list3.AddRange(collection);
					continue;
				}
				PrefabC[] array = CreateFlatPrefabComponentsFromPolygon(gEShapeC2, (Vector3)(-vector5 + vector3) - Vector3.forward * eIC.TC.transform.position.z, gEShapeC2.GPC.tiles[num7], Main.camera, string.Empty, 0f).ToArray();
				CombineInstance[] array2 = new CombineInstance[array.Length];
				for (int num8 = 0; num8 < array2.Length; num8++)
				{
					array2[num8].mesh = array[num8].p_mesh;
					array2[num8].transform = array[num8].p_gameObject.transform.localToWorldMatrix;
				}
				PrefabC prefabC = PrefabS.AddComponent(gEShapeC2.TC, Vector3.zero);
				prefabC.p_mesh.CombineMeshes(array2);
				var o_351_4_639233471813034468 = prefabC.p_mesh;
				Material material = ResourceManager.GetMaterial(gEShapeC2.groundSettings.fillMaterialResourceIdentifier);
				if (material == null)
				{
					material = ResourceManager.GetMaterial("Solid");
				}
				material.mainTextureScale = Vector2.one * gEShapeC2.groundSettings.fillScale;
				if (array.Length > 0)
				{
					prefabC.p_renderer.material = material;
				}
				for (int num9 = 0; num9 < array2.Length; num9++)
				{
					PrefabS.RemoveComponent(array[num9]);
				}
				gEShapeC2.PCs = array;
			}
			if (gEShapeC2.groundSettings.groundType != 3)
			{
				continue;
			}
			List<PrefabC> list4 = new List<PrefabC>();
			bool[] handledSplits = new bool[list3.Count];
			int[] array3 = OrderSplits(list3, handledSplits);
			if (gEShapeC2.groundSettings.hasBelt)
			{
				List<List<SKArc>> _arcs = new List<List<SKArc>>();
				SKBase sKBase = GEStraightSkeleton.GenerateStraightSkeletonFromGroundSplits(list3, array3, ref _arcs, gEShapeC2.groundSettings.beltWidth, 1f, gEShapeC2.groundSettings.beltWeightDirection.ToVector2());
				if (gEShapeC2.groundSettings.hasRoad)
				{
					for (int num10 = 0; num10 < array3.Length; num10++)
					{
						int index2 = array3[num10];
						list4.AddRange(GenerateRoad(Main.camera, gEShapeC2, list3[index2].ToArray(), _arcs[num10].ToArray(), ref _distance).ToArray());
					}
				}
				Polygon polygon = new Polygon();
				int num11 = 0;
				List<Vector2> list5 = new List<Vector2>();
				int num12 = 0;
				while (sKBase.wavefronts.Count > 0 && num12 < 1000)
				{
					SKWavefront sKWavefront = sKBase.wavefronts[0];
					list5.Add(sKWavefront.a1.nB.pos);
					SKWavefront sKWavefront2 = sKWavefront.next;
					int num13 = 0;
					while (sKWavefront2 != sKWavefront && num13 < 1000)
					{
						list5.Add(sKWavefront2.a1.nB.pos);
						SKWavefront next = sKWavefront2.next;
						sKBase.wavefronts.Remove(sKWavefront2);
						sKWavefront2 = next;
						num13++;
					}
					if (sKWavefront2 == sKWavefront)
					{
						sKBase.wavefronts.Remove(sKWavefront);
						polygon.AddContour(new VertexList(list5.ToArray()), false);
						list5.Clear();
					}
					num12++;
					if (num12 == 1000)
					{
						Debug.Log("infinite loop");
					}
				}
				for (int num14 = 0; num14 < gEShapeC2.GPC.tiles.Length; num14++)
				{
					if (gEShapeC2.GPC.tiles[num14].NofContours > 0 && polygon.NofContours > 0)
					{
						Polygon polygon2 = gEShapeC2.GPC.tiles[num14].Clip(GpcOperation.Intersection, polygon);
						PrefabC[] array4 = CreateFlatPrefabComponentsFromPolygon(gEShapeC2, (Vector3)(-vector5 + vector3) - Vector3.forward * eIC.TC.transform.position.z, polygon2, Main.camera, string.Empty, 0f).ToArray();
						CombineInstance[] array5 = new CombineInstance[array4.Length];
						for (int num15 = 0; num15 < array5.Length; num15++)
						{
							array5[num15].mesh = array4[num15].p_mesh;
							array5[num15].transform = array4[num15].p_gameObject.transform.localToWorldMatrix;
						}
						PrefabC prefabC2 = PrefabS.AddComponent(gEShapeC2.TC, Vector3.zero);
						prefabC2.p_mesh.CombineMeshes(array5);
						var o_431_6_639233471813347226 = prefabC2.p_mesh;
						Material material2 = ResourceManager.GetMaterial(gEShapeC2.groundSettings.fillMaterialResourceIdentifier);
						if (material2 == null)
						{
							material2 = ResourceManager.GetMaterial("Solid");
						}
						material2.mainTextureScale = Vector2.one * gEShapeC2.groundSettings.fillScale;
						if (array4.Length > 0)
						{
							prefabC2.p_renderer.material = material2;
						}
						for (int num16 = 0; num16 < array5.Length; num16++)
						{
							PrefabS.RemoveComponent(array4[num16]);
						}
						list4.AddRange(array4);
					}
				}
				gEShapeC2.PCs = list4.ToArray();
				continue;
			}
			if (gEShapeC2.groundSettings.hasRoad)
			{
				foreach (int index3 in array3)
				{
					list4.AddRange(GenerateRoad(Main.camera, gEShapeC2, list3[index3].ToArray(), null, ref _distance).ToArray());
				}
			}
			for (int num18 = 0; num18 < gEShapeC2.GPC.tiles.Length; num18++)
			{
				if (gEShapeC2.GPC.tiles[num18].NofContours > 0)
				{
					PrefabC[] array6 = CreateFlatPrefabComponentsFromPolygon(gEShapeC2, (Vector3)(-vector5 + vector3) - Vector3.forward * eIC.TC.transform.position.z, gEShapeC2.GPC.tiles[num18], Main.camera, string.Empty, 0f).ToArray();
					CombineInstance[] array7 = new CombineInstance[array6.Length];
					for (int num19 = 0; num19 < array7.Length; num19++)
					{
						array7[num19].mesh = array6[num19].p_mesh;
						array7[num19].transform = array6[num19].p_gameObject.transform.localToWorldMatrix;
					}
					PrefabC prefabC3 = PrefabS.AddComponent(gEShapeC2.TC, Vector3.zero);
					prefabC3.p_mesh.CombineMeshes(array7);
					var o_472_5_639233471813352816 = prefabC3.p_mesh;
					Material material3 = ResourceManager.GetMaterial(gEShapeC2.groundSettings.fillMaterialResourceIdentifier);
					if (material3 == null)
					{
						material3 = ResourceManager.GetMaterial("Solid");
					}
					material3.mainTextureScale = Vector2.one * gEShapeC2.groundSettings.fillScale;
					if (array6.Length > 0)
					{
						prefabC3.p_renderer.material = material3;
					}
					for (int num20 = 0; num20 < array7.Length; num20++)
					{
						PrefabS.RemoveComponent(array6[num20]);
					}
				}
			}
			gEShapeC2.PCs = list4.ToArray();
		}
	}

	public static List<PrefabC> CreateFlatPrefabComponentsFromPolygon(GEShapeC _shape, Vector3 _offset, Polygon _polygon, Camera _camera, string _identifier, float _depth)
	{
		List<PrefabC> list = new List<PrefabC>();
		if (_polygon.NofContours > 0)
		{
			Tristrip tristrip = _polygon.ToTristrip();
			for (int i = 0; i < tristrip.NofStrips; i++)
			{
				PrefabC prefabC = PrefabS.AddComponent(_shape.TC, Vector3.zero);
				prefabC.p_gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				prefabC.p_gameObject.layer = _camera.gameObject.layer;
				Material material = ResourceManager.GetMaterial(_shape.groundSettings.fillMaterialResourceIdentifier);
				if (material == null)
				{
					material = ResourceManager.GetMaterial("Solid");
				}
				material.mainTextureScale = Vector2.one * _shape.groundSettings.fillScale;
				prefabC.p_renderer.material = material;
				prefabC.identifier = _identifier;
				VertexList vertexList = tristrip.Strip[i];
				Vector3[] array = new Vector3[vertexList.NofVertices];
				Vector2[] array2 = new Vector2[vertexList.NofVertices];
				Color[] array3 = new Color[vertexList.NofVertices];
				int[] array4 = new int[(vertexList.NofVertices - 2) * 3];
				int num = -1;
				for (int j = 0; j < vertexList.NofVertices; j++)
				{
					Vector2 vector = vertexList.Vertex[j];
					array[j] = new Vector3(vector.x, vector.y, _depth) + _offset;
					array2[j] = vector / Screen.height;
					Color color = DebugDraw.GetColor((_shape.groundSettings.color1 >> 16) & 0xFF, (_shape.groundSettings.color1 >> 8) & 0xFF, _shape.groundSettings.color1 & 0xFF);
					Color color2 = DebugDraw.GetColor((_shape.groundSettings.color2 >> 16) & 0xFF, (_shape.groundSettings.color2 >> 8) & 0xFF, _shape.groundSettings.color2 & 0xFF);
					float polyHeight = _shape.GPC.polyHeight;
					float polyMinY = _shape.GPC.polyMinY;
					float polyMaxY = _shape.GPC.polyMaxY;
					float num2 = (vector.y - polyMinY) / polyHeight;
					array3[j] = color2 * num2 + color * (1f - num2);
					if (j < vertexList.NofVertices - 2)
					{
						if (num == -1)
						{
							array4[j * 3] = j;
							array4[j * 3 + 1] = j + 2;
							array4[j * 3 + 2] = j + 1;
							num *= -1;
						}
						else
						{
							array4[j * 3] = j;
							array4[j * 3 + 1] = j + 1;
							array4[j * 3 + 2] = j + 2;
							num *= -1;
						}
					}
				}
				prefabC.p_mesh.vertices = array;
				prefabC.p_mesh.triangles = array4;
				prefabC.p_mesh.uv = array2;
				prefabC.p_mesh.colors = array3;
				prefabC.p_mesh.RecalculateBounds();
				prefabC.p_mesh.RecalculateNormals();
				list.Add(prefabC);
			}
		}
		return list;
	}

	public static List<PrefabC> GenerateRoad(Camera _camera, GEShapeC _shape, Vector2[] _strip, SKArc[] _beltArcs, ref float _distance)
	{
		List<PrefabC> list = new List<PrefabC>();
		if (_strip.Length > 1)
		{
			Vector3 offset = Vector3.forward * (_shape.groundSettings.roadDepth + 25f);
			if (_shape.groundSettings.hasBelt)
			{
				offset = Vector3.forward * (_shape.groundSettings.roadDepth + _shape.groundSettings.beltDepth + 25f);
			}
			PrefabC prefabC = PrefabS.AddComponent(_shape.TC, offset);
			prefabC.p_gameObject.layer = _camera.gameObject.layer;
			GroundSettings groundSettings = _shape.groundSettings;
			Material material = ResourceManager.GetMaterial(groundSettings.roadMaterialResourceIdentifier);
			if (material == null)
			{
				material = ResourceManager.GetMaterial("Solid");
			}
			material.mainTextureScale = new Vector2(groundSettings.roadScale, 1f);
			prefabC.p_renderer.material = material;
			List<Vector3> list2 = new List<Vector3>(_strip.Length * 2);
			List<Vector2> list3 = new List<Vector2>(_strip.Length * 2);
			List<Color> list4 = new List<Color>(_strip.Length * 2);
			List<int> list5 = new List<int>((_strip.Length * 2 - 2) * 3);
			Vector2 vector = Vector3.zero;
			for (int i = 0; i < _strip.Length; i++)
			{
				if (i > 0)
				{
					_distance += (_strip[i] - _strip[i - 1]).magnitude;
				}
				list2.AddRange(new Vector3[2]);
				list3.AddRange(new Vector2[2]);
				list4.AddRange(new Color[2]);
				list5.AddRange(new int[6]);
				list2[i * 2] = new Vector3(_strip[i].x, _strip[i].y, 0f);
				list2[i * 2 + 1] = new Vector3(_strip[i].x, _strip[i].y, 0f - groundSettings.roadDepth - 25f);
				list3[i * 2] = new Vector2(_distance / 100f, 1f);
				list3[i * 2 + 1] = new Vector2(_distance / 100f, 0.5f);
				float num = 0f;
				float num2 = 0f;
				if (_beltArcs != null)
				{
					if (i > _beltArcs.Length - 1 || i < 0)
					{
						Debug.LogError(i);
					}
					num = Mathf.Pow(_beltArcs[i].normal.y * -0.5f + 0.5f, 2f) * 0.25f;
					num2 = Mathf.Pow(_beltArcs[i].normal.y * 0.5f + 0.5f, 5f) * 0.25f;
				}
				Color value = Color.gray - new Color(0.75f, 0.75f, 0.5f) * num2 + new Color(1f, 0.85f, 0.5f) * num;
				value.a = 1f;
				list4[i * 2] = value;
				list4[i * 2 + 1] = value;
				if (i > 0)
				{
					list5[(i * 2 - 2) * 3] = i * 2 - 2;
					list5[(i * 2 - 2) * 3 + 1] = i * 2 - 2 + 2;
					list5[(i * 2 - 2) * 3 + 2] = i * 2 - 2 + 1;
					list5[(i * 2 - 1) * 3] = i * 2 - 1;
					list5[(i * 2 - 1) * 3 + 1] = i * 2 - 1 + 1;
					list5[(i * 2 - 1) * 3 + 2] = i * 2 - 1 + 2;
				}
			}
			if (_beltArcs != null)
			{
				for (int j = 0; j < _strip.Length; j++)
				{
					SKArc sKArc = _beltArcs[j];
					int num3 = 0;
					while (sKArc.next != null && num3 < 100)
					{
						sKArc = sKArc.next;
						num3++;
						if (num3 == 100)
						{
							Debug.Log("infinite loop");
						}
					}
					list2.Add(new Vector3(sKArc.nB.pos.x, sKArc.nB.pos.y, 0f - groundSettings.roadDepth - groundSettings.beltDepth - 25f));
					list3.Add(new Vector2(list3[j * 2 + 1].x, 0.01f));
					float num4 = 1f;
					list4.Add(Color.gray * num4);
					if (j <= 0)
					{
						continue;
					}
					SKArc sKArc2 = _beltArcs[j - 1];
					int num5 = 0;
					while (sKArc2.next != null && num5 < 100)
					{
						sKArc2 = sKArc2.next;
						num5++;
						if (num5 == 100)
						{
							Debug.Log("infinite loop");
						}
					}
					Vector3 vector2 = list2[j * 2 - 1];
					Vector3 vector3 = list2[j * 2 + 1];
					Vector3 vector4 = list2[_strip.Length * 2 + j - 1];
					Vector3 vector5 = list2[_strip.Length * 2 + j];
					list5.AddRange(new int[6]);
					int num6 = (_strip.Length * 2 - 2) * 3;
					list5[num6 + j * 6] = j * 2 - 1;
					list5[num6 + j * 6 + 1] = j * 2 - 1 + 2;
					list5[num6 + j * 6 + 2] = _strip.Length * 2 + j - 1;
					list5[num6 + j * 6 + 3] = _strip.Length * 2 + j - 1;
					list5[num6 + j * 6 + 4] = j * 2 - 1 + 2;
					list5[num6 + j * 6 + 5] = _strip.Length * 2 + j;
				}
			}
			prefabC.p_mesh.vertices = list2.ToArray();
			prefabC.p_mesh.triangles = list5.ToArray();
			prefabC.p_mesh.uv = list3.ToArray();
			prefabC.p_mesh.colors = list4.ToArray();
			prefabC.p_mesh.RecalculateBounds();
			prefabC.p_mesh.RecalculateNormals();
			list.Add(prefabC);
		}
		return list;
	}

	public static List<PrefabC> GenerateBlock(Camera _camera, GEBlockC _block)
	{
		List<PrefabC> list = new List<PrefabC>();
		Polygon modifiedShape = _block.modifiedShape;
		Color gray = Color.gray;
		uint num = PrefabS.ColorToUInt(gray);
		float num2 = 1f;
		float num3 = 100f;
		if (_block.CMC.colliderLayer == GEState.layer_back)
		{
			num2 = 1f / 3f;
		}
		else if (_block.CMC.colliderLayer == GEState.layer_front)
		{
			num2 = 1f / 3f;
			num3 = 50f;
		}
		GroundSettings groundSettings = _block.groundSettings;
		for (int i = 0; i < modifiedShape.NofContours; i++)
		{
			PrefabC prefabC = PrefabS.AddComponent(_block.CMC.TC, Vector3.forward * num3);
			prefabC.p_gameObject.layer = _camera.gameObject.layer;
			Material material = ResourceManager.GetMaterial(groundSettings.roadMaterialResourceIdentifier);
			if (material == null)
			{
				material = ResourceManager.GetMaterial("Solid");
			}
			material.mainTextureScale = new Vector2(groundSettings.roadScale, 1f);
			prefabC.p_renderer.material = material;
			VertexList vertexList = modifiedShape.Contour[i];
			if (vertexList.Vertex[0] != vertexList.Vertex[vertexList.NofVertices - 1])
			{
				Vector2[] array = new Vector2[vertexList.NofVertices + 1];
				vertexList.Vertex.CopyTo(array, 0);
				array[array.Length - 1] = array[0];
				vertexList = new VertexList(array);
			}
			List<Vector3> list2 = new List<Vector3>(vertexList.NofVertices * 2);
			List<Vector2> list3 = new List<Vector2>(vertexList.NofVertices * 2);
			List<Color> list4 = new List<Color>(vertexList.NofVertices * 2);
			List<int> list5 = new List<int>((vertexList.NofVertices * 2 - 2) * 3);
			float num4 = 0f;
			Vector2 zero = Vector2.zero;
			for (int j = 0; j < vertexList.NofVertices; j++)
			{
				Vector2 vector = vertexList.Vertex[j];
				if (j > 0)
				{
					num4 += (vector - vertexList.Vertex[j - 1]).magnitude;
				}
				list2.AddRange(new Vector3[2]);
				list3.AddRange(new Vector2[2]);
				list4.AddRange(new Color[2]);
				list5.AddRange(new int[6]);
				list2[j * 2] = new Vector3(vector.x, vector.y, -25f);
				if (groundSettings.hasBelt)
				{
					list2[j * 2 + 1] = new Vector3(vector.x, vector.y, (0f - groundSettings.roadDepth) * num2 + groundSettings.beltDepth - 25f);
				}
				else
				{
					list2[j * 2 + 1] = new Vector3(vector.x, vector.y, (0f - groundSettings.roadDepth) * num2 - 25f);
				}
				list3[j * 2] = new Vector2(num4 / 100f, 1f);
				list3[j * 2 + 1] = new Vector2(num4 / 100f, 0.5f);
				list4[j * 2] = gray;
				list4[j * 2 + 1] = gray;
				if (j > 0)
				{
					list5[(j * 2 - 2) * 3] = j * 2 - 2;
					list5[(j * 2 - 2) * 3 + 1] = j * 2 - 2 + 2;
					list5[(j * 2 - 2) * 3 + 2] = j * 2 - 2 + 1;
					list5[(j * 2 - 1) * 3] = j * 2 - 1;
					list5[(j * 2 - 1) * 3 + 1] = j * 2 - 1 + 1;
					list5[(j * 2 - 1) * 3 + 2] = j * 2 - 1 + 2;
				}
			}
			if (groundSettings.hasBelt && groundSettings.beltWidth > 0f)
			{
				Polygon polygon = new Polygon();
				polygon.AddContour(vertexList, false);
				List<SKArc> _arcs = new List<SKArc>();
				SKBase sKBase = GEStraightSkeleton.GenerateStraightSkeleton(polygon, ref _arcs, groundSettings.beltWidth, 1f);
				Polygon polygon2 = new Polygon();
				List<Vector2> list6 = new List<Vector2>();
				int index = 0;
				for (int k = 0; k < sKBase.wavefronts.Count; k++)
				{
					SKWavefront sKWavefront = sKBase.wavefronts[k];
					list6.Add(sKWavefront.a1.nB.pos);
					if (sKWavefront.next == sKBase.wavefronts[index])
					{
						index = k + 1;
						polygon2.AddContour(new VertexList(list6.ToArray()), false);
						list6.Clear();
					}
				}
				Material material2 = ResourceManager.GetMaterial(groundSettings.fillMaterialResourceIdentifier);
				if (material2 == null)
				{
					material2 = ResourceManager.GetMaterial("Solid");
				}
				material2.mainTextureScale = Vector2.one * groundSettings.fillScale;
				list.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(_block.CMC.TC, Vector3.forward * (0f - (groundSettings.roadDepth * num2 + 25f)) + Vector3.forward * num3, polygon2, num, num, material2, Main.camera, string.Empty));
				for (int l = 0; l < vertexList.NofVertices; l++)
				{
					SKArc sKArc = _arcs[l];
					int num5 = 0;
					while (sKArc.next != null && num5 < 100)
					{
						sKArc = sKArc.next;
						num5++;
						if (num5 == 100)
						{
							Debug.Log("infinite loop");
						}
					}
					list2.Add(new Vector3(sKArc.nB.pos.x, sKArc.nB.pos.y, (0f - groundSettings.roadDepth) * num2 - 25f));
					list3.Add(new Vector2(list3[l * 2 + 1].x, 0.01f));
					float num6 = 1f;
					list4.Add(Color.gray * num6);
					if (l <= 0)
					{
						continue;
					}
					SKArc sKArc2 = _arcs[l - 1];
					int num7 = 0;
					while (sKArc2.next != null && num7 < 100)
					{
						sKArc2 = sKArc2.next;
						num7++;
						if (num7 == 100)
						{
							Debug.Log("infinite loop");
						}
					}
					Vector3 vector2 = list2[l * 2 - 1];
					Vector3 vector3 = list2[l * 2 + 1];
					Vector3 vector4 = list2[vertexList.NofVertices * 2 + l - 1];
					Vector3 vector5 = list2[vertexList.NofVertices * 2 + l];
					list5.AddRange(new int[6]);
					int num8 = (vertexList.NofVertices * 2 - 2) * 3;
					list5[num8 + l * 6] = l * 2 - 1;
					list5[num8 + l * 6 + 1] = l * 2 - 1 + 2;
					list5[num8 + l * 6 + 2] = vertexList.NofVertices * 2 + l - 1;
					list5[num8 + l * 6 + 3] = vertexList.NofVertices * 2 + l - 1;
					list5[num8 + l * 6 + 4] = l * 2 - 1 + 2;
					list5[num8 + l * 6 + 5] = vertexList.NofVertices * 2 + l;
				}
			}
			prefabC.p_mesh.vertices = list2.ToArray();
			prefabC.p_mesh.triangles = list5.ToArray();
			prefabC.p_mesh.uv = list3.ToArray();
			prefabC.p_mesh.colors = list4.ToArray();
			prefabC.p_mesh.RecalculateBounds();
			prefabC.p_mesh.RecalculateNormals();
			list.Add(prefabC);
		}
		if (!_block.groundSettings.hasBelt)
		{
			Material material3 = ResourceManager.GetMaterial(groundSettings.fillMaterialResourceIdentifier);
			if (material3 == null)
			{
				material3 = ResourceManager.GetMaterial("Solid");
			}
			material3.mainTextureScale = Vector2.one * groundSettings.fillScale;
			list.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(_block.CMC.TC, Vector3.forward * (0f - (groundSettings.roadDepth * num2 + 25f)) + Vector3.forward * num3, modifiedShape, num, num, material3, Main.camera, string.Empty));
		}
		return list;
	}

	private static int[] OrderSplits(List<List<Vector2>> _splits, bool[] _handledSplits)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < _splits.Count; i++)
		{
			if (_handledSplits[i])
			{
				continue;
			}
			if (_splits[i].Count > 1)
			{
				if (_splits[i][0] == _splits[i][_splits[i].Count - 1])
				{
					list.Insert(0, i);
					_handledSplits[i] = true;
					continue;
				}
				Vector2 vector = _splits[i][_splits[i].Count - 1];
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (vector == _splits[list[j]][0])
					{
						list.Insert(j, i);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(i);
				}
				_handledSplits[i] = true;
			}
			else
			{
				_handledSplits[i] = true;
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			if (k <= 1)
			{
				continue;
			}
			Vector2 vector2 = _splits[list[k - 1]][_splits[list[k - 1]].Count - 1];
			if (!(vector2 != _splits[list[k]][0]) || !(vector2 != _splits[list[k - 1]][0]))
			{
				continue;
			}
			for (int l = 0; l < list.Count; l++)
			{
				if (vector2 == _splits[list[l]][0])
				{
					list.Insert(k, list[l]);
					if (k <= l)
					{
						list.RemoveAt(l + 1);
					}
					else
					{
						list.RemoveAt(l);
					}
					break;
				}
			}
		}
		return list.ToArray();
	}

	private static List<List<Vector2>> SplitShape(GEShapeC _shape, int _polyIndex)
	{
		List<List<Vector2>> list = new List<List<Vector2>>();
		int num = 0;
		bool flag = false;
		Polygon polygon = _shape.GPC.tiles[_polyIndex];
		for (int i = 0; i < polygon.NofContours; i++)
		{
			GraphicsPath graphicsPath = polygon.Contour[i].ToGraphicsPath();
			Vector2[] pathPoints = graphicsPath.PathPoints;
			list.Add(new List<Vector2>());
			int num2 = num;
			for (int j = 0; j < pathPoints.Length; j++)
			{
				Vector2 vector = pathPoints[j];
				Vector2 vector3;
				if (j == 0)
				{
					Vector2 vector2 = pathPoints[pathPoints.Length - 1];
					vector3 = pathPoints[j + 1];
				}
				else if (j == pathPoints.Length - 1)
				{
					Vector2 vector2 = pathPoints[j - 1];
					vector3 = pathPoints[0];
				}
				else
				{
					Vector2 vector2 = pathPoints[j - 1];
					vector3 = pathPoints[j + 1];
				}
				Vector2 vector4 = vector3 - vector;
				Vector2 vector5 = vector + vector4 * 0.5f;
				float f = Mathf.Atan2(vector4.y, vector4.x) * 57.29578f;
				flag = false;
				float num3 = Mathf.Abs(f);
				if ((num3 == 0f || num3 == 90f || num3 == 180f) && ((vector.x - _shape.GPC.polyMinX != 0f && vector.y - _shape.GPC.polyMinY != 0f) || (vector3.x - _shape.GPC.polyMinX != 0f && vector3.y - _shape.GPC.polyMinY != 0f)))
				{
					float num4 = (vector.x - _shape.GPC.polyMinX) / (float)_shape.GPC.tileWidth;
					float num5 = (vector.y - _shape.GPC.polyMinY) / (float)_shape.GPC.tileHeight;
					if ((((double)(num4 % 1f) < 1E-05 || (double)(num4 % 1f) > 0.99999) && num3 == 90f) || (((double)(num5 % 1f) < 1E-05 || (double)(num5 % 1f) > 0.99999) && num3 != 90f))
					{
						flag = true;
					}
				}
				list[num].Add(vector);
				if (flag)
				{
					list.Add(new List<Vector2>());
					num++;
				}
			}
			if (num > num2)
			{
				if (!flag)
				{
					list[num2].InsertRange(0, list[num]);
					if (i < polygon.NofContours - 1)
					{
						list[num].Clear();
						continue;
					}
					list.RemoveAt(num);
					num--;
				}
			}
			else
			{
				list[num2].Add(list[num2][0]);
				list.Add(new List<Vector2>());
				num++;
			}
		}
		return list;
	}
}

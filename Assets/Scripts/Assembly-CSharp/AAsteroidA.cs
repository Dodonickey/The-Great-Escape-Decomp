using System;
using UnityEngine;

public class AAsteroidA
{
	public static AAsteroidC Assemble(Vector2 _pos, int _size, int _speed)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		int num = 16;
		float num2 = _size;
		float num3 = 0f;
		float num4 = 360 / (num - 1);
		Vector2[] array = new Vector2[num];
		for (int i = 0; i < num; i++)
		{
			float num5 = num2 * UnityEngine.Random.Range(0.25f, 1.5f);
			array[i] = new Vector2(Mathf.Sin(num3 * ((float)Math.PI / 180f)) * num5, Mathf.Cos(num3 * ((float)Math.PI / 180f)) * num5);
			num3 += num4;
		}
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(array);
		polygon = GpcS.CleanPolygon(polygon, 0.1f, 1f, 30f, true);
		polygon = GpcS.SmoothPolygon(polygon, 1);
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.zero, polygon, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, false, (ColliderType)15);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(false, false, _pos, chipmunkC.index, (ColliderType)15));
		ChipmunkWrapper.SetCustomBodyGravity(chipmunkC.cpBodyPtr, Vector2.zero);
		for (int j = 0; j < polygon.NofContours; j++)
		{
			Vector2[] vertex = polygon.Contour[j].Vertex;
			Vector2 zero = Vector2.zero;
			Vector2[] array2 = vertex;
			for (int k = 0; k < array2.Length; k++)
			{
				Vector2 vector = array2[k];
				zero.x += vector.x;
				zero.y += vector.y;
			}
			zero.x /= vertex.Length;
			zero.y /= vertex.Length;
			ChipmunkWrapper.AddPolyShape(chipmunkC.cpBodyPtr, zero, _size / 2, vertex.Length, vertex, 1f, 3f, 0u, GEState.layer_back | GEState.layer_front, false);
		}
		ChipmunkWrapper.SetBodyVelocityLimits(chipmunkC.cpBodyPtr, 200f, 200f);
		AAsteroidC aAsteroidC = ASystem.AddAsteroidComponent(entity, chipmunkC);
		aAsteroidC.TC = transformC;
		aAsteroidC.size = _size;
		ChipmunkS.SetCustomComponent(aAsteroidC.CMC, aAsteroidC);
		return aAsteroidC;
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
		Vector2 vector = new Vector2(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-100f, 100f));
		BasicLevelData basicLevelData = _eic.data as BasicLevelData;
		AAsteroidC aAsteroidC = Assemble(basicLevelData.position.ToVector2(), 20, 20);
		ChipmunkWrapper.ApplyImpulse(aAsteroidC.CMC.cpBodyPtr, new Vector2(vector.x, vector.y), Vector2.zero, true);
		_eic.gameComponents.Add(aAsteroidC.TC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(aAsteroidC.TC, _eic.TC, Vector3.zero);
		}
	}
}

using UnityEngine;

public class AEmitterA
{
	public static TransformC Assemble(EIC _eic, BasicLevelData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		int num = 5;
		int num2 = 8;
		int size = 40;
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, _data.position.ToVector3());
		AEmitterC aEmitterC = ASystem.AddEmitterComponent(entity, _data);
		aEmitterC.TC = transformC;
		aEmitterC.asteroids = new AAsteroidC[num * 10];
		aEmitterC.numAsteroids = num;
		aEmitterC.asteroidSpeed = num2;
		if (!GEState.editorMode)
		{
			for (int i = 0; i < num; i++)
			{
				Vector2 vector = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
				Vector2 pos = new Vector2(Random.Range(-300f, 300f), Random.Range(-225f, 225f));
				while (pos.x > -55f && pos.x < 55f)
				{
					pos.x = Random.Range(-300f, 300f);
				}
				while (pos.y > -55f && pos.y < 55f)
				{
					pos.y = Random.Range(-225f, 225f);
				}
				AAsteroidC aAsteroidC = AAsteroidA.Assemble(pos, size, num2);
				ChipmunkWrapper.ApplyImpulse(aAsteroidC.CMC.cpBodyPtr, new Vector2(vector.x, vector.y), Vector2.zero, true);
			}
		}
		return transformC;
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
		BasicLevelData data = _eic.data as BasicLevelData;
		TransformC transformC = Assemble(_eic, data);
		_eic.gameComponents.Add(transformC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(transformC, _eic.TC, Vector3.zero);
		}
	}
}

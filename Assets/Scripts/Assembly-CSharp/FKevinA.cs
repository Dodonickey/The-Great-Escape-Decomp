using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FKevinA
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024map9;

	public static GECharacterC Assemble(EIC _eic)
	{
		return Assemble(_eic, (uint)(2000 + _eic.index), 0f - _eic.data.position.z);
	}

	public static GECharacterC Assemble(EIC _eic, uint _colliderGroup, float _sortOffset)
	{
		ColliderType colliderType = (ColliderType)3;
		CreatureType characterType = CreatureType.Biped;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Character",
			LevelManager.m_currentLevel.name
		};
		GESpritePrefabC gESpritePrefabC = SpritePrefabA.Assemble(_eic.data.position.ToVector3(), "Kevin", tags, colliderType, _colliderGroup, GEState.layer_middle, _sortOffset);
		SpritePrefabNode spritePrefabNode = gESpritePrefabC.nodeTable["LocatorHats"] as SpritePrefabNode;
		SpritePrefabNode spritePrefabNode2 = gESpritePrefabC.nodeTable["Head"] as SpritePrefabNode;
		GESpritePrefabC gESpritePrefabC2 = SpritePrefabA.Assemble(gESpritePrefabC.entityIndex, spritePrefabNode.TC.transform.position, "Indian", tags, ColliderType.Any, _colliderGroup, GEState.layer_middle, _sortOffset - spritePrefabNode.globalPosition.z);
		TransformS.ParentComponent(gESpritePrefabC2.rootNode.TC, spritePrefabNode.TC);
		TransformS.SetTransform(gESpritePrefabC2.rootNode.TC, gESpritePrefabC2.rootNode.localCenter, Vector3.zero);
		SpritePrefabNode spritePrefabNode3 = gESpritePrefabC.nodeTable["pelvis"] as SpritePrefabNode;
		List<SpritePrefabNode> list = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list2 = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list3 = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list4 = new List<SpritePrefabNode>();
		for (int i = 0; i < gESpritePrefabC.nodes.Length; i++)
		{
			SpritePrefabNode spritePrefabNode4 = gESpritePrefabC.nodes[i];
			if (spritePrefabNode4.isEye == 1)
			{
				list.Add(spritePrefabNode4);
			}
			else if (spritePrefabNode4.isArm == 1)
			{
				list2.Add(spritePrefabNode4);
			}
			else if (spritePrefabNode4.isLeg == 1)
			{
				list3.Add(spritePrefabNode4);
			}
			else if (spritePrefabNode4.isHead == 1)
			{
				list4.Add(spritePrefabNode4);
			}
		}
		GECharacterC gECharacterC = GES.AddBipedCharacterComponent(gESpritePrefabC, list.ToArray(), list3.ToArray(), list2.ToArray(), list4.ToArray(), characterType);
		gECharacterC.hatSPC = gESpritePrefabC2;
		gECharacterC.creatureType = CreatureType.Biped;
		gECharacterC.health = 100f;
		gECharacterC.maxHealth = 100f;
		gESpritePrefabC.customComponent = gECharacterC;
		for (int j = 0; j < gESpritePrefabC.nodes.Length; j++)
		{
			if (gESpritePrefabC.nodes[j].hasPhysics == 1)
			{
				gESpritePrefabC.nodes[j].CMC.customComponent = gECharacterC;
			}
		}
		GECharacterLogic.CreateRunner(gECharacterC, Vector2.up * 5f, _sortOffset);
		gECharacterC.SPC.animations = SpritePrefabA.m_animations["Kevin"] as Hashtable;
		return gECharacterC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		BasicLevelData basicLevelData = new BasicLevelData();
		basicLevelData.position = new Vertex3(_pos);
		basicLevelData.position.z = 37.5f;
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
		GECharacterC item = Assemble(_eic);
		_eic.gameComponents.Add(item);
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		BasicLevelData basicLevelData = _eic.data as BasicLevelData;
	}

	public static void HandlePropertyChange(EventC _c)
	{
		BasicLevelData basicLevelData = EditorState.m_selection[0].data as BasicLevelData;
		string identifier = _c.identifier;
		if (identifier != null)
		{
			if (_003C_003Ef__switch_0024map9 == null)
			{
				_003C_003Ef__switch_0024map9 = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024map9.TryGetValue(identifier, out value))
			{
			}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

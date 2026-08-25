using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FSmallKartA
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024mapE;

	public static GEVehicleC Assemble(EIC _eic)
	{
		return Assemble(_eic, (uint)(2000 + _eic.index), 0f - _eic.data.position.z);
	}

	public static GEVehicleC Assemble(EIC _eic, uint _colliderGroup, float _sortOffset)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Vehicle",
			LevelManager.m_currentLevel.name
		};
		GESpritePrefabC gESpritePrefabC = SpritePrefabA.Assemble(_eic.data.position.ToVector3(), "SmallKart", tags, (ColliderType)12, _colliderGroup, GEState.layer_middle, _sortOffset);
		SpritePrefabNode spritePrefabNode = gESpritePrefabC.nodeTable["chassis"] as SpritePrefabNode;
		List<SpritePrefabNode> list = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list2 = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list3 = new List<SpritePrefabNode>();
		List<GECreatureC> list4 = new List<GECreatureC>();
		for (int i = 0; i < gESpritePrefabC.nodes.Length; i++)
		{
			SpritePrefabNode spritePrefabNode2 = gESpritePrefabC.nodes[i];
			if (spritePrefabNode2.isTire == 1)
			{
				list.Add(spritePrefabNode2);
			}
			else if (spritePrefabNode2.isCrawler == 1)
			{
				list2.Add(spritePrefabNode2);
			}
			if (spritePrefabNode2.name.Length > 11 && spritePrefabNode2.name.Substring(0, 11) == "LocatorSeat")
			{
				list3.Add(spritePrefabNode2);
				list4.Add(null);
			}
		}
		GEVehicleC gEVehicleC = GES.AddVehicleComponent(gESpritePrefabC, list.ToArray(), list2.ToArray(), GameState.m_playerStates[0]);
		gEVehicleC.balanceSpring = ChipmunkWrapper.AddDampedRotarySpring(ChipmunkWrapper.GetSpaceStaticBody(), spritePrefabNode.CMC.cpBodyPtr, gESpritePrefabC.rootNode.globalRotation.z * ((float)Math.PI / 180f), 1000000f, 5000f);
		gEVehicleC.vehicleType = (VehicleType)2;
		gEVehicleC.creatureType = CreatureType.Vehicle;
		gEVehicleC.seats = list3;
		gEVehicleC.seatsTaken = list4;
		gEVehicleC.health = 1f;
		gEVehicleC.maxHealth = 1f;
		gEVehicleC.defensiveAttributes.effects[5] = 150;
		gEVehicleC.defensiveAttributes.effectActive[5] = true;
		gEVehicleC.driveSoundLoop = SoundS.AddComponent(EntityManager.m_entities.m_array[gESpritePrefabC.entityIndex], "SoundCartRumble", spritePrefabNode.TC.transform.gameObject);
		GEVehicleLogic.SetCartLayer(gEVehicleC, GEState.layer_all);
		for (int j = 0; j < gESpritePrefabC.nodes.Length; j++)
		{
			if (gESpritePrefabC.nodes[j].hasPhysics == 1)
			{
				gESpritePrefabC.nodes[j].CMC.customComponent = gEVehicleC;
			}
		}
		return gEVehicleC;
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
		GEVehicleC item = Assemble(_eic);
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
			if (_003C_003Ef__switch_0024mapE == null)
			{
				_003C_003Ef__switch_0024mapE = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024mapE.TryGetValue(identifier, out value))
			{
			}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

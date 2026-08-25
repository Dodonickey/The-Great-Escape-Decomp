using System;
using System.Collections.Generic;
using UnityEngine;

public static class GESmallVehicleA
{
	public static GEVehicleC Assemble(Vector3 _pos, uint _colliderGroup, PlayerState _playerState, uint _colliderLayer)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Vehicle",
			LevelManager.m_currentLevel.name
		};
		GESpritePrefabC gESpritePrefabC = SpritePrefabA.Assemble(_pos, "Small", tags, (ColliderType)12, _colliderGroup, GEState.layer_middle, 0f);
		SpritePrefabNode spritePrefabNode = gESpritePrefabC.nodeTable["chassis"] as SpritePrefabNode;
		List<SpritePrefabNode> list = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list2 = new List<SpritePrefabNode>();
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
		}
		GEVehicleC gEVehicleC = GES.AddVehicleComponent(gESpritePrefabC, list.ToArray(), list2.ToArray(), _playerState);
		gEVehicleC.balanceSpring = ChipmunkWrapper.AddDampedRotarySpring(ChipmunkWrapper.GetSpaceStaticBody(), spritePrefabNode.CMC.cpBodyPtr, gESpritePrefabC.rootNode.globalRotation.z * ((float)Math.PI / 180f), 1000000f, 5000f);
		gEVehicleC.vehicleType = (VehicleType)2;
		gEVehicleC.creatureType = CreatureType.Vehicle;
		gEVehicleC.health = 100f;
		gEVehicleC.maxHealth = 100f;
		GEVehicleLogic.SetCartLayer(gEVehicleC, _colliderLayer);
		for (int j = 0; j < gESpritePrefabC.nodes.Length; j++)
		{
			if (gESpritePrefabC.nodes[j].hasPhysics == 1)
			{
				gESpritePrefabC.nodes[j].CMC.customComponent = gEVehicleC;
			}
		}
		return gEVehicleC;
	}
}

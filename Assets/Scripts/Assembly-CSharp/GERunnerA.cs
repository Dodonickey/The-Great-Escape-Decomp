using System.Collections.Generic;
using UnityEngine;

public static class GERunnerA
{
	public static GEVehicleC Assemble(Vector3 _pos, uint _colliderGroup, PlayerState _playerState, uint _colliderLayer)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Vehicle",
			LevelManager.m_currentLevel.name
		};
		GESpritePrefabC gESpritePrefabC = SpritePrefabA.Assemble(_pos, "Runner", tags, (ColliderType)12, _colliderGroup, GEState.layer_middle, 0f);
		List<SpritePrefabNode> list = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list2 = new List<SpritePrefabNode>();
		for (int i = 0; i < gESpritePrefabC.nodes.Length; i++)
		{
			SpritePrefabNode spritePrefabNode = gESpritePrefabC.nodes[i];
			if (spritePrefabNode.isTire == 1)
			{
				list.Add(spritePrefabNode);
			}
			else if (spritePrefabNode.isCrawler == 1)
			{
				list2.Add(spritePrefabNode);
			}
		}
		GEVehicleC gEVehicleC = GES.AddVehicleComponent(gESpritePrefabC, list.ToArray(), list2.ToArray(), _playerState);
		gEVehicleC.vehicleType = VehicleType.Runner;
		gEVehicleC.balanceSpring = ChipmunkWrapper.AddDampedRotarySpring(ChipmunkWrapper.GetSpaceStaticBody(), gESpritePrefabC.rootNode.CMC.cpBodyPtr, 0f, gESpritePrefabC.rootNode.rotarySpringStrength, gESpritePrefabC.rootNode.rotarySpringDamp);
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

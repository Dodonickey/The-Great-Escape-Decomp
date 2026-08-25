using System.Collections.Generic;
using UnityEngine;

public static class GECharacterA
{
	public static GECharacterC Assemble(Vector3 _pos, uint _colliderGroup, string _spritePrefabKey, string _hatKey, float _sortOffset, uint _colliderLayer)
	{
		ColliderType colliderType = (ColliderType)3;
		CreatureType characterType = CreatureType.Biped;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Character",
			LevelManager.m_currentLevel.name
		};
		GESpritePrefabC gESpritePrefabC = SpritePrefabA.Assemble(_pos, _spritePrefabKey, tags, colliderType, _colliderGroup, _colliderLayer, _sortOffset);
		SpritePrefabNode spritePrefabNode = gESpritePrefabC.nodeTable["LocatorHats"] as SpritePrefabNode;
		GESpritePrefabC gESpritePrefabC2 = SpritePrefabA.Assemble(gESpritePrefabC.entityIndex, _pos, _hatKey, tags, colliderType, _colliderGroup, _colliderLayer, _sortOffset - spritePrefabNode.globalPosition.z);
		TransformS.ParentComponent(gESpritePrefabC2.rootNode.TC, spritePrefabNode.TC, Vector2.zero);
		TransformS.SetTransform(gESpritePrefabC2.rootNode.TC, gESpritePrefabC2.rootNode.localCenter, Vector3.zero);
		SpritePrefabNode spritePrefabNode2 = gESpritePrefabC.nodeTable["pelvis"] as SpritePrefabNode;
		List<SpritePrefabNode> list = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list2 = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list3 = new List<SpritePrefabNode>();
		List<SpritePrefabNode> list4 = new List<SpritePrefabNode>();
		for (int i = 0; i < gESpritePrefabC.nodes.Length; i++)
		{
			SpritePrefabNode spritePrefabNode3 = gESpritePrefabC.nodes[i];
			if (spritePrefabNode3.isEye == 1)
			{
				list.Add(spritePrefabNode3);
			}
			else if (spritePrefabNode3.isArm == 1)
			{
				list2.Add(spritePrefabNode3);
			}
			else if (spritePrefabNode3.isLeg == 1)
			{
				list3.Add(spritePrefabNode3);
			}
			else if (spritePrefabNode3.isHead == 1)
			{
				list4.Add(spritePrefabNode3);
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
		return gECharacterC;
	}
}

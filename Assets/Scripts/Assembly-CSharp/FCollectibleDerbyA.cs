using System;
using UnityEngine;

public static class FCollectibleDerbyA
{
	public static ChipmunkC Assemble(GameObject _go, Vector3 _pos, Vector3 _rot, Vector2 _lvel, float _avel, uint _group, uint _layer)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, false, (ColliderType)8, _group, _layer, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, _pos, chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddBoxShape(chipmunkC.cpBodyPtr, Vector2.zero, 0.5f, 10f, 10f, 0.15f, 0.8f, chipmunkC.colliderGroup, chipmunkC.colliderLayer, false);
		ChipmunkWrapper.SetVelocity(chipmunkC.cpBodyPtr, _lvel);
		ChipmunkWrapper.SetAngularVelocity(chipmunkC.cpBodyPtr, _avel);
		ChipmunkWrapper.SetAngle(chipmunkC.cpBodyPtr, _rot.z * ((float)Math.PI / 180f));
		TransformS.SetTransform(transformC, _pos, _rot, chipmunkC.cpBodyPtr);
		PrefabC customComponent = PrefabS.AddComponent(transformC, Vector3.zero, _go);
		chipmunkC.customComponent = customComponent;
		TweenC c = TweenS.AddTransformTween(transformC, TweenedProperty.Scale, TweenStyle.CubicIn, Vector3.zero, 0.5f, 1f);
		TweenS.SetRemoveEntityAtFinish(c, true);
		return chipmunkC;
	}
}

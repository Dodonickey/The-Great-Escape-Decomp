using System;
using UnityEngine;

public class ABulletA
{
	public static TransformC Assemble(AShipC _ship)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		ABulletC aBulletC = ASystem.AddBulletComponent(entity);
		aBulletC.TC = transformC;
		float f = Mathf.Atan2(_ship.CMC.ucpBodyStruct.rot.x, _ship.CMC.ucpBodyStruct.rot.y) - (float)Math.PI / 2f;
		Vector2 vector = new Vector2(Mathf.Sin(f) * 20f, Mathf.Cos(f) * 20f);
		Vector2[] circle = DebugDraw.GetCircle(5f, 8, Vector2.zero);
		Frame frame = new Frame(0f, 0f, 32f, 32f);
		aBulletC.numFrames = 3;
		aBulletC.curFrame = 0;
		aBulletC.timer = _ship.data.bulletLifetime;
		aBulletC.CMC = ChipmunkS.AddInactiveComponent(aBulletC.TC, false, (ColliderType)16);
		ChipmunkS.ActivateChipmunkComponent(aBulletC.CMC, ChipmunkWrapper.AddCircleBody(false, false, new Vector3(_ship.position.x + vector.x, _ship.position.y + vector.y, _ship.position.z), aBulletC.CMC.index, Vector2.zero, 10f, 16f, 1f, 2f, 9u, GEState.layer_back, false, (ColliderType)16));
		aBulletC.sprite = SpriteS.AddComponent(transformC, frame, AState.tss);
		ChipmunkWrapper.SetCustomBodyGravity(aBulletC.CMC.cpBodyPtr, Vector2.zero);
		ChipmunkS.SetCustomComponent(aBulletC.CMC, aBulletC);
		ChipmunkWrapper.ApplyImpulse(aBulletC.CMC.cpBodyPtr, new Vector2(vector.x * 300f, vector.y * 300f), Vector2.zero, true);
		return transformC;
	}
}

using UnityEngine;

public static class FStunStarA
{
	public static int m_starCount;

	public static void Assemble(TransformC _tc, float _scale)
	{
		m_starCount++;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Effect",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		Vector3 position = _tc.transform.position;
		float num = Random.Range(-20, 20);
		int num2 = Random.Range(0, 4);
		Frame frame = new Frame(32f, 48f, 16f, 16f);
		switch (num2)
		{
		case 2:
			frame = new Frame(48f, 48f, 16f, 16f);
			break;
		case 3:
			frame = new Frame(32f, 64f, 16f, 16f);
			break;
		case 4:
			frame = new Frame(48f, 64f, 16f, 16f);
			break;
		}
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetTransform(transformC, position, Vector3.forward * num);
		TweenS.AddTransformTween(transformC, TweenedProperty.Position, TweenStyle.Linear, position + Vector3.up * 100f, 1f, 0f);
		TweenS.AddTransformTween(transformC, TweenedProperty.Rotation, TweenStyle.Linear, Vector3.forward * num, Vector3.forward * 360f, 1f, 0f);
		TweenC c = TweenS.AddTransformTween(transformC, TweenedProperty.Alpha, TweenStyle.ExpoOut, Vector3.zero, Vector3.one, 0.5f, 0f);
		TweenS.SetAdditionalTweenProperties(c, 0, true, TweenStyle.ExpoIn);
		SpriteC spriteC = SpriteS.AddComponent(transformC, frame, FarmState.effectSheet);
		SpriteS.SetDimensionScale(spriteC, _scale * Random.Range(0.75f, 1.25f));
		SpriteS.SetOffset(spriteC, Vector3.right * Random.Range(-10, 10), 0f);
		if (Random.value < 0.5f)
		{
			SpriteS.SetColor(spriteC, DebugDraw.GetColor(152f, 26f, 0f, 0f) * 0.5f);
		}
		else
		{
			SpriteS.SetColor(spriteC, DebugDraw.GetColor(255f, 144f, 0f, 0f) * 0.5f);
		}
		EventC eventC = EventS.AddComponent(entity.index, "ImpactStarDestroy", EndHandler, 1f, true, false, false, false);
		EventC eventC2 = EventS.AddComponent(entity.index, "ImpactStar", SpawnEndHandler, Random.Range(0.25f, 0.95f), true, false, false, false);
		eventC2.properties.Add("TC", _tc);
		eventC2.properties.Add("scale", _scale);
	}

	public static void EndHandler(EventC _c)
	{
		EntityManager.RemoveEntity(_c.entityIndex);
		m_starCount--;
	}

	public static void SpawnEndHandler(EventC _c)
	{
		TransformC tc = _c.properties["TC"] as TransformC;
		float scale = (float)_c.properties["scale"];
		if (m_starCount < 6)
		{
			Assemble(tc, scale);
		}
	}
}

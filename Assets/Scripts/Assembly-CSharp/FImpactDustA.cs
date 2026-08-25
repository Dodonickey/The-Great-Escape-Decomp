using UnityEngine;

public static class FImpactDustA
{
	public static void Assemble(Vector3 _pos, float angle, int _particleCount, float _scale)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Effect",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetTransform(transformC, _pos, Vector3.forward * angle);
		float num = Random.Range(-180, 180);
		for (int i = 0; i < _particleCount; i++)
		{
			float num2 = (float)Random.Range(-20, 20) * _scale;
			TransformC transformC2 = TransformS.AddComponent(entity);
			TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
			int num3 = Random.Range(0, 4);
			Frame frame = new Frame(0f, 16f, 32f, 32f);
			if (num3 == 1)
			{
				frame = new Frame(32f, 16f, 16f, 16f);
			}
			if (num3 == 2)
			{
				frame = new Frame(32f, 32f, 16f, 16f);
			}
			if (num3 == 3)
			{
				frame = new Frame(48f, 16f, 16f, 16f);
			}
			if (num3 == 4)
			{
				frame = new Frame(48f, 32f, 16f, 16f);
			}
			SpriteC c = SpriteS.AddComponent(transformC2, frame, FarmState.effectSheet);
			SpriteS.SetDimensionScale(c, Random.Range(0.75f, 1.25f) * _scale);
			TweenC tweenC = TweenS.AddTransformTween(transformC2, TweenedProperty.Position, TweenStyle.QuadOut, Vector3.right * num2, 1.1f, 0f);
			TweenC tweenC2 = TweenS.AddTransformTween(transformC2, TweenedProperty.Scale, TweenStyle.QuadOut, Vector3.one * 2f, 1f, 0f);
			TweenC tweenC3 = TweenS.AddTransformTween(transformC2, TweenedProperty.Rotation, TweenStyle.QuadOut, Vector3.forward * (angle + num), Vector3.forward * (angle + num + num2 * 5f), 1f, 0f);
			TweenC tweenC4 = TweenS.AddTransformTween(transformC2, TweenedProperty.Alpha, TweenStyle.QuadIn, Vector3.one * 0.5f, Vector3.zero, 1f, 0f);
			if (i == 0)
			{
				tweenC.removeEntityAtFinish = true;
			}
		}
	}

	public static void TweenEventDelegate(TweenC _c)
	{
		Debug.Log("lol");
		EntityManager.RemoveEntity(_c.entityIndex);
	}
}

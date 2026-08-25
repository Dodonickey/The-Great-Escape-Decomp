using UnityEngine;

public static class FImpactStarA
{
	public static void Assemble(Vector3 _pos, Vector3 _dir, int _level, int _particleCount, float _scale)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Effect",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		float num = Random.Range(-20, 20);
		TransformS.SetTransform(transformC, _pos, Vector3.forward * num);
		Frame frame = new Frame(0f, 48f, 32f, 32f);
		switch (_level)
		{
		case 1:
			frame = new Frame(32f, 48f, 16f, 16f);
			break;
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
		TransformC transformC2 = TransformS.AddComponent(entity);
		TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
		if (_level == 0)
		{
			TweenS.AddTransformTween(transformC2, TweenedProperty.Position, TweenStyle.Linear, _dir * 10f, 0.25f, 0f);
		}
		else
		{
			TweenS.AddTransformTween(transformC2, TweenedProperty.Position, TweenStyle.Linear, _dir * 25f, 0.25f, 0f);
		}
		TweenS.AddTransformTween(transformC2, TweenedProperty.Rotation, TweenStyle.Linear, Vector3.forward * num, Vector3.forward * (num + (float)Random.Range(-90, 90)), 0.25f, 0f);
		SpriteC c = SpriteS.AddComponent(transformC2, frame, FarmState.effectSheet);
		SpriteS.SetDimensionScale(c, _scale);
		EventC eventC = null;
		eventC = ((_level != 0) ? EventS.AddComponent(entity.index, "ImpactStar", EndHandler, 0.25f, true, false, false, false) : EventS.AddComponent(entity.index, "ImpactStar", EndHandler, 0.1f, true, false, false, false));
		eventC.properties.Add("level", _level);
		eventC.properties.Add("TC", transformC2);
		eventC.properties.Add("scale", _scale);
		eventC.properties.Add("dir", _dir);
		if (_level <= 3)
		{
			for (int i = 0; i < 4 - _level; i++)
			{
				transformC2 = TransformS.AddComponent(entity);
				TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
				float x = Random.Range(-1f, 1f);
				float y = Random.Range(-1f, 1f);
				TweenC tweenC = TweenS.AddTransformTween(transformC2, TweenedProperty.Position, TweenStyle.Linear, _dir * 0.5f + new Vector3(x, y, 0f) * 25f, 0.25f, 0f);
				num = Random.Range(-180, 180);
				TweenS.AddTransformTween(transformC2, TweenedProperty.Rotation, TweenStyle.Linear, Vector3.forward * num, Vector3.forward * (num + (float)Random.Range(-360, 360)), 0.25f, 0f);
				frame = new Frame(48f, 64f, 16f, 16f);
				c = SpriteS.AddComponent(transformC2, frame, FarmState.effectSheet);
				SpriteS.SetDimensionScale(c, _scale);
			}
		}
	}

	public static void EndHandler(EventC _c)
	{
		TransformC transformC = _c.properties["TC"] as TransformC;
		int num = (int)_c.properties["level"];
		float scale = (float)_c.properties["scale"];
		Vector3 dir = (Vector3)_c.properties["dir"];
		if (num < 3)
		{
			Assemble(transformC.transform.position, dir, num + 1, 1, scale);
		}
		EntityManager.RemoveEntity(_c.entityIndex);
	}
}

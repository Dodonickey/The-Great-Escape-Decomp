using UnityEngine;

public class TemplateState : BasicState
{
	public static TemplateScene p_parent;

	private TransformC tc;

	public override void Enter(IStatedObject _parent)
	{
		p_parent = _parent as TemplateScene;
		Debug.Log("Template State Entered");
		Camera.mainCamera.transform.position = Vector3.forward * -500f;
		Entity entity = EntityManager.AddEntity("test entity");
		tc = TransformS.AddComponent(entity);
		Vector2[] rect = DebugDraw.GetRect(100f, 100f, Vector2.zero);
		Vector2[] rect2 = DebugDraw.GetRect(50f, 100f, Vector2.up * 50f);
		Polygon polygon = new Polygon();
		polygon.AddContour(new VertexList(rect), false);
		Polygon polygon2 = new Polygon();
		polygon2.AddContour(new VertexList(rect2), false);
		polygon = polygon.Clip(GpcOperation.Difference, polygon2);
		polygon = GpcS.CleanPolygon(polygon, 5f, 5f, 50f, false);
		polygon = GpcS.SmoothPolygon(polygon, 1);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(tc, Vector3.zero, polygon, Color.white, null, Camera.mainCamera);
		DebugDraw.CreateBox(Camera.mainCamera, tc, Vector2.zero, 100f, 100f, false);
		TweenC c = TweenS.AddTransformTween(tc, TweenedProperty.Rotation, TweenStyle.Linear, Vector3.one * 360f, 1f, 0f);
		TweenS.SetAdditionalTweenProperties(c, -1, false, TweenStyle.Linear);
	}

	public override void Execute()
	{
	}

	public override void Exit()
	{
		EntityManager.RemoveEntity(tc.entityIndex);
		tc = null;
	}
}

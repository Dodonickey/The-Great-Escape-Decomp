using UnityEngine;

public static class GETestAreaA
{
	public static UIC Assemble(UIC _parent)
	{
		string[] tags = new string[1] { "TestArea" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, 0, "TestArea", null, tags);
		UIS.AddToCanvas(uIC, _parent, Vector3.zero);
		UIS.SetCanvasRelativeSize(uIC, 0.55f, 1f, 0f, 0f);
		UIS.SetRelativePosition(uIC, new Vector2(0f, 1f), 0);
		UIS.SetCanvasRelativeMarginAndSpacing(uIC, 0f, 0.025f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		UIC uIC2 = ButtonA.Assemble(Main.uiCamera, 0, "Edit", HandleTouches, tags);
		UIS.AddToCanvasGrid(uIC2, uIC, false);
		UIS.SetRelativeSize(uIC2, 0.175f, 0.065f);
		UIS.PlaceCanvasContent(uIC);
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(uIC2.width, uIC2.height, 30f, 8, Vector2.zero, false);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
		uIC2.outlinePCs.AddRange(PrefabS.CreatePathPrefabComponentFromPolygon(uIC2.TC, Vector3.forward * 0f, polygon, 6f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true));
		uIC2.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC2.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		TextS.SetStyle("subheader");
		TextC textC = TextS.AddSingleLineComponent(uIC2.contentTC, uIC2.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		TransformS.Move(textC.contentTC, Vector3.right * (uIC2.width * -0.5f + 45f));
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC2.canvasCamera, true);
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		switch (touchEvent)
		{
		case TouchEvent.Began:
		{
			Vector2[] roundedRect2 = DebugDraw.GetRoundedRect(uIC.width, uIC.height, 30f, 8, Vector2.zero, false);
			Polygon polygon2 = DebugDraw.Vector2ArrayToPolygon(roundedRect2);
			while (uIC.backgroundPCs.Count > 0)
			{
				int index2 = uIC.backgroundPCs.Count - 1;
				PrefabS.RemoveComponent(uIC.backgroundPCs[index2]);
				uIC.backgroundPCs.RemoveAt(index2);
			}
			uIC.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.TC, Vector3.forward * 10f, polygon2, DebugDraw.GetColor(255f, 255f, 255f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
			break;
		}
		case TouchEvent.Release:
		case TouchEvent.ReleaseOutside:
		case TouchEvent.RollOut:
			if (_c.touchStartedInside[_i])
			{
				Vector2[] roundedRect = DebugDraw.GetRoundedRect(uIC.width, uIC.height, 30f, 8, Vector2.zero, false);
				Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
				while (uIC.backgroundPCs.Count > 0)
				{
					int index = uIC.backgroundPCs.Count - 1;
					PrefabS.RemoveComponent(uIC.backgroundPCs[index]);
					uIC.backgroundPCs.RemoveAt(index);
				}
				uIC.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
			}
			break;
		}
		if (touchEvent == TouchEvent.Release && _c.touchStartedInside[_i])
		{
			if (_c.identifier == "Edit")
			{
				GEState.editorCameraStartPosition = Main.camera.transform.position;
				TestState.p_parent.StateMachine.ChangeState(new EditorState());
			}
			else if (!(_c.identifier == "Reset"))
			{
			}
		}
	}
}

using UnityEngine;

public static class GEDrawButtonsAreaA
{
	private static int ADD;

	private static int SUB = 1;

	private static int SPECIAL = 2;

	public static UIC Assemble(UIC _parent, bool _draw)
	{
		string[] tags = new string[1] { "DrawArea" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, 0, "DrawArea", null, tags);
		UIS.AddToCanvas(uIC, _parent, Vector3.zero);
		UIS.SetCanvasAbsoluteSize(uIC, (float)Screen.width * 0.5f, Screen.height, 0f, 0f);
		UIS.SetRelativePosition(uIC, new Vector2(0f, 0f), 0);
		UIS.SetCanvasRelativeMarginAndSpacing(uIC, 0.05f, 0.05f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Bottom);
		if (_draw)
		{
			UIC uIC2 = ButtonA.Assemble(Main.uiCamera, ADD, "Add", HandleTouches, tags);
			UIC uIC3 = ButtonA.Assemble(Main.uiCamera, SUB, "Sub", HandleTouches, tags);
			UIS.AddToCanvasGrid(uIC2, uIC, false);
			UIS.AddToCanvasGrid(uIC3, uIC, true);
			UIS.SetRelativeSize(uIC2, 0.2f);
			UIS.SetRelativeSize(uIC3, 0.2f);
			Vector2[] circle = DebugDraw.GetCircle(uIC.height * 0.1f, 36, Vector2.zero);
			PrefabS.CreatePathPrefabComponentFromVectorArray(uIC2.TC, Vector3.forward * -10f, circle, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
			PrefabS.CreatePathPrefabComponentFromVectorArray(uIC3.TC, Vector3.forward * -10f, circle, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
			TextS.SetStyle("subheader");
			uIC2.textC = TextS.AddSingleLineComponent(uIC2.TC, uIC2.label, 1f, Align.Center, Align.Middle);
			uIC3.textC = TextS.AddSingleLineComponent(uIC3.TC, uIC3.label, 1f, Align.Center, Align.Middle);
		}
		else
		{
			UIC uIC4 = ButtonA.Assemble(Main.uiCamera, SPECIAL, "Ctrl", HandleTouches, tags);
			UIS.AddToCanvasGrid(uIC4, uIC, true);
			UIS.SetRelativeSize(uIC4, 0.2f);
			TextS.SetStyle("subheader");
			uIC4.textC = TextS.AddSingleLineComponent(uIC4.TC, uIC4.label, 1f, Align.Center, Align.Middle);
			Vector2[] circle2 = DebugDraw.GetCircle(uIC.height * 0.1f, 36, Vector2.zero);
			PrefabS.CreatePathPrefabComponentFromVectorArray(uIC4.TC, Vector3.forward * -10f, circle2, 6f, new Color(1f, 1f, 1f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true);
		}
		UIS.PlaceCanvasContent(uIC);
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed)
		{
			return;
		}
		UIC uIC = _c.customComponent as UIC;
		switch (_c.touchEvent[_i])
		{
		case TouchEvent.Began:
		case TouchEvent.RollIn:
			if (_c.touchStartedInside[_i])
			{
				if (uIC.identifier == ADD)
				{
					GEState.m_addDown = true;
				}
				else if (uIC.identifier == SUB)
				{
					GEState.m_subDown = true;
				}
				if (uIC.identifier == SPECIAL)
				{
					GEState.m_specialDown = true;
				}
			}
			break;
		case TouchEvent.Release:
		case TouchEvent.ReleaseOutside:
		case TouchEvent.RollOut:
			if (_c.touchStartedInside[_i])
			{
				if (uIC.identifier == ADD)
				{
					GEState.m_addDown = false;
				}
				else if (uIC.identifier == SUB)
				{
					GEState.m_subDown = false;
				}
				if (uIC.identifier == SPECIAL)
				{
					GEState.m_specialDown = false;
				}
			}
			break;
		}
	}
}

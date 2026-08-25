using UnityEngine;

public static class GEPropertySliderA
{
	public static UIC Assemble(UIC _parent, string _label)
	{
		string[] tags = new string[1] { "Slider" };
		UIC uIC = CanvasA.Assemble(_parent.canvasCamera, -1, string.Empty, null, tags);
		UIS.AddToCanvasGrid(uIC, _parent, true);
		UIS.SetCanvasAbsoluteSize(uIC, _parent.width - _parent.contentMargin * 2f, 40f, 0f, 0f);
		UIS.SetCanvasRelativeMarginAndSpacing(uIC, 0f, 0f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		TextS.SetStyle("body");
		TextC textC = TextS.AddSingleLineComponent(uIC.contentTC, _label, 1f, Align.Center, Align.Bottom);
		TransformS.Move(textC.contentTC, Vector3.up * uIC.height * 0.5f);
		Vector2[] line = DebugDraw.GetLine(Vector2.right * uIC.width * -0.45f, Vector2.right * uIC.width * 0.45f, 0);
		PrefabS.CreateLinePrefabComponentFromVectorArray(uIC.TC, Vector3.forward * -10f + Vector3.up * uIC.height * 0.5f, line, 4f, Color.gray, ResourceManager.GetMaterial("Line4"), _parent.canvasCamera, Position.Center);
		TextS.SetStyle("header");
		uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, uIC.currentVal.ToString(), 1f, Align.Center, Align.Middle);
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		if (touchEvent != TouchEvent.Began && touchEvent != TouchEvent.RollIn && touchEvent != TouchEvent.RollOut && touchEvent == TouchEvent.Release && !(_c.identifier != "background"))
		{
		}
	}
}

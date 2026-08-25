using UnityEngine;

public static class ButtonA
{
	public static UIC Assemble(Camera _camera, int _identifier, string _label, TouchEventDelegate _touchEventHandler, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.Button);
		uIC.width = 100f;
		uIC.height = 100f;
		uIC.canvasHeight = 100f;
		uIC.canvasWidth = 100f;
		uIC.headerHeight = 0f;
		uIC.footerHeight = 0f;
		uIC.contentMargin = 0f;
		uIC.contentSpacing = 0f;
		uIC.contentHAlign = Align.Center;
		uIC.contentVAlign = Align.Middle;
		uIC.identifier = _identifier;
		uIC.label = _label;
		uIC.contentTC = TransformS.AddComponent(transformC.entityIndex);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		if (_touchEventHandler != null)
		{
			uIC.TAC = TouchAreaS.AddComponent(transformC, _label, uIC.width, uIC.height, true, _camera, uIC);
			TouchAreaS.AddTouchEventListener(uIC.TAC, _touchEventHandler);
		}
		return uIC;
	}
}

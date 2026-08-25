using UnityEngine;

public static class GELibraryCategoryA
{
	public static UIC Assemble(UIC _parent, bool _expanded, string _label)
	{
		string[] tags = new string[1] { "LibraryItem" };
		UIC uIC = CanvasA.Assemble(_parent.canvasCamera, 0, _label, null, tags);
		UIS.AddToCanvasGrid(uIC, _parent, true);
		uIC.intent = _parent.intent + 1f;
		int subItemCount = UIS.GetSubItemCount(uIC, 0);
		subItemCount++;
		UIS.SetCanvasAbsoluteSize(uIC, _parent.width - _parent.contentMargin * 2f, 20f * (float)subItemCount, 20f, 0f);
		UIS.SetCanvasAbsoluteMarginAndSpacing(uIC, 0f, 0f);
		UIS.SetCanvasAlign(uIC, Align.Left, Align.Top);
		UIS.SetCanvasExpandable(uIC, true, _expanded);
		DrawItem(uIC, _parent.canvasCamera);
		if (!_parent.expanded)
		{
			EntityManager.SetActivityOfEntity(uIC.entityIndex, false, true);
		}
		return uIC;
	}

	public static void DrawItem(UIC _uic, Camera _camera)
	{
		float width = _uic.width;
		float height = _uic.height;
		float headerHeight = _uic.headerHeight;
		Camera camera = _camera;
		if (_uic.parent != null && _uic.parent.separateRenderSpace)
		{
			camera = _uic.parent.canvasCamera;
		}
		TextS.SetStyle("body");
		_uic.textC = TextS.AddSingleLineComponent(_uic.TC, _uic.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(_uic.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		TransformS.Move(_uic.textC.contentTC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.right * (width * -0.5f + headerHeight * _uic.intent));
		SpriteS.ConvertSpritesToPrefabComponent(_uic.textC.TC, camera, true);
		if (_uic.expandable)
		{
			Vector2[] array = new Vector2[3];
			if (_uic.expanded)
			{
				array[0] = Vector2.up * -5f;
				array[1] = Vector2.right * -5f + Vector2.up * 5f;
				array[2] = Vector2.right * 5f + Vector2.up * 5f;
			}
			else
			{
				array[0] = Vector2.right * 5f;
				array[1] = Vector2.right * -5f + Vector2.up * 5f;
				array[2] = Vector2.right * -5f + Vector2.up * -5f;
			}
			TouchAreaC touchAreaC = TouchAreaS.AddComponent(_uic.TC, "expand", width, headerHeight, true, _camera, _uic);
			TouchAreaS.SetNonRotatedOffset(touchAreaC, Vector3.up * (height * 0.5f - headerHeight * 0.5f));
			TouchAreaS.AddTouchEventListener(touchAreaC, HandleTouches);
			PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -10f + Vector3.right * (width * -0.5f + headerHeight * 0.5f), array, 8f, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
			PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -15f + Vector3.right * (width * -0.5f + headerHeight * 0.5f), array, PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), PrefabS.ColorToUInt(DebugDraw.GetColor(220f, 220f, 220f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty);
		}
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		if (_c.touchEvent[_i] != TouchEvent.Release || !_c.touchStartedInside[_i])
		{
			return;
		}
		if (uIC.expanded)
		{
			UIS.SetCanvasExpandable(uIC, true, false);
			UIS.SetActivityOfChildComponents(uIC, false);
		}
		else
		{
			UIS.SetCanvasExpandable(uIC, true, true);
			UIS.SetActivityOfChildComponents(uIC, true);
		}
		for (UIC uIC2 = uIC; uIC2 != null; uIC2 = uIC2.parent)
		{
			if (uIC2.parent != null && uIC2.identifier > -1)
			{
				int subItemCount = UIS.GetSubItemCount(uIC2, 0);
				subItemCount++;
				UIS.SetCanvasAbsoluteSize(uIC2, uIC2.parent.width - uIC2.parent.contentMargin * 2f, 20f * (float)subItemCount, 20f, 0f);
				PrefabS.RemoveComponentsByEntityIndex(uIC2.entityIndex);
				TouchAreaS.RemoveComponentsByTransformComponent(uIC2.TC);
				for (int i = 0; i < uIC2.TC.childs.Count; i++)
				{
					TouchAreaS.RemoveComponentsByTransformComponent(uIC2.TC.childs[i]);
				}
				TextS.RemoveComponent(uIC2.textC);
				DrawItem(uIC2, uIC2.parent.canvasCamera);
			}
			UIS.ResetCursor(uIC2);
			UIS.PlaceCanvasContent(uIC2);
		}
	}
}

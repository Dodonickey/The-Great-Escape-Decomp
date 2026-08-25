using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class FRoundButtonA
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024map10;

	public static TransformC Assemble(EIC _eic)
	{
		string[] tags = new string[3]
		{
			LevelManager.m_currentLevel.name + ":UI",
			LevelManager.m_currentLevel.name,
			_eic.identifier
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformC transformC2 = TransformS.AddComponent(transformC.entityIndex);
		TransformC transformC3 = TransformS.AddComponent(transformC.entityIndex);
		TransformS.SetPosition(transformC, _eic.data.position.ToVector3());
		TransformS.ParentComponent(transformC2, transformC, Vector3.zero);
		TransformS.ParentComponent(transformC3, transformC, Vector3.up * -105f);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, _eic.data as TriggerData, TriggerType.ButtonController, transformC2);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[0];
		TouchAreaC touchAreaC = null;
		if (!GEState.editorMode)
		{
			touchAreaC = TouchAreaS.AddComponent(transformC, _eic.identifier, 80f, true, _eic.camera, gETriggerC);
			TouchAreaS.AddTouchEventListener(touchAreaC, HandleTouches);
		}
		SpriteC spriteC = null;
		TextS.SetStyle("LP45");
		Color color = Color.gray;
		if (_eic.identifier == "Next Level Button")
		{
			TextC textC = TextS.AddSingleLineComponent(transformC3, "Next Level", 1f, Align.Center, Align.Middle);
			spriteC = SpriteS.AddComponent(transformC2, new Frame(256f, 0f, 128f, 128f), FarmState.menuSheet);
			color = DebugDraw.GetColor(95f, 159f, 0f);
		}
		else if (_eic.identifier == "Reset Level Button")
		{
			TextC textC2 = TextS.AddSingleLineComponent(transformC3, "Play Again", 1f, Align.Center, Align.Middle);
			spriteC = SpriteS.AddComponent(transformC2, new Frame(128f, 0f, 128f, 128f), FarmState.menuSheet);
			color = DebugDraw.GetColor(234f, 172f, 43f);
		}
		else if (_eic.identifier == "Level Menu Button")
		{
			TextC textC3 = TextS.AddSingleLineComponent(transformC3, "Level Menu", 1f, Align.Center, Align.Middle);
			spriteC = SpriteS.AddComponent(transformC2, new Frame(0f, 0f, 128f, 128f), FarmState.menuSheet);
			color = DebugDraw.GetColor(208f, 71f, 17f);
		}
		else if (_eic.identifier == "Jump Button")
		{
			spriteC = SpriteS.AddComponent(transformC2, new Frame(256f, 0f, 128f, 128f), FarmState.menuSheet);
			SpriteS.SetOffset(spriteC, Vector3.zero, 90f);
			color = DebugDraw.GetColor(27f, 21f, 17f, 140f);
			gETriggerC.dispatchOnlyOnce = false;
			if (touchAreaC != null)
			{
				TouchAreaS.RemoveTouchEventListener(touchAreaC, HandleTouches);
				TouchAreaS.AddTouchEventListener(touchAreaC, HandleIngameTouches);
			}
		}
		SpriteS.SetDimensions(spriteC, 90f, 90f);
		UIData uIData = _eic.data as UIData;
		Vector2[] circle = DebugDraw.GetCircle(80f, 50, Vector2.zero);
		DebugDraw.AddRadialRandom(circle, 5f);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(circle);
		polygon = GpcS.CleanPolygon(polygon, 5f, 0f, 20f, true);
		polygon = GpcS.SmoothPolygon(polygon, 5);
		DebugDraw.AddRadialRandom(circle, 5f);
		Polygon polygon2 = DebugDraw.Vector2ArrayToPolygon(circle);
		DebugDraw.ScalePolygon(polygon2, Vector2.one * 0.9f);
		polygon2 = GpcS.CleanPolygon(polygon2, 10f, 0f, 20f, true);
		polygon2 = GpcS.SmoothPolygon(polygon2, 5);
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC2, Vector3.forward * 0f, polygon, 8f, DebugDraw.GetColor(255f, 255f, 255f), ResourceManager.GetMaterial("Line8"), _eic.camera, Position.Center, true);
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC2, Vector3.forward * 5f, polygon2, 10f, DebugDraw.GetColor(255f, 255f, 255f, 64f), ResourceManager.GetMaterial("Line8"), _eic.camera, Position.Inside, true);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(transformC2, Vector3.forward * 10f, polygon, color, ResourceManager.GetMaterial("Solid"), _eic.camera);
		_eic.trigger = gETriggerC;
		return transformC;
	}

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (!_consumed)
		{
			GETriggerC gETriggerC = _c.customComponent as GETriggerC;
			if (_c.touchEvent[_i] == TouchEvent.Began || (_c.touchEvent[_i] == TouchEvent.RollIn && _c.touchStartedInside[_i]))
			{
				TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one * 1.05f, 0.1f, 0f);
			}
			else if (_c.touchEvent[_i] == TouchEvent.RollOut && _c.touchStartedInside[_i])
			{
				TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one, 0.2f, 0f);
			}
			else if (_c.touchEvent[_i] == TouchEvent.Release)
			{
				TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one, 0.2f, 0f);
			}
			if ((!gETriggerC.triggerOnlyOnce || gETriggerC.beganTime == 0f) && _c.touchEvent[_i] == TouchEvent.Release && _c.touchStartedInside[_i])
			{
				gETriggerC.collidingCount++;
				GETriggerLogic.HandleBeginTriggerEvent(gETriggerC);
			}
		}
	}

	private static void HandleIngameTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		if (_consumed)
		{
			return;
		}
		GETriggerC gETriggerC = _c.customComponent as GETriggerC;
		if (!gETriggerC.triggerOnlyOnce || gETriggerC.beganTime == 0f)
		{
			if (_c.touchEvent[_i] == TouchEvent.Began && _c.touchStartedInside[_i])
			{
				gETriggerC.collidingCount++;
				GETriggerLogic.HandleBeginTriggerEvent(gETriggerC);
			}
			else if (_c.touchEvent[_i] == TouchEvent.Release && _c.touchStartedInside[_i])
			{
				gETriggerC.collidingCount--;
				GETriggerLogic.HandleEndTriggerEvent(gETriggerC);
			}
		}
		if (_c.touchEvent[_i] == TouchEvent.Began || (_c.touchEvent[_i] == TouchEvent.RollIn && _c.touchStartedInside[_i]))
		{
			TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one * 1.05f, 0.1f, 0f);
		}
		else if (_c.touchEvent[_i] == TouchEvent.RollOut && _c.touchStartedInside[_i])
		{
			TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one, 0.2f, 0f);
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release)
		{
			TweenS.AddTransformTween(gETriggerC.TC, TweenedProperty.Scale, TweenStyle.CubicOut, Vector3.one, 0.2f, 0f);
		}
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		TriggerData triggerData = new TriggerData();
		triggerData.position = new Vertex3(_pos);
		triggerData.rotation = new Vertex3(_rot);
		triggerData.scale = new Vertex3(_sca);
		triggerData.triggerType = 23u;
		triggerData.active = true;
		triggerData.toggle = false;
		triggerData.triggerOnlyOnce = false;
		triggerData.triggerUntilOutOfEnergy = false;
		triggerData.triggerOnlyOnFullEnergy = false;
		triggerData.autoTrigger = false;
		triggerData.energy = 1f;
		triggerData.energyClips = -1;
		triggerData.energyGain = 0f;
		triggerData.energyConsume = 0f;
		triggerData.gainInterval = 0f;
		triggerData.consumeInterval = 0f;
		triggerData.cooldown = 0f;
		uint uniqueId = GES.GetUniqueId();
		triggerData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, triggerData, Main.uiCamera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = false;
		eIC.isScaleable = false;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.uiCamera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		TransformC item = Assemble(_eic);
		_eic.gameComponents.Add(item);
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		BasicLevelData basicLevelData = _eic.data as BasicLevelData;
	}

	public static void HandlePropertyChange(EventC _c)
	{
		BasicLevelData basicLevelData = EditorState.m_selection[0].data as BasicLevelData;
		string identifier = _c.identifier;
		if (identifier != null)
		{
			if (_003C_003Ef__switch_0024map10 == null)
			{
				_003C_003Ef__switch_0024map10 = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024map10.TryGetValue(identifier, out value))
			{
			}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

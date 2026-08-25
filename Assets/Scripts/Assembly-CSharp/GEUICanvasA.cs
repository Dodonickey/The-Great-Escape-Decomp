using UnityEngine;

public static class GEUICanvasA
{
	public static GETriggerC Assemble(Camera _camera, TriggerData _data, Vector3 _pos)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":UI",
			LevelManager.m_currentLevel.name
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformS.SetPosition(transformC, _pos);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_camera, _data, TriggerType.SensorController, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[0];
		gETriggerC.debugTC = EntityManager.AddEntityWithTC(tags);
		TransformS.ParentComponent(gETriggerC.debugTC, transformC, Vector3.zero);
		gETriggerC.debug = TextS.AddSingleLineComponent(gETriggerC.debugTC, string.Empty, 0.3f, Align.Center, Align.Middle);
		if (!GEState.editorMode)
		{
			TouchAreaC touchAreaComponent = TouchAreaS.AddComponent(transformC, "button", 40f, true, _camera, gETriggerC);
			TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleTouches);
		}
		DebugDraw.CreateCircle(_camera, transformC, Vector2.zero, 40f, false);
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		return gETriggerC;
	}

	private static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		GETriggerC gETriggerC = _c.customComponent as GETriggerC;
		if (_c.touchEvent[_i] == TouchEvent.Began || _c.touchEvent[_i] == TouchEvent.RollIn || (_c.touchWasInside[_i] && (_c.touchEvent[_i] == TouchEvent.Down || _c.touchEvent[_i] == TouchEvent.Drag)))
		{
			if (gETriggerC.toggle && gETriggerC.triggered && !gETriggerC.triggerOnlyOnce)
			{
				gETriggerC.update = true;
				gETriggerC.triggered = false;
				gETriggerC.end = true;
				gETriggerC.endTime = Main.m_gameTime;
			}
			else if (!gETriggerC.triggered && (!gETriggerC.triggerOnlyOnFullEnergy || (gETriggerC.triggerOnlyOnFullEnergy && gETriggerC.energy == 1f)))
			{
				gETriggerC.update = true;
				gETriggerC.triggered = true;
				gETriggerC.began = true;
				gETriggerC.beganTime = Main.m_gameTime;
			}
		}
		else if ((_c.touchEvent[_i] == TouchEvent.RollOut || _c.touchEvent[_i] == TouchEvent.Release) && !gETriggerC.toggle && !gETriggerC.triggerOnlyOnce && !gETriggerC.triggerUntilOutOfEnergy)
		{
			gETriggerC.update = true;
			gETriggerC.triggered = false;
			gETriggerC.end = true;
			gETriggerC.endTime = Main.m_gameTime;
		}
	}
}

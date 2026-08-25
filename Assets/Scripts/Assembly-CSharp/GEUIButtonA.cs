using System.Collections.Generic;
using UnityEngine;

public static class GEUIButtonA
{
	public static List<IComponent> Assemble(Camera _camera, BasicLevelData _data, Vector3 _pos)
	{
		List<IComponent> list = new List<IComponent>();
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":UI",
			LevelManager.m_currentLevel.name
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformS.SetPosition(transformC, _pos);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_camera, _data as TriggerData, TriggerType.SensorController, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[0];
		if (!GEState.editorMode)
		{
			TouchAreaC touchAreaComponent = TouchAreaS.AddComponent(transformC, "button", 40f, true, _camera, gETriggerC);
			TouchAreaS.AddTouchEventListener(touchAreaComponent, HandleTouches);
		}
		UIData uIData = _data as UIData;
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(uIData.width, uIData.height, uIData.round, 6, Vector2.zero, false);
		DebugDraw.AddRadialRandom(roundedRect, uIData.random);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
		polygon = GpcS.CleanPolygon(polygon, 10f, 0f, 20f, true);
		polygon = GpcS.SmoothPolygon(polygon, 5);
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.forward * 0f, polygon, 8f, DebugDraw.GetColor(255f, 255f, 255f), ResourceManager.GetMaterial("Line8"), _camera, Position.Center, true);
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.forward * 5f, polygon, 18f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Line16"), _camera, Position.Inside, true);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(transformC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(128f, 128f, 128f), ResourceManager.GetMaterial("Solid"), _camera);
		list.Add(gETriggerC);
		list.Add(transformC);
		if (!_data.active)
		{
			EntityManager.SetActivityOfEntity(gETriggerC.entityIndex, _data.active, true);
		}
		return list;
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

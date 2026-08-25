using UnityEngine;

public static class GECounterA
{
	public static GETriggerC Assemble(Camera _camera, TriggerData _data, Vector3 _pos)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Controller",
			LevelManager.m_currentLevel.name
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		TransformS.SetPosition(transformC, _pos);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_camera, _data, TriggerType.CountController, transformC);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[0];
		return gETriggerC;
	}
}

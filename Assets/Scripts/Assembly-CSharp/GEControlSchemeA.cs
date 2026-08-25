using UnityEngine;

public static class GEControlSchemeA
{
	public static GEControlSchemeC Assemble(EIC _eic, ControlSchemeData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Controller",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, _data.position.ToVector3());
		PlayerState playerState = GameState.AddPlayer();
		GEControlSchemeC gEControlSchemeC = GES.AddControlSchemeComponent(_eic.camera, transformC, _data.id, playerState);
		gEControlSchemeC.outputSlots = new ConnectionSlot[1];
		gEControlSchemeC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gEControlSchemeC.inputSlots = new ConnectionSlot[10];
		gEControlSchemeC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Move, 0);
		gEControlSchemeC.inputSlots[1] = new ConnectionSlot(ConnectionSlotType.Look, 1);
		gEControlSchemeC.inputSlots[2] = new ConnectionSlot(ConnectionSlotType.Boost, 2);
		gEControlSchemeC.inputSlots[3] = new ConnectionSlot(ConnectionSlotType.Jump, 3);
		gEControlSchemeC.inputSlots[4] = new ConnectionSlot(ConnectionSlotType.Fly, 4);
		gEControlSchemeC.inputSlots[5] = new ConnectionSlot(ConnectionSlotType.Dive, 5);
		gEControlSchemeC.inputSlots[6] = new ConnectionSlot(ConnectionSlotType.Grip, 6);
		gEControlSchemeC.inputSlots[7] = new ConnectionSlot(ConnectionSlotType.Carry, 7);
		gEControlSchemeC.inputSlots[8] = new ConnectionSlot(ConnectionSlotType.Use, 8);
		gEControlSchemeC.inputSlots[9] = new ConnectionSlot(ConnectionSlotType.Gravity, 9);
		gEControlSchemeC.modifierSlots = new ConnectionSlot[0];
		gEControlSchemeC.autoTrigger = true;
		gEControlSchemeC.energy = 1f;
		gEControlSchemeC.triggerType = TriggerType.ControlScheme;
		_eic.trigger = gEControlSchemeC;
		return gEControlSchemeC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		ControlSchemeData controlSchemeData = new ControlSchemeData();
		controlSchemeData.position = new Vertex3(_pos);
		controlSchemeData.rotation = new Vertex3(_rot);
		controlSchemeData.scale = new Vertex3(_sca);
		controlSchemeData.triggerType = 11u;
		controlSchemeData.playerIndex = 0;
		controlSchemeData.playerName = "Player";
		controlSchemeData.active = true;
		uint uniqueId = GES.GetUniqueId();
		controlSchemeData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, controlSchemeData, Main.uiCamera);
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
		ControlSchemeData data = _eic.data as ControlSchemeData;
		GEControlSchemeC gEControlSchemeC = Assemble(_eic, data);
		_eic.gameComponents.Add(gEControlSchemeC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(gEControlSchemeC.TC, _eic.TC, Vector3.zero);
		}
	}
}

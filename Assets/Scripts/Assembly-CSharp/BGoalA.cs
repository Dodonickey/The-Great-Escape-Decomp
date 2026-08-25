using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BGoalA
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024map3;

	public static BGoalC Assemble(EIC _eic)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		BlobGoalData blobGoalData = _eic.data as BlobGoalData;
		TransformC transformC = TransformS.AddComponent(entity);
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, true, (ColliderType)21, 0u, blobGoalData.colliderType, false, false);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBody(chipmunkC.isStatic, chipmunkC.isRogue, blobGoalData.position.ToVector2(), chipmunkC.index, chipmunkC.colliderType));
		ChipmunkWrapper.AddCircleShape(chipmunkC.cpBodyPtr, Vector2.zero, 1f, blobGoalData.radius, 0f, 0f, 0u, 17895697u, true);
		Vector2[] circle = DebugDraw.GetCircle(blobGoalData.radius, 36, Vector2.zero);
		PrefabS.CreatePathPrefabComponentFromPolygon(transformC, Vector3.zero, DebugDraw.Vector2ArrayToPolygon(circle), 6f, Color.red, ResourceManager.GetMaterial("Line6"), Main.camera, Position.Center, true);
		BGoalC bGoalC = BlobS.AddGoalComponent(chipmunkC, blobGoalData.radius);
		GETriggerC gETriggerC = GES.AddTriggerComponent(_eic.camera, blobGoalData, chipmunkC);
		gETriggerC.inputSlots = new ConnectionSlot[1];
		gETriggerC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Input, 0);
		gETriggerC.outputSlots = new ConnectionSlot[1];
		gETriggerC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		gETriggerC.modifierSlots = new ConnectionSlot[3];
		gETriggerC.modifierSlots[0] = new ConnectionSlot(ConnectionSlotType.Activate, 0);
		gETriggerC.modifierSlots[1] = new ConnectionSlot(ConnectionSlotType.Destroy, 1);
		gETriggerC.modifierSlots[2] = new ConnectionSlot(ConnectionSlotType.Modifier, 2);
		bGoalC.trigger = gETriggerC;
		_eic.trigger = gETriggerC;
		return bGoalC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		BlobGoalData blobGoalData = new BlobGoalData();
		blobGoalData.position = new Vertex3(_pos);
		blobGoalData.rotation = new Vertex3(_rot);
		blobGoalData.scale = new Vertex3(_sca);
		blobGoalData.radius = 35f;
		blobGoalData.triggerType = 50u;
		blobGoalData.active = true;
		blobGoalData.toggle = false;
		blobGoalData.triggerOnlyOnce = false;
		blobGoalData.triggerUntilOutOfEnergy = false;
		blobGoalData.triggerOnlyOnFullEnergy = false;
		blobGoalData.autoTrigger = false;
		blobGoalData.energy = 1f;
		blobGoalData.energyClips = -1;
		blobGoalData.energyGain = 0f;
		blobGoalData.energyConsume = 0f;
		blobGoalData.gainInterval = 0f;
		blobGoalData.consumeInterval = 0f;
		blobGoalData.cooldown = 0f;
		uint uniqueId = GES.GetUniqueId();
		blobGoalData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, blobGoalData, Main.camera);
		eIC.isRealtimeMovable = true;
		eIC.isDrawable = false;
		eIC.isRotateable = false;
		eIC.isScaleable = false;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		BGoalC bGoalC = Assemble(_eic);
		_eic.gameComponents.Add(bGoalC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(bGoalC.CMC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		BlobData blobData = _eic.data as BlobData;
	}

	public static void HandleShipPropertyChange(EventC _c)
	{
		BlobData blobData = EditorState.m_selection[0].data as BlobData;
		string identifier = _c.identifier;
		if (identifier != null)
		{
			if (_003C_003Ef__switch_0024map3 == null)
			{
				_003C_003Ef__switch_0024map3 = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024map3.TryGetValue(identifier, out value))
			{
			}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

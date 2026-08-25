using UnityEngine;

public class AShipA
{
	public int accSpeed;

	public float rotSpeed;

	public int breakStrength;

	public int frameSpeed;

	public static AShipC Assemble(EIC _eic, ShipData _data)
	{
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":GameEntity",
			LevelManager.m_currentLevel.name
		};
		Entity entity = EntityManager.AddEntity(tags);
		Vector3 vector = _data.position.ToVector3();
		vector.z += 40f;
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, vector);
		Vector2[] array = new Vector2[4]
		{
			new Vector2(0f, 20f),
			new Vector2(-10f, -20f),
			new Vector2(0f, -10f),
			new Vector2(10f, -20f)
		};
		PrefabC prefab = PrefabS.AddComponent(transformC, Vector3.zero, ResourceManager.GetGameObject("Ship"));
		ChipmunkC chipmunkC = ChipmunkS.AddInactiveComponent(transformC, false, ColliderType.Any);
		ChipmunkS.ActivateChipmunkComponent(chipmunkC, ChipmunkWrapper.AddBoxBody(false, false, vector, chipmunkC.index, Vector2.zero, 2f, 20f, 40f, 1f, 3f, 0u, GEState.layer_front, false, ColliderType.Any));
		ChipmunkWrapper.SetBodyVelocityLimits(chipmunkC.cpBodyPtr, _data.maxSpeed, _data.maxSpeed);
		ChipmunkWrapper.SetCustomBodyGravity(chipmunkC.cpBodyPtr, Vector2.zero);
		ChipmunkWrapper.SetCustomBodyLinearDamp(chipmunkC.cpBodyPtr, Vector2.one * 0.95f);
		AShipC aShipC = ASystem.AddShipComponent(entity, chipmunkC, _data);
		ChipmunkS.SetCustomComponent(chipmunkC, aShipC);
		aShipC.TC = transformC;
		aShipC.prefab = prefab;
		aShipC.inputSlots = new ConnectionSlot[3];
		aShipC.inputSlots[0] = new ConnectionSlot(ConnectionSlotType.Move, 0);
		aShipC.inputSlots[1] = new ConnectionSlot(ConnectionSlotType.Boost, 1);
		aShipC.inputSlots[2] = new ConnectionSlot(ConnectionSlotType.Dive, 2);
		aShipC.outputSlots = new ConnectionSlot[1];
		aShipC.outputSlots[0] = new ConnectionSlot(ConnectionSlotType.Output, 0);
		aShipC.modifierSlots = new ConnectionSlot[0];
		_eic.trigger = aShipC;
		return aShipC;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		ShipData shipData = new ShipData();
		shipData.position = new Vertex3(_pos);
		shipData.rotation = new Vertex3(_rot);
		shipData.scale = new Vertex3(_sca);
		uint uniqueId = GES.GetUniqueId();
		shipData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, shipData, Main.camera);
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
		ShipData data = _eic.data as ShipData;
		AShipC aShipC = Assemble(_eic, data);
		_eic.gameComponents.Add(aShipC);
		if (GEState.editorMode)
		{
			TransformS.ParentComponent(aShipC.CMC.TC, _eic.TC, Vector3.zero);
		}
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		ShipData shipData = _eic.data as ShipData;
		UIC component = NumericFieldA.Assemble(canvasCamera, "Player Index", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 3f, shipData.plrIdx, tags);
		UIC component2 = NumericFieldA.Assemble(canvasCamera, "Max Speed", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 9999f, shipData.maxSpeed, tags);
		UIC component3 = NumericFieldA.Assemble(canvasCamera, "Acceleration Spd", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 5000f, shipData.accSpeed, tags);
		UIC component4 = NumericFieldA.Assemble(canvasCamera, "Rotation Spd", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, false, 0f, 10f, shipData.rotSpeed, tags);
		UIC component5 = NumericFieldA.Assemble(canvasCamera, "Break Strength (less is more)", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 20f, shipData.breakStrength, tags);
		UIC component6 = NumericFieldA.Assemble(canvasCamera, "Anim Frame Spd", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 60f, shipData.frameSpeed, tags);
		UIC component7 = NumericFieldA.Assemble(canvasCamera, "Bullet Speed", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 9999f, shipData.bulletSpeed, tags);
		UIC component8 = NumericFieldA.Assemble(canvasCamera, "Bullet Lifetime", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 999f, shipData.bulletLifetime, tags);
		UIC component9 = NumericFieldA.Assemble(canvasCamera, "Health / Hitpoints", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 99f, shipData.health, tags);
		UIC component10 = NumericFieldA.Assemble(canvasCamera, "Bullet Firing Delay", HandleShipPropertyChange, null, true, Align.Left, 40f, 1f, true, 0f, 999f, shipData.firingDelay, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Ship Modifiers", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component9, _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, true);
		UIS.AddToCanvasGrid(component3, _propertyBar, true);
		UIS.AddToCanvasGrid(component4, _propertyBar, true);
		UIS.AddToCanvasGrid(component5, _propertyBar, true);
		UIS.AddToCanvasGrid(component6, _propertyBar, true);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Bullet Modifiers", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component7, _propertyBar, true);
		UIS.AddToCanvasGrid(component8, _propertyBar, true);
		UIS.AddToCanvasGrid(component10, _propertyBar, true);
	}

	public static void HandleShipPropertyChange(EventC _c)
	{
		ShipData shipData = EditorState.m_selection[0].data as ShipData;
		switch (_c.identifier)
		{
		case "Player Index":
			shipData.plrIdx = int.Parse(_c.properties["value"].ToString());
			break;
		case "Max Speed":
			shipData.maxSpeed = int.Parse(_c.properties["value"].ToString());
			break;
		case "Acceleration Spd":
			shipData.accSpeed = int.Parse(_c.properties["value"].ToString());
			break;
		case "Rotation Spd":
			shipData.rotSpeed = (float)_c.properties["value"];
			break;
		case "Break Strength (less is more)":
			shipData.breakStrength = int.Parse(_c.properties["value"].ToString());
			break;
		case "Anim Frame Spd":
			shipData.frameSpeed = int.Parse(_c.properties["value"].ToString());
			break;
		case "Health / Hitpoints":
			shipData.health = int.Parse(_c.properties["value"].ToString());
			break;
		case "Bullet Speed":
			shipData.bulletSpeed = int.Parse(_c.properties["value"].ToString());
			break;
		case "Bullet Lifetime":
			shipData.bulletLifetime = int.Parse(_c.properties["value"].ToString());
			break;
		case "Bullet Firing Delay":
			shipData.firingDelay = int.Parse(_c.properties["value"].ToString());
			break;
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

using System.IO;
using UnityEngine;

public class GEGame : IGame
{
	private string _projectCode;

	private string _projectVersion;

	private float m_prevTime;

	private float m_currentTime;

	private float m_delta;

	private float m_cumulatedFrameTime;

	private int m_fpsUpdateInterval;

	private float m_fpsCumulatedDelta;

	public static SceneManager m_sceneManager;

	public static int m_updateLoops = 1;

	public string m_projectCode
	{
		get
		{
			return _projectCode;
		}
		set
		{
			_projectCode = value;
		}
	}

	public string m_projectVersion
	{
		get
		{
			return _projectVersion;
		}
		set
		{
			_projectVersion = value;
		}
	}

	public GEGame(GEPlugin[] _plugins, string _projectCode, string _projectVersion)
	{
		if (Main.m_targetFPS == 30)
		{
			m_updateLoops = 2;
		}
		Application.targetFrameRate = Main.m_targetFPS;
		Debug.Initialize(DebugLevel.All);
		GEState.plugins = _plugins;
		m_projectCode = _projectCode;
		m_projectVersion = _projectVersion;
		GameState.Initialize(4);
		GameObject gameObject = new GameObject("UI Camera");
		Camera camera = gameObject.AddComponent<Camera>() as Camera;
		camera.orthographic = true;
		camera.orthographicSize = (float)Screen.height * 0.5f;
		camera.depth = 1f;
		camera.cullingMask = 256;
		camera.gameObject.layer = 8;
		camera.nearClipPlane = 1f;
		camera.farClipPlane = 1000f;
		camera.gameObject.transform.position = new Vector3(0f, 0f, -500f);
		camera.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		camera.clearFlags = CameraClearFlags.Nothing;
		Main.uiCamera = camera;
		Main.camera = Camera.main;
		Main.camera.cullingMask = 1;
		Main.camera.fieldOfView = 40f;
		Main.camera.farClipPlane = 2600f;
		Main.camera.nearClipPlane = 10f;
		AbstractPhysics.Initialize(PhysicsEngine.Chipmunk, 300);
		AbstractPhysics.CreateWorld(Vector2.up * -450f, 30, 0.9f, 1f, 0.5f);
		EntityManager.Initialize(800);
		GpcS.Initialize(100);
		TextS.Initialize(5);
		TouchAreaS.Initialize(500);
		TransformS.Initialize(2500);
		TweenS.Initialize(500);
		PrefabS.Initialize(1500);
		SoundS.Initialize(12, 20, Main.camera.GetComponent("AudioListener") as AudioListener);
		EventS.Initialize(200);
		FTP.Inititalize();
		UIS.Initialize(500);
		SpriteS.Initialize();
		CameraS.Initialize(Main.camera);
		GES.Initialize();
		GESpritePrefabS.Initialize(500);
		AffectionInfo affectionInfo = new AffectionInfo
		{
			duration = 0f,
			interval = 0f,
			maxStack = 1,
			effect = new GEEffect(),
			type = GEAffectionType.Damaging
		};
		GECreatureLogic.m_beganAffections[0] = affectionInfo;
		AffectionInfo affectionInfo2 = new AffectionInfo
		{
			duration = 1f,
			interval = 0.1f,
			maxStack = 1,
			effect = new GEEffect()
		};
		affectionInfo2.effect.effects[5] = 100;
		affectionInfo2.effect.effectActive[5] = true;
		affectionInfo2.type = GEAffectionType.Damaging;
		GECreatureLogic.m_tickAffections[1] = affectionInfo2;
		AffectionInfo affectionInfo3 = new AffectionInfo
		{
			duration = 1f,
			interval = 0.5f,
			maxStack = 1,
			effect = new GEEffect()
		};
		affectionInfo3.effect.effects[5] = 10;
		affectionInfo3.effect.effectActive[5] = true;
		affectionInfo3.type = GEAffectionType.Damaging;
		GECreatureLogic.m_tickAffections[2] = affectionInfo3;
		ResourceManager.AddResourceGroup("Generated");
		ResourceManager.AddResourceToGroup("Generated", new UnityResource("Solid", "SolidMat", ResourceType.Material));
		ResourceManager.AddResourceToGroup("Generated", new UnityResource("Line4", "Line4Mat", ResourceType.Material));
		ResourceManager.AddResourceToGroup("Generated", new UnityResource("Line6", "Line6Mat", ResourceType.Material));
		ResourceManager.AddResourceToGroup("Generated", new UnityResource("Line8", "Line8Mat", ResourceType.Material));
		ResourceManager.AddResourceToGroup("Generated", new UnityResource("Line16", "Line16Mat", ResourceType.Material));
		ResourceManager.AddResourceToGroup("Generated", new UnityResource("Line32", "Line32Mat", ResourceType.Material));
		ResourceManager.LoadResourceGroup("Generated");
		ResourceManager.AddResourceGroup("EditorUI");
		ResourceManager.AddResourceToGroup("EditorUI", new UnityResource("EditorUIShader", "GameEditor/Shaders/EditorUIShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("EditorUI", new UnityResource("EditorUI", "GameEditor/SpriteSheets/EditorUIDif", ResourceType.Texture));
		ResourceManager.AddResourceToGroup("EditorUI", new UnityResource("EditorItemIcons", "GameEditor/SpriteSheets/EditorIconsDif", ResourceType.Texture));
		ResourceManager.AddResourceGroup("CommonGameAssets");
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("RotaryMotor", "GameEditor/Models/Constraints/RotaryMotor", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("Bolt", "GameEditor/Models/Constraints/Bolt", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("BoltNut", "GameEditor/Models/Constraints/BoltNut", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("Pin", "GameEditor/Models/Constraints/Pin", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("Bar", "GameEditor/Models/Constraints/Bar", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("ButtonBase", "GameEditor/Models/Triggers/Button/ButtonBase", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("ButtonTile", "GameEditor/Models/Triggers/Button/ButtonTile", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("ConstraintShader", "GameEditor/Shaders/ConstraintShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("ConstraintEndShader", "GameEditor/Shaders/ConstraintEndShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("ConstraintDif", "GameEditor/Materials/Constraints/ConstraintDif", ResourceType.Material));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("TriplanarShader", "GameEditor/Shaders/TriplanarShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("CommonGameAssets", new UnityResource("VoxelShader", "GameEditor/Shaders/VoxelShader", ResourceType.Shader));
		ResourceManager.LoadResourceGroup("CommonGameAssets");
		GEState.constraintSheet = SpriteS.AddSpriteSheet(200, Main.camera, ResourceManager.GetTexture("ConstraintDif"), ResourceManager.GetShader("ConstraintEndShader"), 1f);
		GEState.groundMats.Add(new GEMat("GameEditor", "Grass", "GrassFill", 5f, "GrassRoad", 1f));
		GEState.blockMats.Add(new GEMat("GameEditor", "Grass", "GrassFill", 0.025f, "GrassDynamic", 1f));
		GEState.backgroundMats.Add(new GEMat("GameEditor", "1", "Foliage", 10f));
		GEState.landscapeMats.Add(new GEMat("GameEditor", "1", "Landscape1"));
		ResourceManager.AddResourceGroup("Materials");
		for (int i = 0; i < GEState.groundMats.Count; i++)
		{
			GEMat gEMat = GEState.groundMats[i];
			if (gEMat.fill != string.Empty)
			{
				ResourceManager.AddResourceToGroup("Materials", new UnityResource(gEMat.fill, gEMat.projectCode + "/Materials/Fills/" + gEMat.fill + "Mat", ResourceType.Material));
			}
			if (gEMat.road != string.Empty)
			{
				ResourceManager.AddResourceToGroup("Materials", new UnityResource(gEMat.road, gEMat.projectCode + "/Materials/Belts/Road/" + gEMat.road + "Mat", ResourceType.Material));
			}
		}
		for (int j = 0; j < GEState.blockMats.Count; j++)
		{
			GEMat gEMat2 = GEState.blockMats[j];
			if (gEMat2.fill != string.Empty)
			{
				ResourceManager.AddResourceToGroup("Materials", new UnityResource(gEMat2.fill, gEMat2.projectCode + "/Materials/Fills/" + gEMat2.fill + "Mat", ResourceType.Material));
			}
			if (gEMat2.road != string.Empty)
			{
				ResourceManager.AddResourceToGroup("Materials", new UnityResource(gEMat2.road, gEMat2.projectCode + "/Materials/Belts/Dynamic/" + gEMat2.road + "Mat", ResourceType.Material));
			}
		}
		for (int k = 0; k < GEState.backgroundMats.Count; k++)
		{
			GEMat gEMat3 = GEState.backgroundMats[k];
			if (gEMat3.fill != string.Empty)
			{
				ResourceManager.AddResourceToGroup("Materials", new UnityResource(gEMat3.fill, gEMat3.projectCode + "/Materials/Backgrounds/" + gEMat3.fill + "Mat", ResourceType.Material));
			}
			if (gEMat3.road != string.Empty)
			{
				ResourceManager.AddResourceToGroup("Materials", new UnityResource(gEMat3.road, gEMat3.projectCode + "/Materials/Backgrounds/" + gEMat3.road + "Mat", ResourceType.Material));
			}
		}
		for (int l = 0; l < GEState.landscapeMats.Count; l++)
		{
			GEMat gEMat4 = GEState.landscapeMats[l];
			if (gEMat4.fill != string.Empty)
			{
				ResourceManager.AddResourceToGroup("Materials", new UnityResource(gEMat4.fill, gEMat4.projectCode + "/Materials/Landscapes/" + gEMat4.fill + "Mat", ResourceType.Material));
			}
			if (gEMat4.road != string.Empty)
			{
				ResourceManager.AddResourceToGroup("Materials", new UnityResource(gEMat4.road, gEMat4.projectCode + "/Materials/Landscapes/" + gEMat4.road + "Mat", ResourceType.Material));
			}
		}
		Font font = TextS.AddFont("Museo14", "GameEditor/Fonts/Museo14/", 500, 256, 128, 1f, Main.uiCamera);
		Font font2 = TextS.AddFont("Museo20", "GameEditor/Fonts/Museo20/", 200, 256, 128, 1f, Main.uiCamera);
		Font font3 = TextS.AddFont("Museo26", "GameEditor/Fonts/Museo26/", 200, 256, 256, 1f, Main.uiCamera);
		TextS.AddStyle("body", font);
		TextS.AddStyle("subheader", font2);
		TextS.AddStyle("header", font3);
		TextS.SetStyle("body");
		Entity entity = EntityManager.AddEntity();
		entity.persistent = true;
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, new Vector3((float)Screen.width * -0.5f, (float)Screen.height * 0.5f, 0f));
		GEState.fpsText = TextS.AddSingleLineComponent(transformC, "fps: ", 1f, Align.Left, Align.Top);
		DebugDraw.Initialize(Main.camera, Main.uiCamera);
		m_sceneManager = new SceneManager();
	}

	public void Initialize(IScene _scene)
	{
		m_sceneManager.ChangeScene(_scene);
	}

	public IScene GetCurrentScene()
	{
		return m_sceneManager.GetCurrentScene();
	}

	public void RemoveComponent(IComponent _c)
	{
		bool flag = false;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			flag = gEPlugin.RemoveComponent(_c);
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			switch (_c.componentType)
			{
			case (ComponentType)114:
				GES.RemoveAffectionComponent(_c as GEAffectionC);
				break;
			case (ComponentType)102:
				GES.RemoveBipedCharacterComponent(_c as GECharacterC);
				break;
			case (ComponentType)100:
				GES.RemoveBlockComponent(_c as GEBlockC);
				break;
			case (ComponentType)103:
				GES.RemoveConnectionComponent(_c as GEConnectionC);
				break;
			case (ComponentType)104:
				GES.RemoveControlSchemeComponent(_c as GEControlSchemeC);
				break;
			case (ComponentType)105:
				GES.RemoveConstraintComponent(_c as GEConstraintC);
				break;
			case (ComponentType)106:
				GES.RemoveEditorItemComponent(_c as EIC);
				break;
			case (ComponentType)108:
				GES.RemovePortalComponent(_c as GEPortalC);
				break;
			case (ComponentType)110:
				GES.RemoveShapeComponent(_c as GEShapeC);
				break;
			case (ComponentType)112:
				GES.RemoveTriggerComponent(_c as GETriggerC);
				break;
			case (ComponentType)111:
				GES.RemoveTransformGizmoComponent(_c as GETransformGizmoC);
				break;
			case (ComponentType)113:
				GES.RemoveVehicleComponent(_c as GEVehicleC);
				break;
			case (ComponentType)115:
				GEVoxelShapeS.RemoveComponent(_c as GEVoxelShapeC);
				break;
			case (ComponentType)116:
				GESpritePrefabS.RemoveComponent(_c as GESpritePrefabC);
				break;
			case (ComponentType)117:
				GES.RemovePhysicsAffectorComponent(_c as GEPhysicsAffectorC);
				break;
			case (ComponentType)101:
			case (ComponentType)107:
			case (ComponentType)109:
				break;
			}
		}
	}

	public ILevel GenerateLevel(ILevel _level)
	{
		ILevel level = null;
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			level = gEPlugin.GenerateLevel(_level);
			if (level != null)
			{
				return level;
			}
		}
		return GELevelGenerator.GenerateLevel(_level);
	}

	public void OptimizeUniqueIDs(EIC _item, ref uint[] _indices, ref uint _lastIndex)
	{
		if (_indices[_item.data.id] == 0)
		{
			_indices[_item.data.id] = _lastIndex;
			_item.data.id = _lastIndex;
			_lastIndex++;
		}
		else
		{
			_item.data.id = _indices[_item.data.id];
		}
		for (int i = 0; i < _item.subItems.Count; i++)
		{
			if (_item.subItems[i].data != null)
			{
				OptimizeUniqueIDs(_item.subItems[i], ref _indices, ref _lastIndex);
			}
		}
	}

	public void OptimizeUniqueIDs2(EIC _item, ref uint[] _indices, ref uint _lastIndex)
	{
		ConnectionData connectionData = _item.data as ConnectionData;
		if (_indices[connectionData.id] == 0)
		{
			_indices[connectionData.id] = _lastIndex;
			connectionData.id = _lastIndex;
			_lastIndex++;
		}
		else
		{
			connectionData.id = _indices[connectionData.id];
		}
		if (_indices[connectionData.startId] != 0)
		{
			connectionData.startId = _indices[connectionData.startId];
		}
		if (_indices[connectionData.endId] != 0)
		{
			connectionData.endId = _indices[connectionData.endId];
		}
		for (int i = 0; i < _item.subItems.Count; i++)
		{
			if (_item.subItems[i].data != null)
			{
				OptimizeUniqueIDs2(_item.subItems[i], ref _indices, ref _lastIndex);
			}
		}
	}

	public void SaveLevel(ILevel _level, string _fileName)
	{
		bool flag = false;
		if (EditorState.m_drawMode)
		{
			EditorState.m_isSelectionLocked = false;
			EditorState.m_drawMode = false;
			EditorState.SetHighlight(false, EditorState.m_selection[0]);
		}
		if (EditorState.m_selection != null)
		{
			EditorState.m_selection.Clear();
			EditorState.UpdateSelection();
			EntityManager.Update();
		}
		uint[] _indices = new uint[10000];
		GELevel gELevel = _level as GELevel;
		uint _lastIndex = 1u;
		for (int i = 0; i < gELevel.items.Count; i++)
		{
			OptimizeUniqueIDs(gELevel.items[i], ref _indices, ref _lastIndex);
		}
		for (int j = 0; j < gELevel.connections.Count; j++)
		{
			OptimizeUniqueIDs2(gELevel.connections[j], ref _indices, ref _lastIndex);
		}
		GEPlugin[] plugins = GEState.plugins;
		foreach (GEPlugin gEPlugin in plugins)
		{
			if (gEPlugin.SaveLevel(_level, _fileName))
			{
				return;
			}
		}
		string text = IO.GetApplicationRunPath() + "/Levels/";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string file = text + _fileName + ".bytes";
		GELevelSerializer gELevelSerializer = new GELevelSerializer();
		gELevelSerializer.SerializeLevel(file, _level as GELevel);
	}

	public void ClearLevel(ILevel _level)
	{
		EntityManager.RemoveEntitiesByTag(_level.name, true);
		TouchAreaS.m_abort = true;
	}

	public void Update()
	{
		Vector3 vector = -Main.camera.ScreenToWorldPoint(new Vector3((float)Screen.width * 0.5f + 1f, (float)Screen.height * 0.5f, Main.camera.transform.position.z));
		Main.m_gameCameraDistanceMultipler = 1f / (vector + Main.camera.transform.position).x;
		m_currentTime = Time.time;
		Main.m_gameDeltaTime = m_currentTime - m_prevTime;
		m_prevTime = m_currentTime;
		Main.m_gameTime += Main.m_gameDeltaTime;
		InputManager.Update();
		TouchAreaS.Update();
		for (int i = 0; i < m_updateLoops; i++)
		{
			TweenS.Update();
			CameraS.Update();
			UIS.Update();
			TransformS.Update();
			m_sceneManager.UpdateLogic();
			GES.Update();
			GEPlugin[] plugins = GEState.plugins;
			foreach (GEPlugin gEPlugin in plugins)
			{
				gEPlugin.Update();
			}
			EventS.Update();
			GESpritePrefabS.Update();
			PrefabS.Update();
			SpriteS.Update();
			SoundS.Update();
			EntityManager.Update();
		}
		m_fpsCumulatedDelta += Main.m_gameDeltaTime;
		m_fpsUpdateInterval++;
		if (m_fpsUpdateInterval == 30)
		{
			if (GEState.fpsText.active)
			{
				TextS.SetStyle("subheader");
				TextS.ChangeText(GEState.fpsText, "fps: " + (int)((float)m_fpsUpdateInterval / m_fpsCumulatedDelta));
			}
			m_fpsCumulatedDelta = 0f;
			m_fpsUpdateInterval = 0;
		}
	}
}

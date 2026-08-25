using UnityEngine;

public class TemplateGame : IGame
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

	public TemplateGame(string _projectCode, string _projectVersion)
	{
		Application.targetFrameRate = 60;
		m_projectCode = _projectCode;
		m_projectVersion = _projectVersion;
		GameObject gameObject = new GameObject("UI Camera");
		Camera camera = gameObject.AddComponent("Camera") as Camera;
		camera.orthographic = true;
		camera.orthographicSize = (float)Screen.height * 0.5f;
		camera.depth = 1f;
		camera.cullingMask = 256;
		camera.gameObject.layer = 8;
		camera.nearClipPlane = 1f;
		camera.farClipPlane = 500f;
		camera.gameObject.transform.position = new Vector3(0f, 0f, -250f);
		camera.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		camera.clearFlags = CameraClearFlags.Nothing;
		Camera.main.cullingMask = 1;
		AbstractPhysics.Initialize(PhysicsEngine.Chipmunk, 300);
		AbstractPhysics.CreateWorld(Vector2.up * -450f, 10, 0.9f, 1f, 0.5f);
		EntityManager.Initialize(800);
		GpcS.Initialize(100);
		TextS.Initialize(5);
		TouchAreaS.Initialize(300);
		TransformS.Initialize(1000);
		TweenS.Initialize(20);
		PrefabS.Initialize(800);
		SoundS.Initialize(12, 20, Main.camera.GetComponent("AudioListener") as AudioListener);
		EventS.Initialize(200);
		UIS.Initialize(200);
		SpriteS.Initialize();
		CameraS.Initialize(Camera.main);
		DebugDraw.Initialize(Camera.main, camera);
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
	}

	public ILevel GenerateLevel(ILevel _level)
	{
		return null;
	}

	public void SaveLevel(ILevel _level, string _fileName)
	{
	}

	public void ClearLevel(ILevel _level)
	{
		EntityManager.RemoveEntitiesByTag(_level.name, true);
	}

	public void Update()
	{
		m_currentTime = Time.time;
		Main.m_gameDeltaTime = m_currentTime - m_prevTime;
		m_prevTime = m_currentTime;
		Main.m_gameTime += Main.m_gameDeltaTime;
		InputManager.Update();
		TouchAreaS.Update();
		TweenS.Update();
		for (int i = 0; i < 1; i++)
		{
			CameraS.Update();
			UIS.Update();
			TransformS.Update();
			m_sceneManager.UpdateLogic();
			EventS.Update();
			PrefabS.Update();
			SpriteS.Update();
			SoundS.Update();
			EntityManager.Update();
		}
	}
}

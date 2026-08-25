using UnityEngine;

public class FGameScene : IScene, IStatedObject
{
	private StateMachine m_stateMachine;

	private bool m_destroyLoadingScreen;

	private int m_tics;

	public StateMachine StateMachine
	{
		get
		{
			return m_stateMachine;
		}
		set
		{
			m_stateMachine = value;
		}
	}

	public IState GetCurrentState()
	{
		return m_stateMachine.GetCurrentState();
	}

	public void CreateLoadingScreen()
	{
		Entity entity = EntityManager.AddEntity("LoadingScreen");
		entity.persistent = true;
		TransformC transformC = TransformS.AddComponent(entity);
		TransformS.SetPosition(transformC, Vector3.forward * -240f);
		Vector2[] rect = DebugDraw.GetRect(Screen.width, Screen.height, Vector2.zero);
		PrefabS.CreateFlatPrefabComponentsFromVectorArray(transformC, Vector3.forward, rect, PrefabS.ColorToUInt(Color.black), PrefabS.ColorToUInt(Color.black), ResourceManager.GetMaterial("Solid"), Main.uiCamera, "loadingBG");
		TextS.SetStyle("header");
		TextS.AddSingleLineComponent(transformC, "Loading...", 1f, Align.Center, Align.Bottom);
		m_destroyLoadingScreen = false;
	}

	public void DestroyLoadingScreen()
	{
		m_destroyLoadingScreen = true;
		m_tics = 0;
	}

	public void RemoveLoadingScreen()
	{
		EntityManager.RemoveEntitiesByTag("LoadingScreen");
		m_destroyLoadingScreen = false;
	}

	public void Load()
	{
		ResourceManager.LoadResourceGroup("menu resources");
		Initialize();
	}

	public void Initialize()
	{
		m_stateMachine = new StateMachine(this);
		m_stateMachine.ChangeState(new FFirstState());
		DestroyLoadingScreen();
	}

	public void Reset()
	{
		Destroy();
		Load();
	}

	public void Update()
	{
		m_stateMachine.Update();
		if (m_destroyLoadingScreen && m_tics > 1)
		{
			RemoveLoadingScreen();
		}
		m_tics++;
	}

	public void Destroy()
	{
		LevelManager.RemoveLevels();
		ResourceManager.UnloadResourceGroup("menu resources");
		m_stateMachine.Destroy();
		EntityManager.RemoveAllEntities();
	}

	~FGameScene()
	{
		Debug.Log(string.Concat(this, ": Memory Freed"));
	}
}

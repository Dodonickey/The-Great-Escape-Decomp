public class EditorScene : IScene, IStatedObject
{
	private StateMachine m_stateMachine;

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
	}

	public void DestroyLoadingScreen()
	{
		EntityManager.RemoveEntitiesByTag("LoadingScreen");
	}

	public void Load()
	{
		ResourceManager.LoadResourceGroup("editor resources");
		Initialize();
	}

	public void Initialize()
	{
		m_stateMachine = new StateMachine(this);
		m_stateMachine.ChangeState(new EditorState());
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
	}

	public void Destroy()
	{
		LevelManager.RemoveLevels();
		ResourceManager.UnloadResourceGroup("editor resources");
		m_stateMachine.Destroy();
		EntityManager.RemoveAllEntities();
	}

	~EditorScene()
	{
		Debug.Log(string.Concat(this, ": Memory Freed"));
	}
}

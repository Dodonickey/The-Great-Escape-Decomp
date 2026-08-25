public class TemplateScene : IScene, IStatedObject
{
	public string m_projectCode;

	private StateMachine m_stateMachine;

	public IState m_editorState;

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
		Initialize();
	}

	public void Initialize()
	{
		m_stateMachine = new StateMachine(this);
		m_stateMachine.ChangeState(new TemplateState());
		DestroyLoadingScreen();
		Debug.Log("Template Scene Initialized");
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
		m_stateMachine.Destroy();
		EntityManager.RemoveAllEntities();
	}

	~TemplateScene()
	{
		Debug.Log(string.Concat(this, ": Memory Freed"));
	}
}

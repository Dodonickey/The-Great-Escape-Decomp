public class StateMachine
{
	private IStatedObject m_parent;

	private IState m_currentState;

	private IState m_previousState;

	private IState m_globalState;

	private IState m_changeToState;

	public StateMachine(IStatedObject _parent)
	{
		m_parent = _parent;
	}

	public StateMachine()
	{
	}

	private void SetCurrentState(IState _s)
	{
		m_currentState = _s;
	}

	private void SetPreviousState(IState _s)
	{
		m_previousState = _s;
	}

	private void SetGlobalState(IState _s)
	{
		m_globalState = _s;
	}

	public IState GetCurrentState()
	{
		return m_currentState;
	}

	public IState GetPreviousState()
	{
		return m_previousState;
	}

	public IState GetGlobalState()
	{
		return m_globalState;
	}

	public void Update()
	{
		if (m_globalState != null)
		{
			m_globalState.Execute();
		}
		if (m_changeToState != null)
		{
			if (m_currentState != null)
			{
				m_previousState = m_currentState;
				m_currentState.Exit();
				m_currentState = m_changeToState;
				m_currentState.StateMachine = this;
				m_currentState.Enter(m_parent);
				m_currentState.Execute();
			}
			else
			{
				m_currentState = m_changeToState;
				m_currentState.StateMachine = this;
				m_currentState.Enter(m_parent);
			}
			m_changeToState = null;
		}
		if (m_currentState != null)
		{
			m_currentState.Execute();
		}
	}

	public void ChangeState(IState newState)
	{
		m_changeToState = newState;
	}

	public void ChangeGlobalState(IState newState)
	{
		if (m_globalState != null && newState != null)
		{
			m_globalState.Exit();
			m_globalState = newState;
			m_globalState.StateMachine = this;
			m_globalState.Enter(m_parent);
		}
		else if (newState != null)
		{
			m_globalState = newState;
			m_globalState.StateMachine = this;
			m_globalState.Enter(m_parent);
		}
	}

	public void RevertToPreviousState()
	{
		if (m_previousState != null)
		{
			ChangeState(m_previousState);
		}
	}

	public void Destroy()
	{
		if (m_currentState != null)
		{
			m_currentState.Exit();
		}
		if (m_globalState != null)
		{
			m_globalState.Exit();
		}
	}
}

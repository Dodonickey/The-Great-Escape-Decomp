public class BasicState : IState
{
	private StateMachine p_stateMachine;

	public StateMachine StateMachine
	{
		get
		{
			return p_stateMachine;
		}
		set
		{
			p_stateMachine = value;
		}
	}

	public virtual void Enter(IStatedObject _parent)
	{
	}

	public virtual void Execute()
	{
	}

	public virtual void Exit()
	{
	}
}

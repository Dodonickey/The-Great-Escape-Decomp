public interface IState
{
	StateMachine StateMachine { get; set; }

	void Enter(IStatedObject _parent);

	void Execute();

	void Exit();
}

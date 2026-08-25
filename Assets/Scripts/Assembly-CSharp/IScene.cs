public interface IScene
{
	StateMachine StateMachine { get; set; }

	IState GetCurrentState();

	void CreateLoadingScreen();

	void DestroyLoadingScreen();

	void Load();

	void Initialize();

	void Reset();

	void Update();

	void Destroy();
}

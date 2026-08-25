public class ControllerState
{
	public PlayerState playerState;

	public bool active;

	public bool[] changed;

	public IControlledComponent[] components;

	public ControllerState(PlayerState _playerState)
	{
		playerState = _playerState;
		components = new IControlledComponent[20];
	}
}

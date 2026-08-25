public static class GameState
{
	public static bool[] m_freeStates;

	public static PlayerState[] m_playerStates;

	public static int[] m_scores;

	public static void Initialize(int _maxPlayerCount)
	{
		m_freeStates = new bool[_maxPlayerCount];
		m_playerStates = new PlayerState[_maxPlayerCount];
		for (int i = 0; i < _maxPlayerCount; i++)
		{
			m_freeStates[i] = true;
			m_playerStates[i] = new PlayerState();
			m_playerStates[i].m_index = i;
			m_playerStates[i].m_playerIdentifier = -1;
			m_playerStates[i].m_playerName = "Player " + (i + 1);
			m_playerStates[i].m_contollerState = new ControllerState(m_playerStates[i]);
		}
		m_scores = new int[_maxPlayerCount];
	}

	public static PlayerState AddPlayer()
	{
		for (int i = 0; i < m_freeStates.Length; i++)
		{
			if (m_freeStates[i])
			{
				PlayerState result = m_playerStates[i];
				m_freeStates[i] = false;
				return result;
			}
		}
		return null;
	}

	public static void RemovePlayer(PlayerState _playerState)
	{
		m_freeStates[_playerState.m_index] = true;
		_playerState.m_playerIdentifier = -1;
		_playerState.m_playerName = "Player " + (_playerState.m_index + 1);
	}
}

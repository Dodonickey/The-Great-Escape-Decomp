using UnityEngine;

public static class Debug
{
	private static DebugLevel _debugLevel;

	public static DebugLevel debugLevel
	{
		get
		{
			return _debugLevel;
		}
	}

	public static void Initialize(DebugLevel debugLevel)
	{
		_debugLevel = debugLevel;
	}

	public static void Log(object message)
	{
		if (_debugLevel >= DebugLevel.All)
		{
			UnityEngine.Debug.Log(message);
		}
	}

	public static void LogError(object message)
	{
		if (_debugLevel >= DebugLevel.Error)
		{
			UnityEngine.Debug.LogError(message);
		}
	}

	public static void LogWarning(object message)
	{
		if (_debugLevel >= DebugLevel.Error)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}
}

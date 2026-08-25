using UnityEngine;

public class GEMain : MonoBehaviour
{
	private void Start()
	{
		GEPlugin[] plugins = new GEPlugin[0];
		Main.m_currentGame = new GEGame(plugins, "GameEditor", "0-9-0");
		Main.m_currentGame.Initialize(new EditorScene());
	}

	private void Update()
	{
		Main.m_currentGame.Update();
	}
}

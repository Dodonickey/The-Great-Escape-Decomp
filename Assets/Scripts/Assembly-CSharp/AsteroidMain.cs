using UnityEngine;

public class AsteroidMain : MonoBehaviour
{
	private void Start()
	{
		Main.m_currentGame = new GEGame(new GEPlugin[1]
		{
			new AsteroidPlugin()
		}, "Asteroids", "0-0-1");
		Main.m_currentGame.Initialize(new EditorScene());
	}

	private void Update()
	{
		Main.m_currentGame.Update();
	}
}

using UnityEngine;

public class Main : MonoBehaviour
{
	public static float m_gameTime;

	public static float m_gameDeltaTime;

	public static float m_gameCameraDistanceMultipler = 1f;

	public static int m_targetFPS = 60;

	public new static Camera camera;

	public static Camera uiCamera;

	public static IGame m_currentGame;

	private void Start()
	{
		m_currentGame = new TemplateGame("Framework", "1-0-0");
		m_currentGame.Initialize(new TemplateScene());
	}

	private void Update()
	{
		m_currentGame.Update();
	}
}

using UnityEngine;

public class FarmMain : MonoBehaviour
{
	private void Start()
	{
		Main.m_currentGame = new GEGame(new GEPlugin[4]
		{
			new FarmPlugin(),
			new AsteroidPlugin(),
			new BlobPlugin(),
			new PBPlugin()
		}, "Farm", "0-6-0");
		Main.m_currentGame.Initialize(new FGameScene());
	}

	private void Update()
	{
		Main.m_currentGame.Update();
	}
}

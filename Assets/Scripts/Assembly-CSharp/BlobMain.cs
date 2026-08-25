using UnityEngine;

public class BlobMain : MonoBehaviour
{
	private void Start()
	{
		Main.m_currentGame = new GEGame(new GEPlugin[1]
		{
			new BlobPlugin()
		}, "Blob", "0-0-1");
		Main.m_currentGame.Initialize(new EditorScene());
	}

	private void Update()
	{
		Main.m_currentGame.Update();
	}
}

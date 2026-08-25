using System;
using UnityEngine;

public class SceneManager
{
	private bool m_ready;

	public IScene m_currentScene;

	public IScene m_changeToScene;

	public void ChangeScene(IScene _scene)
	{
		m_changeToScene = _scene;
		m_ready = false;
	}

	public void DestroyScene(IScene _scene)
	{
		if (m_currentScene != null)
		{
			m_currentScene.Destroy();
			m_currentScene = null;
		}
	}

	public IScene GetCurrentScene()
	{
		return m_currentScene;
	}

	public void UpdateLogic()
	{
		UpdateLogic(true);
	}

	public void UpdateLogic(bool lastTick)
	{
		if (m_changeToScene != null)
		{
			if (!m_ready)
			{
				m_changeToScene.CreateLoadingScreen();
				if (m_currentScene != null)
				{
					m_currentScene.Destroy();
					m_currentScene = null;
					EntityManager.Update();
					AsyncOperation asyncOperation = Resources.UnloadUnusedAssets();
					GC.Collect();
				}
				m_ready = true;
			}
			else
			{
				m_currentScene = m_changeToScene;
				m_currentScene.Load();
				m_changeToScene = null;
			}
		}
		else
		{
			m_currentScene.Update();
		}
	}
}

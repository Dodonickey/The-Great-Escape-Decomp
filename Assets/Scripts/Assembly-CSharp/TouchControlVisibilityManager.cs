using System.Collections.Generic;
using UnityEngine;

public static class TouchControlVisibilityManager
{
	private static bool m_initialized;
	private static bool m_touchControlsVisible;
	private static List<TransformC> m_controlTransforms = new List<TransformC>();
	private static bool m_isNonMobilePlatform;
	private static bool m_touchDetectedOnce;

	public static void Initialize()
	{
		m_initialized = true;
		m_touchControlsVisible = false;
		m_touchDetectedOnce = false;
		
		// Determine if we're on a non-mobile platform
		m_isNonMobilePlatform = !Input.touchSupported || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer;
		
		// If on mobile, always show controls
		if (!m_isNonMobilePlatform)
		{
			m_touchControlsVisible = true;
		}
	}

	public static void Update()
	{
		if (!m_initialized)
			return;

		// Only apply visibility logic on non-mobile platforms
		if (!m_isNonMobilePlatform)
			return;

		// Check for actual touch input (not mouse)
		bool hasActualTouch = Input.touchCount > 0;

		if (hasActualTouch && !m_touchDetectedOnce)
		{
			// Real touch detected for the first time
			m_touchDetectedOnce = true;
			m_touchControlsVisible = true;
			SetControlsVisibility(true);
		}

		// Once touch has been detected once, controls stay visible permanently
		if (m_touchDetectedOnce)
		{
			m_touchControlsVisible = true;
		}
	}

	private static void SetControlsVisibility(bool _visible)
	{
		for (int i = 0; i < m_controlTransforms.Count; i++)
		{
			if (m_controlTransforms[i] != null)
			{
				m_controlTransforms[i].transform.gameObject.SetActive(_visible);
			}
		}
	}

	public static void RegisterControlTransform(TransformC _tc)
	{
		if (_tc != null && !m_controlTransforms.Contains(_tc))
		{
			m_controlTransforms.Add(_tc);
			
			// If controls should be hidden, hide immediately
			if (!m_touchControlsVisible && m_isNonMobilePlatform)
			{
				_tc.transform.gameObject.SetActive(false);
			}
		}
	}

	public static void UnregisterControlTransform(TransformC _tc)
	{
		if (_tc != null)
		{
			m_controlTransforms.Remove(_tc);
		}
	}

	public static bool AreControlsVisible()
	{
		return m_touchControlsVisible;
	}

	public static bool IsNonMobilePlatform()
	{
		return m_isNonMobilePlatform;
	}

	public static void ResetTouchDetection()
	{
		m_touchDetectedOnce = false;
		m_touchControlsVisible = false;
		
		for (int i = 0; i < m_controlTransforms.Count; i++)
		{
			if (m_controlTransforms[i] != null)
			{
				m_controlTransforms[i].transform.gameObject.SetActive(false);
			}
		}
	}
}
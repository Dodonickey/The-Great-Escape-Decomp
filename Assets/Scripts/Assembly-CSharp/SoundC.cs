using UnityEngine;

public class SoundC : IComponent
{
	private bool _active;

	private int _index;

	private int _entityIndex;

	private ComponentType _componentType;

	public string m_soundName;

	public string m_soundTag;

	public GameObject m_anchorObject;

	public bool m_isPlaying;

	public int m_soundId;

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
		}
	}

	public int index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}

	public int entityIndex
	{
		get
		{
			return _entityIndex;
		}
		set
		{
			_entityIndex = value;
		}
	}

	public ComponentType componentType
	{
		get
		{
			return _componentType;
		}
		set
		{
			_componentType = value;
		}
	}

	public void Play(float volume)
	{
		if (!m_isPlaying)
		{
			int num = SoundS.PlaySound(m_soundName, m_anchorObject, volume, true, m_soundTag);
			if (num >= 0)
			{
				m_soundId = num;
				m_isPlaying = true;
			}
			else
			{
				m_soundId = -1;
			}
		}
	}

	public void Stop()
	{
		if (m_isPlaying)
		{
			SoundS.RemoveSoundsWithTag(m_soundTag);
			m_isPlaying = false;
			m_soundId = -1;
		}
	}
}

using UnityEngine;

public static class SoundS
{
	private static SSound[] m_sounds;

	private static int m_maxSounds;

	private static AudioListener m_listener;

	private static float m_defaultMinDist;

	private static float m_defaultMaxDist;

	private static AudioRolloffMode m_defaultRollOffMode;

	private static float m_defaultDopplerLevel;

	private static float m_defaultPanLevel;

	public static GenericArray<SoundC> m_soundComponents;

	private static int ticks;

	public static void Initialize(int _maxSounds, int _maxComponents, AudioListener _listener)
	{
		m_soundComponents = new GenericArray<SoundC>(_maxComponents);
		for (int i = 0; i < _maxComponents; i++)
		{
			m_soundComponents.m_array[i] = new SoundC();
			m_soundComponents.m_array[i].m_soundTag = null;
			m_soundComponents.m_array[i].m_soundName = null;
			m_soundComponents.m_array[i].m_isPlaying = false;
			m_soundComponents.m_array[i].componentType = ComponentType.Sound;
		}
		m_maxSounds = _maxSounds;
		m_sounds = new SSound[m_maxSounds];
		for (int j = 0; j < m_maxSounds; j++)
		{
			m_sounds[j].source = null;
			m_sounds[j].tag = null;
		}
		SetListener(_listener);
		SetDefault3DSoundParameters(100f, 1000f, AudioRolloffMode.Linear, 0f, 0.9f);
	}

	public static void SetListener(AudioListener _listener)
	{
		m_listener = _listener;
		m_listener.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
	}

	public static void SetDefault3DSoundParameters(float _minDist, float _maxDist, AudioRolloffMode _rollOffMode, float _dopplerLevel, float _panLevel)
	{
		m_defaultMinDist = _minDist;
		m_defaultMaxDist = _maxDist;
		m_defaultRollOffMode = _rollOffMode;
		m_defaultDopplerLevel = _dopplerLevel;
		m_defaultPanLevel = _panLevel;
	}

	public static void Mute(bool mute)
	{
		m_listener.enabled = !mute;
	}

	public static int PlaySound(string _clipName, GameObject _gameObject = null, float _volume = 0.5f, bool _loop = false, string _tag = null, int _priority = 0, float _pitch = 1f)
	{
		AudioClip audioClip = ResourceManager.GetAudioClip(_clipName);
		return PlaySound(audioClip, _gameObject, _volume, _loop, _tag, _priority, _pitch);
	}

	public static int PlaySound(AudioClip _clip, GameObject _gameObject = null, float _volume = 0.5f, bool _loop = false, string _tag = null, int _priority = 0, float _pitch = 1f)
	{
		if (_clip == null)
		{
			return -1;
		}
		int num = -1;
		int num2 = 256;
		for (int i = 0; i < m_maxSounds; i++)
		{
			if (!m_sounds[i].alive)
			{
				num = i;
				break;
			}
			if (m_sounds[i].priority < _priority && m_sounds[i].priority < num2)
			{
				num2 = m_sounds[i].priority;
				num = i;
			}
		}
		if (num >= 0)
		{
			m_sounds[num].alive = true;
			m_sounds[num].paused = false;
			m_sounds[num].priority = _priority;
			m_sounds[num].tag = _tag;
			m_sounds[num].volumeTarget = _volume;
			m_sounds[num].volumeTweenSpeed = 0.1f;
			if (_gameObject == null)
			{
				_gameObject = m_listener.gameObject;
			}
			m_sounds[num].source = _gameObject.AddComponent<AudioSource>() as AudioSource;
			m_sounds[num].source.playOnAwake = false;
			m_sounds[num].source.clip = _clip;
			m_sounds[num].source.loop = _loop;
			m_sounds[num].source.volume = _volume;
			m_sounds[num].source.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
			m_sounds[num].source.dopplerLevel = m_defaultDopplerLevel;
			m_sounds[num].source.minDistance = m_defaultMinDist;
			m_sounds[num].source.maxDistance = m_defaultMaxDist;
			m_sounds[num].source.rolloffMode = m_defaultRollOffMode;
			m_sounds[num].source.spatialBlend = m_defaultPanLevel;
			m_sounds[num].source.pitch = _pitch;
			m_sounds[num].source.Play();
		}
		return num;
	}

	public static int FindFirstIdWithTag(string tag)
	{
		for (int i = 0; i < m_maxSounds; i++)
		{
			if (m_sounds[i].alive && m_sounds[i].tag != null && m_sounds[i].tag.Equals(tag))
			{
				return i;
			}
		}
		return -1;
	}

	public static void RemoveSound(int id)
	{
		m_sounds[id].alive = false;
		if (m_sounds[id].source != null)
		{
			Object.DestroyImmediate(m_sounds[id].source);
			m_sounds[id].source = null;
		}
	}

	public static void RemoveSoundsWithTag(string tag)
	{
		bool flag = true;
		while (flag)
		{
			int num = FindFirstIdWithTag(tag);
			if (num >= 0)
			{
				RemoveSound(num);
			}
			else
			{
				flag = false;
			}
		}
	}

	public static int PlayMusic(AudioClip _clip, float _volume, GameObject _gameObject)
	{
		return PlaySound(_clip, _gameObject, _volume, true, "Music", 256);
	}

	public static void StopMusic()
	{
		int num = FindFirstIdWithTag("Music");
		if (num >= 0)
		{
			RemoveSound(num);
		}
	}

	public static void PauseSound(int id)
	{
		if (!m_sounds[id].paused)
		{
			m_sounds[id].paused = true;
			m_sounds[id].source.Pause();
		}
	}

	public static void ResumeSound(int id)
	{
		if (m_sounds[id].paused)
		{
			m_sounds[id].paused = false;
			m_sounds[id].source.Play();
		}
	}

	public static void PauseAllAudio()
	{
		for (int i = 0; i < m_maxSounds; i++)
		{
			if (m_sounds[i].alive && !m_sounds[i].paused)
			{
				PauseSound(i);
			}
		}
	}

	public static void ResumeAllAudio()
	{
		for (int i = 0; i < m_maxSounds; i++)
		{
			if (m_sounds[i].alive && m_sounds[i].paused)
			{
				ResumeSound(i);
			}
		}
	}

	public static void setVolumeTarget(int _id, float _target, bool instant)
	{
		m_sounds[_id].volumeTarget = _target;
		if (instant && m_sounds[_id].source != null)
		{
			m_sounds[_id].source.volume = _target;
		}
	}

	public static void setVolumeTweenSpeed(int _id, float _speed)
	{
		m_sounds[_id].volumeTweenSpeed = _speed;
	}

	public static void setPitch(int _id, float _pitch)
	{
		if (m_sounds[_id].source != null)
		{
			m_sounds[_id].source.pitch = _pitch;
		}
	}

	public static void Update()
	{
		for (int i = 0; i < m_maxSounds; i++)
		{
			if (m_sounds[i].alive && !m_sounds[i].paused)
			{
				if (!m_sounds[i].source.isPlaying && !m_sounds[i].source.loop)
				{
					RemoveSound(i);
				}
				if (m_sounds[i].source != null && m_sounds[i].source.volume != m_sounds[i].volumeTarget)
				{
					m_sounds[i].source.volume += (m_sounds[i].volumeTarget - m_sounds[i].source.volume) * m_sounds[i].volumeTweenSpeed;
				}
			}
		}
	}

	public static SoundC AddComponent(Entity _e, string _soundName, GameObject _soundAnchor)
	{
		string soundTag = _soundName + _e.index;
		int num = m_soundComponents.AddItem();
		SoundC soundC = m_soundComponents.m_array[num];
		soundC.m_soundName = _soundName;
		soundC.m_soundTag = soundTag;
		soundC.m_anchorObject = _soundAnchor;
		_e.components.Add(soundC);
		return soundC;
	}

	public static void RemoveComponent(IComponent _c)
	{
		SoundC soundC = _c as SoundC;
		if (soundC.m_soundTag != null)
		{
			RemoveSoundsWithTag(soundC.m_soundTag);
		}
		soundC.m_isPlaying = false;
		soundC.m_soundId = -1;
		m_soundComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[soundC.entityIndex].components.Remove(_c);
	}
}

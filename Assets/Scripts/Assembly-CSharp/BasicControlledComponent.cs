using UnityEngine;

public class BasicControlledComponent : BasicComponent, IControlledComponent
{
	private TriggerType _triggerType;

	private TextC _debug;

	private TransformC _debugTC;

	private uint _id;

	private Camera _camera;

	private TransformC _TC;

	private ConnectionSlot[] _inputSlots;

	private ConnectionSlot[] _outputSlots;

	private ConnectionSlot[] _modifierSlots;

	private bool _update;

	private int _collidingCount;

	private int _triggerCount;

	private bool _autoTrigger;

	private bool _triggered;

	private bool _began;

	private bool _end;

	private float _beganTime;

	private float _endTime;

	private float _gainCooldown;

	private GEControlledValue _def;

	private GEControlledValue _input;

	private GEControlledValue _output;

	private GEControlledValue _modifier;

	private ModifierType _modifierType;

	private bool _toggle;

	private bool _triggerOnlyOnce;

	private bool _triggerOnlyOnFullEnergy;

	private bool _triggerUntilOutOfEnergy;

	private int _energyClips;

	private float _triggerCooldown;

	private float _reloadCooldown;

	private float _lastReload;

	private bool _reloading;

	private float _energy;

	private float _energyConsume;

	private float _energyGain;

	private float _energyConsumeInterval;

	private float _energyGainInterval;

	private float _lastConsume;

	private float _lastGain;

	private int _beganDelegatedCount;

	private TriggerEventDelegate _BeganEventDelegate;

	private int _endDelegatedCount;

	private TriggerEventDelegate _EndEventDelegate;

	public TriggerType triggerType
	{
		get
		{
			return _triggerType;
		}
		set
		{
			_triggerType = value;
		}
	}

	public TextC debug
	{
		get
		{
			return _debug;
		}
		set
		{
			_debug = value;
		}
	}

	public TransformC debugTC
	{
		get
		{
			return _debugTC;
		}
		set
		{
			_debugTC = value;
		}
	}

	public uint id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public Vector3 position
	{
		get
		{
			return TC.transform.position;
		}
	}

	public Camera camera
	{
		get
		{
			return _camera;
		}
		set
		{
			_camera = value;
		}
	}

	public TransformC TC
	{
		get
		{
			return _TC;
		}
		set
		{
			_TC = value;
		}
	}

	public ConnectionSlot[] inputSlots
	{
		get
		{
			return _inputSlots;
		}
		set
		{
			_inputSlots = value;
		}
	}

	public ConnectionSlot[] outputSlots
	{
		get
		{
			return _outputSlots;
		}
		set
		{
			_outputSlots = value;
		}
	}

	public ConnectionSlot[] modifierSlots
	{
		get
		{
			return _modifierSlots;
		}
		set
		{
			_modifierSlots = value;
		}
	}

	public bool update
	{
		get
		{
			return _update;
		}
		set
		{
			_update = value;
		}
	}

	public int collidingCount
	{
		get
		{
			return _collidingCount;
		}
		set
		{
			_collidingCount = value;
		}
	}

	public int triggerCount
	{
		get
		{
			return _triggerCount;
		}
		set
		{
			_triggerCount = value;
		}
	}

	public bool autoTrigger
	{
		get
		{
			return _autoTrigger;
		}
		set
		{
			_autoTrigger = value;
		}
	}

	public bool triggered
	{
		get
		{
			return _triggered;
		}
		set
		{
			_triggered = value;
		}
	}

	public bool began
	{
		get
		{
			return _began;
		}
		set
		{
			_began = value;
		}
	}

	public bool end
	{
		get
		{
			return _end;
		}
		set
		{
			_end = value;
		}
	}

	public float beganTime
	{
		get
		{
			return _beganTime;
		}
		set
		{
			_beganTime = value;
		}
	}

	public float endTime
	{
		get
		{
			return _endTime;
		}
		set
		{
			_endTime = value;
		}
	}

	public float gainCooldown
	{
		get
		{
			return _gainCooldown;
		}
		set
		{
			_gainCooldown = value;
		}
	}

	public GEControlledValue def
	{
		get
		{
			return _def;
		}
		set
		{
			_def = value;
		}
	}

	public GEControlledValue input
	{
		get
		{
			return _input;
		}
		set
		{
			_input = value;
		}
	}

	public GEControlledValue output
	{
		get
		{
			return _output;
		}
		set
		{
			_output = value;
		}
	}

	public GEControlledValue modifier
	{
		get
		{
			return _modifier;
		}
		set
		{
			_modifier = value;
		}
	}

	public ModifierType modifierType
	{
		get
		{
			return _modifierType;
		}
		set
		{
			_modifierType = value;
		}
	}

	public bool toggle
	{
		get
		{
			return _toggle;
		}
		set
		{
			_toggle = value;
		}
	}

	public bool triggerOnlyOnce
	{
		get
		{
			return _triggerOnlyOnce;
		}
		set
		{
			_triggerOnlyOnce = value;
		}
	}

	public bool triggerOnlyOnFullEnergy
	{
		get
		{
			return _triggerOnlyOnFullEnergy;
		}
		set
		{
			_triggerOnlyOnFullEnergy = value;
		}
	}

	public bool triggerUntilOutOfEnergy
	{
		get
		{
			return _triggerUntilOutOfEnergy;
		}
		set
		{
			_triggerUntilOutOfEnergy = value;
		}
	}

	public int energyClips
	{
		get
		{
			return _energyClips;
		}
		set
		{
			_energyClips = value;
		}
	}

	public float triggerCooldown
	{
		get
		{
			return _triggerCooldown;
		}
		set
		{
			_triggerCooldown = value;
		}
	}

	public float reloadCooldown
	{
		get
		{
			return _reloadCooldown;
		}
		set
		{
			_reloadCooldown = value;
		}
	}

	public float lastReload
	{
		get
		{
			return _lastReload;
		}
		set
		{
			_lastReload = value;
		}
	}

	public bool reloading
	{
		get
		{
			return _reloading;
		}
		set
		{
			_reloading = value;
		}
	}

	public float energy
	{
		get
		{
			return _energy;
		}
		set
		{
			_energy = value;
		}
	}

	public float energyConsume
	{
		get
		{
			return _energyConsume;
		}
		set
		{
			_energyConsume = value;
		}
	}

	public float energyGain
	{
		get
		{
			return _energyGain;
		}
		set
		{
			_energyGain = value;
		}
	}

	public float energyConsumeInterval
	{
		get
		{
			return _energyConsumeInterval;
		}
		set
		{
			_energyConsumeInterval = value;
		}
	}

	public float energyGainInterval
	{
		get
		{
			return _energyGainInterval;
		}
		set
		{
			_energyGainInterval = value;
		}
	}

	public float lastConsume
	{
		get
		{
			return _lastConsume;
		}
		set
		{
			_lastConsume = value;
		}
	}

	public float lastGain
	{
		get
		{
			return _lastGain;
		}
		set
		{
			_lastGain = value;
		}
	}

	public int beganDelegatedCount
	{
		get
		{
			return _beganDelegatedCount;
		}
		set
		{
			_beganDelegatedCount = value;
		}
	}

	public TriggerEventDelegate BeganEventDelegate
	{
		get
		{
			return _BeganEventDelegate;
		}
		set
		{
			_BeganEventDelegate = value;
		}
	}

	public int endDelegatedCount
	{
		get
		{
			return _endDelegatedCount;
		}
		set
		{
			_endDelegatedCount = value;
		}
	}

	public TriggerEventDelegate EndEventDelegate
	{
		get
		{
			return _EndEventDelegate;
		}
		set
		{
			_EndEventDelegate = value;
		}
	}
}

using UnityEngine;

public static class KeyboardInputManager
{
	private static bool m_initialized;
	
	// Movement controls
	private static GETriggerC m_activeJoystick;
	private static GETriggerC m_activeDPad;
	private static GETriggerC m_jumpButton;
	private static GETriggerC m_sensorButton;
	
	// Tilt controller
	private static GETriggerC m_tiltController;
	private static float m_tiltSensitivity = 1f;

	public static void Initialize()
	{
		m_initialized = true;
	}

	public static void Update()
	{
		if (!m_initialized)
			return;

		HandleJoystickInput();
		HandleDPadInput();
		HandleJumpInput();
		HandleSensorInput();
		HandleTiltInput();
	}

	#region Movement Controls

	private static void HandleJoystickInput()
	{
		if (m_activeJoystick == null || m_activeJoystick.TC == null || !m_activeJoystick.TC.active)
		{
			m_activeJoystick = null;
			return;
		}

		Vector2 inputVector = Vector2.zero;

		// WASD or Arrow keys for movement
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			inputVector.y += 1f;
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			inputVector.y -= 1f;
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			inputVector.x -= 1f;
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			inputVector.x += 1f;

		// Normalize diagonal movement
		if (inputVector.sqrMagnitude > 1f)
			inputVector.Normalize();

		ApplyJoystickInput(inputVector);
	}

	private static void HandleDPadInput()
	{
		if (m_activeDPad == null || m_activeDPad.TC == null || !m_activeDPad.TC.active)
		{
			m_activeDPad = null;
			return;
		}

		Vector2 inputVector = Vector2.zero;
		TriggerData triggerData = m_activeDPad.data as TriggerData;

		// Up/Down input (Vertical or Full dpad)
		if (triggerData != null && triggerData.triggerType != 16) // Not horizontal-only
		{
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				inputVector.y = 1f;
			else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				inputVector.y = -1f;
		}

		// Left/Right input (Horizontal or Full dpad)
		if (triggerData != null && triggerData.triggerType != 28) // Not vertical-only
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				inputVector.x = -1f;
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				inputVector.x = 1f;
		}

		// Handle state changes
		bool isPressed = inputVector.sqrMagnitude > 0f;

		if (isPressed && !m_activeDPad.triggered)
		{
			m_activeDPad.triggered = true;
			m_activeDPad.began = true;
			m_activeDPad.beganTime = Main.m_gameTime;
		}
		else if (!isPressed && m_activeDPad.triggered)
		{
			m_activeDPad.triggered = false;
			m_activeDPad.end = true;
			m_activeDPad.endTime = Main.m_gameTime;
		}

		// Apply dpad output
		m_activeDPad.def.vector.x = inputVector.x;
		m_activeDPad.def.vector.y = inputVector.y;
		m_activeDPad.output.vector = inputVector;
		m_activeDPad.outputSlots[0].m_value.vector = inputVector;
		m_activeDPad.outputSlots[0].m_triggered = m_activeDPad.triggered;
		m_activeDPad.update = true;

		// Update visual position
		if (m_activeDPad.TC.childs.Count > 0)
		{
			Vector2 visualPos = inputVector * 100f;
			if (visualPos.sqrMagnitude > 10000f)
				visualPos = visualPos.normalized * 100f;
			TransformS.SetPosition(m_activeDPad.TC.childs[0], visualPos);
		}
	}

	private static void HandleJumpInput()
	{
		if (m_jumpButton == null || m_jumpButton.TC == null || !m_jumpButton.TC.active)
		{
			m_jumpButton = null;
			return;
		}

		bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

		if (jumpPressed)
		{
			m_jumpButton.triggered = true;
			m_jumpButton.began = true;
			m_jumpButton.beganTime = Main.m_gameTime;
			m_jumpButton.update = true;
		}

		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
		{
			m_jumpButton.triggered = false;
			m_jumpButton.end = true;
			m_jumpButton.endTime = Main.m_gameTime;
			m_jumpButton.update = true;
		}

		m_jumpButton.outputSlots[0].m_triggered = m_jumpButton.triggered;
	}

	private static void HandleSensorInput()
	{
		if (m_sensorButton == null || m_sensorButton.TC == null || !m_sensorButton.TC.active)
		{
			m_sensorButton = null;
			return;
		}

		bool sensorPressed = Input.GetKeyDown(KeyCode.E);

		if (sensorPressed)
		{
			m_sensorButton.triggered = true;
			m_sensorButton.began = true;
			m_sensorButton.beganTime = Main.m_gameTime;
			m_sensorButton.update = true;
		}

		if (Input.GetKeyUp(KeyCode.E))
		{
			m_sensorButton.triggered = false;
			m_sensorButton.end = true;
			m_sensorButton.endTime = Main.m_gameTime;
			m_sensorButton.update = true;
		}

		m_sensorButton.outputSlots[0].m_triggered = m_sensorButton.triggered;
	}

	private static void ApplyJoystickInput(Vector2 _inputVector)
	{
		if (m_activeJoystick == null || m_activeJoystick.TC == null || m_activeJoystick.TC.childs.Count == 0)
		{
			m_activeJoystick = null;
			return;
		}

		Vector3 worldPos = _inputVector * 100f;
		
		if (worldPos.sqrMagnitude > 10000f)
			worldPos = worldPos.normalized * 100f;

		// Update visual representation
		TransformS.SetPosition(m_activeJoystick.TC.childs[0], worldPos);

		// Update trigger values
		m_activeJoystick.triggered = _inputVector.sqrMagnitude > 0.1f;
		m_activeJoystick.output.vector.x = _inputVector.x;
		m_activeJoystick.output.vector.y = _inputVector.y;
		m_activeJoystick.outputSlots[0].m_value.vector = m_activeJoystick.output.vector;
		m_activeJoystick.outputSlots[0].m_triggered = m_activeJoystick.triggered;
		m_activeJoystick.update = true;
	}

	#endregion

	#region Tilt Controller (Keyboard Simulation)

	private static void HandleTiltInput()
	{
		if (m_tiltController == null || m_tiltController.TC == null || !m_tiltController.TC.active)
		{
			m_tiltController = null;
			return;
		}

		Vector2 tiltVector = Vector2.zero;

		// Use arrow keys to simulate device tilt
		if (Input.GetKey(KeyCode.LeftArrow))
			tiltVector.x -= m_tiltSensitivity;
		if (Input.GetKey(KeyCode.RightArrow))
			tiltVector.x += m_tiltSensitivity;
		if (Input.GetKey(KeyCode.UpArrow))
			tiltVector.y += m_tiltSensitivity;
		if (Input.GetKey(KeyCode.DownArrow))
			tiltVector.y -= m_tiltSensitivity;

		// Normalize
		if (tiltVector.sqrMagnitude > 1f)
			tiltVector.Normalize();

		// Apply to tilt controller
		m_tiltController.output.vector.x = tiltVector.x;
		m_tiltController.output.vector.y = tiltVector.y;
		m_tiltController.outputSlots[0].m_value.vector = m_tiltController.output.vector;
		m_tiltController.triggered = tiltVector.sqrMagnitude > 0.1f;
		m_tiltController.outputSlots[0].m_triggered = m_tiltController.triggered;
		m_tiltController.update = true;
	}

	#endregion

	#region Registration Methods

	public static void RegisterJoystick(GETriggerC _joystick)
	{
		m_activeJoystick = _joystick;
	}

	public static void UnregisterJoystick()
	{
		if (m_activeJoystick != null)
		{
			m_activeJoystick.triggered = false;
			m_activeJoystick.output.vector = Vector3.zero;
			m_activeJoystick.outputSlots[0].m_value.vector = Vector3.zero;
			m_activeJoystick = null;
		}
	}

	public static void RegisterDPad(GETriggerC _dpad)
	{
		m_activeDPad = _dpad;
	}

	public static void UnregisterDPad()
	{
		if (m_activeDPad != null)
		{
			m_activeDPad.triggered = false;
			m_activeDPad.def.vector = Vector3.zero;
			m_activeDPad.output.vector = Vector3.zero;
			m_activeDPad.outputSlots[0].m_value.vector = Vector3.zero;
			m_activeDPad = null;
		}
	}

	public static void RegisterJumpButton(GETriggerC _jumpButton)
	{
		m_jumpButton = _jumpButton;
	}

	public static void UnregisterJumpButton()
	{
		if (m_jumpButton != null)
		{
			m_jumpButton.triggered = false;
			m_jumpButton = null;
		}
	}

	public static void RegisterSensorButton(GETriggerC _sensorButton)
	{
		m_sensorButton = _sensorButton;
	}

	public static void UnregisterSensorButton()
	{
		if (m_sensorButton != null)
		{
			m_sensorButton.triggered = false;
			m_sensorButton = null;
		}
	}

	public static void RegisterTiltController(GETriggerC _tiltController)
	{
		m_tiltController = _tiltController;
	}

	public static void UnregisterTiltController()
	{
		if (m_tiltController != null)
		{
			m_tiltController.triggered = false;
			m_tiltController.output.vector = Vector3.zero;
			m_tiltController.outputSlots[0].m_value.vector = Vector3.zero;
			m_tiltController = null;
		}
	}

	public static void SetTiltSensitivity(float _sensitivity)
	{
		m_tiltSensitivity = Mathf.Clamp(_sensitivity, 0.1f, 2f);
	}

	public static bool IsJoystickActive()
	{
		return m_activeJoystick != null && m_activeJoystick.TC != null && m_activeJoystick.TC.active;
	}

	public static bool IsDPadActive()
	{
		return m_activeDPad != null && m_activeDPad.TC != null && m_activeDPad.TC.active;
	}

	public static bool IsJumpButtonActive()
	{
		return m_jumpButton != null && m_jumpButton.TC != null && m_jumpButton.TC.active;
	}

	public static bool IsSensorButtonActive()
	{
		return m_sensorButton != null && m_sensorButton.TC != null && m_sensorButton.TC.active;
	}

	public static bool IsTiltControllerActive()
	{
		return m_tiltController != null && m_tiltController.TC != null && m_tiltController.TC.active;
	}

	#endregion
}
using System;
using UnityEngine;

public static class CameraS
{
	private static Camera m_mainCamera;

	private static int m_cameraTargetCount = 20;

	private static int m_cameraBorderCount = 20;

	private static GenericArray<CameraTargetC> m_cameraTargetComponents;

	private static GenericArray<CameraBorderC> m_cameraBorderComponents;

	public static Vector3 m_currentCameraPositionOffset;

	public static Vector3 m_currentCameraRotationOffset;

	public static Vector3 m_currentCameraPosition;

	public static Vector3 m_currentCameraRotation;

	public static bool m_offsetLocked;

	public static float m_offsetFalloff;

	public static void Initialize(Camera _mainCamera)
	{
		m_mainCamera = _mainCamera;
		m_currentCameraPositionOffset = Vector3.zero;
		m_currentCameraRotationOffset = Vector3.zero;
		m_currentCameraRotation = Vector3.zero;
		m_currentCameraPosition = Vector3.zero;
		m_offsetLocked = false;
		m_offsetFalloff = 0.9f;
		m_cameraTargetComponents = new GenericArray<CameraTargetC>(m_cameraTargetCount);
		m_cameraBorderComponents = new GenericArray<CameraBorderC>(m_cameraBorderCount);
		for (int i = 0; i < m_cameraTargetCount; i++)
		{
			m_cameraTargetComponents.m_array[i] = new CameraTargetC();
			m_cameraTargetComponents.m_array[i].entityIndex = -1;
			m_cameraTargetComponents.m_array[i].index = i;
			m_cameraTargetComponents.m_array[i].componentType = ComponentType.CameraTarget;
		}
		for (int j = 0; j < m_cameraBorderCount; j++)
		{
			m_cameraBorderComponents.m_array[j] = new CameraBorderC();
			m_cameraBorderComponents.m_array[j].entityIndex = -1;
			m_cameraBorderComponents.m_array[j].index = j;
			m_cameraBorderComponents.m_array[j].componentType = ComponentType.CameraBorder;
		}
	}

	public static CameraTargetC AddTargetComponent(Camera _camera, TransformC _tc, Vector3 _offset, float _destinationSmooth, float _directionalSmooth, Vector3 _lowSpeed, Vector3 _highSpeed, float _lowSpeedDistance, float _highSpeedDistance, float _directionalOffset, float _maxDisplacement)
	{
		int num = m_cameraTargetComponents.AddItem();
		CameraTargetC cameraTargetC = m_cameraTargetComponents.m_array[num];
		cameraTargetC.entityIndex = _tc.entityIndex;
		cameraTargetC.active = true;
		cameraTargetC.offset = _offset;
		cameraTargetC.prevPos = _tc.transform.position;
		cameraTargetC.prevVel = Vector3.zero;
		cameraTargetC.TC = _tc;
		cameraTargetC.destinationSmooth = _destinationSmooth;
		cameraTargetC.velocityDirectionSmooth = _directionalSmooth;
		cameraTargetC.lowSpeed = _lowSpeed;
		cameraTargetC.highSpeed = _highSpeed;
		cameraTargetC.lowSpeedDistance = _lowSpeedDistance;
		cameraTargetC.highSpeedDistance = _highSpeedDistance;
		cameraTargetC.directionalOffset = _directionalOffset;
		cameraTargetC.maxDisplacement = _maxDisplacement;
		cameraTargetC.shake = Vector3.zero;
		cameraTargetC.camera = _camera;
		EntityManager.m_entities.m_array[cameraTargetC.entityIndex].components.Add(cameraTargetC);
		return cameraTargetC;
	}

	public static void RemoveTargetComponent(CameraTargetC _c)
	{
		_c.active = false;
		m_cameraTargetComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static CameraBorderC AddBorderComponent(Camera _camera, TransformC _tc, int _border)
	{
		int num = m_cameraBorderComponents.AddItem();
		CameraBorderC cameraBorderC = m_cameraBorderComponents.m_array[num];
		cameraBorderC.entityIndex = _tc.entityIndex;
		cameraBorderC.active = true;
		cameraBorderC.TC = _tc;
		cameraBorderC.camera = _camera;
		cameraBorderC.border = _border;
		EntityManager.m_entities.m_array[cameraBorderC.entityIndex].components.Add(cameraBorderC);
		return cameraBorderC;
	}

	public static void RemoveBorderComponent(CameraBorderC _c)
	{
		_c.active = false;
		m_cameraBorderComponents.RemoveItem(_c.index);
		EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
		_c.entityIndex = -1;
	}

	public static void ShakeCamera(CameraTargetC _cameraTargetC, float _duration, float _amount, float _interval, float _falloff)
	{
		_cameraTargetC.shakeDuration = _duration;
		_cameraTargetC.shakeAmount = _amount;
		_cameraTargetC.shakeBegin = Main.m_gameTime;
		_cameraTargetC.shakeInterval = _interval;
		_cameraTargetC.shakeFalloff = _falloff;
	}

	public static void Update()
	{
		int aliveCount = m_cameraTargetComponents.m_aliveCount;
		for (int i = 0; i < aliveCount; i++)
		{
			CameraTargetC cameraTargetC = m_cameraTargetComponents.m_array[m_cameraTargetComponents.m_aliveIndices[i]];
			if (cameraTargetC.active)
			{
				if (cameraTargetC.TC.parent != null && cameraTargetC.TC.transform.localPosition != Vector3.zero)
				{
					cameraTargetC.TC.transform.localPosition = Vector3.zero;
				}
				if (Main.m_gameTime > cameraTargetC.shakeBegin && Main.m_gameTime < cameraTargetC.shakeBegin + cameraTargetC.shakeDuration && Main.m_gameTime - cameraTargetC.lastShake > cameraTargetC.shakeInterval)
				{
					cameraTargetC.shake.x = UnityEngine.Random.Range(-1f, 1f);
					cameraTargetC.shake.y = UnityEngine.Random.Range(-1f, 1f);
					cameraTargetC.shake.z = UnityEngine.Random.Range(-1f, 1f);
					cameraTargetC.shake = cameraTargetC.shake.normalized * cameraTargetC.shakeAmount;
					cameraTargetC.lastShake = Main.m_gameTime;
					cameraTargetC.shakeAmount *= cameraTargetC.shakeFalloff;
				}
				else if (Main.m_gameTime > cameraTargetC.shakeBegin + cameraTargetC.shakeDuration && cameraTargetC.shake != Vector3.zero)
				{
					cameraTargetC.shake = Vector3.zero;
				}
				Vector3 vector = cameraTargetC.TC.transform.position + cameraTargetC.offset + cameraTargetC.shake;
				Vector3 vector2 = vector - m_currentCameraPosition;
				Vector3 vector3 = vector - cameraTargetC.prevPos;
				Vector3 vector4 = vector3 - cameraTargetC.prevVel;
				float num = cameraTargetC.prevVel.x + vector4.x * cameraTargetC.velocityDirectionSmooth;
				float num2 = cameraTargetC.prevVel.y + vector4.y * cameraTargetC.velocityDirectionSmooth;
				Vector3 vector5 = Vector3.forward * cameraTargetC.lowSpeedDistance;
				Vector3 normalized = new Vector3(num, num2, 0f).normalized;
				if (Mathf.Abs(num) > cameraTargetC.lowSpeed.x)
				{
					float b = (Mathf.Abs(num) - cameraTargetC.lowSpeed.x) / (cameraTargetC.highSpeed.x - cameraTargetC.lowSpeed.x);
					b = Mathf.Min(1f, Mathf.Max(0f, b));
					float num3 = TweenS.tween(TweenStyle.Linear, b, 1f, 0f, 1f);
					vector5.x += normalized.x * cameraTargetC.directionalOffset * num3;
					vector5.z += (cameraTargetC.highSpeedDistance - cameraTargetC.lowSpeedDistance) * num3;
				}
				if (Mathf.Abs(num2) > cameraTargetC.lowSpeed.y)
				{
					float b2 = (Mathf.Abs(num2) - cameraTargetC.lowSpeed.y) / (cameraTargetC.highSpeed.y - cameraTargetC.lowSpeed.y);
					b2 = Mathf.Min(1f, Mathf.Max(0f, b2));
					float num4 = TweenS.tween(TweenStyle.Linear, b2, 1f, 0f, 1f);
					vector5.y += normalized.y * cameraTargetC.directionalOffset * num4 * ((float)Screen.height / (float)Screen.width);
					vector5.z += (cameraTargetC.highSpeedDistance - cameraTargetC.lowSpeedDistance) * num4;
				}
				vector2 += vector5;
				m_currentCameraPosition += vector2 * cameraTargetC.destinationSmooth;
				vector2 = vector - m_currentCameraPosition;
				Vector3 vector6 = new Vector3(vector2.x, vector2.y, 0f);
				if (vector6.sqrMagnitude > cameraTargetC.maxDisplacement * cameraTargetC.maxDisplacement)
				{
					Vector3 vector7 = vector6 - vector6.normalized * cameraTargetC.maxDisplacement;
					m_currentCameraPosition += vector7 * 0.25f;
				}
				cameraTargetC.prevPos = cameraTargetC.TC.transform.position + cameraTargetC.offset + cameraTargetC.shake;
				cameraTargetC.prevVel.x = num;
				cameraTargetC.prevVel.y = num2;
			}
		}
		m_mainCamera.transform.position = m_currentCameraPosition + m_currentCameraPositionOffset;
		if (!m_offsetLocked && m_currentCameraPositionOffset != Vector3.zero)
		{
			m_currentCameraPositionOffset *= m_offsetFalloff;
			if (m_currentCameraPositionOffset.sqrMagnitude < 0.25f)
			{
				m_currentCameraPositionOffset = Vector3.zero;
			}
		}
		Vector3 position = m_mainCamera.transform.position;
		Vector3 position2 = position;
		float num5 = m_mainCamera.fov * ((float)Math.PI / 180f);
		float num6 = (float)Screen.width / (float)Screen.height;
		float num7 = (0f - Mathf.Tan(num5 * 0.5f)) * position.z;
		float num8 = num7 * num6;
		float num9 = position.x - num8;
		float num10 = position.x + num8;
		float num11 = position.y + num7;
		float num12 = position.y - num7;
		float num13 = -999999f;
		float num14 = 999999f;
		float num15 = 999999f;
		float num16 = -999999f;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		aliveCount = m_cameraBorderComponents.m_aliveCount;
		for (int j = 0; j < aliveCount; j++)
		{
			CameraBorderC cameraBorderC = m_cameraBorderComponents.m_array[m_cameraBorderComponents.m_aliveIndices[j]];
			if (!cameraBorderC.active)
			{
				continue;
			}
			Vector3 position3 = cameraBorderC.TC.transform.position;
			if (cameraBorderC.border == 0)
			{
				if (position3.x > num13)
				{
					num13 = position3.x;
				}
			}
			else if (cameraBorderC.border == 1)
			{
				if (position3.x < num14)
				{
					num14 = position3.x;
				}
			}
			else if (cameraBorderC.border == 2)
			{
				if (position3.y < num15)
				{
					num15 = position3.y;
				}
			}
			else if (cameraBorderC.border == 3 && position3.y > num16)
			{
				num16 = position3.y;
			}
		}
		float num17 = num14 - num13;
		float num18 = num15 - num16;
		float x = num13 + num17 * 0.5f;
		float y = num16 + num18 * 0.5f;
		if (num17 < num8 * 2f)
		{
			flag3 = true;
			flag = true;
		}
		if (num18 < num7 * 2f)
		{
			flag3 = true;
			flag2 = true;
		}
		if (flag3)
		{
			if (flag)
			{
				position2.x = x;
			}
			else if (num9 < num13)
			{
				position2.x += Mathf.Abs(num9 - num13);
			}
			else if (num10 > num14)
			{
				position2.x -= Math.Abs(num10 - num14);
			}
			if (flag2)
			{
				position2.y = y;
			}
			else if (num12 < num16)
			{
				position2.y += Mathf.Abs(num12 - num16);
			}
			else if (num11 > num15)
			{
				position2.y -= Math.Abs(num11 - num15);
			}
			float num19 = num17 / num18;
			if (num19 > num6)
			{
				position2.z = (0f - num18) * 0.5f / Mathf.Tan(num5 * 0.5f);
			}
			else
			{
				position2.z = (0f - num17) * 0.5f / Mathf.Tan(num5 * 0.5f);
			}
		}
		else
		{
			if (num9 < num13)
			{
				position2.x += Mathf.Abs(num9 - num13);
			}
			else if (num10 > num14)
			{
				position2.x -= Math.Abs(num10 - num14);
			}
			if (num12 < num16)
			{
				position2.y += Mathf.Abs(num12 - num16);
			}
			else if (num11 > num15)
			{
				position2.y -= Math.Abs(num11 - num15);
			}
		}
		m_mainCamera.transform.position = position2;
	}
}

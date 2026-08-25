using System;
using System.Collections.Generic;
using UnityEngine;

public static class TouchAreaS
{
	private static GenericArray<TouchAreaC> m_components;

	public static float m_touchAreaSizeMultipler = 1f;

	public static int[] m_consumedTouches = new int[10];

	private static bool[] m_clearConsumedTouches = new bool[10];

	private static List<TouchAreaC> m_removeList = new List<TouchAreaC>();

	private static bool m_sortOrder;

	public static bool m_abort;

	private static List<TouchAreaC> m_releaseList = new List<TouchAreaC>();

	private static List<int> removeList = new List<int>();

	private static bool m_cleanTouches = false;

	public static void Initialize(int _maxComponentCount)
	{
		m_components = new GenericArray<TouchAreaC>(_maxComponentCount);
		for (int i = 0; i < _maxComponentCount; i++)
		{
			m_components.m_array[i] = new TouchAreaC();
			ResetComponent(m_components.m_array[i]);
			m_components.m_array[i].index = i;
			m_components.m_array[i].componentType = ComponentType.TouchArea;
			m_components.m_array[i].touchEventDelegate = DelegatedDebugMethod;
		}
		for (int j = 0; j < m_consumedTouches.Length; j++)
		{
			m_consumedTouches[j] = -1;
			m_clearConsumedTouches[j] = false;
		}
	}

	public static void AddTouchEventListener(TouchAreaC _touchAreaComponent, TouchEventDelegate _touchEventHandler)
	{
		if (_touchAreaComponent.delegatedCount == 0)
		{
			_touchAreaComponent.touchEventDelegate = _touchEventHandler;
		}
		else
		{
			_touchAreaComponent.touchEventDelegate = (TouchEventDelegate)Delegate.Combine(_touchAreaComponent.touchEventDelegate, _touchEventHandler);
		}
		_touchAreaComponent.delegatedCount++;
	}

	public static void RemoveTouchEventListener(TouchAreaC _touchAreaComponent, TouchEventDelegate _touchEventHandler)
	{
		if (_touchAreaComponent.delegatedCount > 0)
		{
			_touchAreaComponent.touchEventDelegate = (TouchEventDelegate)Delegate.Remove(_touchAreaComponent.touchEventDelegate, _touchEventHandler);
			_touchAreaComponent.delegatedCount--;
		}
	}

	private static void DelegatedDebugMethod(TouchAreaC _c, int _i, bool _consumed)
	{
	}

	private static void ResetComponent(TouchAreaC _c)
	{
		_c.active = false;
		_c.entityIndex = -1;
		_c.TC = null;
		_c.offset = Vector3.zero;
		_c.radius = 0f;
		_c.width = 0f;
		_c.height = 0f;
		_c.delegatedCount = 0;
		_c.touchEventDelegate = DelegatedDebugMethod;
		_c.reservingFingerId = -1;
		_c.isReserved = false;
		_c.reservingStartedInside = false;
		_c.consumeTouches = false;
		_c.scaleByCameraDistance = false;
		_c.scaleByTransformComponent = false;
		_c.touchPos = new List<Vector2>();
		_c.touchStartPos = new List<Vector2>();
		_c.touchEvent = new List<TouchEvent>();
		_c.touchStartedInside = new List<bool>();
		_c.touchWasDragged = new List<bool>();
		_c.touchWasInside = new List<bool>();
		_c.touchFingerId = new List<int>();
		_c.touchIndex = new List<int>();
		_c.clip = false;
		_c.clipMaxX = 0f;
		_c.clipMinX = 0f;
		_c.clipMaxY = 0f;
		_c.clipMinY = 0f;
		_c.order = 0;
	}

	public static void SetClip(TouchAreaC _c, int _minX, int _maxX, int _minY, int _maxY)
	{
		_c.clipMinX = _minX;
		_c.clipMaxX = _maxX;
		_c.clipMinY = _minY;
		_c.clipMaxY = _maxY;
		_c.clip = true;
	}

	public static void SetOrder(TouchAreaC _c, int _order)
	{
		_c.order = _order;
		m_sortOrder = true;
	}

	public static TouchAreaC AddComponent(TransformC _transformComponent, string _customIdentifier, float _width, float _height, bool _consumeTouches, Camera _camera, IComponent _customComponent)
	{
		int num = m_components.AddItem();
		TouchAreaC touchAreaC = m_components.m_array[num];
		ResetComponent(touchAreaC);
		touchAreaC.entityIndex = _transformComponent.entityIndex;
		touchAreaC.active = true;
		touchAreaC.TC = _transformComponent;
		touchAreaC.camera = _camera;
		touchAreaC.width = _width;
		touchAreaC.height = _height;
		touchAreaC.customComponent = _customComponent;
		touchAreaC.identifier = _customIdentifier;
		touchAreaC.consumeTouches = _consumeTouches;
		EntityManager.m_entities.m_array[touchAreaC.entityIndex].components.Add(touchAreaC);
		return touchAreaC;
	}

	public static TouchAreaC AddComponent(TransformC _transformComponent, string _customIdentifier, float _radius, bool _consumeTouches, Camera _camera, IComponent _customComponent)
	{
		int num = m_components.AddItem();
		TouchAreaC touchAreaC = m_components.m_array[num];
		ResetComponent(touchAreaC);
		touchAreaC.active = true;
		touchAreaC.radius = _radius;
		touchAreaC.consumeTouches = _consumeTouches;
		touchAreaC.TC = _transformComponent;
		touchAreaC.entityIndex = _transformComponent.entityIndex;
		touchAreaC.camera = _camera;
		touchAreaC.customComponent = _customComponent;
		touchAreaC.identifier = _customIdentifier;
		EntityManager.m_entities.m_array[touchAreaC.entityIndex].components.Add(touchAreaC);
		return touchAreaC;
	}

	public static void RemoveComponent(TouchAreaC _c)
	{
		if (_c.entityIndex != -1)
		{
			_c.active = false;
			_c.TC = null;
			_c.touchStartPos = null;
			_c.touchPos = null;
			_c.touchEvent = null;
			_c.touchStartedInside = null;
			_c.touchWasDragged = null;
			_c.touchWasInside = null;
			_c.touchFingerId = null;
			_c.touchIndex = null;
			Delegate[] invocationList = _c.touchEventDelegate.GetInvocationList();
			Delegate[] array = invocationList;
			foreach (Delegate obj in array)
			{
				_c.touchEventDelegate = (TouchEventDelegate)Delegate.Remove(_c.touchEventDelegate, (TouchEventDelegate)obj);
			}
			_c.delegatedCount = 0;
			EntityManager.m_entities.m_array[_c.entityIndex].components.Remove(_c);
			m_components.RemoveItem(_c.index);
			_c.entityIndex = -1;
		}
	}

	public static void RemoveComponentsByTransformComponent(TransformC _tc)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int num = aliveCount - 1; num > -1; num--)
		{
			TouchAreaC touchAreaC = m_components.m_array[m_components.m_aliveIndices[num]];
			if (touchAreaC.TC == _tc)
			{
				m_removeList.Add(touchAreaC);
			}
		}
	}

	public static void SetNonRotatedOffset(TouchAreaC _c, Vector3 _offset)
	{
		_c.offset = _offset;
	}

	public static void ReleaseTouches(TouchAreaC _c)
	{
		m_releaseList.Add(_c);
	}

	public static void ReleaseTouch(TLTouch _t)
	{
		int aliveCount = m_components.m_aliveCount;
		for (int num = aliveCount - 1; num > -1; num--)
		{
			TouchAreaC touchAreaC = m_components.m_array[m_components.m_aliveIndices[num]];
			if (touchAreaC.active)
			{
				for (int i = 0; i < touchAreaC.touchFingerId.Count; i++)
				{
					if (touchAreaC.touchFingerId[i] == _t.fingerId)
					{
						touchAreaC.touchEvent[i] = TouchEvent.Cancel;
						touchAreaC.touchEventDelegate(touchAreaC, i, _t.consumed && _t.consumingTAC != touchAreaC);
						if (touchAreaC.reservingFingerId == _t.fingerId)
						{
							touchAreaC.isReserved = false;
							touchAreaC.reservingFingerId = -1;
						}
						if (Vector2.Distance(touchAreaC.touchStartPos[i], touchAreaC.touchPos[i]) > 10f)
						{
							touchAreaC.touchEvent[i] = TouchEvent.Drag;
							touchAreaC.touchEventDelegate(touchAreaC, i, false);
						}
						touchAreaC.touchPos.RemoveAt(i);
						touchAreaC.touchEvent.RemoveAt(i);
						touchAreaC.touchStartedInside.RemoveAt(i);
						touchAreaC.touchWasDragged.RemoveAt(i);
						touchAreaC.touchWasInside.RemoveAt(i);
						touchAreaC.touchFingerId.RemoveAt(i);
						touchAreaC.touchIndex.RemoveAt(i);
						touchAreaC.touchStartPos.RemoveAt(i);
						break;
					}
				}
			}
		}
		_t.consumed = false;
		_t.consumingTAC = null;
	}

	public static void ForceTouch(TouchAreaC _c, TLTouch _t, int _i, TouchEvent _e, bool _touchWasDragged)
	{
		_c.isReserved = true;
		_c.reservingFingerId = _t.fingerId;
		int count = _c.touchIndex.Count;
		_c.touchIndex.Add(_i);
		_c.touchFingerId.Add(_t.fingerId);
		_c.touchEvent.Add(_e);
		_c.touchStartedInside.Add(true);
		_c.touchWasDragged.Add(_touchWasDragged);
		_c.touchWasInside.Add(true);
		_c.touchPos.Add(_t.position);
		_c.touchStartPos.Add(_t.position);
		if (_c.consumeTouches)
		{
			_t.consumed = true;
			_t.consumingTAC = _c;
		}
	}

	public static Vector3 GetTouchWorldPos(Camera _camera, Vector2 _screenPos, float _zOffset)
	{
		Vector3 result = -_camera.ScreenToWorldPoint(new Vector3(_screenPos.x, _screenPos.y, _camera.transform.position.z + _zOffset));
		result += _camera.transform.position * 2f;
		result.z = 0f;
		return result;
	}

	public static Vector3 GetTouchWorldPos(Camera _camera, Vector2 _screenPos)
	{
		return GetTouchWorldPos(_camera, _screenPos, 0f);
	}

	public static bool IsTouchInside(Vector2 touchPos, Vector2 position, float radius)
	{
		Vector2 vector = touchPos - position;
		float num = vector.x * vector.x + vector.y * vector.y;
		if (num < radius * radius)
		{
			return true;
		}
		return false;
	}

	public static bool IsTouchInside(Vector2 touchPos, Vector2 position, Vector2 dimensions, float rotAngle)
	{
		float num = touchPos.x;
		float num2 = touchPos.y;
		if (rotAngle != 0f)
		{
			Vector2 vec = new Vector2(num, num2) - position;
			float num3 = ToolBox.getAngleFromVector2(vec) * 57.29578f;
			float num4 = rotAngle - num3;
			num = position.x + Mathf.Cos(num4 * ((float)Math.PI / 180f)) * vec.magnitude;
			num2 = position.y + Mathf.Sin(num4 * ((float)Math.PI / 180f)) * vec.magnitude;
		}
		position.x -= dimensions.x * 0.5f;
		position.y -= dimensions.y * 0.5f;
		if (num > position.x && num < position.x + dimensions.x && num2 > position.y && num2 < position.y + dimensions.y)
		{
			return true;
		}
		return false;
	}

	private static void Sort()
	{
		int num = m_components.m_aliveCount;
		List<int> list = new List<int>(m_components.m_aliveIndices);
		int[] array = new int[num];
		for (int i = 0; i < m_components.m_aliveCount; i++)
		{
			int num2 = 9999999;
			int index = -1;
			for (int j = 0; j < num; j++)
			{
				TouchAreaC touchAreaC = m_components.m_array[list[j]];
				if (touchAreaC.order < num2)
				{
					num2 = touchAreaC.order;
					index = j;
				}
			}
			array[i] = list[index];
			list.RemoveAt(index);
			num--;
		}
		array.CopyTo(m_components.m_aliveIndices, 0);
		m_sortOrder = false;
	}

	public static void Update()
	{
		m_abort = false;
		if (m_sortOrder)
		{
			Sort();
		}
		int touchAmount = InputManager.m_touchAmount;
		if (touchAmount > 0)
		{
			m_cleanTouches = true;
			int num = m_components.m_aliveCount - 1;
			for (int num2 = num; num2 > -1; num2--)
			{
				TouchAreaC touchAreaC = m_components.m_array[m_components.m_aliveIndices[num2]];
				if (touchAreaC.active)
				{
					TransformC tC = touchAreaC.TC;
					Vector3 vector = touchAreaC.camera.WorldToScreenPoint(tC.transform.position + touchAreaC.offset);
					float num3 = touchAreaC.radius * m_touchAreaSizeMultipler;
					float num4 = touchAreaC.width * m_touchAreaSizeMultipler;
					float num5 = touchAreaC.height * m_touchAreaSizeMultipler;
					if (touchAreaC.scaleByCameraDistance)
					{
						num3 *= Main.m_gameCameraDistanceMultipler;
						num4 *= Main.m_gameCameraDistanceMultipler;
						num5 *= Main.m_gameCameraDistanceMultipler;
					}
					if (touchAreaC.scaleByTransformComponent)
					{
						num3 *= (tC.transform.lossyScale.x + tC.transform.lossyScale.y) * 0.25f;
						num4 *= tC.transform.lossyScale.x;
						num5 *= tC.transform.lossyScale.y;
					}
					for (int i = 0; i < touchAmount; i++)
					{
						TLTouch tLTouch = InputManager.m_touches[i];
						if (num2 == num)
						{
							for (int j = 0; j < m_components.m_aliveCount; j++)
							{
								TouchAreaC touchAreaC2 = m_components.m_array[m_components.m_aliveIndices[j]];
								if (touchAreaC2.consumeTouches && touchAreaC2.reservingFingerId == tLTouch.fingerId)
								{
									tLTouch.consumed = true;
									tLTouch.consumingTAC = touchAreaC2;
									break;
								}
							}
						}
						int num6 = -1;
						for (int k = 0; k < touchAreaC.touchFingerId.Count; k++)
						{
							if (tLTouch.fingerId == touchAreaC.touchFingerId[k])
							{
								num6 = k;
								break;
							}
						}
						if (!touchAreaC.clip || (touchAreaC.clip && tLTouch.position.x > touchAreaC.clipMinX && tLTouch.position.x < touchAreaC.clipMaxX && tLTouch.position.y > touchAreaC.clipMinY && tLTouch.position.y < touchAreaC.clipMaxY))
						{
							bool flag = false;
							flag = ((!(touchAreaC.radius > 0f)) ? IsTouchInside(tLTouch.position, vector, new Vector2(num4, num5), tC.transform.rotation.eulerAngles.z) : IsTouchInside(tLTouch.position, vector, num3));
							bool consumed = tLTouch.consumed;
							if (touchAreaC == tLTouch.consumingTAC)
							{
								consumed = false;
							}
							if (flag && touchAreaC.isReserved && touchAreaC.consumeTouches && !tLTouch.consumed)
							{
								tLTouch.consumed = true;
								tLTouch.consumingTAC = touchAreaC;
								consumed = true;
							}
							if (touchAreaC.reservingFingerId == tLTouch.fingerId)
							{
								consumed = false;
							}
							if (tLTouch.phase == TouchPhase.Began)
							{
								if (flag)
								{
									if (!touchAreaC.isReserved && !tLTouch.consumed && touchAreaC.consumeTouches)
									{
										touchAreaC.isReserved = true;
										touchAreaC.reservingFingerId = tLTouch.fingerId;
										touchAreaC.reservingStartedInside = true;
									}
									int count = touchAreaC.touchIndex.Count;
									touchAreaC.touchIndex.Add(i);
									touchAreaC.touchFingerId.Add(tLTouch.fingerId);
									touchAreaC.touchEvent.Add(TouchEvent.Began);
									touchAreaC.touchStartedInside.Add(true);
									touchAreaC.touchWasDragged.Add(false);
									touchAreaC.touchWasInside.Add(true);
									touchAreaC.touchPos.Add(tLTouch.position);
									touchAreaC.touchStartPos.Add(tLTouch.position);
									touchAreaC.touchEventDelegate(touchAreaC, count, consumed);
									if (m_abort)
									{
										return;
									}
								}
							}
							else if (tLTouch.phase == TouchPhase.Moved || tLTouch.phase == TouchPhase.Stationary)
							{
								if (num6 != -1)
								{
									touchAreaC.touchIndex[num6] = i;
									touchAreaC.touchPos[num6] = tLTouch.position;
									if (flag)
									{
										if (touchAreaC.touchWasInside[num6])
										{
											if (tLTouch.deltaPosition.sqrMagnitude > 0f)
											{
												if (touchAreaC.touchWasDragged[num6])
												{
													touchAreaC.touchEvent[num6] = TouchEvent.Drag;
												}
												else
												{
													touchAreaC.touchEvent[num6] = TouchEvent.DragStart;
												}
												touchAreaC.touchWasDragged[num6] = true;
											}
											else
											{
												touchAreaC.touchEvent[num6] = TouchEvent.Down;
											}
										}
										else
										{
											touchAreaC.touchEvent[num6] = TouchEvent.RollIn;
											touchAreaC.touchWasDragged[num6] = true;
											touchAreaC.touchWasInside[num6] = true;
										}
									}
									else
									{
										if (touchAreaC.touchWasInside[num6])
										{
											if (!touchAreaC.touchStartedInside[num6])
											{
												touchAreaC.touchEvent[num6] = TouchEvent.Slice;
												touchAreaC.touchEventDelegate(touchAreaC, num6, consumed);
												if (m_abort)
												{
													return;
												}
											}
											touchAreaC.touchEvent[num6] = TouchEvent.RollOut;
											touchAreaC.touchWasDragged[num6] = true;
											if (touchAreaC.isReserved && !touchAreaC.touchStartedInside[num6] && touchAreaC.reservingFingerId == touchAreaC.touchFingerId[num6])
											{
												touchAreaC.isReserved = false;
												touchAreaC.reservingFingerId = -1;
												touchAreaC.reservingStartedInside = false;
												removeList.Add(num6);
											}
										}
										else if (tLTouch.deltaPosition.sqrMagnitude > 0f)
										{
											if (touchAreaC.touchWasDragged[num6])
											{
												touchAreaC.touchEvent[num6] = TouchEvent.Drag;
											}
											else
											{
												touchAreaC.touchEvent[num6] = TouchEvent.Down;
											}
										}
										touchAreaC.touchWasInside[num6] = false;
									}
									touchAreaC.touchEventDelegate(touchAreaC, num6, consumed);
									if (m_abort)
									{
										return;
									}
								}
								else if (flag)
								{
									if (!touchAreaC.isReserved && !tLTouch.consumed)
									{
										touchAreaC.isReserved = true;
										touchAreaC.reservingFingerId = tLTouch.fingerId;
										touchAreaC.reservingStartedInside = false;
									}
									int count2 = touchAreaC.touchIndex.Count;
									touchAreaC.touchIndex.Add(i);
									touchAreaC.touchFingerId.Add(tLTouch.fingerId);
									touchAreaC.touchEvent.Add(TouchEvent.RollIn);
									touchAreaC.touchStartedInside.Add(false);
									touchAreaC.touchWasDragged.Add(true);
									touchAreaC.touchWasInside.Add(true);
									touchAreaC.touchPos.Add(tLTouch.position);
									touchAreaC.touchStartPos.Add(tLTouch.position);
									touchAreaC.touchEventDelegate(touchAreaC, count2, consumed);
									if (m_abort)
									{
										return;
									}
								}
							}
							else if ((tLTouch.phase == TouchPhase.Ended || tLTouch.phase == TouchPhase.Canceled) && num6 != -1)
							{
								touchAreaC.touchIndex[num6] = i;
								if (flag)
								{
									touchAreaC.touchEvent[num6] = TouchEvent.Release;
								}
								else
								{
									if (touchAreaC.touchWasInside[num6])
									{
										touchAreaC.touchEvent[num6] = TouchEvent.RollOut;
										touchAreaC.touchWasDragged[num6] = true;
										touchAreaC.touchEventDelegate(touchAreaC, num6, consumed);
										if (m_abort)
										{
											return;
										}
										touchAreaC.touchWasInside[num6] = false;
									}
									touchAreaC.touchEvent[num6] = TouchEvent.ReleaseOutside;
								}
								touchAreaC.touchEventDelegate(touchAreaC, num6, consumed);
								if (m_abort)
								{
									return;
								}
								removeList.Add(num6);
							}
							while (removeList.Count > 0)
							{
								int index = removeList.Count - 1;
								int index2 = removeList[index];
								if (!touchAreaC.isReserved || touchAreaC.reservingFingerId == touchAreaC.touchFingerId[index2])
								{
								}
								touchAreaC.touchPos.RemoveAt(index2);
								touchAreaC.touchEvent.RemoveAt(index2);
								touchAreaC.touchStartedInside.RemoveAt(index2);
								touchAreaC.touchWasDragged.RemoveAt(index2);
								touchAreaC.touchWasInside.RemoveAt(index2);
								touchAreaC.touchFingerId.RemoveAt(index2);
								touchAreaC.touchIndex.RemoveAt(index2);
								touchAreaC.touchStartPos.RemoveAt(index2);
								removeList.RemoveAt(index);
							}
							if (flag && !tLTouch.consumed && touchAreaC.consumeTouches)
							{
								tLTouch.consumed = true;
								tLTouch.consumingTAC = touchAreaC;
							}
						}
						else if (num6 != -1 && touchAreaC.touchEvent[num6] != TouchEvent.RollOutOfClipArea)
						{
							touchAreaC.touchEvent[num6] = TouchEvent.RollOutOfClipArea;
							touchAreaC.touchPos[num6] = tLTouch.position;
							touchAreaC.touchEventDelegate(touchAreaC, num6, tLTouch.consumed && tLTouch.consumingTAC != touchAreaC);
							if (m_abort)
							{
								return;
							}
						}
					}
				}
			}
		}
		else if (m_cleanTouches)
		{
			int aliveCount = m_components.m_aliveCount;
			for (int num7 = aliveCount - 1; num7 > -1; num7--)
			{
				TouchAreaC touchAreaC3 = m_components.m_array[m_components.m_aliveIndices[num7]];
				while (touchAreaC3.touchIndex.Count > 0)
				{
					int index3 = touchAreaC3.touchIndex.Count - 1;
					touchAreaC3.touchPos.RemoveAt(index3);
					touchAreaC3.touchEvent.RemoveAt(index3);
					touchAreaC3.touchStartedInside.RemoveAt(index3);
					touchAreaC3.touchWasDragged.RemoveAt(index3);
					touchAreaC3.touchWasInside.RemoveAt(index3);
					touchAreaC3.touchFingerId.RemoveAt(index3);
					touchAreaC3.touchIndex.RemoveAt(index3);
					touchAreaC3.touchStartPos.RemoveAt(index3);
				}
				touchAreaC3.isReserved = false;
				touchAreaC3.reservingFingerId = -1;
				touchAreaC3.reservingStartedInside = false;
			}
			m_cleanTouches = false;
		}
		while (m_releaseList.Count > 0)
		{
			int index4 = m_releaseList.Count - 1;
			TouchAreaC touchAreaC4 = m_releaseList[index4];
			while (touchAreaC4.touchFingerId.Count > 0)
			{
				int num8 = touchAreaC4.touchIndex.Count - 1;
				InputManager.m_touches[touchAreaC4.touchIndex[num8]].consumed = false;
				InputManager.m_touches[touchAreaC4.touchIndex[num8]].consumingTAC = null;
				touchAreaC4.touchEvent[num8] = TouchEvent.Cancel;
				touchAreaC4.touchEventDelegate(touchAreaC4, num8, false);
				touchAreaC4.touchPos.RemoveAt(num8);
				touchAreaC4.touchEvent.RemoveAt(num8);
				touchAreaC4.touchStartedInside.RemoveAt(num8);
				touchAreaC4.touchWasDragged.RemoveAt(num8);
				touchAreaC4.touchWasInside.RemoveAt(num8);
				touchAreaC4.touchFingerId.RemoveAt(num8);
				touchAreaC4.touchIndex.RemoveAt(num8);
				touchAreaC4.touchStartPos.RemoveAt(num8);
			}
			touchAreaC4.isReserved = false;
			touchAreaC4.reservingFingerId = -1;
			touchAreaC4.reservingStartedInside = false;
			m_releaseList.RemoveAt(index4);
		}
		while (m_removeList.Count > 0)
		{
			int index5 = m_removeList.Count - 1;
			RemoveComponent(m_removeList[index5]);
			m_removeList.RemoveAt(index5);
		}
	}
}

using System;
using UnityEngine;

public static class GEConstraintLogic
{
	public static void RemoveChipmunkConstraints(GEConstraintC _c)
	{
		if (_c.connectJointPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.connectJointPtr);
		}
		if (_c.railJointPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.railJointPtr);
		}
		if (_c.rotaryLimitJointPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.rotaryLimitJointPtr);
		}
		if (_c.rotaryMotorPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.rotaryMotorPtr);
		}
		if (_c.rotarySpringPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.rotarySpringPtr);
		}
		if (_c.rotaryStiffnessPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.rotaryStiffnessPtr);
		}
		if (_c.slideJointPtr != IntPtr.Zero)
		{
			ChipmunkWrapper.RemoveConstraint(_c.slideJointPtr);
		}
		_c.connectJointPtr = IntPtr.Zero;
		_c.railJointPtr = IntPtr.Zero;
		_c.rotaryLimitJointPtr = IntPtr.Zero;
		_c.rotaryMotorPtr = IntPtr.Zero;
		_c.rotarySpringPtr = IntPtr.Zero;
		_c.rotaryStiffnessPtr = IntPtr.Zero;
		_c.slideJointPtr = IntPtr.Zero;
	}

	private static TweenStyle GetEasing(int _easeExit, int _easeEntry)
	{
		TweenStyle result = TweenStyle.Linear;
		if (_easeExit == 0 && _easeEntry == 0)
		{
			result = TweenStyle.Linear;
		}
		else if (_easeExit == 1 && _easeEntry == 1)
		{
			result = TweenStyle.QuadInOut;
		}
		else if (_easeExit == 1 && _easeEntry == 0)
		{
			result = TweenStyle.QuadIn;
		}
		else if (_easeExit == 0 && _easeEntry == 1)
		{
			result = TweenStyle.QuadOut;
		}
		else
		{
			switch (_easeEntry)
			{
			case 2:
				result = TweenStyle.ElasticOut;
				break;
			case 3:
				result = TweenStyle.BounceOut;
				break;
			}
		}
		return result;
	}

	public static void Update(GEConstraintC _c)
	{
		if (_c.constraintType == ConstraintType.Rope)
		{
			if (_c.connectedBodies == null)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < _c.connectedBodies.Length; i++)
			{
				Vector3 vector = _c.connectedBodyLocalAnchors[i];
				if (_c.connectedBodies[i] != null)
				{
					vector = ChipmunkWrapper.GetWorldPos(_c.connectedBodies[i].cpBodyPtr, vector);
				}
				vector.z = -7f;
				if (num != -1)
				{
					int num2 = 6;
					_c.lineRenderer.SetVertexCount((_c.ropeCMCs.Length - 1) * num2 - (_c.ropeCMCs.Length - 2));
					int num3 = 0;
					Vector2 zero = Vector2.zero;
					Vector2 zero2 = Vector2.zero;
					Vector2 zero3 = Vector2.zero;
					Vector2 zero4 = Vector2.zero;
					Vector2 vector2 = _c.ropeCMCs[_c.ropeCMCs.Length - 1].TC.transform.position - _c.ropeCMCs[0].TC.transform.position;
					Vector2 vector3 = vector2 / (_c.ropeCMCs.Length - 1);
					float num4 = _c.ropeLength * _c.ropeLength;
					float num5 = _c.ropeLength * 0.9f * (_c.ropeLength * 0.9f);
					float num6 = 0f;
					if (vector2.sqrMagnitude > num5)
					{
						num6 = Mathf.Min(1f, (vector2.sqrMagnitude - num5) / (num4 - num5));
					}
					if (_c.ropeCutTime > -1f)
					{
						float num7 = Main.m_gameTime - _c.ropeCutTime;
						if (num7 < 1f)
						{
							float a = 1f - num7 / 1f;
							_c.lineRenderer.SetColors(new Color(1f, 1f, 1f, a), new Color(1f, 1f, 1f, a));
						}
						else
						{
							_c.active = false;
						}
					}
					if (_c.isFlexible && !_c.flexDisabled && vector2.sqrMagnitude < num4)
					{
						float num8 = 3f;
						_c.lineRenderer.SetWidth(num8, num8);
						ChipmunkWrapper.SetDampedSpringProperties(_c.connectJointPtr, 0f, 0f, _c.ropeLength);
						_c.flexDisabled = true;
					}
					else if (_c.isFlexible && vector2.sqrMagnitude > num4)
					{
						float num9 = vector2.sqrMagnitude - num4;
						num9 = 1f - num9 / (num4 * 10f);
						float num10 = Mathf.Max(1f, num9 * 3f);
						_c.lineRenderer.SetWidth(num10, num10);
						if (_c.flexDisabled)
						{
							ChipmunkWrapper.SetDampedSpringProperties(_c.connectJointPtr, _c.flexForce, _c.flexDamp, _c.ropeLength);
							_c.flexDisabled = false;
						}
					}
					for (int j = 0; j < _c.ropeCMCs.Length - 1; j++)
					{
						Vector2 vector4 = (Vector2)_c.ropeCMCs[0].TC.transform.position + vector3 * j;
						Vector2 vector5 = vector4 - vector3;
						Vector2 vector6 = vector4 + vector3;
						Vector2 vector7 = vector6 + vector3;
						if (j == 0)
						{
							zero2 = _c.ropeCMCs[j].TC.transform.position;
							zero3 = _c.ropeCMCs[j + 1].TC.transform.position;
							zero4 = ((j + 2 < _c.ropeCMCs.Length) ? ((Vector2)_c.ropeCMCs[j + 2].TC.transform.position) : (zero3 + (zero3 - zero2)));
							zero = zero2 - (zero3 - zero2);
						}
						else if (j == _c.ropeCMCs.Length - 2)
						{
							zero = _c.ropeCMCs[j - 1].TC.transform.position;
							zero2 = _c.ropeCMCs[j].TC.transform.position;
							zero3 = _c.ropeCMCs[j + 1].TC.transform.position;
							zero4 = zero3 + (zero3 - zero2);
						}
						else
						{
							zero = _c.ropeCMCs[j - 1].TC.transform.position;
							zero2 = _c.ropeCMCs[j].TC.transform.position;
							zero3 = _c.ropeCMCs[j + 1].TC.transform.position;
							zero4 = ((j + 2 < _c.ropeCMCs.Length) ? ((Vector2)_c.ropeCMCs[j + 2].TC.transform.position) : (zero3 + (zero3 - zero2)));
						}
						if (num6 > 0f)
						{
							zero -= (zero - vector5) * num6;
							zero2 -= (zero2 - vector4) * num6;
							zero3 -= (zero3 - vector6) * num6;
							zero4 -= (zero4 - vector7) * num6;
						}
						for (int k = 0; k < num2; k++)
						{
							float t = (float)k / (float)(num2 - 1);
							Vector3 position = ToolBox.PointOnSplineSegment(zero, zero2, zero3, zero4, t);
							position.z = -6.5f;
							if (k > 0 || j == 0)
							{
								_c.lineRenderer.SetPosition(num3, position);
								num3++;
							}
						}
					}
				}
				num *= -1;
			}
		}
		else if (_c.constraintType == ConstraintType.RotaryMotor)
		{
			if (GEState.editorMode || !_c.update)
			{
				return;
			}
			bool flag = false;
			for (int l = 0; l < _c.inputSlots[1].m_connections.Count; l++)
			{
				IControlledComponent controller = _c.inputSlots[1].m_connections[l].controller;
				if (!controller.began && !controller.end)
				{
					continue;
				}
				_c.rotaryMotorRate = _c.inputSlots[1].m_value.vector.x;
				if (_c.rotaryMotorPtr != IntPtr.Zero)
				{
					if (_c.rotaryMotorEnabled)
					{
						ChipmunkWrapper.SetMotorProperties(_c.rotaryMotorPtr, _c.rotaryMotorRate * ((float)Math.PI / 180f), _c.rotaryMotorMaxForce * 90000000f);
					}
					flag = true;
				}
			}
			for (int m = 0; m < _c.inputSlots[0].m_connections.Count; m++)
			{
				IControlledComponent controller2 = _c.inputSlots[0].m_connections[m].controller;
				if (controller2.began)
				{
					_c.collidingCount++;
					if (_c.collidingCount == 1)
					{
						_c.triggered = true;
						_c.rotaryMotorEnabled = true;
						if (_c.rotaryMotorPtr != IntPtr.Zero)
						{
							ChipmunkWrapper.SetMotorProperties(_c.rotaryMotorPtr, _c.rotaryMotorRate * ((float)Math.PI / 180f), _c.rotaryMotorMaxForce * 90000000f);
						}
						flag = true;
					}
				}
				else
				{
					if (!controller2.end)
					{
						continue;
					}
					_c.collidingCount--;
					if (_c.collidingCount != 0)
					{
						continue;
					}
					_c.triggered = false;
					_c.rotaryMotorEnabled = false;
					if (_c.rotaryMotorPtr != IntPtr.Zero)
					{
						if (_c.motorIsStiff)
						{
							ChipmunkWrapper.SetMotorProperties(_c.rotaryMotorPtr, 0f, _c.rotaryMotorMaxForce * 90000000f);
						}
						else
						{
							ChipmunkWrapper.SetMotorProperties(_c.rotaryMotorPtr, 0f, 0f);
						}
					}
					flag = true;
				}
			}
			if (flag)
			{
				_c.update = false;
				_c.began = false;
				_c.end = false;
			}
		}
		else
		{
			if (_c.constraintType != ConstraintType.RailMotor)
			{
				return;
			}
			if (_c.update)
			{
				bool flag2 = false;
				for (int n = 0; n < _c.inputSlots[1].m_connections.Count; n++)
				{
					IControlledComponent controller3 = _c.inputSlots[1].m_connections[n].controller;
					if (controller3.began || controller3.end)
					{
						_c.linearMotorRate = _c.inputSlots[1].m_value.vector.x;
						flag2 = true;
						break;
					}
				}
				for (int num11 = 0; num11 < _c.inputSlots[0].m_connections.Count; num11++)
				{
					IControlledComponent controller4 = _c.inputSlots[0].m_connections[num11].controller;
					if (controller4.began)
					{
						_c.collidingCount++;
						if (_c.collidingCount == 1)
						{
							_c.triggered = true;
							_c.linearMotorEnabled = true;
							flag2 = true;
						}
					}
					else if (controller4.end)
					{
						_c.collidingCount--;
						if (_c.collidingCount == 0)
						{
							_c.triggered = false;
							_c.linearMotorEnabled = false;
							flag2 = true;
						}
					}
				}
				if (flag2)
				{
					_c.update = false;
					_c.began = false;
					_c.end = false;
				}
			}
			if (GEState.editorMode || _c.rail == null || _c.rail.anchorPoints.Length <= 1)
			{
				return;
			}
			Vector2 vector8 = _c.rail.anchorPoints[_c.currentIndex].position;
			int num12 = _c.currentIndex + 1;
			if (num12 == _c.rail.anchorPoints.Length)
			{
				num12 = 0;
			}
			Vector2 vector9 = (Vector2)_c.rail.anchorPoints[num12].position - vector8;
			Vector3 vector10 = Vector3.zero;
			Vector2 vector11 = vector9;
			if (_c.rail.railInterpolationStyle == 1)
			{
				vector10.x = TweenS.tween(_c.railTweenStyle, _c.currentRailPos, 1f, vector8.x, vector9.x);
				vector10.y = TweenS.tween(_c.railTweenStyle, _c.currentRailPos, 1f, vector8.y, vector9.y);
			}
			else if (_c.rail.railInterpolationStyle == 0)
			{
				int currentIndex = _c.currentIndex;
				Vector3 position2;
				Vector3 position3;
				Vector3 vector12;
				Vector3 vector13;
				if (currentIndex == 0)
				{
					position2 = _c.rail.anchorPoints[currentIndex].position;
					position3 = _c.rail.anchorPoints[currentIndex + 1].position;
					vector12 = ((currentIndex + 2 < _c.rail.anchorPoints.Length) ? _c.rail.anchorPoints[currentIndex + 2].position : (position3 + (position3 - position2)));
					vector13 = ((!_c.rail.railClosed) ? (position2 - (position3 - position2)) : _c.rail.anchorPoints[_c.rail.anchorPoints.Length - 1].position);
				}
				else if (currentIndex == _c.rail.anchorPoints.Length - 1)
				{
					vector13 = _c.rail.anchorPoints[currentIndex - 1].position;
					position2 = _c.rail.anchorPoints[currentIndex].position;
					position3 = _c.rail.anchorPoints[0].position;
					vector12 = _c.rail.anchorPoints[1].position;
				}
				else
				{
					vector13 = _c.rail.anchorPoints[currentIndex - 1].position;
					position2 = _c.rail.anchorPoints[currentIndex].position;
					position3 = _c.rail.anchorPoints[currentIndex + 1].position;
					vector12 = ((currentIndex + 2 < _c.rail.anchorPoints.Length) ? _c.rail.anchorPoints[currentIndex + 2].position : ((!_c.rail.railClosed) ? (position3 + (position3 - position2)) : _c.rail.anchorPoints[0].position));
				}
				vector10 = ToolBox.PointOnSplineSegment(vector13, position2, position3, vector12, _c.currentRailPos);
			}
			if (_c.railedPivotJointPtr != IntPtr.Zero)
			{
				ChipmunkWrapper.SetPivotJointOffsetA(_c.railedPivotJointPtr, vector10 + _c.pivotOffset);
			}
			if (_c.railedSlideJointAPtr != IntPtr.Zero)
			{
				ChipmunkWrapper.SetSlideJointOffsetA(_c.railedSlideJointAPtr, vector10);
				if (_c.railedDampedSpringAPtr != IntPtr.Zero)
				{
					ChipmunkWrapper.SetDampedSpringOffsetA(_c.railedDampedSpringAPtr, vector10);
				}
				TransformS.SetPosition(_c.railedSlideJointATC, vector10);
			}
			else if (_c.railedSlideJointBPtr != IntPtr.Zero)
			{
				ChipmunkWrapper.SetSlideJointOffsetB(_c.railedSlideJointBPtr, vector10);
				if (_c.railedDampedSpringBPtr != IntPtr.Zero)
				{
					ChipmunkWrapper.SetDampedSpringOffsetB(_c.railedDampedSpringBPtr, vector10);
				}
				TransformS.SetPosition(_c.railedSlideJointBTC, vector10);
			}
			if (!(_c.moveFromPoint < Main.m_gameTime))
			{
				return;
			}
			if (_c.linearMotorEnabled)
			{
				_c.currentRailPos += _c.linearMotorRate / _c.rail.anchorPoints[_c.currentIndex].length * (float)_c.linearMotorDirection;
			}
			if (_c.currentRailPos >= 1f)
			{
				_c.moveFromPoint = Main.m_gameTime + _c.rail.anchorPoints[_c.currentIndex].waitAtPoint;
				_c.currentRailPos -= 1f;
				_c.currentIndex++;
				if (_c.currentIndex < _c.rail.anchorPoints.Length)
				{
					_c.moveFromPoint = Main.m_gameTime + _c.rail.anchorPoints[_c.currentIndex].waitAtPoint;
				}
				else
				{
					_c.moveFromPoint = Main.m_gameTime + _c.rail.anchorPoints[0].waitAtPoint;
				}
				if (_c.currentIndex != _c.rail.anchorPoints.Length && (_c.rail.railClosed || _c.currentIndex != _c.rail.anchorPoints.Length - 1))
				{
					return;
				}
				if (!_c.rail.railClosed)
				{
					_c.currentIndex--;
					_c.currentRailPos = 1f;
					if (_c.loopStyle > 0)
					{
						_c.linearMotorDirection *= -1;
					}
				}
				else
				{
					_c.currentIndex = 0;
				}
			}
			else
			{
				if (!(_c.currentRailPos <= 0f))
				{
					return;
				}
				_c.moveFromPoint = Main.m_gameTime + _c.rail.anchorPoints[_c.currentIndex].waitAtPoint;
				_c.currentRailPos += 1f;
				_c.currentIndex--;
				if (_c.currentIndex != -1)
				{
					return;
				}
				if (!_c.rail.railClosed)
				{
					_c.currentIndex++;
					_c.currentRailPos = 0f;
					if (_c.loopStyle > 0)
					{
						_c.linearMotorDirection *= -1;
					}
				}
				else
				{
					_c.currentIndex = _c.rail.anchorPoints.Length - 1;
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

public static class BlobLogic
{
	public static TransformC tempTC;

	public static void Initialize()
	{
		Entity entity = EntityManager.AddEntity();
		entity.persistent = true;
		tempTC = TransformS.AddComponent(entity);
		ChipmunkS.AddCollisionInterest(true, false, true, (ColliderType)20, (ColliderType)20, HandleBLOBtoBLOB);
	}

	private static void HandleBLOBtoBLOB(ChipmunkCollisionPair _collisionPair, ChipmunkCollisionList _collisionList)
	{
		if (GEState.editorMode)
		{
			return;
		}
		ChipmunkC chipmunkC = ChipmunkS.m_components.m_array[_collisionPair.componentIndexA];
		ChipmunkC chipmunkC2 = ChipmunkS.m_components.m_array[_collisionPair.componentIndexB];
		BlobC blobC = chipmunkC.customComponent as BlobC;
		BlobC blobC2 = chipmunkC2.customComponent as BlobC;
		float num = 0f;
		if (blobC.collidingUnits == null || blobC2.collidingUnits == null)
		{
			return;
		}
		switch (_collisionList)
		{
		case ChipmunkCollisionList.BEGIN:
		{
			bool flag = false;
			for (int k = 0; k < blobC.collidingUnits.Count; k++)
			{
				if (blobC.collidingUnits[k] == blobC2)
				{
					flag = true;
					List<int> collidingUnitTouchCounts3;
					List<int> list4 = (collidingUnitTouchCounts3 = blobC.collidingUnitTouchCounts);
					int index2;
					int index6 = (index2 = k);
					index2 = collidingUnitTouchCounts3[index2];
					list4[index6] = index2 + 1;
					break;
				}
			}
			if (!flag && blobC != blobC2 && !blobC.doNotMerge)
			{
				blobC.collidingUnits.Add(blobC2);
				blobC.collidingUnitTouchCounts.Add(1);
				blobC.collidingUnitFirstTouched.Add(Main.m_gameTime);
			}
			flag = false;
			for (int l = 0; l < blobC2.collidingUnits.Count; l++)
			{
				if (blobC2.collidingUnits[l] == blobC)
				{
					flag = true;
					List<int> collidingUnitTouchCounts4;
					List<int> list5 = (collidingUnitTouchCounts4 = blobC2.collidingUnitTouchCounts);
					int index2;
					int index7 = (index2 = l);
					index2 = collidingUnitTouchCounts4[index2];
					list5[index7] = index2 + 1;
					break;
				}
			}
			if (!flag && blobC != blobC2 && !blobC2.doNotMerge)
			{
				blobC2.collidingUnits.Add(blobC);
				blobC2.collidingUnitTouchCounts.Add(1);
				blobC2.collidingUnitFirstTouched.Add(Main.m_gameTime);
			}
			break;
		}
		case ChipmunkCollisionList.SEPARATE:
		{
			List<int> list = new List<int>();
			for (int i = 0; i < blobC.collidingUnits.Count; i++)
			{
				if (blobC.collidingUnits[i] == blobC2)
				{
					List<int> collidingUnitTouchCounts;
					List<int> list2 = (collidingUnitTouchCounts = blobC.collidingUnitTouchCounts);
					int index2;
					int index = (index2 = i);
					index2 = collidingUnitTouchCounts[index2];
					list2[index] = index2 - 1;
					if (blobC.collidingUnitTouchCounts[i] == 0)
					{
						list.Add(i);
					}
					break;
				}
			}
			while (list.Count > 0)
			{
				int index3 = list.Count - 1;
				blobC.collidingUnitTouchCounts.RemoveAt(list[index3]);
				blobC.collidingUnits.RemoveAt(list[index3]);
				blobC.collidingUnitFirstTouched.RemoveAt(list[index3]);
				list.RemoveAt(index3);
			}
			for (int j = 0; j < blobC2.collidingUnits.Count; j++)
			{
				if (blobC2.collidingUnits[j] == blobC)
				{
					List<int> collidingUnitTouchCounts2;
					List<int> list3 = (collidingUnitTouchCounts2 = blobC2.collidingUnitTouchCounts);
					int index2;
					int index4 = (index2 = j);
					index2 = collidingUnitTouchCounts2[index2];
					list3[index4] = index2 - 1;
					if (blobC2.collidingUnitTouchCounts[j] == 0)
					{
						list.Add(j);
					}
					break;
				}
			}
			while (list.Count > 0)
			{
				int index5 = list.Count - 1;
				blobC2.collidingUnitTouchCounts.RemoveAt(list[index5]);
				blobC2.collidingUnits.RemoveAt(list[index5]);
				blobC2.collidingUnitFirstTouched.RemoveAt(list[index5]);
				list.RemoveAt(index5);
			}
			break;
		}
		}
	}

	public static void SplitBlob(BlobC _blob, float _angle)
	{
		BGoalC goal = _blob.goal;
		if (goal != null)
		{
			goal.blob = null;
		}
		Vector3 position = _blob.TAC.TC.transform.position;
		Vector3 vector = _blob.feet[0].TC.transform.position - position;
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		float num2 = 360f / (float)_blob.feet.Count;
		int num3 = Mathf.FloorToInt(ToolBox.getCappedAngle(_angle - num) / num2);
		float num4 = (float)Math.PI * _blob.radius * _blob.radius * 0.5f;
		float num5 = Mathf.Pow(num4 / (float)Math.PI, 0.5f);
		float num6 = (float)Math.PI * 2f * num5;
		int num7 = Mathf.RoundToInt(num6 / _blob.segmentLength);
		float num8 = 360f / (float)num7;
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		int count = _blob.feet.Count;
		int num9 = Mathf.RoundToInt((float)count * 0.5f);
		int num10 = 0;
		for (int i = 0; i < count; i++)
		{
			if (i < num9)
			{
				int num11 = num3 + i + num10;
				if (num11 >= count)
				{
					num10 -= count;
					num11 -= count;
				}
				ChipmunkC chipmunkC = _blob.feet[num11];
				list.Add(chipmunkC.TC.transform.position - position);
			}
			else
			{
				int num12 = num3 + i + num10;
				if (num12 >= count)
				{
					num10 -= count;
					num12 -= count;
				}
				ChipmunkC chipmunkC2 = _blob.feet[num12];
				list2.Add(chipmunkC2.TC.transform.position - position);
			}
		}
		List<Vector3> list3 = new List<Vector3>();
		for (int j = 0; j < num7; j++)
		{
			int index = Mathf.FloorToInt((float)list.Count / (float)num7 * (float)j);
			list3.Add(list[index]);
		}
		List<Vector3> list4 = new List<Vector3>();
		for (int k = 0; k < num7; k++)
		{
			int index2 = Mathf.FloorToInt((float)list2.Count / (float)num7 * (float)k);
			list4.Add(list2[index2]);
		}
		float friction = _blob.friction;
		float elasticy = _blob.elasticy;
		float minElasticy = _blob.minElasticy;
		float shapeDamp = _blob.shapeDamp;
		float segmentLength = _blob.segmentLength;
		EntityManager.RemoveEntity(_blob.entityIndex, true);
		BlobC blobC = ProtoBlobA.Assemble(position, list3.ToArray(), num5, friction, elasticy, minElasticy, shapeDamp, segmentLength);
		BlobC blobC2 = ProtoBlobA.Assemble(position, list4.ToArray(), num5, friction, elasticy, minElasticy, shapeDamp, segmentLength);
		blobC.doNotMerge = true;
		blobC2.doNotMerge = true;
	}

	public static void MergeBlob(BlobC _blob1, BlobC _blob2)
	{
		if (_blob1.TAC == null || _blob2.TAC == null || GEState.editorMode)
		{
			return;
		}
		BGoalC goal = _blob1.goal;
		if (goal == null)
		{
			goal = _blob2.goal;
		}
		BlobType blobType = _blob1.blobType;
		BlobType blobType2 = _blob2.blobType;
		Vector3 position = _blob1.TAC.TC.transform.position;
		Vector3 position2 = _blob2.TAC.TC.transform.position;
		Vector3 vector = (position + position2) * 0.5f;
		Vector3 vector2 = position2 - position;
		float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
		if (blobType != blobType2 || _blob1.feet == null || _blob2.feet == null)
		{
			return;
		}
		Vector3 vector3 = _blob1.feet[0].TC.transform.position - position;
		Vector3 vector4 = _blob2.feet[0].TC.transform.position - position2;
		float num2 = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
		float num3 = Mathf.Atan2(vector4.y, vector4.x) * 57.29578f;
		float num4 = (float)Math.PI * 2f * _blob1.radius;
		float num5 = (float)Math.PI * 2f * _blob2.radius;
		int num6 = Mathf.RoundToInt(num4 / ((_blob1.segmentLength + _blob2.segmentLength) * 0.5f));
		int num7 = Mathf.RoundToInt(num5 / ((_blob1.segmentLength + _blob2.segmentLength) * 0.5f));
		float num8 = 360f / (float)num6;
		float num9 = 360f / (float)num7;
		int num10 = Mathf.FloorToInt(ToolBox.getCappedAngle(num - num2 + 180f) / num8);
		int num11 = Mathf.FloorToInt(ToolBox.getCappedAngle(num - num3) / num9);
		if ((_blob1.feet[num10].TC.transform.position - _blob2.feet[num11].TC.transform.position).sqrMagnitude > vector2.sqrMagnitude)
		{
			num10 = Mathf.FloorToInt(ToolBox.getCappedAngle(num - num2) / num8);
			num11 = Mathf.FloorToInt(ToolBox.getCappedAngle(num - num3 + 180f) / num9);
		}
		float num12 = (float)Math.PI * _blob1.radius * _blob1.radius;
		float num13 = (float)Math.PI * _blob2.radius * _blob2.radius;
		float num14 = num12 + num13;
		float num15 = Mathf.Sqrt(num14 / (float)Math.PI);
		float num16 = (float)Math.PI * 2f * num15;
		int num17 = Mathf.RoundToInt(num16 / ((_blob1.segmentLength + _blob2.segmentLength) * 0.5f));
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < _blob1.feet.Count + _blob2.feet.Count; i++)
		{
			if (i < _blob1.feet.Count)
			{
				int num18 = num10 + i;
				if (num18 >= _blob1.feet.Count)
				{
					num18 -= _blob1.feet.Count;
				}
				ChipmunkC chipmunkC = _blob1.feet[num18];
				list.Add(chipmunkC.TC.transform.position - vector);
			}
			else
			{
				int num19 = num11 + i - _blob1.feet.Count;
				if (num19 >= _blob2.feet.Count)
				{
					num19 -= _blob2.feet.Count;
				}
				ChipmunkC chipmunkC2 = _blob2.feet[num19];
				list.Add(chipmunkC2.TC.transform.position - vector);
			}
		}
		List<Vector3> list2 = new List<Vector3>();
		for (int j = 0; j < num17; j++)
		{
			int index = Mathf.FloorToInt((float)list.Count / (float)num17 * (float)j);
			list2.Add(list[index]);
		}
		EntityManager.RemoveEntity(_blob1.entityIndex, true);
		EntityManager.RemoveEntity(_blob2.entityIndex, true);
		float friction = (_blob1.friction + _blob2.friction) * 0.5f;
		float elasticy = (_blob1.elasticy + _blob2.elasticy) * 0.5f;
		float minElasticy = (_blob1.minElasticy + _blob2.minElasticy) * 0.5f;
		float shapeDamp = (_blob1.shapeDamp + _blob2.shapeDamp) * 0.5f;
		float segmentLength = (_blob1.segmentLength + _blob2.segmentLength) * 0.5f;
		BlobC blobC = ProtoBlobA.Assemble(vector, list2.ToArray(), num15, friction, elasticy, minElasticy, shapeDamp, segmentLength);
		if (goal != null)
		{
			goal.blob = blobC;
			blobC.goal = goal;
			for (int k = 0; k < blobC.feet.Count; k++)
			{
				ChipmunkWrapper.SetCustomBodyLinearDamp(blobC.feet[k].cpBodyPtr, Vector2.one * 0.5f);
			}
		}
	}

	public static void Update(BlobC c)
	{
		if (c.feet != null)
		{
			List<Vector2> list = new List<Vector2>();
			Vector3 vector = Vector2.zero;
			for (int i = 0; i < c.feet.Count; i++)
			{
				if (c.feet[i].TC != null)
				{
					vector += c.feet[i].TC.transform.position;
					list.Add(c.feet[i].TC.transform.position);
				}
			}
			vector /= (float)c.feet.Count;
			TransformS.SetPosition(c.TAC.TC, vector);
			Vector3 vector2 = c.feet[0].TC.transform.position - vector;
			float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			TransformS.SetRotation(c.TAC.TC, Vector3.forward * num);
			Vector2[] vectorArray = list.ToArray();
			Polygon polygon = DebugDraw.Vector2ArrayToPolygon(vectorArray);
			polygon = GpcS.ScalePolygon(polygon, (0f - c.segmentLength) * 0.5f - 0.5f);
			DebugDraw.DrawVectorArray(Main.camera, tempTC, polygon.Contour[0].Vertex);
		}
		if (c.doNotMerge && (c.TAC.TC.transform.position - c.bornPos).sqrMagnitude > 2500f)
		{
			c.doNotMerge = false;
		}
		for (int j = 0; j < c.collidingUnits.Count; j++)
		{
			if (c.collidingUnitFirstTouched[j] + 0.5f < Main.m_gameTime && !c.merged)
			{
				BlobS.m_mergeList.Add(c);
				c.willMergeWithIndex = j;
				c.merged = true;
				c.collidingUnits[c.willMergeWithIndex].merged = true;
			}
		}
	}
}

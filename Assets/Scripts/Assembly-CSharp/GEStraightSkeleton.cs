using System.Collections.Generic;
using UnityEngine;

public class GEStraightSkeleton
{
	public static SKBase GenerateStraightSkeleton(Polygon _polygon, ref List<SKArc> _arcs, float _amount, float _step)
	{
		SKBase sKBase = new SKBase(_amount, _step, Vector2.zero);
		int num = 0;
		int index = 0;
		int num2 = 0;
		int index2 = 0;
		for (int i = 0; i < _polygon.NofContours; i++)
		{
			VertexList vertexList = _polygon.Contour[i];
			if (vertexList.NofVertices > 2)
			{
				for (int j = 0; j < vertexList.NofVertices - 1; j++)
				{
					Vector2 vector = vertexList.Vertex[j];
					Vector2 vector2;
					Vector2 vector3;
					if (j == 0)
					{
						vector2 = vertexList.Vertex[vertexList.NofVertices - 1];
						vector3 = vertexList.Vertex[j + 1];
					}
					else if (j == vertexList.NofVertices - 1)
					{
						vector2 = vertexList.Vertex[j - 1];
						vector3 = vertexList.Vertex[0];
					}
					else
					{
						vector2 = vertexList.Vertex[j - 1];
						vector3 = vertexList.Vertex[j + 1];
					}
					Vector2 normalized = (vector2 - vector).normalized;
					Vector2 normalized2 = (vector - vector3).normalized;
					float f = Mathf.Atan2(0f - normalized.y, normalized.x);
					float x = Mathf.Sin(f);
					float y = Mathf.Cos(f);
					Vector2 vector4 = new Vector2(x, y);
					float f2 = Mathf.Atan2(0f - normalized2.y, normalized2.x);
					float x2 = Mathf.Sin(f2);
					float y2 = Mathf.Cos(f2);
					Vector2 vector5 = new Vector2(x2, y2);
					Vector2 normalized3 = ((vector4 + vector5) * 0.5f).normalized;
					if (_polygon.ContourIsHole[i])
					{
						normalized3 *= -1f;
					}
					SKNode a = new SKNode(sKBase, _polygon.Contour[i].Vertex[j]);
					SKNode b = new SKNode(sKBase, vector + normalized3);
					SKArc sKArc = new SKArc(sKBase, a, b);
					_arcs.Add(sKArc);
					if (j > 0)
					{
						SKWavefront sKWavefront = CreateSKWavefront(sKBase, sKBase.arcs[num - 1], sKArc);
						if (j > 1)
						{
							sKWavefront.prev = sKBase.wavefronts[num2 - 1];
							sKWavefront.prev.next = sKWavefront;
						}
						sKArc.w1 = sKWavefront;
						sKBase.arcs[num - 1].w2 = sKWavefront;
						num2++;
					}
					num++;
					if (j == vertexList.NofVertices - 2)
					{
						_arcs.Add(sKBase.arcs[index]);
						SKWavefront sKWavefront2 = CreateSKWavefront(sKBase, sKBase.arcs[num - 1], sKBase.arcs[index]);
						sKWavefront2.prev = sKBase.wavefronts[num2 - 1];
						sKWavefront2.prev.next = sKWavefront2;
						sKWavefront2.next = sKBase.wavefronts[index2];
						sKWavefront2.next.prev = sKWavefront2;
						sKBase.arcs[num - 1].w2 = sKWavefront2;
						sKBase.arcs[index].w1 = sKWavefront2;
						num2++;
					}
				}
			}
			index = num;
			index2 = num2;
		}
		return IterateSK(sKBase);
	}

	public static SKBase GenerateStraightSkeletonFromGroundSplits(List<List<Vector2>> _roadPoints, int[] _orderedSplitIndices, ref List<List<SKArc>> _arcs, float _amount, float _step, Vector2 _weight)
	{
		SKBase sKBase = new SKBase(_amount, _step, _weight);
		Vector2 vector = Vector2.zero;
		bool flag = true;
		bool flag2 = false;
		int num = 0;
		int index = 0;
		int num2 = 0;
		int index2 = 0;
		for (int i = 0; i < _orderedSplitIndices.Length; i++)
		{
			int index3 = _orderedSplitIndices[i];
			List<Vector2> list = _roadPoints[index3];
			_arcs.Add(new List<SKArc>());
			List<Vector2> list2 = null;
			List<Vector2> list3 = null;
			if (list[0] == list[list.Count - 1])
			{
				list2 = list;
			}
			else
			{
				for (int j = 0; j < _orderedSplitIndices.Length; j++)
				{
					List<Vector2> list4 = _roadPoints[_orderedSplitIndices[j]];
					if (list[0] == list4[list4.Count - 1])
					{
						list2 = list4;
						break;
					}
				}
			}
			if (flag)
			{
				vector = list[0];
			}
			list3 = ((i >= _orderedSplitIndices.Length - 1) ? _roadPoints[_orderedSplitIndices[0]] : _roadPoints[_orderedSplitIndices[i + 1]]);
			flag2 = ((list[list.Count - 1] == vector) ? true : false);
			for (int k = 0; k < list.Count - 1; k++)
			{
				Vector2 vector2 = list[k];
				Vector2 vector3;
				Vector2 vector4;
				if (k == 0)
				{
					if (list2 != null)
					{
						vector3 = list2[list2.Count - 2];
					}
					else
					{
						vector3 = list[0];
						Debug.Log("contour is not closed");
						Debug.Log(string.Concat(list[0], " ", list[list.Count - 1]));
					}
					vector4 = list[k + 1];
				}
				else if (k >= list.Count - 2)
				{
					vector3 = list[k - 1];
					vector4 = ((!flag2) ? list3[0] : vector);
				}
				else
				{
					vector3 = list[k - 1];
					vector4 = list[k + 1];
				}
				Vector2 normalized = (vector3 - vector2).normalized;
				Vector2 normalized2 = (vector2 - vector4).normalized;
				float f = Mathf.Atan2(0f - normalized.y, normalized.x);
				float x = Mathf.Sin(f);
				float y = Mathf.Cos(f);
				Vector2 vector5 = new Vector2(x, y);
				float f2 = Mathf.Atan2(0f - normalized2.y, normalized2.x);
				float x2 = Mathf.Sin(f2);
				float y2 = Mathf.Cos(f2);
				Vector2 vector6 = new Vector2(x2, y2);
				Vector2 normalized3 = ((vector5 + vector6) * 0.5f).normalized;
				SKNode a = new SKNode(sKBase, vector2);
				SKNode b = new SKNode(sKBase, vector2 + normalized3);
				SKArc sKArc = new SKArc(sKBase, a, b);
				_arcs[i].Add(sKArc);
				if (k == 0)
				{
					if (!flag)
					{
						if (num2 == 0)
						{
							Debug.Log(i);
							Debug.LogError("lolll");
						}
						_arcs[i - 1].Add(sKArc);
						SKWavefront sKWavefront = CreateSKWavefront(sKBase, sKBase.arcs[num - 1], sKArc);
						sKWavefront.prev = sKBase.wavefronts[num2 - 1];
						sKWavefront.prev.next = sKWavefront;
						sKArc.w1 = sKWavefront;
						sKBase.arcs[num - 1].w2 = sKWavefront;
						num2++;
					}
				}
				else if (k > 0)
				{
					SKWavefront sKWavefront2 = CreateSKWavefront(sKBase, sKBase.arcs[num - 1], sKArc);
					if (k > 1 || !flag)
					{
						sKWavefront2.prev = sKBase.wavefronts[num2 - 1];
						sKWavefront2.prev.next = sKWavefront2;
					}
					sKArc.w1 = sKWavefront2;
					sKBase.arcs[num - 1].w2 = sKWavefront2;
					num2++;
				}
				num++;
				if (k != list.Count - 2)
				{
					continue;
				}
				if (flag2)
				{
					_arcs[i].Add(sKBase.arcs[index]);
					SKWavefront sKWavefront3 = CreateSKWavefront(sKBase, sKBase.arcs[num - 1], sKBase.arcs[index]);
					sKWavefront3.prev = sKBase.wavefronts[num2 - 1];
					sKWavefront3.prev.next = sKWavefront3;
					sKWavefront3.next = sKBase.wavefronts[index2];
					sKWavefront3.next.prev = sKWavefront3;
					sKBase.arcs[num - 1].w2 = sKWavefront3;
					sKBase.arcs[index].w1 = sKWavefront3;
					num2++;
				}
				else if (list.Count == 2 && flag)
				{
					SKWavefront sKWavefront4 = CreateSKWavefront(sKBase, sKBase.arcs[num - 1], sKArc);
					if (!flag)
					{
						sKWavefront4.prev = sKBase.wavefronts[num2 - 1];
						sKWavefront4.prev.next = sKWavefront4;
					}
					sKArc.w1 = sKWavefront4;
					sKBase.arcs[num - 1].w2 = sKWavefront4;
					num2++;
				}
			}
			if (flag2)
			{
				flag = true;
				index2 = num2;
				index = num;
			}
			else
			{
				flag = false;
			}
		}
		return IterateSK(sKBase);
	}

	private static SKWavefront CreateSKWavefront(SKBase _sk, SKArc _a1, SKArc _a2)
	{
		SKWavefront result = new SKWavefront(_sk, _a1, _a2);
		SKPolygon sKPolygon = new SKPolygon(_sk);
		sKPolygon.nodes.Add(_a1.nA);
		sKPolygon.nodes.Add(_a1.nB);
		sKPolygon.nodes.Add(_a2.nB);
		sKPolygon.nodes.Add(_a2.nA);
		return result;
	}

	private static SKBase IterateSK(SKBase _sk)
	{
		EntityManager.RemoveEntitiesByTag("test");
		TransformC transformC = EntityManager.AddEntityWithTC("test");
		Vector2 _pos = Vector2.zero;
		List<int> list = new List<int>();
		int num = 0;
		while (_sk.depth < _sk.maxDepth && num < 100)
		{
			num++;
			_sk.depth += _sk.depthStep;
			for (int i = 0; i < _sk.wavefronts.Count; i++)
			{
				SKWavefront sKWavefront = _sk.wavefronts[i];
				if (sKWavefront.a1.active)
				{
					sKWavefront.a1.nB.pos += sKWavefront.a1.normal * _sk.depthStep;
				}
			}
			list.Clear();
			for (int j = 0; j < _sk.wavefronts.Count; j++)
			{
				SKWavefront sKWavefront2 = _sk.wavefronts[j];
				if (ToolBox.DoLinesIntersect(sKWavefront2.a1.nA.pos, sKWavefront2.a1.nB.pos, sKWavefront2.a2.nA.pos, sKWavefront2.a2.nB.pos, ref _pos))
				{
					sKWavefront2.a1.nB.pos = _pos;
					sKWavefront2.a2.nB.pos = _pos;
					sKWavefront2.a1.active = false;
					sKWavefront2.a2.active = false;
					int num2 = sKWavefront2.a1.multiplier + sKWavefront2.a2.multiplier;
					Vector2 vector = _pos - sKWavefront2.a1.nA.pos;
					Vector2 vector2 = _pos - sKWavefront2.a2.nA.pos;
					Vector2 normalized = ((vector.normalized * sKWavefront2.a1.multiplier + vector2.normalized * sKWavefront2.a2.multiplier) / num2).normalized;
					SKNode a = new SKNode(_sk, _pos);
					SKNode b = new SKNode(_sk, _pos + normalized);
					SKArc sKArc = new SKArc(_sk, a, b);
					sKWavefront2.a1.next = sKArc;
					sKWavefront2.a2.next = sKArc;
					sKWavefront2.a1.w2 = null;
					sKWavefront2.a2.w1 = null;
					sKWavefront2.prev.a2 = sKArc;
					sKWavefront2.next.a1 = sKArc;
					sKWavefront2.prev.next = sKWavefront2.next;
					sKWavefront2.next.prev = sKWavefront2.prev;
					sKArc.w1 = sKWavefront2.prev;
					sKArc.w2 = sKWavefront2.next;
					sKArc.multiplier = num2;
					list.Add(j);
				}
			}
			while (list.Count > 0)
			{
				int index = list.Count - 1;
				_sk.wavefronts.RemoveAt(list[index]);
				list.RemoveAt(index);
			}
			for (int k = 0; k < _sk.arcs.Count; k++)
			{
				SKArc sKArc2 = _sk.arcs[k];
				if (sKArc2.active)
				{
					continue;
				}
				int num3 = 1;
				for (int l = k + num3; l < _sk.arcs.Count; l++)
				{
					SKArc sKArc3 = _sk.arcs[l];
					if (sKArc3.active && sKArc3 != sKArc2.next && sKArc3 != sKArc2 && ToolBox.DoLinesIntersect(sKArc2.nA.pos, sKArc2.nB.pos, sKArc3.nA.pos, sKArc3.nB.pos, ref _pos) && sKArc2.nB.pos != sKArc3.nA.pos && sKArc2.nA.pos != sKArc3.nB.pos)
					{
						SKArc sKArc4 = sKArc2;
						while (sKArc4.next != null)
						{
							sKArc4 = sKArc4.next;
						}
						sKArc3.next = sKArc2.next;
						sKArc3.nB.pos = sKArc2.nB.pos;
						sKArc4.w2 = sKArc3.w2;
						sKArc4.w2.a1 = sKArc4;
						_sk.wavefronts.Remove(sKArc3.w1);
						sKArc3.w1 = null;
						sKArc3.w2 = null;
						sKArc3.active = false;
						int num4 = sKArc4.multiplier + sKArc3.multiplier;
						Vector2 normalized2 = ((sKArc4.normal * sKArc4.multiplier + sKArc3.normal * sKArc3.multiplier) * num4).normalized;
						float magnitude = (sKArc4.nB.pos - sKArc4.nA.pos).magnitude;
						sKArc4.nB.pos = sKArc4.nA.pos + normalized2 * magnitude;
						sKArc4.normal = normalized2;
						sKArc4.multiplier = num4;
					}
				}
			}
			for (int m = 0; m < _sk.wavefronts.Count; m++)
			{
				SKWavefront sKWavefront3 = _sk.wavefronts[m];
				for (int n = 0; n < _sk.arcs.Count; n++)
				{
					SKArc sKArc5 = _sk.arcs[n];
					if (sKArc5.active && sKArc5 != sKWavefront3.a1 && sKArc5 != sKWavefront3.a2 && ToolBox.DoLinesIntersect(sKArc5.nA.pos, sKArc5.nB.pos, sKWavefront3.a1.nB.pos, sKWavefront3.a2.nB.pos, ref _pos))
					{
						sKArc5.nB.pos = _pos;
						sKArc5.active = false;
						SKNode a2 = new SKNode(_sk, _pos);
						SKNode b2 = new SKNode(_sk, _pos);
						SKArc sKArc6 = new SKArc(_sk, a2, b2);
						sKArc6.active = false;
						SKWavefront sKWavefront4 = new SKWavefront(_sk, sKArc6, sKWavefront3.next.a1);
						sKWavefront4.prev = sKWavefront3;
						sKWavefront4.next = sKWavefront3.next;
						sKWavefront3.next = sKWavefront4;
						sKWavefront3.a2 = sKArc6;
					}
				}
			}
		}
		return _sk;
	}
}

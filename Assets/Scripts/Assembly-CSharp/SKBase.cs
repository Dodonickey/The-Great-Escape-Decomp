using System.Collections.Generic;
using UnityEngine;

public class SKBase
{
	public float maxDepth;

	public float depthStep;

	public float depth;

	public Vector2 weight;

	public List<SKArc> arcs;

	public List<SKNode> nodes;

	public List<SKWavefront> wavefronts;

	public List<SKPolygon> polygons;

	public SKBase(float _maxDepth, float _depthStep, Vector2 _weight)
	{
		maxDepth = _maxDepth;
		depthStep = Mathf.Max(_depthStep, 1f);
		arcs = new List<SKArc>();
		nodes = new List<SKNode>();
		wavefronts = new List<SKWavefront>();
		polygons = new List<SKPolygon>();
		weight = _weight;
	}
}

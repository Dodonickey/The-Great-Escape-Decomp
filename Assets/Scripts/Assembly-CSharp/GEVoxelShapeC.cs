using System.Collections.Generic;
using UnityEngine;
using VoxelTerrain;

public class GEVoxelShapeC : BasicComponent
{
	public ChipmunkC CMC;

	public TransformC TC;

	public GameObject GO;

	public float GOScale;

	public int width;

	public int height;

	public int depth;

	public byte[,,] map;

	public Color[,,] colors;

	public List<GEVCube> cubes;

	public List<GEVCube> reBuild;

	public List<GEVCube> reBuildCollider;

	public bool enableFreeSculpting;

	public int reBuildColliderCount;

	public int RES;

	public byte ISO;

	public GroundSettings groundSettings;

	public Vector2 linearDamp;

	public float angularDamp;

	public Vector2 gravity;

	public float area;

	public float mass;

	public bool isPhysical;

	public bool isStatic;

	public bool isBreakable;

	public bool isSculptable;

	public float breakingImpulse;
}

using System.Collections.Generic;
using UnityEngine;

public static class GEState
{
	public static TextC fpsText;

	public static TouchAreaC fullScreenTAC;

	public static TransformC connectionTC;

	public static TransformC drawTC;

	public static bool editorMode;

	public static SpriteSheet editorUISheet;

	public static SpriteSheet outlinerIconSheet;

	public static SpriteSheet constraintSheet;

	public static List<GEMat> blockMats = new List<GEMat>();

	public static List<GEMat> groundMats = new List<GEMat>();

	public static List<GEMat> backgroundMats = new List<GEMat>();

	public static List<GEMat> landscapeMats = new List<GEMat>();

	public static Vector3 editorCameraStartPosition = Vector3.forward * -800f;

	public static uint layer_solid = 17891329u;

	public static uint layer_liquid = 17891344u;

	public static uint layer_solidHazard = 17891584u;

	public static uint layer_liquidHazard = 17895424u;

	public static uint layer_back = 65536u;

	public static uint layer_middle = 1048576u;

	public static uint layer_front = 16777216u;

	public static uint layer_all = 17891328u;

	public static uint hit_level = 4369u;

	public static float defaultChipmunkDensity = 0.01f;

	public static GEPlugin[] plugins;

	public static bool generateShapes;

	public static float defaultBackgroundDepth = 100f;

	public static bool m_addDown;

	public static bool m_subDown;

	public static bool m_specialDown;
}

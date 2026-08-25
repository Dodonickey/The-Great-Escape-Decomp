using System;
using UnityEngine;

public class Display
{
	private static Vector3 VX = new Vector3(0.1f, 0f, 0f);

	private static Vector3 VY = new Vector3(0f, 0.1f, 0f);

	private static Vector3 VZ = new Vector3(0f, 0f, 0.1f);

	public static Color R = new Color(1f, 0.1f, 0.1f, 0.4f);

	public static Color G = new Color(0.1f, 1f, 0.1f, 0.4f);

	public static Color B = new Color(0.1f, 0.1f, 1f, 0.4f);

	public static void Sphere(Vector3 center, float rayon)
	{
		float num = (float)Math.PI / 16f;
		float num2 = (float)Math.PI * 2f + num;
		float f = 0f;
		rayon /= 2f;
		for (float num3 = num; num3 < num2; num3 += num)
		{
			float num4 = Mathf.Sin(f);
			float num5 = Mathf.Sin(num3);
			float num6 = Mathf.Cos(f);
			float num7 = Mathf.Cos(num3);
			Vector3 start = center + new Vector3(num4, num6, 0f) * rayon;
			Vector3 end = center + new Vector3(num5, num7, 0f) * rayon;
			Vector3 start2 = center + new Vector3(num4, 0f, num6) * rayon;
			Vector3 end2 = center + new Vector3(num5, 0f, num7) * rayon;
			Vector3 start3 = center + new Vector3(0f, num4, num6) * rayon;
			Vector3 end3 = center + new Vector3(0f, num5, num7) * rayon;
			UnityEngine.Debug.DrawLine(start, end, Color.green);
			UnityEngine.Debug.DrawLine(start2, end2, Color.green);
			UnityEngine.Debug.DrawLine(start3, end3, Color.green);
			f = num3;
		}
	}

	public static void Vertices(Vector3[] vertices)
	{
		for (int i = 1; i < vertices.Length; i++)
		{
			UnityEngine.Debug.DrawLine(vertices[i - 1], vertices[i], Color.green, 0f);
		}
	}

	public static void Point(Vector3 pos, Color color)
	{
		UnityEngine.Debug.DrawLine(pos - VX, pos + VX, color);
		UnityEngine.Debug.DrawLine(pos - VY, pos + VY, color);
		UnityEngine.Debug.DrawLine(pos - VZ, pos + VZ, color);
	}

	public static void DrawGrid(int width, int depth, int res, Matrix4x4 toWorld)
	{
		for (int i = 0; i < width; i += res)
		{
			Vector3 vector = new Vector3(i, 0f, 0f);
			Vector3 to = new Vector3(i, 0f, depth);
			Gizmos.DrawLine(vector, to);
		}
		for (float num = 0f; num < (float)depth; num += (float)res)
		{
			Vector3 vector2 = new Vector3(0f, 0f, num);
			Vector3 to2 = new Vector3(width, 0f, num);
			Gizmos.DrawLine(vector2, to2);
		}
	}

	public static void DebugDrawCube(Vector3 center, Vector3 size, Color c)
	{
		Bounds bounds = new Bounds(center, size);
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		Vector3 vector = new Vector3(min.x, min.y, max.z);
		Vector3 vector2 = new Vector3(max.x, min.y, max.z);
		Vector3 vector3 = new Vector3(max.x, min.y, min.z);
		Vector3 vector4 = new Vector3(min.x, max.y, max.z);
		Vector3 vector5 = new Vector3(max.x, max.y, min.z);
		Vector3 vector6 = new Vector3(min.x, max.y, min.z);
		UnityEngine.Debug.DrawLine(vector, vector2, c);
		UnityEngine.Debug.DrawLine(vector2, vector3, c);
		UnityEngine.Debug.DrawLine(vector3, min, c);
		UnityEngine.Debug.DrawLine(min, vector, c);
		UnityEngine.Debug.DrawLine(vector4, max, c);
		UnityEngine.Debug.DrawLine(max, vector5, c);
		UnityEngine.Debug.DrawLine(vector5, vector6, c);
		UnityEngine.Debug.DrawLine(vector6, vector4, c);
		UnityEngine.Debug.DrawLine(vector, vector4, c);
		UnityEngine.Debug.DrawLine(vector2, max, c);
		UnityEngine.Debug.DrawLine(vector3, vector5, c);
		UnityEngine.Debug.DrawLine(min, vector6, c);
	}

	public static void GizmosDrawCube(Vector3 center, Vector3 size, Matrix4x4 matrix)
	{
		Bounds bounds = new Bounds(center, size);
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		Vector3 vector = matrix.MultiplyPoint(new Vector3(min.x, min.y, max.z));
		Vector3 vector2 = matrix.MultiplyPoint(new Vector3(max.x, min.y, max.z));
		Vector3 vector3 = matrix.MultiplyPoint(new Vector3(max.x, min.y, min.z));
		Vector3 vector4 = matrix.MultiplyPoint(new Vector3(min.x, max.y, max.z));
		Vector3 vector5 = matrix.MultiplyPoint(new Vector3(max.x, max.y, min.z));
		Vector3 vector6 = matrix.MultiplyPoint(new Vector3(min.x, max.y, min.z));
		min = matrix.MultiplyPoint(min);
		max = matrix.MultiplyPoint(max);
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, min);
		Gizmos.DrawLine(min, vector);
		Gizmos.DrawLine(vector4, max);
		Gizmos.DrawLine(max, vector5);
		Gizmos.DrawLine(vector5, vector6);
		Gizmos.DrawLine(vector6, vector4);
		Gizmos.DrawLine(vector, vector4);
		Gizmos.DrawLine(vector2, max);
		Gizmos.DrawLine(vector3, vector5);
		Gizmos.DrawLine(min, vector6);
	}
}

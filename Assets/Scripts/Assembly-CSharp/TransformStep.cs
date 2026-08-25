using System.Collections.Generic;
using UnityEngine;

public class TransformStep : Step
{
	public List<EIC> eic;

	public List<Vector3> oldPos;

	public List<Vector3> oldScale;

	public List<Vector3> oldRot;

	public List<Vector3> pos;

	public List<Vector3> scale;

	public List<Vector3> rot;

	public TransformStep(List<EIC> _eic, TouchAreaC _c, List<Vector3> _scales)
		: base(UndoStepType.Transform)
	{
		eic = new List<EIC>();
		foreach (EIC item in _eic)
		{
			eic.Add(item);
		}
		pos = new List<Vector3>();
		rot = new List<Vector3>();
		scale = new List<Vector3>();
		oldPos = new List<Vector3>();
		oldRot = new List<Vector3>();
		oldScale = new List<Vector3>();
		GETransformGizmoC gETransformGizmoC = _c.customComponent as GETransformGizmoC;
		foreach (Vector3 item2 in gETransformGizmoC.originalPosition)
		{
			oldPos.Add(item2);
		}
		foreach (Vector3 item3 in gETransformGizmoC.originalRotation)
		{
			oldRot.Add(item3);
		}
		foreach (Vector3 item4 in gETransformGizmoC.originalScale)
		{
			oldScale.Add(item4);
		}
		for (int i = 0; i < eic.Count; i++)
		{
			pos.Add(_eic[i].data.position.ToVector3());
			rot.Add(_eic[i].data.rotation.ToVector3());
			scale.Add(_scales[i]);
		}
	}

	public override bool ChainDestroy()
	{
		eic = null;
		oldPos.Clear();
		oldScale.Clear();
		oldRot.Clear();
		pos.Clear();
		scale.Clear();
		rot.Clear();
		return base.ChainDestroy();
	}

	public override object Undo()
	{
		for (int i = 0; i < eic.Count; i++)
		{
			eic[i].data.scale = new Vertex3(oldScale[i]);
			eic[i].data.position = new Vertex3(oldPos[i]);
			eic[i].data.rotation = new Vertex3(oldRot[i]);
		}
		EditorState.m_selection.Clear();
		foreach (EIC item in eic)
		{
			EditorState.ResetEditorItem(item);
		}
		EditorState.UpdateSelection();
		return 0;
	}

	public override object Redo()
	{
		for (int i = 0; i < eic.Count; i++)
		{
			eic[i].data.scale = new Vertex3(scale[i]);
			eic[i].data.position = new Vertex3(pos[i]);
			eic[i].data.rotation = new Vertex3(rot[i]);
		}
		EditorState.m_selection.Clear();
		foreach (EIC item in eic)
		{
			EditorState.ResetEditorItem(item);
		}
		EditorState.UpdateSelection();
		return 0;
	}
}

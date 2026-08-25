using System.Collections.Generic;
using UnityEngine;

public class DestroyStep : Step
{
	public List<EIC> items;

	public List<string> identifier;

	public List<Vector3> pos;

	public List<Vector3> rot;

	public List<Vector3> scale;

	public DestroyStep(List<EIC> _eic)
		: base(UndoStepType.Destroy)
	{
		items = new List<EIC>();
		identifier = new List<string>();
		pos = new List<Vector3>();
		rot = new List<Vector3>();
		scale = new List<Vector3>();
		foreach (EIC item in _eic)
		{
			items.Add(item);
			identifier.Add(item.identifier);
			pos.Add(item.TC.transform.position);
			rot.Add(item.TC.transform.rotation.eulerAngles);
			scale.Add(item.TC.transform.localScale);
		}
	}

	public override bool ChainDestroy()
	{
		items.Clear();
		identifier.Clear();
		pos.Clear();
		rot.Clear();
		scale.Clear();
		return base.ChainDestroy();
	}

	public override object Redo()
	{
		foreach (EIC item in items)
		{
			EditorState.ClearEditorItem(item);
			EditorState.RemoveEditorItem(item);
		}
		EditorState.m_selection.Clear();
		EditorState.UpdateSelection();
		return 0;
	}

	public override object Undo()
	{
		for (int i = 0; i < items.Count; i++)
		{
			items[i] = EditorState.CreateNewEditorItem(null, identifier[i], pos[i], rot[i], scale[i], true)[0];
		}
		return items;
	}
}

using System.Collections.Generic;
using UnityEngine;

public class CreateNewStep : Step
{
	public List<EIC> items;

	public string identifier;

	public Vector3 pos;

	public Vector3 rot;

	public Vector3 scale;

	public CreateNewStep(List<EIC> _eic, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
		: base(UndoStepType.Create)
	{
		items = _eic;
		identifier = _identifier;
		pos = _pos;
		rot = _rot;
		scale = _sca;
	}

	public override bool ChainDestroy()
	{
		items.Clear();
		identifier = string.Empty;
		pos = Vector3.zero;
		rot = Vector3.zero;
		scale = Vector3.zero;
		return base.ChainDestroy();
	}

	public override object Undo()
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

	public override object Redo()
	{
		items = EditorState.CreateNewEditorItem(null, identifier, pos, rot, scale, true);
		return items;
	}
}

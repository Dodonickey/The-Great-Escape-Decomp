public class Step
{
	public int idx;

	public UndoStepType type;

	public Step previous;

	public Step next;

	public Step(UndoStepType _type)
	{
		type = _type;
	}

	public virtual bool ChainDestroy()
	{
		bool flag = next == null;
		if (!flag)
		{
			next.ChainDestroy();
		}
		next = null;
		previous = null;
		idx = 0;
		return flag;
	}

	public virtual object Undo()
	{
		return 0;
	}

	public virtual object Redo()
	{
		return true;
	}
}

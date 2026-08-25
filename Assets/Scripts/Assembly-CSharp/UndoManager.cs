public class UndoManager
{
	private static Step first;

	private static Step last;

	public static Step current;

	private static int steps;

	public static void Initialize()
	{
		first = null;
		last = null;
		current = null;
		steps = 0;
	}

	public static void AddStep(Step _step)
	{
		if (current != null)
		{
			if (current.next != null)
			{
				current.next.ChainDestroy();
			}
			current.next = _step;
			_step.previous = current;
		}
		steps++;
		current = _step;
		current.idx = steps;
		last = current;
		if (first == null)
		{
			first = current;
		}
	}

	public static void Undo()
	{
		if (current != null)
		{
			current.Undo();
			if (current.previous != null)
			{
				current = current.previous;
			}
			else
			{
				current = null;
			}
		}
	}

	public static void Redo()
	{
		if (current == null)
		{
			if (first != null)
			{
				current = first;
				current.Redo();
			}
		}
		else if (current.next != null)
		{
			current = current.next;
			current.Redo();
		}
	}
}

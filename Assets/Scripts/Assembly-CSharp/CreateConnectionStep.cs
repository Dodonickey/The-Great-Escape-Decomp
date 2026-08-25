public class CreateConnectionStep : Step
{
	private EIC eic;

	private ConnectionSlot start;

	private ConnectionSlot end;

	private int entityIndex;

	private BasicControlledComponent controller;

	private BasicControlledComponent controllee;

	private GEConnectionC ccom;

	public CreateConnectionStep(EIC _eic, GEConnectionC _ccom, int _entityIndex, ConnectionSlot _startSlot, ConnectionSlot _endSlot, BasicControlledComponent _controller, BasicControlledComponent _controllee)
		: base(UndoStepType.CreateConnection)
	{
		ccom = _ccom;
		eic = _eic;
		entityIndex = _entityIndex;
		start = _startSlot;
		end = _endSlot;
		controller = _controller;
		controllee = _controllee;
	}

	public override object Undo()
	{
		GES.RemoveConnectionComponent(ccom);
		return 0;
	}

	public override object Redo()
	{
		ccom = GES.AddConnectionComponent(entityIndex, start, end, controller, controllee);
		eic.gameComponents.Add(ccom);
		if (GEState.editorMode)
		{
			ccom.container = eic;
		}
		return "No Luke, I am your father.";
	}
}

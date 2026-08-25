using UnityEngine;

public class FFirstState : BasicState
{
	public static FGameScene p_parent;

	private string identifier;

	private bool exiting;

	public override void Enter(IStatedObject _parent)
	{
		p_parent = _parent as FGameScene;
		string[] tags = new string[1] { "MainMenu" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, -1, "TestCanvas", null, tags);
		UIS.SetCanvasRelativeSize(uIC, 1f, 1f, 0f, 0f);
		UIS.SetCanvasRelativeMarginAndSpacing(uIC, 0.02f, 0.01f);
		UIS.SetCanvasAlign(uIC, Align.Center, Align.Middle);
		UIC uIC2 = ButtonA.Assemble(Main.uiCamera, 0, "Editor", HandleUI, tags);
		UIS.AddToCanvasGrid(uIC2, uIC, false);
		UIS.SetRelativeSize(uIC2, 0.175f, 0.065f);
		UIC uIC3 = ButtonA.Assemble(Main.uiCamera, 0, "Farm", HandleUI, tags);
		UIS.AddToCanvasGrid(uIC3, uIC, false);
		UIS.SetRelativeSize(uIC3, 0.175f, 0.065f);
		UIS.PlaceCanvasContent(uIC);
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(uIC2.width, uIC2.height, 30f, 8, Vector2.zero, false);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
		uIC2.outlinePCs.AddRange(PrefabS.CreatePathPrefabComponentFromPolygon(uIC2.TC, Vector3.forward * 0f, polygon, 6f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true));
		uIC2.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC2.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		TextS.SetStyle("subheader");
		TextC textC = TextS.AddSingleLineComponent(uIC2.contentTC, uIC2.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		TransformS.Move(textC.contentTC, Vector3.right * (uIC2.width * -0.5f + 45f));
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC2.canvasCamera, true);
		roundedRect = DebugDraw.GetRoundedRect(uIC3.width, uIC3.height, 30f, 8, Vector2.zero, false);
		polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
		uIC3.outlinePCs.AddRange(PrefabS.CreatePathPrefabComponentFromPolygon(uIC3.TC, Vector3.forward * 0f, polygon, 6f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true));
		uIC3.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC3.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		TextS.SetStyle("subheader");
		textC = TextS.AddSingleLineComponent(uIC3.contentTC, uIC3.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		TransformS.Move(textC.contentTC, Vector3.right * (uIC3.width * -0.5f + 45f));
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, uIC3.canvasCamera, true);
		exiting = false;
	}

	public override void Execute()
	{
		if (exiting)
		{
			if (identifier == "Editor")
			{
				GEGame.m_sceneManager.ChangeScene(new EditorScene());
			}
			else if (identifier == "Farm")
			{
				p_parent.StateMachine.ChangeState(new FGameState());
			}
		}
	}

	public override void Exit()
	{
		EntityManager.RemoveEntitiesByTag("MainMenu");
	}

	protected virtual void HandleUI(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		if (_c.touchEvent[_i] == TouchEvent.Release)
		{
			if (_c.identifier == "Editor")
			{
				identifier = _c.identifier;
				exiting = true;
				Main.m_currentGame.GetCurrentScene().CreateLoadingScreen();
			}
			else if (_c.identifier == "Farm")
			{
				identifier = _c.identifier;
				exiting = true;
				Main.m_currentGame.GetCurrentScene().CreateLoadingScreen();
			}
		}
	}
}

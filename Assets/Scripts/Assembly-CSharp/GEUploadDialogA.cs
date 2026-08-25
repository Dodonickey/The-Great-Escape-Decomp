using System;
using UnityEngine;

public static class GEUploadDialogA
{
	private static string[] builtInLevelNames;

	public static UIC Assemble(UIC _parent)
	{
		string[] tags = new string[1] { "Upload" };
		UIC uIC = CanvasA.Assemble(Main.uiCamera, -1, "Upload...", null, tags);
		UIS.AddToCanvasGrid(uIC, _parent, false);
		UIS.SetCanvasRelativeSize(uIC, 0.725f, 0.75f, 0.065f, 0f);
		UIS.SetCanvasAbsoluteMarginAndSpacing(uIC, 4f, 10f);
		UIS.SetCanvasAlign(uIC, Align.Right, Align.Top);
		DrawCanvas(uIC, Main.uiCamera);
		UIC uIC2 = CanvasA.Assemble(Main.uiCamera, -1, string.Empty, CanvasA.HandleTouches, tags);
		UIS.AddToCanvasGrid(uIC2, uIC, true);
		UIS.SetCanvasRelativeSize(uIC2, 1f, 0.7f, 0f, 0f);
		UIS.SetCanvasAbsoluteMarginAndSpacing(uIC2, 0f, 0f);
		UIS.SetCanvasSeparateRenderSpace(uIC2);
		Vector2[] rect = DebugDraw.GetRect(uIC2.width, uIC2.height, Vector2.zero);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(rect);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC2.TC, Vector3.forward * -10f, polygon, DebugDraw.GetColor(250f, 250f, 250f), ResourceManager.GetMaterial("Solid"), Main.uiCamera);
		UnityEngine.Object[] array = Resources.LoadAll(Main.m_currentGame.m_projectCode + "/Levels");
		builtInLevelNames = new string[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			builtInLevelNames[i] = (array[i] as TextAsset).name;
			Resources.UnloadAsset(array[i]);
		}
		array = null;
		for (int j = 0; j < builtInLevelNames.Length; j++)
		{
			string label = builtInLevelNames[j] + " (build)";
			UIC uIC3 = ButtonA.Assemble(uIC2.canvasCamera, j, label, HandleFileList, tags);
			UIS.AddToCanvasGrid(uIC3, uIC2, true);
			UIS.SetAbsoluteSize(uIC3, uIC2.width, uIC.canvasHeight * 0.1f);
			DrawListButton(uIC3, uIC2.canvasCamera);
		}
		string[] customLevelFolderContents = IO.GetCustomLevelFolderContents();
		string[] separator = new string[3] { "/", "\\", "." };
		for (int k = 0; k < customLevelFolderContents.Length; k++)
		{
			string[] array2 = customLevelFolderContents[k].Split(separator, StringSplitOptions.None);
			if (array2[array2.Length - 1] != "meta")
			{
				string label2 = array2[array2.Length - 2];
				UIC uIC4 = ButtonA.Assemble(uIC2.canvasCamera, k, label2, HandleFileList, tags);
				UIS.AddToCanvasGrid(uIC4, uIC2, true);
				UIS.SetAbsoluteSize(uIC4, uIC2.width, uIC.canvasHeight * 0.1f);
				DrawListButton(uIC4, uIC2.canvasCamera);
			}
		}
		UIS.PlaceCanvasContent(uIC2);
		UIC uIC5 = TextFieldA.Assemble(Main.uiCamera, 100001, "File", null, TextFieldA.HandleTouches, tags);
		UIS.AddToCanvasGrid(uIC5, uIC, true);
		UIS.SetRelativeSize(uIC5, 1f, 0.1f);
		TextFieldA.DrawTextField(uIC5, LevelManager.m_currentLevel.name);
		UIC uIC6 = ButtonA.Assemble(Main.uiCamera, 0, "Upload", HandleUploadWindow, tags);
		UIC uIC7 = ButtonA.Assemble(Main.uiCamera, 0, "Cancel", HandleUploadWindow, tags);
		UIS.AddToCanvasGrid(uIC6, uIC, true);
		UIS.AddToCanvasGrid(uIC7, uIC, false);
		UIS.SetRelativeSize(uIC6, 0.3f, 0.08f);
		UIS.SetRelativeSize(uIC7, 0.3f, 0.08f);
		DrawButton(uIC6);
		DrawButton(uIC7);
		UIS.PlaceCanvasContent(uIC);
		return uIC;
	}

	private static void DrawListButton(UIC _button, Camera _camera)
	{
		Vector2[] line = DebugDraw.GetLine(Vector2.right * _button.parent.width * -0.5f, Vector2.right * _button.parent.width * 0.5f, 0);
		TextS.SetStyle("subheader");
		TextC textC = TextS.AddSingleLineComponent(_button.contentTC, _button.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		TransformS.Move(textC.contentTC, Vector3.right * (_button.width * -0.5f + 45f) + Vector3.forward * -20f);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, _button.parent.canvasCamera, true);
		PrefabS.CreateLinePrefabComponentFromVectorArray(_button.TC, Vector3.up * _button.height * -0.5f, line, 1f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Solid"), _camera, Position.Center);
	}

	private static void DrawButton(UIC _button)
	{
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(_button.width, _button.height, 25f, 8, Vector2.zero, false);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
		_button.outlinePCs.AddRange(PrefabS.CreatePathPrefabComponentFromPolygon(_button.TC, Vector3.forward * 0f, polygon, 6f, DebugDraw.GetColor(200f, 200f, 200f), ResourceManager.GetMaterial("Line6"), Main.uiCamera, Position.Center, true));
		_button.backgroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(_button.TC, Vector3.forward * 10f, polygon, DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), Main.uiCamera));
		TextS.SetStyle("subheader");
		TextC textC = TextS.AddSingleLineComponent(_button.contentTC, _button.label, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(textC.contentTC, _button.parent.canvasCamera, true);
	}

	public static void DrawCanvas(UIC _uic, Camera _camera)
	{
		float width = _uic.width;
		float height = _uic.height;
		float headerHeight = _uic.headerHeight;
		float footerHeight = _uic.footerHeight;
		Camera camera = _camera;
		if (_uic.parent != null && _uic.parent.separateRenderSpace)
		{
			camera = _uic.parent.canvasCamera;
		}
		Vector2[] roundedRect = DebugDraw.GetRoundedRect(width, height, 8f, 8, Vector2.zero, false);
		Polygon polygon = DebugDraw.Vector2ArrayToPolygon(roundedRect);
		PrefabS.CreatePathPrefabComponentFromPolygon(_uic.TC, Vector3.forward * -5f, polygon, 6f, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
		PrefabS.CreateFlatPrefabComponentsFromPolygon(_uic.TC, Vector3.forward * 0f, polygon, DebugDraw.GetColor(220f, 220f, 220f), ResourceManager.GetMaterial("Solid"), camera);
		Vector2[] roundedRect2 = DebugDraw.GetRoundedRect(width - 8f, headerHeight - 8f, 5f, 8, Vector2.zero, false);
		PrefabS.CreatePathPrefabComponentFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -10f, roundedRect2, 6f, DebugDraw.GetColor(92f, 156f, 50f), ResourceManager.GetMaterial("Line4"), camera, Position.Center, true);
		PrefabS.CreateFlatPrefabComponentsFromVectorArray(_uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f) + Vector3.forward * -5f, roundedRect2, PrefabS.ColorToUInt(DebugDraw.GetColor(92f, 156f, 50f)), PrefabS.ColorToUInt(DebugDraw.GetColor(112f, 176f, 70f)), ResourceManager.GetMaterial("Solid"), camera, string.Empty);
		TransformC transformC = TransformS.AddComponent(_uic.TC.entityIndex);
		TransformS.ParentComponent(transformC, _uic.TC, Vector3.up * (height * 0.5f - headerHeight * 0.5f));
		TextS.SetStyle("header");
		_uic.textC = TextS.AddSingleLineComponent(transformC, _uic.label, 1f, Align.Left, Align.Middle);
		SpriteS.SetColorByTransformComponent(_uic.textC.contentTC, DebugDraw.GetColor(255f, 255f, 255f), false, false);
		TransformS.Move(_uic.textC.contentTC, Vector3.right * (width * -0.5f + headerHeight) + Vector3.forward * -10f);
		SpriteS.ConvertSpritesToPrefabComponent(_uic.textC.TC, camera, true);
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		if (_c.touchEvent[_i] == TouchEvent.Began)
		{
			if (_c.identifier == "background" && !_consumed)
			{
				EntityManager.RemoveEntitiesByTransformComponentHierarchy(uIC.TC, false);
			}
		}
		else if (_c.touchEvent[_i] == TouchEvent.Release && !_c.touchStartedInside[_i])
		{
		}
	}

	private static void HandleFileList(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		if (_c.touchEvent[_i] == TouchEvent.Began || _c.touchEvent[_i] == TouchEvent.DragStart || _c.touchEvent[_i] == TouchEvent.RollOut || _c.touchEvent[_i] != TouchEvent.Release || !_c.touchStartedInside[_i] || !((_c.touchPos[_i] - _c.touchStartPos[_i]).sqrMagnitude < 100f))
		{
			return;
		}
		UIC uIComponentByIdentifier = UIS.GetUIComponentByIdentifier(100001);
		if (uIComponentByIdentifier != null)
		{
			while (uIComponentByIdentifier.textPCs.Count > 0)
			{
				int index = uIComponentByIdentifier.textPCs.Count - 1;
				PrefabS.RemoveComponent(uIComponentByIdentifier.textPCs[index]);
				uIComponentByIdentifier.textPCs.RemoveAt(index);
			}
			TextS.SetStyle("subheader");
			TextS.ChangeText(uIComponentByIdentifier.textC, uIC.label);
			SpriteS.SetColorByTransformComponent(uIComponentByIdentifier.textC.contentTC, Color.black, false, false);
			uIComponentByIdentifier.textPCs.AddRange(SpriteS.ConvertSpritesToPrefabComponent(uIComponentByIdentifier.textC.contentTC, true));
		}
	}

	public static void HandleUploadWindow(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		if (_c.touchEvent[_i] != TouchEvent.Release || !_c.touchStartedInside[_i])
		{
			return;
		}
		if (_c.identifier == "Upload")
		{
			string[] customLevelFolderContents = IO.GetCustomLevelFolderContents();
			bool flag = false;
			string[] separator = new string[3] { "/", "\\", "." };
			for (int i = 0; i < customLevelFolderContents.Length; i++)
			{
				string[] array = customLevelFolderContents[i].Split(separator, StringSplitOptions.None);
				string text = array[array.Length - 2];
				if (text == UIS.GetUIComponentByIdentifier(100001).textC.text)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (!FTP.IsConnected())
				{
					FTP.Connect();
				}
				if (!FTP.FolderExists("/public_html/alteration/_L/" + Main.m_currentGame.m_projectCode + "/"))
				{
					FTP.CreateFolder("/public_html/alteration/_L/" + Main.m_currentGame.m_projectCode);
				}
				string text2 = UIS.GetUIComponentByIdentifier(100001).textC.text;
				string text3 = IO.GetResourceLevelPath() + "/" + text2 + ".bytes";
				FTP.Upload(text3, "public_html/alteration/_L/" + Main.m_currentGame.m_projectCode + "/" + text2 + ".bytes");
				GEMenuAreaA.CloseMenu();
				LevelManager.ChangeLevel(new GELevelResource(null, text2, text3, ResourceType.Level), true);
				EditorState.ResetOutliner();
			}
		}
		else if (_c.identifier == "Cancel")
		{
			GEMenuAreaA.CloseMenu();
		}
	}
}

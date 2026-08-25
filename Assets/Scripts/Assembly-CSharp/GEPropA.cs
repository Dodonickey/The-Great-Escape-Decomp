using System.Collections.Generic;
using UnityEngine;

public static class GEPropA
{
	public static List<IComponent> Assemble(Vector2 _pos, string _spritePrefabKey)
	{
		List<IComponent> list = new List<IComponent>();
		ColliderType colliderType = (ColliderType)8;
		string[] tags = new string[2]
		{
			LevelManager.m_currentLevel.name + ":Prop",
			LevelManager.m_currentLevel.name
		};
		TransformC transformC = EntityManager.AddEntityWithTC(tags);
		GESpritePrefabC gESpritePrefabC = SpritePrefabA.Assemble(_pos, _spritePrefabKey, tags, colliderType, 0u, 0u, 0f);
		TransformS.ParentComponent(gESpritePrefabC.rootNode.TC, transformC, gESpritePrefabC.rootNode.globalPosition + gESpritePrefabC.rootNode.localCenter);
		list.Add(transformC);
		list.Add(gESpritePrefabC);
		return list;
	}

	public static void PopulatePropertyBar(EIC _eiC, UIC _propertyBar)
	{
		string[] tags = new string[1] { "propertyBar" };
		PropData propData = _eiC.container.data as PropData;
		Camera canvasCamera = _propertyBar.canvasCamera;
		UIC component = NumericFieldA.Assemble(Main.uiCamera, "Color R", HandlePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, (propData.color >> 16) & 0xFF, tags);
		UIC component2 = NumericFieldA.Assemble(Main.uiCamera, "Color G", HandlePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, (propData.color >> 8) & 0xFF, tags);
		UIC component3 = NumericFieldA.Assemble(Main.uiCamera, "Color B", HandlePropertyChange, null, true, Align.Left, 50f, 1f, true, 0f, 255f, propData.color & 0xFF, tags);
		UIC component4 = NumericFieldA.Assemble(Main.uiCamera, "Z", HandlePropertyChange, null, true, Align.Left, 50f, 1f, false, -200f, 200f, _eiC.data.position.z, tags);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Color", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component, _propertyBar, true);
		UIS.AddToCanvasGrid(component2, _propertyBar, false);
		UIS.AddToCanvasGrid(component3, _propertyBar, false);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Location", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(RadioButtonA.Assemble(Main.uiCamera, "Landscape", HandlePropertyChange, null, true, Align.Right, 1f, propData.location == 0, 0, 102, tags), _propertyBar, true);
		UIS.AddToCanvasGrid(RadioButtonA.Assemble(Main.uiCamera, "Background", HandlePropertyChange, null, true, Align.Right, 1f, propData.location == 2, 2, 102, tags), _propertyBar, true);
		UIS.AddToCanvasGrid(RadioButtonA.Assemble(Main.uiCamera, "Road", HandlePropertyChange, null, true, Align.Right, 1f, propData.location == 4, 4, 102, tags), _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
		UIS.AddToCanvasGrid(LabelA.Assemble(canvasCamera, "Tweak", tags), _propertyBar, true);
		UIS.AddToCanvasGrid(component4, _propertyBar, true);
		UIS.MoveCursor(_propertyBar, 0f, -15f);
	}

	public static void HandlePropertyChange(EventC _c)
	{
	}
}

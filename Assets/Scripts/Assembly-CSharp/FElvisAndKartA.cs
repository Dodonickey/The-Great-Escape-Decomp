using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FElvisAndKartA
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024map8;

	public static List<IComponent> Assemble(EIC _eic)
	{
		List<IComponent> list = new List<IComponent>();
		GECharacterC gECharacterC = FElvisA.Assemble(_eic, 2000u, 0f - _eic.data.position.z + 50f);
		GEVehicleC gEVehicleC = FBigKartA.Assemble(_eic, 2000u, 0f - _eic.data.position.z);
		GECharacterLogic.JumpToCart(gECharacterC, gEVehicleC, Vector3.zero, 0f);
		GEVehicleLogic.SetTireBrakes(gEVehicleC, false);
		list.Add(gECharacterC);
		list.Add(gEVehicleC);
		return list;
	}

	public static EIC CreateNewEditorItem(EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		BasicLevelData basicLevelData = new BasicLevelData();
		basicLevelData.position = new Vertex3(_pos);
		basicLevelData.position.z = 37.5f;
		basicLevelData.rotation = new Vertex3(_rot);
		basicLevelData.scale = new Vertex3(_sca);
		uint uniqueId = GES.GetUniqueId();
		basicLevelData.Init(uniqueId, _identifier + uniqueId);
		EIC eIC = GEItemA.Assemble(_container, _identifier, basicLevelData, Main.camera);
		eIC.isRealtimeMovable = false;
		eIC.isDrawable = false;
		eIC.isRotateable = false;
		eIC.isScaleable = false;
		eIC.isScaleUnified = false;
		return eIC;
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return GEItemA.Assemble(_container, _loadedItem.identifier, _loadedItem.data, Main.camera);
	}

	public static void FillEditorItem(EIC _eic)
	{
		List<IComponent> collection = Assemble(_eic);
		_eic.gameComponents.AddRange(collection);
	}

	public static void PopulatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		string[] array = new string[1] { "propertyBar" };
		Camera canvasCamera = _propertyBar.canvasCamera;
		BasicLevelData basicLevelData = _eic.data as BasicLevelData;
	}

	public static void HandlePropertyChange(EventC _c)
	{
		BasicLevelData basicLevelData = EditorState.m_selection[0].data as BasicLevelData;
		string identifier = _c.identifier;
		if (identifier != null)
		{
			if (_003C_003Ef__switch_0024map8 == null)
			{
				_003C_003Ef__switch_0024map8 = new Dictionary<string, int>(0);
			}
			int value;
			if (!_003C_003Ef__switch_0024map8.TryGetValue(identifier, out value))
			{
			}
		}
		EditorState.ResetEditorItem(EditorState.m_selection[0]);
	}
}

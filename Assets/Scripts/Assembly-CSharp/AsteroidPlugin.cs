using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AsteroidPlugin : GEPlugin
{
	public AsteroidPlugin()
	{
		ResourceManager.AddResourceGroup("AsteroidCommon");
		ResourceManager.AddResourceToGroup("AsteroidCommon", new UnityResource("Explosion", "Asteroids/FX/Explosion/ExplosionPrefab", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("AsteroidCommon", new UnityResource("Ship", "Asteroids/3D/Ship/Ship", ResourceType.GameObject));
		ResourceManager.AddResourceToGroup("AsteroidCommon", new UnityResource("RocketBlast", "Asteroids/Materials/RocketBlast", ResourceType.Texture));
		ResourceManager.AddResourceToGroup("AsteroidCommon", new UnityResource("RocketBlastShader", "Asteroids/Materials/RocketBlastShader", ResourceType.Shader));
		ResourceManager.AddResourceToGroup("AsteroidCommon", new UnityResource("RocketBlastMat", "Asteroids/Materials/RocketBlastMat", ResourceType.Material));
		ResourceManager.LoadResourceGroup("AsteroidCommon");
		ResourceManager.AddResourceGroup("AsteroidEditor");
		ResourceManager.AddResourceToGroup("AsteroidEditor", new UnityResource("AsteroidEditorIcons", "Asteroids/Materials/AsteroidsEditorIconsDif", ResourceType.Texture));
	}

	public override void Execute()
	{
	}

	public override void Enter(IStatedObject _parent)
	{
		ResourceManager.LoadResourceGroup("AsteroidEditor");
		if (AState.icons == null)
		{
			AState.icons = SpriteS.AddSpriteSheet(100, Main.uiCamera, ResourceManager.GetTexture("AsteroidEditorIcons"), ResourceManager.GetShader("EditorUIShader"), 1f);
		}
	}

	public override void Exit()
	{
		if (AState.icons != null)
		{
			SpriteS.RemoveSpriteSheet(AState.icons);
			AState.icons = null;
		}
		ResourceManager.UnloadResourceGroup("AsteroidEditor");
	}

	public override void Initialize()
	{
		ASystem.Initialize();
		if (AState.tss == null)
		{
			AState.tss = SpriteS.AddSpriteSheet(200, Main.camera, ResourceManager.GetMaterial("RocketBlastMat"), 0.5f);
		}
	}

	public override void Update()
	{
		ASystem.Update();
	}

	public override bool RemoveComponent(IComponent _c)
	{
		switch (_c.componentType)
		{
		case (ComponentType)31:
			ASystem.RemoveAsteroidComponent(_c);
			return true;
		case (ComponentType)33:
			ASystem.RemoveEmitterComponent(_c);
			return true;
		case (ComponentType)32:
			ASystem.RemoveBulletComponent(_c);
			return true;
		case (ComponentType)30:
			ASystem.RemoveShipComponent(_c);
			return true;
		default:
			return false;
		}
	}

	public override bool UpdatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		bool result = true;
		if (_eic.identifier == "Emitter")
		{
			string[] tags = new string[1] { "propertyBar" };
			Camera canvasCamera = _propertyBar.canvasCamera;
			ShapeData shapeData = _eic.data as ShapeData;
			UIC component = NumericFieldA.Assemble(canvasCamera, "Asteroids", null, null, true, Align.Left, 40f, 1f, true, 5f, 50f, 20f, tags);
			UIC component2 = NumericFieldA.Assemble(canvasCamera, "Speed", null, null, true, Align.Left, 40f, 1f, true, 0f, 100f, 10f, tags);
			UIS.AddToCanvasGrid(component, _propertyBar, true);
			UIS.AddToCanvasGrid(component2, _propertyBar, true);
		}
		else if (_eic.identifier == "AShip")
		{
			AShipA.PopulatePropertyBar(_eic, _propertyBar);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override bool FillItemBar(UIC m_itemBar)
	{
		UIC uIC = GELibraryCategoryA.Assemble(m_itemBar, false, "Asteroid Plugin Stuff");
		GELibraryItemA.Assemble(uIC, "Emitter", 54, AState.icons);
		GELibraryItemA.Assemble(uIC, "AShip", 53, AState.icons);
		GELibraryItemA.Assemble(uIC, "Asteroid", 52, AState.icons);
		UIS.PlaceCanvasContent(uIC);
		return true;
	}

	public override bool FillEditorItem(EIC _eic)
	{
		bool result = true;
		if (_eic.identifier == "Emitter")
		{
			AEmitterA.FillEditorItem(_eic);
		}
		else if (_eic.identifier == "AShip")
		{
			AShipA.FillEditorItem(_eic);
		}
		else if (_eic.identifier == "Asteroid")
		{
			AAsteroidA.FillEditorItem(_eic);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override bool CreateNewEditorItem(GELevel _level, List<EIC> _newItems, EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		bool result = true;
		switch (_identifier)
		{
		case "Emitter":
			_newItems.Add(AEmitterA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "AShip":
			_newItems.Add(AShipA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Asteroid":
			_newItems.Add(AShipA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		default:
			result = false;
			break;
		}
		return result;
	}

	public override EIC CreateLoadedEditorItem(GELevel _level, EIC _container, EIC _loadedItem)
	{
		EIC result = null;
		if (_loadedItem.identifier == "Emitter")
		{
			result = AEmitterA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		else if (_loadedItem.identifier == "AShip")
		{
			result = AShipA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		else if (_loadedItem.identifier == "Asteroid")
		{
			result = AShipA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		return result;
	}

	public override IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		return ASystem.GetControlledComponentWithUniqueId(_id);
	}

	public override bool GetObjectData(SerializationInfo info, ILevelData data)
	{
		bool result = true;
		if (data.dataType == 20)
		{
			info.AddValue("data", (ShipData)data);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override SpriteSheet GetIconSheet()
	{
		return AState.icons;
	}

	public override int GetIconIndex(string _identifier)
	{
		switch (_identifier)
		{
		case "AShip":
			return 53;
		case "Emitter":
			return 54;
		case "Asteroid":
			return 52;
		default:
			return 15;
		}
	}
}

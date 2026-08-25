using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PBPlugin : GEPlugin
{
	public PBPlugin()
	{
		GEMat item = new GEMat("Pinb", "Pinball", "PinballFill", 5f, "PinballRoad", 1f);
		GEState.groundMats.Add(item);
		GEState.backgroundMats.Add(new GEMat("Pinb", "Pinball", "PinballBackground", 1f));
		ResourceManager.AddResourceGroup("PinbEditor");
		ResourceManager.AddResourceToGroup("PinbEditor", new UnityResource("PinbEditorIcons", "Pinb/SpriteSheets/Editor/PinbEditorIconsDif", ResourceType.Texture));
		ResourceManager.AddResourceGroup("PinballCommon");
		ResourceManager.AddResourceToGroup("PinballCommon", new UnityResource("PinballSheet", "Pinb/SpriteSheets/PinbSheet", ResourceType.Texture));
		ResourceManager.LoadResourceGroup("PinballCommon");
	}

	public override void Enter(IStatedObject _parent)
	{
		ResourceManager.LoadResourceGroup("PinbEditor");
		if (PBState.icons == null)
		{
			PBState.icons = SpriteS.AddSpriteSheet(50, Main.uiCamera, ResourceManager.GetTexture("PinbEditorIcons"), ResourceManager.GetShader("EditorUIShader"), 1f);
		}
	}

	public override void Execute()
	{
	}

	public override void Exit()
	{
		if (PBState.icons != null)
		{
			SpriteS.RemoveSpriteSheet(PBState.icons);
			PBState.icons = null;
		}
		ResourceManager.UnloadResourceGroup("PinbEditor");
	}

	public override void Initialize()
	{
		if (PBState.pinballSheet == null)
		{
			PBState.pinballSheet = SpriteS.AddSpriteSheet(100, Main.camera, ResourceManager.GetTexture("PinballSheet"), ResourceManager.GetShader("PropsShader"), 1f);
		}
		PBSystem.Initialize();
	}

	public override void Update()
	{
		PBSystem.Update();
	}

	public override bool RemoveComponent(IComponent _c)
	{
		ComponentType componentType = _c.componentType;
		if (componentType == (ComponentType)70)
		{
			PBSystem.RemovePadComponent(_c);
			return true;
		}
		return false;
	}

	public override bool FillItemBar(UIC m_itemBar)
	{
		UIC uIC = GELibraryCategoryA.Assemble(m_itemBar, false, "Pinball");
		GELibraryItemA.Assemble(uIC, "Ball", 15, BlobState.icons);
		GELibraryItemA.Assemble(uIC, "Pad", 15, BlobState.icons);
		GELibraryItemA.Assemble(uIC, "Round Bumper", 15, BlobState.icons);
		GELibraryItemA.Assemble(uIC, "Wall Bumber", 15, BlobState.icons);
		UIS.PlaceCanvasContent(uIC);
		return true;
	}

	public override bool CreateNewEditorItem(GELevel _level, List<EIC> _newItems, EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		bool result = true;
		switch (_identifier)
		{
		case "Ball":
			_newItems.Add(PBBallA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Pad":
			_newItems.Add(PBPadA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Round Bumper":
			_newItems.Add(PBRoundBumperA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
			break;
		case "Wall Bumber":
			_newItems.Add(PBWallBumperA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
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
		if (_loadedItem.identifier == "Ball")
		{
			result = PBBallA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		else if (_loadedItem.identifier == "Pad")
		{
			result = PBPadA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		else if (_loadedItem.identifier == "Round Bumper")
		{
			result = PBRoundBumperA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		else if (_loadedItem.identifier == "Wall Bumber")
		{
			result = PBWallBumperA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		return result;
	}

	public override bool FillEditorItem(EIC _eic)
	{
		bool flag = true;
		if (_eic.identifier == "Ball")
		{
			PBBallA.FillEditorItem(_eic);
		}
		else if (_eic.identifier == "Pad")
		{
			PBPadA.FillEditorItem(_eic);
		}
		else if (_eic.identifier == "Round Bumper")
		{
			PBRoundBumperA.FillEditorItem(_eic);
		}
		else if (_eic.identifier == "Wall Bumber")
		{
			PBWallBumperA.FillEditorItem(_eic);
		}
		return false;
	}

	public override bool UpdatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		bool result = true;
		if (!(_eic.identifier == "Ball"))
		{
			result = false;
		}
		return result;
	}

	public override IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		return PBSystem.GetControlledComponentWithUniqueId(_id);
	}

	public override bool GetObjectData(SerializationInfo info, ILevelData data)
	{
		bool flag = true;
		return false;
	}

	public override SpriteSheet GetIconSheet()
	{
		return BlobState.icons;
	}

	public override int GetIconIndex(string _identifier)
	{
		switch (_identifier)
		{
		case "Proto Blob":
			return 15;
		default:
			return 15;
		}
	}
}

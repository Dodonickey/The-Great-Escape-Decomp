using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class BlobPlugin : GEPlugin
{
	public BlobPlugin()
	{
		ResourceManager.AddResourceGroup("BlobEditor");
		ResourceManager.AddResourceToGroup("BlobEditor", new UnityResource("BlobEditorIcons", "Blob/SpriteSheets/Editor/BlobEditorIconsDif", ResourceType.Texture));
	}

	public override void Enter(IStatedObject _parent)
	{
		ResourceManager.LoadResourceGroup("BlobEditor");
		if (BlobState.icons == null)
		{
			BlobState.icons = SpriteS.AddSpriteSheet(50, Main.uiCamera, ResourceManager.GetTexture("BlobEditorIcons"), ResourceManager.GetShader("EditorUIShader"), 1f);
		}
	}

	public override void Execute()
	{
	}

	public override void Exit()
	{
		if (BlobState.icons != null)
		{
			SpriteS.RemoveSpriteSheet(BlobState.icons);
			BlobState.icons = null;
		}
		ResourceManager.UnloadResourceGroup("BlobEditor");
	}

	public override void Initialize()
	{
		BlobS.Initialize();
	}

	public override void Update()
	{
		BlobS.Update();
	}

	public override bool RemoveComponent(IComponent _c)
	{
		switch (_c.componentType)
		{
		case (ComponentType)60:
			BlobS.RemoveBlobComponent(_c);
			return true;
		case (ComponentType)61:
			BlobS.RemoveGoalComponent(_c);
			return true;
		default:
			return false;
		}
	}

	public override bool FillItemBar(UIC m_itemBar)
	{
		UIC uIC = GELibraryCategoryA.Assemble(m_itemBar, false, "Blob");
		GELibraryItemA.Assemble(uIC, "Proto Blob", 15, BlobState.icons);
		GELibraryItemA.Assemble(uIC, "Blob Goal", 15, BlobState.icons);
		UIS.PlaceCanvasContent(uIC);
		return true;
	}

	public override bool CreateNewEditorItem(GELevel _level, List<EIC> _newItems, EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		bool result = true;
		if (_identifier == "Proto Blob")
		{
			_newItems.Add(ProtoBlobA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
		}
		else if (_identifier == "Blob Goal")
		{
			_newItems.Add(BGoalA.CreateNewEditorItem(_container, _identifier, _pos, _rot, _sca));
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override EIC CreateLoadedEditorItem(GELevel _level, EIC _container, EIC _loadedItem)
	{
		EIC result = null;
		if (_loadedItem.identifier == "Proto Blob")
		{
			result = ProtoBlobA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		else if (_loadedItem.identifier == "Blob Goal")
		{
			result = BGoalA.CreateLoadedEditorItem(_container, _loadedItem);
		}
		return result;
	}

	public override bool FillEditorItem(EIC _eic)
	{
		bool flag = true;
		if (_eic.identifier == "Proto Blob")
		{
			ProtoBlobA.FillEditorItem(_eic);
		}
		else if (_eic.identifier == "Blob Goal")
		{
			BGoalA.FillEditorItem(_eic);
		}
		return false;
	}

	public override bool UpdatePropertyBar(EIC _eic, UIC _propertyBar)
	{
		bool result = true;
		if (!(_eic.identifier == "Proto Blob"))
		{
			result = false;
		}
		return result;
	}

	public override IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		return BlobS.GetControlledComponentWithUniqueId(_id);
	}

	public override bool GetObjectData(SerializationInfo info, ILevelData data)
	{
		bool result = true;
		if (data.dataType == 30)
		{
			info.AddValue("data", (BlobData)data);
		}
		else if (data.dataType == 31)
		{
			info.AddValue("data", (BlobGoalData)data);
		}
		else
		{
			result = false;
		}
		return result;
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

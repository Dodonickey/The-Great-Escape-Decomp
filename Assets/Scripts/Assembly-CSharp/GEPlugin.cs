using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GEPlugin
{
	public virtual void Initialize()
	{
	}

	public virtual void Enter(IStatedObject _parent)
	{
	}

	public virtual void Execute()
	{
	}

	public virtual void Exit()
	{
	}

	public virtual void Update()
	{
	}

	public virtual bool RemoveComponent(IComponent _c)
	{
		return false;
	}

	public virtual ILevel GenerateLevel(ILevel _level)
	{
		return null;
	}

	public virtual bool SaveLevel(ILevel _level, string _fileName)
	{
		return false;
	}

	public virtual bool FillItemBar(UIC m_itemBar)
	{
		return false;
	}

	public virtual bool CreateNewEditorItem(GELevel _level, List<EIC> _newItems, EIC _container, string _identifier, Vector3 _pos, Vector3 _rot, Vector3 _sca)
	{
		return false;
	}

	public virtual EIC CreateLoadedEditorItem(GELevel _level, EIC _container, EIC _loadedItem)
	{
		return null;
	}

	public virtual bool UpdatePropertyBar(EIC eic, UIC _propertyBar)
	{
		return false;
	}

	public virtual bool FillEditorItem(EIC _eic)
	{
		return false;
	}

	public virtual IControlledComponent GetControlledComponentWithUniqueId(uint _id)
	{
		return null;
	}

	public virtual bool GetObjectData(SerializationInfo _info, ILevelData _data)
	{
		return false;
	}

	public virtual int GetIconIndex(string _identifier)
	{
		return 15;
	}

	public virtual SpriteSheet GetIconSheet()
	{
		return GEState.outlinerIconSheet;
	}

	public virtual bool CreateShapes()
	{
		return false;
	}
}

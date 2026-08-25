public class GEBuiltinLevelResource : IResource
{
	private IResourceGroup _resourceGroup;

	private string _identifier;

	private ResourceType _type;

	private object _resourceObject;

	private string _path;

	private ResourceLoadState _loadState;

	public IResourceGroup resourceGroup
	{
		get
		{
			return _resourceGroup;
		}
		set
		{
			_resourceGroup = value;
		}
	}

	public string identifier
	{
		get
		{
			return _identifier;
		}
		set
		{
			_identifier = value;
		}
	}

	public ResourceType type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public object resourceObject
	{
		get
		{
			return _resourceObject;
		}
		set
		{
			_resourceObject = value;
		}
	}

	public string path
	{
		get
		{
			return _path;
		}
		set
		{
			_path = value;
		}
	}

	public ResourceLoadState loadState
	{
		get
		{
			return _loadState;
		}
		set
		{
			_loadState = value;
		}
	}

	public GEBuiltinLevelResource(string _identifier, string _path, ResourceType _type)
	{
		identifier = _identifier;
		path = _path;
		type = _type;
		loadState = ResourceLoadState.Unloaded;
	}

	public void Load()
	{
		loadState = ResourceLoadState.Loading;
		GELevelSerializer gELevelSerializer = new GELevelSerializer();
		resourceObject = gELevelSerializer.DeSerializeUnityLevel(path);
		if (resourceGroup != null)
		{
			resourceGroup.ResourceLoaded(this);
		}
		loadState = ResourceLoadState.Loaded;
	}

	public void Unload()
	{
		loadState = ResourceLoadState.Unloading;
		resourceObject = null;
		if (resourceGroup != null)
		{
			resourceGroup.ResourceUnloaded(this);
		}
		loadState = ResourceLoadState.Unloaded;
	}
}

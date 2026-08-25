using System.Collections.Generic;

public class BasicResourceGroup : IResourceGroup
{
	private string _identifier;

	private List<IResource> _resources;

	private ResourceLoadState _loadState;

	private int _loadedCount;

	private int _unloadedCount;

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

	public List<IResource> resources
	{
		get
		{
			return _resources;
		}
		set
		{
			_resources = value;
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

	public int loadedCount
	{
		get
		{
			return _loadedCount;
		}
		set
		{
			_loadedCount = value;
		}
	}

	public int unloadedCount
	{
		get
		{
			return _unloadedCount;
		}
		set
		{
			_unloadedCount = value;
		}
	}

	public BasicResourceGroup(string _identifier)
	{
		identifier = _identifier;
		resources = new List<IResource>();
		loadState = ResourceLoadState.Unloaded;
		loadedCount = 0;
	}

	public void ResourceLoaded(IResource _resource)
	{
		loadedCount++;
		if (loadedCount == resources.Count)
		{
			loadState = ResourceLoadState.Loaded;
		}
	}

	public void ResourceUnloaded(IResource _resource)
	{
		unloadedCount--;
		if (unloadedCount == resources.Count)
		{
			loadState = ResourceLoadState.Unloaded;
		}
	}
}

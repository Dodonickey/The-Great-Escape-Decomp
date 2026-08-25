using System;
using System.Collections;
using UnityEngine;

public static class ResourceManager
{
	private static Hashtable m_resourceGroups = new Hashtable();

	private static Hashtable m_resources = new Hashtable();

	public static IResourceGroup AddResourceGroup(string _identifier)
	{
		BasicResourceGroup basicResourceGroup = new BasicResourceGroup(_identifier);
		m_resourceGroups.Add(_identifier, basicResourceGroup);
		return basicResourceGroup;
	}

	public static IResource AddResourceToGroup(IResourceGroup _group, IResource _resource)
	{
		if (!m_resources.Contains(_resource.identifier))
		{
			m_resources.Add(_resource.identifier, _resource);
			_group.resources.Add(_resource);
			_group.loadState = ResourceLoadState.Unloaded;
		}
		return _resource;
	}

	public static IResource AddResourceToGroup(string _groupIdentifier, IResource _resource)
	{
		if (m_resourceGroups.Contains(_groupIdentifier))
		{
			return AddResourceToGroup(m_resourceGroups[_groupIdentifier] as IResourceGroup, _resource);
		}
		return null;
	}

	public static void LoadResourceGroup(IResourceGroup _group)
	{
		_group.loadedCount = 0;
		if (_group.loadState == ResourceLoadState.Unloaded)
		{
			for (int i = 0; i < _group.resources.Count; i++)
			{
				LoadResource(_group.resources[i]);
			}
		}
		_group.loadState = ResourceLoadState.Loading;
	}

	public static void LoadResourceGroup(string _identifier)
	{
		if (m_resourceGroups.Contains(_identifier))
		{
			LoadResourceGroup((IResourceGroup)m_resourceGroups[_identifier]);
		}
	}

	public static void UnloadResourceGroup(IResourceGroup _group)
	{
		_group.loadState = ResourceLoadState.Unloading;
		if (_group.loadState == ResourceLoadState.Loaded)
		{
			for (int i = 0; i < _group.resources.Count; i++)
			{
				UnloadResource(_group.resources[i], false);
			}
		}
		AsyncOperation asyncOperation = Resources.UnloadUnusedAssets();
		GC.Collect();
		_group.loadState = ResourceLoadState.Unloaded;
	}

	public static void UnloadResourceGroup(string _identifier)
	{
		if (m_resourceGroups.Contains(_identifier))
		{
			UnloadResourceGroup((IResourceGroup)m_resourceGroups[_identifier]);
		}
	}

	public static void LoadResource(IResource _resource)
	{
		if (_resource.loadState == ResourceLoadState.Unloaded)
		{
			_resource.Load();
		}
	}

	public static void LoadResource(string _identifier)
	{
		if (m_resources.Contains(_identifier))
		{
			LoadResource((IResource)m_resources[_identifier]);
		}
	}

	public static void UnloadResource(IResource _resource)
	{
		UnloadResource(_resource, true);
	}

	public static void UnloadResource(IResource _resource, bool _unloadAssets)
	{
		if (_resource.loadState == ResourceLoadState.Loaded)
		{
			_resource.Unload();
		}
		if (_unloadAssets)
		{
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
	}

	public static void UnloadResource(string _identifier)
	{
		if (m_resources.Contains(_identifier))
		{
			UnloadResource((IResource)m_resources[_identifier]);
		}
	}

	public static IResource GetResourceClass(string _resourceIdentifier)
	{
		if (m_resources.Contains(_resourceIdentifier))
		{
			IResource resource = (IResource)m_resources[_resourceIdentifier];
			if (resource.loadState != ResourceLoadState.Loaded)
			{
				LoadResource(resource);
			}
			return resource;
		}
		return null;
	}

	public static AudioClip GetAudioClip(string _resourceIdentifier)
	{
		IResource resourceClass = GetResourceClass(_resourceIdentifier);
		if (resourceClass != null && resourceClass.resourceObject != null)
		{
			return resourceClass.resourceObject as AudioClip;
		}
		return null;
	}

	public static Texture GetTexture(string _resourceIdentifier)
	{
		IResource resourceClass = GetResourceClass(_resourceIdentifier);
		if (resourceClass != null && resourceClass.resourceObject != null)
		{
			return resourceClass.resourceObject as Texture;
		}
		return null;
	}

	public static Shader GetShader(string _resourceIdentifier)
	{
		IResource resourceClass = GetResourceClass(_resourceIdentifier);
		if (resourceClass != null && resourceClass.resourceObject != null)
		{
			return resourceClass.resourceObject as Shader;
		}
		return null;
	}

	public static Material GetMaterial(string _resourceIdentifier)
	{
		IResource resourceClass = GetResourceClass(_resourceIdentifier);
		if (resourceClass != null && resourceClass.resourceObject != null)
		{
			return resourceClass.resourceObject as Material;
		}
		return null;
	}

	public static GameObject GetGameObject(string _resourceIdentifier)
	{
		IResource resourceClass = GetResourceClass(_resourceIdentifier);
		if (resourceClass != null && resourceClass.resourceObject != null)
		{
			return resourceClass.resourceObject as GameObject;
		}
		return null;
	}

	public static ILevel GetLevel(string _resourceIdentifier)
	{
		IResource resourceClass = GetResourceClass(_resourceIdentifier);
		if (resourceClass != null && resourceClass.resourceObject != null)
		{
			return resourceClass.resourceObject as ILevel;
		}
		return null;
	}
}

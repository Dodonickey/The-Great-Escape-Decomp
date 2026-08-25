using System;
using UnityEngine;

public class UnityResource : IResource
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

    public UnityResource(string _identifier, string _path, ResourceType _type)
    {
        identifier = _identifier;
        path = _path;
        type = _type;
        loadState = ResourceLoadState.Unloaded;
    }

    public void Load()
    {
        loadState = ResourceLoadState.Loading;

        // Explicitly tell Resources.Load which System.Type to fetch based on ResourceType
        Type systemType = GetSystemTypeFromResourceType(type);

        if (systemType != null)
        {
            resourceObject = Resources.Load(path, systemType);
        }
        else
        {
            resourceObject = Resources.Load(path);
        }

        // Fallback: If it failed to load with systemType, try generic load
        if (resourceObject == null)
        {
            resourceObject = Resources.Load(path);
        }

        if (resourceGroup != null)
        {
            resourceGroup.ResourceLoaded(this);
        }
        loadState = ResourceLoadState.Loaded;
    }

    private Type GetSystemTypeFromResourceType(ResourceType _resType)
    {
        switch (_resType)
        {
            case ResourceType.GameObject:
            case ResourceType.SpritePrefab:
                return typeof(GameObject);

            case ResourceType.Texture:
                return typeof(Texture);

            case ResourceType.Shader:
                return typeof(Shader);

            case ResourceType.Sound:
                return typeof(AudioClip);

            default:
                return typeof(UnityEngine.Object);
        }
    }

    public void Unload()
    {
        loadState = ResourceLoadState.Unloading;
        if (type != ResourceType.GameObject)
        {
            Resources.UnloadAsset(resourceObject as UnityEngine.Object);
        }
        resourceObject = null;
        if (resourceGroup != null)
        {
            resourceGroup.ResourceUnloaded(this);
        }
        loadState = ResourceLoadState.Unloaded;
    }
}
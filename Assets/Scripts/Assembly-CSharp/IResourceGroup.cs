using System.Collections.Generic;

public interface IResourceGroup
{
	string identifier { get; set; }

	List<IResource> resources { get; set; }

	ResourceLoadState loadState { get; set; }

	int loadedCount { get; set; }

	void ResourceLoaded(IResource _resource);

	void ResourceUnloaded(IResource _resource);
}

public interface IResource
{
	IResourceGroup resourceGroup { get; set; }

	string identifier { get; set; }

	ResourceType type { get; set; }

	object resourceObject { get; set; }

	string path { get; set; }

	ResourceLoadState loadState { get; set; }

	void Load();

	void Unload();
}

public interface IComponent
{
	bool active { get; set; }

	int index { get; set; }

	int entityIndex { get; set; }

	ComponentType componentType { get; set; }
}

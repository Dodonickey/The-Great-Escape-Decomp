public interface ILevelData
{
	uint dataType { get; set; }

	string name { get; set; }

	uint id { get; set; }

	bool active { get; set; }

	bool initialized { get; set; }

	Vertex3 position { get; set; }

	Vertex3 rotation { get; set; }

	Vertex3 scale { get; set; }

	void Init(uint _id, string _name);

	ILevelData DeepCopy();
}

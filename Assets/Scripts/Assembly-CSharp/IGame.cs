public interface IGame
{
	string m_projectCode { get; set; }

	string m_projectVersion { get; set; }

	IScene GetCurrentScene();

	void RemoveComponent(IComponent _c);

	ILevel GenerateLevel(ILevel _level);

	void SaveLevel(ILevel _level, string _fileName);

	void ClearLevel(ILevel _level);

	void Initialize(IScene _scene);

	void Update();
}

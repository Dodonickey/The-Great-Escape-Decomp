public static class GEContainerA
{
	public static EIC Assemble(EIC _container, string _identifier, ILevelData _data)
	{
		string[] tags = new string[3]
		{
			LevelManager.m_currentLevel.name,
			LevelManager.m_currentLevel.name + ":EditorItem",
			"EditorItem"
		};
		Entity entity = EntityManager.AddEntity(tags);
		TransformC tc = TransformS.AddComponent(entity);
		TransformC uiTC = TransformS.AddComponent(entity);
		return GES.AddEditorItemContainerComponent(entity, _container, _identifier, _data, EditorItemType.Container, tc, uiTC);
	}
}

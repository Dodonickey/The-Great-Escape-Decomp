using UnityEngine;

public static class GEConnectionA
{
	public static EIC Assemble(ILevelData _data)
	{
		string[] tags = new string[3]
		{
			LevelManager.m_currentLevel.name,
			LevelManager.m_currentLevel.name + ":EditorConnection",
			"EditorConnection"
		};
		ConnectionData connectionData = _data as ConnectionData;
		int num = Mathf.Max(LevelManager.m_levels.Count - 1, 0);
		connectionData.startId += (uint)(10000 * num);
		connectionData.endId += (uint)(10000 * num);
		Entity entity = EntityManager.AddEntity(tags);
		return GES.AddEditorItemContainerComponent(entity, null, "EditorConnection", connectionData, EditorItemType.Container, null, null);
	}

	public static EIC CreateLoadedEditorItem(EIC _container, EIC _loadedItem)
	{
		return Assemble(_loadedItem.data);
	}

	public static void FillEditorItem(EIC _eic)
	{
		uint startId = (_eic.data as ConnectionData).startId;
		uint endId = (_eic.data as ConnectionData).endId;
		ConnectionSlotType startType = (ConnectionSlotType)(_eic.data as ConnectionData).startType;
		ConnectionSlotType endType = (ConnectionSlotType)(_eic.data as ConnectionData).endType;
		BasicControlledComponent basicControlledComponent = GES.GetControlledComponentWithUniqueId(startId) as BasicControlledComponent;
		BasicControlledComponent basicControlledComponent2 = GES.GetControlledComponentWithUniqueId(endId) as BasicControlledComponent;
		if (basicControlledComponent == null || basicControlledComponent2 == null)
		{
			Debug.LogError(string.Concat("start or end controlled component not found. controller: ", basicControlledComponent, ", controllee: ", basicControlledComponent2));
		}
		else if (GEConnectionLogic.GetConnection(startId, endId, startType, endType) == null)
		{
			string[] tags = new string[2]
			{
				LevelManager.m_currentLevel.name + ":GameEntity",
				LevelManager.m_currentLevel.name
			};
			Entity entity = EntityManager.AddEntity(tags);
			ConnectionSlot startSlot = null;
			ConnectionSlot endSlot = null;
			bool flag = false;
			switch (endType)
			{
			case ConnectionSlotType.Activate:
			case ConnectionSlotType.ColliderType:
			case ConnectionSlotType.Deactivate:
			case ConnectionSlotType.Destroy:
			case ConnectionSlotType.Modifier:
				flag = true;
				break;
			}
			if (!flag)
			{
				for (int i = 0; i < basicControlledComponent.outputSlots.Length; i++)
				{
					if (basicControlledComponent.outputSlots[i].m_connectionSlotType == startType)
					{
						startSlot = basicControlledComponent.outputSlots[i];
						break;
					}
				}
				for (int j = 0; j < basicControlledComponent2.inputSlots.Length; j++)
				{
					if (basicControlledComponent2.inputSlots[j].m_connectionSlotType == endType)
					{
						endSlot = basicControlledComponent2.inputSlots[j];
						break;
					}
				}
			}
			else
			{
				for (int k = 0; k < basicControlledComponent.outputSlots.Length; k++)
				{
					if (basicControlledComponent.outputSlots[k].m_connectionSlotType == startType)
					{
						startSlot = basicControlledComponent.outputSlots[k];
						break;
					}
				}
				for (int l = 0; l < basicControlledComponent2.modifierSlots.Length; l++)
				{
					if (basicControlledComponent2.modifierSlots[l].m_connectionSlotType == endType)
					{
						endSlot = basicControlledComponent2.modifierSlots[l];
						break;
					}
				}
			}
			GEConnectionC gEConnectionC = GES.AddConnectionComponent(entity.index, startSlot, endSlot, basicControlledComponent, basicControlledComponent2);
			_eic.gameComponents.Add(gEConnectionC);
			if (GEState.editorMode)
			{
				gEConnectionC.container = _eic;
			}
			UndoManager.AddStep(new CreateConnectionStep(_eic, gEConnectionC, entity.index, startSlot, endSlot, basicControlledComponent, basicControlledComponent2));
		}
		else
		{
			Debug.LogError("duplicate connection");
		}
	}
}

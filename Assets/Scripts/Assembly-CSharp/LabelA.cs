using UnityEngine;

public static class LabelA
{
	public static UIC Assemble(Camera _camera, string _defaultText, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		TransformS.SetPosition(transformC, Vector3.zero);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.TextField);
		uIC.label = _defaultText;
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[transformC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		TransformS.SetPosition(uIC.contentTC, Vector3.forward * -15f);
		TextS.SetStyle("subheader");
		uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _defaultText, 1f, Align.Center, Align.Middle);
		SpriteS.SetColorByTransformComponent(uIC.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
		SpriteS.ConvertSpritesToPrefabComponent(uIC.textC.contentTC, _camera, true);
		uIC.width = uIC.textC.textWidth;
		uIC.height = uIC.textC.textHeight;
		return uIC;
	}
}

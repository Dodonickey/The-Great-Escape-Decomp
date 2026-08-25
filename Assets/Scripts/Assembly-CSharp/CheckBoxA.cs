using UnityEngine;

public static class CheckBoxA
{
	public static UIC Assemble(Camera _camera, string _identifier, EventDelegate _eventHandler, TouchEventDelegate _customTouchEventHandler, bool _defaultDraw, Align _labelPlacement, float _scale, bool _checked, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.CheckBox);
		if (_eventHandler != null)
		{
			uIC.EC = EventS.AddComponent(uIC.entityIndex, _identifier, _eventHandler, 0f, false, false, false, false);
			EventS.AddProperty(uIC.EC, "identifier", _identifier);
			EventS.AddProperty(uIC.EC, "checked", _checked);
		}
		uIC.radius = 40f * _scale * 0.5f;
		uIC.width = 40f * _scale;
		uIC.height = 40f * _scale;
		uIC.isChecked = _checked;
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[transformC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		TransformS.SetPosition(uIC.contentTC, Vector3.forward * -25f);
		if (_defaultDraw)
		{
			TextS.SetStyle("body");
			Vector3 zero = Vector3.zero;
			switch (_labelPlacement)
			{
			case Align.Left:
				uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Right, Align.Middle);
				TransformS.Move(uIC.textC.contentTC, Vector3.right * (0f - uIC.width) * 0.75f);
				zero.x = uIC.width * 0.167f + uIC.textC.textWidth;
				uIC.width += zero.x;
				TransformS.Move(uIC.contentTC, zero * 0.5f);
				break;
			case Align.Right:
				uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Left, Align.Middle);
				TransformS.Move(uIC.textC.contentTC, Vector3.right * uIC.width * 0.75f);
				zero.x = uIC.width * 0.167f + uIC.textC.textWidth;
				uIC.width += zero.x;
				TransformS.Move(uIC.contentTC, zero * -0.5f);
				break;
			case Align.Top:
				uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Center, Align.Bottom);
				TransformS.Move(uIC.textC.contentTC, Vector3.up * uIC.width * 0.75f);
				zero.y = uIC.height * 0.167f + uIC.textC.textHeight;
				uIC.height += zero.y;
				TransformS.Move(uIC.contentTC, zero * -0.5f);
				break;
			case Align.Bottom:
				uIC.textC = TextS.AddSingleLineComponent(uIC.contentTC, _identifier, 1f * _scale, Align.Center, Align.Top);
				TransformS.Move(uIC.textC.contentTC, Vector3.up * (0f - uIC.width) * 0.75f);
				zero.y = uIC.height * 0.167f + uIC.textC.textHeight;
				uIC.height += zero.y;
				TransformS.Move(uIC.contentTC, zero * 0.5f);
				break;
			}
			SpriteS.SetColorByTransformComponent(uIC.textC.contentTC, DebugDraw.GetColor(0f, 0f, 0f), false, false);
			SpriteS.ConvertSpritesToPrefabComponent(uIC.textC.contentTC, _camera, true);
			Vector2[] roundedRect = DebugDraw.GetRoundedRect(40f * _scale, 40f * _scale, 5f * _scale, 5, Vector2.zero);
			Vector2[] roundedRect2 = DebugDraw.GetRoundedRect(40f * _scale * 0.647f, 40f * _scale * 0.647f, 5f * _scale * 0.647f, 5, Vector2.zero);
			PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.contentTC, Vector3.forward * 10f, DebugDraw.Vector2ArrayToPolygon(roundedRect), DebugDraw.GetColor(230f, 230f, 230f), ResourceManager.GetMaterial("Solid"), _camera);
			uIC.foregroundPCs.AddRange(PrefabS.CreateFlatPrefabComponentsFromPolygon(uIC.contentTC, Vector3.forward * -10f, DebugDraw.Vector2ArrayToPolygon(roundedRect2), DebugDraw.GetColor(0f, 0f, 0f), ResourceManager.GetMaterial("Solid"), _camera));
		}
		uIC.TAC = TouchAreaS.AddComponent(transformC, _identifier, uIC.width, uIC.height, true, _camera, uIC);
		if (_customTouchEventHandler != null)
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, _customTouchEventHandler);
		}
		else
		{
			TouchAreaS.AddTouchEventListener(uIC.TAC, HandleTouches);
		}
		if (_checked)
		{
			UIS.CheckBox(uIC);
		}
		else
		{
			UIS.UncheckBox(uIC);
		}
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		if (!uIC.enabled || touchEvent != TouchEvent.Began)
		{
			return;
		}
		if (uIC.isChecked)
		{
			UIS.UncheckBox(uIC);
			for (int i = 0; i < uIC.controlledUICs.Count; i++)
			{
				if (uIC.controlledUICDirs[i])
				{
					UIS.Enable(uIC.controlledUICs[i]);
				}
				else
				{
					UIS.Disable(uIC.controlledUICs[i]);
				}
			}
		}
		else
		{
			UIS.CheckBox(uIC);
			for (int j = 0; j < uIC.controlledUICs.Count; j++)
			{
				if (uIC.controlledUICDirs[j])
				{
					UIS.Disable(uIC.controlledUICs[j]);
				}
				else
				{
					UIS.Enable(uIC.controlledUICs[j]);
				}
			}
		}
		if (uIC.EC != null)
		{
			uIC.EC.properties["checked"] = uIC.isChecked;
			EventS.Dispatch(uIC.EC, false);
		}
	}
}

using UnityEngine;

public static class SubListCanvasA
{
	public static UIC Assemble(Camera _camera, string _identifier, UIC _parent, int _width, int _height, float _margin, float _spacing, string[] _tags, Align _align, bool _horizontal, Vector2 _direction, bool _drawBackground)
	{
		TransformC tc = EntityManager.AddEntityWithTC(_tags);
		UIC uIC = null;
		uIC = ((!_horizontal) ? UIS.AddComponent(tc, UIComponentType.VerticalSubListCanvas) : UIS.AddComponent(tc, UIComponentType.HorizontalSubListCanvas));
		uIC.width = _width;
		uIC.height = _height;
		uIC.canvasHeight = _height;
		uIC.canvasWidth = _width;
		uIC.contentMargin = _margin;
		uIC.contentSpacing = _spacing;
		bool flag = _direction.x == 1f;
		bool flag2 = _direction.x == -1f;
		bool flag3 = _direction.y == 1f;
		bool flag4 = _direction.y == -1f;
		Vector2 vector = default(Vector2);
		if (flag && _horizontal)
		{
			vector.x = _parent.width * 0.5f + (float)_width * 0.5f;
		}
		else if (flag2 && _horizontal)
		{
			vector.x = _parent.width * -0.5f - (float)_width * 0.5f;
		}
		else
		{
			vector.x = (float)_width * 0.5f * _direction.x;
			if (_direction.x > 0f)
			{
				vector.x -= _parent.width * 0.5f;
			}
			else if (_direction.x < 0f)
			{
				vector.x += _parent.width * 0.5f;
			}
		}
		if (flag3 && !_horizontal)
		{
			vector.y = _parent.height * 0.5f + (float)_height * 0.5f;
		}
		else if (flag4 && !_horizontal)
		{
			vector.y = _parent.height * -0.5f - (float)_height * 0.5f;
		}
		else
		{
			vector.y = (float)_height * 0.5f * _direction.y;
			if (_direction.y > 0f)
			{
				vector.y -= _parent.height * 0.5f;
			}
			else if (_direction.y < 0f)
			{
				vector.y += _parent.height * 0.5f;
			}
		}
		switch (_align)
		{
		case Align.Left:
			uIC.startContentX = (float)_width * -0.5f + uIC.contentMargin;
			uIC.startContentY = (float)_height * 0.5f - uIC.contentMargin;
			break;
		case Align.Right:
			uIC.startContentX = (float)_width * 0.5f - uIC.contentMargin;
			uIC.startContentY = (float)_height * 0.5f - uIC.contentMargin;
			break;
		case Align.Center:
			uIC.startContentX = 0f;
			uIC.startContentY = (float)_height * 0.5f - uIC.contentMargin;
			break;
		}
		uIC.currentContentX = uIC.startContentX;
		uIC.currentContentY = uIC.startContentY;
		uIC.nextContentX = uIC.startContentX;
		uIC.nextContentY = uIC.startContentY;
		if (_parent != null)
		{
			TransformS.ParentComponent(uIC.TC, _parent.TC);
		}
		TransformS.SetPosition(uIC.TC, Vector3.forward * -10f);
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[uIC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		TransformS.SetPosition(uIC.contentTC, new Vector3(vector.x, vector.y, 0f));
		uIC.TAC = TouchAreaS.AddComponent(uIC.contentTC, _identifier, _width, _height, true, _camera, uIC);
		TouchAreaS.AddTouchEventListener(uIC.TAC, HandleTouches);
		if (_drawBackground)
		{
			PrefabS.CreateFlatPrefabComponentsFromPolygon(tc, Vector3.forward * 10f, DebugDraw.GetSubmenuArea(_parent, _width, _height, 15f, 6, _horizontal, _direction), DebugDraw.GetColor(204f, 214f, 141f), ResourceManager.GetMaterial("Solid"), _camera);
		}
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
	}
}

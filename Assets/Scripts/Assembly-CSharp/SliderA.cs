using UnityEngine;

public static class SliderA
{
	public static UIC Assemble(Camera _camera, string _identifier, TransformC _parent, Vector3 _pos, float _width, float _height, float _minValue, float _maxValue, float _startValue, int _snapPoints, float _minX, float _maxX, float _minY, float _maxY, string[] _tags)
	{
		TransformC transformC = EntityManager.AddEntityWithTC(_tags);
		if (_parent != null)
		{
			TransformS.ParentComponent(transformC, _parent);
		}
		TransformS.SetPosition(transformC, _pos);
		UIC uIC = UIS.AddComponent(transformC, UIComponentType.Slider);
		uIC.width = _width;
		uIC.height = _height;
		uIC.minValX = _minValue;
		uIC.maxValX = _maxValue;
		uIC.currentValX = _startValue;
		uIC.minValY = _minValue;
		uIC.maxValY = _maxValue;
		uIC.currentValY = _startValue;
		uIC.minX = _minX;
		uIC.maxX = _maxX;
		uIC.minY = _minY;
		uIC.maxY = _maxY;
		uIC.draggable = true;
		uIC.limitedDrag = true;
		uIC.isDragged = false;
		uIC.snapPoints = _snapPoints;
		uIC.snap = false;
		if (_snapPoints > 1)
		{
			uIC.snap = true;
			uIC.snapPointDistanceX = (uIC.maxX - uIC.minX) / (float)(_snapPoints - 1);
			uIC.snapPointDistanceY = (uIC.maxY - uIC.minY) / (float)(_snapPoints - 1);
		}
		uIC.contentTC = TransformS.AddComponent(EntityManager.m_entities.m_array[transformC.entityIndex]);
		TransformS.ParentComponent(uIC.contentTC, uIC.TC);
		float num = uIC.maxX - uIC.minX;
		float num2 = uIC.maxY - uIC.minY;
		float num3 = 0f;
		if (num > 0f)
		{
			num3 = _startValue / num;
		}
		float num4 = 0f;
		if (num2 > 0f)
		{
			num4 = _startValue / num2;
		}
		TransformS.SetPosition(uIC.contentTC, new Vector3(_minX + num3, _minY + num4, _pos.z));
		uIC.TAC = TouchAreaS.AddComponent(uIC.contentTC, _identifier, _width, _height, true, _camera, uIC);
		TouchAreaS.AddTouchEventListener(uIC.TAC, HandleTouches);
		return uIC;
	}

	public static void HandleTouches(TouchAreaC _c, int _i, bool _consumed)
	{
		UIC uIC = _c.customComponent as UIC;
		TouchEvent touchEvent = _c.touchEvent[_i];
		Vector2 vector = _c.touchPos[_i];
		if (touchEvent == TouchEvent.Began && _c.touchStartedInside[_i])
		{
			uIC.dragOffset = -(vector - new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f) - (Vector2)uIC.contentTC.transform.position);
			uIC.isDragged = true;
		}
		else if ((touchEvent == TouchEvent.Drag || (touchEvent == TouchEvent.Down && _c.touchWasDragged[_i])) && _c.touchStartedInside[_i])
		{
			if (!uIC.snap)
			{
				Vector3 vector2 = new Vector3(vector.x - (float)Screen.width * 0.5f + uIC.dragOffset.x, vector.y - (float)Screen.height * 0.5f + uIC.dragOffset.y, 0f);
				vector2.x = Mathf.Max(Mathf.Min(vector2.x, uIC.maxX + uIC.TC.transform.position.x), uIC.minX + uIC.TC.transform.position.x);
				vector2.y = Mathf.Max(Mathf.Min(vector2.y, uIC.maxY + uIC.TC.transform.position.y), uIC.minY + uIC.TC.transform.position.y);
				TransformS.SetPosition(uIC.contentTC, vector2 - uIC.TC.transform.position);
				float num = uIC.maxX - uIC.minX;
				float num2 = uIC.maxY - uIC.minY;
				uIC.currentValX = (0f - (uIC.TC.transform.position.x + uIC.minX - vector2.x)) / num * (uIC.maxValX - uIC.minValX) + uIC.minValX;
				uIC.currentValY = (0f - (uIC.TC.transform.position.y + uIC.minY - vector2.y)) / num2 * (uIC.maxValY - uIC.minValY) + uIC.minValY;
			}
			else
			{
				Vector3 vector3 = new Vector3(vector.x + uIC.dragOffset.x - (float)Screen.width * 0.5f, vector.y + uIC.dragOffset.y - (float)Screen.height * 0.5f, 0f);
				vector3.x = Mathf.Max(Mathf.Min(vector3.x, uIC.maxX + uIC.TC.transform.position.x), uIC.minX + uIC.TC.transform.position.x);
				vector3.y = Mathf.Max(Mathf.Min(vector3.y, uIC.maxY + uIC.TC.transform.position.y), uIC.minY + uIC.TC.transform.position.y);
				float num3 = 0f - (uIC.TC.transform.position.x + uIC.minX - vector3.x);
				float num4 = 0f - (uIC.TC.transform.position.y + uIC.minY - vector3.y);
				uIC.currentSnapIndexX = Mathf.RoundToInt(num3 / uIC.snapPointDistanceX);
				uIC.currentSnapIndexY = Mathf.RoundToInt(num4 / uIC.snapPointDistanceY);
			}
		}
		if (touchEvent == TouchEvent.Release || touchEvent == TouchEvent.ReleaseOutside)
		{
			uIC.isDragged = false;
		}
	}
}

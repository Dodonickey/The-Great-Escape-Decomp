using UnityEngine;

public class GEControlledValue
{
	public Vector3 vector;

	public string text;

	public GEControlledValue()
	{
		vector = Vector3.zero;
		text = string.Empty;
	}

	public void Add(GEControlledValue _v)
	{
		vector += _v.vector;
		text += _v.text;
	}

	public GEControlledValue AddR(GEControlledValue _v)
	{
		GEControlledValue gEControlledValue = new GEControlledValue();
		gEControlledValue.vector = vector + _v.vector;
		gEControlledValue.text = text + _v.text;
		return gEControlledValue;
	}

	public void Sub(GEControlledValue _v)
	{
		vector -= _v.vector;
	}

	public GEControlledValue SubR(GEControlledValue _v)
	{
		GEControlledValue gEControlledValue = new GEControlledValue();
		gEControlledValue.vector = vector - _v.vector;
		return gEControlledValue;
	}

	public void Div(GEControlledValue _v)
	{
		if (_v.vector.x != 0f)
		{
			vector.x /= _v.vector.x;
		}
		if (_v.vector.y != 0f)
		{
			vector.y /= _v.vector.y;
		}
		if (_v.vector.z != 0f)
		{
			vector.z /= _v.vector.z;
		}
	}

	public GEControlledValue DivR(GEControlledValue _v)
	{
		GEControlledValue result = new GEControlledValue();
		if (_v.vector.x != 0f)
		{
			vector.x /= _v.vector.x;
		}
		if (_v.vector.y != 0f)
		{
			vector.y /= _v.vector.y;
		}
		if (_v.vector.z != 0f)
		{
			vector.z /= _v.vector.z;
		}
		return result;
	}

	public void Mul(GEControlledValue _v)
	{
		vector.x *= _v.vector.x;
		vector.y *= _v.vector.y;
		vector.z *= _v.vector.z;
	}

	public GEControlledValue MulR(GEControlledValue _v)
	{
		GEControlledValue result = new GEControlledValue();
		vector.x *= _v.vector.x;
		vector.y *= _v.vector.y;
		vector.z *= _v.vector.z;
		return result;
	}

	public void One()
	{
		vector = Vector3.one;
		text = string.Empty;
	}

	public void Zero()
	{
		vector = Vector3.zero;
		text = string.Empty;
	}

	public GEControlledValue Modify(GEControlledValue _v, ModifierType _m)
	{
		GEControlledValue gEControlledValue = new GEControlledValue();
		switch (_m)
		{
		case ModifierType.Add:
			return AddR(_v);
		case ModifierType.Sub:
			return SubR(_v);
		case ModifierType.Div:
			return DivR(_v);
		case ModifierType.Mul:
			return MulR(_v);
		default:
			return this;
		}
	}
}

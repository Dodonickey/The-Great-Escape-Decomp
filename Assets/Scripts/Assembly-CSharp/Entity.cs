using System.Collections.Generic;

public class Entity
{
	public int index;

	public List<IComponent> components;

	public bool persistent;
}

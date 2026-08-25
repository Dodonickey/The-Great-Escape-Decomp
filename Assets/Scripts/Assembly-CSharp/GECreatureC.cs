using System.Collections.Generic;

public class GECreatureC : BasicComponent
{
	public CreatureType creatureType;

	public SpritePrefabNode rootNode;

	public GESpritePrefabC SPC;

	public ContactState contactState;

	public MovementState movementState;

	public float contactStateChanged;

	public float movementStateChanged;

	public float health;

	public float maxHealth;

	public float fear;

	public float desire;

	public float determination;

	public float actionSpeed;

	public float runSpeed;

	public float jumpSpeed;

	public float flySpeed;

	public float gripSpeed;

	public GEEffect defensiveAttributes;

	public GEEffect offensiveAttributes;

	public List<GEAffectionC> affections;
}

public class CollisionInterestPair
{
	public ColliderType colliderA;

	public ColliderType colliderB;

	public CollisionEventDelegate collisionDelegate;

	public CollisionInterestPair(ColliderType _colliderA, ColliderType _colliderB, CollisionEventDelegate _collisionDelegate)
	{
		colliderA = _colliderA;
		colliderB = _colliderB;
		collisionDelegate = _collisionDelegate;
	}
}

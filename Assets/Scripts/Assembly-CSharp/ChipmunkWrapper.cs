using System;
using System.Runtime.InteropServices;
using UnityEngine;

internal static class ChipmunkWrapper
{
#if UNITY_IOS || UNITY_IPHONE || UNITY_STANDALONE_OSX
    private const string lookFrom = "__Internal";
#else
    private const string lookFrom = "chipmunk";
#endif

    public static ChipmunkCollisionPair[] beginList = new ChipmunkCollisionPair[100];

	public static ChipmunkCollisionPair[] persistList = new ChipmunkCollisionPair[100];

	public static ChipmunkCollisionPair[] separateList = new ChipmunkCollisionPair[100];

	public static ChipmunkSimpleInfo[] simpleList = new ChipmunkSimpleInfo[20];

	[DllImport(lookFrom)]
	public static extern IntPtr CreateWorld(Vector2 gravity, int iterations, float damping, float sleepTimeTreshold, float collisionSlop);

	[DllImport(lookFrom)]
	public static extern void UpdateWorld(float step);

	[DllImport(lookFrom)]
	public static extern IntPtr GetSpaceStaticBody();

	[DllImport(lookFrom)]
	public static extern void ReIndexAllStatic();

	[DllImport(lookFrom)]
	public static extern void ReIndexShape(IntPtr shape);

	[DllImport(lookFrom)]
	public static extern void ReIndexBody(IntPtr body);

	[DllImport(lookFrom)]
	public static extern void ResizeStaticHash(float dim, int count);

	[DllImport(lookFrom)]
	public static extern void UpdateBodyPosition(IntPtr bodyPtr);

	[DllImport(lookFrom)]
	public static extern void UpdateBodyVelocity(IntPtr bodyPtr);

	[DllImport(lookFrom)]
	public static extern void SetBodyVelocityLimits(IntPtr bodyPtr, float maxLinearVel, float maxAngularVel);

	[DllImport(lookFrom)]
	public static extern IntPtr AddBody(bool isStatic, bool isRogue, Vector2 position, int componentIndex, ColliderType colliderType);

	[DllImport(lookFrom)]
	public static extern IntPtr AddBodyWithCustomProperties(bool isStatic, bool isRogue, Vector2 position, int componentIndex, ColliderType _colliderType, Vector2 linearDamp, float angularDamp, Vector2 gravity);

	[DllImport(lookFrom)]
	public static extern void SetCustomBodyProperties(IntPtr bodyPtr, Vector2 linearDamp, float angularDamp, Vector2 gravity);

	[DllImport(lookFrom)]
	public static extern void SetCustomBodyLinearDamp(IntPtr bodyPtr, Vector2 linearDamp);

	[DllImport(lookFrom)]
	public static extern void SetCustomBodyAngularDamp(IntPtr bodyPtr, float angularDamp);

	[DllImport(lookFrom)]
	public static extern void SetCustomBodyGravity(IntPtr bodyPtr, Vector2 gravity);

	[DllImport(lookFrom)]
	public static extern void SetBodyColliderType(IntPtr bodyPtr, ColliderType colliderType);

	[DllImport(lookFrom)]
	public static extern void RemoveBody(IntPtr bodyPtr);

	[DllImport(lookFrom)]
	public static extern Vector2 GetCenteroidForPoly(int count, Vector2[] vertices);

	[DllImport(lookFrom)]
	public static extern float GetAreaForPoly(int count, Vector2[] vertices);

	[DllImport(lookFrom)]
	public static extern void SetBodyCollisionType(IntPtr bodyPtr, ChipmunkCollisionType _type);

	[DllImport(lookFrom)]
	public static extern void SetShapeCollisionType(IntPtr shapePtr, ChipmunkCollisionType _type);

	[DllImport(lookFrom)]
	public static extern void SetBodyOneWayDirection(IntPtr bodyPtr, Vector2 direction);

	[DllImport(lookFrom)]
	public static extern void SetBodyGroup(IntPtr bodyPtr, uint group);

	[DllImport(lookFrom)]
	public static extern void SetBodyLayers(IntPtr bodyPtr, uint layers);

	[DllImport(lookFrom)]
	public static extern void SetBodySensor(IntPtr bodyPtr, bool sensor);

	[DllImport(lookFrom)]
	public static extern void SetBodySurfaceVelocity(IntPtr bodyPtr, Vector2 vel);

	[DllImport(lookFrom)]
	public static extern IntPtr AddCircleShape(IntPtr bodyPtr, Vector2 offset, float mass, float radius, float restitution, float friction, uint collisionGroup, uint layers, bool sensor);

	[DllImport(lookFrom)]
	public static extern IntPtr AddSegmentShape(IntPtr bodyPtr, Vector2 a, Vector2 b, float radius, float restitution, float friction, uint collisionGroup, uint layers, bool sensor);

	[DllImport(lookFrom)]
	public static extern IntPtr AddPolyShape(IntPtr bodyPtr, Vector2 offset, float mass, int vertexCount, Vector2[] vertices, float restitution, float friction, uint collisionGroup, uint layers, bool sensor);

	[DllImport(lookFrom)]
	public static extern void RemoveShapesFromBody(IntPtr bodyPtr);

	[DllImport(lookFrom)]
	public static extern void SetShapeSurfaceVelocity(IntPtr shapePtr, Vector2 vel);

	[DllImport(lookFrom)]
	public static extern void SetShapeProperties(IntPtr shapePtr, float restitution, float friction);

	[DllImport(lookFrom)]
	public static extern void SetPropertiesForShapesInBody(IntPtr bodyPtr, float restitution, float friction);

	public static IntPtr AddBoxShape(IntPtr bodyPtr, Vector2 offset, float mass, float width, float height, float restitution, float friction, uint collisionGroup, uint layers, bool sensor)
	{
		float num = width / 2f;
		float num2 = height / 2f;
		return AddPolyShape(bodyPtr, offset, mass, 4, new Vector2[4]
		{
			new Vector2(0f - num, 0f - num2),
			new Vector2(0f - num, num2),
			new Vector2(num, num2),
			new Vector2(num, 0f - num2)
		}, restitution, friction, collisionGroup, layers, sensor);
	}

	public static IntPtr AddCircleBody(bool isStatic, bool isRogue, Vector2 position, int componentIndex, Vector2 offset, float mass, float radius, float restitution, float friction, uint collisionGroup, uint layers, bool sensor, ColliderType _colliderType)
	{
		IntPtr intPtr = AddBody(isStatic, isRogue, position, componentIndex, _colliderType);
		AddCircleShape(intPtr, offset, mass, radius, restitution, friction, collisionGroup, layers, sensor);
		return intPtr;
	}

	public static IntPtr AddBoxBody(bool isStatic, bool isRogue, Vector2 position, int componentIndex, Vector2 offset, float mass, float width, float height, float restitution, float friction, uint collisionGroup, uint layers, bool sensor, ColliderType _colliderType)
	{
		IntPtr intPtr = AddBody(isStatic, isRogue, position, componentIndex, _colliderType);
		AddBoxShape(intPtr, offset, mass, width, height, restitution, friction, collisionGroup, layers, sensor);
		return intPtr;
	}

	[DllImport(lookFrom)]
	public static extern IntPtr AddSlideJoint(IntPtr bodyA, IntPtr bodyB, Vector2 offsetA, Vector2 offsetB, float min, float max);

	[DllImport(lookFrom)]
	public static extern IntPtr AddPinJoint(IntPtr bodyA, IntPtr bodyB, Vector2 offsetA, Vector2 offsetB);

	[DllImport(lookFrom)]
	public static extern IntPtr AddPulleyJoint(IntPtr bodyA, IntPtr bodyB, IntPtr bodyC, Vector2 anchorA, Vector2 anchorB, Vector2 anchorCA, Vector2 anchorCB, float ratio);

	[DllImport(lookFrom)]
	public static extern IntPtr AddPivotJoint(IntPtr bodyA, IntPtr bodyB, Vector2 pivotWorldCoord);

	[DllImport(lookFrom)]
	public static extern IntPtr AddPivotJoint2(IntPtr bodyA, IntPtr bodyB, Vector2 offsetA, Vector2 offsetB);

	[DllImport(lookFrom)]
	public static extern IntPtr AddGrooveJoint(IntPtr bodyA, IntPtr bodyB, Vector2 grooveA, Vector2 grooveB, Vector2 anchor2);

	[DllImport(lookFrom)]
	public static extern IntPtr AddRotaryLimitJoint(IntPtr bodyA, IntPtr bodyB, float angleMin, float angleMax);

	[DllImport(lookFrom)]
	public static extern IntPtr AddDampedSpring(IntPtr bodyA, IntPtr bodyB, Vector2 offsetA, Vector2 offsetB, float restLength, float stiffness, float damping);

	[DllImport(lookFrom)]
	public static extern IntPtr AddDampedSpring2(IntPtr bodyA, IntPtr bodyB, Vector2 worldPos, float restLength, float stiffness, float damping);

	[DllImport(lookFrom)]
	public static extern IntPtr AddDampedRotarySpring(IntPtr bodyA, IntPtr bodyB, float restAngle, float stiffness, float damping);

	[DllImport(lookFrom)]
	public static extern IntPtr AddGearJoint(IntPtr bodyA, IntPtr bodyB, float phase, float ratio);

	[DllImport(lookFrom)]
	public static extern IntPtr AddSimpleMotor(IntPtr bodyA, IntPtr bodyB, float rate, float maxForce);

	[DllImport(lookFrom)]
	public static extern void RemoveConstraint(IntPtr constraintPtr);

	[DllImport(lookFrom)]
	public static extern void RemoveConstraintsFromBody(IntPtr bodyPtr);

	[DllImport(lookFrom)]
	public static extern void SetConstraintProperties(IntPtr constraint, float biasCoef, float maxBias, float maxForce);

	[DllImport(lookFrom)]
	public static extern void SetSlideJointProperties(IntPtr constraint, float minLength, float maxLength);

	[DllImport(lookFrom)]
	public static extern void SetMotorProperties(IntPtr constraint, float rate, float maxForce);

	[DllImport(lookFrom)]
	public static extern void SetDampedSpringProperties(IntPtr constraint, float stiffness, float damping, float restLength);

	[DllImport(lookFrom)]
	public static extern void GetDampedSpringProperties(IntPtr constraint, ref ChipmunkDampedSpringStruct springRef);

	[DllImport(lookFrom)]
	public static extern void SetDampedRotarySpringProperties(IntPtr constraint, float stiffness, float damping, float restAngle);

	[DllImport(lookFrom)]
	public static extern void GetDampedRotarySpringProperties(IntPtr constraint, ref ChipmunkDampedRotarySpringStruct springRef);

	[DllImport(lookFrom)]
	public static extern void SetPinJointProperties(IntPtr constraint, Vector2 offsetA, Vector2 offsetB, float distance);

	[DllImport(lookFrom)]
	public static extern void GetPivotJointProperties(IntPtr constraint, ref ChipmunkPivotJointStruct jointRef);

	[DllImport(lookFrom)]
	public static extern void SetPivotJointProperties(IntPtr constraint, Vector2 offsetA, Vector2 offsetB);

	[DllImport(lookFrom)]
	public static extern void SetPivotJointOffsetA(IntPtr constraint, Vector2 offsetA);

	[DllImport(lookFrom)]
	public static extern void SetPivotJointOffsetB(IntPtr constraint, Vector2 offsetB);

	[DllImport(lookFrom)]
	public static extern void SetSlideJointOffsetA(IntPtr constraint, Vector2 offsetA);

	[DllImport(lookFrom)]
	public static extern void SetSlideJointOffsetB(IntPtr constraint, Vector2 offsetB);

	[DllImport(lookFrom)]
	public static extern void SetDampedSpringOffsetA(IntPtr constraint, Vector2 offsetA);

	[DllImport(lookFrom)]
	public static extern void SetDampedSpringOffsetB(IntPtr constraint, Vector2 offsetB);

	[DllImport(lookFrom)]
	public static extern void SetRotaryLimitJointProperties(IntPtr constraint, float min, float max);

	public static void AddPivotJointWithRotarySpring(IntPtr bodyA, IntPtr bodyB, Vector2 pivotPos, float restAngle, float stiffness, float damping)
	{
		AddPivotJoint(bodyA, bodyB, pivotPos);
		AddDampedRotarySpring(bodyA, bodyB, restAngle, stiffness, damping);
	}

	[DllImport(lookFrom)]
	public static extern void GetBodyValues(IntPtr bodyPtr, ref ChipmunkBodyStruct bodyStructRef);

	[DllImport(lookFrom)]
	public static extern Vector2 GetLocalPos(IntPtr bodyPtr, Vector2 worldPos);

	[DllImport(lookFrom)]
	public static extern Vector2 GetWorldPos(IntPtr bodyPtr, Vector2 localPos);

	[DllImport(lookFrom)]
	public static extern void SetPosition(IntPtr bodyPtr, Vector2 position);

	[DllImport(lookFrom)]
	public static extern void SetAngle(IntPtr bodyPtr, float angle);

	[DllImport(lookFrom)]
	public static extern void SetVelocity(IntPtr bodyPtr, Vector2 vel);

	[DllImport(lookFrom)]
	public static extern void SetXVelocity(IntPtr bodyPtr, float velX);

	[DllImport(lookFrom)]
	public static extern void SetYVelocity(IntPtr bodyPtr, float velY);

	[DllImport(lookFrom)]
	public static extern void SetAngularVelocity(IntPtr bodyPtr, float aVel);

	[DllImport(lookFrom)]
	public static extern void ResetForces(IntPtr bodyPtr);

	[DllImport(lookFrom)]
	public static extern void ApplyForce(IntPtr bodyPtr, Vector2 f, Vector2 r, bool globalCoordinates);

	[DllImport(lookFrom)]
	public static extern void ApplyImpulse(IntPtr bodyPtr, Vector2 j, Vector2 r, bool globalCoordinates);

	[DllImport(lookFrom)]
	public static extern void ActivateBody(IntPtr bodyPtr);

	[DllImport(lookFrom)]
	public static extern void SleepBody(IntPtr bodyPtr);

	[DllImport(lookFrom)]
	public static extern void AddCollisionInterestPair(ChipmunkCollisionList collisionList, ColliderType colliderType1, ColliderType colliderType2);

	[DllImport(lookFrom)]
	public static extern void ClearCollisionInterestPairs();

	[DllImport(lookFrom)]
	public static extern int GetCollisionInterestCount(ChipmunkCollisionList collisionList);

	[DllImport(lookFrom)]
	public static extern int GetCollisionInterestList(ChipmunkCollisionList collisionList, ChipmunkCollisionPair[] collisionPairStructArray);

	[DllImport(lookFrom)]
	public static extern int GetCollisionCount(ChipmunkCollisionList collisionList);

	[DllImport(lookFrom)]
	public static extern int GetCollisionList(ChipmunkCollisionList colList, ChipmunkCollisionPair[] collisionPairStructArray);

	[DllImport(lookFrom)]
	public static extern int GetBodyCollisions(IntPtr bodyPtr, ChipmunkSimpleInfo[] simpleInfoStructArray);

	[DllImport(lookFrom)]
	public static extern bool AreBodiesColliding(IntPtr bodyPtr1, IntPtr bodyPtr2);

	[DllImport(lookFrom)]
	public static extern void SegmentQuery(Vector2 start, Vector2 end, uint group, uint layers, ref ChipmunkSegmentQueryInfo result);

	[DllImport(lookFrom)]
	public static extern int BBQuery(Vector2 dimensions, Vector2 offset, uint group, uint layers, ChipmunkQueryInfo[] result);

	[DllImport(lookFrom)]
	public static extern int PointQuery(Vector2 point, uint group, uint layers, ChipmunkQueryInfo[] result);

	[DllImport(lookFrom)]
	public static extern int ShapeQuery(IntPtr shape, ChipmunkQueryInfo[] result);

	[DllImport(lookFrom)]
	public static extern int BodyQuery(IntPtr body, ChipmunkQueryInfo[] result);

	[DllImport(lookFrom)]
	public static extern int GetConnectedBodies(IntPtr body, ChipmunkQueryInfo[] result);

	[DllImport(lookFrom)]
	public static extern int GetBodyConstraints(IntPtr body, ChipmunkConstraintQueryInfo[] result);
}

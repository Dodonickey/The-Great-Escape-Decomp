using UnityEngine;

public class UnityBootstrap : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((bool)other.attachedRigidbody)
		{
			other.attachedRigidbody.AddForce(Vector3.up * 10f);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if ((bool)other.attachedRigidbody)
		{
			other.attachedRigidbody.AddForce(Vector3.up * 10f);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((bool)other.attachedRigidbody)
		{
			other.attachedRigidbody.AddForce(Vector3.up * 10f);
		}
	}

	private void OnCollisionEnter(Collision collisionInfo)
	{
		ContactPoint[] contacts = collisionInfo.contacts;
		foreach (ContactPoint contactPoint in contacts)
		{
		}
	}

	private void OnCollisionStay(Collision collisionInfo)
	{
		ContactPoint[] contacts = collisionInfo.contacts;
		foreach (ContactPoint contactPoint in contacts)
		{
		}
	}

	private void OnCollisionExit(Collision collisionInfo)
	{
		ContactPoint[] contacts = collisionInfo.contacts;
		foreach (ContactPoint contactPoint in contacts)
		{
		}
	}

	private void Update()
	{
	}
}

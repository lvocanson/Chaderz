using System;
using UnityEngine;

namespace Tanks.Complete
{
	public class ShellExplosion : MonoBehaviour
	{
		public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
		[HideInInspector] public float m_MaxLifeTime = 2f;  // The time in seconds before the shell is removed.

		// All those are hidden in inspector as they will actually come from the TankShooting scripts
		[HideInInspector] public float m_MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
		[HideInInspector] public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
		[HideInInspector] public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.

		private void Start()
		{
			// If it isn't destroyed by then, destroy the shell after its lifetime.
			Destroy(gameObject, m_MaxLifeTime);
		}

		public Rigidbody _rb;
		public GameObject _explosionPrefab;
		public float TimeDilationStartDistance;
		public LayerMask TimeDilationTarget;
		public AnimationCurve TimeDilationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		private void Reset()
		{
			_rb = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (Physics.Raycast(_rb.position, Vector3.ProjectOnPlane(_rb.velocity, Vector3.up), out RaycastHit info, TimeDilationStartDistance, TimeDilationTarget))
			{
				var health = info.collider.GetComponent<TankHealth>();
				if (health && health.m_CurrentHealth < m_MaxDamage)
				{
					float t = 1f - info.distance / TimeDilationStartDistance;
					Time.timeScale = TimeDilationCurve.Evaluate(t);
					MaterialManager.Instance.UpdateShellPosition(gameObject, true);
				}
			}
			else
			{
				Time.timeScale = TimeDilationCurve.Evaluate(0f);
				MaterialManager.Instance.UpdateShellPosition(gameObject, false);
			}
		}

		private void OnDestroy()
		{
			Time.timeScale = TimeDilationCurve.Evaluate(0f);
			MaterialManager.Instance.UpdateShellPosition(gameObject, false);
		}

		private void OnTriggerEnter(Collider other)
		{
			// Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
			Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

			// Go through all the colliders...
			for (int i = 0; i < colliders.Length; i++)
			{
				// ... and find their rigidbody.
				Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

				// If they don't have a rigidbody, go on to the next collider.
				if (!targetRigidbody)
					continue;

				// Add an explosion force.
				targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

				// Find the TankHealth script associated with the rigidbody.
				TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

				// If there is no TankHealth script attached to the gameobject, go on to the next collider.
				if (!targetHealth)
					continue;

				// Calculate the amount of damage the target should take based on it's distance from the shell.
				float damage = CalculateDamage(targetRigidbody.position);

				// Deal this damage to the tank.
				targetHealth.TakeDamage(damage);
			}

			// Destroy the shell.
			Destroy(gameObject);

			var go = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
			Destroy(go, 10f);
		}


		private float CalculateDamage(Vector3 targetPosition)
		{
			// Create a vector from the shell to the target.
			Vector3 explosionToTarget = targetPosition - transform.position;

			// Calculate the distance from the shell to the target.
			float explosionDistance = explosionToTarget.magnitude;

			// Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
			float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

			// Calculate damage as this proportion of the maximum possible damage.
			float damage = relativeDistance * m_MaxDamage;

			// Make sure that the minimum damage is always 0.
			damage = Mathf.Max(0f, damage);

			return damage;
		}
	}
}
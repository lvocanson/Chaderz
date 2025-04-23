using System.Collections;
using Tanks.Complete;
using UnityEngine;
using UnityEngine.VFX;

public class Lightning : MonoBehaviour
{
    [SerializeField]
    GameObject _light, _vfx;

    [SerializeField]
    float _lightDelay, _timeToLightMaxRange, _lightMaxRange;
    
    public LayerMask m_TankMask;
    public float m_MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
    public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
    public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
    private void Start()
    {
        StartCoroutine(LightningCoroutine());
    }

    private IEnumerator LightningCoroutine()
    {
        _light.SetActive(true);
        float timer = 0;
        while (timer < _lightDelay)
        {
            _light.GetComponent<Light>().range = Mathf.Lerp(0, _lightMaxRange, timer / (_lightDelay - _timeToLightMaxRange));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        GetComponent<SphereCollider>().enabled = true;
        _vfx.SetActive(true);
        yield return new WaitForSeconds(_vfx.GetComponent<VisualEffect>().GetFloat("_lifeTime"));
        Destroy(gameObject);
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

using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 100f;
    public GameObject explosionParticleSystem;

    private bool exploded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Ground"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        exploded = true;
        
        // Ativar o sistema de part�culas
        if (explosionParticleSystem != null)
        {
            GameObject particleSystemGO = Instantiate(explosionParticleSystem, transform.position, Quaternion.identity);
            Destroy(particleSystemGO, 3f);
        }

        // Aplicar for�a explosiva
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Desativar o objeto do proj�til
        gameObject.SetActive(false);
    }
}
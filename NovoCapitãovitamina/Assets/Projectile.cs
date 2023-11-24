using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float initialSpeed = 10f;
    public float bounceFactor = 0.8f;
    public float impulseStrength = 5f;
    public float detectionRadius = 2f;
    public LayerMask enemyLayer; // Certifique-se de configurar corretamente as layers no Unity.
    private bool hasBounced = false;

    private void Start()
    {
        // Adiciona velocidade inicial

        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se colidiu com o chão
        if (other.CompareTag("Ground") && !hasBounced)
        {
            // Inverte a componente vertical da velocidade para simular o quique
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, -GetComponent<Rigidbody>().velocity.y * bounceFactor, GetComponent<Rigidbody>().velocity.z);
            hasBounced = true;

            // Verifica inimigos na área
            CheckForEnemiesInRadius(transform.position, detectionRadius);
        }
        if (other.CompareTag("Parede") && !hasBounced)
        {
            // Inverte a componente vertical da velocidade para simular o quique
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, -GetComponent<Rigidbody>().velocity.y * bounceFactor, GetComponent<Rigidbody>().velocity.z);
            hasBounced = true;

            // Verifica inimigos na área
            CheckForEnemiesInRadius(transform.position, detectionRadius);
        }
    }

    private void CheckForEnemiesInRadius(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemyLayer);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            // Inimigo encontrado, aplica impulso
            ApplyImpulseToEnemy(hitColliders[i].gameObject);
        }
    }

    private void ApplyImpulseToEnemy(GameObject enemy)
    {
        // Calcula vetor de impulso apenas no eixo X em direção ao inimigo
        Vector3 impulseVector = (enemy.transform.position - transform.position).normalized;
        impulseVector.y = 0; // Zera a componente y para manter o movimento no plano horizontal.
        impulseVector.Normalize(); // Normaliza novamente após zerar a componente y
        impulseVector *= impulseStrength;

        // Adiciona impulso ao projétil
        GetComponent<Rigidbody>().velocity = impulseVector;


    }


}
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
  
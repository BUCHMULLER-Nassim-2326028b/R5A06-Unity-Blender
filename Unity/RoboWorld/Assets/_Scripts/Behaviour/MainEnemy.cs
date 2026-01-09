using UnityEngine;
using UnityEngine.AI;

public class MainEnemy : MonoBehaviour
{
    [Header("Detection Settings")]
    public float rayonDetection = 10f;
    public float rayonPromenade = 20f;
    
    [Header("Movement Settings")]
    public float vitessePromenade = 3.5f;
    public float vitessePoursuite = 6.0f;

    [Header("Death & Loot")]
    public GameObject[] itemReward; 
    public GameObject effetExplosion;
    [Tooltip("Determine if the hit comes from high enough to squash (0.7 = very vertical)")]
    public float seuilEcrasement = 0.7f; 

    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        AllerAuHasard();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Chase state
        if (distance <= rayonDetection)
        {
            agent.speed = vitessePoursuite;
            agent.SetDestination(player.position);
        }
        // Patrol state
        else
        {
            agent.speed = vitessePromenade;

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                AllerAuHasard();
            }
        }
    }

    /// <summary>
    /// Find a random point on the NavMesh.
    /// </summary>
    void AllerAuHasard()
    {
        Vector3 randomDirection = Random.insideUnitSphere * rayonPromenade;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, rayonPromenade, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if player is above the enemy
            if (collision.transform.position.y > transform.position.y + 0.5f)
            {
                Rigidbody rbPlayer = collision.gameObject.GetComponent<Rigidbody>();
                if (rbPlayer != null)
                {
                    Vector3 v = rbPlayer.velocity;
                    v.y = 10f; // Bounce force
                    rbPlayer.velocity = v;
                }

                Mourir();
            }
            else
            {
                // Player is hit
                if (GameManager.instance != null)
                {
                    GameManager.instance.ModifierEnergie(-1);
                }

                PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
                if (pm != null)
                {
                    Vector3 dir = (collision.transform.position - transform.position).normalized;
                    dir.y = 0.3f;
                    pm.ApplyKnockback(dir * 20f);
                }
            }
        }
    }

    void Mourir()
    {
        if (effetExplosion != null)
        {
            Instantiate(effetExplosion, transform.position, Quaternion.identity);
        }

        if (itemReward != null && itemReward.Length > 0)
        {
            int index = Random.Range(0, itemReward.Length);
            GameObject loot = itemReward[index];
            
            if (loot != null)
            {
                Instantiate(loot, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rayonDetection);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rayonPromenade);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    [SerializeField] private float speed = default;    
    [SerializeField] float explosionRange = default;
    [SerializeField] float deathRange = default;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float sqrRange = default;

    private float selfDestructTime = 7.0f;
    private float selfDestructTimer = 0.0f;


    private Rigidbody bombRB;
    private CollisionHandler player;
    private LayerMask playerMask;

    // Start is called before the first frame update
    void Start()
    {
        this.bombRB = this.gameObject.GetComponent<Rigidbody>();
        this.player = GameObject.FindObjectOfType<CollisionHandler>();

        this.playerMask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        this.selfDestructTimer += Time.deltaTime;

        if(this.selfDestructTimer >= this.selfDestructTime)
            GameObject.Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        this.bombRB.MovePosition(this.bombRB.position + this.gameObject.transform.forward * this.speed * Time.fixedDeltaTime);

        Vector3 currentPos = this.gameObject.transform.position;
        float currentSqrMagnitude = (this.player.transform.position - currentPos).sqrMagnitude;

        if (currentSqrMagnitude <= this.sqrRange)
        {
            AudioSource.PlayClipAtPoint(this.explosionSFX, this.gameObject.transform.position);
            GameObject explosion = Instantiate(this.explosionVFX, this.gameObject.transform.position, Quaternion.identity);

            Collider[] player = Physics.OverlapSphere(this.gameObject.transform.position, this.explosionRange, this.playerMask);
            Collider[] playerDeath = Physics.OverlapSphere(this.gameObject.transform.position, this.deathRange, this.playerMask);

            if (player.Length > 0)
            {
                Health playerHealth = this.player.GetComponent<Health>();
                playerHealth.DecreaseHitPoints((int)(playerHealth.PlayerMaxHitPoints / 2));

                if(playerDeath.Length > 0)
                    playerHealth.DecreaseHitPoints((int)playerHealth.PlayerMaxHitPoints);
            }
                

            GameObject.Destroy(explosion, 1.49f);
            GameObject.Destroy(this.gameObject, 1.5f);
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.GetComponentInParent<Health>().DecreaseHitPoints(100);
    }
}

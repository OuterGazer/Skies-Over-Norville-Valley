using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    [SerializeField] private float speed = default;    
    [SerializeField] float explosionRange = default;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float sqrRange = default;

    private float selfDestructTime = 7.0f;
    private float selfDestructTimer = 0.0f;


    private Rigidbody bombRB;
    private CollisionHandler player;

    // Start is called before the first frame update
    void Start()
    {
        this.bombRB = this.gameObject.GetComponent<Rigidbody>();
        this.player = GameObject.FindObjectOfType<CollisionHandler>();
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

            /*Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, this.explosionRange, this.enemyMask);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Health>().ExplodeEnemy();
            }*/

            GameObject.Destroy(explosion, 1.49f);
            GameObject.Destroy(this.gameObject, 1.5f);
        }
            
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.GetComponentInParent<Health>().DecreaseHitPoints(1);
    }*/
}

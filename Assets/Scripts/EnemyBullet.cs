using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = default;
    
    private float range = default;
    private float bulletLifetime = 1.5f;
    private float bulletLifetimeCounter = 0f;


    private Rigidbody bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        this.bulletRB = this.gameObject.GetComponent<Rigidbody>();

        this.range = GameObject.FindObjectOfType<CollisionHandler>().transform.position.y + 200f;
    }

    
    void FixedUpdate()
    {
        this.bulletLifetimeCounter += Time.fixedDeltaTime;

        this.bulletRB.MovePosition(this.bulletRB.position + this.gameObject.transform.forward * this.speed * Time.fixedDeltaTime);

        if ((this.gameObject.transform.position.y >= this.range) || (this.bulletLifetimeCounter >= this.bulletLifetime))
            GameObject.Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.GetComponentInParent<Health>().DecreaseHitPoints(1);
    }
}

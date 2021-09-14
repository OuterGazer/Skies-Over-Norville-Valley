using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float verticalDropForce;
    [SerializeField] float horizontalDropForce = default;
    [SerializeField] float explosionRange = default;
    [SerializeField] AudioClip fallingSFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] GameObject explosionVFX;

    private Rigidbody bombRB;
    private LayerMask groundAndEnemyMask;
    private LayerMask enemyMask;

    // Start is called before the first frame update
    void Start()
    {
        this.bombRB = this.gameObject.GetComponent<Rigidbody>();

        this.groundAndEnemyMask = LayerMask.GetMask("Default", "Water", "Enemy");
        this.enemyMask = LayerMask.GetMask("Enemy");

        if(GameObject.FindObjectsOfType<Bomb>().Length < 2)
        {
            this.gameObject.GetComponent<AudioSource>().dopplerLevel = 0;
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.fallingSFX);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckGroundOrEnemyBelow();

        AddDropForce();
    }

    private void CheckGroundOrEnemyBelow()
    {
        bool hasHitGround = Physics.Raycast(this.gameObject.transform.position, Vector3.down, 1.0f, this.groundAndEnemyMask);

        if (hasHitGround)
        {
            AudioSource.PlayClipAtPoint(this.explosionSFX, this.gameObject.transform.position);
            GameObject explosion = Instantiate(this.explosionVFX, this.gameObject.transform.position, Quaternion.identity);

            Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, this.explosionRange, this.enemyMask);
            
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Health>().ExplodeEnemy();
            }

            GameObject.Destroy(explosion, 1.49f);
            GameObject.Destroy(this.gameObject, 1.5f);
        }
            
    }

    private void AddDropForce()
    {
        this.bombRB.AddForce(Vector3.down * this.verticalDropForce, ForceMode.Acceleration);
        this.bombRB.AddRelativeForce(Vector3.forward * this.horizontalDropForce, ForceMode.Acceleration);
    }
}

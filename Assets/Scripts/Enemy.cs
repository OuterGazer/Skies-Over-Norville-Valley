using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int hitScoreAmount = default;
    [SerializeField] int killScoreAmount = default;

    [SerializeField] GameObject explosionVFX;


    private Score score;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        this.score = GameObject.FindObjectOfType<Score>();
        this.health = this.gameObject.GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyGetsDestroyedByPlayer(other);
    }

    private void EnemyGetsDestroyedByPlayer(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (this.health.IsAlive)
            {
                HitEnemy();
            }
            else
            {
                KillEnemy();
            }
        }
    }

    

    private void HitEnemy()
    {
        this.score.IncreaseScore(this.hitScoreAmount);
        this.health.DecreaseHitPoints(1);
    }

    private void KillEnemy()
    {
        this.score.IncreaseScore(this.killScoreAmount);

        GameObject enemyCrash = Instantiate<GameObject>(this.explosionVFX,
                                                    new Vector3(this.transform.position.x, this.transform.position.y - 1.1f, this.transform.position.z),
                                                    Quaternion.identity);
        ProcessExplosion();

        DestroyEnemy(enemyCrash);
    }

    private void ProcessExplosion()
    {
        this.gameObject.AddComponent<Rigidbody>();

        this.gameObject.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(5, 20),
                                                                    new Vector3(Random.Range(this.gameObject.transform.position.x - 3, this.gameObject.transform.position.x + 3),
                                                                                Random.Range(this.gameObject.transform.position.y - 3, this.gameObject.transform.position.y + 3),
                                                                                Random.Range(this.gameObject.transform.position.z - 3, this.gameObject.transform.position.z + 3)),
                                                                    10f, Random.Range(1f, 10f), ForceMode.Impulse);

        this.gameObject.gameObject.transform.parent.transform.SetParent(null);
    }

    private void DestroyEnemy(GameObject enemyCrash)
    {
        this.gameObject.GetComponent<MeshCollider>().enabled = false;
        GameObject.Destroy(this.gameObject.gameObject.transform.parent.gameObject, 1.0f);
        GameObject.Destroy(enemyCrash, 1.0f);
    }
}
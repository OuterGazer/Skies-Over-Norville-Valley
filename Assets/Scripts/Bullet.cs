using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = default;
    [SerializeField] private float shootRange = default;
    [SerializeField] GameObject enemyAircraftExplosionVFX;

    private float originZ;
    private float originX;

    private Rigidbody bulletRB;
    private TrailRenderer bulletTrail;
    public void EmmitTrail(bool shouldEmmit)
    {
        this.bulletTrail.emitting = shouldEmmit;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        this.bulletRB = this.gameObject.GetComponent<Rigidbody>();
        this.bulletTrail = this.gameObject.GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        this.bulletRB.MovePosition(this.bulletRB.position + this.gameObject.transform.forward * this.bulletSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if(Mathf.Abs(this.gameObject.transform.position.z - this.originZ) >= this.shootRange ||
           Mathf.Abs(this.gameObject.transform.position.x - this.originX) >= this.shootRange)
        {
            StartCoroutine(EngageToParent(GameObject.FindWithTag("Ammo Holder").transform));
        }
    }

    public void DisengageFromParent()
    {
        this.gameObject.transform.SetParent(null);
        this.originZ = this.gameObject.transform.position.z;
        this.originX = this.gameObject.transform.position.x;
    }

    public IEnumerator EngageToParent(Transform parent)
    {
        EmmitTrail(false);

        yield return null;

        this.gameObject.transform.SetParent(parent);
        this.bulletTrail.Clear();        

        yield return null;

        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
    }


    // Physics Callbacks ====================================================================================================================

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject enemyCrash = Instantiate<GameObject>(this.enemyAircraftExplosionVFX,
                                                            new Vector3(other.transform.position.x, other.transform.position.y - 1.1f, other.transform.position.z),
                                                            Quaternion.identity);
            other.gameObject.AddComponent<Rigidbody>();            
            
            other.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(5, 20),
                                        new Vector3(Random.Range(other.transform.position.x - 3, other.transform.position.x + 3),
                                                    Random.Range(other.transform.position.y - 3, other.transform.position.y + 3),
                                                    Random.Range(other.transform.position.z - 3, other.transform.position.z + 3)),
                                        10f, Random.Range(1f, 10f), ForceMode.Impulse);

            other.gameObject.transform.parent.transform.SetParent(null);

            other.GetComponent<MeshCollider>().enabled = false;
            GameObject.Destroy(other.gameObject.transform.parent.gameObject, 1.0f);
            GameObject.Destroy(enemyCrash, 1.0f);
        }
    }
}

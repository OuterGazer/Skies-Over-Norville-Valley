using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = default;
    [SerializeField] private float shootRange = default;

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
            //Debug.Log("Enemy hit!");
            GameObject.Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}

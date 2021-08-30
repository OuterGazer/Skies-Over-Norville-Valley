using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = default;
    [SerializeField] private float shootRange = default;

    private float originZ;

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
        //this.gameObject.GetComponent<TrailRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        this.bulletRB.MovePosition(this.bulletRB.position + this.gameObject.transform.forward * this.bulletSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if(Mathf.Abs(this.gameObject.transform.position.z - this.originZ) >= this.shootRange)
        {
            SetParent(GameObject.FindWithTag("Ammo Holder").transform);            
            this.gameObject.SetActive(false);
        }
    }

    public void DisengageFromParent()
    {
        this.gameObject.transform.SetParent(null);
        this.originZ = Mathf.Abs(this.gameObject.transform.position.z);
    }

    public void SetParent(Transform parent)
    {
        EmmitTrail(false);
        this.gameObject.transform.position = Vector3.zero;        
        this.gameObject.transform.SetParent(parent);        
    }
}

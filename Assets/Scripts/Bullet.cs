using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = default;

    Rigidbody bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        this.bulletRB = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.bulletRB.MovePosition(this.bulletRB.position + this.gameObject.transform.forward * this.bulletSpeed * Time.fixedDeltaTime);
    }
}

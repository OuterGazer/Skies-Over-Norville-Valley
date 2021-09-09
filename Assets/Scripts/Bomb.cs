using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private LayerMask groundAndEnemy;

    // Start is called before the first frame update
    void Start()
    {
        this.groundAndEnemy = LayerMask.GetMask("Default", "Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        bool hasHitGround = Physics.Raycast(this.gameObject.transform.position, Vector3.down, 1.0f, this.groundAndEnemy);

        if (hasHitGround)
            GameObject.Destroy(this.gameObject);
    }
}

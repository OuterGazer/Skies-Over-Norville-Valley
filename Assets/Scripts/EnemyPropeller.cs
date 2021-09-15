using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPropeller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 2000) * Time.deltaTime);
    }
}

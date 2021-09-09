using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stray bomb!");
        if (other.GetComponent<Bomb>() != null)
            GameObject.Destroy(other.gameObject);
    }
}

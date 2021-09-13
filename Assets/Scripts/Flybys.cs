using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flybys : MonoBehaviour
{
    [SerializeField] AudioClip bulletFlybySFX;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = GameObject.FindObjectOfType<CollisionHandler>().GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy Bullet"))
            this.audioSource.PlayOneShot(this.bulletFlybySFX);
    }
}

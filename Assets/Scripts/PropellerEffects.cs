using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerEffects : MonoBehaviour
{
    [SerializeField] AudioClip engineStartSFX;

    private AudioSource audioSource;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.audioSource.PlayOneShot(this.engineStartSFX);

        yield return new WaitUntil(() => !this.audioSource.isPlaying);

        this.audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

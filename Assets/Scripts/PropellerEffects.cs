using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerEffects : MonoBehaviour
{
    [SerializeField] AudioClip engineStartSFX;
    [SerializeField] float propellerMaxSpeed;

    private float propellerSpeed;

    private AudioSource audioSource;

    private bool isStarting = true;

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
        if (this.isStarting)
        {
            this.propellerSpeed += (Time.deltaTime * 200);

            this.gameObject.transform.rotation *= Quaternion.Euler(new Vector3(0, this.propellerSpeed, 0) * Time.deltaTime);

            if (this.propellerSpeed >= this.propellerMaxSpeed)
                this.isStarting = false;
        }
        else
        {
            this.gameObject.transform.rotation *= Quaternion.Euler(new Vector3(0, this.propellerMaxSpeed, 0) * Time.deltaTime);
        }        
    }
}

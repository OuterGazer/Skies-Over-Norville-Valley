using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerActivation : MonoBehaviour
{
    private PlayableDirector masterTimeline;

    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private GameObject crosshair;

    private AudioSource levelMusic;

    // Start is called before the first frame update
    void Start()
    {
        this.masterTimeline = this.gameObject.GetComponent<PlayableDirector>();

        this.playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        this.playerShooting = GameObject.FindObjectOfType<PlayerShooting>();
        this.crosshair = GameObject.Find("Crosshair");
        this.crosshair.SetActive(false);

        this.levelMusic = GameObject.FindWithTag("Music Player").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.masterTimeline.time >= 7.00)
        {
            this.playerMovement.enabled = true;
            this.playerShooting.enabled = true;
            this.crosshair.SetActive(true);

            if (!this.levelMusic.enabled)
                this.levelMusic.enabled = true;

            this.enabled = false;
        }
    }
}

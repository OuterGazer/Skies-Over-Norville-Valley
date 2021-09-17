using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class GameLoop : MonoBehaviour
{
    private PlayableDirector timeline;
    [SerializeField] InputAction enableDisableCrosshair;
    [SerializeField] SpriteRenderer crosshair;
    [SerializeField] InputAction pause;
    [SerializeField] InputAction quit; 

    private bool firstTeleport = false;

    private void OnEnable()
    {
        this.enableDisableCrosshair.Enable();
        this.pause.Enable();
        this.quit.Enable();
    }
    private void OnDisable()
    {
        this.enableDisableCrosshair.Disable();
        this.pause.Disable();
        this.quit.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.timeline = this.gameObject.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        LoopBackToBeginning();

        ProcessControls();
    }

    private void LoopBackToBeginning()
    {
        /*if(this.timeline.time >= 3 && !this.firstTeleport)
                {
                    this.timeline.time = 40;
                    this.firstTeleport = true;
                }
                else*/
        if (this.timeline.time >= 55.9)
        {
            this.timeline.time = 7.33;
        }
    }

    private void ProcessControls()
    {
        if (this.enableDisableCrosshair.triggered)
            this.crosshair.enabled = !this.crosshair.enabled;

        if (this.pause.triggered)
        {
            if (Time.timeScale != 0)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        if (this.quit.triggered)
            Application.Quit();
    }
}

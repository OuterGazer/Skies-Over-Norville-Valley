using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameLoop : MonoBehaviour
{
    private PlayableDirector timeline;

    private bool firstTeleport = false;

    // Start is called before the first frame update
    void Start()
    {
        this.timeline = this.gameObject.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        LoopBackToBeginning();
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
}

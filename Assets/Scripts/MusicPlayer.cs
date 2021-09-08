using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        int musicPlayerAmount = GameObject.FindObjectsOfType<MusicPlayer>().Length;

        if(musicPlayerAmount > 1)
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }
}

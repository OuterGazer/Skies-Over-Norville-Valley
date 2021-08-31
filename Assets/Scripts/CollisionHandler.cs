using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private int loadDelay = default;


    private bool isAlive = true;
    public bool IsAlive => this.isAlive;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + " collided with " + other.gameObject.name);
        StartCoroutine(ProcessPlayerDeath());
    }

    private IEnumerator ProcessPlayerDeath()
    {
        this.isAlive = false;
        GameObject.Destroy(this.gameObject, 1.5f);

        yield return new WaitForSeconds(this.loadDelay);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    /*private void OnCollisionEnter(Collision other)
    {
        Debug.Log(this.gameObject.name + " collided with " + other.gameObject.name);
    }*/
}

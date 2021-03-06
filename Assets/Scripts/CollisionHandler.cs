using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private int loadDelay = default;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioClip bullethitSFX;
    [SerializeField] GameObject bulletSparksVFX;

    private Rigidbody playerRB;
    private AudioSource audioSource;

    private void Start()
    {
        this.playerRB = this.gameObject.GetComponent<Rigidbody>();
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy Bullet"))
        {
            StartCoroutine(ProcessPlayerDeath());
        }            
        else
        {
            this.audioSource.PlayOneShot(this.bullethitSFX);
            GameObject sparks = Instantiate<GameObject>(this.bulletSparksVFX, other.transform.position, Quaternion.identity);
            GameObject.Destroy(sparks, 1.5f);
        }
    }

    public IEnumerator ProcessPlayerDeath()
    {
        ExplodePlayerShip();

        yield return new WaitForSeconds(this.loadDelay);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void ExplodePlayerShip()
    {
        this.explosionVFX.SetActive(true);
        AudioSource.PlayClipAtPoint(this.explosionSFX, Camera.main.transform.position);
        this.gameObject.GetComponent<PlayerShooting>().enabled = false;
        this.gameObject.GetComponent<PlayerMovement>().enabled = false;

        this.playerRB.useGravity = true;
        this.playerRB.mass = 100f;
        this.playerRB.AddExplosionForce(Random.Range(5, 20),
                                        new Vector3(Random.Range(this.gameObject.transform.position.x - 3f, this.gameObject.transform.position.x + 3f),
                                                    Random.Range(this.gameObject.transform.position.y - 3f, this.gameObject.transform.position.y + 3f),
                                                    Random.Range(this.gameObject.transform.position.z - 3f, this.gameObject.transform.position.z + 3f)),
                                        10f, Random.Range(1f, 10f), ForceMode.Impulse);
    }
}

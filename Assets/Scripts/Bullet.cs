using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float bulletSpeed = default;
    [SerializeField] private float shootRange = default;

    [Header("Effect Settings")]
    [SerializeField] private GameObject bulletSparksVFX;
    [SerializeField] private AudioClip hitSFX;

    private float originZ;
    private float originX;

    private AudioSource audioSource;
    private Rigidbody bulletRB;
    private TrailRenderer bulletTrail;
    public void EmmitTrail(bool shouldEmmit)
    {
        this.bulletTrail.emitting = shouldEmmit;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        this.bulletRB = this.gameObject.GetComponent<Rigidbody>();
        this.bulletTrail = this.gameObject.GetComponent<TrailRenderer>();
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        this.bulletRB.MovePosition(this.bulletRB.position + this.gameObject.transform.forward * this.bulletSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if(Mathf.Abs(this.gameObject.transform.position.z - this.originZ) >= this.shootRange ||
           Mathf.Abs(this.gameObject.transform.position.x - this.originX) >= this.shootRange)
        {
            StartCoroutine(EngageToParent(GameObject.FindWithTag("Ammo Holder").transform));
        }
    }

    public void DisengageFromParent()
    {
        this.gameObject.transform.SetParent(null);
        this.originZ = this.gameObject.transform.position.z;
        this.originX = this.gameObject.transform.position.x;
    }

    public IEnumerator EngageToParent(Transform parent)
    {
        EmmitTrail(false);

        yield return null;

        this.gameObject.transform.SetParent(parent);
        this.bulletTrail.Clear();        

        yield return new WaitForSeconds(0.4f); //The duration of the hit sfx

        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
    }


    // Physics Callbacks ====================================================================================================================

    private void OnTriggerEnter(Collider other)
    {
        this.audioSource.PlayOneShot(this.hitSFX); 
        GameObject sparks = Instantiate<GameObject>(this.bulletSparksVFX, this.gameObject.transform.position, Quaternion.identity);
        GameObject.Destroy(sparks, 1.50f);

        StartCoroutine(EngageToParent(GameObject.FindWithTag("Ammo Holder").transform));
    }
}

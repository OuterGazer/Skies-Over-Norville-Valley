using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Machine Gun Settings")]
    [SerializeField] EnemyBullet bullet;
    [SerializeField] float timeBetweenBullets = default;
    [SerializeField] float barrageBulletAmount = default;
    [SerializeField] float timeBetweenBarrages = default;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] GameObject muzzleshot;

    [Header("Cannon Settings")]
    [SerializeField] GameObject bomb;
    [SerializeField] float timeBetweenBombs = default;
    [SerializeField] AudioClip cannonSFX;
    [SerializeField] GameObject cannonVFX;


    private float bulletTimeCounter;
    private float bulletCounter;
    private float barrageTimeCounter;
    private float bombTimeCounter;


    private AudioSource audioSource;


    private bool waitForNextBarrage = false;

    // Start is called before the first frame update
    void Start()
    {
        this.bulletTimeCounter = this.timeBetweenBullets;
        this.barrageTimeCounter = this.timeBetweenBarrages;
        this.bombTimeCounter = this.timeBetweenBombs;

        this.audioSource = this.gameObject.GetComponentInParent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(this.bullet != null)
            SpawnBullets();

        if(this.bomb != null)
            SpawnBombs();
    }

    private void SpawnBullets()
    {
        this.bulletTimeCounter -= Time.fixedDeltaTime;

        if ((this.bulletTimeCounter <= 0) && (this.bulletCounter <= this.barrageBulletAmount) &&
            (!this.waitForNextBarrage))
        {
            this.muzzleshot.SetActive(true);

            EnemyBullet bulletShot = Instantiate<EnemyBullet>(this.bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
            this.audioSource.PlayOneShot(this.shootSFX);

            this.bulletTimeCounter = this.timeBetweenBullets;
            this.bulletCounter++;

           SetBulletDirection(bulletShot);

            if (this.bulletCounter >= this.barrageBulletAmount)
            {
                this.bulletCounter = 0;

                this.waitForNextBarrage = true;

                this.muzzleshot.SetActive(false);
            }
        }

        StopBulletsBetweenBarrages();
    }

    private void SetBulletDirection(EnemyBullet bulletShot)
    {
        switch (this.gameObject.tag)
        {
            case "Left Turret":
                bulletShot.transform.Rotate(Vector3.up, -90f);
                break;

            case "Right Turret":
                bulletShot.transform.Rotate(Vector3.up, 90f);
                break;

            default:
                break;
        }
    }

    private void StopBulletsBetweenBarrages()
    {
        if (this.waitForNextBarrage)
        {
            this.barrageTimeCounter -= Time.fixedDeltaTime;

            if (this.barrageTimeCounter <= 0)
            {
                this.waitForNextBarrage = false;
                this.barrageTimeCounter = this.timeBetweenBarrages;
            }

        }
    }

    private void SpawnBombs()
    {
        this.bombTimeCounter -= Time.fixedDeltaTime;

        if(this.bombTimeCounter <= 0)
        {
            GameObject bombShot = Instantiate<GameObject>(this.bomb, this.gameObject.transform.position, this.gameObject.transform.rotation);
            this.audioSource.PlayOneShot(this.cannonSFX);
            this.StartCoroutine(ActivateDeactivateCannonVFX());

            if (this.gameObject.CompareTag("Tank Turret"))
                bombShot.transform.Rotate(Vector3.right, 90f);

            this.bombTimeCounter = this.timeBetweenBombs;
        }

        
    }

    private IEnumerator ActivateDeactivateCannonVFX()
    {
        this.cannonVFX.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        this.cannonVFX.SetActive(false);
    }
}

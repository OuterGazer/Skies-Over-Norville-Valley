using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] EnemyBullet bullet;
    [SerializeField] float timeBetweenBullets = default;
    [SerializeField] float barrageBulletAmount = default;
    [SerializeField] float timeBetweenBarrages = default;


    private float bulletTimeCounter;
    private float bulletCounter;
    private float barrageTimeCounter;

    private bool waitForNextBarrage = false;

    // Start is called before the first frame update
    void Start()
    {
        this.bulletTimeCounter = this.timeBetweenBullets;
        this.barrageTimeCounter = this.timeBetweenBarrages;
    }

    private void FixedUpdate()
    {
        SpawnBullets();
    }

    private void SpawnBullets()
    {
        this.bulletTimeCounter -= Time.fixedDeltaTime;

        if ((this.bulletTimeCounter <= 0) && (this.bulletCounter <= this.barrageBulletAmount) &&
            (!this.waitForNextBarrage))
        {
            EnemyBullet bulletShot = Instantiate<EnemyBullet>(this.bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
            this.bulletTimeCounter = this.timeBetweenBullets;
            this.bulletCounter++;

           SetBulletDirection(bulletShot);

            if (this.bulletCounter >= this.barrageBulletAmount)
            {
                this.bulletCounter = 0;

                this.waitForNextBarrage = true;
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
}

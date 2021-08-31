using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private InputAction playerShooting;
    [SerializeField] private Transform ammoParent;
    [SerializeField] private GameObject rightBarrel;
    [SerializeField] private GameObject leftBarrel;
    [SerializeField] private Bullet shootingAmmo;
    [SerializeField] private int maxAmmo = default;
    [SerializeField] private float bulletBarrageFactor = default; // For debugging purposes only

    
    private Bullet[] airshipAmmo;
    private int currentBullet = 0;
    private float fireNextBullet;
    private void SetFireNextBullet()
    {
        this.fireNextBullet = this.bulletBarrageFactor;
    }


    private bool wasLastBulletOnRightBarrel = false;


    private void OnEnable()
    {
        this.playerShooting.Enable();
    }
    private void OnDisable()
    {
        this.playerShooting.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateAndStoreAmmo();
    }

    private void CreateAndStoreAmmo()
    {
        this.airshipAmmo = new Bullet[this.maxAmmo];

        for (int i = 0; i < airshipAmmo.Length; i++)
        {
            airshipAmmo[i] = GameObject.Instantiate<Bullet>(this.shootingAmmo, this.ammoParent);
            airshipAmmo[i].gameObject.SetActive(false);
        }

        SetFireNextBullet();
    }

    // Update is called once per frame
    void Update()
    {
        this.fireNextBullet -= Time.deltaTime;

        ShootBullets();
    }

    private void ShootBullets()
    {
        if ((this.playerShooting.ReadValue<float>() != 0) &&
            (this.fireNextBullet < 0) &&
            (this.airshipAmmo[this.currentBullet].gameObject.transform.localPosition == Vector3.zero))
        {
            this.airshipAmmo[this.currentBullet].gameObject.SetActive(true);

            if (!this.wasLastBulletOnRightBarrel)
            {
                this.airshipAmmo[this.currentBullet].gameObject.transform.localPosition = this.rightBarrel.transform.localPosition;
                this.airshipAmmo[this.currentBullet].gameObject.transform.localRotation = this.rightBarrel.transform.localRotation;
            }
            else
            {
                this.airshipAmmo[this.currentBullet].gameObject.transform.localPosition = this.leftBarrel.transform.localPosition;
                this.airshipAmmo[this.currentBullet].gameObject.transform.localRotation = this.leftBarrel.transform.localRotation;
            }

            this.wasLastBulletOnRightBarrel = !this.wasLastBulletOnRightBarrel;
            this.airshipAmmo[this.currentBullet].EmmitTrail(true);

            this.airshipAmmo[this.currentBullet].DisengageFromParent();

            SetFireNextBullet();
            this.currentBullet++;
        }

        if (this.currentBullet >= this.airshipAmmo.Length)
            this.currentBullet = 0;
    }

}

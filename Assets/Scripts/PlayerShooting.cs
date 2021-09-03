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
    [SerializeField] private SpriteRenderer crosshair;
    [SerializeField] private float aimRange = 210.0f;
    [SerializeField] private float aimRadius = 2.20f;

    
    private Bullet[] airshipAmmo;
    private int currentBullet = 0;
    private float fireNextBullet;
    private void SetFireNextBullet()
    {
        this.fireNextBullet = this.bulletBarrageFactor;
    }
    private LayerMask enemyMask;
    private CollisionHandler player;
    private Enemy lockedOnEnemy;


    private bool wasLastBulletOnRightBarrel = false;
    private bool lockOn = false;


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

        this.enemyMask = LayerMask.GetMask("Enemy");
        this.player = this.gameObject.GetComponent<CollisionHandler>();
    }

    private void CreateAndStoreAmmo()
    {
        this.airshipAmmo = new Bullet[this.maxAmmo];

        for (int i = 0; i < airshipAmmo.Length; i++)
        {
            airshipAmmo[i] = GameObject.Instantiate<Bullet>(this.shootingAmmo, this.ammoParent);
            airshipAmmo[i].gameObject.layer = 7;
            airshipAmmo[i].gameObject.SetActive(false);
        }

        SetFireNextBullet();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        bool hasHitEnemy =
            Physics.SphereCast(this.crosshair.gameObject.transform.position, this.aimRadius, this.crosshair.gameObject.transform.forward, out hit, this.aimRange, this.enemyMask);

        if (hasHitEnemy)
        {
            this.crosshair.color = Color.red;
            this.lockOn = true;
            this.lockedOnEnemy = hit.collider.GetComponent<Enemy>();
        }            
        else
        {
            this.crosshair.color = Color.white;
            this.lockOn = false;
            this.lockedOnEnemy = null;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        //if (!this.player.IsAlive) { return; }

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
            this.airshipAmmo[this.currentBullet].IsEnemyLockedOn = false;

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

            if (this.lockOn)
            {
                this.airshipAmmo[this.currentBullet].IsEnemyLockedOn = true;
                this.airshipAmmo[this.currentBullet].SetLockedOnPosition(this.lockedOnEnemy);
            }

            SetFireNextBullet();
            this.currentBullet++;
        }

        if (this.currentBullet >= this.airshipAmmo.Length)
            this.currentBullet = 0;
    }

}

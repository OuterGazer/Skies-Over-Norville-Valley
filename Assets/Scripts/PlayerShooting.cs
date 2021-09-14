using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private float maxShootingTime = default;
    [SerializeField] private GameObject normalMuzzleFlash;
    [SerializeField] private GameObject heatedMuzzleFlash;

    [Header("Bombing Settings")]
    [SerializeField] private InputAction dropBombs;
    [SerializeField] private GameObject bomb;
    [SerializeField] private Transform dropSpot;
    [SerializeField] private float timeBetweenBombs = default;
    [SerializeField] private float timeToLoadBombs = default;

    [Header("UI Settings")]
    [SerializeField] Slider overheatingSlider;
    private float overheatingTimer;
    [SerializeField] TextMeshProUGUI bombLoadPercentage;
    private float bombLoadingTimer;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip dryShootSFX;


    private AudioSource audioSource;
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
    private bool isMachineGunStuck = false;
    private bool areBombsLoaded = true;
    private bool isMachineGunOverheated = false;


    private void OnEnable()
    {
        this.playerShooting.Enable();
        this.dropBombs.Enable();
    }
    private void OnDisable()
    {
        this.playerShooting.Disable();
        this.dropBombs.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateAndStoreAmmo();

        SetMaxFiringTime();

        this.audioSource = this.gameObject.GetComponent<AudioSource>();

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

    private void SetMaxFiringTime()
    {
        this.overheatingTimer = this.maxShootingTime;
        this.overheatingSlider.maxValue = this.maxShootingTime;
        this.overheatingSlider.value = this.overheatingSlider.maxValue;

        this.bombLoadingTimer = this.timeToLoadBombs;
    }
    

    private void FixedUpdate()
    {
        CheckEnemiesInFront();
    }

    private void CheckEnemiesInFront()
    {
        RaycastHit hit;
        bool hasHitEnemy =
            Physics.SphereCast(this.crosshair.gameObject.transform.position, this.aimRadius, this.crosshair.gameObject.transform.forward, out hit, this.aimRange, this.enemyMask);

        SetCrosshairColorOnEnemyLockOn(hit, hasHitEnemy);
    }

    private void SetCrosshairColorOnEnemyLockOn(RaycastHit hit, bool hasHitEnemy)
    {
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
        AttackOnButtonPress();
    }

    private void AttackOnButtonPress()
    {
        this.fireNextBullet -= Time.deltaTime;

        ShootBullets();

        this.overheatingTimer = Mathf.Clamp(this.overheatingTimer, 0f, this.maxShootingTime);

        DropBombs();
    }

    private void ShootBullets()
    {

        // Barrage fire while button keeps being pressed, the timer between bullets has reached 0 and the bullet position is back at the machine gun
        if ((this.playerShooting.ReadValue<float>() != 0) &&
            (!this.isMachineGunStuck) &&
            (this.fireNextBullet < 0) &&
            (this.airshipAmmo[this.currentBullet].gameObject.transform.localPosition == Vector3.zero))
        {
            this.airshipAmmo[this.currentBullet].gameObject.SetActive(true); // Activate the cached bullet
            this.airshipAmmo[this.currentBullet].IsEnemyLockedOn = false; // Set the lock on to standard value so the bullet flies straight

            PlayShootingEffects();

            ChangeShootingBarrelForEachBullet();

            this.airshipAmmo[this.currentBullet].EmmitTrail(true);
            this.airshipAmmo[this.currentBullet].DisengageFromParent(); // So the bullets don't move with the airship movement

            SetLockOnEnemy();

            // Set the timer between bullets to standard and prepare the next bullet in the array to be fired
            SetFireNextBullet();
            this.currentBullet++;
        }
        else if((this.playerShooting.ReadValue<float>() != 0) &&
                (this.isMachineGunStuck) &&
                (this.fireNextBullet < 0))
        {
            this.audioSource.PlayOneShot(this.dryShootSFX);
            this.normalMuzzleFlash.SetActive(false);
            this.heatedMuzzleFlash.SetActive(false);
            SetFireNextBullet();
        }

        if (((Mathf.Approximately(this.playerShooting.ReadValue<float>(), 0)) && this.normalMuzzleFlash.activeSelf) ||
            ((Mathf.Approximately(this.playerShooting.ReadValue<float>(), 0)) && this.heatedMuzzleFlash.activeSelf))
        {
            if (!this.isMachineGunOverheated)
            {
                this.normalMuzzleFlash.SetActive(false);
            }
            else
            {
                this.heatedMuzzleFlash.SetActive(false);
            }
        }

        // Go bak to the beginning of the array once we reached the end
        if (this.currentBullet >= this.airshipAmmo.Length)
            this.currentBullet = 0;

        UpdateTemperatureSlider();
    }

    private void PlayShootingEffects()
    {
        this.audioSource.PlayOneShot(this.shootSFX);

        if (!this.isMachineGunOverheated)
        {
            this.normalMuzzleFlash.SetActive(true);
            this.heatedMuzzleFlash.SetActive(false);
        }
        else
        {
            this.normalMuzzleFlash.SetActive(false);
            this.heatedMuzzleFlash.SetActive(true);
        }
    }

    private void ChangeShootingBarrelForEachBullet()
    {
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
    }

    private void SetLockOnEnemy()
    {
        // If player fires while nearby enemies, bullets will go homing towards the enemy to aid aiming
        if (this.lockOn)
        {
            this.airshipAmmo[this.currentBullet].IsEnemyLockedOn = true;
            this.airshipAmmo[this.currentBullet].SetLockedOnPosition(this.lockedOnEnemy);
        }
    }

    private void UpdateTemperatureSlider()
    {
        if ((this.playerShooting.ReadValue<float>() != 0) && !this.isMachineGunStuck)
        {
            this.overheatingTimer -= Time.deltaTime;
        }
        else
        {
            this.overheatingTimer += Time.deltaTime;
        }

        this.overheatingSlider.value = this.overheatingTimer;

        if (this.overheatingTimer > 0.85f)
        {            
            this.isMachineGunOverheated = false;
        }

        if ((this.overheatingTimer <= 0.85f) && !this.isMachineGunStuck)
        {
            this.airshipAmmo[this.currentBullet].SetTrailToOverheat();
            this.isMachineGunOverheated = true;
        }
        
        if ((this.overheatingTimer <= 0) && !this.isMachineGunStuck)
        {
            this.isMachineGunStuck = true;
            this.crosshair.enabled = false;
            StartCoroutine(CoolMachineGun());
        }
    }

    private IEnumerator CoolMachineGun()
    {
        yield return new WaitForSeconds(this.maxShootingTime);

        this.isMachineGunStuck = false;
        this.isMachineGunOverheated = false;
        this.crosshair.enabled = true;
    }

    private void DropBombs()
    {
        if(this.dropBombs.triggered && this.areBombsLoaded)
        {
            this.areBombsLoaded = false;
            StartCoroutine(SpawnBombs());            
        }

        UpdateUILoadingPercentage();
    }

    private IEnumerator SpawnBombs()
    {
        GameObject bomb1 = Instantiate<GameObject>(this.bomb, this.dropSpot.position, this.dropSpot.rotation);

        yield return new WaitForSeconds(this.timeBetweenBombs);

        GameObject bomb2 = Instantiate<GameObject>(this.bomb, this.dropSpot.position, this.dropSpot.rotation);

        yield return new WaitForSeconds(this.timeBetweenBombs);

        GameObject bomb3 = Instantiate<GameObject>(this.bomb, this.dropSpot.position, this.dropSpot.rotation);
        this.bombLoadingTimer = 0;

        yield return new WaitForSeconds(this.timeToLoadBombs);

        this.areBombsLoaded = true;
    }

    private void UpdateUILoadingPercentage()
    {
        if (!this.areBombsLoaded)
        {            
            this.bombLoadingTimer += Time.deltaTime;
        }

        this.bombLoadingTimer = Mathf.Clamp(this.bombLoadingTimer, 0, this.timeToLoadBombs);

        int loadPercentage = (int)((this.bombLoadingTimer / this.timeToLoadBombs) * 100);

        this.bombLoadPercentage.text = loadPercentage.ToString() + "%";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float hitPoints = default;

    [Header("Player UI Settings")]
    [SerializeField] Slider healthBar;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image fillImage;
    [SerializeField] float yellowThreshold = default;
    [SerializeField] float redThreshold = default;
    [SerializeField] float startRepairTime = default;

    private float playerMaxHitPoints;
    public float PlayerMaxHitPoints => this.playerMaxHitPoints;


    private Coroutine repairCoroutine = null;


    private bool isAlive = true;
    public bool IsAlive => this.isAlive;
    public void SetIsAliveToFalse()
    {
        this.isAlive = false;
    }
    private bool canRepair = false;

    private void Start()
    {
        SetPlayerHealthBarValues();
    }

    private void SetPlayerHealthBarValues()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            this.playerMaxHitPoints = this.hitPoints;

            this.healthBar.value = this.healthBar.maxValue;
            ChangeDamageColor(Color.green);
        }
    }

    private void Update()
    {
        RepairPlayerAircraft();
    }

    private void RepairPlayerAircraft()
    {
        if (this.canRepair)
        {
            this.hitPoints += Time.deltaTime;

            SetHealthBarColor();

            if(this.hitPoints >= this.playerMaxHitPoints)
            {
                this.hitPoints = this.playerMaxHitPoints;
                this.canRepair = false;
            }
        }
    }

    private void SetHealthBarColor()
    {
        float damageLevel = (this.hitPoints / this.playerMaxHitPoints) * 100;

        switch (damageLevel)
        {
            case float i when i <= this.yellowThreshold && i > this.redThreshold:
                ChangeDamageColor(Color.yellow);
                break;

            case float i when i <= this.redThreshold:
                ChangeDamageColor(Color.red);
                break;

            default:
                ChangeDamageColor(Color.green);
                break;
        }

        this.healthBar.value = damageLevel;
    }

    public void DecreaseHitPoints(int amountToDecrease)
    {
        this.hitPoints -= amountToDecrease;

        if (this.hitPoints <= 0)
        {
            this.isAlive = false;
        }

        UpdatePlayerDamageSlider();
    }

    private void UpdatePlayerDamageSlider()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            TriggerRepairCoroutine();            

            SetHealthBarColor();            

            if (!this.isAlive)
                StartCoroutine(this.gameObject.GetComponent<CollisionHandler>().ProcessPlayerDeath());
        }
    }

    private void TriggerRepairCoroutine()
    {
        if (this.canRepair)
            this.canRepair = false;

        if (this.repairCoroutine == null)
        {
            this.repairCoroutine = StartCoroutine(InitializeRepairs());
        }
        else
        {
            StopCoroutine(this.repairCoroutine);
            this.repairCoroutine = StartCoroutine(InitializeRepairs());
        }
    }

    private IEnumerator InitializeRepairs()
    {
        yield return new WaitForSeconds(this.startRepairTime);

        this.canRepair = true;
        this.repairCoroutine = null;
    }

    private void ChangeDamageColor(Color color)
    {
        this.backgroundImage.color = color;
        this.fillImage.color = color;
    }

    public void ExplodeEnemy()
    {
        this.hitPoints = 0;
        this.isAlive = false;
        this.gameObject.GetComponent<Enemy>().DestroyEnemyOnGround();
    }
}

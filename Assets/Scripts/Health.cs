using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int hitPoints = default;

    [Header("Player UI Settings")]
    [SerializeField] Slider healthBar;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image fillImage;
    [SerializeField] float greenThreshold = default;
    [SerializeField] float yellowThreshold = default;
    [SerializeField] float redThreshold = default;

    private float playerMaxHitPoints;


    private bool isAlive = true;
    public bool IsAlive => this.isAlive;
    public void SetIsAliveToFalse()
    {
        this.isAlive = false;
    }

    private void Start()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            this.playerMaxHitPoints = this.hitPoints;

            this.healthBar.value = this.healthBar.maxValue;
            ChangeDamageColor(Color.green);
        }
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
            float damageLevel = (this.hitPoints / this.playerMaxHitPoints) * 100;

            switch (damageLevel)
            {
                case float i when i <= 60f && i > 30f:
                    ChangeDamageColor(Color.yellow);
                    break;

                case float i when i <= 30f:
                    ChangeDamageColor(Color.red);
                    break;

                default:
                    ChangeDamageColor(Color.green);
                    break;
            }

            this.healthBar.value = damageLevel;

            if (!this.isAlive)
                StartCoroutine(this.gameObject.GetComponent<CollisionHandler>().ProcessPlayerDeath());
        }
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

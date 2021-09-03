using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int hitPoints = default;


    private bool isAlive = true;
    public bool IsAlive => this.isAlive;
    public void SetIsAliveToFalse()
    {
        this.isAlive = false;
    }

    public void DecreaseHitPoints(int amountToDecrease)
    {
        this.hitPoints -= amountToDecrease;

        if(this.hitPoints <= 0)
        {
            this.isAlive = false;
        }
    }
}

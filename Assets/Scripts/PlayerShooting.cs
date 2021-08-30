using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] InputAction playerShooting;
    [SerializeField] Bullet shootingAmmo;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerShooting.triggered)
        {

        }
    }
}

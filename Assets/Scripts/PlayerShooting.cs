using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] InputAction playerShooting;
    [SerializeField] Transform ammoParent;
    [SerializeField] GameObject rightBarrel;
    [SerializeField] GameObject leftBarrel;
    [SerializeField] Bullet shootingAmmo;
    [SerializeField] int maxAmmo = default;

    Bullet[] airshipAmmo;
    int currentBullet = 0;

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
        this.airshipAmmo = new Bullet[this.maxAmmo];

        for(int i = 0; i < airshipAmmo.Length; i++)
        {
            airshipAmmo[i] = GameObject.Instantiate<Bullet>(this.shootingAmmo, this.ammoParent);
            airshipAmmo[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerShooting.triggered)
        {
            this.airshipAmmo[this.currentBullet].gameObject.SetActive(true);
            this.airshipAmmo[this.currentBullet].gameObject.transform.localPosition = this.rightBarrel.transform.localPosition;
            this.airshipAmmo[this.currentBullet].gameObject.transform.localRotation = this.rightBarrel.transform.localRotation;
            this.currentBullet++;

            if (this.currentBullet > this.airshipAmmo.Length)
                this.currentBullet = 0;
        }
    }
}

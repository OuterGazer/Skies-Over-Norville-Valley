using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputAction playerMov;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        this.playerMov.Enable();
    }

    private void OnDisable()
    {
        this.playerMov.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalThrow = this.playerMov.ReadValue<Vector2>().x;
        float verticalThrow = this.playerMov.ReadValue<Vector2>().y;

        Debug.Log(horizontalThrow);
        Debug.Log(verticalThrow);
    }
}

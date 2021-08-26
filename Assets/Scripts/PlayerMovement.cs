using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputAction playerMov;
    [SerializeField] float moveSpeed = default;

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
        float xThrow = this.playerMov.ReadValue<Vector2>().x;
        float yThrow = this.playerMov.ReadValue<Vector2>().y;

        float xPosNextFrame = xThrow * this.moveSpeed * Time.deltaTime;
        float yPosNextFrame = yThrow * this.moveSpeed * Time.deltaTime;

        this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x + xPosNextFrame,
                                                              this.gameObject.transform.localPosition.y + yPosNextFrame,
                                                              this.gameObject.transform.localPosition.z);


    }
}

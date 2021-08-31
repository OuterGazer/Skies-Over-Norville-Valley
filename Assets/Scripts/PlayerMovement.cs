using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] InputAction playerMov;
    [SerializeField] float moveSpeed = default;
    [SerializeField] float rotatingSpeed = default;
    [SerializeField] float positionPitchFactor = default;
    [SerializeField] float controlPitchFactor = default;
    [SerializeField] float positionYawFactor = default;
    [SerializeField] float controlRollFactor = default;

    [Header("Camera Settings")]
    [Range(0, 1)][SerializeField] float xScreenLimitPercent = default;
    [Range(0, 1)] [SerializeField] float yScreenLimitPercent = default;

    float xThrow, yThrow;
    private float xScreenLimit;
    private float yScreenLimit;

    private Camera playerView;
    private CollisionHandler player;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CalculateScreenLimits();

        this.player = this.gameObject.GetComponent<CollisionHandler>();
    }

    private void CalculateScreenLimits()
    {
        this.playerView = Camera.main;

        // To get the real horizontal screen space we apply some trigonometry. We multiply the camera distance to the player times the tangent of half of the horizontal field of view angle
        this.xScreenLimit = Mathf.Abs(this.playerView.transform.localPosition.z) *
                            Mathf.Tan(Mathf.Deg2Rad *
                                      (Camera.VerticalToHorizontalFieldOfView(this.playerView.fieldOfView, this.playerView.aspect) / 2));
        this.xScreenLimit /= 1_000; // To convert from absolute pixel measurement to screen measurement
        this.xScreenLimit *= this.xScreenLimitPercent; // To avoid half of the airplane to disappear from the screen, due to the pivot position.

        this.yScreenLimit = Mathf.Abs(this.playerView.transform.localPosition.z) *
                            Mathf.Tan(Mathf.Deg2Rad * this.playerView.fieldOfView / 2);
        this.yScreenLimit /= 1_000;
        this.yScreenLimit *= this.yScreenLimitPercent;
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
        if (!this.player.IsAlive) { return; }

        ProcessPlayerMovement();

        ProcessPlayerRotation();
    }
    

    private void ProcessPlayerMovement()
    {
        
        ReadPlayerInput();

        Vector3 nextPlayerPos = CalculateNextPlayerPosition(this.xThrow, this.yThrow);

        Vector3 playerPos = this.playerView.ScreenToViewportPoint(nextPlayerPos); // Convert player position into camera viewport units

        playerPos = ClampPlayerMovementToScreenView(playerPos);

        nextPlayerPos = this.playerView.ViewportToScreenPoint(playerPos); // Convert from viewport units back into world units

        MovePlayer(nextPlayerPos);
    }

    private void ReadPlayerInput()
    {
        this.xThrow = this.playerMov.ReadValue<Vector2>().x;
        this.yThrow = this.playerMov.ReadValue<Vector2>().y;
    }

    private Vector3 CalculateNextPlayerPosition(float xThrow, float yThrow)
    {
        float xPosNextFrame = xThrow * this.moveSpeed * Time.deltaTime;
        float yPosNextFrame = yThrow * this.moveSpeed * Time.deltaTime;

        Vector3 nextPlayerPos = new Vector3(this.gameObject.transform.localPosition.x + xPosNextFrame,
                                            this.gameObject.transform.localPosition.y + yPosNextFrame,
                                            this.gameObject.transform.localPosition.z);
        return nextPlayerPos;
    }

    private Vector3 ClampPlayerMovementToScreenView(Vector3 playerPos)
    {
        playerPos.x = Mathf.Clamp(playerPos.x, -this.xScreenLimit, this.xScreenLimit);
        playerPos.y = Mathf.Clamp(playerPos.y, -this.yScreenLimit, 3 * this.yScreenLimit); // The factor 3 accounts for the camera already being rotated downwards to look at the player
        return playerPos;
    }

    private void MovePlayer(Vector3 nextPlayerPos)
    {
        this.gameObject.transform.localPosition = new Vector3(nextPlayerPos.x,
                                                              nextPlayerPos.y,
                                                              this.gameObject.transform.localPosition.z);
    }

    private void ProcessPlayerRotation()
    {
        float xPitch, yYaw, zRoll;

        xPitch = this.gameObject.transform.localPosition.y * this.positionPitchFactor + // To have the ship not always face the center of the screen, but be more or less straight in relation to its local position
                 this.yThrow * this.controlPitchFactor; // Rotates up/down according to player input
        yYaw = this.gameObject.transform.localPosition.x * this.positionYawFactor; // Same, moving sideways we apply a slight yaw that keeps the airship straight, as it automatically rotates to face the relative center of the view
        zRoll = this.xThrow * this.controlRollFactor;

        this.gameObject.transform.localRotation = Quaternion.RotateTowards(this.gameObject.transform.localRotation,
                                                                           Quaternion.Euler(xPitch, yYaw, zRoll),
                                                                           this.rotatingSpeed * Time.deltaTime);
    }
}

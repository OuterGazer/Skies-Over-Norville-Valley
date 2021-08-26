using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputAction playerMov;
    [SerializeField] float moveSpeed = default;
    [Range(0, 1)][SerializeField] float xScreenLimitPercent = default;
    [Range(0, 1)] [SerializeField] float yScreenLimitPercent = default;

    private float xScreenLimit;
    private float yScreenLimit;

    private Camera playerView;

    // Start is called before the first frame update
    void Start()
    {
        this.playerView = Camera.main;

        // To get the real horizontal screen space we apply som trigonometry. We multiply the camera distance to the player times the tangent of half of the horizontal field of view angle
        this.xScreenLimit = Mathf.Abs(this.playerView.transform.localPosition.z) *
                            Mathf.Tan(Mathf.Deg2Rad * 
                                      (Camera.VerticalToHorizontalFieldOfView(this.playerView.fieldOfView, this.playerView.aspect) / 2));
        this.xScreenLimit /= 1_000; // To convert from absolute pixel measurement to screen measurement
        this.xScreenLimit *= this.xScreenLimitPercent; 

        this.yScreenLimit = Mathf.Abs(this.playerView.transform.localPosition.z) *
                            Mathf.Tan(Mathf.Deg2Rad * this.playerView.fieldOfView / 2);
        this.yScreenLimit /= 1_000; // To convert from absolute pixel measurement to screen measurement
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
        float xThrow, yThrow;
        ReadPlayerInput(out xThrow, out yThrow);

        Vector3 nextPlayerPos = CalculateNextPlayerPosition(xThrow, yThrow);

        Vector3 playerPos = this.playerView.ScreenToViewportPoint(nextPlayerPos); // Convert player position into camera viewport units

        playerPos = ClampPlayerMovementToScreenView(playerPos);

        nextPlayerPos = this.playerView.ViewportToScreenPoint(playerPos); // Convert from viewport units back into world units

        MovePlayer(nextPlayerPos);
    }    

    private void ReadPlayerInput(out float xThrow, out float yThrow)
    {
        xThrow = this.playerMov.ReadValue<Vector2>().x;
        yThrow = this.playerMov.ReadValue<Vector2>().y;
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
}

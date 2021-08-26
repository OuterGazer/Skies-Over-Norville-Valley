using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputAction playerMov;
    [SerializeField] float moveSpeed = default;
    [Range(0, 1)][SerializeField] float xScreenLimitPercent = default;

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

        Debug.Log(this.xScreenLimit);

        /*this.xScreenLimit = Screen.width;
        this.yScreenLimit = Screen.height;*/

        /*this.xScreenLimit = Display.main.renderingWidth;
        this.yScreenLimit = Display.main.renderingHeight;*/
        /*Debug.Log(this.xScreenLimit);
        Debug.Log(this.yScreenLimit);*/
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

        Vector3 nextPlayerPos = new Vector3(this.gameObject.transform.localPosition.x + xPosNextFrame,
                                            this.gameObject.transform.localPosition.y + yPosNextFrame,
                                            this.gameObject.transform.localPosition.z);

        Vector3 playerPos = this.playerView.ScreenToViewportPoint(nextPlayerPos);

        playerPos.x = Mathf.Clamp(playerPos.x, -this.xScreenLimit, this.xScreenLimit);

        nextPlayerPos = this.playerView.ViewportToScreenPoint(playerPos);

        this.gameObject.transform.localPosition = new Vector3(nextPlayerPos.x,
                                                              this.gameObject.transform.localPosition.y + yPosNextFrame,
                                                              this.gameObject.transform.localPosition.z);

        Debug.Log(playerPos.x + " x position");
        Debug.Log(playerPos.y + " y position");
    }
}

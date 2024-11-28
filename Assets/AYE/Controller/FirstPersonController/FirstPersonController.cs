using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    // wsad
    float horizontal = 0f;
    float vertical = 0f;
    float wsadMix = 10f;
    [SerializeField] Rigidbody rb;
    [SerializeField] float downSpeed = 0.5f;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 4f;
    float speed = 0f;
    // mouse
    float mouseX = 0f;
    float mouseY = 0f;
    [SerializeField] Transform playerBody;
    [SerializeField] Transform playerHead;
    [SerializeField] float mouseSpeed = 1f;
    [SerializeField] Sensor floorSensor = null;
    // Jump
    [SerializeField] float jumpForce = 5f;
    float lastJumpTime = 0f;
    // Down
    [SerializeField] Collider topCollider = null;
    [SerializeField] Sensor topSensor = null;
    [SerializeField] Vector3 playerHeadLocalPosition = Vector3.zero;
    [SerializeField] Vector3 playerHeadDownLocalPosition = Vector3.zero;
    // QE
    [SerializeField] Transform cameraQEBase = null;
    [SerializeField] Transform qTransform = null;
    [SerializeField] Transform eTransform = null;
    // 體力
    [SerializeField] float maxRunTime = 10f;
    [SerializeField] float maxStopRunTime = 5f;
    float runTime = 0f;
    float stopRunTime = 0f;
    // 輸出
    /// <summary>在走路</summary>
    public bool isWalk = false;
    /// <summary>在跑步</summary>
    public bool isRun = false;
    /// <summary>在跳躍瞬間</summary>
    public bool isJump = false;
    /// <summary>是否蹲下</summary>
    public bool isDown = false;

    private void Awake()
    {
        playerHeadLocalPosition = playerHead.localPosition;
        runTime = maxRunTime;
    }
    bool isControlDown = false;
    private void Update()
    {
        // wsad
        horizontal = Mathf.Lerp(horizontal, Input.GetAxisRaw("Horizontal"), wsadMix * Time.deltaTime);
        vertical = Mathf.Lerp(vertical, Input.GetAxisRaw("Vertical"), wsadMix * Time.deltaTime);
        Vector3 move = Vector3.ClampMagnitude(new Vector3(horizontal, 0f, vertical), 1f);
        move = playerBody.TransformDirection(move);
        bool inputLeftShift = Input.GetKey(KeyCode.LeftShift) && (Time.time > stopRunTime);
        if (isDown == false)
            speed = Mathf.Lerp(speed, inputLeftShift ? runSpeed : walkSpeed, wsadMix * Time.deltaTime);
        else
            speed = Mathf.Lerp(speed, downSpeed, wsadMix * Time.deltaTime);
        move *= speed;
        move.y = rb.velocity.y;
        rb.velocity = move;
        // mouse
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y")*-1f * mouseSpeed;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);
        playerBody.rotation = Quaternion.Euler(0f, mouseX, 0f);
        playerHead.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        // jump
        isJump = false;
        if (Input.GetKey(KeyCode.Space) && Time.time > stopRunTime && floorSensor.on && Time.time > lastJumpTime + 0.5f)
        {
            lastJumpTime = Time.time;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isJump = true;
        }

        // 輸出
        isRun = false;
        isWalk = false;
        isJump = false;
        if (move.magnitude > 0.2f)
        {
            if (inputLeftShift)
                isRun = true;
            else
                isWalk = true;
        }

        // down
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 如果站著就蹲下
            if (isDown == false)
                isDown = true;
            else if (isDown && topSensor.on == false) // 如果蹲下而且上方沒東西就站起來
            {
                isDown = false;
                isControlDown = false;
            }
        }
        // Ctrl蹲下
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftCommand))
        {
            // 如果站著就蹲下
            if (isDown == false)
            {
                isDown = true;
                isControlDown = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftCommand))
        {
            // 如果蹲下而且上方沒東西就站起來
            if (isDown && topSensor.on == false)
            {
                isDown = false;
                isControlDown = false;
            }
        }
        // 如果是用Ctrl蹲下就自動取消
        if (Input.GetKey(KeyCode.LeftControl) == false && Input.GetKey(KeyCode.LeftCommand) == false && isControlDown && isDown)
        {
            if (topSensor.on == false)
            {
                isDown = false;
                isControlDown = false;
            }
        }
        // 跑步取消蹲下
        if (inputLeftShift && isDown && topSensor.on == false)
            isDown = false;
        topCollider.enabled = !isDown;
        playerHead.localPosition = Vector3.Lerp(playerHead.localPosition, isDown ? playerHeadDownLocalPosition : playerHeadLocalPosition, 10f * Time.deltaTime);

        // QE
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E) == false)
        {
            cameraQEBase.localPosition = Vector3.Lerp(cameraQEBase.localPosition, qTransform.localPosition, 10f * Time.deltaTime);
            cameraQEBase.localRotation = Quaternion.Lerp(cameraQEBase.localRotation, qTransform.localRotation, 10f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q) == false)
        {
            cameraQEBase.localPosition = Vector3.Lerp(cameraQEBase.localPosition, eTransform.localPosition, 10f * Time.deltaTime);
            cameraQEBase.localRotation = Quaternion.Lerp(cameraQEBase.localRotation, eTransform.localRotation, 10f * Time.deltaTime);
        }
        else
        {
            cameraQEBase.localPosition = Vector3.Lerp(cameraQEBase.localPosition, Vector3.zero, 10f * Time.deltaTime);
            cameraQEBase.localRotation = Quaternion.Lerp(cameraQEBase.localRotation, Quaternion.identity, 10f * Time.deltaTime);
        }

        // 體力
        if (isRun)
            runTime -= Time.deltaTime;
        else
            runTime += Time.deltaTime;
        if (runTime < 0f)
            stopRunTime = Time.time + maxStopRunTime;
        if (runTime > maxRunTime)
            runTime = maxRunTime;
    }
}

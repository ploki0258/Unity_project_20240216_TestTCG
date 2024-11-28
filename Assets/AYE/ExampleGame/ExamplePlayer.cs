using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExamplePlayer : MonoBehaviour
{
    [SerializeField] Rigidbody rigi = null;
    [SerializeField] Transform yRoot = null;
    [SerializeField] Transform xRoot = null;
    [SerializeField] Transform sideRoot = null;
    [SerializeField] float sideAngle = 45f;
    float ws = 0, ad = 0, mouseY = 0f, side = 0f, _mp = 5f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float mouseSpeed = 1f;
    [SerializeField] Image bar0 = null, bar1 = null;
    float mp { get { return _mp; } set { _mp = Mathf.Clamp(value, 0f, maxMp); bar0.fillAmount = bar1.fillAmount = mp / maxMp; } }
    [SerializeField] float maxMp = 5f;
    bool needF = false;
    void Update()
    {
        // 位移
        ws = Mathf.Lerp(ws, Input.GetAxisRaw("Vertical"), 5f * Time.deltaTime);
        ad = Mathf.Lerp(ad, Input.GetAxisRaw("Horizontal"), 5f * Time.deltaTime);
        Vector3 move = yRoot.TransformDirection(Vector3.ClampMagnitude(new Vector3(ad, 0f, ws), 1f) * ((Input.GetKey(KeyCode.LeftShift) && mp > 0.1f) ? runSpeed : walkSpeed));
        move.y = rigi.velocity.y;
        rigi.velocity = move;
        mp = mp + (Input.GetKey(KeyCode.LeftShift) ? -1f * Time.deltaTime : Time.deltaTime);
        // 旋轉
        yRoot.Rotate(0f, Input.GetAxis("Mouse X") * mouseSpeed, 0f);
        mouseY = Mathf.Clamp(mouseY + Input.GetAxis("Mouse Y") * -1f * mouseSpeed, -80f, 80f);
        xRoot.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        // 側身
        side = Mathf.Lerp(side, (Input.GetKey(KeyCode.Q) ? sideAngle : 0f) + (Input.GetKey(KeyCode.E) ? -1f * sideAngle : 0f), 10f * Time.deltaTime);
        sideRoot.localRotation = Quaternion.Euler(0f, 0f, side);
        // 互動
        if (Input.GetKeyDown(KeyCode.F))
        {
            needF = true;
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mp = maxMp;
    }
    RaycastHit hit;
    bool isHit = false;
    void FixedUpdate()
    {
        if (needF)
        {
            isHit = Physics.Raycast(xRoot.position, xRoot.forward, out hit, 2f);
            if (isHit)
            {
                hit.collider.gameObject.SendMessageUpwards("F", SendMessageOptions.DontRequireReceiver);
            }
            needF = false;
        }
    }
}

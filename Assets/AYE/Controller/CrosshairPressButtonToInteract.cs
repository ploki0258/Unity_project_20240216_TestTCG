using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AYE/CrosshairPressButtonToInteract")]
public class CrosshairPressButtonToInteract : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    [Header("判定距離")]
    [SerializeField] float interactDistance = 2f;
    [Header("互動按鈕")]
    [SerializeField] KeyCode interactButton = KeyCode.E;
    [Header("可互動和可阻擋的圖層")]
    [SerializeField] LayerMask allLayer = 0;
    [Header("可互動的圖層")]
    [SerializeField] LayerMask interactLayer = 0;
    [Header("發射點")]
    [SerializeField] Transform playerHead = null;
    [Header("縮放顯示準心UI")]
    [SerializeField] Transform crosshair = null;
    /// <summary>偵測對象</summary>
    public RaycastHit raycastHit;
    [Header("按鈕互動時對該物件送SendMessage名稱")]
    [SerializeField] string message = "OnInteract";
    /// <summary>是否偵測到</summary>
    [ShowOnly] public bool isHit = false;

    private void Awake()
    {
        if (playerHead == null)
        {
            Debug.Log("playerHead自動使用攝影機", this.gameObject);
            playerHead = Camera.main.transform;
        }
    }
    private void FixedUpdate()
    {
        isHit = Physics.Raycast(playerHead.position, playerHead.forward, out raycastHit, interactDistance, allLayer);
        // 非專用圖層表示被擋住
        if (isHit && Aye.IsInLayerMask(raycastHit.collider.gameObject.layer, interactLayer) == false)
            isHit = false;
        crosshair.localScale = isHit? Vector3.one : Vector3.zero;
    }
    private void Update()
    {
        if (isHit && Input.GetKeyDown(interactButton) && message != "")
        {
            raycastHit.collider.gameObject.SendMessage(message, SendMessageOptions.DontRequireReceiver);
            Debug.Log("向 " + raycastHit.collider.gameObject.name + " 發送訊息 " + message, raycastHit.collider.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerHead.position, raycastHit.point);
            Gizmos.DrawSphere(raycastHit.point, 0.1f);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerHead.position, playerHead.position + playerHead.forward * interactDistance);
        }
    }
}

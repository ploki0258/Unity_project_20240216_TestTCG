using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	[SerializeField] Image buttonImage; // 按鈕圖片
	[SerializeField] Color hoverColor = new Color(0.8f, 0.8f, 0.8f, 1f); // 鼠標懸停時的顏色
	[SerializeField] UnityEvent clickEvent; // 點擊事件

	Color originalColor; // 原始顏色

	void Start()
	{
		originalColor = buttonImage.color; // 儲存原始顏色
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		buttonImage.color = hoverColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		buttonImage.color = originalColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		clickEvent.Invoke();
	}
}

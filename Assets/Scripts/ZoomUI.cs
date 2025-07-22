using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 縮放UI
/// </summary>
public class ZoomUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] float zoomScale = 1.1f; // 縮放比例

	public void OnPointerEnter(PointerEventData eventData)
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector2(zoomScale, zoomScale); // 縮放UI元素
		//transform.localScale = Vector2.one * zoomScale; // 縮放UI元素
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = Vector2.one; // 縮放UI元素
		//transform.localScale = Vector3.one; // 恢復原始大小
	}
}

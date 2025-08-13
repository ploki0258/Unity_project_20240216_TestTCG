using UnityEngine;

public class Arrow : MonoBehaviour
{
	public Vector2 startPoint;
	[SerializeField] Vector2 endPoint;
	[SerializeField] RectTransform arrow;

	float arrowLength;
	float arrowTheta;
	Vector2 arrowPosition;

	private void Start()
	{
		arrow = transform.GetComponent<RectTransform>();
	}

	private void Update()
	{
		// 獲得當前螢幕的寬與高
		float width = Screen.width;
		float height = Screen.height;

		// 設置結束點
		endPoint = Input.mousePosition - new Vector3((int)(width / 2), (int)(height / 2));
		arrowPosition = new Vector2((endPoint.x + startPoint.x) / 2, (endPoint.y + startPoint.y) / 2);
		arrowLength = Mathf.Sqrt((Mathf.Pow((endPoint.x - startPoint.x), 2) + Mathf.Pow((endPoint.y - startPoint.y), 2)));
		arrowTheta = Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x);

		// 更新箭頭物件的局部位置
		arrow.localPosition = arrowPosition;
		arrow.sizeDelta = new Vector2(arrowLength, arrow.sizeDelta.y);
		arrow.localEulerAngles = new Vector3(0f, 0f, (arrowTheta * 180 / Mathf.PI));

		//arrow.localPosition = endPoint;
		//Debug.Log($"結束點:{endPoint}");
	}

	/// <summary>
	/// 設置初始點
	/// </summary>
	/// <param name="startPoint">起始點</param>
	public void SetStartPoint(Vector2 startPoint)
	{
		float width = Screen.width;
		float height = Screen.height;

		this.startPoint = startPoint - new Vector2((width / 2), (height / 2));
	}
}

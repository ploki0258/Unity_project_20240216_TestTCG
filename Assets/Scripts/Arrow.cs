using UnityEngine;

public class Arrow : MonoBehaviour
{
	public Vector2 startPoint;
	[SerializeField] Vector2 endPoint;
	[SerializeField] RectTransform arrow;

	double arrowLength;
	double arrowTheta;
	Vector2 arrowPosition;

	private void Start()
	{
		arrow = transform.GetComponent<RectTransform>();
	}

	private void Update()
	{
		// 獲得當前螢幕的寬與高
		double width = Screen.width;
		double height = Screen.height;

		// 設置結束點
		endPoint = Input.mousePosition - (Vector3)new Vector2((float)(width / 2), (float)(height / 2));
		arrowLength = Mathf.Sqrt(Mathf.Pow((endPoint.x - startPoint.x), 2) + Mathf.Pow((endPoint.y - startPoint.y), 2));
		arrowPosition = new Vector2((endPoint.x + startPoint.x) / 2, (endPoint.y + startPoint.y) / 2);
		arrowTheta = Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x);

		// 更新箭頭物件的局部位置
		arrow.localPosition = arrowPosition;
		arrow.sizeDelta = new Vector2((float)arrowLength, arrow.sizeDelta.y);
		arrow.localEulerAngles = new Vector3(0f, 0f, (float)(arrowTheta * 180 / Mathf.PI));
	}

	public void SetStartPoint(Vector2 startPoint)
	{
		float width = Screen.width;
		float height = Screen.height;

		startPoint = startPoint - new Vector2((width / 2), (height / 2));
	}
}

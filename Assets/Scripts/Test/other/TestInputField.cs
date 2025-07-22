using TMPro;
using UnityEngine;

public class TestInputField : MonoBehaviour
{
	[SerializeField] TMP_InputField input_TMP = null;
	[SerializeField] TextMeshProUGUI text_TMP = null;
	[SerializeField] string solTarget;
	[SerializeField] Color colorInput = new Color(1f, 1f, 1f, 1f);

	//送出按鈕
	public void InputHandler(string input)
	{
		input = input_TMP.text;

		if (input == "")
		{
			text_TMP.text = "答案不能為空白";
		}

		if (CompareInput(input, solTarget, false))
		{
			text_TMP.color = colorInput;
			text_TMP.text = "答案：" + input;
		}
	}

	public void GetInput(string input)
	{
		if (input == "")
			return;

		Debug.Log("輸入值為：" + input);
		//text_TMP.color = colorInput;
		//string solution = text_TMP.text = "我是" + input;

		if (CompareInput(input, solTarget))
		{
			Debug.Log("<color=green>答對!</color>");
			text_TMP.color = Color.green;
			text_TMP.text = "恭喜答對! 我是" + input;
		}
		else
		{
			Debug.Log("<color=red>答錯~</color>");
			text_TMP.color = Color.red;
			text_TMP.text = "答錯囉~ ";
			input_TMP.text = "";
		}
	}

	bool CompareInput(string input, string output, bool equals = true)
	{
		// 如果輸入值 等於 輸出值
		if (equals == true)
		{
			if (input == output)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			// 如果輸入值 包含 輸出值
			if (input.Contains(output))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

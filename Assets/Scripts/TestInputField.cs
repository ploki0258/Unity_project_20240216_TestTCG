using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestInputField : MonoBehaviour
{
	[SerializeField] TMP_InputField input_TMP = null;
	[SerializeField] TextMeshProUGUI text_TMP = null;
	[SerializeField] string solTarget;
	[SerializeField] Color colorInput = new Color(1f, 1f, 1f, 1f);

	//�e�X���s
	public void InputHandler(string input)
	{
		input = input_TMP.text;

		if (input == "")
		{
			text_TMP.text = "���פ��ର�ť�";
		}

		if (CompareInput(input, solTarget, false))
		{
			text_TMP.color = colorInput;
			text_TMP.text = "���סG" + input;
		}
	}

	public void GetInput(string input)
	{
		if (input == "")
			return;

		Debug.Log("��J�Ȭ��G" + input);
		//text_TMP.color = colorInput;
		//string solution = text_TMP.text = "�ڬO" + input;

		if (CompareInput(input, solTarget))
		{
			Debug.Log("<color=green>����!</color>");
			text_TMP.color = Color.green;
			text_TMP.text = "���ߵ���! �ڬO" + input;
		}
		else
		{
			Debug.Log("<color=red>����~</color>");
			text_TMP.color = Color.red;
			text_TMP.text = "�����o~ ";
			input_TMP.text = "";
		}
	}

	bool CompareInput(string input, string output, bool equals = true)
	{
		// �p�G��J�� ���� ��X��
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
			// �p�G��J�� �]�t ��X��
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

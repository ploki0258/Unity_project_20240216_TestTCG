using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCalculate : MonoBehaviour
{
    [SerializeField, Header("��J�� 1")]
    float input_1;
	[SerializeField, Header("��J�� 2")]
	float input_2;
	[SerializeField, Header("�B��O")]
	�B������ �|�h�B�� = �B������.�[�k;

	private void Start()
	{
		Debug.Log($"<color=pink>�p��ȡG{Calculate(input_1, input_2)}</color>");
	}

	float ans;
	float Calculate(float input1, float input2)
	{	
		switch (�|�h�B��)
		{
			case �B������.�[�k:
				ans = input1 + input2;
				break;
			case �B������.��k:
				ans = input1 - input2;
				break;
			case �B������.���k:
				ans = input1 * input2;
				break;
			case �B������.���k:
				ans = input1 / input2;
				break;
		}
		return ans;
	}

	public enum �B������
	{
		�[�k,
		��k,
		���k,
		���k,
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCalculate : MonoBehaviour
{
    [SerializeField, Header("輸入值 1")]
    float input_1;
	[SerializeField, Header("輸入值 2")]
	float input_2;
	[SerializeField, Header("運算別")]
	運算類型 四則運算 = 運算類型.加法;

	private void Start()
	{
		Debug.Log($"<color=pink>計算值：{Calculate(input_1, input_2)}</color>");
	}

	float ans;
	float Calculate(float input1, float input2)
	{	
		switch (四則運算)
		{
			case 運算類型.加法:
				ans = input1 + input2;
				break;
			case 運算類型.減法:
				ans = input1 - input2;
				break;
			case 運算類型.乘法:
				ans = input1 * input2;
				break;
			case 運算類型.除法:
				ans = input1 / input2;
				break;
		}
		return ans;
	}

	public enum 運算類型
	{
		加法,
		減法,
		乘法,
		除法,
	}
}

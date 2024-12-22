using System.Collections.Generic;
using UnityEngine;

public class TestPrimeNumber : MonoBehaviour
{
	[SerializeField, Header("是否為數組")]
	bool isArrayData = false;
	[SerializeField, Header("輸入值")]
	int number;
	[SerializeField, Header("輸入數組")]
	List<int> numbers;

	[Tooltip("質數列表")] List<int> primesList;

	private void Start()
	{
		primesList = new List<int>();

		判斷質數();
	}

	void 判斷質數()
	{
		if (isArrayData == false)
		{
			if (IsPrime(number))
				Debug.Log($"<color=#ff9870>{number}是質數</color>");
			else
				Debug.Log($"<color=#FF7ABF>{number}不是質數</color>");
		}
		else
		{
			Primes(numbers);
		}
	}

	/// <summary>
	/// 判斷是否為質數
	/// </summary>
	/// <param name="number">正整數</param>
	/// <returns>是否為質數 Bool</returns>
	bool IsPrime(int number)
	{
		int 餘 = 0;

		if (number <= 1)
			return false;

		for (int i = 2; i < number; i++)
		{
			//int 商 = (number / i);
			餘 = (number % i);

			//Debug.Log("除數：" + i);
			//Debug.Log("商：" + 商 + "\n餘：" + 餘);

			if (餘 == 0)
			{
				return false;
			}
		}

		if (number == 2) return true;
		return 餘 != 0;
	}

	/// <summary>
	/// 多個質數判斷
	/// </summary>
	/// <param name="numbers">整數列表</param>
	void Primes(List<int> numbers)
	{
		if (isArrayData == false)
			return;

		foreach (int num in numbers)
		{
			if (IsPrime(num))
			{
				Debug.Log($"<color=#ff9870>{num}是質數</color>");
				primesList.Add(num);
			}
			else
			{
				Debug.Log($"<color=#FF7ABF>{num}不是質數</color>");
			}
		}

		int total = primesList.Count;
		Debug.Log($"<color=#789f0f>共有{total}個質數</color>");
	}

	[ContextMenu("產生數組列表")]
	void NumbersNeed()
	{
		for (int i = 0; i < numbers.Count; i++)
		{
			numbers[i] = (i + 1);
		}
	}

	[ContextMenu("產生奇數列表")]
	void NumbersNeed_1()
	{
		int a = 1;
		for (int i = 0; i < numbers.Count; i++)
		{
			numbers[i] = (i + a);
			a++;
		}
	}

	[ContextMenu("產生偶數列表")]
	void NumbersNeed_2()
	{
		int a = 2;
		for (int i = 0; i < numbers.Count; i++)
		{
			numbers[i] = (i + a);
			a++;
		}
	}

	//	後項 = 前兩項相加
	/// <summary>
	/// 費波那契數列
	/// </summary>
	[ContextMenu("產生費波那契列表")]
	void NumbersNeed_rite()
	{
		int front_1 = 1;
		int front_2 = 1;

		//numbers[0] = front_1;
		//numbers[1] = front_2;

		for (int i = 0; i < numbers.Count; i++)
		{
			if (i < 2)
			{
				numbers[i] = 1;
			}
			else if (i >= 2)
			{
				front_1 = numbers[i - 1];       // 前一項
				front_2 = numbers[i - 2];       // 前二項
				int back = front_1 + front_2;   // 後項 = 前兩項相加
				numbers[i] = back;
			}
		}
	}

	[SerializeField] SequenceType 數列類型 = SequenceType.等差;
	[SerializeField] int 首項 = 0;
	[SerializeField, Tooltip("兩數相差之值")] int 公差 = 1; // 公差 d
	[SerializeField, Tooltip("兩數相除之值")] int 公比 = 2; // 公比 r

	/// <summary>
	/// 自定義數列：
	/// 1.等差數列
	/// 2.等比數列
	/// </summary>
	[ContextMenu("產生自定義列表")]
	void NumbersNeed_customized()
	{
		int a = 首項;

		for (int i = 0; i < numbers.Count; i++)
		{
			#region switch 寫法
			switch (數列類型)
			{
				case SequenceType.等差:
					// 第1項
					if (i == 0)
					{
						// 將初始值設定為列表中的第一項
						numbers[i] = 首項;
					}
					else
					{
						// 之後的項次為前項加上差值
						numbers[i] = a + 公差;
						a = numbers[i];
					}
					break;
				case SequenceType.等比:
					if (i == 0)
					{
						numbers[i] = 首項;
					}
					else
					{
						numbers[i] = a * 公比;
						a = numbers[i];
					}
					break;
			}
			#endregion

			#region if 寫法
			/*if (數列類型 == SequenceType.等差)
			{
				if (i == 0)
				{
					numbers[i] = 初始值;
				}
				else
				{
					numbers[i] = a + 差值;
					a = numbers[i];
				}
			}
			else if (數列類型 == SequenceType.等比)
			{
				if (i == 0)
				{
					numbers[i] = 初始值;
				}
				else
				{
					numbers[i] = a * 比值;
					a = numbers[i];
				}
			}*/
			#endregion
		}
	}

	/// <summary>
	/// 數列類型
	/// </summary>
	public enum SequenceType
	{
		等差,
		等比,
	}
}

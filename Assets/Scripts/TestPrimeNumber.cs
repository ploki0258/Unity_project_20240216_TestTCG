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

	List<int> primesList;

	private void Start()
	{
		//numbers = new List<int>();
		primesList = new List<int>();

		判斷質數();

		Primes(numbers);
	}

	void 判斷質數()
	{
		if (isArrayData == false)
		{
			if (Prime(number))
			{
				Debug.Log($"<color=#ff9870>{number}是質數</color>");
			}
			else
			{
				Debug.Log($"<color=#FF7ABF>{number}不是質數</color>");
			}
		}
	}

	/// <summary>
	/// 質數
	/// </summary>
	/// <param name="number">正整數</param>
	/// <returns>是否為質數</returns>
	bool Prime(int number)
	{
		int 餘 = 0;

		if (number <= 1)
			return false;

		for (int i = 2; i < number; i++)
		{
			int 商 = (number / i);
			餘 = (number % i);

			//Debug.Log("除數：" + i);
			//Debug.Log("商：" + 商 + "\n餘：" + 餘);

			if (餘 == 0)
			{
				return false;
			}
			//Debug.Log("平方根：" + Mathf.Sqrt(number));
		}

		return 餘 != 0;
	}

	void Primes(List<int> numbers)
	{
		if (isArrayData == false)
			return;

		foreach (int num in numbers)
		{
			if (Prime(num))
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

	[ContextMenu("產生費波那契列表")]     //	後項 = 前兩項相加
	void NumbersNeed_rite()
	{
		int front_1 = 0;
		int front_2 = 1;

		numbers[0] = front_1;
		numbers[1] = front_2;

		for (int i = 2; i < numbers.Count; i++)
		{
			if (i >= 2)
			{
				front_1 = numbers[i - 1];       // 前一項
				front_2 = numbers[i - 2];       // 前二項
				int back = front_1 + front_2;   // 後項 = 前兩項相加
				numbers[i] = back;
			}
		}
	}

	[SerializeField] SequenceType 數列類型 = SequenceType.等差;
	[SerializeField] int 初始值 = 0;
	[SerializeField] int 差值 = 1;
	[SerializeField] int 比值 = 2;
	[ContextMenu("產生自定義列表")]     //	後項 = 前兩項相加
	void NumbersNeed_customized()
	{
		int a = 初始值;

		for (int i = 0; i < numbers.Count; i++)
		{
			if (數列類型 == SequenceType.等差)
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
			}
		}
	}

	public enum SequenceType
	{
		等差, 等比
	}
}

using System.Collections.Generic;
using UnityEngine;

public class TestPrimeNumber : MonoBehaviour
{
	[SerializeField, Header("是否為數組")]
	bool isArrayData = false;
	[SerializeField, Header("輸入值")]
	int number;
	[SerializeField, Header("輸入數組長度")]
	List<int> numbersLength;
	
	[Tooltip("質數列表")] List<int> primesList;

	private void Start()
	{
		primesList = new List<int>();

		判斷質數();

		//BaboSort(numbersLength);
		QuickSort(numbersLength, 0, numbersLength.Count - 1);
	}

	#region 質數 PrimeNumber
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
			Primes(numbersLength);
		}
	}

	/// <summary>
	/// 判斷是否為質數
	/// </summary>
	/// <param name="number">正整數</param>
	/// <returns>是否為質數 Bool</returns>
	bool IsPrime(int number)
	{
		int 餘數 = 0;
		// 如果 number <= 1，則不是質數
		if (number <= 1)
			return false;
		// 如果 number = 2，則是質數
		if (number == 2) return true;

		for (int i = 2; i < number; i++)
		{
			餘數 = (number % i);
			//int 商 = (number / i);
			//Debug.Log("除數：" + i);
			//Debug.Log($"商：{商}\n餘：{餘數}");

			if (餘數 == 0)
				return false;
		}

		return 餘數 != 0; // 如果餘數不為0，則是質數
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
	#endregion

	#region 數列 Sequence
	[ContextMenu("產生數列")]
	void NumbersNeed()
	{
		for (int i = 0; i < numbersLength.Count; i++)
		{
			numbersLength[i] = (i + 1);
		}
	}

	/*
	 * i = 0, a = 1 => 0 + 1 = 1
	 * i = 1, a = 2 => 1 + 2 = 3
	 * i = 2, a = 3 => 2 + 3 = 5
	 */
	[ContextMenu("產生奇數數列")]
	void NumbersNeed_1()
	{
		int a = 1;
		for (int i = 0; i < numbersLength.Count; i++)
		{
			numbersLength[i] = (i + a);
			a++;
		}
	}

	/*
 * i = 0, a = 2 => 0 + 2 = 2
 * i = 1, a = 3 => 1 + 3 = 4
 * i = 2, a = 4 => 2 + 4 = 6
 */
	[ContextMenu("產生偶數數列")]
	void NumbersNeed_2()
	{
		int a = 2;
		for (int i = 0; i < numbersLength.Count; i++)
		{
			numbersLength[i] = (i + a);
			a++;
		}
	}

	/// <summary>
	/// 費波那契數列
	/// 後項 = 前兩項相加
	/// </summary>
	[ContextMenu("產生費波那契數列")]
	void NumbersNeed_rite()
	{
		int front_1;
		int front_2;

		//numbers[0] = front_1;
		//numbers[1] = front_2;

		for (int i = 0; i < numbersLength.Count; i++)
		{
			if (i < 2)
			{
				numbersLength[i] = 1;
			}
			else if (i >= 2)
			{
				front_1 = numbersLength[i - 1];       // 前一項
				front_2 = numbersLength[i - 2];       // 前二項
				int back = front_1 + front_2;   // 後項 = 前兩項相加
				numbersLength[i] = back;
			}
		}
	}

	[SerializeField] SequenceType 數列類型 = SequenceType.等差;
	[SerializeField] int 首項 = 0;
	[SerializeField, Tooltip("前後兩數相差之值")] int 公差 = 1; // 公差 d
	[SerializeField, Tooltip("前後兩數相除之比")] int 公比 = 2; // 公比 r

	/// <summary>
	/// 自定義數列：
	/// 1.等差數列
	/// 2.等比數列
	/// </summary>
	[ContextMenu("產生自定義列表")]
	void NumbersNeed_customized()
	{
		int a = 首項;

		for (int i = 0; i < numbersLength.Count; i++)
		{
			#region switch 寫法
			switch (數列類型)
			{
				case SequenceType.等差:
					// 第1項
					if (i == 0)
					{
						// 將初始值設定為數列中的第一項
						numbersLength[i] = 首項;
					}
					else
					{
						// 之後的項次為前項加上公差
						numbersLength[i] = a + 公差;
						a = numbersLength[i];
					}
					break;
				case SequenceType.等比:
					if (i == 0)
					{
						numbersLength[i] = 首項;
					}
					else
					{
						numbersLength[i] = a * 公比;
						a = numbersLength[i];
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
	#endregion

	#region 排序法 Sort
	// 冒泡排序法
	List<int> BaboSort(List<int> list, bool 排序方式 = true)
	{
		for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
				if (排序方式)
				{
					// 如果前一個數字 大於 後一個數字，則將兩者交換位置
					if (list[i] > list[j])
					{
						int temp = list[i];
						list[i] = list[j];
						list[j] = temp;
					}
				}
				else
				{
					// 如果前一個數字 小於 後一個數字，則將兩者交換位置
					if (list[i] < list[j])
					{
						int temp = list[i];
						list[i] = list[j];
						list[j] = temp;
					}
				}
            }
        }
		Debug.Log($"<color=#ff9870>排序後的數列：{string.Join(",", list)}</color>");
		return list;
	}

	// 快速排序法
	// 參考網址：https://www.geeksforgeeks.org/quick-sort/
	/// <summary>
	/// 快速排序法
	/// </summary>
	/// <param name="list">整數陣列</param>
	/// <param name="left">左邊界</param>
	/// <param name="right">右邊界</param>
	/// <returns></returns>
	List<int> QuickSort(List<int> list, int left, int right)
	{
		int l = left;   // 左邊界
		int r = right;  // 右邊界
		int pivot = list[(left + right) / 2]; // 中樞數(比較標準)取中間值

		// 當 左邊界 <= 右邊界時，進行排序
		while (l <= r)
		{
			while (list[l] < pivot) l++;    // 從左邊開始找比 pivot 大的數
			while (list[r] > pivot) r--;    // 從右邊開始找比 pivot 小的數
											
			// 如果 l <= r，則交換兩者的數值
			if (l <= r)
			{
				int temp = list[l];
				list[l] = list[r];
				list[r] = temp;
				l++;    // 左邊界右移
				r--;    // 右邊界左移
				Debug.Log($"<color=#ff7098>排序後的數列：{string.Join(",", list)}</color>");
			}
		}
		// 遞迴排序
		// 如果 left < r，則繼續排序
		if (left < r) QuickSort(list, left, r);
		// 如果 l < right，則繼續排序
		if (l < right) QuickSort(list, l, right);

		//Debug.Log($"<color=#ff7098>排序後的數列：{string.Join(",", list)}</color>");
		return list;
	}
	#endregion
}

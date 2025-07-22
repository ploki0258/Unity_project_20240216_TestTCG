using UnityEngine;

public class AddTwoNumbers : MonoBehaviour
{
	[SerializeField] int[] ints;

	private void Start()
	{
		int a1 = ChangeToNumber(ints);
		Debug.Log(a1);
		//Debug.Log(Mathf.Pow(10, 9));
		int[] a2 = ChangeToArray(153);
		foreach (int n in a2)
		{
			Debug.Log(n);
		}
	}

	public int ChangeToNumber(int[] li)
	{
		foreach (int num in li)
		{
			if (num < 0)
			{
				Debug.LogError("數字不能小於0");
				return -1;
			}
		}

		int result = 0;
		if (li.Length > 0)
		{
			for (int i = 0; i < li.Length; i++)
			{
				float n = Mathf.Pow(10, (li.Length - i - 1));
				float a = n * li[i];
				result += (int)a;
				Debug.Log($"n={n}\na={a}\nresult={result}");
			}
			return result;
		}
		else
		{
			return -1;
		}
	}

	/// <summary>
	/// 將一個整數轉為依序的整數陣列
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public int[] ChangeToArray(int num)
	{
		if (num < 0)
		{
			Debug.LogError("數字不能小於0");
			return null;
		}

		int[] result = new int[num.ToString().Length];
		for (int i = 0; i < result.Length; i++)
		{
			result[i] = num % 10;
			num /= 10;
		}
		return result;
	}
}

using System.Collections.Generic;
using UnityEngine;

public class TestPrimeNumber : MonoBehaviour
{
	[SerializeField, Header("�O�_���Ʋ�")]
	bool isArrayData = false;
	[SerializeField, Header("��J��")]
	int number;
	[SerializeField, Header("��J�Ʋ�")]
	List<int> numbers;

	List<int> primesList;

	private void Start()
	{
		//numbers = new List<int>();
		primesList = new List<int>();

		�P�_���();

		Primes(numbers);
	}

	void �P�_���()
	{
		if (isArrayData == false)
		{
			if (Prime(number))
			{
				Debug.Log($"<color=#ff9870>{number}�O���</color>");
			}
			else
			{
				Debug.Log($"<color=#FF7ABF>{number}���O���</color>");
			}
		}
	}

	/// <summary>
	/// ���
	/// </summary>
	/// <param name="number">�����</param>
	/// <returns>�O�_�����</returns>
	bool Prime(int number)
	{
		int �l = 0;

		if (number <= 1)
			return false;

		for (int i = 2; i < number; i++)
		{
			int �� = (number / i);
			�l = (number % i);

			//Debug.Log("���ơG" + i);
			//Debug.Log("�ӡG" + �� + "\n�l�G" + �l);

			if (�l == 0)
			{
				return false;
			}
			//Debug.Log("����ڡG" + Mathf.Sqrt(number));
		}

		return �l != 0;
	}

	void Primes(List<int> numbers)
	{
		if (isArrayData == false)
			return;

		foreach (int num in numbers)
		{
			if (Prime(num))
			{
				Debug.Log($"<color=#ff9870>{num}�O���</color>");
				primesList.Add(num);
			}
			else
			{
				Debug.Log($"<color=#FF7ABF>{num}���O���</color>");
			}
		}

		int total = primesList.Count;
		Debug.Log($"<color=#789f0f>�@��{total}�ӽ��</color>");
	}

	[ContextMenu("���ͼƲզC��")]
	void NumbersNeed()
	{
		for (int i = 0; i < numbers.Count; i++)
		{
			numbers[i] = (i + 1);
		}
	}

	[ContextMenu("���ͩ_�ƦC��")]
	void NumbersNeed_1()
	{
		int a = 1;
		for (int i = 0; i < numbers.Count; i++)
		{
			numbers[i] = (i + a);
			a++;
		}
	}

	[ContextMenu("���Ͱ��ƦC��")]
	void NumbersNeed_2()
	{
		int a = 2;
		for (int i = 0; i < numbers.Count; i++)
		{
			numbers[i] = (i + a);
			a++;
		}
	}

	[ContextMenu("���ͶO�i�����C��")]     //	�ᶵ = �e�ⶵ�ۥ[
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
				front_1 = numbers[i - 1];       // �e�@��
				front_2 = numbers[i - 2];       // �e�G��
				int back = front_1 + front_2;   // �ᶵ = �e�ⶵ�ۥ[
				numbers[i] = back;
			}
		}
	}

	[SerializeField] SequenceType �ƦC���� = SequenceType.���t;
	[SerializeField] int ��l�� = 0;
	[SerializeField] int �t�� = 1;
	[SerializeField] int ��� = 2;
	[ContextMenu("���ͦ۩w�q�C��")]     //	�ᶵ = �e�ⶵ�ۥ[
	void NumbersNeed_customized()
	{
		int a = ��l��;

		for (int i = 0; i < numbers.Count; i++)
		{
			if (�ƦC���� == SequenceType.���t)
			{
				if (i == 0)
				{
					numbers[i] = ��l��;
				}
				else
				{
					numbers[i] = a + �t��;
					a = numbers[i];
				}
			}
			else if (�ƦC���� == SequenceType.����)
			{
				if (i == 0)
				{
					numbers[i] = ��l��;
				}
				else
				{
					numbers[i] = a * ���;
					a = numbers[i];
				}
			}
		}
	}

	public enum SequenceType
	{
		���t, ����
	}
}

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

	[Tooltip("��ƦC��")] List<int> primesList;

	private void Start()
	{
		primesList = new List<int>();

		�P�_���();
	}

	void �P�_���()
	{
		if (isArrayData == false)
		{
			if (IsPrime(number))
				Debug.Log($"<color=#ff9870>{number}�O���</color>");
			else
				Debug.Log($"<color=#FF7ABF>{number}���O���</color>");
		}
		else
		{
			Primes(numbers);
		}
	}

	/// <summary>
	/// �P�_�O�_�����
	/// </summary>
	/// <param name="number">�����</param>
	/// <returns>�O�_����� Bool</returns>
	bool IsPrime(int number)
	{
		int �l = 0;

		if (number <= 1)
			return false;

		for (int i = 2; i < number; i++)
		{
			//int �� = (number / i);
			�l = (number % i);

			//Debug.Log("���ơG" + i);
			//Debug.Log("�ӡG" + �� + "\n�l�G" + �l);

			if (�l == 0)
			{
				return false;
			}
		}

		if (number == 2) return true;
		return �l != 0;
	}

	/// <summary>
	/// �h�ӽ�ƧP�_
	/// </summary>
	/// <param name="numbers">��ƦC��</param>
	void Primes(List<int> numbers)
	{
		if (isArrayData == false)
			return;

		foreach (int num in numbers)
		{
			if (IsPrime(num))
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

	//	�ᶵ = �e�ⶵ�ۥ[
	/// <summary>
	/// �O�i�����ƦC
	/// </summary>
	[ContextMenu("���ͶO�i�����C��")]
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
				front_1 = numbers[i - 1];       // �e�@��
				front_2 = numbers[i - 2];       // �e�G��
				int back = front_1 + front_2;   // �ᶵ = �e�ⶵ�ۥ[
				numbers[i] = back;
			}
		}
	}

	[SerializeField] SequenceType �ƦC���� = SequenceType.���t;
	[SerializeField] int ���� = 0;
	[SerializeField, Tooltip("��Ƭۮt����")] int ���t = 1; // ���t d
	[SerializeField, Tooltip("��Ƭ۰�����")] int ���� = 2; // ���� r

	/// <summary>
	/// �۩w�q�ƦC�G
	/// 1.���t�ƦC
	/// 2.����ƦC
	/// </summary>
	[ContextMenu("���ͦ۩w�q�C��")]
	void NumbersNeed_customized()
	{
		int a = ����;

		for (int i = 0; i < numbers.Count; i++)
		{
			#region switch �g�k
			switch (�ƦC����)
			{
				case SequenceType.���t:
					// ��1��
					if (i == 0)
					{
						// �N��l�ȳ]�w���C�����Ĥ@��
						numbers[i] = ����;
					}
					else
					{
						// ���᪺�������e���[�W�t��
						numbers[i] = a + ���t;
						a = numbers[i];
					}
					break;
				case SequenceType.����:
					if (i == 0)
					{
						numbers[i] = ����;
					}
					else
					{
						numbers[i] = a * ����;
						a = numbers[i];
					}
					break;
			}
			#endregion

			#region if �g�k
			/*if (�ƦC���� == SequenceType.���t)
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
			}*/
			#endregion
		}
	}

	/// <summary>
	/// �ƦC����
	/// </summary>
	public enum SequenceType
	{
		���t,
		����,
	}
}

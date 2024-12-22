//using System;
using UnityEngine;

public class PasswordCreator : MonoBehaviour
{
	[SerializeField, Header("�K�X�O�_�]�t�j�g�^��")] bool isIncludeUppercase;
	[SerializeField, Header("�K�X����")] int passwordLength;

	string numbers = "0123456789";
	string lowercase = "abcdefghijklmnopqrstuvwxyz";
	string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	[Tooltip("�K�X")] string password = "";

	private void Start()
	{
		print($"�K�X�G{RandomPassword(passwordLength, isIncludeUppercase)}");
	}

	string RandomPassword(int length, bool includeUpper = false, bool includeNumbers = true)
	{
		// �̾ڬO�_�]�t�j�g �ӷs�W�K�X�Φr��
		string pw = includeUpper ? numbers + lowercase + uppercase : numbers + lowercase;
		//print($"��K�X�G{pw}");
		// �p�G�]�t�Ʀr
		if (includeNumbers)
		{
			int n = pw.IndexOf('a');
			string pw_num = pw.Substring(0, n);
			//print(pw_num);
			// ���ö���
			for (int i = 0; i < length; i++)
			{
				// �Ʀr����
				int id_1 = Random.Range(0, pw_num.Length);
				int id_2 = Random.Range(0, pw.Length);
				password += pw[id_2];
			}
		}

		return password;
	}
}

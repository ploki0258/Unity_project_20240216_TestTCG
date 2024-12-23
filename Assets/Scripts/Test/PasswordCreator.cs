using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
		string password = RandomPassword(passwordLength, isIncludeUppercase, false);
		//print($"�K�X�G{password}\n���סG{password.Length}");
	}

	string pw_random;
	int id;
	string RandomPassword(int length, bool includeUpper = false, bool isRepeat = true)
	{
		// �̾ڬO�_�]�t�j�g �ӷs�W�K�X�Φr��
		string pw = includeUpper ? numbers + lowercase + uppercase : numbers + lowercase;
		print($"��K�X�G{pw}\n���סG{pw.Length}");

		int n = pw.IndexOf('a');
		// �`���Ʀr����
		string pw_num = pw.Substring(0, n);
		//print(pw_num);

		// ���s���ñK�Xpw����
		
		for (int i = 0; i < pw.Length; i++)
		{
			id = Random.Range(0, pw.Length);
			pw_random += pw[id];

			if (isRepeat == false)
				pw.ToList().RemoveAt(id);
		}

		password = pw_random;

		// ���ö���
		/*for (int i = 0; i < length; i++)
		{
			// �Ʀr����
			int id_1 = Random.Range(0, pw_num.Length);
			int id_2 = Random.Range(0, pw.Length);
			password += pw[id_2];
		}*/

		return password;
	}
}

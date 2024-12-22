//using System;
using UnityEngine;

public class PasswordCreator : MonoBehaviour
{
	[SerializeField, Header("密碼是否包含大寫英文")] bool isIncludeUppercase;
	[SerializeField, Header("密碼長度")] int passwordLength;

	string numbers = "0123456789";
	string lowercase = "abcdefghijklmnopqrstuvwxyz";
	string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	[Tooltip("密碼")] string password = "";

	private void Start()
	{
		print($"密碼：{RandomPassword(passwordLength, isIncludeUppercase)}");
	}

	string RandomPassword(int length, bool includeUpper = false, bool includeNumbers = true)
	{
		// 依據是否包含大寫 來新增密碼用字符
		string pw = includeUpper ? numbers + lowercase + uppercase : numbers + lowercase;
		//print($"原密碼：{pw}");
		// 如果包含數字
		if (includeNumbers)
		{
			int n = pw.IndexOf('a');
			string pw_num = pw.Substring(0, n);
			//print(pw_num);
			// 打亂順序
			for (int i = 0; i < length; i++)
			{
				// 數字部份
				int id_1 = Random.Range(0, pw_num.Length);
				int id_2 = Random.Range(0, pw.Length);
				password += pw[id_2];
			}
		}

		return password;
	}
}

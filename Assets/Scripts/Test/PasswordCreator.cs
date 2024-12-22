//using System;
using UnityEngine;

public class PasswordCreator : MonoBehaviour
{
	[SerializeField, Header("盞絏琌糶璣ゅ")] bool isIncludeUppercase;
	[SerializeField, Header("盞絏")] int passwordLength;

	string numbers = "0123456789";
	string lowercase = "abcdefghijklmnopqrstuvwxyz";
	string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	[Tooltip("盞絏")] string password = "";

	private void Start()
	{
		print($"盞絏{RandomPassword(passwordLength, isIncludeUppercase)}");
	}

	string RandomPassword(int length, bool includeUpper = false, bool includeNumbers = true)
	{
		// ㄌ沮琌糶 ㄓ穝糤盞絏ノ才
		string pw = includeUpper ? numbers + lowercase + uppercase : numbers + lowercase;
		//print($"盞絏{pw}");
		// 狦计
		if (includeNumbers)
		{
			int n = pw.IndexOf('a');
			string pw_num = pw.Substring(0, n);
			//print(pw_num);
			// ゴ睹抖
			for (int i = 0; i < length; i++)
			{
				// 计场
				int id_1 = Random.Range(0, pw_num.Length);
				int id_2 = Random.Range(0, pw.Length);
				password += pw[id_2];
			}
		}

		return password;
	}
}

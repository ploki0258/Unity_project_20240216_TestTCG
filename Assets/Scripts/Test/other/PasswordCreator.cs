using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
		string password = RandomPassword(passwordLength, isIncludeUppercase, false);
		//print($"密碼：{password}\n長度：{password.Length}");
	}

	string pw_random;
	int id;
	string RandomPassword(int length, bool includeUpper = false, bool isRepeat = true)
	{
		// 依據是否包含大寫 來新增密碼用字符
		string pw = includeUpper ? numbers + lowercase + uppercase : numbers + lowercase;
		print($"原密碼：{pw}\n長度：{pw.Length}");

		int n = pw.IndexOf('a');
		// 節錄數字部分
		string pw_num = pw.Substring(0, n);
		//print(pw_num);

		// 重新打亂密碼pw順序
		
		for (int i = 0; i < pw.Length; i++)
		{
			id = Random.Range(0, pw.Length);
			pw_random += pw[id];

			if (isRepeat == false)
				pw.ToList().RemoveAt(id);
		}

		password = pw_random;

		// 打亂順序
		/*for (int i = 0; i < length; i++)
		{
			// 數字部份
			int id_1 = Random.Range(0, pw_num.Length);
			int id_2 = Random.Range(0, pw.Length);
			password += pw[id_2];
		}*/

		return password;
	}
}

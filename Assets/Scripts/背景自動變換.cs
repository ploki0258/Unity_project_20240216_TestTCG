using UnityEngine;

public class 背景自動變換 : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] float changeInterval = 1f; // 背景變換間隔時間

	float lastChangeTime;   // 上次變換背景的時間

	private void Start()
	{
		BackgoundChange();
	}

	private void Update()
	{
		//BackgoundChange(changeInterval);
	}

	void BackgoundChange()
	{
		int randomIndex = Random.Range(0, materials.Length);
		// 改變Lighting的Environment的Skybox
		RenderSettings.skybox = materials[randomIndex];
	}

	void BackgoundChange(float interval)
	{
		// 每隔指定時間變換背景
		if (Time.time - lastChangeTime > interval)
		{
			int randomIndex = Random.Range(0, materials.Length);
			// 改變Lighting的Environment的Skybox
			RenderSettings.skybox = materials[randomIndex];

			lastChangeTime = Time.time;
			Debug.Log($"背景已變換為: {materials[randomIndex].name}");
		}
	}
}
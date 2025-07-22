using UnityEngine;

public class 背景自動變換 : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] float changeInterval = 1f; // 背景變換間隔時間
	[SerializeField] Camera mainCamera; // 主攝像機

	float lastChangeTime;

	private void Update()
	{
		// 每隔指定時間變換背景
		if (Time.time - lastChangeTime > changeInterval)
		{
			int randomIndex = Random.Range(0, materials.Length);
			GetComponent<Renderer>().material = materials[randomIndex];
			
			lastChangeTime = Time.time;
			Debug.Log($"背景已變換為: {materials[randomIndex].name}");
		}
	}
}

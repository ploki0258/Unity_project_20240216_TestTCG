using TMPro;
using UnityEngine;

public class EnergyDisplay : MonoBehaviour
{
	[SerializeField, Header("能量")] TextMeshProUGUI energyText;

	BattleManager battleManager;

	private void Awake()
	{
		battleManager = FindObjectOfType<BattleManager>();
	}

	private void Start()
	{
		UpdateEnergyText();
		battleManager.onEnergyChange += UpdateEnergyText;
	}

	private void OnDisable()
	{
		battleManager.onEnergyChange -= UpdateEnergyText;
	}

	/// <summary>
	/// 更新能量文本顯示
	/// </summary>
	void UpdateEnergyText()
	{
		energyText.text = $"{battleManager.energy} / {battleManager.maxInitEnergy}";
	}
}

using TMPro;
using UnityEngine;

public class EnergyDisplay : MonoBehaviour
{
	[SerializeField, Header("能量")] TextMeshProUGUI energyText;

	private void Start()
	{
		UpdateEnergyText();
		BattleManager.instance.onEnergyChange += UpdateEnergyText;
	}

	private void OnDisable()
	{
		BattleManager.instance.onEnergyChange -= UpdateEnergyText;
	}

	/// <summary>
	/// 更新能量文本顯示
	/// </summary>
	void UpdateEnergyText()
	{
		energyText.text = $"{BattleManager.instance.energy} / {BattleManager.instance.maxInitEnergy}";
	}
}

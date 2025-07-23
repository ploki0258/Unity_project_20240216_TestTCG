using UnityEngine;
using UnityEngine.UI;

public class PhaseDisplay : MonoBehaviour
{
	[SerializeField, Header("階段")] Text phaseText;
	[SerializeField, Header("階段")] Image phaseImg;
	[SerializeField, Header("階段底色")] Color playerPhaseColor, enemyPhaseColor;

	void Start()
	{
		// 初始階段顯示
		UpdatePhaseText();
		BattleManager.instance.onPhaseChange += UpdatePhaseText;
	}

	void OnDestroy()
	{
		BattleManager.instance.onPhaseChange -= UpdatePhaseText;
	}

	void UpdatePhaseText()
	{
		string phase = "";
		Color phaseColor = Color.white;
		switch (BattleManager.instance.currentPhase)
		{
			case PhaseType.none:
				break;
			case PhaseType.gameInit:
				break;
			case PhaseType.playerDraw:
				phase = "Draw Phase";
				phaseColor = playerPhaseColor;
				break;
			case PhaseType.playerMain:
				phase = "Main Phase";
				phaseColor = playerPhaseColor;
				break;
			case PhaseType.playerEnd:
				phase = "End Phase";
				phaseColor = playerPhaseColor;
				break;
			case PhaseType.enemyDraw:
				phase = "Draw Phase";
				phaseColor = enemyPhaseColor;
				break;
			case PhaseType.enemyMain:
				phase = "Main Phase";
				phaseColor = enemyPhaseColor;
				break;
			case PhaseType.enemyEnd:
				phase = "End Phase";
				phaseColor = enemyPhaseColor;
				break;
		}
		phaseText.text = phase;
		phaseImg.color = phaseColor;
	}
}

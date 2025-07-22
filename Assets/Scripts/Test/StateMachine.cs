using UnityEngine;

namespace Jack_Chiu
{
	/// <summary>
	/// 狀態機：
	/// 1.預設狀態
	/// 2.狀態切換
	/// </summary>
	public class StateMachine : MonoBehaviour
	{
		// 當前狀態
		State currentState;

		/// <summary>
		/// 預設狀態
		/// </summary>
		/// <param name="defaultState">預設的狀態</param>
		public void DefaultState(State defaultState)
		{
			// 設定預設狀態
			currentState = defaultState;
			// 進入 當前狀態
			currentState.Enter();
		}

		/// <summary>
		/// 更新當前狀態
		/// </summary>
		public void UpdateState()
		{
			// 更新 當前狀態
			currentState.Update();
		}

		/// <summary>
		/// 切換狀態
		/// </summary>
		/// <param name="switchState">切換後的新狀態</param>
		public void SwitchState(State switchState)
		{
			// 離開 當前狀態
			currentState.Exit();
			// 切換到 新狀態
			currentState = switchState;
			// 進入 當前狀態
			currentState.Enter();
		}
	}
}

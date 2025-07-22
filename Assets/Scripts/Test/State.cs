using UnityEngine;

namespace Jack_Chiu
{
	/// <summary>
	/// 狀態：
	/// 1.進入狀態
	/// 2.更新狀態
	/// 3.離開狀態
	/// </summary>
	public class State : MonoBehaviour
	{
		// 狀態名稱
		string stateName;

		//[field: Header("移動速度")]
		//[field: SerializeField]
		//float moveSpeed { get; set; } = 3.5f;

		protected StateMachine stateMachine;

		public virtual void Enter()
		{
			Debug.Log($"<color=#f69>進入{stateName}狀態</color>");
		}

		public virtual void Update()
		{
			Debug.Log($"<color=#999>更新{stateName}狀態</color>");
		}

		public virtual void Exit()
		{
			Debug.Log($"<color=#f33>離開{stateName}狀態</color>");
		}
	}
}
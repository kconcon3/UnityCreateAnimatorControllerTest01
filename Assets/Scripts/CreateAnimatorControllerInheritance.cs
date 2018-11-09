
#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;

/// <summary>
/// 自作のCreateAnimatorControllerBaseクラスを使って
/// スクリプトからAnimatorControllerを作成するサンプル
/// </summary>
public class CreateAnimatorControllerInheritance : CreateAnimatorControllerBase
{
	/// <summary>
	/// 初期化(生成、ロード、アタッチ)
	/// </summary>
	public void Init()
	{
		// Animator生成
		Create("Assets/Resources/", "Test01_ctrl", new Vector2(0, 0), new Vector2(200, 0));
	}

	/// <summary>
	/// 状態遷移用のParameterの生成
	/// 基底クラスのCreate関数から呼ばれます
	/// </summary>
	protected override void CreateParameters()
	{
		// トリガー変数を追加
		AddParameter("change_state", AnimatorControllerParameterType.Trigger);
	}

	/// <summary>
	/// AnimatorStateの生成
	/// 基底クラスのCreate関数から呼ばれます
	/// </summary>
	protected override void CreateAnimatorState()
	{
		AddAnimatorState("state_01", new Vector2(200, 100));
		AddAnimatorState("state_02", new Vector2(300, 200));
	}

	/// <summary>
	/// 遷移情報の生成
	/// 基底クラスのCreate関数から呼ばれます
	/// </summary>
	protected override void CreateTransition()
	{
		// "change_state"トリガーがたてられた時に"state_01"から"state_02"へ遷移
		AddTransition("state_01", "state_02", AnimatorConditionMode.If, 0.0f, "change_state");

		// "state_01"から"state_02"への遷移のHasExitTimeフラグを立てる
		SetTransitionHasExitTime("state_01", "state_02", true);
	}
}

#endif

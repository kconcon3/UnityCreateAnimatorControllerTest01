
#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;


/// <summary>
/// AnimatiorControllerをスクリプトから生成するクラス
/// 使い方：
///  CreateAnimatorControllerクラスを継承して使用して下さい。
///  実行ファイル状態の時は生成できないので、エディタ専用です。
/// </summary>
public class CreateAnimatorControllerBase
{
	AnimatorController m_animController = null;
	AnimatorStateMachine m_rootState = null;
	Dictionary<string, Int32> m_paramTbl = new Dictionary<string, int>();		// 注：valueは未使用
	Dictionary<string, AnimatorState> m_stateTbl = new Dictionary<string, AnimatorState>();

	/// <summary>
	/// 生成
	/// </summary>
	/// <param name="filePath">AnimatorControllerを生成する場所(ディレクトリのパス)</param>
	/// <param name="fileName">AnimatorControllerを生成するファイル名(拡張子は含まない事)</param>
	/// <param name="anyStatePos"></param>
	/// <param name="entryStatePos"></param>
	public void Create(string filePath, string fileName, Vector2 anyStatePos, Vector2 entryStatePos)
	{
		// pathの最後に'/'が付いていなかったら付加 
		string tmpFilePath = filePath;

		if (tmpFilePath.Length > 0)
		{
			if (tmpFilePath[tmpFilePath.Length - 1] != '/')
			{
				tmpFilePath += '/';
			}
		}

		///// 指定のフォルダに AnimatorController を作成
		m_animController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(tmpFilePath + fileName + ".controller");
		Assert.IsNotNull(m_animController);

		///// rootのState Machine
		m_rootState = m_animController.layers[0].stateMachine;
		Assert.IsNotNull(m_rootState);

		// 座標変更
		m_rootState.anyStatePosition = anyStatePos;
		m_rootState.entryPosition = entryStatePos;

		///// AnimatorControllerの中身を生成 
		CreateParameters();     // Parametersの生成
		CreateAnimatorState();  // AnimatorStateの生成 
		CreateTransition();     // 遷移情報の生成 
	}

	/// <summary>
	/// Parametersの生成
	/// オーバーライドして使用して下さい
	///	Create関数から呼ばれます
	/// </summary>
	protected virtual void CreateParameters()
	{
		Assert.IsTrue(false);
	}

	/// <summary>
	/// AnimatorStateの生成
	/// オーバーライドして使用して下さい
	///	Create関数から呼ばれます
	/// </summary>
	protected virtual void CreateAnimatorState()
	{
		Assert.IsTrue(false);
	}

	/// <summary>
	/// 遷移情報の生成
	/// オーバーライドして使用して下さい
	///	Create関数から呼ばれます
	/// </summary>
	protected virtual void CreateTransition()
	{
		Assert.IsTrue(false);
	}

	/// <summary>
	/// rootのAnimatorStateMachineを取得
	/// </summary>
	/// <returns></returns>
	protected AnimatorStateMachine GetRootStateMachine()
	{
		Assert.IsNotNull(m_rootState);
		return m_rootState;
	}

	/// <summary>
	/// 状態遷移用のパラメータを追加
	/// </summary>
	/// <param name="paramName">パラメータ名</param>
	/// <param name="type">タイプ</param>
	protected void AddParameter(string paramName, AnimatorControllerParameterType type)
	{
		Assert.IsNotNull(m_animController);
		Assert.IsFalse(m_paramTbl.ContainsKey(paramName));

		m_animController.AddParameter(paramName, type);

		m_paramTbl.Add(paramName, 0);
	}

	/// <summary>
	/// Animator内に空のStateを追加
	/// </summary>
	/// <param name="animName">アニメーション名(拡張子は含まない事)</param>
	/// <param name="pos">AnimatorWindowの中での表示位置</param>
	protected void AddAnimatorState(string animName, Vector2 pos)
	{
		Assert.IsNotNull(m_animController);
		Assert.IsFalse(m_stateTbl.ContainsKey(animName));

		// state作成と追加
		AnimatorState state = m_rootState.AddState(animName, pos);
		m_stateTbl.Add(animName, state);
	}

	/// <summary>
	/// 遷移条件を追加
	/// </summary>
	/// <param name="animName1">遷移元のアニメーション名</param>
	/// <param name="animName2">遷移先のアニメーション名</param>
	/// <param name="condition">条件</param>
	/// <param name="val">条件の判定値(Triggerの場合は使用しないはず(Unity側に依存))</param>
	/// <param name="paramName">パラメータ名</param>
	protected void AddTransition(string animName1, string animName2, AnimatorConditionMode condition, float val, string paramName)
	{
		Assert.IsTrue(m_stateTbl.ContainsKey(animName1));
		Assert.IsTrue(m_stateTbl.ContainsKey(animName2));
		Assert.IsTrue(m_paramTbl.ContainsKey(paramName));

		AnimatorState baseState = m_stateTbl[animName1];
		AnimatorState nextState = m_stateTbl[animName2];

		baseState.AddTransition(nextState).AddCondition(condition, val, paramName);  // 遷移条件
	}

	/// <summary>
	/// 遷移条件のhasExitTimeをセット
	/// </summary>
	/// <param name="animName1"></param>
	/// <param name="animName2"></param>
	protected void SetTransitionHasExitTime(string animName1, string animName2, bool flg)
	{
		Assert.IsTrue(m_stateTbl.ContainsKey(animName1));
		Assert.IsTrue(m_stateTbl.ContainsKey(animName2));

		AnimatorState baseState = m_stateTbl[animName1];
		AnimatorState nextState = m_stateTbl[animName2];

		baseState.AddTransition(nextState).hasExitTime = flg;   // 遷移条件
	}

	/// <summary>
	/// Animatorにアタッチ
	/// </summary>
	/// <param name="rAnimator">アタッチするAnimator</param>
	public void Attach(Animator rAnimator)
	{
		// アタッチ 
		rAnimator.runtimeAnimatorController = new AnimatorOverrideController() { runtimeAnimatorController = (RuntimeAnimatorController)m_animController };
	}
}

#endif


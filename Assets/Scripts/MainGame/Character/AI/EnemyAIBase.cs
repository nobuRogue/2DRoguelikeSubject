/**
 * @file EnemyAIBase.cs
 * @brief エネミーAIの基底
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAIBase {
	protected static System.Action<MoveAction> _AddMove = null;

	protected System.Func<CharacterBase> _GetSourceCharacter = null;

	public static void SetAddMoveCallback(System.Action<MoveAction> setProcess) {
		_AddMove = setProcess;
	}

	public EnemyAIBase(System.Func<CharacterBase> SetGetSourceProcess) {
		_GetSourceCharacter = SetGetSourceProcess;
	}

	public abstract void ThinkAction();

}

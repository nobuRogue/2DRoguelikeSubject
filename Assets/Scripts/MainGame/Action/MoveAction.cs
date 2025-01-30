/**
 * @file MoveAction.cs
 * @brief 移動アクション
 * @author yao
 * @date 2025/1/21
 */

using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction {
	private static Action<eFloorEndReason> _EndFloor = null;

	public static void SetEndFloorCallback(Action<eFloorEndReason> setProcess) {
		_EndFloor = setProcess;
	}

	private int _moveCharacterID = -1;
	private ChebyshevMoveData _moveData = null;

	/// <summary>
	/// 内部的な移動処理
	/// </summary>
	public void ProcessData(CharacterBase moveCharacter, ChebyshevMoveData moveData) {
		_moveCharacterID = moveCharacter.ID;
		_moveData = moveData;

		moveCharacter.SetSquareData(MapSquareManager.instance.Get(moveData.targetSquareID));
	}

	/// <summary>
	/// 見た目上の移動処理
	/// </summary>
	/// <param name="duration">移動にかかる秒数</param>
	/// <returns></returns>
	public async UniTask ProcessObject(float duration) {
		CharacterBase moveCharacter = CharacterManager.instance.Get(_moveCharacterID);
		MapSquareData startSquare = MapSquareManager.instance.Get(_moveData.sourceSquareID);
		Vector3 startPos = startSquare.GetCharacterRoot().position;

		MapSquareData goalSquare = MapSquareManager.instance.Get(_moveData.targetSquareID);
		Vector3 goalPos = goalSquare.GetCharacterRoot().position;

		float elapsedTime = 0.0f;
		while (elapsedTime < duration) {
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / duration;
			Vector3 setPos = Vector3.Lerp(startPos, goalPos, t);
			moveCharacter.SetPosition(setPos);
			await UniTask.DelayFrame(1);
		}
		moveCharacter.SetPosition(goalPos);
		_moveCharacterID = -1;
		_moveData = null;

		if (!moveCharacter.IsPlayer()) return;

		if (goalSquare.terrain != eTerrain.Stair) return;

		_EndFloor(eFloorEndReason.Stair);
	}

}

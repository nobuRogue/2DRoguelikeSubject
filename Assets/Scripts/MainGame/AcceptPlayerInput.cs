/**
 * @file AcceptPlayerInput.cs
 * @brief プレイヤーの入力受付
 * @author yao
 * @date 2025/1/21
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Input;

public class AcceptPlayerInput {
	private System.Action<MoveAction> _AddMove = null;

	public void SetAddMoveActionCallback(System.Action<MoveAction> setProcess) {
		_AddMove = setProcess;
	}

	/// <summary>
	/// プレイヤー入力の受付
	/// </summary>
	/// <returns></returns>
	public async UniTask AcceptInput() {
		while (true) {
			if (AcceptMove()) break;

			await UniTask.DelayFrame(1);
		}
	}

	/// <summary>
	/// 移動の受付
	/// </summary>
	/// <returns>移動したか否か</returns>
	public bool AcceptMove() {
		// 8方向の入力を受け付ける
		eDirectionEight dir = AcceptDirInput();
		if (dir == eDirectionEight.Invalid) return false;
		// 移動可否の判定
		PlayerCharacter player = CharacterManager.instance.GetPlayer();
		if (player == null) return false;

		int playerX = player.positionX, playerY = player.positionY;
		MapSquareData playerSquare = MapSquareManager.instance.Get(playerX, playerY);
		MapSquareData moveSquare = MapSquareUtility.GetToDirSquare(playerX, playerY, dir);
		if (!MapSquareUtility.CanMove(playerX, playerY, moveSquare, dir)) return false;
		// 受け付けた入力に応じて移動
		MoveAction moveAction = new MoveAction();
		var moveData = new ChebyshevMoveData(playerSquare.ID, moveSquare.ID, dir);
		moveAction.ProcessData(player, moveData);
		_AddMove(moveAction);
		return true;
	}

	private eDirectionEight AcceptDirInput() {
		if (GetKey(KeyCode.UpArrow)) {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.UpRight;
			} else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.UpLeft;
			} else {
				return eDirectionEight.Up;
			}
		} else if (GetKey(KeyCode.DownArrow)) {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.DownRight;
			} else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.DownLeft;
			} else {
				return eDirectionEight.Down;
			}
		} else {
			if (GetKey(KeyCode.RightArrow)) {
				return eDirectionEight.Right;
			} else if (GetKey(KeyCode.LeftArrow)) {
				return eDirectionEight.Left;
			}
		}
		return eDirectionEight.Invalid;
	}

}

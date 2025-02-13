/**
 * @file EnemyAI00_Normal.cs
 * @brief プレイヤーに向かうAI
 * @author yao
 * @date 2025/1/21
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static CommonModule;

public class EnemyAI00_Normal : EnemyAIBase {
	public EnemyAI00_Normal(Func<CharacterBase> SetGetSourceProcess) : base(SetGetSourceProcess) {

	}

	public override void ThinkAction() {
		// 視界とプレイヤー有無の取得
		CharacterBase sourceCharacter = _GetSourceCharacter();
		MapSquareData sourceSquare = MapSquareManager.instance.Get(sourceCharacter.positionX, sourceCharacter.positionY);
		List<int> visibleArea = null;
		MapSquareUtility.GetVisibleArea(ref visibleArea, sourceSquare);
		// 視界にプレイヤーが居るか
		PlayerCharacter player = CharacterManager.instance.GetPlayer();
		bool visiblePlayer = visibleArea.Exists(player.ExistMoveTrail);
		if (visiblePlayer) {
			// 可能な行動があれば実効

			// 可能な行動が無ければプレイヤーに近づく
			MapSquareData playerSquare = MapSquareManager.instance.Get(player.positionX, player.positionY);
			List<ChebyshevMoveData> toPlayerRoute = RouteSearcher.RouteSearch(sourceSquare.ID, playerSquare.ID, CanPassCharacter);
			if (IsEnableIndex(toPlayerRoute, 1)) {
				// 移動アクションの生成、内部処理実行
				MoveAction toPlayerMove = new MoveAction();
				ChebyshevMoveData currentMove = toPlayerRoute[0];
				sourceCharacter.SetDirection(currentMove.dir);
				toPlayerMove.ProcessData(sourceCharacter, currentMove);
				_AddMove(toPlayerMove);
			}
		} else {
			// プレイヤーが見えていないのでランダム移動
			RandomMove(sourceCharacter, sourceSquare);
		}
	}

	private bool CanPassCharacter(MapSquareData sourceSquare, MapSquareData moveSquare, eDirectionEight dir, int distance) {
		CharacterBase squareCharacter = CharacterManager.instance.Get(moveSquare.characterID);
		if (squareCharacter == null) return MapSquareUtility.CanMove(sourceSquare.positionX, sourceSquare.positionY, moveSquare, dir);

		return squareCharacter.IsPlayer() && MapSquareUtility.CanMoveTerrain(sourceSquare.positionX, sourceSquare.positionY, moveSquare, dir);
	}

	private void RandomMove(CharacterBase sourceCharacter, MapSquareData sourceSquare) {
		// 移動可能な方向を取得
		int sourceX = sourceSquare.positionX, sourceY = sourceSquare.positionY;
		int dirMaxIndex = (int)eDirectionEight.Max;
		List<eDirectionEight> canMoveDirList = new List<eDirectionEight>(dirMaxIndex);
		for (int i = 0; i < dirMaxIndex; i++) {
			var dir = (eDirectionEight)i;
			MapSquareData mapSquare = MapSquareUtility.GetToDirSquare(sourceX, sourceY, dir);
			if (!MapSquareUtility.CanMove(sourceX, sourceY, mapSquare, dir)) continue;

			canMoveDirList.Add(dir);
		}
		if (IsEmpty(canMoveDirList)) return;
		// ランダムな方向に移動
		eDirectionEight moveDir = canMoveDirList[UnityEngine.Random.Range(0, canMoveDirList.Count)];
		MoveAction moveAction = new MoveAction();
		MapSquareData moveSquare = MapSquareUtility.GetToDirSquare(sourceX, sourceY, moveDir);
		var moveData = new ChebyshevMoveData(sourceSquare.ID, moveSquare.ID, moveDir);
		sourceCharacter.SetDirection(moveData.dir);
		moveAction.ProcessData(sourceCharacter, moveData);
		_AddMove(moveAction);
	}

}

/**
 * @file ActionRange02_DirForward10.cs
 * @brief キャラの向き前方10マスの射程
 * @author yao
 * @date 2025/3/13
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CharacterUtility;
using static MapSquareUtility;
using static CommonModule;

public class ActionRange02_DirForward10 : ActionRangeBase {
	public override void Setup(CharacterBase sourceCharacter) {
		InitializeList(ref targetList);
		// 前方1マスを取得
		int sourceX = sourceCharacter.positionX, sourceY = sourceCharacter.positionY;
		MapSquareData sourceSquare = GetCharacterSquare(sourceCharacter);
		MapSquareData targetSquare = GetToDirSquare(sourceX, sourceY, sourceCharacter.direction);
		// 攻撃するマスにキャラが居るか判定
		if (!targetSquare.existCharacter) return;
		// 攻撃可能なマスか判定
		if (!CanAttack(sourceX, sourceY, targetSquare, sourceCharacter.direction)) return;

		CharacterBase targetCharacter = CharacterManager.instance.Get(targetSquare.characterID);
		if (IsRelativeEnemy(sourceCharacter, targetCharacter)) targetList.Add(targetCharacter.ID);

	}
}

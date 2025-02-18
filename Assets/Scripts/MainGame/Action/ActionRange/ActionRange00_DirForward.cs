/**
 * @file ActionRange00_DirForward.cs
 * @brief キャラの向き前方の射程
 * @author yao
 * @date 2025/1/9
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class ActionRange00_DirForward : ActionRangeBase {
	public override void Setup(CharacterBase sourceCharacter) {
		InitializeList(ref targetList);
		// とりあえず前方1マスで実装
		int sourceX = sourceCharacter.positionX, sourceY = sourceCharacter.positionY;
		MapSquareData sourceSquare = MapSquareUtility.GetCharacterSquare(sourceCharacter);
		MapSquareData targetSquare = MapSquareUtility.GetToDirSquare(sourceX, sourceY, sourceCharacter.direction);
		if (!targetSquare.existCharacter) return;

		CharacterBase targetCharacter = CharacterManager.instance.Get(targetSquare.characterID);
		if (sourceCharacter.IsPlayer()) {
			// プレイヤーはエネミーを対象に取る
			if (!targetCharacter.IsPlayer()) targetList.Add(targetCharacter.ID);

		} else {
			// エネミーはプレイヤーを対象に取る
			if (targetCharacter.IsPlayer()) targetList.Add(targetCharacter.ID);

		}

	}

}

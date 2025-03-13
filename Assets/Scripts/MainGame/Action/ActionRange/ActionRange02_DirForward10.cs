/**
 * @file ActionRange02_DirForward10.cs
 * @brief キャラの向き前方10マスの射程
 * @author yao
 * @date 2025/3/13
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MapSquareUtility;
using static CommonModule;

public class ActionRange02_DirForward10 : ActionRangeBase {
	// 射程のマス数
	private readonly int _RANGE_COUNT = 10;

	public override void Setup(CharacterBase sourceCharacter) {
		// 対象リストを初期化
		InitializeList(ref targetList);
		MapSquareData targetSquare = GetCharacterSquare(sourceCharacter);
		eDirectionEight sourceDir = sourceCharacter.direction;
		for (int i = 0; i < _RANGE_COUNT; i++) {
			// 前方1マスを取得
			targetSquare = GetToDirSquare(targetSquare, sourceDir);
			// 壁であれば終了
			if (targetSquare == null ||
				targetSquare.terrain == eTerrain.Wall) break;
			// 対象マスにキャラが居なければ継続
			if (!targetSquare.existCharacter) continue;
			// 対象に追加して終了
			targetList.Add(targetSquare.characterID);
			break;
		}
	}
}

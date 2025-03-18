/**
 * @file ActionEffect001_RecoveryHP.cs
 * @brief HP回復の効果処理
 * @author yao
 * @date 2025/3/11
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect001_RecoveryHP : ActionEffectBase {
	private enum eParamIndex {
		RecoveryValue,  // HP回復量
	}
	// ログメッセージID
	private readonly int _RECOVERY_HP_MESSAGE_ID = 1;

	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		// マスターデータからHP回復量を取得
		int recoveryValue = effectMaster.param[(int)eParamIndex.RecoveryValue];
		List<int> targetList = range.targetList;
		// 対象ごとにHP回復効果の処理
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = CharacterUtility.GetCharacter(targetList[i]);
			if (target == null) continue;
			// ログ表示
			string logMessage = string.Format(_RECOVERY_HP_MESSAGE_ID.ToMessage(), target.GetName(), recoveryValue);
			MenuRogueLog.instance.AddLog(logMessage);
			// HP回復処理
			target.AddHP(recoveryValue);
		}
		await UniTask.Delay(500);
	}
}

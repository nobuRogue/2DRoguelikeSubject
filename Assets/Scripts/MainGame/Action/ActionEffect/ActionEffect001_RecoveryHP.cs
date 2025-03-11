/**
 * @file ActionEffect001_RecoveryHP.cs
 * @brief HP‰ñ•œ‚ÌŒø‰Êˆ—
 * @author yao
 * @date 2025/3/11
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect001_RecoveryHP : ActionEffectBase {
	private enum eParamIndex {
		RecoveryValue,  // HP‰ñ•œ—Ê
	}

	private readonly int _RECOVERY_HP_MESSAGE_ID = 1;

	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		int recoveryValue = effectMaster.param[(int)eParamIndex.RecoveryValue];
		List<int> targetList = range.targetList;
		// ‘ÎÛ‚²‚Æ‚ÉŒø‰Ê‚Ìˆ—
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = CharacterUtility.GetCharacter(targetList[i]);
			if (target == null) continue;

			MenuRogueLog.instance.AddLog(string.Format(_RECOVERY_HP_MESSAGE_ID.ToMessage(), recoveryValue));
			target.AddHP(recoveryValue);
		}
		await UniTask.Delay(500);
	}
}

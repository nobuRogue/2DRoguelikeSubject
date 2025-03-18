/**
 * @file ActionEffect002_RecoveryStamina.cs
 * @brief 満腹度回復の効果処理
 * @author yao
 * @date 2025/3/13
 */


using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class ActionEffect002_RecoveryStamina : ActionEffectBase {
	private enum eParamIndex {
		RecoveryValue,  // 満腹度回復量
	}
	// ログメッセージID
	private readonly int _RECOVERY_STAMINA_MESSAGE_ID = 2;

	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		// マスターデータから満腹度回復量を取得
		int recoveryValue = effectMaster.param[(int)eParamIndex.RecoveryValue];
		List<int> targetList = range.targetList;
		// 対象ごとに満腹度回復効果の処理
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = CharacterUtility.GetCharacter(targetList[i]);
			if (target == null) continue;
			// プレイヤーでなければ処理しない
			if (!target.IsPlayer()) continue;
			// ログ表示
			string logMessage = string.Format(_RECOVERY_STAMINA_MESSAGE_ID.ToMessage(), target.GetName(), recoveryValue / 100);
			MenuRogueLog.instance.AddLog(logMessage);
			// 満腹度回復処理
			(target as PlayerCharacter).AddStamina(recoveryValue);
		}
		await UniTask.Delay(500);
	}
}

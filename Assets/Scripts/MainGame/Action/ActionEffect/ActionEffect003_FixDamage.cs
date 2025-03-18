/**
 * @file ActionEffect003_FixDamage.cs
 * @brief 固定ダメージの効果処理
 * @author yao
 * @date 2025/2/18
 */
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

using static CommonModule;

public class ActionEffect003_FixDamage : ActionEffectBase {
	private enum eParamIndex {
		DamageValue,   // ダメージ量
	}
	// SEのID
	private const int _ATTACK_HIT_SE_ID = 0;

	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		// 行動者の攻撃アニメーション再生
		sourceCharacter.SetAnimation(eCharacterAnimation.Attack);

		int damageValue = effectMaster.param[(int)eParamIndex.DamageValue];
		List<int> targetList = range.targetList;
		int targetCount = targetList.Count;
		List<UniTask> taskList = new List<UniTask>(targetCount);
		// 対象ごとに攻撃の処理
		for (int i = 0; i < targetCount; i++) {
			CharacterBase target = CharacterManager.instance.Get(targetList[i]);
			if (target == null) continue;
			// SEの再生
			UniTask task = SoundManager.instance.PlaySE(_ATTACK_HIT_SE_ID);
			taskList.Add(ExecuteFixDamage(damageValue, target));
		}
		// 攻撃アニメーションの終了待ち
		while (sourceCharacter.GetCurrentAnimation() == eCharacterAnimation.Attack) await UniTask.DelayFrame(1);

		await WaitTask(taskList);
	}

	private async UniTask ExecuteFixDamage(int damageValue, CharacterBase targetCharacter) {
		// 対象の被ダメージアニメーション
		targetCharacter.SetAnimation(eCharacterAnimation.Damage);
		await ExecuteDamage(damageValue, targetCharacter);
	}

}

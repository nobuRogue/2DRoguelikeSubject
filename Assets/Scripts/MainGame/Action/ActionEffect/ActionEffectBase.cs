/**
 * @file ActionEffectBase.cs
 * @brief 行動の効果の基底
 * @author yao
 * @date 2025/2/18
 */

using Cysharp.Threading.Tasks;
using System;

public abstract class ActionEffectBase {
	protected static Action<eDungeonEndReason> _EndDungeon = null;

	private readonly int _DAMAGE_LOG_ID = 0;

	public static void SetEndCallback(Action<eDungeonEndReason> setDungeonProcess) {
		_EndDungeon = setDungeonProcess;
	}

	public abstract UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range);

	public virtual void TearDown() {

	}

	/// <summary>
	/// ダメージを与える
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="targetCharacter"></param>
	/// <returns></returns>
	protected async UniTask ExecuteDamage(int damage, CharacterBase targetCharacter) {
		// ログ表示
		string logMessage = string.Format(_DAMAGE_LOG_ID.ToMessage(), targetCharacter.GetName(), damage);
		MenuRogueLog.instance.AddLog(logMessage);
		// HPを減らす
		targetCharacter.RemoveHP(damage);
		// アニメーションの終了待ち
		while (targetCharacter.GetCurrentAnimation() == eCharacterAnimation.Damage) await UniTask.DelayFrame(1);
		// 死亡判定、処理
		if (!targetCharacter.IsDead()) return;

		await CharacterUtility.DeadCharacter(targetCharacter);
	}

}

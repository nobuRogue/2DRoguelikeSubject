/**
 * @file ActionManager.cs
 * @brief 行動の管理
 * @author yao
 * @date 2025/2/18
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class ActionManager {
	private static List<ActionEffectBase> _actionEffectList = null;
	// アクションを使用したときのログメッセージID
	private static readonly int _USE_ACTION_LOG_ID = 3;

	public static void Initialize() {
		_actionEffectList = new List<ActionEffectBase>();
		_actionEffectList.Add(new ActionEffect000_Attack());
		_actionEffectList.Add(new ActionEffect001_RecoveryHP());
		_actionEffectList.Add(new ActionEffect002_RecoveryStamina());
		_actionEffectList.Add(new ActionEffect003_FixDamage());
		_actionEffectList.Add(new ActionEffect004_BurnItem());
	}

	/// <summary>
	/// アクション実行
	/// </summary>
	/// <param name="sourceCharacter"></param>
	/// <param name="actionID"></param>
	/// <returns></returns>
	public static async UniTask ExecuteAction(CharacterBase sourceCharacter, int actionID) {
		Entity_ActionData.Param actionMaster = ActionMasterUtility.GetActionMaster(actionID);
		if (actionMaster == null) return;

		ActionRangeBase range = ActionRangeManager.GetRange(actionMaster.rangeType);
		if (range == null) return;
		// ログ表示
		string sourceName = sourceCharacter.GetName();
		string actionName = actionMaster.nameID.ToMessage();
		string logMessage = string.Format(_USE_ACTION_LOG_ID.ToMessage(), sourceName, actionName);
		MenuRogueLog.instance.AddLog(logMessage);
		// 射程設定
		range.Setup(sourceCharacter);
		// アクションの効果処理
		int[] effectIDList = actionMaster.effectID;
		for (int i = 0, max = effectIDList.Length; i < max; i++) {
			if (effectIDList[i] < 0) continue;

			await ExecuteActionEffect(effectIDList[i], sourceCharacter, range);
		}
	}

	/// <summary>
	/// アクション効果実行
	/// </summary>
	/// <param name="effectID"></param>
	/// <param name="sourceCharacter"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	private static async UniTask ExecuteActionEffect(int effectID, CharacterBase sourceCharacter, ActionRangeBase range) {
		var effectMaster = ActionMasterUtility.GetActionEffectMaster(effectID);
		if (effectMaster == null) return;

		int effectIndex = effectMaster.effectType;
		if (!IsEnableIndex(_actionEffectList, effectIndex)) return;

		await _actionEffectList[effectIndex].Execute(sourceCharacter, effectMaster, range);
		_actionEffectList[effectIndex].TearDown();
	}

}

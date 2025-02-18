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

	public static void Initialize() {
		_actionEffectList = new List<ActionEffectBase>();
		_actionEffectList.Add(new ActionEffect000_Attack());
	}

	/// <summary>
	/// アクション実行
	/// </summary>
	/// <param name="effectType"></param>
	/// <param name="sourceCharacter"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public static async UniTask ExecuteAction(int effectType, CharacterBase sourceCharacter, ActionRangeBase range) {
		if (!IsEnableIndex(_actionEffectList, effectType)) return;

		await _actionEffectList[effectType].Execute(sourceCharacter, range);
	}

}

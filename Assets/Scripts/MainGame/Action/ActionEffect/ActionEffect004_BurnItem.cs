/**
 * @file ActionEffect004_BurnItem.cs
 * @brief キャラクターの持ち物を全て焼く
 * @author yao
 * @date 2025/3/18
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect004_BurnItem : ActionEffectBase {

	private readonly int _BURN_ITEM_LOG_ID = 4;

	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		List<int> targetList = range.targetList;
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = CharacterUtility.GetCharacter(targetList[i]);
			if (target == null) continue;
			// 対象のアイテムを全て焼く
			BurnAllItem(target);
		}
		await UniTask.Delay(500);
	}

	/// <summary>
	///	指定キャラクターの所持アイテムを全て焼く
	/// </summary>
	/// <param name="target"></param>
	private void BurnAllItem(CharacterBase target) {
		List<int> itemList = target.possessItemList;
		for (int i = 0, max = itemList.Count; i < max; i++) {
			BurnItem(ItemUtility.GetItemData(itemList[i]));
		}
	}

	/// <summary>
	/// アイテム一つを焼く
	/// </summary>
	/// <param name="itemData"></param>
	private void BurnItem(ItemBase itemData) {
		// 変化先を確認
		var itemMaster = ItemMasterUtility.GetItemMaster(itemData.masterID);
		if (itemMaster.burnID < 0) return;
		// 変化させる
		string logMessage = string.Format(_BURN_ITEM_LOG_ID.ToMessage(), itemData.GetItemName());
		MenuRogueLog.instance.AddLog(logMessage);
		itemData.ChangeMasterID(itemMaster.burnID);
	}

}

/**
 * @file ActionEffect005_RotItem.cs
 * @brief キャラクターの持ち物を全て腐らせる
 * @author yao
 * @date 2025/3/25
 */

using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class ActionEffect005_RotItem : ActionEffectBase {

	private readonly int _ROT_ITEM_LOG_ID = 5;

	public override async UniTask Execute(
		CharacterBase sourceCharacter,
		Entity_ActionEffectData.Param effectMaster,
		ActionRangeBase range) {
		List<int> targetList = range.targetList;
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = CharacterUtility.GetCharacter(targetList[i]);
			if (target == null) continue;
			// 対象のアイテムを全て腐らせる
			RotAllItem(target);
		}
		await UniTask.Delay(500);
	}

	/// <summary>
	///	指定キャラクターの所持アイテムを全て腐らせる
	/// </summary>
	/// <param name="target"></param>
	private void RotAllItem(CharacterBase target) {
		List<int> itemList = target.possessItemList;
		for (int i = 0, max = itemList.Count; i < max; i++) {
			RotItem(ItemUtility.GetItemData(itemList[i]));
		}
	}

	/// <summary>
	/// アイテム一つを腐らせる
	/// </summary>
	/// <param name="itemData"></param>
	private void RotItem(ItemBase itemData) {
		// 変化先を確認
		var itemMaster = ItemMasterUtility.GetItemMaster(itemData.masterID);
		if (itemMaster.rotID < 0) return;
		// 変化させる
		string logMessage = string.Format(_ROT_ITEM_LOG_ID.ToMessage(), itemData.GetItemName());
		MenuRogueLog.instance.AddLog(logMessage);
		itemData.ChangeMasterID(itemMaster.rotID);
	}

}

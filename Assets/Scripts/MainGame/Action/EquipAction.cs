/**
 * @file EquipAction.cs
 * @brief 装備の着脱アクション
 * @author yao
 * @date 2025/3/27
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static ItemUtility;

public class EquipAction {
	// ログメッセージID
	private static int _EQUIP_LOG_ID = 8;
	private static int _REMOVE_LOG_ID = 9;

	/// <summary>
	/// 指定IDのアイテムをプレイヤーに装備させる
	/// </summary>
	/// <param name="itemID"></param>
	/// <returns></returns>
	public static async UniTask ExecuteSetEquip(int itemID) {
		ItemBase itemData = GetItemData(itemID);
		if (itemData == null) return;

		PlayerCharacter player = CharacterUtility.GetPlayer();
		switch (itemData.GetCategory()) {
			case eItemCategory.Weapon:
			player.SetWeapon(itemID);
			string weaponLog = string.Format(_EQUIP_LOG_ID.ToMessage(), itemData.GetItemName());
			MenuRogueLog.instance.AddLog(weaponLog);
			break;
			case eItemCategory.Armor:
			player.SetArmor(itemID);
			string armorLog = string.Format(_EQUIP_LOG_ID.ToMessage(), itemData.GetItemName());
			MenuRogueLog.instance.AddLog(armorLog);
			break;
		}
		await UniTask.CompletedTask;
	}

	/// <summary>
	/// 指定IDのカテゴリの装備をプレイヤーに外させる
	/// </summary>
	/// <param name="itemID"></param>
	/// <returns></returns>
	public static async UniTask ExecuteRemoveEquip(int itemID) {
		ItemBase itemData = GetItemData(itemID);
		if (itemData == null) return;

		PlayerCharacter player = CharacterUtility.GetPlayer();
		switch (itemData.GetCategory()) {
			case eItemCategory.Weapon:
			player.RemoveWeapon();
			string weaponLog = string.Format(_REMOVE_LOG_ID.ToMessage(), itemData.GetItemName());
			MenuRogueLog.instance.AddLog(weaponLog);
			break;
			case eItemCategory.Armor:
			player.RemoveArmor();
			string armorLog = string.Format(_REMOVE_LOG_ID.ToMessage(), itemData.GetItemName());
			MenuRogueLog.instance.AddLog(armorLog);
			break;
		}
		await UniTask.CompletedTask;
	}

}

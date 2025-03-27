/**
 * @file MenuItemCommandList.cs
 * @brief キャラクターの持ち物を全て腐らせる
 * @author yao
 * @date 2025/3/25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemCommandList : MenuList {

	public async void Setup(int itemID, MenuListCallbackFortmat callbackFortmat) {
		// コールバックの設定
		SetCallbackFortmat(callbackFortmat);
		await SetIndex(-1);
		// 項目をすべて消す
		RemoveAllItem();
		// 項目の設定
		AddItemCommand(itemID);
		await SetIndex(0);
	}

	private void AddItemCommand(int itemID) {
		var itemData = ItemUtility.GetItemData(itemID);
		PlayerCharacter player = CharacterUtility.GetPlayer();
		// アイテムに応じたコマンドリスト設定
		MenuItemCommandListItem addItem = null;
		switch (itemData.GetCategory()) {
			case eItemCategory.Potion:
			case eItemCategory.Food:
			case eItemCategory.Wand:
			addItem = AddListItem() as MenuItemCommandListItem;
			addItem.Setup(eItemCommand.Use);
			break;
			case eItemCategory.Scroll:
			break;
			case eItemCategory.Bag:
			break;
			case eItemCategory.Throwing:
			break;
			case eItemCategory.Weapon:
			addItem = AddListItem() as MenuItemCommandListItem;
			if (player.equipWeaponID == itemID) {
				// 装備中の武器なので外すコマンド追加
				addItem.Setup(eItemCommand.RemoveEquip);
			} else {
				// 装備コマンド追加
				addItem.Setup(eItemCommand.SetEquip);
			}
			break;
			case eItemCategory.Armor:
			addItem = AddListItem() as MenuItemCommandListItem;
			if (player.equipArmorID == itemID) {
				// 装備中の武器なので外すコマンド追加
				addItem.Setup(eItemCommand.RemoveEquip);
			} else {
				// 装備コマンド追加
				addItem.Setup(eItemCommand.SetEquip);
			}
			break;
		}

		addItem = AddListItem() as MenuItemCommandListItem;
		addItem.Setup(eItemCommand.Puton);
	}

}

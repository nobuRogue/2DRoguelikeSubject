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
		// アイテムに応じたコマンドリスト設定
		var addItem = AddListItem() as MenuItemCommandListItem;
		addItem.Setup(eItemCommand.Use);

		await SetIndex(0);
	}

}

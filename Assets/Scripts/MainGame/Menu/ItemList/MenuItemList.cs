/**
 * @file MenuListItem.cs
 * @brief リスト項目の基底クラス
 * @author yao
 * @date 2025/3/6
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MenuItemList : MenuList {

	/// <summary>
	/// リスト項目の生成
	/// </summary>
	/// <param name="itemList"></param>
	/// <returns></returns>
	public async UniTask Setup( List<int> itemList,
		MenuListCallbackFortmat itemListCallbackFortmat ) {
		// アイテムリスト決定時の処理だけはここで設定
		SetCallbackFortmat( itemListCallbackFortmat );

		await SetIndex( -1 );
		RemoveAllItem();
		if (IsEmpty( itemList )) return;
		// 項目の生成
		bool existItem = false;
		PlayerCharacter player = CharacterUtility.GetPlayer();
		for (int i = 0, max = itemList.Count; i < max; i++) {
			var itemID = itemList[i];
			if (itemID < 0) break;
			// 項目有無の判定
			if (!existItem) existItem = true;
			// 項目の生成
			var addItem = AddListItem() as MenuItemListItem;
			addItem.Setup( itemID, player.IsEquip( itemID ) );
		}
		if (existItem) await SetIndex( 0 );

	}
}

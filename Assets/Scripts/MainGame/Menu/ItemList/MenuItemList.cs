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
	[SerializeField]
	private MenuItemCommandList commandList = null;

	public override async UniTask Initialize() {
		await base.Initialize();
		await commandList.Initialize();
	}

	private System.Func<MenuListItem, UniTask<bool>> _OnDecide = null;
	private MenuListCallbackFortmat _commandFormat = null;

	/// <summary>
	/// リスト項目の生成
	/// </summary>
	/// <param name="itemList"></param>
	/// <returns></returns>
	public async UniTask Setup(List<int> itemList,
		MenuListCallbackFortmat itemListCallbackFortmat,
		System.Func<MenuListItem, UniTask<bool>> SetOnItemListDecide,
		MenuListCallbackFortmat itemCommandListCallbackFortmat) {

		// アイテムリスト決定時の処理だけはここで設定
		itemListCallbackFortmat.OnDecide = OnItemListDecide;
		SetCallbackFortmat(itemListCallbackFortmat);
		_OnDecide = SetOnItemListDecide;

		_commandFormat = itemCommandListCallbackFortmat;

		await SetIndex(-1);
		RemoveAllItem();
		if (IsEmpty(itemList)) return;
		// 項目の生成
		bool existItem = false;
		for (int i = 0, max = itemList.Count; i < max; i++) {
			if (itemList[i] < 0) break;
			// 項目有無の判定
			if (!existItem) existItem = true;
			// 項目の生成
			var addItem = AddListItem() as MenuItemListItem;
			addItem.Setup(itemList[i]);
		}
		if (existItem) await SetIndex(0);

	}

	private async UniTask<bool> OnItemListDecide(MenuListItem decideItem) {
		await _OnDecide(decideItem);
		// アイテムコマンドリストを開く
		var itemListItem = decideItem as MenuItemListItem;
		commandList.Setup(itemListItem.itemID, _commandFormat);

		await UniTask.DelayFrame(1);
		await commandList.Open();
		await commandList.AcceptInput();
		await commandList.Close();
		return true;
	}

}

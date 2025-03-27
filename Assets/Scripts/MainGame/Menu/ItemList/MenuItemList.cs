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
	// アイテムリストで決定が押されたとき呼び出すコールバック
	private System.Func<MenuListItem, UniTask<bool>> _OnDecide = null;
	// コマンドリストでキャンセルが押されたとき呼び出すコールバック
	private System.Func<MenuListItem, UniTask<bool>> _OnCommandCancel = null;
	private bool _isCommandCancel = false;
	private MenuListCallbackFortmat _commandFormat = null;

	/// <summary>
	/// リスト項目の生成
	/// </summary>
	/// <param name="itemList"></param>
	/// <returns></returns>
	public async UniTask Setup(List<int> itemList,
		MenuListCallbackFortmat itemListCallbackFortmat,
		System.Func<MenuListItem, UniTask<bool>> SetOnItemListDecide,
		MenuListCallbackFortmat commandListCallbackFortmat,
		System.Func<MenuListItem, UniTask<bool>> SetOnCommandCancel) {

		// アイテムリスト決定時の処理だけはここで設定
		itemListCallbackFortmat.OnDecide = OnItemListDecide;
		SetCallbackFortmat(itemListCallbackFortmat);
		_OnDecide = SetOnItemListDecide;

		// コマンドリストキャンセル時の処理はここで設定
		commandListCallbackFortmat.OnCancel = OnCommandCancel;
		_commandFormat = commandListCallbackFortmat;
		_OnCommandCancel = SetOnCommandCancel;

		await SetIndex(-1);
		RemoveAllItem();
		if (IsEmpty(itemList)) return;
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
			addItem.Setup(itemID, player.IsEquip(itemID));
		}
		if (existItem) await SetIndex(0);

	}

	private async UniTask<bool> OnItemListDecide(MenuListItem decideItem) {
		await _OnDecide(decideItem);
		// アイテムコマンドリストを開く
		var itemListItem = decideItem as MenuItemListItem;
		commandList.Setup(itemListItem.itemID, _commandFormat);

		_isCommandCancel = false;
		await UniTask.DelayFrame(1);
		await commandList.Open();
		await commandList.AcceptInput();
		await commandList.Close();
		await UniTask.DelayFrame(1);
		// コマンドリストでキャンセルされていればアイテムリスト選択は継続
		return _isCommandCancel ? false : true;
	}

	private async UniTask<bool> OnCommandCancel(MenuListItem currentItem) {
		_isCommandCancel = true;

		return await _OnCommandCancel(currentItem);
	}
}

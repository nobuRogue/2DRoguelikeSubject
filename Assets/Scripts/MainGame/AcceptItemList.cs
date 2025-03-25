/**
 * @file AcceptItemList.cs
 * @brief アイテムリストの受付処理
 * @author yao
 * @date 2025/3/25
 */

using Cysharp.Threading.Tasks;
using UnityEngine;

using static UnityEngine.Input;
using static CharacterUtility;
using static MenuList;

public class AcceptItemList {
	// アイテムリストのコールバック集
	private MenuListCallbackFortmat _itemListFormat = null;
	private MenuListCallbackFortmat _itemCommandListFormat = null;
	// アイテムリストで選択されたアイテムID
	private int _selectItemID = -1;
	private eItemCommand _selectItemCommand = eItemCommand.Invalid;

	public AcceptItemList() {
		// アイテムリストのコールバック生成
		_itemListFormat = new MenuListCallbackFortmat();
		_itemListFormat.OnCancel = CloseItemList;// キャンセル時の処理
		_itemListFormat.FreeAccept = AcceptSortPlayerItem;//ソートの受付

		_itemCommandListFormat = new MenuListCallbackFortmat();
		_itemCommandListFormat.OnDecide = DecideItemCommand;
		_itemCommandListFormat.OnCancel = CancelItemCommand;
	}

	public async UniTask<bool> Accept() {
		// アイテムリストの選択受付
		var itemList = MenuManager.instance.Get<MenuItemList>();
		await itemList.Setup(GetPlayer().possessItemList, _itemListFormat, DecideItemList, _itemCommandListFormat);
		await itemList.Open();
		await itemList.AcceptInput();
		await itemList.Close();
		// アイテムが選択されてなければ終了
		if (_selectItemID < 0 ||
			_selectItemCommand == eItemCommand.Invalid) return false;
		// アイテムコマンド毎の処理
		switch (_selectItemCommand) {
			case eItemCommand.Use:
			// 使用したアイテム効果処理
			var itemMaster = ItemUtility.GetItemMasterFromID(_selectItemID);
			await ActionManager.ExecuteAction(GetPlayer(), itemMaster.actionID);
			// 使用したアイテムの消費
			ItemUtility.GetItemData(_selectItemID)?.Consume();
			break;
			case eItemCommand.Puton:
			// アイテムを地面に置く処理
			MapSquareData playerSquare = MapSquareUtility.GetCharacterSquare(GetPlayer());
			await PutonAction.ExecutePuton(playerSquare, _selectItemID);
			break;
		}
		_selectItemID = -1;
		_selectItemCommand = eItemCommand.Invalid;
		return true;
	}

	/// <summary>
	/// アイテムリスト決定時処理
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> DecideItemList(MenuListItem currentItem) {
		// 決定したアイテム項目のアイテムIDを取得しておく
		var itemListItem = currentItem as MenuItemListItem;
		if (itemListItem == null) return true;

		_selectItemID = itemListItem.itemID;
		await UniTask.CompletedTask;
		return true;
	}

	/// <summary>
	/// アイテムリストの入力受付を終了する
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> CloseItemList(MenuListItem currentItem) {
		await UniTask.CompletedTask;
		return true;
	}

	/// <summary>
	/// アイテムコマンドリストで決定が押されたとき
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> DecideItemCommand(MenuListItem currentItem) {
		var commandItem = currentItem as MenuItemCommandListItem;
		if (commandItem == null) return true;

		_selectItemCommand = commandItem.command;
		await UniTask.CompletedTask;
		return true;
	}

	/// <summary>
	/// アイテムコマンドリストでキャンセルが押されたとき
	/// </summary>
	/// <param name="currentItem"></param>
	/// <returns></returns>
	private async UniTask<bool> CancelItemCommand(MenuListItem currentItem) {
		_selectItemID = -1;
		await UniTask.CompletedTask;
		return true;
	}

	private async UniTask<bool> AcceptSortPlayerItem(MenuListItem currentItem) {
		if (!GetKeyDown(KeyCode.V)) return false;
		// プレイヤーの所持アイテムをソートする
		PlayerCharacter player = GetPlayer();
		player.possessItemList.Sort(ItemSortMethod);
		var itemList = MenuManager.instance.Get<MenuItemList>();
		await itemList.Setup(player.possessItemList, _itemListFormat, DecideItemList, _itemCommandListFormat);
		return false;
	}

	/// <summary>
	/// アイテムリストのソート関数
	/// </summary>
	/// <param name="itemID_A"></param>
	/// <param name="itemID_B"></param>
	/// <returns></returns>
	private int ItemSortMethod(int itemID_A, int itemID_B) {
		var itemAMaster = ItemUtility.GetItemMasterFromID(itemID_A);
		var itemBMaster = ItemUtility.GetItemMasterFromID(itemID_B);
		return itemAMaster.ID - itemBMaster.ID;
	}
}

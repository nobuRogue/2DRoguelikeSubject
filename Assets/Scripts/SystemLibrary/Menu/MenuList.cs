/**
 * @file MenuList.cs
 * @brief リストメニューの基底
 * @author yao
 * @date 2025/3/6
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MenuList : MenuBase {
	/// <summary>
	/// リスト項目のオリジナル
	/// </summary>
	[SerializeField]
	private MenuListItem _itemOrigin = null;
	/// <summary>
	/// 項目を並べるルートオブジェクト
	/// </summary>
	[SerializeField]
	private Transform _contentRoot = null;
	/// <summary>
	/// 未使用状態の項目のルートオブジェクト
	/// </summary>
	[SerializeField]
	private Transform _unuseRoot = null;

	/// <summary>
	/// リストメニューのコールバック集クラス
	/// </summary>
	protected class MenuListCallbackFortmat {
		// 決定された際の処理
		public System.Func<MenuListItem, UniTask<bool>> OnDecide = null;
		// キャンセルされた際の処理
		public System.Func<MenuListItem, UniTask<bool>> OnCancel = null;
		// カーソルが移動した際の処理
		public System.Func<MenuListItem, MenuListItem, UniTask<bool>> OnMoveCursor = null;
	}
	private MenuListCallbackFortmat _currentFormat = null;

	private int _currentIndex = -1;
	private bool _isContinue = false;

	private List<MenuListItem> _useList = null;
	private List<MenuListItem> _unuseList = null;

	public override async UniTask Initialize() {
		await base.Initialize();
		_useList = new List<MenuListItem>();
		_unuseList = new List<MenuListItem>();
	}

	/// <summary>
	/// コールバック用クラスの設定
	/// </summary>
	/// <param name="setFormat"></param>
	protected void SetCallbackFortmat(MenuListCallbackFortmat setFormat) {
		_currentFormat = setFormat;
	}

	/// <summary>
	/// リスト項目の生成
	/// </summary>
	/// <returns></returns>
	protected MenuListItem AddListItem() {
		MenuListItem addItem;
		if (IsEmpty(_unuseList)) {
			// 未使用リストが空なので生成
			addItem = Instantiate(_itemOrigin, _contentRoot);
		} else {
			// 未使用リストから取得
			addItem = _unuseList[0];
			_unuseList.RemoveAt(0);
			addItem.transform.SetParent(_contentRoot);
		}
		addItem.Deselect();
		_useList.Add(addItem);
		return addItem;
	}
}

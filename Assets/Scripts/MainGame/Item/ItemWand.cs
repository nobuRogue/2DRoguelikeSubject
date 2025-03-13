/**
 * @file ItemWand.cs
 * @brief 杖アイテムデータ
 * @author yao
 * @date 2025/3/6
 */

using UnityEngine;
public class ItemWand : ItemBase {
	// 使用可能回数
	private int _count = -1;

	public override void Setup(int setID, int setMasterID, MapSquareData square) {
		base.Setup(setID, setMasterID, square);
		var itemMaster = ItemMasterUtility.GetItemMaster(masterID);
		_count = Random.Range(itemMaster.minCount, itemMaster.maxCount + 1);
	}

	/// <summary>
	/// カテゴリ取得
	/// </summary>
	/// <returns></returns>
	public override eItemCategory GetCategory() {
		return eItemCategory.Wand;
	}

	public override string GetItemName() {
		_itemNameStringBuilder.Append(base.GetItemName());
		_itemNameStringBuilder.Append("[");
		_itemNameStringBuilder.Append(_count);
		_itemNameStringBuilder.Append("]");
		string result = _itemNameStringBuilder.ToString();
		_itemNameStringBuilder.Clear();
		return result;
	}

	/// <summary>
	/// 消費処理
	/// </summary>
	public override void Consume() {
		if (_count > 1) {
			_count--;
		} else {
			base.Consume();
		}
	}
}

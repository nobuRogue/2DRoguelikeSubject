/**
 * @file ItemScroll.cs
 * @brief 巻物アイテムデータ
 * @author yao
 * @date 2025/3/6
 */
public class ItemBag : ItemBase {
	/// <summary>
	/// カテゴリ取得
	/// </summary>
	/// <returns></returns>
	public override eItemCategory GetCategory() {
		return eItemCategory.Bag;
	}
}

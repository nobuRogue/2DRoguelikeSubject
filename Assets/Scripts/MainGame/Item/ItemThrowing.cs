/**
 * @file ItemThrowing.cs
 * @brief 投げ物アイテムデータ
 * @author yao
 * @date 2025/3/6
 */
public class ItemThrowing : ItemBase {
	/// <summary>
	/// カテゴリ取得
	/// </summary>
	/// <returns></returns>
	public override eItemCategory GetCategory() {
		return eItemCategory.Throwing;
	}
}

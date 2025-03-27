/**
 * @file ItemArmor.cs
 * @brief 防具アイテムデータ
 * @author yao
 * @date 2025/3/27
 */
public class ItemArmor : ItemBase {
	/// <summary>
	/// カテゴリ取得
	/// </summary>
	/// <returns></returns>
	public override eItemCategory GetCategory() {
		return eItemCategory.Armor;
	}
}

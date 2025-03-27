/**
 * @file ItemWeapon.cs
 * @brief 武器アイテムデータ
 * @author yao
 * @date 2025/3/6
 */
public class ItemWeapon : ItemBase {



	/// <summary>
	/// カテゴリ取得
	/// </summary>
	/// <returns></returns>
	public override eItemCategory GetCategory() {
		return eItemCategory.Weapon;
	}
}

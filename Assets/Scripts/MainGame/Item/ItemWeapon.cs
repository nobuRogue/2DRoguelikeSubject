/**
 * @file ItemWeapon.cs
 * @brief 武器アイテムデータ
 * @author yao
 * @date 2025/3/6
 */
public class ItemWeapon : ItemBase {
	/// <summary>
	/// 武器の攻撃力
	/// </summary>
	private int _attack = 0;

	public override void Setup(int setID, int setMasterID, MapSquareData square) {
		base.Setup(setID, setMasterID, square);
		var itemMaster = ItemMasterUtility.GetItemMaster(masterID);
		_attack = itemMaster.equipValue;
	}

	/// <summary>
	/// カテゴリ取得
	/// </summary>
	/// <returns></returns>
	public override eItemCategory GetCategory() {
		return eItemCategory.Weapon;
	}

	/// <summary>
	/// 攻撃力を取得
	/// </summary>
	/// <returns></returns>
	public override int GetAttack() {
		return _attack;
	}
}

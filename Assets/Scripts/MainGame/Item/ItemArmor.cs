/**
 * @file ItemArmor.cs
 * @brief –h‹ïƒAƒCƒeƒ€ƒf[ƒ^
 * @author yao
 * @date 2025/3/27
 */
public class ItemArmor : ItemBase {
	/// <summary>
	/// –h‹ï‚Ì–hŒä—Í
	/// </summary>
	private int _defense = 0;

	public override void Setup(int setID, int setMasterID, MapSquareData square) {
		base.Setup(setID, setMasterID, square);
		var itemMaster = ItemMasterUtility.GetItemMaster(masterID);
		_defense = itemMaster.equipValue;
	}

	/// <summary>
	/// ƒJƒeƒSƒŠæ“¾
	/// </summary>
	/// <returns></returns>
	public override eItemCategory GetCategory() {
		return eItemCategory.Armor;
	}

	/// <summary>
	/// –hŒä—Í‚ğæ“¾
	/// </summary>
	/// <returns></returns>
	public override int GetDefense() {
		return _defense;
	}
}

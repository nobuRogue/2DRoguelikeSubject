/**
 * @file PlayerCharacter.cs
 * @brief プレイヤーキャラクター
 * @author yao
 * @date 2025/1/21
 */

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class PlayerCharacter : CharacterBase {

	private PlayerMoveObserver _moveObserver = null;

	private List<int> _moveTrailSquareList = null;
	private readonly int PLAYER_MOVE_TRAIL_COUNT = 3;

	// 初期満腹度
	private const int _MAX_STAMINA = 10000;
	private const int _SHOW_STAMINA_RATIO = 100;
	private const int _TURN_DECREASE_STAMINA = 10;
	// 現在の満腹度
	private int _stamina = 0;
	// 装備の情報
	public int equipWeaponID { get; private set; } = -1;
	public int equipArmorID { get; private set; } = -1;

	public override void Setup(int setID, MapSquareData squareData, int masterID) {
		_moveTrailSquareList = new List<int>(PLAYER_MOVE_TRAIL_COUNT);
		base.Setup(setID, squareData, masterID);
	}

	public override void ResetStatus() {
		base.ResetStatus();
		SetStamina(_MAX_STAMINA);
	}

	public override void SetMaxHP(int setValue) {
		base.SetMaxHP(setValue);
		MenuPlayerStatus.instance.SetHP(HP, maxHP);
	}

	public override void SetHP(int setValue) {
		base.SetHP(setValue);
		MenuPlayerStatus.instance.SetHP(HP, maxHP);
	}

	public override void SetRawAttack(int setValue) {
		base.SetRawAttack(setValue);
		MenuPlayerStatus.instance.SetAttack(rawAttack);
	}

	public override void SetRawDefense(int setValue) {
		base.SetRawDefense(setValue);
		MenuPlayerStatus.instance.SetDefense(rawDefense);
	}

	/// <summary>
	/// 攻撃力の取得
	/// </summary>
	/// <returns></returns>
	public override int GetAttack() {
		// 武器の攻撃力を取得
		int weaponAttack = 0;
		ItemBase weapon = ItemUtility.GetItemData(equipWeaponID);
		if (weapon != null) weaponAttack = weapon.GetAttack();

		return base.GetAttack() + weaponAttack;
	}

	/// <summary>
	/// 防御力の取得
	/// </summary>
	/// <returns></returns>
	public override int GetDefense() {
		// 防具の防御力を取得
		int armorDefense = 0;
		ItemBase armor = ItemUtility.GetItemData(equipArmorID);
		if (armor != null) armorDefense = armor.GetDefense();

		return base.GetDefense() + armorDefense;
	}

	public void SetStamina(int setValue) {
		// 0～10000で丸める
		_stamina = Mathf.Clamp(setValue, 0, _MAX_STAMINA);
		// UIへの反映
		MenuPlayerStatus.instance.SetStamina(GetShowStamina());
	}

	public void AddStamina(int addValue) {
		SetStamina(_stamina + addValue);
	}

	public void RemoveStamina(int removeValue) {
		SetStamina(_stamina - removeValue);
	}

	/// <summary>
	/// 満腹度を%表記に変換
	/// </summary>
	/// <returns></returns>
	private int GetShowStamina() {
		return (_stamina + _SHOW_STAMINA_RATIO - 1) / _SHOW_STAMINA_RATIO;
	}

	public void SetMoveObserver(PlayerMoveObserver setObserver) {
		_moveObserver = setObserver;
	}

	public override bool IsPlayer() {
		return true;
	}

	/// <summary>
	/// 情報のみの移動
	/// </summary>
	/// <param name="squareData"></param>
	public override void SetSquareData(MapSquareData squareData) {
		base.SetSquareData(squareData);
		AddMoveTrail(squareData);
	}

	/// <summary>
	/// ターン終了時処理
	/// </summary>
	/// <returns></returns>
	public override async UniTask OnEndTurn() {
		await base.OnEndTurn();
		if (_stamina <= 0) {
			// HPが減る
			RemoveHP(1);
			if (IsDead()) await CharacterUtility.DeadCharacter(this);

		} else {
			// 満腹度が減る
			RemoveStamina(_TURN_DECREASE_STAMINA);
			if (!IsDead()) AddHP(1);

		}
	}

	/// <summary>
	/// フロア終了時処理
	/// </summary>
	public override void OnEndFloor() {
		base.OnEndFloor();
		// 移動軌跡をクリア
		ClearMoveTrail();
	}

	/// <summary>
	/// 移動軌跡マスリストにマスを追加
	/// </summary>
	/// <param name="addSquare"></param>
	private void AddMoveTrail(MapSquareData addSquare) {
		if (_moveTrailSquareList.Exists(trailSquareID => trailSquareID == addSquare.ID)) return;

		while (_moveTrailSquareList.Count >= PLAYER_MOVE_TRAIL_COUNT) {
			MapSquareManager.instance.Get(_moveTrailSquareList[0])?.HideMark();
			_moveTrailSquareList.RemoveAt(0);
		}
		addSquare.ShowMark(Color.blue);
		_moveTrailSquareList.Add(addSquare.ID);
	}

	/// <summary>
	/// 移動軌跡マスをクリア
	/// </summary>
	private void ClearMoveTrail() {
		if (IsEmpty(_moveTrailSquareList)) return;

		for (int i = 0, max = _moveTrailSquareList.Count; i < max; i++) {
			MapSquareManager.instance.Get(_moveTrailSquareList[i])?.HideMark();
		}
		_moveTrailSquareList.Clear();
	}

	/// <summary>
	/// 移動軌跡マスリストに指定のマスIDが含まれているか
	/// </summary>
	/// <param name="squareID"></param>
	/// <returns></returns>
	public bool ExistMoveTrail(int squareID) {
		if (IsEmpty(_moveTrailSquareList)) return false;

		return _moveTrailSquareList.Exists(trailSquareID => trailSquareID == squareID);
	}

	/// <summary>
	/// 見た目の移動
	/// </summary>
	/// <param name="position"></param>
	public override void SetPosition(Vector3 position) {
		base.SetPosition(position);
		if (_moveObserver != null) _moveObserver.OnPlayerMove(position);

	}

	/// <summary>
	/// ID指定の所持アイテム除去
	/// </summary>
	/// <param name="removeItemID"></param>
	public override void RemoveIDItem(int removeItemID) {
		// 装備なら外れる
		if (equipWeaponID == removeItemID) {
			RemoveWeapon();
		} else if (equipArmorID == removeItemID) {
			RemoveArmor();
		}
		base.RemoveIDItem(removeItemID);
	}

	/// <summary>
	/// 武器を装備させる
	/// </summary>
	/// <param name="itemID"></param>
	public void SetWeapon(int itemID) {
		// 武器を着けているなら外す
		if (equipWeaponID >= 0) RemoveWeapon();

		equipWeaponID = itemID;
	}

	/// <summary>
	/// 武器を外させる
	/// </summary>
	public void RemoveWeapon() {
		equipWeaponID = -1;
	}

	/// <summary>
	/// 防具を装備させる
	/// </summary>
	/// <param name="itemID"></param>
	public void SetArmor(int itemID) {
		// 防具を着けているなら外す
		if (equipArmorID >= 0) RemoveArmor();

		equipArmorID = itemID;
	}

	/// <summary>
	/// 防具を外させる
	/// </summary>
	public void RemoveArmor() {
		equipArmorID = -1;
	}

	public bool IsEquip(int itemID) {
		if (itemID < 0) return false;

		return equipWeaponID == itemID || equipArmorID == itemID;
	}
}

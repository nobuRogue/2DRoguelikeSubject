/**
 * @file PlayerCharacter.cs
 * @brief プレイヤーキャラクター
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static CommonModule;

public class PlayerCharacter : CharacterBase {

	private PlayerMoveObserver _moveObserver = null;

	private List<int> _moveTrailSquareList = null;
	private readonly int PLAYER_MOVE_TRAIL_COUNT = 3;

	public override void Setup(int setID, MapSquareData squareData, int masterID) {
		_moveTrailSquareList = new List<int>(PLAYER_MOVE_TRAIL_COUNT);
		base.Setup(setID, squareData, masterID);
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


}

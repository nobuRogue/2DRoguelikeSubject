/**
 * @file PlayerCharacter.cs
 * @brief プレイヤーキャラクター
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase {

	private PlayerMoveObserver _moveObserver = null;

	public void Setup(int setID, MapSquareData squareData) {
		base.Setup(setID, squareData);

	}

	public void SetMoveObserver(PlayerMoveObserver setObserver) {
		_moveObserver = setObserver;
	}

	public override bool IsPlayer() {
		return true;
	}

	public override void SetPosition(Vector3 position) {
		base.SetPosition(position);
		if (_moveObserver != null) _moveObserver.OnPlayerMove(position);

	}


}

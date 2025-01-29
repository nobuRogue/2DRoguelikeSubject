/**
 * @file EnemyCharacter.cs
 * @brief エネミーキャラクター
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterBase {

	private EnemyAIBase _currentAI = null;

	public void Setup(int setID, MapSquareData squareData) {
		base.Setup(setID, squareData);
		_currentAI = new EnemyAI00_Normal(() => this);
	}

	public override bool IsPlayer() {
		return false;
	}

	public override void ThinkAction() {
		_currentAI.ThinkAction();
	}

}

/**
 * @file CharacterBase.cs
 * @brief キャラクターの基底
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase {
	protected static System.Func<int, CharacterObject> _GetObject = null;

	public static void SetGetObjectCallback(System.Func<int, CharacterObject> setCallback) {
		_GetObject = setCallback;
	}

	public int ID { get; protected set; } = -1;
	public int positionX { get; protected set; } = -1;
	public int positionY { get; protected set; } = -1;
	/// <summary>
	/// キャラの向き
	/// </summary>
	public eDirectionEight direction { get; protected set; } = eDirectionEight.Invalid;
	public int maxHP { get; protected set; } = -1;
	public int HP { get; private set; } = -1;
	public int attack { get; private set; } = -1;
	public int defense { get; private set; } = -1;

	public virtual void Setup(int setID, MapSquareData squareData, int masterID) {
		ID = setID;
		SetSquare(squareData);
		var characterMaster = CharacterMasterUtility.GetCharacterMaster(masterID);
		if (characterMaster != null) {
			SetMaxHP(characterMaster.HP);
			SetHP(characterMaster.HP);
			SetAttack(characterMaster.Attack);
			SetDefense(characterMaster.Defense);
		}
		_GetObject(ID).Setup(characterMaster);
		SetDirection(eDirectionEight.Down);
	}

	public void Teardown() {
		_GetObject(ID).Teardown();
		ID = -1;
	}

	/// <summary>
	/// キャラの向き設定
	/// </summary>
	/// <param name="dir"></param>
	public void SetDirection(eDirectionEight dir) {
		if (direction == dir) return;

		direction = dir;
		_GetObject(ID).SetDirection(direction);
	}

	public virtual void SetMaxHP(int setValue) {
		maxHP = setValue;
	}

	public virtual void SetHP(int setValue) {
		HP = setValue;
	}

	public virtual void SetAttack(int setValue) {
		attack = setValue;
	}

	public virtual void SetDefense(int setValue) {
		defense = setValue;
	}

	/// <summary>
	/// 見た目と情報、両方の変更
	/// </summary>
	/// <param name="squareData"></param>
	public void SetSquare(MapSquareData squareData) {
		SetSquareData(squareData);
		SetPosition(squareData.GetCharacterRoot().position);
	}

	/// <summary>
	/// 情報のみの移動
	/// </summary>
	/// <param name="squareData"></param>
	public virtual void SetSquareData(MapSquareData squareData) {
		MapSquareData prevSquare = MapSquareManager.instance.Get(positionX, positionY);
		if (prevSquare != null) prevSquare.RemoveCharacter();

		positionX = squareData.positionX;
		positionY = squareData.positionY;
		squareData.SetCharacter(ID);
	}

	/// <summary>
	/// 見た目の移動
	/// </summary>
	/// <param name="position"></param>
	public virtual void SetPosition(Vector3 position) {
		_GetObject(ID).SetPosition(position);
	}

	public abstract bool IsPlayer();

	public virtual void ThinkAction() {

	}

	/// <summary>
	/// フロア終了時処理
	/// </summary>
	public virtual void OnEndFloor() {

	}

	/// <summary>
	/// アニメーション再生
	/// </summary>
	/// <param name="setAnim"></param>
	public void SetAnimation(eCharacterAnimation setAnim) {
		_GetObject(ID).SetAnimation(setAnim);
	}

}

/**
 * @file CharacterBase.cs
 * @brief キャラクターの基底
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase {
	public int positionX { get; protected set; } = -1;
	public int positionY { get; protected set; } = -1;

	public void Setup(MapSquareData squareData) {
		SetSquare(squareData);
	}

	public void Teardown() {

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
	public void SetSquareData(MapSquareData squareData) {

	}

	/// <summary>
	/// 見た目の移動
	/// </summary>
	/// <param name="position"></param>
	public void SetPosition(Vector3 position) {

	}

}

/**
 * @file CharacterBase.cs
 * @brief �L�����N�^�[�̊��
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
	/// �����ڂƏ��A�����̕ύX
	/// </summary>
	/// <param name="squareData"></param>
	public void SetSquare(MapSquareData squareData) {
		SetSquareData(squareData);
		SetPosition(squareData.GetCharacterRoot().position);
	}

	/// <summary>
	/// ���݂̂̈ړ�
	/// </summary>
	/// <param name="squareData"></param>
	public void SetSquareData(MapSquareData squareData) {

	}

	/// <summary>
	/// �����ڂ̈ړ�
	/// </summary>
	/// <param name="position"></param>
	public void SetPosition(Vector3 position) {

	}

}

/**
 * @file MapSquareData.cs
 * @brief 1�}�X�̏��
 * @author yao
 * @date 2025/1/9
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareData {
	// �}�X�I�u�W�F�N�g���擾����R�[���o�b�N
	private static System.Func<int, MapSquareObject> _GetObject = null;
	public static void SetGetObjectCallback(System.Func<int, MapSquareObject> setCallback) {
		_GetObject = setCallback;
	}

	public int ID { get; private set; } = -1;
	public int squarePositionX { get; private set; } = -1;
	public int squarePositionY { get; private set; } = -1;
	public eTerrain terrain { get; private set; } = eTerrain.Invalid;

	public void Setup(int setID, int setX, int setY) {
		ID = setID;
		squarePositionX = setX;
		squarePositionY = setY;
		_GetObject(ID)?.Setup(squarePositionX, squarePositionY);
	}

	public void SetTerrain(eTerrain setTerrain, int spriteIndex) {
		terrain = setTerrain;
		_GetObject(ID)?.SetTerrain(terrain, spriteIndex);
	}

}

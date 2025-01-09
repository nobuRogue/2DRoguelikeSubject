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
	public Vector2Int squarePosition { get; private set; } = new Vector2Int(-1, -1);
	public eTerrain terrain { get; private set; } = eTerrain.Invalid;

	public void Setup(int setID, Vector2Int setSquarePosition) {
		ID = setID;
		squarePosition = setSquarePosition;
		_GetObject(ID)?.Setup(squarePosition);
	}

	public void SetTerrain(eTerrain setTerrain) {
		terrain = setTerrain;
		_GetObject(ID)?.SetTerrain(terrain);
	}

}

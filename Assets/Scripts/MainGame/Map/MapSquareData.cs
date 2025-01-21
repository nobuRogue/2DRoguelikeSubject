/**
 * @file MapSquareData.cs
 * @brief 1マスの情報
 * @author yao
 * @date 2025/1/9
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareData {
	// マスオブジェクトを取得するコールバック
	private static System.Func<int, MapSquareObject> _GetObject = null;
	public static void SetGetObjectCallback(System.Func<int, MapSquareObject> setCallback) {
		_GetObject = setCallback;
	}

	public int ID { get; private set; } = -1;
	public int positionX { get; private set; } = -1;
	public int positionY { get; private set; } = -1;
	public eTerrain terrain { get; private set; } = eTerrain.Invalid;
	public int roomID { get; private set; } = -1;

	public void Setup(int setID, int setX, int setY) {
		ID = setID;
		positionX = setX;
		positionY = setY;
		_GetObject(ID)?.Setup(positionX, positionY);
	}

	public void SetTerrain(eTerrain setTerrain, int spriteIndex = -1) {
		terrain = setTerrain;
		_GetObject(ID)?.SetTerrain(terrain, spriteIndex);
	}

	public void SetRoomID(int setID) {
		roomID = setID;
	}

	public Transform GetCharacterRoot() {
		return _GetObject(ID)?.GetCharacterRoot();
	}

}

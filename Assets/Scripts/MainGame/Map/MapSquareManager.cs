/**
 * @file MapSquareManager.cs
 * @brief マスの管理
 * @author yao
 * @date 2025/1/9
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MapSquareManager : MonoBehaviour {
	[SerializeField]
	private MapSquareObject _squareObjectOrigin = null; // マスオブジェクトのオリジナル

	[SerializeField]
	private Transform _squareObjectRoot = null;     // マスオブジェクトの親

	public static MapSquareManager instance { get; private set; } = null;

	private List<MapSquareData> _squareDataList = null;
	private List<MapSquareObject> _squareObjectList = null;

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize() {
		instance = this;
		MapSquareData.SetGetObjectCallback(GetSquareObject);
		int squareCount = GameConst.MAP_SQUARE_HEIGHT_COUNT * GameConst.MAP_SQUARE_WIDTH_COUNT;
		_squareDataList = new List<MapSquareData>(squareCount);
		_squareObjectList = new List<MapSquareObject>(squareCount);
		// マスの生成
		for (int i = 0; i < squareCount; i++) {
			// オブジェクト生成
			MapSquareObject createObject = Instantiate(_squareObjectOrigin, _squareObjectRoot);
			_squareObjectList.Add(createObject);
			// データ生成
			MapSquareData createSquare = new MapSquareData();
			createSquare.Setup(i, GetSquarePosition(i));
			_squareDataList.Add(createSquare);
			createSquare.SetTerrain(eTerrain.Wall);
		}
	}

	private MapSquareObject GetSquareObject(int ID) {
		if (!IsEnableIndex(_squareObjectList, ID)) return null;

		return _squareObjectList[ID];
	}

	/// <summary>
	/// IDを2次元座標に変換
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public Vector2Int GetSquarePosition(int ID) {
		Vector2Int result = new Vector2Int();
		result.x = ID % GameConst.MAP_SQUARE_WIDTH_COUNT;
		result.y = ID / GameConst.MAP_SQUARE_WIDTH_COUNT;
		return result;
	}


}

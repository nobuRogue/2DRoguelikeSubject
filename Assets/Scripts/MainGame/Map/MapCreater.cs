/**
 * @file MapCreater.cs
 * @brief ランダムマップ生成
 * @author yao
 * @date 2025/1/14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class MapCreater {

	private class AreaData {
		public int startX = -1;
		public int startY = -1;
		public int width = -1;
		public int height = -1;

		public AreaData(int setX, int setY, int setWidth, int setHeight) {
			startX = setX;
			startY = setY;
			width = setWidth;
			height = setHeight;
		}
	}

	private static List<AreaData> _areaList = null;
	private static List<int> _devideLineList = null;

	// エリア分割回数
	private static readonly int AREA_DEVIDE_COUNT = 8;
	// 最小部屋サイズ
	private static readonly int MIN_ROOM_SIZE = 3;

	public static void CreateMap() {
		// 壁で埋める
		_devideLineList = new List<int>(MAP_SQUARE_WIDTH_COUNT * MAP_SQUARE_HEIGHT_COUNT);
		MapSquareManager.instance.ExecuteAllSquare(SetWall);
		// 最初のエリアを作る
		_areaList = new List<AreaData>(AREA_DEVIDE_COUNT + 1);
		_areaList.Add(new AreaData(2, 2, MAP_SQUARE_WIDTH_COUNT - 4, MAP_SQUARE_HEIGHT_COUNT - 4));
		// エリアを分割する
		DevideArea();
		// 部屋を置く

		// 部屋を繋げる

		// 階段を置く

	}

	private static void SetWall(MapSquareData square) {
		square?.SetTerrain(eTerrain.Wall, 0);
		int x = square.squarePositionX, y = square.squarePositionY;
		if (x == 0 || x == MAP_SQUARE_WIDTH_COUNT - 1 ||
			y == 0 || y == MAP_SQUARE_HEIGHT_COUNT - 1) return;

		if (x != 1 && x != MAP_SQUARE_WIDTH_COUNT - 2 &&
			y != 1 && y != MAP_SQUARE_HEIGHT_COUNT - 2) return;

		square.SetTerrain(eTerrain.Wall, 2);
		_devideLineList.Add(square.ID);
	}

	private static void DevideArea() {
		for (int i = 0; i < AREA_DEVIDE_COUNT; i++) {
			// 幅最大のエリアを取得
			AreaData maxSizeArea = GetMaxSizeArea(out int maxSize, out bool isVertical);
			if (maxSizeArea == null || maxSize < (MIN_ROOM_SIZE + 2) * 2 + 1) break;
			// 取得したエリアを分割
			DevideArea(maxSizeArea, isVertical);
		}
	}

	/// <summary>
	/// 最大サイズのエリアを取得
	/// </summary>
	/// <param name="maxSize">エリアのサイズ</param>
	/// <param name="isVertical">縦か横か</param>
	/// <returns></returns>
	private static AreaData GetMaxSizeArea(out int maxSize, out bool isVertical) {
		maxSize = -1;
		isVertical = false;
		AreaData result = null;
		for (int i = 0, max = _areaList.Count; i < max; i++) {
			AreaData area = _areaList[i];
			if (area.width > maxSize) {
				maxSize = area.width;
				isVertical = false;
				result = area;
			}

			if (area.height > maxSize) {
				maxSize = area.height;
				isVertical = true;
				result = area;
			}
		}
		return result;
	}

	/// <summary>
	/// 指定したエリアを分割する
	/// </summary>
	/// <param name="devideArea"></param>
	/// <param name="isVertical"></param>
	private static void DevideArea(AreaData devideArea, bool isVertical) {
		if (isVertical) {
			// 水平に分割する処理
			DevideAreaVertical(devideArea);
		} else {
			// 垂直に分割する処理
			DevideAreaHorizontal(devideArea);
		}
	}

	/// <summary>
	/// 水平方向のエリア分割処理
	/// </summary>
	/// <param name="devideArea"></param>
	private static void DevideAreaVertical(AreaData devideArea) {
		// 分割位置の決定
		int randomMax = devideArea.height - (MIN_ROOM_SIZE + 2) * 2;
		int devidePos = Random.Range(0, randomMax);
		devidePos += MIN_ROOM_SIZE + 2 + devideArea.startY;
		// 新しいエリアの生成
		int newAreaHeight = devideArea.startY + devideArea.height - devidePos - 1;
		_areaList.Add(new AreaData(devideArea.startX, devidePos + 1, devideArea.width, newAreaHeight));
		// 既存エリアの修正
		devideArea.height = devidePos - devideArea.startY;
		// 分割線マスの追加
		for (int x = 0, max = devideArea.width; x < max; x++) {
			MapSquareData square = MapSquareManager.instance.Get(devideArea.startX + x, devidePos);
			square.SetTerrain(eTerrain.Wall, 2);
			_devideLineList.Add(square.ID);
		}
	}

	/// <summary>
	/// 垂直方向のエリア分割処理
	/// </summary>
	/// <param name="devideArea"></param>
	private static void DevideAreaHorizontal(AreaData devideArea) {
		// 分割位置の決定
		int randomMax = devideArea.width - (MIN_ROOM_SIZE + 2) * 2;
		int devidePos = Random.Range(0, randomMax);
		devidePos += MIN_ROOM_SIZE + 2 + devideArea.startX;
		// 新しいエリアの生成
		int newAreaWidth = devideArea.startX + devideArea.width - devidePos - 1;
		_areaList.Add(new AreaData(devidePos + 1, devideArea.startY, newAreaWidth, devideArea.height));
		// 既存エリアの修正
		devideArea.width = devidePos - devideArea.startX;
		// 分割線マスの追加
		for (int y = 0, max = devideArea.height; y < max; y++) {
			MapSquareData square = MapSquareManager.instance.Get(devidePos, devideArea.startY + y);
			square.SetTerrain(eTerrain.Wall, 2);
			_devideLineList.Add(square.ID);
		}
	}

}

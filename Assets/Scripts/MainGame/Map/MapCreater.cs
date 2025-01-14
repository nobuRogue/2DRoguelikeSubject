/**
 * @file MapCreater.cs
 * @brief �����_���}�b�v����
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

	public static void CreateMap() {
		// �ǂŖ��߂�
		_devideLineList = new List<int>(MAP_SQUARE_WIDTH_COUNT * MAP_SQUARE_HEIGHT_COUNT);
		MapSquareManager.instance.ExecuteAllSquare(SetWall);

		// �ŏ��̃G���A�����

		// �G���A�𕪊�����

		// ������u��

		// �������q����

		// �K�i��u��

	}

	private static void SetWall(MapSquareData square) {
		square?.SetTerrain(eTerrain.Wall);

		int x = square.squarePositionX, y = square.squarePositionY;
		if (x == 0 || x == MAP_SQUARE_WIDTH_COUNT - 1 ||
			y == 0 || y == MAP_SQUARE_HEIGHT_COUNT - 1) return;

		if (x != 1 && x != MAP_SQUARE_WIDTH_COUNT - 2 &&
			y != 1 && y != MAP_SQUARE_HEIGHT_COUNT - 2) return;

		_devideLineList.Add(square.ID);
	}
}

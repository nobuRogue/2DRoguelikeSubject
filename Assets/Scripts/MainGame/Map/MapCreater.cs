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

	// �G���A������
	private static readonly int AREA_DEVIDE_COUNT = 8;
	// �ŏ������T�C�Y
	private static readonly int MIN_ROOM_SIZE = 3;

	public static void CreateMap() {
		// �ǂŖ��߂�
		_devideLineList = new List<int>(MAP_SQUARE_WIDTH_COUNT * MAP_SQUARE_HEIGHT_COUNT);
		MapSquareManager.instance.ExecuteAllSquare(SetWall);
		// �ŏ��̃G���A�����
		_areaList = new List<AreaData>(AREA_DEVIDE_COUNT + 1);
		_areaList.Add(new AreaData(2, 2, MAP_SQUARE_WIDTH_COUNT - 4, MAP_SQUARE_HEIGHT_COUNT - 4));
		// �G���A�𕪊�����
		DevideArea();
		// ������u��

		// �������q����

		// �K�i��u��

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
			// ���ő�̃G���A���擾
			AreaData maxSizeArea = GetMaxSizeArea(out int maxSize, out bool isVertical);
			if (maxSizeArea == null || maxSize < (MIN_ROOM_SIZE + 2) * 2 + 1) break;
			// �擾�����G���A�𕪊�
			DevideArea(maxSizeArea, isVertical);
		}
	}

	/// <summary>
	/// �ő�T�C�Y�̃G���A���擾
	/// </summary>
	/// <param name="maxSize">�G���A�̃T�C�Y</param>
	/// <param name="isVertical">�c������</param>
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
	/// �w�肵���G���A�𕪊�����
	/// </summary>
	/// <param name="devideArea"></param>
	/// <param name="isVertical"></param>
	private static void DevideArea(AreaData devideArea, bool isVertical) {
		if (isVertical) {
			// �����ɕ������鏈��
			DevideAreaVertical(devideArea);
		} else {
			// �����ɕ������鏈��
			DevideAreaHorizontal(devideArea);
		}
	}

	/// <summary>
	/// ���������̃G���A��������
	/// </summary>
	/// <param name="devideArea"></param>
	private static void DevideAreaVertical(AreaData devideArea) {
		// �����ʒu�̌���
		int randomMax = devideArea.height - (MIN_ROOM_SIZE + 2) * 2;
		int devidePos = Random.Range(0, randomMax);
		devidePos += MIN_ROOM_SIZE + 2 + devideArea.startY;
		// �V�����G���A�̐���
		int newAreaHeight = devideArea.startY + devideArea.height - devidePos - 1;
		_areaList.Add(new AreaData(devideArea.startX, devidePos + 1, devideArea.width, newAreaHeight));
		// �����G���A�̏C��
		devideArea.height = devidePos - devideArea.startY;
		// �������}�X�̒ǉ�
		for (int x = 0, max = devideArea.width; x < max; x++) {
			MapSquareData square = MapSquareManager.instance.Get(devideArea.startX + x, devidePos);
			square.SetTerrain(eTerrain.Wall, 2);
			_devideLineList.Add(square.ID);
		}
	}

	/// <summary>
	/// ���������̃G���A��������
	/// </summary>
	/// <param name="devideArea"></param>
	private static void DevideAreaHorizontal(AreaData devideArea) {
		// �����ʒu�̌���
		int randomMax = devideArea.width - (MIN_ROOM_SIZE + 2) * 2;
		int devidePos = Random.Range(0, randomMax);
		devidePos += MIN_ROOM_SIZE + 2 + devideArea.startX;
		// �V�����G���A�̐���
		int newAreaWidth = devideArea.startX + devideArea.width - devidePos - 1;
		_areaList.Add(new AreaData(devidePos + 1, devideArea.startY, newAreaWidth, devideArea.height));
		// �����G���A�̏C��
		devideArea.width = devidePos - devideArea.startX;
		// �������}�X�̒ǉ�
		for (int y = 0, max = devideArea.height; y < max; y++) {
			MapSquareData square = MapSquareManager.instance.Get(devidePos, devideArea.startY + y);
			square.SetTerrain(eTerrain.Wall, 2);
			_devideLineList.Add(square.ID);
		}
	}

}

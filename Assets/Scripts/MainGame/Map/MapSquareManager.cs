/**
 * @file MapSquareManager.cs
 * @brief �}�X�̊Ǘ�
 * @author yao
 * @date 2025/1/9
 */
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using static CommonModule;

public class MapSquareManager : MonoBehaviour {
	[SerializeField]
	private MapSquareObject _squareObjectOrigin = null; // �}�X�I�u�W�F�N�g�̃I���W�i��

	[SerializeField]
	private Transform _squareObjectRoot = null;     // �}�X�I�u�W�F�N�g�̐e

	public static MapSquareManager instance { get; private set; } = null;

	private List<MapSquareData> _squareDataList = null;
	private List<MapSquareObject> _squareObjectList = null;

	/// <summary>
	/// ������
	/// </summary>
	public void Initialize() {
		instance = this;
		MapSquareData.SetGetObjectCallback(GetSquareObject);
		int squareCount = GameConst.MAP_SQUARE_HEIGHT_COUNT * GameConst.MAP_SQUARE_WIDTH_COUNT;
		_squareDataList = new List<MapSquareData>(squareCount);
		_squareObjectList = new List<MapSquareObject>(squareCount);
		// �}�X�̐���
		for (int i = 0; i < squareCount; i++) {
			// �I�u�W�F�N�g����
			MapSquareObject createObject = Instantiate(_squareObjectOrigin, _squareObjectRoot);
			_squareObjectList.Add(createObject);
			// �f�[�^����
			MapSquareData createSquare = new MapSquareData();
			int x, y;
			GetSquarePosition(i, out x, out y);
			createSquare.Setup(i, x, y);
			_squareDataList.Add(createSquare);
			createSquare.SetTerrain(eTerrain.Wall);
		}
	}

	private MapSquareObject GetSquareObject(int ID) {
		if (!IsEnableIndex(_squareObjectList, ID)) return null;

		return _squareObjectList[ID];
	}

	/// <summary>
	/// ID��2�������W�ɕϊ�
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public void GetSquarePosition(int ID, out int x, out int y) {
		x = ID % GameConst.MAP_SQUARE_WIDTH_COUNT;
		y = ID / GameConst.MAP_SQUARE_WIDTH_COUNT;
	}


	public void ExecuteAllSquare(System.Action<MapSquareData> action) {
		if (action == null || IsEmpty(_squareDataList)) return;

		for (int i = 0, max = _squareDataList.Count; i < max; i++) {
			action(_squareDataList[i]);
		}
	}

}

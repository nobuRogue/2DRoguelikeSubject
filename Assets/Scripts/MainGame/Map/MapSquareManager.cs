/**
 * @file MapSquareManager.cs
 * @brief �}�X�̊Ǘ�
 * @author yao
 * @date 2025/1/9
 */
using System.Collections;
using System.Collections.Generic;
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
	/// ID��2�������W�ɕϊ�
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

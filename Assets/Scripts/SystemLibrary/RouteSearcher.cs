/**
 * @file RouteSearcher.cs
 * @brief �o�H�T���N���X
 * @author yao
 * @date 2025/1/16
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;
using static CommonModule;

public class RouteSearcher {

	private abstract class DistanceNode {

		public int distance { get; private set; } = -1;
		public int squareID { get; private set; } = -1;

		public DistanceNode(int setDistance, int setSquareID) {
			distance = setDistance;
			squareID = setSquareID;
		}
		/// <summary>
		/// �S�[������̋������X�R�A�Ƃ��ĕԂ�
		/// </summary>
		/// <param name="goalX"></param>
		/// <param name="goalY"></param>
		/// <returns></returns>
		public abstract int GetScore(int goalX, int goalY);
	}

	private class DistanceNodeManhattan : DistanceNode {
		public eDirectionFour dir;
		public DistanceNodeManhattan prevNode = null;
		public DistanceNodeManhattan(eDirectionFour setDir, DistanceNodeManhattan setPrevNode, int setDistance, int setSquareID) : base(setDistance, setSquareID) {
			dir = setDir;
			prevNode = setPrevNode;
		}

		/// <summary>
		/// �S�[������̋������X�R�A�Ƃ��ĕԂ�
		/// </summary>
		/// <param name="goalX"></param>
		/// <param name="goalY"></param>
		/// <returns></returns>
		public override int GetScore(int goalX, int goalY) {
			MapSquareData square = MapSquareManager.instance.Get(squareID);
			int diffX = Mathf.Abs(square.positionX - goalX);
			int diffY = Mathf.Abs(square.positionY - goalY);
			return diffX + diffY;
		}
	}

	private class DistanceNodeTableManhattan {
		public DistanceNodeManhattan goalNode = null;
		public List<DistanceNodeManhattan> nodeList = null;
		public DistanceNodeTableManhattan() {
			nodeList = new List<DistanceNodeManhattan>(MAP_SQUARE_HEIGHT_COUNT * MAP_SQUARE_WIDTH_COUNT);
		}

		public void Clear() {
			goalNode = null;
			nodeList.Clear();
		}
	}

	private static DistanceNodeTableManhattan _nodeTableManhattan = null;
	private static List<DistanceNodeManhattan> _manhattanOpenList = null;

	public static List<ManhattanMoveData> RouteSearch(int startSquareID, int goalSquareID,
		System.Func<MapSquareData, eDirectionFour, int, bool> CanPass) {
		// �S�[���m�[�h��������܂Ńm�[�h���J���Ă���
		OpenNodeToGoal(startSquareID, goalSquareID, CanPass);
		// �S�[���m�[�h����X�^�[�g�܂ők���Čo�H�𐶐�
		return CreateRouteManhattan();
	}

	private static void OpenNodeToGoal(int startSquareID, int goalSquareID,
		System.Func<MapSquareData, eDirectionFour, int, bool> CanPass) {
		if (_nodeTableManhattan == null) {
			_nodeTableManhattan = new DistanceNodeTableManhattan();
		} else {
			_nodeTableManhattan.Clear();
		}
		InitializeList(ref _manhattanOpenList, MAP_SQUARE_HEIGHT_COUNT * MAP_SQUARE_WIDTH_COUNT);
		// �X�^�[�g�}�X�̃m�[�h�𐶐����ăI�[�v�����X�g�ɉ�����
		_manhattanOpenList.Add(new DistanceNodeManhattan(eDirectionFour.Invalid, null, 0, startSquareID));
		// �S�[���}�X�̈ʒu���擾���Ă���
		MapSquareData goalSquare = MapSquareManager.instance.Get(goalSquareID);
		int goalX = goalSquare.positionX, goalY = goalSquare.positionY;
		while (_nodeTableManhattan.goalNode == null) {
			// �X�R�A�ŏ��̃m�[�h���擾
			var minScoreNode = GetMinScoreNodeManhattan(goalX, goalY);
			if (minScoreNode == null) break;
			// �X�R�A�ŏ��̃m�[�h�̎��͂��I�[�v������
			OpenNodeAroundManhattan(minScoreNode, goalSquareID, CanPass);
		}

	}

	/// <summary>
	/// �ŏ��X�R�A�̃m�[�h���擾
	/// </summary>
	/// <param name="goalX"></param>
	/// <param name="goalY"></param>
	/// <returns></returns>
	private static DistanceNodeManhattan GetMinScoreNodeManhattan(int goalX, int goalY) {
		if (IsEmpty(_manhattanOpenList)) return null;

		DistanceNodeManhattan minScoreNode = null;
		int minScore = -1;
		for (int i = 0, max = _manhattanOpenList.Count; i < max; i++) {
			DistanceNodeManhattan currentNode = _manhattanOpenList[i];
			if (currentNode == null) continue;

			int currentScore = currentNode.GetScore(goalX, goalY);
			if (minScoreNode == null || minScore > currentScore) {
				minScoreNode = currentNode;
				minScore = currentScore;
			}
		}
		return minScoreNode;
	}

	/// <summary>
	/// ��m�[�h�̎���4�}�X���I�[�v������
	/// </summary>
	/// <param name="baseNode"></param>
	/// <param name="goalSquareID"></param>
	/// <param name="CanPass"></param>
	private static void OpenNodeAroundManhattan(DistanceNodeManhattan baseNode, int goalSquareID,
		System.Func<MapSquareData, eDirectionFour, int, bool> CanPass) {
		if (baseNode == null) return;

		MapSquareData baseSquare = MapSquareManager.instance.Get(baseNode.squareID);
		int baseX = baseSquare.positionX, baseY = baseSquare.positionY;
		// ����4�}�X���I�[�v������
		for (int i = (int)eDirectionFour.Up, max = (int)eDirectionFour.Max; i < max; i++) {
			var dir = (eDirectionFour)i;
			MapSquareData openSquare = MapSquareManager.instance.GetToDirSquare(baseX, baseY, dir);
			if (openSquare == null) continue;
			// ����1�x�I�[�v�����ꂽ�m�[�h�Ȃ珈�����Ȃ�
			if (_nodeTableManhattan.nodeList.Exists(node => node.squareID == openSquare.ID)) continue;
			// �ʍs�۔���
			int distance = baseNode.distance + 1;
			if (!CanPass(openSquare, dir, distance)) continue;

			DistanceNodeManhattan addNode = new DistanceNodeManhattan(dir, baseNode, distance, openSquare.ID);
			_nodeTableManhattan.nodeList.Add(addNode);
			_manhattanOpenList.Add(addNode);
			// �S�[������
			if (openSquare.ID != goalSquareID) continue;

			_nodeTableManhattan.goalNode = addNode;
			return;
		}
	}

	private static List<ManhattanMoveData> CreateRouteManhattan() {
		if (_nodeTableManhattan == null || _nodeTableManhattan.goalNode == null) return null;

		int routeCount = _nodeTableManhattan.goalNode.distance;
		List<ManhattanMoveData> result = new List<ManhattanMoveData>(routeCount);
		for (int i = 0; i < routeCount; i++) {
			result.Add(null);
		}
		// �S�[������k���Čo�H����
		DistanceNodeManhattan currentNode = _nodeTableManhattan.goalNode;
		for (int i = routeCount - 1; i >= 0; i++) {
			var moveData = new ManhattanMoveData(currentNode.prevNode.squareID, currentNode.squareID, currentNode.dir);
			result[i] = moveData;
			currentNode = currentNode.prevNode;
		}
		return result;
	}

}

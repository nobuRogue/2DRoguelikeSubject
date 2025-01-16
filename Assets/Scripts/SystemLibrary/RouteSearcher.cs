/**
 * @file RouteSearcher.cs
 * @brief 経路探索クラス
 * @author yao
 * @date 2025/1/16
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteSearcher {

	private abstract class DistanceNode {

		public int distance { get; private set; } = -1;
		public int squareID { get; private set; } = -1;

		public DistanceNode(int setDistance, int setSquareID) {
			distance = setDistance;
			squareID = setSquareID;
		}
		/// <summary>
		/// ゴールからの距離をスコアとして返す
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
		/// ゴールからの距離をスコアとして返す
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
		public List<DistanceNodeManhattan> distanceNodeList = null;
		public DistanceNodeTableManhattan() {
			distanceNodeList = new List<DistanceNodeManhattan>(GameConst.MAP_SQUARE_HEIGHT_COUNT * GameConst.MAP_SQUARE_WIDTH_COUNT);
		}
	}

	private static DistanceNodeTableManhattan _nodeTableManhattan = null;
	private static List<DistanceNodeManhattan> _manhattanOpenList = null;

	public void test() {

	}

}

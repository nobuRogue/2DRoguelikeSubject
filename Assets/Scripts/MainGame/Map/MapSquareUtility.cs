/**
 * @file MapSquareUtility.cs
 * @brief マス関連実行処理
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MapSquareUtility {

	/// <summary>
	/// 指定方向のマス取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static MapSquareData GetToDirSquare(int x, int y, eDirectionFour dir) {
		ToVectorPos(ref x, ref y, dir);
		return MapSquareManager.instance.Get(x, y);
	}

	/// <summary>
	/// 指定方向のマス取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static MapSquareData GetToDirSquare(int x, int y, eDirectionEight dir) {
		ToVectorPos(ref x, ref y, dir);
		return MapSquareManager.instance.Get(x, y);
	}

	/// <summary>
	/// 移動可否の判定
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="startY"></param>
	/// <param name="moveSquare"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static bool CanMove(int startX, int startY, MapSquareData moveSquare, eDirectionEight dir) {
		if (moveSquare == null ||
			moveSquare.terrain == eTerrain.Wall) return false;
		// 斜め移動か否か
		if (!dir.IsSlant()) return true;
		// 斜め移動なら、方向を分割し各方向のマスをチェック
		eDirectionFour[] separateDir = dir.Separate();
		for (int i = 0, max = separateDir.Length; i < max; i++) {
			MapSquareData checkSquare = GetToDirSquare(startX, startY, separateDir[i]);
			if (checkSquare == null ||
				checkSquare.terrain == eTerrain.Wall) return false;

		}
		return true;
	}

}

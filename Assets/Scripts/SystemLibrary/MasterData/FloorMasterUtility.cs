/**
 * @file FloorMasterUtility.cs
 * @brief フロアマスターデータ実行処理
 * @author yao
 * @date 2025/2/4
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMasterUtility {
	/// <summary>
	/// フロアマスターデータ取得
	/// </summary>
	/// <param name="floorCount"></param>
	/// <returns></returns>
	public static Entity_FloorData.Param GetFloorMaster(int floorCount) {
		List<Entity_FloorData.Param> floorMasterList = MasterDataManager.floorData[0];
		for (int i = 0, max = floorMasterList.Count; i < max; i++) {
			if (floorMasterList[i].floorCount != floorCount) continue;

			return floorMasterList[i];
		}
		return null;
	}

	/// <summary>
	/// 現在のフロアのエネミーテーブルマスター取得
	/// </summary>
	/// <returns></returns>
	public static Entity_EnemySpawnTableData.Param GetCurrentEnemyTable() {
		int floorCount = UserDataHolder.currentData.floorCount;
		var floorMaster = GetFloorMaster(floorCount);
		int enemyTableID = floorMaster.enemyTableID;

		var enemyTableMasterList = MasterDataManager.enemyTableData[0];
		for (int i = 0, max = enemyTableMasterList.Count; i < max; i++) {
			if (enemyTableMasterList[i].ID != enemyTableID) continue;

			return enemyTableMasterList[i];
		}
		return null;
	}

}

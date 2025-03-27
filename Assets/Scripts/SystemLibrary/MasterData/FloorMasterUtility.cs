/**
 * @file FloorMasterUtility.cs
 * @brief フロアマスターデータ実行処理
 * @author yao
 * @date 2025/2/4
 */

using System.Collections.Generic;

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
		// 現在のフロアのマスターデータ取得
		int floorCount = UserDataHolder.currentData.floorCount;
		var floorMaster = GetFloorMaster(floorCount);
		int enemyTableID = floorMaster.enemyTableID;
		// 現在のフロアのエネミーテーブル取得
		var enemyTableMasterList = MasterDataManager.enemyTableData[0];
		for (int i = 0, max = enemyTableMasterList.Count; i < max; i++) {
			if (enemyTableMasterList[i].ID != enemyTableID) continue;

			return enemyTableMasterList[i];
		}
		return null;
	}

	/// <summary>
	/// 現在のフロアのアイテムテーブルマスター取得
	/// </summary>
	/// <returns></returns>
	public static Entity_ItemDropTableData.Param GetCurrentItemDropTable() {
		// 現在のフロアのマスターデータ取得
		int floorCount = UserDataHolder.currentData.floorCount;
		var floorMaster = GetFloorMaster(floorCount);
		int itemTableID = floorMaster.itemTableID;
		// 現在のフロアのアイテムドロップテーブル取得
		var itemTableMasterList = MasterDataManager.itemTableData[0];
		for (int i = 0, max = itemTableMasterList.Count; i < max; i++) {
			if (itemTableMasterList[i].ID != itemTableID) continue;

			return itemTableMasterList[i];
		}
		return null;
	}

}

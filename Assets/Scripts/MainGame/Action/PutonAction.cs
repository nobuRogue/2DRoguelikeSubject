/**
 * @file PutonAction.cs
 * @brief 地面にアイテムを置くアクション
 * @author yao
 * @date 2025/3/25
 */
using Cysharp.Threading.Tasks;

using static ItemUtility;

public class PutonAction {
	// ログメッセージID
	private static int _PUTON_LOG_ID = 6;
	private static int _CANNOT_PUTON_LOG_ID = 7;

	/// <summary>
	/// アイテム地面置き処理実行
	/// </summary>
	/// <param name="squareID"></param>
	/// <param name="itemID"></param>
	/// <returns></returns>
	public static async UniTask ExecutePuton(MapSquareData putonSquare, int itemID) {
		ItemBase item = GetItemData(itemID);
		if (item == null) return;
		// 置くマスにアイテムがないか確認
		if (putonSquare == null ||
			putonSquare.existItem) {
			// アイテムが置けない
			string cannotLogMessage = string.Format(_CANNOT_PUTON_LOG_ID.ToMessage(), item.GetItemName());
			MenuRogueLog.instance.AddLog(cannotLogMessage);
			return;
		}
		// 地面にアイテムを置く
		item.SetSquare(putonSquare);
		string logMessage = string.Format(_PUTON_LOG_ID.ToMessage(), item.GetItemName());
		MenuRogueLog.instance.AddLog(logMessage);

		await UniTask.CompletedTask;
	}

}

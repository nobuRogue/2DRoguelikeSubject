/**
 * @file ItemBase.cs
 * @brief アイテムデータの基底
 * @author yao
 * @date 2025/3/4
 */


public abstract class ItemBase {
	/// <summary>
	/// IDに紐づいたオブジェクトを取得
	/// </summary>
	private static System.Func<int, ItemObject> _GetObject = null;

	public static void SetGetObjectCallback(System.Func<int, ItemObject> setProcess) {
		_GetObject = setProcess;
	}

	public int ID { get; private set; } = -1;
	private int _masterID = -1;
	public int positionX { get; private set; } = -1;
	public int positionY { get; private set; } = -1;
	public int possessCharacterID { get; private set; } = -1;
	public abstract eItemCategory GetCategory();

	public void Setup(int setID, int setMasterID, MapSquareData square) {
		ID = setID;
		_masterID = setMasterID;
		SetSquare(square);
		_GetObject(ID).Setup(ID, ItemMasterUtility.GetItemMaster(_masterID));
	}

	public void Teardown() {
		RemoveCurrentPlace();
		ID = -1;
		_masterID = -1;
	}

	/// <summary>
	/// マスにアイテムを置く
	/// </summary>
	/// <param name="square"></param>
	public void SetSquare(MapSquareData square) {
		if (square == null) return;
		// 現在の場所から取り除く
		RemoveCurrentPlace();
		positionX = square.positionX;
		positionY = square.positionY;
		square.SetItem(ID);
		// オブジェクトの処理
		_GetObject(ID).SetSquare(square);
	}

	/// <summary>
	/// アイテムを現在の場所から取り除く
	/// </summary>
	public void RemoveCurrentPlace() {
		if (positionX >= 0 && positionY >= 0) {
			// 床落ちアイテム
			MapSquareUtility.GetSquare(positionX, positionY)?.RemoveItem();
			positionX = -1;
			positionY = -1;
			// オブジェクトの処理
			_GetObject(ID).UnuseSelf();
		} else if (possessCharacterID >= 0) {
			// キャラが持っているアイテム
			// TODO:キャラの手持ちから取り除く
			possessCharacterID = -1;
		}
	}

}

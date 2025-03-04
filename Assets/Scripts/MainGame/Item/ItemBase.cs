/**
 * @file ItemBase.cs
 * @brief �A�C�e���f�[�^�̊��
 * @author yao
 * @date 2025/3/4
 */


public abstract class ItemBase {
	/// <summary>
	/// ID�ɕR�Â����I�u�W�F�N�g���擾
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
	/// �}�X�ɃA�C�e����u��
	/// </summary>
	/// <param name="square"></param>
	public void SetSquare(MapSquareData square) {
		if (square == null) return;
		// ���݂̏ꏊ�����菜��
		RemoveCurrentPlace();
		positionX = square.positionX;
		positionY = square.positionY;
		square.SetItem(ID);
		// �I�u�W�F�N�g�̏���
		_GetObject(ID).SetSquare(square);
	}

	/// <summary>
	/// �A�C�e�������݂̏ꏊ�����菜��
	/// </summary>
	public void RemoveCurrentPlace() {
		if (positionX >= 0 && positionY >= 0) {
			// �������A�C�e��
			MapSquareUtility.GetSquare(positionX, positionY)?.RemoveItem();
			positionX = -1;
			positionY = -1;
			// �I�u�W�F�N�g�̏���
			_GetObject(ID).UnuseSelf();
		} else if (possessCharacterID >= 0) {
			// �L�����������Ă���A�C�e��
			// TODO:�L�����̎莝�������菜��
			possessCharacterID = -1;
		}
	}

}

/**
 * @file CharacterManager.cs
 * @brief �L�����N�^�[�Ǘ�
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;
using static CommonModule;

public class CharacterManager : MonoBehaviour {
	[SerializeField]
	private CharacterObject _characterObjectOrigin = null;

	[SerializeField]
	private Transform _useObjectRoot = null;

	[SerializeField]
	private Transform _unuseObjectRoot = null;

	public static CharacterManager instance { get; private set; } = null;
	// �g�p���̃L�����N�^�[���X�g
	private List<CharacterBase> _useList = null;
	// ���g�p��Ԃ̃v���C���[
	private List<PlayerCharacter> _unusePlayer = null;
	// ���g�p��Ԃ̃G�l�~�[���X�g
	private List<EnemyCharacter> _unuseEnemyList = null;

	// �g�p���̃L�����N�^�[�I�u�W�F�N�g���X�g
	private List<CharacterObject> _useObjectList = null;
	// ���g�p��Ԃ̃L�����N�^�[�I�u�W�F�N�g���X�g
	private List<CharacterObject> _unuseObjectList = null;

	public void Initialize() {
		instance = this;
		CharacterBase.SetGetObjectCallback(GetCharacterObject);
		// �K�v�ȃL�����N�^�[�ƃI�u�W�F�N�g�̃C���X�^���X�𐶐����Ė��g�p��Ԃɂ��Ă���
		_useList = new List<CharacterBase>(FLOOR_ENEMY_MAX + 1);
		_useObjectList = new List<CharacterObject>(FLOOR_ENEMY_MAX + 1);

		_unusePlayer = new List<PlayerCharacter>(1);
		_unusePlayer.Add(new PlayerCharacter());

		_unuseEnemyList = new List<EnemyCharacter>(FLOOR_ENEMY_MAX);
		for (int i = 0; i < FLOOR_ENEMY_MAX; i++) {
			_unuseEnemyList.Add(new EnemyCharacter());
		}
		_unuseObjectList = new List<CharacterObject>(FLOOR_ENEMY_MAX + 1);
		for (int i = 0; i < FLOOR_ENEMY_MAX + 1; i++) {
			_unuseObjectList.Add(Instantiate(_characterObjectOrigin, _unuseObjectRoot));
		}
	}

	/// <summary>
	/// �v���C���[�L�����̐���
	/// </summary>
	/// <param name="squareData"></param>
	public void UsePlayer(MapSquareData squareData) {
		// �C���X�^���X�̎擾
		PlayerCharacter usePlayer = null;
		if (IsEmpty(_unusePlayer)) {
			usePlayer = new PlayerCharacter();
		} else {
			usePlayer = _unusePlayer[0];
			_unusePlayer.RemoveAt(0);
		}
		// �g�p�\��ID���擾���Ďg�p���X�g�ɒǉ�
		int useID = -1;
		for (int i = 0, max = _useList.Count; i < max; i++) {
			if (_useList[i] != null) continue;

			useID = i;
			_useList[i] = usePlayer;
		}
		if (useID < 0) {
			useID = _useList.Count;
			_useList.Add(usePlayer);
		}
		// �I�u�W�F�N�g�̎擾
		CharacterObject useObject = null;
		if (IsEmpty(_unuseObjectList)) {
			useObject = Instantiate(_characterObjectOrigin);
		} else {
			useObject = _unuseObjectList[0];
			_unuseObjectList.RemoveAt(0);
		}
		// �I�u�W�F�N�g�̎g�p���X�g�ւ̒ǉ�
		while (!IsEnableIndex(_useObjectList, useID)) _useObjectList.Add(null);

		_useObjectList[useID] = useObject;
		useObject.transform.SetParent(_useObjectRoot);
		usePlayer.Setup(useID, squareData);
	}

	private CharacterObject GetCharacterObject(int ID) {
		if (!IsEnableIndex(_useObjectList, ID)) return null;

		return _useObjectList[ID];
	}

}

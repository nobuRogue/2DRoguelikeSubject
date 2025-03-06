/**
 * @file MenuList.cs
 * @brief ���X�g���j���[�̊��
 * @author yao
 * @date 2025/3/6
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MenuList : MenuBase {
	/// <summary>
	/// ���X�g���ڂ̃I���W�i��
	/// </summary>
	[SerializeField]
	private MenuListItem _itemOrigin = null;
	/// <summary>
	/// ���ڂ���ׂ郋�[�g�I�u�W�F�N�g
	/// </summary>
	[SerializeField]
	private Transform _contentRoot = null;
	/// <summary>
	/// ���g�p��Ԃ̍��ڂ̃��[�g�I�u�W�F�N�g
	/// </summary>
	[SerializeField]
	private Transform _unuseRoot = null;

	/// <summary>
	/// ���X�g���j���[�̃R�[���o�b�N�W�N���X
	/// </summary>
	protected class MenuListCallbackFortmat {
		// ���肳�ꂽ�ۂ̏���
		public System.Func<MenuListItem, UniTask<bool>> OnDecide = null;
		// �L�����Z�����ꂽ�ۂ̏���
		public System.Func<MenuListItem, UniTask<bool>> OnCancel = null;
		// �J�[�\�����ړ������ۂ̏���
		public System.Func<MenuListItem, MenuListItem, UniTask<bool>> OnMoveCursor = null;
	}
	private MenuListCallbackFortmat _currentFormat = null;

	private int _currentIndex = -1;
	private bool _isContinue = false;

	private List<MenuListItem> _useList = null;
	private List<MenuListItem> _unuseList = null;

	public override async UniTask Initialize() {
		await base.Initialize();
		_useList = new List<MenuListItem>();
		_unuseList = new List<MenuListItem>();
	}

	/// <summary>
	/// �R�[���o�b�N�p�N���X�̐ݒ�
	/// </summary>
	/// <param name="setFormat"></param>
	protected void SetCallbackFortmat(MenuListCallbackFortmat setFormat) {
		_currentFormat = setFormat;
	}

	/// <summary>
	/// ���X�g���ڂ̐���
	/// </summary>
	/// <returns></returns>
	protected MenuListItem AddListItem() {
		MenuListItem addItem;
		if (IsEmpty(_unuseList)) {
			// ���g�p���X�g����Ȃ̂Ő���
			addItem = Instantiate(_itemOrigin, _contentRoot);
		} else {
			// ���g�p���X�g����擾
			addItem = _unuseList[0];
			_unuseList.RemoveAt(0);
			addItem.transform.SetParent(_contentRoot);
		}
		addItem.Deselect();
		_useList.Add(addItem);
		return addItem;
	}
}

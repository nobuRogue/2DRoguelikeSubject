/**
 * @file MenuPlayerStatus.cs
 * @brief �v���C���[�̃X�e�[�^�X�\��
 * @author yao
 * @date 2025/2/6
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPlayerStatus : MenuBase {
	// �t���A��
	[SerializeField]
	private TextMeshProUGUI _floorCount = null;

	// �v���C���[��HP�\�L
	[SerializeField]
	private TextMeshProUGUI _HPText = null;

	// �v���C���[�̍U���͕\�L
	[SerializeField]
	private TextMeshProUGUI _attackText = null;

	// �v���C���[�̖h��\�L
	[SerializeField]
	private TextMeshProUGUI _defenseText = null;


	public void SetHP(int HP, int maxHP) {
						// ���ǂ��Ȃ��I
		_HPText.text = HP.ToString() + "/" + maxHP.ToString();
	}
}

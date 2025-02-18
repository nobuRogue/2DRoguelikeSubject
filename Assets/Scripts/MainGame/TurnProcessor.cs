/**
 * @file TurnProcessor.cs
 * @brief �^�[�����s����
 * @author yao
 * @date 2025/1/21
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;
using static GameConst;

public class TurnProcessor {
	// �v���C���[�̓��͎�t����
	AcceptPlayerInput _acceptPlayerInput = null;
	// �^�[���p���t���O
	private bool _isContinueTurn = false;

	private List<MoveAction> _moveActionList = null;
	private List<UniTask> _moveTaskList = null;

	private System.Action<eFloorEndReason> _EndFloor = null;
	private System.Action<eDungeonEndReason> _EndDungeon = null;

	public void Initialize(
		System.Action<eFloorEndReason> SetEndFloor,
		System.Action<eDungeonEndReason> SetEndDungeon) {
		_acceptPlayerInput = new AcceptPlayerInput();
		_acceptPlayerInput.SetAddMoveActionCallback(moveAction => _moveActionList.Add(moveAction));
		EnemyAIBase.SetAddMoveCallback(moveAction => _moveActionList.Add(moveAction));

		_moveActionList = new List<MoveAction>(FLOOR_ENEMY_MAX + 1);
		_moveTaskList = new List<UniTask>(FLOOR_ENEMY_MAX + 1);

		_EndFloor = SetEndFloor;
		_EndDungeon = SetEndDungeon;
		MoveAction.SetEndCallback(EndFloor, EndDungeon);
		ActionEffectBase.SetEndCallback(EndDungeon);
	}

	public async UniTask Execute() {
		_isContinueTurn = true;
		// �v���C���[�̓��͎�t
		await AcceptPlayerInput();
		// �S�ẴG�l�~�[�ɍs�����v�l������
		CharacterManager.instance.ExecuteAll(character => character?.ThinkAction());
		for (int i = 0, max = _moveActionList.Count; i < max; i++) {
			_moveTaskList.Add(_moveActionList[i].ProcessObject(MOVE_DURATION));
		}
		await WaitTask(_moveTaskList);
		_moveActionList.Clear();
		_moveTaskList.Clear();
		if (!_isContinueTurn) return;
		// �S�ẴG�l�~�[���ړ��ȊO�̍s��������

	}

	/// <summary>
	/// �v���C���[�̓��͎�t
	/// </summary>
	/// <returns></returns>
	private async UniTask AcceptPlayerInput() {
		// �p���ړ������邩�m�F
		if (_acceptPlayerInput.AcceptMove()) return;
		// �S�ẴL�����N�^�[��ҋ@�A�j���[�V�����ɂ���
		CharacterManager.instance.ExecuteAll(character => character.SetAnimation(eCharacterAnimation.Wait));
		await _acceptPlayerInput.AcceptInput();
	}

	private void EndTurn() {
		_isContinueTurn = false;
	}

	private void EndFloor(eFloorEndReason endReason) {
		_EndFloor(endReason);
		EndTurn();
	}

	private void EndDungeon(eDungeonEndReason endReason) {
		_EndDungeon(endReason);
		EndFloor(endReason.GetFloorEndReaosn());
	}

}

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


	public void Initialize() {
		_acceptPlayerInput = new AcceptPlayerInput();
		_acceptPlayerInput.SetAddMoveActionCallback(moveAction => _moveActionList.Add(moveAction));
		EnemyAIBase.SetAddMoveCallback(moveAction => _moveActionList.Add(moveAction));

		_moveActionList = new List<MoveAction>(FLOOR_ENEMY_MAX + 1);
		_moveTaskList = new List<UniTask>(FLOOR_ENEMY_MAX + 1);
	}

	public async UniTask Execute() {
		await _acceptPlayerInput.AcceptInput();
		// �S�ẴG�l�~�[�ɍs�����v�l������
		CharacterManager.instance.ExecuteAll(character => character?.ThinkAction());
		for (int i = 0, max = _moveActionList.Count; i < max; i++) {
			_moveTaskList.Add(_moveActionList[i].ProcessObject(MOVE_DURATION));
		}
		await WaitTask(_moveTaskList);
		_moveActionList.Clear();
		_moveTaskList.Clear();

	}

}

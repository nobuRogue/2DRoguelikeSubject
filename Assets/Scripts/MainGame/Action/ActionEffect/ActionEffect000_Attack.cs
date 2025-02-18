/**
 * @file ActionEffect000_Attack.cs
 * @brief �ʏ�U���̌��ʏ���
 * @author yao
 * @date 2025/2/18
 */

using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect000_Attack : ActionEffectBase {

	public override async UniTask Execute(CharacterBase sourceCharacter, ActionRangeBase range) {
		// �s���҂̍U���A�j���[�V�����Đ�
		sourceCharacter.SetAnimation(eCharacterAnimation.Attack);
		// �Ώۂ��ƂɍU���̏���
		int sourceAttack = sourceCharacter.attack;
		List<int> targetList = range.targetList;
		for (int i = 0, max = targetList.Count; i < max; i++) {
			CharacterBase target = CharacterManager.instance.Get(targetList[i]);
			if (target == null) continue;

			ExecuteAttack(sourceAttack, target);
		}
		// �U���A�j���[�V�����̏I���҂�
		while (sourceCharacter.GetCurrentAnimation() == eCharacterAnimation.Attack) await UniTask.DelayFrame(1);

	}

	private void ExecuteAttack(int sourceAttack, CharacterBase targetCharacter) {
		// �Ώۂ̔�_���[�W�A�j���[�V����
		targetCharacter.SetAnimation(eCharacterAnimation.Damage);
		// �_���[�W�v�Z
		int defense = targetCharacter.defense;
		int damage = (int)(sourceAttack * Mathf.Pow(15.0f / 16.0f, defense));
		// HP�����炷
		targetCharacter.RemoveHP(damage);
		// ���S����A����
		if (!targetCharacter.IsDead()) return;

		if (targetCharacter.IsPlayer()) {
			// �v���C���[���S�̏���
			_EndDungeon(eDungeonEndReason.Dead);
		} else {
			// �G�l�~�[���S�̏���
			CharacterManager.instance.UnuseEnemy(targetCharacter as EnemyCharacter);
		}
	}

}

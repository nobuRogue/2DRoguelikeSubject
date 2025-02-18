/**
 * @file ActionRange00_DirForward.cs
 * @brief �L�����̌����O���̎˒�
 * @author yao
 * @date 2025/1/9
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class ActionRange00_DirForward : ActionRangeBase {
	public override void Setup(CharacterBase sourceCharacter) {
		InitializeList(ref targetList);
		// �Ƃ肠�����O��1�}�X�Ŏ���
		int sourceX = sourceCharacter.positionX, sourceY = sourceCharacter.positionY;
		MapSquareData sourceSquare = MapSquareUtility.GetCharacterSquare(sourceCharacter);
		MapSquareData targetSquare = MapSquareUtility.GetToDirSquare(sourceX, sourceY, sourceCharacter.direction);
		if (!targetSquare.existCharacter) return;

		CharacterBase targetCharacter = CharacterManager.instance.Get(targetSquare.characterID);
		if (sourceCharacter.IsPlayer()) {
			// �v���C���[�̓G�l�~�[��ΏۂɎ��
			if (!targetCharacter.IsPlayer()) targetList.Add(targetCharacter.ID);

		} else {
			// �G�l�~�[�̓v���C���[��ΏۂɎ��
			if (targetCharacter.IsPlayer()) targetList.Add(targetCharacter.ID);

		}

	}

}

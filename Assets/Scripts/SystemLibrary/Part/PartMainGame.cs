/**
 * @file PartMainGame.cs
 * @brief ���C���Q�[���p�[�g
 * @author yao
 * @date 2025/1/9
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMainGame : PartBase {
	[SerializeField]
	private MapSquareManager _squareManager = null;

	[SerializeField]
	private CharacterManager _characterManager = null;

	private DungeonProcessor _dungeonProcessor = null;

	public override async UniTask Initialize() {
		TerrainSpriteAssignor.Initialize();
		TerrainSpriteAssignor.SetFloorSpriteTypeIndex(0);

		_dungeonProcessor = new DungeonProcessor();
		_dungeonProcessor.Initialize();

		_squareManager.Initialize();
		_characterManager.Initialize();
	}

	public override async UniTask Setup() {

	}

	public override async UniTask Execute() {
		CharacterManager.instance.UsePlayer(MapSquareManager.instance.Get(0, 0));
		// �_���W�����̎��s
		eDungeonEndReason endReason = await _dungeonProcessor.Execute();
		// �_���W�����I�����ʂ̏���
		UniTask task;
		switch (endReason) {
			case eDungeonEndReason.Dead:
			// 1�K����n�܂�
			task = PartManager.instance.TransitionPart(eGamePart.MainGame);
			break;
			case eDungeonEndReason.Clear:
			// �G���f�B���O�p�[�g�ֈڍs
			task = PartManager.instance.TransitionPart(eGamePart.Ending);
			break;
		}
	}

	public override async UniTask Teardown() {

	}
}

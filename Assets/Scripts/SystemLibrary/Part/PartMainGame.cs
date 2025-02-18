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

		await MenuManager.instance.Get<MenuPlayerStatus>("Prefabs/Menu/CanvasPlayerStatus").Initialize();
		await MenuManager.instance.Get<MenuGameOver>("Prefabs/Menu/CanvasGameOver").Initialize();

		ActionRangeManager.Initialize();
		ActionManager.Initialize();
	}

	public override async UniTask Setup() {
		// �K�w����1�ɐݒ�
		UserDataHolder.currentData.SetFloorCount(1);
		// �v���C���[�����Ȃ���ΐ���
		SetupPlayer();
	}

	public override async UniTask Execute() {
		// ���C��UI�\��
		var menuPlayerStatus = MenuManager.instance.Get<MenuPlayerStatus>();
		await menuPlayerStatus.Open();
		// �_���W�����̎��s
		eDungeonEndReason endReason = await _dungeonProcessor.Execute();
		await menuPlayerStatus.Close();
		// �_���W�����I�����ʂ̏���
		UniTask task;
		switch (endReason) {
			case eDungeonEndReason.Dead:
			MenuGameOver menuGameOver = MenuManager.instance.Get<MenuGameOver>();
			await menuGameOver.Open();
			await menuGameOver.Close();
			// �^�C�g���֖߂�
			task = PartManager.instance.TransitionPart(eGamePart.Title);
			break;
			case eDungeonEndReason.Clear:
			// �G���f�B���O�p�[�g�ֈڍs
			task = PartManager.instance.TransitionPart(eGamePart.Ending);
			break;
		}
	}

	private void SetupPlayer() {
		PlayerCharacter player = CharacterManager.instance.GetPlayer();
		if (player != null) return;

		CharacterManager.instance.UsePlayer(MapSquareManager.instance.Get(0, 0), 0);
		CharacterManager.instance.GetPlayer().SetMoveObserver(CameraManager.instance);
	}

	public override async UniTask Teardown() {

	}
}

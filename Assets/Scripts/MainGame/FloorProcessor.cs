/**
 * @file FloorProcessor.cs
 * @brief �t���A���s����
 * @author yao
 * @date 2025/1/21
 */


using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

using static CommonModule;
using static GameConst;

public class FloorProcessor {
	private TurnProcessor _turnProcessor = null;

	private eFloorEndReason _endReason = eFloorEndReason.Invalid;

	public void Initialize(System.Action<eDungeonEndReason> SetEndDungeon) {
		_turnProcessor = new TurnProcessor();
		_turnProcessor.Initialize(EndFloor, SetEndDungeon);
	}

	public async UniTask<eFloorEndReason> Execute() {
		// �t���A�̐���
		await SetupFloor();
		while (_endReason == eFloorEndReason.Invalid) {
			await _turnProcessor.Execute();
		}
		// �t���A�̔j��
		await TeardownFloor();
		OnEndFloor();
		return _endReason;
	}

	/// <summary>
	/// �t���A����
	/// </summary>
	private async UniTask SetupFloor() {
		// ���݂̃t���A�}�X�^�[�f�[�^����}�b�v�`�b�v�C���f�b�N�X�擾
		Entity_FloorData.Param floorMaster = FloorMasterUtility.GetFloorMaster(UserDataHolder.currentData.floorCount);
		int floorTypeIndex = floorMaster == null ? 0 : floorMaster.spriteIndex;
		TerrainSpriteAssignor.SetFloorSpriteTypeIndex(floorTypeIndex);
		// �t���A�̐���
		MapCreater.CreateMap();
		// �v���C���[�̔z�u
		SetPlayer();
		// �G�l�~�[�̔z�u
		SpawnEnemy(3);
		_endReason = eFloorEndReason.Invalid;
		await FadeManager.instance.FadeIn();
	}

	/// <summary>
	/// �v���C���[�̔z�u
	/// </summary>
	private void SetPlayer() {
		PlayerCharacter player = CharacterManager.instance.GetPlayer();
		if (player == null) return;
		// �����_���ȕ����}�X���擾
		RoomData roomData = MapSquareManager.instance.GetRandomRoom();
		if (roomData == null) return;

		List<int> roomSquareList = roomData.squareIDList;
		int playerSquareID = roomSquareList[Random.Range(0, roomSquareList.Count)];
		MapSquareData playerSquare = MapSquareManager.instance.Get(playerSquareID);
		player.SetSquare(playerSquare);
	}

	/// <summary>
	/// �G�l�~�[�̐���
	/// </summary>
	/// <param name="spawnCount"></param>
	private void SpawnEnemy(int spawnCount) {
		// �G�l�~�[�̐������}�X���擾
		List<MapSquareData> roomSquareList = new List<MapSquareData>(MAP_SQUARE_HEIGHT_COUNT * MAP_SQUARE_WIDTH_COUNT);
		MapSquareManager.instance.ExecuteAllSquare(square => {
			if (square.existCharacter ||
				square.terrain != eTerrain.Room) return;

			roomSquareList.Add(square);
		});
		// �G�l�~�[�̐���
		for (int i = 0; i < spawnCount; i++) {
			if (IsEmpty(roomSquareList)) return;

			MapSquareData enemySquare = roomSquareList[Random.Range(0, roomSquareList.Count)];
			CharacterManager.instance.UseEnemy(enemySquare, 1);
			roomSquareList.Remove(enemySquare);
		}
	}

	private async UniTask TeardownFloor() {
		await FadeManager.instance.FadeOut();
		// �G�l�~�[�̑S�폜
		CharacterUtility.ExecuteAllCharacter(character => {
			if (character.IsPlayer()) return;

			CharacterManager.instance.UnuseEnemy(character as EnemyCharacter);
		});
		// �L�����N�^�[�̃t���A�I��������
		CharacterUtility.ExecuteAllCharacter(character => character.OnEndFloor());
	}

	/// <summary>
	/// �t���A���I��������
	/// </summary
	/// <param name="endReason"></param>
	private void EndFloor(eFloorEndReason endReason) {
		_endReason = endReason;
	}

	/// <summary>
	/// �t���A�I��������
	/// </summary>
	private void OnEndFloor() {
		switch (_endReason) {
			case eFloorEndReason.Dead:
			break;
			case eFloorEndReason.Stair:
			UserData userData = UserDataHolder.currentData;
			userData.SetFloorCount(userData.floorCount + 1);
			break;
		}
	}

}

/**
 * @file FloorProcessor.cs
 * @brief フロア実行処理
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

	public void Initialize() {
		_turnProcessor = new TurnProcessor();
		_turnProcessor.Initialize();
	}

	public async UniTask<eFloorEndReason> Execute() {
		// フロアの生成
		SetupFloor();
		while (_endReason == eFloorEndReason.Invalid) {
			await _turnProcessor.Execute();
		}
		// フロアの破棄

		return _endReason;
	}

	/// <summary>
	/// フロア準備
	/// </summary>
	private void SetupFloor() {
		// フロアの生成
		MapCreater.CreateMap();
		// プレイヤーの配置
		SetPlayer();
		// エネミーの配置
		SetEnemy();
	}

	/// <summary>
	/// プレイヤーの配置
	/// </summary>
	private void SetPlayer() {
		PlayerCharacter player = CharacterManager.instance.GetPlayer();
		if (player == null) return;
		// ランダムな部屋マスを取得
		RoomData roomData = MapSquareManager.instance.GetRandomRoom();
		if (roomData == null) return;

		List<int> roomSquareList = roomData.squareIDList;
		int playerSquareID = roomSquareList[Random.Range(0, roomSquareList.Count)];
		MapSquareData playerSquare = MapSquareManager.instance.Get(playerSquareID);
		player.SetSquare(playerSquare);
	}

	private void SetEnemy() {
		List<MapSquareData> roomSquareList = new List<MapSquareData>(MAP_SQUARE_HEIGHT_COUNT * MAP_SQUARE_WIDTH_COUNT);
		MapSquareManager.instance.ExecuteAllSquare(square => {
			if (square.existCharacter ||
				square.terrain != eTerrain.Room) return;

			roomSquareList.Add(square);
		});

		if (IsEmpty(roomSquareList)) return;

		MapSquareData enemySquare = roomSquareList[Random.Range(0, roomSquareList.Count)];
		CharacterManager.instance.UseEnemy(enemySquare);

		roomSquareList.Remove(enemySquare);

	}

}

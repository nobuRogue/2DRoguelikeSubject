/**
 * @file PartMainGame.cs
 * @brief メインゲームパート
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

	private DungeonProcessor _dungeonProcessor = null;

	public override async UniTask Initialize() {
		TerrainSpriteAssignor.Initialize();
		TerrainSpriteAssignor.SetFloorSpriteTypeIndex(0);

		_dungeonProcessor = new DungeonProcessor();
		_dungeonProcessor.Initialize();
	}

	public override async UniTask Setup() {

	}

	public override async UniTask Execute() {
		_squareManager.Initialize();
		MapCreater.CreateMap();
		// ダンジョンの実行
		eDungeonEndReason endReason = await _dungeonProcessor.Execute();
		// ダンジョン終了結果の処理
		UniTask task;
		switch (endReason) {
			case eDungeonEndReason.Dead:
			// 1階から始まる
			task = PartManager.instance.TransitionPart(eGamePart.MainGame);
			break;
			case eDungeonEndReason.Clear:
			// エンディングパートへ移行
			task = PartManager.instance.TransitionPart(eGamePart.Ending);
			break;
		}
	}

	public override async UniTask Teardown() {

	}
}

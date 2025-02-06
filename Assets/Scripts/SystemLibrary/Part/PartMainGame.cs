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
	}

	public override async UniTask Setup() {

	}

	public override async UniTask Execute() {
		var menuPlayerStatus = MenuManager.instance.Get<MenuPlayerStatus>();
		await menuPlayerStatus.Open();

		CharacterManager.instance.UsePlayer(MapSquareManager.instance.Get(0, 0), 0);
		CharacterManager.instance.GetPlayer().SetMoveObserver(CameraManager.instance);
		// ダンジョンの実行
		eDungeonEndReason endReason = await _dungeonProcessor.Execute();

		await menuPlayerStatus.Close();
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

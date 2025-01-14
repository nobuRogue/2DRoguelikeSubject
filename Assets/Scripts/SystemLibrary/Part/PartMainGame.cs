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

	public override async UniTask Initialize() {
		TerrainSpriteAssignor.Initialize();
		TerrainSpriteAssignor.SetFloorSpriteTypeIndex(0);
	}

	public override async UniTask Setup() {

	}

	public override async UniTask Execute() {
		_squareManager.Initialize();
		MapCreater.CreateMap();
	}

	public override async UniTask Teardown() {

	}
}

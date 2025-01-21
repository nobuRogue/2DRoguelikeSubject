/**
 * @file DungeonProcessor.cs
 * @brief É_ÉìÉWÉáÉìé¿çsèàóù
 * @author yao
 * @date 2025/1/21
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonProcessor {
	private FloorProcessor _floorProcessor = null;

	private eDungeonEndReason _endReason = eDungeonEndReason.Invalid;

	public void Initialize() {
		_floorProcessor = new FloorProcessor();
		_floorProcessor.Initialize();
	}

	public async UniTask<eDungeonEndReason> Execute() {
		while (_endReason == eDungeonEndReason.Invalid) {
			await _floorProcessor.Execute();
		}
		return _endReason;
	}

}

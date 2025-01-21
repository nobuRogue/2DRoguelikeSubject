/**
 * @file FloorProcessor.cs
 * @brief �t���A���s����
 * @author yao
 * @date 2025/1/21
 */


using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorProcessor {
	private TurnProcessor _turnProcessor = null;

	private eFloorEndReason _endReason = eFloorEndReason.Invalid;

	public void Initialize() {
		_turnProcessor = new TurnProcessor();
		_turnProcessor.Initialize();
	}

	public async UniTask<eFloorEndReason> Execute() {
		// �t���A�̐���

		while (_endReason == eFloorEndReason.Invalid) {
			await _turnProcessor.Execute();
		}
		// �t���A�̔j��

		return _endReason;
	}
}

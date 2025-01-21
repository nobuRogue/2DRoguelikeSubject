/**
 * @file TurnProcessor.cs
 * @brief ƒ^[ƒ“Àsˆ—
 * @author yao
 * @date 2025/1/21
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnProcessor {
	public void Initialize() {

	}

	public async UniTask Execute() {
		while (true) {
			await UniTask.DelayFrame(1);
		}
	}
}

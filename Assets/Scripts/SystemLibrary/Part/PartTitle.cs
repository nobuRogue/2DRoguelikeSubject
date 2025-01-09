/**
 * @file PartTitle.cs
 * @brief タイトルパート
 * @author yao
 * @date 2025/1/9
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTitle : PartBase {
	public override async UniTask Execute() {
		UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
	}

	public override async UniTask Initialize() {

	}

	public override async UniTask Setup() {

	}

	public override async UniTask Teardown() {

	}
}

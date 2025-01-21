/**
 * @file CharacterObject.cs
 * @brief キャラクターオブジェクト
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _characterSprite = null;

	public void Setup() {

	}

	public void Teardown() {

	}

	public void SetPosition(Vector3 position) {
		transform.position = position;
	}

}

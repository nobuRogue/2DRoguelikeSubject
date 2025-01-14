/**
 * @file MapSquareObject.cs
 * @brief 1マスのオブジェクト
 * @author yao
 * @date 2025/1/9
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareObject : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _terrainSprite = null;

	public void Setup(int setX, int setY) {
		Vector3 position = transform.position;
		position.x = setX * 0.32f;
		position.y = setY * 0.32f;
		position.z = setY;
		transform.position = position;
	}

	public void SetTerrain(eTerrain setTerrain, int spriteIndex) {
		// 地形に応じたスプライトの設定
		_terrainSprite.sprite = TerrainSpriteAssignor.GetTerrainSprite(setTerrain, spriteIndex);
	}

}

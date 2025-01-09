/**
 * @file MapSquareObject.cs
 * @brief 1�}�X�̃I�u�W�F�N�g
 * @author yao
 * @date 2025/1/9
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSquareObject : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _terrainSprite = null;

	public void Setup(Vector2Int setSquarePosition) {
		Vector3 position = transform.position;
		position.x = setSquarePosition.x;
		position.y = setSquarePosition.y;
		position.z = setSquarePosition.y;
	}

	public void SetTerrain(eTerrain setTerrain) {
		// �n�`�ɉ������X�v���C�g�̐ݒ�

	}

}

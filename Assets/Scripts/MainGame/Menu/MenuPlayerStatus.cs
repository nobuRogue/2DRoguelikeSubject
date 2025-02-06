/**
 * @file MenuPlayerStatus.cs
 * @brief プレイヤーのステータス表示
 * @author yao
 * @date 2025/2/6
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPlayerStatus : MenuBase {
	// フロア数
	[SerializeField]
	private TextMeshProUGUI _floorCount = null;

	// プレイヤーのHP表記
	[SerializeField]
	private TextMeshProUGUI _HPText = null;

	// プレイヤーの攻撃力表記
	[SerializeField]
	private TextMeshProUGUI _attackText = null;

	// プレイヤーの防御表記
	[SerializeField]
	private TextMeshProUGUI _defenseText = null;


	public void SetHP(int HP, int maxHP) {
						// ↓良くない！
		_HPText.text = HP.ToString() + "/" + maxHP.ToString();
	}
}

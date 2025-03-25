/**
 * @file MenuItemCommandListItem.cs
 * @brief アイテムコマンドの項目クラス
 * @author yao
 * @date 2025/3/25
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuItemCommandListItem : MenuListItem {
	[SerializeField]
	private TextMeshProUGUI _commandNameText = null;

	public eItemCommand command { get; private set; } = eItemCommand.Invalid;

	public void Setup(eItemCommand setCommand) {
		command = setCommand;

	}

}

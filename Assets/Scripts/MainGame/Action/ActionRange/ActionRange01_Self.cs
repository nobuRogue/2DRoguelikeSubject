/**
 * @file ActionRange01_Self.cs
 * @brief ©g‚ğ‘ÎÛ‚Éæ‚é
 * @author yao
 * @date 2025/3/11
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class ActionRange01_Self : ActionRangeBase {
	public override void Setup(CharacterBase sourceCharacter) {
		InitializeList(ref targetList);
		targetList.Add(sourceCharacter.ID);
	}
}

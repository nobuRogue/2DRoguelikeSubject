/**
 * @file CharacterUtility.cs
 * @brief �L�����N�^�[�֘A���s����
 * @author yao
 * @date 2025/2/20
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUtility {

	/// <summary>
	/// �S�ẴL�����N�^�[�ɏ������s
	/// </summary>
	/// <param name="action"></param>
	public static void ExecuteAllCharacter(System.Action<CharacterBase> action) {
		CharacterManager.instance.ExecuteAll(action);
	}

	/// <summary>
	/// �S�ẴL�����N�^�[�Ƀ^�X�N���s
	/// </summary>
	/// <param name="task"></param>
	/// <returns></returns>
	public static async UniTask ExecuteTaskAllCharacter(System.Func<CharacterBase, UniTask> task) {
		await CharacterManager.instance.ExecuteAllTask(task);
	}

}

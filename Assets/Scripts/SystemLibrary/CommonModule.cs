/**
 * @file CommonModule.cs
 * @brief ���p���W���[��
 * @author yao
 * @date 2025/1/9
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonModule {

	/// <summary>
	/// ���X�g���󂩔ۂ�
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <returns></returns>
	public static bool IsEmpty<T>(List<T> list) {
		return list == null || list.Count == 0;
	}

	/// <summary>
	/// ���X�g�ɑ΂��ėL���ȃC���f�N�X���ۂ�
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public static bool IsEnableIndex<T>(List<T> list, int index) {
		if (IsEmpty(list)) return false;

		return list.Count > index && index >= 0;
	}

	/// <summary>
	/// �����̃^�X�N�̏I���҂�
	/// </summary>
	/// <param name="taskList"></param>
	/// <returns></returns>
	public static async UniTask WaitTask(List<UniTask> taskList) {
		while (!IsEmpty(taskList)) {
			for (int i = taskList.Count - 1; i >= 0; i--) {
				if (!taskList[i].Status.IsCompleted()) continue;

				taskList.RemoveAt(i);
			}
			await UniTask.DelayFrame(1);
		}
	}

}

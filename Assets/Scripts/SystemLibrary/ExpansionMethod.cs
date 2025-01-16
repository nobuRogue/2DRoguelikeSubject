/**
 * @file ExpansionMethod.cs
 * @brief �g�����\�b�h�N���X
 * @author yao
 * @date 2025/1/14
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ExpansionMethod {

	/// <summary>
	/// ���Ε������擾
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static eDirectionFour ReverseDir(this eDirectionFour dir) {
		int result = (int)dir + 2;
		if (result >= (int)eDirectionFour.Max) result -= (int)eDirectionFour.Max;

		return (eDirectionFour)result;
	}

}

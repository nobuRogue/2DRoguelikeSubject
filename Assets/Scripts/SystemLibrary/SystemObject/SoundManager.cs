/**
 * @file SoundManager.cs
 * @brief �T�E���h�Ǘ�
 * @author yao
 * @date 2025/2/25
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

using static CommonModule;

public class SoundManager : SystemObject {
	[SerializeField]
	private AudioSource _bgmAudioSource = null;
	[SerializeField]
	private AudioSource _seAudioSource = null;

	[SerializeField]
	private BGMAssign _bgmAssign = null;
	[SerializeField]
	private SEAssign _seAssign = null;

	public static SoundManager instance { get; private set; } = null;

	public override async UniTask Initialize() {
		instance = this;
	}

	/// <summary>
	/// BGM�Đ�
	/// </summary>
	/// <param name="bgmID"></param>
	public void PlayBGM(int bgmID) {
		if (!IsEnableIndex(_bgmAssign.bgmArray, bgmID)) return;

		_bgmAudioSource.clip = _bgmAssign.bgmArray[bgmID];
		_bgmAudioSource.Play();
	}

	/// <summary>
	/// BGM���~�߂�
	/// </summary>
	public void StopBGM() {
		_bgmAudioSource.Stop();
	}

	/// <summary>
	/// SE�Đ�
	/// </summary>
	/// <param name="seID"></param>
	public void PlaySE(int seID) {
		if (!IsEnableIndex(_seAssign.seArray, seID)) return;

		_seAudioSource.clip = _seAssign.seArray[seID];
		_seAudioSource.Play();
	}

	/// <summary>
	/// SE���~�߂�
	/// </summary>
	public void StopSE() {
		_seAudioSource.Stop();
	}
}

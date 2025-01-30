/**
 * @file MapSquareUtility.cs
 * @brief �}�X�֘A���s����
 * @author yao
 * @date 2025/1/21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class MapSquareUtility {

	/// <summary>
	/// �w������̃}�X�擾
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static MapSquareData GetToDirSquare(int x, int y, eDirectionFour dir) {
		ToVectorPos(ref x, ref y, dir);
		return MapSquareManager.instance.Get(x, y);
	}

	/// <summary>
	/// �w������̃}�X�擾
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static MapSquareData GetToDirSquare(int x, int y, eDirectionEight dir) {
		ToVectorPos(ref x, ref y, dir);
		return MapSquareManager.instance.Get(x, y);
	}

	/// <summary>
	/// �ړ��ۂ̔���
	/// </summary>
	/// <param name="startX"></param>
	/// <param name="startY"></param>
	/// <param name="moveSquare"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public static bool CanMove(int startX, int startY, MapSquareData moveSquare, eDirectionEight dir) {
		return CanMoveTerrain(startX, startY, moveSquare, dir) && !moveSquare.existCharacter;
	}

	public static bool CanMoveTerrain(int startX, int startY, MapSquareData moveSquare, eDirectionEight dir) {
		if (moveSquare == null ||
			moveSquare.terrain == eTerrain.Wall) return false;
		// �΂߈ړ����ۂ�
		if (!dir.IsSlant()) return true;
		// �΂߈ړ��Ȃ�A�����𕪊����e�����̃}�X���`�F�b�N
		eDirectionFour[] separateDir = dir.Separate();
		for (int i = 0, max = separateDir.Length; i < max; i++) {
			MapSquareData checkSquare = GetToDirSquare(startX, startY, separateDir[i]);
			if (checkSquare == null ||
				checkSquare.terrain == eTerrain.Wall) return false;

		}
		return true;
	}

	public static bool GetVisibleArea(ref List<int> visibleArea, MapSquareData sourceSquare) {
		InitializeList(ref visibleArea);
		if (sourceSquare == null) return false;
		// ����8�}�X���擾
		GetChebyshevAroundSquare(ref visibleArea, sourceSquare);
		visibleArea.Add(sourceSquare.ID);
		// ����8�}�X�����g�̃}�X�ɕ���������Ύ擾
		List<int> aroundRoomList = new List<int>(visibleArea.Count);
		bool existPlayer = false;
		PlayerCharacter player = CharacterManager.instance.GetPlayer();
		for (int i = 0, max = visibleArea.Count; i < max; i++) {
			MapSquareData targetSquare = MapSquareManager.instance.Get(visibleArea[i]);
			if (targetSquare == null) continue;
			// �v���C���[�������邩����
			if (!existPlayer && player.ExistMoveTrail(targetSquare.ID)) existPlayer = true;

			if (targetSquare.roomID < 0) continue;

			if (aroundRoomList.Exists(roomID => roomID == targetSquare.roomID)) continue;

			aroundRoomList.Add(targetSquare.roomID);
		}
		// �אڂ��Ă��镔���̑S�}�X�擾
		for (int i = 0, max = aroundRoomList.Count; i < max; i++) {
			RoomData roomData = MapSquareManager.instance.GetRoom(aroundRoomList[i]);
			List<int> roomSquareList = roomData.squareIDList;
			for (int j = 0, roomSquareMax = roomSquareList.Count; j < roomSquareMax; j++) {
				if (visibleArea.Exists(roomSquareID => roomSquareID == roomSquareList[j])) continue;
				// �v���C���[�������邩����
				MapSquareData targetSquare = MapSquareManager.instance.Get(roomSquareList[j]);
				if (!existPlayer && player.ExistMoveTrail(targetSquare.ID)) existPlayer = true;

				visibleArea.Add(roomSquareList[j]);
			}
		}
		return existPlayer;
	}

	/// <summary>
	/// ���`�F�r�V�F�t�����̃}�X��S�Ď擾
	/// </summary>
	/// <param name="result"></param>
	/// <param name="sourceSquare"></param>
	/// <param name="distance"></param>
	public static void GetChebyshevAroundSquare(ref List<int> result, MapSquareData sourceSquare, int distance = 1) {
		InitializeList(ref result, distance * 8);
		if (sourceSquare == null) return;

		if (distance == 0) {
			result.Add(sourceSquare.ID);
			return;
		}

		int countMax = distance * 2;
		int sourceX = sourceSquare.positionX;
		int sourceY = sourceSquare.positionY;
		for (int count = 0; count < countMax; count++) {
			MapSquareData targetSquare = MapSquareManager.instance.Get(sourceX - distance + count, sourceY - distance);
			if (targetSquare != null) result.Add(targetSquare.ID);

			targetSquare = MapSquareManager.instance.Get(sourceX + distance, sourceY - distance + count);
			if (targetSquare != null) result.Add(targetSquare.ID);

			targetSquare = MapSquareManager.instance.Get(sourceX + distance - count, sourceY + distance);
			if (targetSquare != null) result.Add(targetSquare.ID);

			targetSquare = MapSquareManager.instance.Get(sourceX - distance, sourceY + distance - count);
			if (targetSquare != null) result.Add(targetSquare.ID);

		}

	}

}

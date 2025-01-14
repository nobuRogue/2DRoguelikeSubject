/**
 * @file GameEnum.cs
 * @brief 列挙体定義
 * @author yao
 * @date 2025/1/9
 */


public enum eGamePart {
	Invalid = -1,//不正値
	Standby,    // 準備パート 
	Title,      // タイトルパート
	MainGame,   // メインパート
	Ending,     // エンディングパート
	Max,        // 
}

public enum eTerrain {
	Invalid = -1,   // 不正値
	Passage,		// 通路
	Room,			// 部屋
	Wall,			// 壁
	Stair,			// 階段
	Max,			
}


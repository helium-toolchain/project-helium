#pragma once
namespace Helium {
	namespace FastUtil {
		enum __declspec(dllexport) ChatFormatting {
			Black = 0,
			DarkBlue = 1,
			DarkGreen = 2,
			Cyan = 3,
			DarkRed = 4,
			DarkPurple = 5,
			Orange = 6,
			Gray = 7,
			DarkGray = 8,
			Blue = 9,
			Green = 10,
			Aqua = 11,
			Red = 12,
			LightPurple = 13,
			Yellow = 14,
			White = 15,

			Obfuscated = 64,
			Bold = 65,
			Strikethrough = 66,
			Underline = 67,
			Italic = 68,

			Reset = 128
		};
	}
}
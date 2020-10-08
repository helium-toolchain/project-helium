#pragma once

#include "./chatformatting.h"
#include <string>

using namespace std;

namespace Helium {
	namespace FastUtil {
		namespace Nbt {
			class __declspec(dllexport) Tag {
				public:
					ChatFormatting SyntaxHighlighting_Key = Aqua;
					ChatFormatting SyntaxHighlighting_String = Green;
					ChatFormatting SyntaxHighlighting_Number = Orange;
					ChatFormatting SyntaxHighlighting_NumberType = Red;
					short int Id;

					virtual void Write(string Filename);
					virtual string ToString();
					virtual Tag Copy();
			};
		}
	}
}


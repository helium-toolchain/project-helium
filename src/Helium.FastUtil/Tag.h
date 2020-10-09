#pragma once

#include "./chatformatting.h"
#include <string>

using namespace std;

namespace Helium {
	namespace FastUtil {
		namespace Nbt {
			class __declspec(dllexport) Tag {
				public:
					ChatFormatting SyntaxHighlighting_Key = ChatFormatting::Aqua;
					ChatFormatting SyntaxHighlighting_String = ChatFormatting::Green;
					ChatFormatting SyntaxHighlighting_Number = ChatFormatting::Orange;
					ChatFormatting SyntaxHighlighting_NumberType = ChatFormatting::Red;
					short int Id;

					virtual void Write(string Filename);
					virtual string ToString();
					virtual Tag Copy();
			};
		}
	}
}


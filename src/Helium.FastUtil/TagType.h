#pragma once

#include <fstream>
#include <string>
#include "Tag.h"

using namespace std;

namespace Helium {
	namespace FastUtil {
		namespace Nbt {
			class __declspec(dllexport) TagType {
				public:
					virtual Tag Load(istream inputStream);
					virtual string GetName();
					virtual string GetPrettyName();
			};
		}
	}
}


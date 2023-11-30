// Native C++ ‚ÌŠÖ”‚ðŒÄ‚Ño‚·
#include "pch.h"

#if _WIN64
EXTERN_C __declspec(dllexport) int APIENTRY GetMaxValue64( int left, int right )
#else
EXTERN_C __declspec(dllexport) int APIENTRY GetMaxValue32( int left, int right )
#endif
{
	return left > right ? left : right;
}


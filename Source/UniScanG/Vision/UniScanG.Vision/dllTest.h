#pragma once

#ifdef UniScanG.Vision_EXPORTS
#define MYDLL __declspec(dllexport)
#else
#define MYDLL __declspec(dllimport)
#endif

MYDLL int add(int x, int y);
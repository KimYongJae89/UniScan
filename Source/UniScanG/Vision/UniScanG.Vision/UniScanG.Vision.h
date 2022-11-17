#pragma once

#define AVX
//#define SSE2 

#if defined(SSE2)
#define SIMDTYPE "SSE2"
#include <emmintrin.h>
#define MULTIPLE_BYTE_COUNT 0x10
#define DATATYPE __m128i
#define SET08(x) _mm_set1_epi8(((char)x))
#define SET16(x) _mm_set1_epi16(((char)x))
#define LOAD(x) _mm_loadu_si128(((DATATYPE*)x))
#define STORE(x,y) _mm_storeu_si128(((DATATYPE*)x), ((DATATYPE)y))
#define PACK16(x,y) _mm_packs_epi16(((DATATYPE)x),((DATATYPE)y))
#define UNPACKHI08(x,y) _mm_unpackhi_epi8(((DATATYPE)x),((DATATYPE)y))
#define UNPACKLO08(x,y) _mm_unpacklo_epi8(((DATATYPE)x),((DATATYPE)y))
#define CMPGT16(x,y) _mm_cmpgt_epi16(((DATATYPE)x),((DATATYPE)y))
#define SUB08(x,y) _mm_subs_epu8(((DATATYPE)x),((DATATYPE)y))
#define MIN08(x,y) _mm_min_epu8(((DATATYPE)x),((DATATYPE)y))
#define MAX08(x,y) _mm_max_epu8(((DATATYPE)x),((DATATYPE)y))

#elif defined(AVX)
#define SIMDTYPE "AVX"
#include <immintrin.h>
#define MULTIPLE_BYTE_COUNT 0x20
#define INT __m256i
#define SINGLE __m256

#define SET08i(x) _mm256_set1_epi8(((char)x))
#define SET16i(x)_mm256_set1_epi16(((short)x))
#define SET32s(x)_mm256_set1_ps(((float)x))

#define LOAD(x) _mm256_loadu_si256(((INT*)x))
#define STORE(x,y) _mm256_storeu_si256(((INT*)x), ((INT)y))

#define UNPACKHI08(x,y) _mm256_unpackhi_epi8(((INT)x),((INT)y))
#define UNPACKLO08(x,y) _mm256_unpacklo_epi8(((INT)x),((INT)y))
#define PACK16(x,y) _mm256_packs_epi16(((INT)x),((INT)y))
#define CMPGT16(x,y) _mm256_cmpgt_epi16(((INT)x),((INT)y))
#define CMPGE16(x,y) _mm256_cmpge_epi16(((INT)x),((INT)y))
#define SUB08(x,y) _mm256_subs_epu8(((INT)x),((INT)y))
#define MIN08(x,y) _mm256_min_epu8(((INT)x),((INT)y))
#define MAX08(x,y) _mm256_max_epu8(((INT)x),((INT)y))

#define UNPACKHI16(x,y) _mm256_unpackhi_epi16(((INT)x),((INT)y))
#define UNPACKLO16(x,y) _mm256_unpacklo_epi16(((INT)x),((INT)y))
#define PACK32(x,y) _mm256_packus_epi32(((INT)x),((INT)y))
#define MUL16(x,y) _mm256_mulhrs_epi16(((INT)x),((INT)y))
#define DIV32(x,y) _mm256_div_ps(((DATATYPE)x),((DATATYPE)y))

#endif

struct ImageData
{
	BYTE* ptr;
	int iPitch;
};

#define BINALIZE(x, T, Z) PACK16(CMPGT16(UNPACKLO08(x, Z), UNPACKLO08(T, Z)), CMPGT16(UNPACKHI08(x, Z), UNPACKHI08(T, Z)))
//#define SIMD(c,l,r,T,Z) BINALIZE(MAX08(MIN08(SUB08(c, l), SUB08(c, r)), MIN08(SUB08(l, c), SUB08(r, c))), T, Z)

inline INT SIMD(INT c, INT l, INT r, INT T, INT Z);
inline void _GvCalcV2E(BYTE *pSrc, BYTE *pLeft, BYTE *pRight, BYTE *pEdge, BYTE *pDiff, BYTE *pTh, BYTE *pBin, INT xmmOfsL, INT xmmOfsR, INT xmmZero);
inline void _Sub(INT src1, INT src2, INT dst);
inline void _Mul(BYTE *pSrc, BYTE *pDst, BYTE *pMul100, INT xmmZero, SINGLE xmmHundredth);

extern "C" __declspec(dllexport) const char* GetSIMDType();
extern "C" __declspec(dllexport) int HelloWorld(int integer);

extern "C" __declspec(dllexport) void SubMinSubMinMaxBin(BYTE *pSrc, BYTE *pRefL, BYTE *pRefR, BYTE *pDst, int iWidth, int iHeight, int iPitch);
extern "C" __declspec(dllexport) void Binarize(BYTE *pSrc, BYTE *pDst, int iWidth, int iHeigth, int iPitch);
extern "C" __declspec(dllexport) void Subtract(BYTE *pSrc1, BYTE *pSrc2, BYTE *pDst, int iWidth, int iHeight, int iPitch);

extern "C" __declspec(dllexport) void GvCalcV2E(ImageData src, ImageData refL, ImageData refR, ImageData refE, ImageData diff, ImageData thMap, ImageData bin, BYTE ofsL, BYTE ofsR, int iWidth, int iHeight);
extern "C" __declspec(dllexport) void CalibrateLine(ImageData src, ImageData dst, ImageData mul100, int iWidth, int iHeigth);


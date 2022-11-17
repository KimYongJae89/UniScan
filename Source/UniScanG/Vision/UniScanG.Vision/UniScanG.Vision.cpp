// UniScanG.Vision.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "UniScanG.Vision.h"


inline INT SIMD(INT c, INT l, INT r, INT T, INT Z)
{
	INT x = MIN08(SUB08(c, l), SUB08(c, r));
	INT y = MIN08(SUB08(l, c), SUB08(r, c));
	INT z = MAX08(x, y);
	return BINALIZE(z, T, Z);
}

inline void _GvCalcV2E(BYTE *pSrc, BYTE *pLeft, BYTE *pRight, BYTE *pEdge, BYTE *pDiff, BYTE *pTh, BYTE *pBin, INT xmmOfsL, INT xmmOfsR, INT xmmZero)
{
	INT xmmSrc = LOAD(pSrc);
	INT xmmRefL = LOAD(pLeft);
	INT xmmRefR = LOAD(pRight);
	INT xmmRefE = pEdge == NULL ? SET08i(0) : LOAD(pEdge);

	INT a = MIN08(SUB08(SUB08(xmmSrc, xmmRefL), xmmOfsL), SUB08(SUB08(xmmSrc, xmmRefR), xmmOfsR));
	INT b = MIN08(SUB08(SUB08(xmmRefL, xmmSrc), xmmOfsL), SUB08(SUB08(xmmRefR, xmmSrc), xmmOfsR));
	INT c = SUB08(MAX08(a, b), xmmRefE);
	STORE(pDiff, c);

	INT xmmTh = LOAD(pTh);
	INT d = BINALIZE(c, xmmTh, xmmZero);
	STORE(pBin, d);
}

inline void _Sub(BYTE *pSrc1, BYTE *pSrc2, BYTE *pDst)
{
	INT src1 = LOAD(pSrc1);
	INT src2 = LOAD(pSrc2);

	INT dst = SUB08(src1, src2);
	STORE(pDst, dst);
}

inline void _Mul(BYTE *pSrc, BYTE *pDst, BYTE *pMul100, INT xmmZero, SINGLE xmmHundredth)
{
	INT xmmSrc08 = LOAD(pSrc);
	INT pSrc16[2] = { UNPACKLO08(xmmSrc08, xmmZero), UNPACKHI08(xmmSrc08, xmmZero) };

	INT pSrc32[4] =
	{
		UNPACKLO16(pSrc16[0], xmmZero),		UNPACKHI16(pSrc16[0], xmmZero),
		UNPACKLO16(pSrc16[1], xmmZero),		UNPACKHI16(pSrc16[1], xmmZero)
	};

	INT xmmMul08 = LOAD(pMul100);
	INT pMul16[2] = { UNPACKLO08(xmmMul08, xmmZero), UNPACKHI08(xmmMul08, xmmZero) };
	INT pMul32[4] = 
	{
		UNPACKLO16(pMul16[0], xmmZero),	UNPACKHI16(pMul16[0], xmmZero),
		UNPACKLO16(pMul16[1], xmmZero),	UNPACKHI16(pMul16[1], xmmZero)
	};

	INT pRes32[4];
	for (int i = 0; i < 4; i++)
	{
		SINGLE xmmSrc = _mm256_cvtepi32_ps(pSrc32[i]);
		SINGLE xmmMul1 = _mm256_cvtepi32_ps(pMul32[i]);
		SINGLE xmmMul2 = _mm256_mul_ps(xmmMul1, xmmHundredth);
		SINGLE xmmRes = _mm256_mul_ps(xmmSrc, xmmMul2);
		pRes32[i] = _mm256_cvtps_epi32(xmmRes);
	}
	
	INT pRes16[2] = { PACK32(pRes32[0], pRes32[1]), PACK32(pRes32[2], pRes32[3]) };
	INT pRes08 = PACK16(pRes16[0], pRes16[1]);
	STORE(pDst, pRes08);
}

const char* GetSIMDType()
{
	return (char*)SIMDTYPE;
}

int HelloWorld(int integer)
{
	return integer * 3;
}

void SubMinSubMinMaxBin(BYTE *pSrc, BYTE *pRefL, BYTE *pRefR, BYTE *pDst, int iWidth, int iHeigth, int iPitch)
{
	int iAlignStep = iWidth / MULTIPLE_BYTE_COUNT;
	int iRemainWidth = iWidth % MULTIPLE_BYTE_COUNT;

	INT xmmTh = SET08i(50);
	INT xmm0 = SET08i(0);

	for (int r = 0; r < iHeigth; r++)
	{
		int offsetBytes = r * iPitch;
		BYTE *_pSrc = pSrc + offsetBytes;
		BYTE *_pRefL = pRefL + offsetBytes;
		BYTE *_pRefR = pRefR + offsetBytes;
		BYTE *_pDst = pDst + offsetBytes;

		for (int c = 0; c < iAlignStep; c++)
		{
			INT dC = LOAD(_pSrc);
			INT dL = LOAD(_pRefL);
			INT dR = LOAD(_pRefR);
			STORE(_pDst, SIMD(dC, dL, dR, xmmTh, xmm0));

			_pSrc += MULTIPLE_BYTE_COUNT;
			_pRefL += MULTIPLE_BYTE_COUNT;
			_pRefR += MULTIPLE_BYTE_COUNT;
			_pDst += MULTIPLE_BYTE_COUNT;
		}

		if (iRemainWidth > 0)
		{
			if (iAlignStep > 0)
			{
				offsetBytes += (iWidth - MULTIPLE_BYTE_COUNT);
				_pSrc = pSrc + offsetBytes;
				_pRefL = pRefL + offsetBytes;
				_pRefR = pRefR + offsetBytes;
				_pDst = pDst + offsetBytes;

				INT xmmSrc = LOAD(_pSrc);
				INT xmmRefL = LOAD(_pRefL);
				INT xmmRefR = LOAD(_pRefR);
				STORE(_pDst, SIMD(xmmSrc, xmmRefL, xmmRefR, xmmTh, xmm0));
			}
			else
			{
				// 으허허허허허허
			}
		}
	}
}

void Binarize(BYTE *pSrc, BYTE *pDst, int iWidth, int iHeight, int iPitch)
{
	int iAlignStep = iWidth / MULTIPLE_BYTE_COUNT;
	int iRemainStep = iWidth % MULTIPLE_BYTE_COUNT;

	INT xmmTh = SET08i(50);
	INT xmm0 = SET08i(0);

	for (int r = 0; r < iHeight; r++)
	{
		int offsetBytes = (r * iPitch);
		BYTE *_pSrc = pSrc + offsetBytes;
		BYTE *_pDst = pDst + offsetBytes;

		for (int c = 0; c < iAlignStep; c++)
		{
			STORE(_pDst, BINALIZE(LOAD(_pSrc), xmmTh, xmm0));

			_pSrc += MULTIPLE_BYTE_COUNT;
			_pDst += MULTIPLE_BYTE_COUNT;
		}

		if (iRemainStep > 0)
		{
			_pSrc = pSrc + offsetBytes + iWidth - MULTIPLE_BYTE_COUNT;
			_pDst = pDst + offsetBytes + iWidth - MULTIPLE_BYTE_COUNT;

			STORE(_pDst, BINALIZE(LOAD(_pSrc), xmmTh, xmm0));
		}
	}
}

void Subtract(BYTE *pSrc1, BYTE *pSrc2, BYTE *pDst, int iWidth, int iHeight, int iPitch)
{
	int iAlignStep = iWidth / MULTIPLE_BYTE_COUNT;
	int iRemainStep = iWidth % MULTIPLE_BYTE_COUNT;

	for (int r = 0; r < iHeight; r++)
	{
		int offsetBytes = (r * iPitch);
		BYTE *_pSrc1 = pSrc1 + offsetBytes;
		BYTE *_pSrc2 = pSrc2 + offsetBytes;
		BYTE *_pDst = pDst + offsetBytes;

		for (int c = 0; c < iAlignStep; c++)
		{
			_Sub(_pSrc1, _pSrc2, _pDst);

			_pSrc1 += MULTIPLE_BYTE_COUNT;
			_pSrc2 += MULTIPLE_BYTE_COUNT;
			_pDst += MULTIPLE_BYTE_COUNT;
		}

		if (iRemainStep > 0)
		{
			_pSrc1 = pSrc1 + offsetBytes + iWidth - MULTIPLE_BYTE_COUNT;
			_pSrc2 = pSrc2 + offsetBytes + iWidth - MULTIPLE_BYTE_COUNT;
			_pDst = pDst + offsetBytes + iWidth - MULTIPLE_BYTE_COUNT;
			
			_Sub(_pSrc1, _pSrc2, _pDst);
		}
	}
}


void GvCalcV2E(ImageData src, ImageData refL, ImageData refR, ImageData refE, ImageData diff, ImageData thMap, ImageData bin, BYTE ofsL, BYTE ofsR, int iWidth, int iHeight)
{
	int iAlignStep = iWidth / MULTIPLE_BYTE_COUNT;
	int iRemainWidth = iWidth % MULTIPLE_BYTE_COUNT;

	INT xmmOfsL = SET08i(ofsL);
	INT xmmOfsR = SET08i(ofsR);
	INT xmmZero = SET08i(0);

	for (int r = 0; r < iHeight; r++)
	{
		BYTE *_pSrc = src.ptr + (r * src.iPitch);
		BYTE *_pRefL = refL.ptr + (r * refL.iPitch);
		BYTE *_pRefR = refR.ptr + (r * refR.iPitch);
		BYTE *_pEdge = refE.ptr + (r * refE.iPitch);
		BYTE *_pDiff = diff.ptr + (r * diff.iPitch);
		BYTE *_pTh = thMap.ptr + (r * thMap.iPitch);
		BYTE *_pBin = bin.ptr + (r * bin.iPitch);
		
		for (int c = 0; c < iAlignStep; c++)
		{
			_GvCalcV2E(_pSrc, _pRefL, _pRefR, _pEdge, _pDiff, _pTh, _pBin, xmmOfsL, xmmOfsR, xmmZero);
			//STORE(_pDiff, xmm255);

			_pSrc += MULTIPLE_BYTE_COUNT;
			_pRefL += MULTIPLE_BYTE_COUNT;
			_pRefR += MULTIPLE_BYTE_COUNT;
			_pEdge += MULTIPLE_BYTE_COUNT;
			_pDiff += MULTIPLE_BYTE_COUNT;
			_pTh += MULTIPLE_BYTE_COUNT;
			_pBin += MULTIPLE_BYTE_COUNT;
		}

		if (iRemainWidth > 0)
		{
			if (iAlignStep > 0)
			{
				_pSrc = src.ptr + ((r * src.iPitch) + (iWidth - MULTIPLE_BYTE_COUNT));
				_pRefL = refL.ptr + ((r * refL.iPitch) + (iWidth - MULTIPLE_BYTE_COUNT));
				_pRefR = refR.ptr + ((r * refR.iPitch) + (iWidth - MULTIPLE_BYTE_COUNT));
				_pEdge = refE.ptr + ((r * refE.iPitch) + (iWidth - MULTIPLE_BYTE_COUNT));
				_pDiff = diff.ptr + ((r * diff.iPitch) + (iWidth - MULTIPLE_BYTE_COUNT));
				_pTh = thMap.ptr + ((r * thMap.iPitch) + (iWidth - MULTIPLE_BYTE_COUNT));
				_pBin = bin.ptr + ((r * bin.iPitch) + (iWidth - MULTIPLE_BYTE_COUNT));

				_GvCalcV2E(_pSrc, _pRefL, _pRefR, _pEdge, _pDiff, _pTh, _pBin, xmmOfsL, xmmOfsR, xmmZero);
				//STORE(_pDiff, xmm255);
			}
			else
			{
				// 으허허허허허허
			}
		}
	}
}


void CalibrateLine(ImageData src, ImageData dst, ImageData mul100, int iWidth, int iHeight)
{
	int iAlignStep = iWidth / MULTIPLE_BYTE_COUNT;
	int iRemainWidth = iWidth % MULTIPLE_BYTE_COUNT;

	INT xmmZero = SET08i(0);
	SINGLE xmmHundredth = _mm256_set1_ps(0.01f);

	for (int r = 0; r < iHeight; r++)
	{
		BYTE *_pSrc = src.ptr + (r * src.iPitch);
		BYTE *_pDst = dst.ptr + (r * dst.iPitch);
		BYTE *_pMul = mul100.ptr;

		for (int c = 0; c < iAlignStep; c++)
		{
			_Mul(_pSrc, _pDst, _pMul, xmmZero, xmmHundredth);

			_pSrc += MULTIPLE_BYTE_COUNT;
			_pDst += MULTIPLE_BYTE_COUNT;
			_pMul += MULTIPLE_BYTE_COUNT;
		}

		if (iRemainWidth > 0)
		{
			//BYTE *_pSrc = src.ptr + ((r + 1) * src.iPitch - 1) - MULTIPLE_BYTE_COUNT;
			//BYTE *_pDst = dst.ptr + ((r + 1) * dst.iPitch - 1) - MULTIPLE_BYTE_COUNT;
			//BYTE *_pMul = mul100.ptr + (mul100.iPitch - 1) - MULTIPLE_BYTE_COUNT;
			//_Mul(_pSrc, _pDst, _pMul, xmmZero, xmmHundredth);

			_pSrc -= iRemainWidth;
			_pDst -= iRemainWidth;
			_pMul -= iRemainWidth;
			_Mul(_pSrc, _pDst, _pMul, xmmZero, xmmHundredth);
		}
	}
}


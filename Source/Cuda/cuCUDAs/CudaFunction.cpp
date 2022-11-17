#include "CudaFunction.h"
#include "DeviceFunction.h"
#include "HostFunction.h"
#include "CCL/CudaCCL.h"
#include "Morphology/CudaMorphology.h"
#include "NPP/NppProcessing.h"
#include "helper_image.h"
#include "Ransac/CudaRansac.h"
#include "Math/CudaMath.h"

#include <set>
#include <map>
#include <vector>
#include <cmath>

#ifdef __CUDACC__
#define KERNEL_ARGS2(grid, block) <<< grid, block >>>
#define KERNEL_ARGS3(grid, block, sh_mem) <<< grid, block, sh_mem >>>
#define KERNEL_ARGS4(grid, block, sh_mem, stream) <<< grid, block, sh_mem, stream >>>
#else
#define KERNEL_ARGS2(grid, block)
#define KERNEL_ARGS3(grid, block, sh_mem)
#define KERNEL_ARGS4(grid, block, sh_mem, stream)
#endif


using namespace std;

static int selectedDeviceId = 0;
static map<int, CudaImage*> cudaImageMap;
static map<int, CudaBlobList*> cudaBlobMap;

//cudaError_t lastCudaError;
std::string lastErrorString;

inline void ThrowIfInvalidData(CudaImage *pImageSrc)
{
	if (pImageSrc == NULL)
	{
		lastErrorString = "Image Missed";
		throw std::exception();
	}
}

inline void ThrowIfInvalidData(CudaImage *pImageSrc, CudaImage *pImageDst, bool checkSize)
{
	if (pImageSrc == NULL)
	{
		lastErrorString = ("Image Missed (Src)");
		throw std::exception();
	}

	if (pImageDst == NULL)
	{
		lastErrorString = ("Image Missed (Dst)");
		throw std::exception();
	}

	if (checkSize)
	{
		if ((pImageSrc->Width != pImageDst->Width) || (pImageSrc->Height != pImageDst->Height))
		{
			lastErrorString = ("Size mismatch");
			throw std::exception();
		}
	}
}

inline void ThrowIfInvalidData(CudaImage *pImageSrc1, CudaImage *pImageSrc2, CudaImage *pImageDst, bool checkSize)
{
	if (pImageSrc1 == NULL)
	{
		lastErrorString = "Image Missed (Src1)";
		throw std::exception();
	}

	if (pImageSrc2 == NULL)
	{
		lastErrorString = "Image Missed (Src2)";
		throw std::exception();
	}

	if (pImageDst == NULL)
	{
		lastErrorString = "Image Missed (Dst)";
		throw std::exception();
	}

	if (checkSize)
	{
		if ((pImageSrc1->Width != pImageSrc2->Width) || (pImageSrc1->Width != pImageDst->Width) ||
			(pImageSrc1->Height != pImageSrc2->Height) || (pImageSrc1->Height != pImageDst->Height))
		{
			lastErrorString = "Size mismatch";
			throw std::exception();
		}
	}
}

inline void ThrowIfError(cudaError_t cudaStatus)
{
	if (cudaStatus != cudaSuccess)
	{
		lastErrorString = cudaGetErrorString(cudaStatus);
		throw std::exception();
	}
}

inline void ThrowIfError()
{
	ThrowIfError(cudaGetLastError());
}


CudaImage* GetCudaImage(UINT imageId)
{
	auto itr = cudaImageMap.find(imageId);
	if (itr == cudaImageMap.end())
		return NULL;

	return itr->second;
}

////////////////////////////////////////////////////////////////////////////////////

int  CUDA_DEVICE_LAST_ERROR_LEN()
{
	return lastErrorString.length();
}

void CUDA_DEVICE_LAST_ERROR(char* pString, int maxLen)
{
	int len = MIN(lastErrorString.length(), maxLen);
	lastErrorString.copy(pString, len);
}

int CUDA_DEVICE_COUNT()
{
	int count = 0;
	cudaGetDeviceCount(&count);
	ThrowIfError();
	return count;
}

char* CUDA_DEVICE_NAME(int gpuNo)
{
	cudaDeviceProp prop;
	cudaGetDeviceProperties(&prop, gpuNo);
	return prop.name;
}

void CUDA_DEVICE_SELECT(int gpuNo)
{
	//cudaDeviceReset();

	// Choose which GPU to run on, change this on a multi-GPU system.
	int count = 0;
	cudaGetDeviceCount(&count);
	ThrowIfError();

	/*if (gpuNo >= count)
		return false;*/

	cudaSetDevice(gpuNo);
	ThrowIfError();

	selectedDeviceId = gpuNo;
}

void CUDA_DEVICE_RESET()
{
	for (auto itr = cudaImageMap.begin();
		itr != cudaImageMap.end();
		itr++)
	{
		delete itr->second;
	}

	cudaImageMap.clear();

	cudaDeviceReset();

	UNIQUE_IMAGE_NO = 1;
}

void CUDA_THREAD_NUM(int threadNum)
{
	MAX_GPU_THREAD_NUM = threadNum;
}

void CUDA_WAIT()
{
	cudaDeviceSynchronize();
}

UINT CUDA_CREATE_IMAGE(int width, int height, int depth)
{
	int id = UNIQUE_IMAGE_NO++;

	CudaImage* cudaImage = new CudaImage(id, 0);
	cudaImage->Alloc(width, height, depth);

	ThrowIfError();

	cudaImageMap[id] = cudaImage;
	return id;
}

void CUDA_CLIP_IMAGE(UINT srcImage, UINT dstImage, int x, int y, int width, int height)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	int blockSize = static_cast<int>(sqrt(static_cast<double>(cudaSrcImage->Width * cudaSrcImage->Height) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(blockSize, blockSize, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	devClip KERNEL_ARGS2(grid, threads) ((BYTE*)cudaSrcImage->pImageBuffer, (BYTE*)cudaDstImage->pImageBuffer,
		cudaSrcImage->Width, cudaSrcImage->Height,
		x, y, width, height);

	ThrowIfError();
}

void CUDA_SET_IMAGE(UINT image, void* pImageBuffer)
{
	auto cudaImage = GetCudaImage(image);
	cudaImage->SetImage(pImageBuffer);
	ThrowIfError();
}

void CUDA_CLEAR_IMAGE(UINT image)
{
	auto cudaImage = GetCudaImage(image);
	cudaImage->ClearImage();
	ThrowIfError();
}

void CUDA_FREE_IMAGE(UINT image)
{
	auto cudaImage = GetCudaImage(image);
	if (!cudaImage)
		return;

	delete cudaImage;
	cudaImageMap.erase(image);
}

void CUDA_GET_IMAGE(UINT image, void* pDstBuffer)
{
	auto cudaSrcImage = GetCudaImage(image);
	cudaSrcImage->GetImage(pDstBuffer);
	ThrowIfError();
}


void CUDA_SET_ROI(UINT image, double x, double y, double width, double height)
{
	auto cudaSrcImage = GetCudaImage(image);
	if (!cudaSrcImage)
		return;

	cudaSrcImage->SetImageROI(x, y, width, height);
	ThrowIfError();
}

void CUDA_RESET_ROI(UINT image)
{
	auto cudaSrcImage = GetCudaImage(image);
	if (!cudaSrcImage)
		return;

	cudaSrcImage->ResetImageROI();
	ThrowIfError();
}

#include <cassert>

void CUDA_CREATE_PROFILE(UINT srcImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);

	int blockNum = (cudaSrcImage->Width + MAX_GPU_THREAD_NUM - 1) / MAX_GPU_THREAD_NUM;
	devProfile KERNEL_ARGS2(blockNum, MAX_GPU_THREAD_NUM) ((BYTE*)cudaSrcImage->pImageBuffer, cudaSrcImage->pProfile, cudaSrcImage->Width, cudaSrcImage->Height);

	ThrowIfError();
}

void CUDA_AREA_AVERAGE(UINT srcImage, int areaCount, float* hostAverage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);

	float* devAverage;
	cudaMalloc(&devAverage, sizeof(float) * areaCount);
	cudaMemset(devAverage, 0, sizeof(float) * areaCount);

	//int width = (cudaSrcImage->Width / (float)areaCount) + 0.5;
	dim3 grid = GetThreadGrid(cudaSrcImage->Width, cudaSrcImage->Height, MAX_GPU_THREAD_NUM, 1);
	dim3 thread(MAX_GPU_THREAD_NUM, 1);

	int* mutex;
	cudaMalloc(&mutex, sizeof(int));
	cudaMemset(mutex, 0, sizeof(int));

	devAreaSum KERNEL_ARGS2(grid, thread) ((BYTE*)cudaSrcImage->pImageBuffer, cudaSrcImage->Width, cudaSrcImage->Height, areaCount, devAverage, mutex);

	cudaMemcpy(hostAverage, devAverage, areaCount * sizeof(float), cudaMemcpyDeviceToHost);
	cudaFree(devAverage);

	ThrowIfError();
}

void CUDA_CREATE_LABEL_BUFFER(UINT srcImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);

	int size = cudaSrcImage->Width * cudaSrcImage->Height;
	if (cudaSrcImage->plabelBuffer == NULL)
	{
		cudaSrcImage->plabelBuffer = new LabelBuffer();

		if (cudaSuccess != cudaMalloc(&cudaSrcImage->plabelBuffer->pLabel, size * sizeof(int)))
			ThrowIfError();

		if (cudaSuccess != cudaMalloc(&cudaSrcImage->plabelBuffer->pMask, size * sizeof(BYTE)))
			ThrowIfError();
	}
}

void CUDA_CREATE_FFT_BUFFER(UINT srcImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);

	ThrowIfError();

	//auto result = cudaMalloc(&(cudaSrcImage->pComplexBuffer), 16384 * 16384 * sizeof(cufftComplex));
	if (cudaSuccess != cudaMalloc((void**)(&cudaSrcImage->pComplexBuffer), cudaSrcImage->Height * (cudaSrcImage->Width + 1) * sizeof(cufftComplex)))
		ThrowIfError();

	ThrowIfError();

	if (CUFFT_SUCCESS != cufftPlan2d(&cudaSrcImage->forwardPlan, cudaSrcImage->Height, cudaSrcImage->Width, CUFFT_R2C))
		ThrowIfError();

	ThrowIfError();

	if (CUFFT_SUCCESS != cufftPlan2d(&cudaSrcImage->inversePlan, cudaSrcImage->Height, cudaSrcImage->Width, CUFFT_C2R))
		ThrowIfError();

	ThrowIfError();
}

void CUDA_BINARIZE(UINT srcImage, UINT dstImage, float lower, float upper, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	int width = static_cast<int>(sqrt(static_cast<double>(cudaSrcImage->Width * cudaSrcImage->Height) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devBinarize KERNEL_ARGS2(grid, threads) (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, lower, upper, inverse, cudaSrcImage->pRoiRect);

	//cudaError_t cudaStatus = cudaDeviceSynchronize();
	//assert(cudaStatus == cudaSuccess);

	ThrowIfError();
}

void CUDA_BINARIZE_LOWER(UINT srcImage, UINT dstImage, float lower, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	int blockSize = sqrt(MAX_GPU_THREAD_NUM);

	int gridWidth = cudaSrcImage->Width %  blockSize == 0 ? cudaSrcImage->Width / blockSize : cudaSrcImage->Width / blockSize + 1;
	int gridHeight = cudaSrcImage->Height %  blockSize == 0 ? cudaSrcImage->Height / blockSize : cudaSrcImage->Height / blockSize + 1;

	dim3 grid(gridWidth, gridHeight, 1);
	dim3 block(blockSize, blockSize, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devLowerBinarize KERNEL_ARGS2(grid, block) (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, lower, inverse, cudaSrcImage->pRoiRect);

	ThrowIfError();
}

void CUDA_BINARIZE_UPPER(UINT srcImage, UINT dstImage, float upper, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	int width = static_cast<int>(sqrt(static_cast<double>(cudaSrcImage->Width * cudaSrcImage->Height) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devUpperBinarize KERNEL_ARGS2(grid, threads) (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, upper, inverse, cudaSrcImage->pRoiRect);

	ThrowIfError();
}

void CUDA_ADAPTIVE_BINARIZE(UINT srcImage, UINT dstImage, float lower, float upper, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	int width = static_cast<int>(sqrt(static_cast<double>(size) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devAdaptiveBinarize KERNEL_ARGS2(grid, threads) (pSrcBuffer, pDstBuffer, cudaSrcImage->pProfile, cudaSrcImage->Width, cudaSrcImage->Height, lower, upper, inverse, cudaSrcImage->pRoiRect);

	ThrowIfError();
}

void CUDA_ADAPTIVE_BINARIZE_LOWER(UINT srcImage, UINT dstImage, float lower, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	int width = static_cast<int>(sqrt(static_cast<double>(size) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devLowerAdaptiveBinarize KERNEL_ARGS2(grid, threads) (pSrcBuffer, pDstBuffer, cudaSrcImage->pProfile, cudaSrcImage->Width, cudaSrcImage->Height, lower, inverse, cudaSrcImage->pRoiRect);

	ThrowIfError();

	cudaDeviceSynchronize();
}

void CUDA_ADAPTIVE_BINARIZE_UPPER(UINT srcImage, UINT dstImage, float upper, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	int width = static_cast<int>(sqrt(static_cast<double>(size) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	devUpperAdaptiveBinarize KERNEL_ARGS2(grid, threads) (pSrcBuffer, pDstBuffer, cudaSrcImage->pProfile, cudaSrcImage->Width, cudaSrcImage->Height, upper, inverse, cudaSrcImage->pRoiRect);

	ThrowIfError();

	cudaDeviceSynchronize();
}

void CUDA_AREA_BINARIZE(UINT srcImage, UINT dstImage,
	int areaCount,
	float* hostAverage,
	int lowThreshold, int highThreshold,
	bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	float* devAverage;
	cudaMalloc(&devAverage, sizeof(float) * areaCount);
	cudaMemcpy(devAverage, hostAverage, areaCount * sizeof(float), cudaMemcpyHostToDevice);

	int size = cudaSrcImage->Width * cudaSrcImage->Height;

	int width = static_cast<int>(sqrt(static_cast<double>(size) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	devAreaBinarize KERNEL_ARGS2(grid, threads)
		((BYTE*)cudaSrcImage->pImageBuffer, (BYTE*)cudaDstImage->pImageBuffer,
			areaCount, devAverage,
			cudaSrcImage->Width, cudaSrcImage->Height,
			lowThreshold, highThreshold, inverse, cudaSrcImage->pRoiRect);

	cudaFree(devAverage);

	ThrowIfError();
}


bool CUDA_EDGE_DETECT(UINT srcImage, EdgeSearchDirection dir, int threshold, int* startPos, int* endPos)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	ThrowIfInvalidData(cudaSrcImage);

	*startPos = cudaSrcImage->Width - 1;
	*endPos = 0;

	int range = cudaSrcImage->Width;
	float* pProfile = new float[cudaSrcImage->Width];
	cudaSrcImage->GetProfile(pProfile);

	return hostEdgeDetectX(pProfile, 0, range, range, threshold, startPos, endPos, 1);
}

#include <thrust/unique.h>

int CUDA_LABELING(UINT binImage)
{
	auto cudaBinImage = GetCudaImage(binImage);

	if (!cudaBinImage)
		return -1;

	int width = cudaBinImage->Width;
	int height = cudaBinImage->Height;

	if (cudaBinImage->plabelBuffer == NULL)
		return -1;

	LabelBuffer* labelBuffer = cudaBinImage->plabelBuffer;

	CudaCCL ccl;
	return ccl.ccl_hm((BYTE*)cudaBinImage->pImageBuffer, (int*)labelBuffer->pLabel, (BYTE*)labelBuffer->pMask, width, height);

	//// Device array -> Device vector
	//thrust::device_vector<int> imageMap((int*)cudaLabelImage->pImageBuffer, (int*)cudaLabelImage->pImageBuffer + (width * height));

	//// Remove duplicate pairs
	//auto newEnd = thrust::unique(imageMap.begin(), imageMap.end());

	//// Trim the vectors
	//imageMap.erase(newEnd, imageMap.end());
	/////

	//int* imageData = imageMap.data().get();
	//int count = imageMap.size();

}

bool CUDA_BLOBING(UINT binImage, UINT srcImage, int count,
	UINT* areaArray, UINT* xMinArray, UINT* xMaxArray, UINT* yMinArray, UINT* yMaxArray,
	UINT* vMinArray, UINT* vMaxArray, float* vMeanArray)
{
	auto cudaBinmage = GetCudaImage(binImage);
	auto cudaSrcImage = GetCudaImage(srcImage);

	if (!cudaBinmage)
		return false;

	int width = cudaBinmage->Width;
	int height = cudaBinmage->Height;

	if (cudaBinmage->plabelBuffer == NULL)
		return false;

	CudaCCL ccl;
	return ccl.blob_hm((BYTE*)cudaSrcImage->pImageBuffer, (int*)cudaBinmage->plabelBuffer->pLabel, width, height, count, areaArray, xMinArray, xMaxArray, yMinArray, yMaxArray, vMinArray, vMaxArray, vMeanArray);
}

void CUDA_MORPHOLOGY_ERODE(UINT srcImage, UINT dstImage, int maskSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	CudaMorphology::Erode(cudaSrcImage, cudaDstImage, maskSize);
}

void CUDA_MORPHOLOGY_DILATE(UINT srcImage, UINT dstImage, int maskSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	CudaMorphology::Dilate(cudaSrcImage, cudaDstImage, maskSize);
}

void CUDA_MORPHOLOGY_OPEN(UINT srcImage, UINT dstImage, int maskSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	CudaMorphology::Open(cudaSrcImage, cudaDstImage, maskSize);
}

void CUDA_MORPHOLOGY_CLOSE(UINT srcImage, UINT dstImage, int maskSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	CudaMorphology::Close(cudaSrcImage, cudaDstImage, maskSize);
}

void CUDA_MORPHOLOGY_THINNING(UINT srcImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);

	ThrowIfInvalidData(cudaSrcImage);
	CudaMorphology::Thinning(cudaSrcImage);
}

void CUDA_SOBEL(UINT srcImage, UINT dstImage)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	CudaMorphology::Sobel(cudaSrcImage, cudaDstImage);
}

void CUDA_CANNY(UINT srcImage, UINT dstImage, int lowThreshold, int highThreshold)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	NppProcessing::Canny(cudaSrcImage, cudaDstImage, lowThreshold, highThreshold);
}

void Save(UINT srcImage, char* fileName)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	ThrowIfInvalidData(cudaSrcImage);

	UINT w = (UINT)cudaSrcImage->Width;
	UINT h = (UINT)cudaSrcImage->Height;

	BYTE* pBuffer = new BYTE[w * h];

	cudaSrcImage->GetImage(pBuffer);
	ThrowIfError();

	sdkSavePGM<BYTE>(fileName, pBuffer, w, h);
	ThrowIfError();
}

void NoiseRemoval(UINT srcImage, double radius, double threshold)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	ThrowIfInvalidData(cudaSrcImage);

	if (CUFFT_SUCCESS != cufftExecR2C(cudaSrcImage->forwardPlan, (float*)cudaSrcImage->pImageBuffer, cudaSrcImage->pComplexBuffer))
		throw exception("cufftExecR2C");

	cudaDeviceSynchronize();
	ThrowIfError();

	int width = cudaSrcImage->Width / 2 + 1;
	int height = cudaSrcImage->Height;

	devNoiseRemoval KERNEL_ARGS2(width * height / MAX_GPU_THREAD_NUM, MAX_GPU_THREAD_NUM) (cudaSrcImage->pComplexBuffer, width, height, radius, threshold);
	cudaDeviceSynchronize();

	ThrowIfError();
	auto result = cufftExecC2R(cudaSrcImage->inversePlan, cudaSrcImage->pComplexBuffer, (float*)cudaSrcImage->pImageBuffer);
	if (CUFFT_SUCCESS != result)
		throw exception("cufftExecC2R");

	cudaDeviceSynchronize();
	ThrowIfError();
}

void CUDA_EDGE_FINDER(UINT srcImage, UINT dstImage, float threshold, int arraySize, bool inverse)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);


	int width = static_cast<int>(sqrt(static_cast<double>(cudaSrcImage->Width * cudaSrcImage->Height) / MAX_GPU_THREAD_NUM)) + 1;
	dim3 grid(width, width, 1);
	dim3 threads(MAX_GPU_THREAD_NUM, 1, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	bool isSame = cudaSrcImage == cudaDstImage;
	if (isSame)
	{
		cudaMalloc(&pSrcBuffer, cudaSrcImage->Width * cudaSrcImage->Height * sizeof(BYTE));
		cudaMemcpy(pSrcBuffer, pDstBuffer, cudaSrcImage->Width * cudaSrcImage->Height * sizeof(BYTE), cudaMemcpyHostToHost);
	}

	devEdgeFinder KERNEL_ARGS2(grid, threads) (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, threshold, arraySize, inverse, cudaSrcImage->pRoiRect);

	ThrowIfError();
	if (isSame)
		cudaFree(pSrcBuffer);
}

void CUDA_MEAN_FILTER(UINT srcImage, UINT dstImage, int filterSize)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage);

	int blockSize = sqrt(MAX_GPU_THREAD_NUM);

	int gridWidth = cudaSrcImage->Width %  blockSize == 0 ? cudaSrcImage->Width / blockSize : cudaSrcImage->Width / blockSize + 1;
	int gridHeight = cudaSrcImage->Height %  blockSize == 0 ? cudaSrcImage->Height / blockSize : cudaSrcImage->Height / blockSize + 1;

	dim3 grid(gridWidth, gridHeight, 1);
	dim3 block(blockSize, blockSize, 1);

	BYTE* pSrcBuffer = (BYTE*)cudaSrcImage->pImageBuffer;
	BYTE* pDstBuffer = (BYTE*)cudaDstImage->pImageBuffer;

	bool isSame = cudaSrcImage == cudaDstImage;
	if (isSame)
	{
		cudaMalloc(&pSrcBuffer, cudaSrcImage->Width * cudaSrcImage->Height * sizeof(BYTE));
		cudaMemcpy(pSrcBuffer, pDstBuffer, cudaSrcImage->Width * cudaSrcImage->Height * sizeof(BYTE), cudaMemcpyHostToHost);
	}

	devMeanFilter KERNEL_ARGS3(grid, block, sizeof(int) * (blockSize + filterSize - 1) * (blockSize + filterSize - 1)) (pSrcBuffer, pDstBuffer, cudaSrcImage->Width, cudaSrcImage->Height, filterSize);

	ThrowIfError();

	if (isSame)
		cudaFree(pSrcBuffer);

	cudaDeviceSynchronize();
}

void CUDA_RESIZE(UINT srcImage, UINT dstImage, int interpolation)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	auto cudaDstImage = GetCudaImage(dstImage);

	// 이미지 사이즈 확인
	ThrowIfInvalidData(cudaSrcImage, cudaDstImage, false);

	NppProcessing::Resize(cudaSrcImage, cudaDstImage, interpolation);
}


void CUDA_MATH_XOR(UINT srcImage1, UINT srcImage2, UINT dstImage)
{
	auto cudaSrcImage1 = GetCudaImage(srcImage1);
	auto cudaSrcImage2 = GetCudaImage(srcImage2);
	auto cudaDstImage = GetCudaImage(dstImage);

	ThrowIfInvalidData(cudaSrcImage1, cudaSrcImage2, cudaDstImage);

	CudaMath::XOR(cudaSrcImage1, cudaSrcImage2, cudaDstImage);
}

void CUDA_MATH_AND(UINT srcImage1, UINT srcImage2, UINT dstImage)
{
	auto cudaSrcImage1 = GetCudaImage(srcImage1);
	auto cudaSrcImage2 = GetCudaImage(srcImage2);
	auto cudaDstImage = GetCudaImage(dstImage);

	ThrowIfInvalidData(cudaSrcImage1, cudaSrcImage2, cudaDstImage);

	CudaMath::AND(cudaSrcImage1, cudaSrcImage2, cudaDstImage);
}

void CUDA_MATH_OR(UINT srcImage1, UINT srcImage2, UINT dstImage)
{
	auto cudaSrcImage1 = GetCudaImage(srcImage1);
	auto cudaSrcImage2 = GetCudaImage(srcImage2);
	auto cudaDstImage = GetCudaImage(dstImage);

	ThrowIfInvalidData(cudaSrcImage1, cudaSrcImage2, cudaDstImage);

	CudaMath::OR(cudaSrcImage1, cudaSrcImage2, cudaDstImage);
}

void CUDA_MATH_MUL(UINT srcImage, float* profile)
{
	auto cudaSrcImage = GetCudaImage(srcImage);
	CudaMath::MUL(cudaSrcImage, profile);
	ThrowIfError();
}

bool CUDA_RANSAC(int width, int height,
	double* xArray, double* yArray,
	int size, double* cost, double* gradient,
	double* centerX, double* centerY, double threshold)
{
	CudaRansac ransac;
	return ransac.Compute(width, height, xArray, yArray, size, cost, gradient, centerX, centerY, threshold);
}

void CUDA_CALIBRATION(UINT srcImage) {}

void CUDA_CREATE_CALIBRATION_BUFFER(UINT srcImage, float calValue, UINT length) {}

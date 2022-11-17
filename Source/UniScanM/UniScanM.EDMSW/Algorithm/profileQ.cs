using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using UniEye.Base.Settings;
using UniScanM.EDMSW.Settings;

namespace UniScanM.EDMSW.Algorithm
{
    public class profileQ
    {
        private int buffersize = 10;
        private int imgWidth = 0;
        private int imgHeight = 0;


        private List<float[]> m_Profilelist_Hor = new List<float[]>(); //패턴 가로 엣지용
        private List<float[]> m_Profilelist_Ver = new List<float[]>(); //패턴 세로 길이용
        private List<float> m_AverageIntensitylist = new List<float>(); //프레임당 Intensity// 밝기가 밝아지면 핀홀로인해 압동롤러가 올라가서 인쇄가 되인 않았음을 검출함

        public bool lastValid = false;
        public bool LastValid
        {
            get { return lastValid; }
        }


        public int Buffersize
        {
            get { return buffersize; }
            set { buffersize = value; }
        }

        float m_nowFrameIntensity = 0;
        private readonly object thisLock = new object();

        public profileQ(int img_width, int img_height, int _buffersize)
        {
            imgWidth = img_width;
            imgHeight = img_height;

            buffersize = _buffersize;
            buffer_Image = new AlgoImage[_buffersize];
            for (int i = 0; i < _buffersize; i++)
            {
                buffer_Image[i] = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, img_width, img_height);
            }
            image_Index = 0;

            display_Image = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, img_width, img_height);
        }

        public void Clear()
        {
            lock (thisLock)
            {
                lastValid = false;
                image_Index = 0;
                m_AverageIntensitylist.Clear();
                m_Profilelist_Hor.Clear();
            }
        }
        int image_Index = 0;
        AlgoImage[] buffer_Image = null;
        AlgoImage display_Image = null;

        private float MedianAverage(List<float> list, float CenterPercent=40)
        {
            return 0;
        }

        private void AddProfilelist_Hor(float[] projection)
        {
            m_Profilelist_Hor.Add(projection);
            while (m_Profilelist_Hor.Count > buffersize)
                m_Profilelist_Hor.RemoveAt(0);
        }

        private void AddProfilelist_Ver(float[] projection)
        {
            m_Profilelist_Ver.Add(projection);
            while (m_Profilelist_Ver.Count > buffersize*3)
                m_Profilelist_Ver.RemoveAt(0);
        }

        public void AddImage(AlgoImage algoImage)
        {
            try
            {
                float[] projectionHor = null;
                float[] projectionVer = null;
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
                projectionHor = imageProcessing.Projection(algoImage, DynMvp.Vision.Direction.Horizontal, ProjectionType.Mean); //6.5ms
                projectionVer = imageProcessing.Projection(algoImage, DynMvp.Vision.Direction.Vertical, ProjectionType.Mean); 
                //double resizeScale = 0.1f;
                //AlgoImage resizedImg = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType,
                //    (int)(algoImage.Width * resizeScale), (int)(algoImage.Height * resizeScale));
                //imageProcessing.Resize(algoImage, resizedImg, resizeScale);
                //resizedImg.Save("D:\\TestImage.bmp");

                lock (thisLock)
                {
                    if (buffer_Image[image_Index] == null)
                        buffer_Image[image_Index] = algoImage.Clone();
                    else
                        buffer_Image[image_Index].Copy(algoImage); //memcpy..//이미있는 메모리에 데이터 쓰는거임

                    image_Index++;
                    if (image_Index >= buffersize) image_Index = 0;

                    //프레임 평균 밝기를 저장
                    m_nowFrameIntensity = projectionHor.Average();
                   
                    if (m_AverageIntensitylist.Count < buffersize )
                    {
                        //처음부터 제외실킬건가.. > SKIP을 알수 없으며, > 기존데이터 표시
                        //아님 q는 그대로 작동식키고, 가져갈때 GetAverge()에서 제외시킬것인가. > SKIP 알고, 제외 > 찾아서 제외해야함.. 왜냐하면 비동기며 몇개가 추가 되었는지 알수 없음.
                        AddProfilelist_Hor(projectionHor);
                        AddProfilelist_Ver(projectionVer);
                        m_AverageIntensitylist.Add(m_nowFrameIntensity);
                    }
                    else
                    {
                        //현재 밝기가 기존대비 skipIntensityDiffRange 범위 안에서 변화하면
                        int skipIntensityDiffRange = EDMSSettings.Instance().SkipDiffBrightness;
                        if (Math.Abs(m_nowFrameIntensity - m_AverageIntensitylist.Average()) < skipIntensityDiffRange)//사실 어두워 질순 없다...
                        {
                            lastValid = true;
                            AddProfilelist_Hor(projectionHor);
                            AddProfilelist_Ver(projectionVer);

                            m_AverageIntensitylist.Add(m_nowFrameIntensity);
                            if (m_AverageIntensitylist.Count >= buffersize + 20)
                                m_AverageIntensitylist.RemoveAt(0);//일정량 초과 되면 제한
                        }
                        //현재 밝기가 평균보다 크면(흰색으로 계속 나오면...압동롤러가 올라갔으며 인쇄가 안되고 있음, 핀홀이 발생한경우 )
                        else lastValid = false;
                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
        }

        private void Enqueue(float[] data, AlgoImage algoImage)
        {

        }
        public float[] GetProfileHorAverge(ref bool isValid, ref float intensity)
        {
            float[] avg = null;
            lock (thisLock)
            {
                isValid = this.lastValid;
                intensity = m_nowFrameIntensity;// this.m_AverageIntensitylist.Last(); //현시점의 마지막 이미지 밝기
                if (m_Profilelist_Hor.Count < buffersize) return null;
                int size = m_Profilelist_Hor[0].Length;
                avg = new float[size];
                avg.Initialize();

                foreach (float[] prof in m_Profilelist_Hor)
                {
                    float avgFrame = prof.Average();
                    for (int i = 0; i < size; i++)
                    {
                        avg[i] += prof[i];
                    }
                }

                double col = m_Profilelist_Hor.Count;
                for (int i = 0; i < size; i++)
                {
                    avg[i] = (float)(avg[i] / col);
                }
            }
            return avg;
        }

        public float[] GetProfileVer()
        {
            float[] profile = null;
            lock (thisLock)
            {
                if (m_Profilelist_Ver.Count < buffersize) return null;
                profile = Combine(m_Profilelist_Ver);
            }
            return profile;
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
        private float[] Combine(params float [][] arrays)
        {
            float[] rv = new float[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach(var array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
        private float[] Combine(List<float[]> arrays) //이것만 확인된 나머지도 주의// offset이나 size가 Byte 단위임.
        {
            float[] rv = new float[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (var array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length* sizeof(float));
                offset += array.Length* sizeof(float);
            }
            return rv;
        }

        public AlgoImage getDisplayImage()
        {
            lock (thisLock)
            {
                int last_index = image_Index - 1;
                if (last_index < 0) last_index += Buffersize;

                //if (display_Image == null)
                //    display_Image = buffer_Image[last_index].Clone();
                //else
                display_Image.Copy(buffer_Image[last_index]);
            }
            return display_Image;
        }
        //public List<T> Datalist
        //{
        //    get { return m_Datalist; }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfControlLibrary.Teach
{
    // 티칭할 대상. 배경화면?
    public interface ITeachModel
    {
        BitmapSource BgBitmapSource { get; }

        DrawObj[] GetDrawObjs();

        ITeachProbe CreateProbe(Rect rect);
        ITeachProbe GetProbe(int idx);
        void AddProbe(ITeachProbe probe);
        void RemoveProbe(ITeachProbe probe);
    }

    // 마우스 클릭-드래그로 만들어지는 사각형.
    public interface ITeachProbe
    {
        Rect Rect{ get; set; }

        DrawObj GetDrawObj();
    }
}

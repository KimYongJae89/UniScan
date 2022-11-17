using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public class VersionHelper
    {
        static VersionHelper instance = null;
        public string AssemblyName { get; private set; }

        public int MajorVersion { get; private set; }
        public int MinorVersion { get; private set; }

        public DateTime BuildDateTime { get; private set; }

        public string VersionString { get => $"{MajorVersion}.{MinorVersion}"; }

        public string BuildString { get => this.BuildDateTime.ToString("yyMMdd.HHmm"); }

        public static VersionHelper Instance()
        {
            if (instance == null)
                instance = new VersionHelper();
            return instance;
        }

        private VersionHelper()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            this.AssemblyName = assembly.FullName.Split(',')[0].Trim();

            // 최종 Project의 Version 가져옴
            string strVersionText = assembly.FullName.Split(',')[1].Trim().Split('=')[1];
            int.TryParse(strVersionText.Split('.')[0], out int major);
            int.TryParse(strVersionText.Split('.')[1], out int minor);
            int.TryParse(strVersionText.Split('.')[2], out int buildDay);
            int.TryParse(strVersionText.Split('.')[3], out int buildsec);

            this.MajorVersion = major;
            this.MinorVersion = minor;
            this.BuildDateTime = GetBuildDateTime(buildDay, buildsec);
        }

        private DateTime GetBuildDateTime()
        {
            //1. Assembly.GetExecutingAssembly().FullName의 값은 'ApplicationName, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' 와 같다.  
            Assembly assembly = Assembly.GetEntryAssembly();
            //Assembly assembly = Assembly.GetExecutingAssembly();
            string strVersionText = assembly.FullName.Split(',')[1].Trim().Split('=')[1];

            //2. Version Text의 세번째 값(Build Number)은 2000년 1월 1일부터 Build된 날짜까지의 총 일(Days) 수 이다. 
            int intDays = Convert.ToInt32(strVersionText.Split('.')[2]);
            DateTime refDate = new DateTime(2000, 1, 1);
            DateTime dtBuildDate = refDate.AddDays(intDays);

            //3. Verion Text의 네번째 값(Revision NUmber)은 자정으로부터 Build된 시간까지의 지나간 초(Second) 값 이다. 
            int intSeconds = Convert.ToInt32(strVersionText.Split('.')[3]);
            intSeconds = intSeconds * 2;
            dtBuildDate = dtBuildDate.AddSeconds(intSeconds);

            //4. 시차조정 
            //DaylightTime daylingTime = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildDate.Year);
            //if (TimeZone.IsDaylightSavingTime(dtBuildDate, daylingTime))
            //    dtBuildDate = dtBuildDate.Add(daylingTime.Delta);

            return dtBuildDate;
        }

        private DateTime GetBuildDateTime(int day, int sec)
        {
            DateTime dtBuildDate = new DateTime(2000, 1, 1).AddDays(day);
            dtBuildDate = dtBuildDate.AddSeconds(2 * sec);
            return dtBuildDate;
        }
    }
}

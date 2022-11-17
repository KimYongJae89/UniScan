using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aladdin.HASP;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public class HaspHelper
    {
        static HaspHelper instance = null;
        public static HaspHelper Instance()
        {
            if (instance == null)
            {
                instance = new HaspHelper();
            }

            return instance;
        }

        private string GetVendorCode()
        {
            return
                "6VA968FjuJZ9cWlNImhn0fNYeNLGq7bia6jkjtCUaBxdIzu6W7zUZNeQkvhu1Gd9y0rWaEYNimHuNLcl" +
                "Tctl0t8D9d/WIJqH9Vr4X+fX9As+ch9qKy204l53j6pgwv/kPsqbuM8tcBlixMyxevpNTAgRk+k2nZYG" +
                "41Hlds7yBOzAMG+u7jSuesxItBZ3bTKsihbI8TAv3H6CZX1q5EPl/dInETCK1oUgnOj2yFzQYpIx3/jW" +
                "0o9EyvV6FNXzPvd2tNmmsqckZv2l5s95jyxL9REA4O9PxCkxT2mzO0SSK5NBjO6Gqg0cafVYXMwXjI7l" +
                "RgVV0xDipgPLdnklujau4U341UzX2CNm/hif13GIsujzpVRAKNAkvqwIqfcoYUhVJepI0Bj9hjkXJmN4" +
                "cK8Ag+Swr3a7aDWjjUQMDkJ3v8+N+GonXArEiw6pTRnHjAed2Er/kVrRSJ1k18xlwPbmswc1ANsTTFTI" +
                "rjwHMAlI1nKwGoHFeGJQq1K+pa6bte+yCd6SjnAaK2DihNHbGMTvGHIrNC2v27hxcOc8HMAlRXxRTF11" +
                "Zk4lvA5PAj6VfWyvE9EaE0Lk6EdIkmO6s7O6fJTBFZO8PxVBSck6mBbYTddRHKDfaRPeKlALz/fk9YOt" +
                "tmLwfciybMk77LyTFmM0FqCnZF5t//3ucJYumY6qw4esnxgTenSmTBOztXn7RL3rK8acbcnrMUGXgOJz" +
                "8wSYzmarN0DMECjW6OnefyvjtCjKMMJRCVZ3epQrCu19+ckOSJTBMj88dOSWur+lM/swNrFf6pR9enzu" +
                "ioR6zGHqn6JO+gzPh5kI3XxBb9UvAj92mZKplz+q13mI2AzppAJ1UyFzlgEa3Yp1iLyObmh9hvHNgm1d" +
                "5g4BBG7+4jQB019JU/oi6qQVYETYA23CSFQFzNwl5vMB2F2I8uCb9AVfpvDZxjca31ZksjDGoYgtYWqh" +
                "cuZME1ZDpzrvdnhqhPmWqQ==";

            //return
            //    "vyoZmDLH3lh9u8KHSZUC20+1WYo3zblIcDZooBcyqaTp1ngL/81ScbTs249SWzqkjqSyrBodTRT3FJ4m" +
            //    "dGpYno9CrkV5pEoXzhi94ONDbGFXbQkL1N0IKP8UKuQf5Q+qBuOhX7BgsBAt55qGv+x6GXbNsHI/ayoY" +
            //    "KdHXr6lnYgV+66bujV9XmSESd2FUFrws32V9Gly1I9rd/tg37ur79K05a8yxDKXaUW9BUYRwqIgos3m9" +
            //    "NgSZS2kCZC9Enj0NEOWOEVHoDa8rXMV0bfWBrA3gZklqFiilvvsYseSES1zulaavvGmRp9+s+/nWg521" +
            //    "PDOGGrQ8TGhItSnqp86uq4h7O4f4ESIvqMMcJ4mqq4XREQfQip0mgjKT052gD3NBfiJPwPw1eLnsaxu7" +
            //    "IEXwqr76EkiaKhrmfPXRoqre5DOPy5q+BZTr12IrhOrftecb3kne/XvEMDIsO51NYcYkz1h/vBC386KM" +
            //    "6Ebhgd9fD9ZErlMuVhNbJOYaj+Qbh2misnPymt3OTe45WYK2zMzS0QPN9IuiMw/T9R6jhJszzEol+xkY" +
            //    "wcoJwrq9d+tJD022msXlM6agsIkAG1c3jz/I6Dmxvqr7YhkJiNjXJjlD/POrlXEtajfdTvJHhWV9Xb/v" +
            //    "SQLkKDipVA8IUoQVZNAlx1dr87ZrmZR9hb9jaal2EFlcdwTc5KkhJmv9S69OnG7sh6dXeh8etkjoK6se" +
            //    "S0FBF02EDAoG8NAUV1f0d/qPwJrnw4yAeRADHWcYngzbSNr9dhOojM3w7bCjQ2GM7ftSP9y/+PVXXNzi" +
            //    "BbBtehCtpe1x9MdvR9Odg+FodEedcqMuyA0ubHgrGPWBuTP8bsr+DuncoAcLKXU1GQl98vQBYsOpSnAc" +
            //    "gmhb1XiPG2wpgVma/eKGXS0C9dOOCnF1Fx+PMCUBHvMmyYiPQhnBkA0Dbi4TJI0cket7d3lCeluYKJ95" +
            //    "r9CA4xKbptXtDXcG2lMehw==";
        }

        public static void CheckHaspLicsence()
        {
            bool license = false;

            do
            {
                license = HaspHelper.Instance().CheckLicense();
                if (license == false)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.RetryCancel;
#if DEBUG
                            buttons = MessageBoxButtons.AbortRetryIgnore;
#endif

                    DialogResult dialogResult = MessageBox.Show($"HASP License not founded", "Error", buttons);
                    switch (dialogResult)
                    {
                        case DialogResult.Abort:
                        case DialogResult.Cancel:
                            throw new Exception($"HASP License not founded");

                        case DialogResult.Retry:
                            break;
#if DEBUG

                        case DialogResult.Ignore:
                            license = true;
                            break;
#endif
                    }
                }
            } while (license == false);
        }

        public bool CheckLicense(string vendorCode = "")
        {
            if (string.IsNullOrEmpty(vendorCode))
                vendorCode = GetVendorCode();             

            Hasp hasp = new Hasp();
            HaspStatus status = hasp.Login(vendorCode);

            if (status != HaspStatus.StatusOk)
                return false;
            //throw new Exception($"HASP License Check Fail - {status}");

            return true;
        }

        public int GetFeatureCode(string vendorCode = "")
        {
            if (string.IsNullOrEmpty(vendorCode))
                vendorCode = GetVendorCode();

            if (CheckLicense(vendorCode) == false)
                return -1;

            for (int code = 1; code < 100; code++)
            {
                HaspFeature feature;
                feature = HaspFeature.FromFeature(code);

                Hasp hasp = new Hasp(feature);
                HaspStatus status = hasp.Login(vendorCode);

                if (HaspStatus.StatusOk == status)
                {
                    return code;
                }
            }

            return 0;
        }
    }
}

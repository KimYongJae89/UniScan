using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Vision.Cognex;
using DynMvp.Vision.Euresys;
using DynMvp.Vision.Matrox;
using DynMvp.Base;
using DynMvp.Devices;

namespace DynMvp.Vision
{
    public enum ImagingLibrary
    {
        OpenCv, EuresysOpenEVision, CognexVisionPro, MatroxMIL, Halcon, Custom
    }

    class License
    {
        public ImagingLibrary imagingLibrary;
        public string subLicense;

        public bool exist;

        public License(ImagingLibrary imagingLibrary, string subLicense, bool exist)
        {
            this.imagingLibrary = imagingLibrary;
            this.subLicense = subLicense;
            this.exist = exist;
        }
    }

    public class LicenseManager
    {
        static List<License> licenseList = new List<License>();

        static bool cognexInstallationChecked = false;
        static bool cognexInstalled = false;

        static bool milInstallationChecked = false;
        static bool milInstalled = false;

        public static bool VisionProInstalled()
        {
            if (cognexInstallationChecked == false)
            {
                cognexInstalled = RegistryHelper.IsInstalled("Cognex VisionPro");
                cognexInstallationChecked = true;
            }

            return cognexInstalled;
        }

        public static bool MilInstalled()
        {
            if (milInstallationChecked == false)
            {
                milInstalled = RegistryHelper.IsInstalled("Matrox Imaging") || RegistryHelper.IsInstalled("Matrox Imaging (64-bit)");
                milInstallationChecked = true;
            }

            return milInstalled;
        }

        public static void CheckImagingLicense()
        {
            if (MilInstalled() == true)
            {
                LogHelper.Debug(LoggerType.StartUp, "MIL license installed");
            }

            if (VisionProInstalled() == true)
            {
                LogHelper.Debug(LoggerType.StartUp, "Cognex license installed");
            }
        }

        public static bool LicenseExist(ImagingLibrary imagingLibrary, string subLicenseType)
        {
            License foundLicense = null;

            string[] subLicenses = subLicenseType.Split(new char[] { ',', ';' });

            if (string.IsNullOrWhiteSpace(subLicenseType))
            {
                foundLicense = licenseList.Find(x =>
                    { return x.imagingLibrary == imagingLibrary; });
            }
            else
            {
                foreach (string subLicense in subLicenses)
                {
                    foundLicense = licenseList.Find( x =>
                    { return x.imagingLibrary == imagingLibrary && x.subLicense == subLicense; });

                    if (foundLicense != null)
                        break;
                }
            }

            if (foundLicense != null)
            {
                return foundLicense.exist;
            }
            else
            {
                bool exist = false;

                if (string.IsNullOrWhiteSpace(subLicenseType))
                {
                    switch (imagingLibrary)
                    {
                        case ImagingLibrary.EuresysOpenEVision:
                            exist = EuresysHelper.LicenseExist();
                            break;
                        case ImagingLibrary.OpenCv:
                            exist = true;
                            break;
                    }

                    if (exist == true)
                    {
                        licenseList.Add(new License(imagingLibrary, "", exist));
                    }
                }
                else
                {
                    foreach (string subLicense in subLicenses)
                    {
                        foundLicense = licenseList.Find(x =>
                        { return x.imagingLibrary == imagingLibrary && x.subLicense == subLicense; });

                        if (foundLicense == null)
                        {
                            exist = false;

                            string[] alternativeLicenses = subLicense.Split('|');
                            foreach (string alternativeLicense in alternativeLicenses)
                            {
                                switch (imagingLibrary)
                                {
                                    case ImagingLibrary.MatroxMIL:
                                        if (MilInstalled() == true)
                                            exist |= MatroxHelper.LicenseExist(alternativeLicense);
                                        break;
                                    case ImagingLibrary.CognexVisionPro:
                                        if (VisionProInstalled() == true)
                                            exist |= CognexHelper.LicenseExist(alternativeLicense);
                                        break;
                                }
                            }

                            if (exist == false)
                                return false;

                            licenseList.Add(new License(imagingLibrary, subLicense, exist));
                        }
                    }
                }
            }

            return true;
        }
    }
}

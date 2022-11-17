using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public abstract class UniEyeException : ApplicationException
    {
        public UniEyeException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, "UniEyeException");
        }
        public UniEyeException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public UniEyeException(string message, Exception innerEx) 
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class GrabFailException : UniEyeException
    {
        public GrabFailException()
        {
            LogHelper.Error(LoggerType.Grab, "GrabFailException");
        }

        public GrabFailException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Grab, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }

        public GrabFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Grab, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    // Vision Object의 유효성에 문제가 있을 때 발생하는 Exception
    public class InvalidObjectException : ApplicationException
    {
        public InvalidObjectException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, "InvalidObjectException");
        }
        public InvalidObjectException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public InvalidObjectException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class AllocFailedException : ApplicationException
    {
        public AllocFailedException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, "AllocFailedException");
        }
        public AllocFailedException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public AllocFailedException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class InvalidModelNameException : ApplicationException
    {
        public InvalidModelNameException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, "InvalidModelNameException");
        }
        public InvalidModelNameException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public InvalidModelNameException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class InvalidSourceException : ApplicationException
    {
        public InvalidSourceException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, "InvalidSourceException");
        }
        public InvalidSourceException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public InvalidSourceException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class InvalidTargetException : ApplicationException
    {
        public InvalidTargetException()
            : base()
        {
            LogHelper.Error(LoggerType.Error, "InvalidTargetException");
        }
        public InvalidTargetException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public InvalidTargetException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class InvalidDataException : ApplicationException
    {
        public InvalidDataException()
        {
            LogHelper.Error(LoggerType.Error, "InvalidDataException");
        }
        public InvalidDataException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public InvalidDataException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class TooManyItemsException : ApplicationException
    {
        public TooManyItemsException()
        {
            LogHelper.Error(LoggerType.Error, "TooManyItemsException");
        }
        public TooManyItemsException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public TooManyItemsException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class InvalidImageFormatException : ApplicationException
    {
        public InvalidImageFormatException()
        {
            LogHelper.Error(LoggerType.Error, "InvalidImageFormatException");
        }
        public InvalidImageFormatException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public InvalidImageFormatException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class InvalidTypeException : ApplicationException
    {
        public InvalidTypeException()
        {
            LogHelper.Error(LoggerType.Error, "InvalidTypeException");
        }
        public InvalidTypeException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public InvalidTypeException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class DepthScannerInitializeFailException : ApplicationException
    {
        public DepthScannerInitializeFailException()
        {
            LogHelper.Error(LoggerType.Error, "DepthScannerInitializeFailException");
        }
        public DepthScannerInitializeFailException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public DepthScannerInitializeFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class CameraInitializeFailException : ApplicationException
    {
        public CameraInitializeFailException()
        {
            LogHelper.Error(LoggerType.Error, "CameraInitializeFailException");
        }
        public CameraInitializeFailException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
        public CameraInitializeFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, String.Format("{0} - {1}{2}{3}", this.GetType().Name, message, Environment.NewLine, base.StackTrace));
        }
    }

    public class InvalidResourceException : ApplicationException
    {
        public InvalidResourceException() : base("Camera Initialization is Failed")
        {
            LogHelper.Error(LoggerType.Error, this.Message);
        }
        public InvalidResourceException(string message)
            : base(message)
        {
            LogHelper.Error(LoggerType.Error, message);
        }
        public InvalidResourceException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(LoggerType.Error, message);
        }
    }
}

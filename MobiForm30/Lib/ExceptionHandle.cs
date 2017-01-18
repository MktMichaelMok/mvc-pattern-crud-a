using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MobiForm30.Lib {
    public class ExceptionHandle {

        public enum MBF_ERROR: int {
            ERR_DB_ACCESS = 1000,
            ERR_DB_INSERT = 1001,
            ERR_DB_UPDATE = 1002,
            ERR_DB_DELETE = 1003,
            //
            ERR_DATA_EMPTY = 1101
        }


        /// <summary>
        /// Builds the exception message.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public string BuildExceptionMessage(Exception x)
        {
            Exception logException = x;
            if (x.InnerException != null) {
                logException = x.InnerException;
            }
            StringBuilder message = new StringBuilder();
            message.AppendLine();
            message.AppendLine("Error in Path : " + HttpContext.Current.Request.Path);
            // Get the QueryString along with the Virtual Path
            message.AppendLine("Raw Url : " + HttpContext.Current.Request.RawUrl);
            // Type of Exception
            message.AppendLine("Type of Exception : " + logException.GetType().Name);
            // Get the error message
            message.AppendLine("Message : " + logException.Message);
            // Source of the message
            message.AppendLine("Source : " + logException.Source);
            // Stack Trace of the error
            message.AppendLine("Stack Trace : " + logException.StackTrace);
            // Method where the error occurred
            message.AppendLine("TargetSite : " + logException.TargetSite);
            return message.ToString();
        }


        public string BuildUserMsg(MBF_ERROR errorCode)
        {
            string msg = "";
            switch (errorCode)
            {
                case MBF_ERROR.ERR_DB_ACCESS: msg = "DB acess error";
                break;
                case MBF_ERROR.ERR_DB_INSERT: msg = "DB insert error";                
                break;
                case MBF_ERROR.ERR_DB_UPDATE: msg = "DB update error";
                break;
                case MBF_ERROR.ERR_DB_DELETE: msg = "DB delete error";
                break;
                case MBF_ERROR.ERR_DATA_EMPTY: msg = "Data cannot be empty";
                break;
                //
                default: msg = "undefine error";
                break;
            }
            return string.Format("{0} - {1}", (int)errorCode, msg);
        }

        /// <summary>
        /// Builds the exception message.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public string BuildTraceMessage(MBF_ERROR errorCode, string functionName = "")
        {            
            StringBuilder message = new StringBuilder();
            message.AppendLine();
            message.AppendLine("Error in Path : " + HttpContext.Current.Request.Path);
            // Get the QueryString along with the Virtual Path
            message.AppendLine("Raw Url : " + HttpContext.Current.Request.RawUrl);
            // function 
            if (!string.IsNullOrEmpty(functionName))            
                message.AppendLine("Function : " + functionName);
            // Get the error message
            message.AppendLine("Message : " + BuildUserMsg(errorCode));
            //            
            return message.ToString();
        }
    }
}
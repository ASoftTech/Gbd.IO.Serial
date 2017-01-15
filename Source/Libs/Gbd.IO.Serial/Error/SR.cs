using System.Reflection;
using System.Resources;

namespace Gbd.IO.Serial.Error {
    /// <summary> String Constants stored within local resource. </summary>
    public enum SR {
        Arg_InvalidSerialPort,
        Arg_InvalidSerialPortExtended,
        Arg_SecurityException,

        ArgumentNull_Array,
        ArgumentOutOfRange_OffsetOut,
        ArgumentOutOfRange_WriteTimeout,
        ArgumentOutOfRange_Bounds_Lower_Upper,
        ArgumentOutOfRange_NeedPosNum,
        ArgumentNull_Buffer,
        ArgumentOutOfRange_NeedNonNegNumRequired,
        Argument_InvalidOffLen,

        Cant_be_set_when_open,
        CantSetRtsWithHandshaking,

        In_Break_State,
        InvalidNullEmptyArgument,
        IndexOutOfRange_IORaceCondition,

        IO_PortNotFound,
        IO_PortNotFoundFileName,
        IO_PathTooLong,
        IO_SharingViolation_NoFileName,
        IO_SharingViolation_File,
        IO_UnknownError,
        IO_BindHandleFailed,

        Max_Baud,
        NotSupported_UnseekableStream,

        Port_unlisted,
        Port_already_open,
        PortNameEmpty_String,
        Port_not_open,
        Port_disposed,

        UnauthorizedAccess_IODenied_Path,
        UnauthorizedAccess_IODenied_NoPathName,

        Write_timed_out,
    };

    /// <summary> Retrieve string constant from resource. </summary>
    public static class SRExtensionMethods {
        public static ResourceManager resmanager;

        /// <summary> A SR extension method that converts a enum to a resource string value. </summary>
        /// <param name="srvalue"> The srvalue to act on. </param>
        /// <param name="formatstrings"></param>
        /// <returns> srvalue as a string. </returns>
        public static string ToResValue(this SR srvalue, params object[] formatstrings) {
            if (resmanager == null) {
                var srassem = typeof (SR).GetTypeInfo().Assembly;
                resmanager = new ResourceManager(srassem.GetName().Name + ".Error.ErrorStrings", srassem);
            }
            var resvalue = resmanager.GetString(srvalue.ToString());
            if (resvalue == null)
                return null;
            if (formatstrings == null)
                return resvalue;
            resvalue = string.Format(resvalue, formatstrings);
            return resvalue;
        }
    }
}
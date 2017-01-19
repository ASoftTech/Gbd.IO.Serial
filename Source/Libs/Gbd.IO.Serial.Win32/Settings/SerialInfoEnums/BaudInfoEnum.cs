using System;

namespace Gbd.IO.Serial.Win32.Settings.SerialInfoEnums {
    /// <summary> This represents a maximum baud setting under Windows. </summary>
    [Flags]
    public enum BaudInfoEnum {
        B75 = 0x00000001,
        B110 = 0x00000002,
        B134_5 = 0x00000004,
        B150 = 0x00000008,
        B300 = 0x00000010,
        B600 = 0x00000020,
        B1200 = 0x00000040,
        B1800 = 0x00000080,
        B2400 = 0x00000100,
        B4800 = 0x00000200,
        B7200 = 0x00000400,
        B9600 = 0x00000800,
        B14400 = 0x00001000,
        B19200 = 0x00002000,
        B38400 = 0x00004000,
        B56000 = 0x00008000,
        B57600 = 0x00040000,
        B115200 = 0x00020000,
        B128000 = 0x00010000,
        Programmable = 0x10000000,
    }


    /// <summary> Extension methods for the MaxBaudEnum. </summary>
    public static class MaxBaudEnumExtensions {
        public static double ToBaudRate(this BaudInfoEnum enumValue) {
            switch (enumValue) {
                case BaudInfoEnum.B75:
                    return 75;
                case BaudInfoEnum.B110:
                    return 110;
                case BaudInfoEnum.B134_5:
                    return 134.5;
                case BaudInfoEnum.B150:
                    return 150;
                case BaudInfoEnum.B300:
                    return 300;
                case BaudInfoEnum.B600:
                    return 600;
                case BaudInfoEnum.B1200:
                    return 1200;
                case BaudInfoEnum.B1800:
                    return 1800;
                case BaudInfoEnum.B2400:
                    return 2400;
                case BaudInfoEnum.B4800:
                    return 4800;
                case BaudInfoEnum.B7200:
                    return 7200;
                case BaudInfoEnum.B9600:
                    return 9600;
                case BaudInfoEnum.B14400:
                    return 14400;
                case BaudInfoEnum.B19200:
                    return 19200;
                case BaudInfoEnum.B38400:
                    return 38400;
                case BaudInfoEnum.B56000:
                    return 56000;
                case BaudInfoEnum.B57600:
                    return 57600;
                case BaudInfoEnum.B115200:
                    return 115200;
                case BaudInfoEnum.B128000:
                    return 128000;
                case BaudInfoEnum.Programmable:
                    return double.MaxValue;
                default:
                    return 0;
            }
        }
    }
}
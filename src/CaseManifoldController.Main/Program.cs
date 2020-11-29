using System;
using System.IO.Ports;

namespace CaseManifoldController
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var position = int.Parse(Console.ReadLine());
                SetServoPosition("COM4", 9600, 5, position, 20);
            }
        }

        public static void SetServoPosition(string ComPort, int BaudRate, int ServoNumber, int ServoPos, int servoSpeed)
        {
            SerialPort port = new SerialPort(ComPort, BaudRate, Parity.None, 8, StopBits.One);
            port.Open();

            string ServoNumberHex = ServoNumber.ToString("X");
            byte ServoNumberByte = byte.Parse(ServoNumberHex, System.Globalization.NumberStyles.HexNumber);

            SetSpeed(port, ServoNumberByte, servoSpeed);
            SetPosition(port, ServoNumberByte, ServoPos);

            port.Close();
        }

        private static void SetSpeed(SerialPort port, byte ServoNumberByte, int servoSpeed)
        {
            string ServoSpeedHex = servoSpeed.ToString("X");
            byte SpeedByte = byte.Parse(ServoSpeedHex, System.Globalization.NumberStyles.HexNumber);

            var data = new byte[] {
                0x87,
                ServoNumberByte,    
                SpeedByte,        
                0x00                
            };
            port.Write(data, 0, 4);
        }

        private static void SetPosition(SerialPort port, byte ServoNumberByte, int ServoPos)
        {
            string ServoPosHex = ServoPos.ToString("X");
            byte PositionByte = byte.Parse(ServoPosHex, System.Globalization.NumberStyles.HexNumber);

            port.Write(new byte[] { 0xFF, ServoNumberByte, PositionByte }, 0, 3);
        }
    }
}
using System;
using System.IO.Ports;

namespace CaseManifoldController
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 4)
            {
                var comPort = args[0];
                int baudRate = 9600;
                int servoNumber = int.Parse(args[1]);
                int position = int.Parse(args[2]);
                int speed = int.Parse(args[3]);
                SetServoPosition(comPort, baudRate, servoNumber, position, speed);
            }
            else
            {
                Console.WriteLine("Invalid Arguments, expected 'Comport', 'servo', 'position (0-254)', 'speed (0-140)', example 'COM4 5 254 100'");
            }

            Console.ReadKey();
        }

        public static void SetServoPosition(string ComPort, int BaudRate, int ServoNumber, int ServoPos, int servoSpeed)
        {
            try
            {
                SerialPort port = new SerialPort(ComPort, BaudRate, Parity.None, 8, StopBits.One);
                try
                {
                    port.Open();

                    string ServoNumberHex = ServoNumber.ToString("X");
                    byte ServoNumberByte = byte.Parse(ServoNumberHex, System.Globalization.NumberStyles.HexNumber);

                    SetSpeed(port, ServoNumberByte, servoSpeed);
                    SetPosition(port, ServoNumberByte, ServoPos);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if(port.IsOpen)
                        port.Close();
                }
            }
            catch (Exception e)
            {
                Console.Write("Failed to send command. Error: " + e.ToString());
            }
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
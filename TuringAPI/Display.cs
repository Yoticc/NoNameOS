#define IsSafe
#define Speed

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TuringAPI;
public unsafe partial class Display : IDisplay, IDisposable
{
    public const int WIDTH = 480, HEIGHT = 320;

    static Display()
    {
        BlackBitmap = new MemoryBitmap(WIDTH, HEIGHT);

        var blackPixel = new MemoryPixel(0, 0, 0);
        for (int y = 0; y < HEIGHT; y++)
            for (int x = 0; x < WIDTH; x++)
                BlackBitmap[x, y] = blackPixel;
    }

    public static MemoryBitmap BlackBitmap;

    string deviceID;
    string pnpDeviceID;

    public bool IsConnected { get; private set; }
    public Orientations Orientation = Orientations.Landscape;
    public int Width => Orientation == Orientations.Portrait || Orientation == Orientations.ReversePortrait ? HEIGHT : WIDTH;
    public int Height => Orientation == Orientations.Portrait || Orientation == Orientations.ReversePortrait ? WIDTH : HEIGHT;

    public SerialPort? Port;
    public bool Open()
    {
        try
        {
            using var managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_SerialPort");
            var allDevices = managementObjectSearcher.Get().Cast<ManagementObject>().ToArray();
            ManagementObject deviceObject = null;
            foreach (var device in allDevices)
            {
                try
                {
                    if (((string)device["PNPDeviceID"]).Contains("USB35INCH"))
                    {
                        deviceObject = device;
                        break;
                    }
                }
                catch { }
            }

            if (deviceObject == null)
            {
                Console.WriteLine("[-] Devide not found");
                return false;
            }

            deviceID = (string)deviceObject["DeviceID"];
            pnpDeviceID = (string)deviceObject["PNPDeviceID"];

            deviceObject.Dispose();

            Port = new SerialPort(deviceID)
            {
                #if Speed
                DtrEnable = false,
                #else
                DtrEnable = true,
                #endif

                #if Speed
                RtsEnable = false,
                #else
                RtsEnable = true,
                #endif

                ReadTimeout = 3000,
                BaudRate = 333333,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
            };
            Port.Open();
            return true;
        } 
        catch
        {
            return false;
        }
    }

    public void Close()
    {
        Port?.Dispose();
    }

    public void SendCommand(Commands command) => Write(0, 0, 0, 0, 0, (byte)command);

    public void SendCommand(Commands command, byte val) => SendCommand(command, val, 0, 0, 0);

    public void SendCommand(Commands command, int x, int y, int ex, int ey)
        => Write([
            (byte)(x >> 2),
            (byte)(((x & 3) << 6) + (y >> 4)),
            (byte)(((y & 15) << 4) + (ex >> 6)),
            (byte)(((ex & 63) << 2) + (ey >> 8)),
            (byte)(ey & 255),
            (byte)command
            ]);

    static readonly object writeLocker = new object();
    public void Write(params byte[] bytes)
    {
#if IsSafe
        lock (writeLocker)
            Port?.Write(bytes, 0, bytes.Length);
#else
        Port?.Write(bytes, 0, bytes.Length);
#endif
    }

    public void Write(byte* ptr, int count)
    {
        byte[] bytes = new byte[count];
        fixed (byte* bytesPtr = bytes)
            Buffer.MemoryCopy(ptr, bytesPtr, count, count);
        Write(bytes);
        //Port?.Write(bytes, 0, bytes.Length);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Close();
    }
}
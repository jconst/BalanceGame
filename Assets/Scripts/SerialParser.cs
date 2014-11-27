using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;

public class SerialParser
{
    const int maxLinesPerBatch = 64; // process no more than this many bytes per individual processInput call
    const int baudRate = 57600;
    SerialPort _serialPort;

    string message;

    public static string guessPortName()
    {           
        var devices = System.IO.Ports.SerialPort.GetPortNames();
        
        if (devices.Length ==0) // try manual enumeration
        {
            devices = System.IO.Directory.GetFiles("/dev/");        
        }
        string dev = ""; ;          
        foreach (var d in devices)
        {               
            if (d.StartsWith("/dev/tty.usb") || d.StartsWith("/dev/ttyUSB"))
            {
                dev = d;
                Debug.Log("Guessing that arduino is device " + dev);
                break;
            }
        }       
        return dev;     
    }

    public SerialParser(string serialPortName) {
        Debug.Log(serialPortName);
        _serialPort = new SerialPort(serialPortName, baudRate);
        //_serialPort = Win32SerialPort.CreateInstance();
        
        _serialPort.DtrEnable = true; // win32 hack to try to get DataReceived event to fire
        _serialPort.RtsEnable = true; 
        _serialPort.PortName = serialPortName;
        _serialPort.BaudRate = baudRate;
        
        _serialPort.DataBits = 8;
        _serialPort.Parity = Parity.None;
        _serialPort.StopBits = StopBits.One;
        _serialPort.WriteTimeout = 1000;
        _serialPort.ReadTimeout = 100;

        _serialPort.Open();
    }

    public string Read() {
        for (int i=0; i<maxLinesPerBatch; ++i) {
            if (_serialPort.BytesToRead > 0) {
                try {
                    message = _serialPort.ReadLine();
                } catch (Exception e) {
                     // swallow read timeout exceptions
                    if (e.GetType() == typeof(TimeoutException))
                        return message;
                    else 
                        throw;
                }
            }
        }
        return message;
    }
}

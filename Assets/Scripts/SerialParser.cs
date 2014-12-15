using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;

public class SerialParser : MonoBehaviour
{
    const int maxLinesPerBatch = 2; // process no more than this many lines per individual Read() call
    const int baudRate = 57600;
    SerialPort _serialPort;

    string message = "";
    public List<Vector3> ballVelocity =
       new List<Vector3> {
        Vector3.zero,
        Vector3.zero,
        Vector3.zero
    };
    public Quaternion groundRotation = Quaternion.identity;
    public Vector3 touchpadPosition = Vector3.zero;

    public static string GuessPortName()
    {           
        var devices = System.IO.Ports.SerialPort.GetPortNames();
        
        if (devices.Length ==0) // try manual enumeration
        {
            devices = System.IO.Directory.GetFiles("/dev/");        
        }
        string dev = "";
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

    void Start() {
        string serialPortName = GuessPortName();
        if (serialPortName == null || serialPortName.Length == 0)
            return;
        _serialPort = new SerialPort(serialPortName, baudRate);
        
        _serialPort.DataBits = 8;
        _serialPort.Parity = Parity.None;
        _serialPort.StopBits = StopBits.One;
        _serialPort.WriteTimeout = 1000;
        _serialPort.ReadTimeout = 100;

        _serialPort.Open();
    }

    void FixedUpdate() {
        if (_serialPort == null || !_serialPort.IsOpen)
            return;
        Read();
        Parse();
    }

    void Read() {
        try {
            for (int i=0; i<maxLinesPerBatch; ++i) {
                if (_serialPort.BytesToRead > 0) {
                    message = _serialPort.ReadLine();
                }
            }
            if (_serialPort.BytesToRead > 0) {
                _serialPort.ReadExisting();
            }
        } catch (Exception e) {
             // swallow read timeout exceptions
            if (e.GetType() == typeof(TimeoutException))
                return;
            else 
                throw;
        }
    }

    void Parse() {
        Debug.Log(message);
        if (!message.StartsWith(":"))
            return;

        string[] tokens = message.Split(null);
        string msgType = tokens[0];
        if (msgType.StartsWith(":m")) {
            // :m0 status x z
            int mouseNum = Int32.Parse(msgType.Substring(2,1));
            int stat = Int32.Parse(tokens[1]);
            int x = Int32.Parse(tokens[2]);
            int z = Int32.Parse(tokens[3]);

            bool jumping = (stat & 1) != 0;
            ballVelocity[mouseNum] = new Vector3((float)x / 8, jumping ? 1 : 0, (float)z / 8);
        } else if (msgType.StartsWith(":euler")) {
            groundRotation = Quaternion.Euler(float.Parse(tokens[2]), float.Parse(tokens[1]), -float.Parse(tokens[3]));
        } else if (msgType.StartsWith(":touch")) {
            touchpadPosition = new Vector3(float.Parse(tokens[2]), 0f, float.Parse(tokens[1]));
        }
    }

    void OnDestroy()
    {               
        if (_serialPort != null && _serialPort.IsOpen)
            _serialPort.Close();
    }
}

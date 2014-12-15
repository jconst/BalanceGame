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
    List<SerialPort> serialPorts;

    public List<Vector3> ballVelocity =
       new List<Vector3> {
        Vector3.zero,
        Vector3.zero,
        Vector3.zero
    };
    public Quaternion groundRotation = Quaternion.identity;
    public Vector3 touchpadPosition = Vector3.zero;

    public static List<string> GuessPortNames()
    {           
        var devices = System.IO.Ports.SerialPort.GetPortNames();
        
        if (devices.Length ==0) // try manual enumeration
        {
            devices = System.IO.Directory.GetFiles("/dev/");        
        }
        return devices.Where(d => d.StartsWith("/dev/tty.usb") || d.StartsWith("/dev/ttyUSB"))
                      .ToList();
    }

    void Start() {
        serialPorts = GuessPortNames()
        .Where(name => name != null && name.Length > 0)
        .Select(name => {
            SerialPort port = new SerialPort(name, baudRate);
            
            port.DataBits = 8;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.WriteTimeout = 1000;
            port.ReadTimeout = 100;

            port.Open();
            return port;
        })
        .ToList();
    }

    void FixedUpdate() {
        serialPorts
        .Where(port => port != null && port.IsOpen)
        .ToList()
        .ForEach(port => Parse(Read(port)));
    }

    string Read(SerialPort port) {
        string message = "";
        try {
            for (int i=0; i<maxLinesPerBatch; ++i) {
                if (port.BytesToRead > 0) {
                    message = port.ReadLine();
                }
            }
            if (port.BytesToRead > 0) {
                port.ReadExisting();
            }
        } catch (Exception e) {
             // swallow read timeout exceptions
            if (e.GetType() == typeof(TimeoutException))
                return message;
            else 
                throw;
        }
        return message;
    }

    void Parse(string message) {
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
        serialPorts.ForEach(port => {
            if (port != null && port.IsOpen)
                port.Close();
        });
    }
}

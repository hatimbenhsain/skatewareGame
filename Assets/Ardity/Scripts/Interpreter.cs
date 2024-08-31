using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class InterpretArduino : MonoBehaviour
{

    public SerialController serialController; 

    void OnMessageArrived(string msg)
    {
        Debug.Log(msg);
        //tmsg is the information you are bringing into Unity. It comes in as a string that you will parce HERE
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
        //you can add this code as is, it just makes sure to let you know if it is connected
    }
}
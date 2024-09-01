using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class InterpretArduino : MonoBehaviour
{

    public SerialController serialController;
    
    private Vector2 currentInput = Vector2.zero;
    private bool kickInput = false;

    void OnMessageArrived(string msg)
    {
        string[] values = msg.Split(",");
        float x = int.Parse(values[0]);
        float y = int.Parse(values[1]);
        x = -2 * ((x / 1023) - 0.5f);
        y = 2 * ((y / 1023) - 0.5f);
        currentInput = new Vector2(x, y);

        int kickInt = int.Parse(values[2]);
        kickInput = true ? kickInt == 1 : false;
        //tmsg is the information you are bringing into Unity. It comes in as a string that you will parce HERE
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
        //you can add this code as is, it just makes sure to let you know if it is connected
    }

    public Vector2 GetCurrentInput()
    {
        return currentInput;
    }

    public bool GetKickInput()
    {
        return kickInput;
    }
}
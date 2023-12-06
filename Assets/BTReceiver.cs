using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using System;
using System.Text;

public class BTReceiver : MonoBehaviour
{
    // Use this for initialization
    BluetoothHelper bluetoothHelper;
    public GameObject hand_joint;
    public string deviceName = "TENNIS";
    public Text text;
    string received_message;
    // Start is called before the first frame update
    void Start()
    {
        deviceName = "TENNIS"; //bluetooth should be turned ON;
        try
        {
            bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data

            bluetoothHelper.setTerminatorBasedStream("\n"); //delimits received messages based on \n char
            if(bluetoothHelper.isDevicePaired())
            {
                while(!bluetoothHelper.isConnected())
                {
                    bluetoothHelper.Connect();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    void OnMessageReceived(BluetoothHelper helper)
    {
        //StartCoroutine(blinkSphere());
        received_message = helper.Read();
        Debug.Log(received_message);
        var split = received_message.Split('\t');
        float qw = float.Parse(split[0]);
        float qx = float.Parse(split[1]);
        float qy = float.Parse(split[2]);
        float qz = float.Parse(split[2]);
        Quaternion sensorRotation = new Quaternion(-qy, -qz, qx, qw);
        Quaternion adjustedRotation = sensorRotation * Quaternion.Inverse(offsetRotation);
        transform.rotation = adjustedRotation;
        text.text = transform.rotation.eulerAngles.ToString();
        // Debug.Log(received_message);
    }
    Quaternion offsetRotation = Quaternion.identity;
    public void Reset()
    {
        offsetRotation = transform.rotation;
    }
    void OnConnected(BluetoothHelper helper)
    {
        try
        {
            helper.StartListening();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    void OnConnectionFailed(BluetoothHelper helper)
    {
        if (bluetoothHelper.isDevicePaired())
        {
            while (!bluetoothHelper.isConnected())
            {
                bluetoothHelper.Connect();
            }
        }
    }

    public void MakeVibration()
    {
        if(bluetoothHelper.isConnected())
        {
            bluetoothHelper.SendData("1");
        }
    }
    void OnDestroy()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }
    // Update is called once per frame
    void Update()
    {
        if(hand_joint != null)
        {
            transform.position = hand_joint.transform.position;
        }
    }
}

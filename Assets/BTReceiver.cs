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
    public string deviceName = "TENNIS";

    public GameObject Racket;
    public float distanceFromCamera = 2.0f;
    string received_message;
    public Text text;
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
            if (Racket != null)
            {
                // Calculate the position in front of the camera
                Vector3 centerPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;

                // Place the object at the calculated position
                Racket.transform.position = centerPosition;
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
        text.text = received_message;
        var split = received_message.Split('\t');
        Racket.transform.eulerAngles = new Vector3(float.Parse(split[2]), float.Parse(split[1]), float.Parse(split[0]));
        // Debug.Log(received_message);
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
        
    }
}

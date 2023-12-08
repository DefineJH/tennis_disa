using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using System;
using System.Text;
enum EBTState
{
    unpaired,
    paired,
    connected,
    disconnected
}
public class BTReceiver : MonoBehaviour
{
    // Use this for initialization
    BluetoothHelper bluetoothHelper;
    EBTState btState = EBTState.unpaired;
    public GameObject hand_joint;
    public string deviceName = "TENNIS";
    string received_message;
    public Button connectBtn;
    public Text guideText;
    public Button offsetBtn;
    // Start is called before the first frame update
    void Start()
    {
        if(offsetBtn != null)
        {
            offsetBtn.gameObject.SetActive(false);
        }
        deviceName = "TENNIS"; //bluetooth should be turned ON;
        try
        {
            bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data
            bluetoothHelper.setTerminatorBasedStream("\n"); //delimits received messages based on \n char
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    public void Connect()
    {
        if(bluetoothHelper.isDevicePaired())
        {
            if(!bluetoothHelper.isConnected())
            {
                StopAllCoroutines();
                StartCoroutine("connect_internal");
            }
        }
    }
    IEnumerator connect_internal()
    {
        while(!bluetoothHelper.isConnected())
        {
            bluetoothHelper.Connect();
            yield return null;
        }
        btState = EBTState.connected;
        yield break;
    }
    void OnMessageReceived(BluetoothHelper helper)
    {
        try
        {
            received_message = helper.Read();
            Debug.Log(received_message);
            var split = received_message.Split('\t');
            float qw = float.Parse(split[0]);
            float qx = float.Parse(split[1]);
            float qy = float.Parse(split[2]);
            float qz = float.Parse(split[3]);
            Quaternion sensorRotation = new Quaternion(qy, qz, qx, qw);
            Quaternion adjustedRotation = sensorRotation * Quaternion.Inverse(offsetRotation);
            transform.rotation = adjustedRotation;
        }catch(Exception e)
        {
            Debug.LogError(e.ToString());
        }
        //if(rotText != null)
        //{
        //    rotText.text = qx + " " + qy + " " + qz + " " + qw;
        //}
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
            Debug.Log("connected, start listen");
            helper.StartListening();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    void OnConnectionFailed(BluetoothHelper helper)
    {
    }

    public void MakeVibration()
    {
        if(btState == EBTState.connected)
        {
            if(bluetoothHelper.isConnected())
            {
                bluetoothHelper.SendData("1");
            }
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
        if(bluetoothHelper.isDevicePaired())
        {
            btState = EBTState.paired;
            if(bluetoothHelper.isConnected())
            {
                btState = EBTState.connected;
            }
            else
            {
                btState = EBTState.disconnected;
            }
        }
        else
        {
            btState = EBTState.unpaired;
        }
        if(bluetoothHelper.isConnected() )
        {
            connectBtn.gameObject.SetActive(false);
        }
        else
        {
            connectBtn.gameObject.SetActive(true);
        }
        if(guideText != null)
        {
            switch (btState)
            {
                case EBTState.unpaired:
                    guideText.text = "페어링 되지 않았습니다.";
                    break;
                case EBTState.paired:
                    guideText.text = "페어링 되어 있습니다";
                    break;
                case EBTState.connected:
                    guideText.text = "연결되었습니다.";
                    break;
                case EBTState.disconnected:
                    guideText.text = "연결 해제되었습니다.";
                    break;
            }

        }
    }
    public void OnGUI()
    {
        if(bluetoothHelper.isConnected())
        {
            bluetoothHelper.DrawGUI();
        }
    }
}

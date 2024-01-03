using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
[DefaultExecutionOrder(-1)]
public class SensorManager : MonoBehaviour
{
    public static SensorManager instance { get; private set; }

    private string DeviceName = "FITTAG";// "ESP32 FITNESS LEFT";    
    private string ServiceUUID = "c560";
    private string Characteristic = "c561";

    float qx, qy, qz, qw;

    public enum States
    {
        None,
        Scan,
        Connect,
        RequestMTU,
        Subscribe,
        Unsubscribe,
        Disconnect,
        Error
    }

    private float _timeout = 0f;
    private States _state = States.None;
    private string _deviceAddress;
    private bool _anotherSensorDiscovered = false;

    private Quaternion inverseQt;
    private Quaternion rawQt;
    public Transform cube;
    public bool _connected = false;

    #region Event
    public delegate void ConnectState(States reciever);
    public event ConnectState connectState;

    //private delegate void RegEvent(Action<bool,string> _callback);
    //private event RegEvent regEvent;
    #endregion

    public float scanningTimeOut = 5f;
    float lastTime = 0;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else instance = this;
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;

        DontDestroyOnLoad(this.gameObject);
    }


    void Reset()
    {
        _connected = false;
        _anotherSensorDiscovered = false;
        _timeout = 0f;
        _state = States.None;
        _deviceAddress = null;
    }

    void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    public void NonState()
    {
        _state = States.None;
        _timeout = 0.5f;
        BluetoothLEHardwareInterface.StopScan();
    }

    public void UnsubscribeState()
    {
        _state = States.Unsubscribe;
        _timeout = 0.5f;
    }

    public void StartProcess()
    {
        if (!PlayerPrefs.HasKey("AMC_DeviceAddr"))
        {
            PlayerPrefs.SetString("AMC_DeviceAddr", "");
        }

        if (_anotherSensorDiscovered)
        {
            PlayerPrefs.SetString("AMC_DeviceAddr", _deviceAddress);
        }

        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            SetState(States.Scan, 0.5f);
        }, (error) =>
        {
            // 에러가 나면
            SetState(States.None, 0.5f);

            BluetoothLEHardwareInterface.Log("Error: " + error);

        });
    }

    public void Connection()
    {
        SetState(States.Connect, 0.5f);
    }

    public void Register(Button _regbtn ,Action<bool, bool, string> _callback)
    {
        // 재등록버튼 연결되기 전까지 비활성화 
        _regbtn.enabled = false;
        _anotherSensorDiscovered = false;
        // 처음센서 등록
        if (PlayerPrefs.GetString("AMC_DeviceAddr") == "")
        {
            StartCoroutine(_StartProcess(_callback));
        }
        else
        {
            // 기존센서 등록
            StartCoroutine(_UnsubscribeState(_callback));
        }        
    }

    IEnumerator _UnsubscribeState(Action<bool, bool, string> _callback)
    {
        Debug.Log("in _UnsubscribeState");
        // 1. 기존센서의 전원이 켜져있나? 꺼져있나?
        UnsubscribeState();
        // 기존센서의 전원이 꺼져있다면 Error 로 빠짐
        yield return new WaitUntil(() => _connected == false || _state == States.Error);

        if (_connected == false)
        {
            StartCoroutine(_StartProcess(_callback));
        }
        else if(_state == States.Error)
        {
            Debug.Log("_UnsubscribeState failed infor > Power Off .. ect");
            _state = States.None;
            _connected = false;
            _callback(_connected, _anotherSensorDiscovered, "핏태그 연결에 실패했습니다.\n전원을 켜주세요!");
            yield return null;
        }
    }

    IEnumerator _StartProcess(Action<bool, bool, string> _callback)
    {
        Debug.Log("in _StartProcess");
        StartProcess();

        yield return new WaitUntil(() => _state == States.Scan);

        // 현재상태가 연결되거나(_connected == true , None(Subscribe) or None(Scan Timeout) 이거나
        yield return new WaitUntil(() => _connected == true || _state == States.None);

        if (_connected)
        {
            PlayerPrefs.SetString("AMC_DeviceAddr", _deviceAddress);
            _callback(_connected, _anotherSensorDiscovered,"핏태그가 재등록 되었습니다!");
        }
        else if(!_connected && _state == States.None && _anotherSensorDiscovered)
        {
            // 다른센서 발견 > _anotherSensorDiscovered == true
            _callback(_connected, _anotherSensorDiscovered,"새로운 핏태그 센서를 발견했습니다.\n연결 하시겠습니까?");
        }
        else if (!_connected && _state == States.None && !_anotherSensorDiscovered)
        {
            _callback(_connected, _anotherSensorDiscovered,"핏태그 연결에 실패했습니다.\n신호가 약하거나, 전원이 꺼져있습니다.\n다시 시도해주세요!");
        }
    }

    // Use this for initialization
    void Start()
    {
        inverseQt = Quaternion.identity;
        StartProcess();
    }

    void Update()
    {
        //Debug.Log($"_state is {_state}");
        //Debug.Log($"_connected is {_connected}");
        if (_state == States.Scan)
        {
            lastTime += Time.deltaTime;
            //Debug.Log("lastTime : " + lastTime);
            if (scanningTimeOut < lastTime)
            {
                NonState();
                lastTime = 0;
                connectState?.Invoke(States.None);
            }
        }
        else
        {
            lastTime = 0f;
        }

        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;
                switch (_state)
                {
                    case States.None:
                        break;
                    case States.Scan:
                        connectState?.Invoke(States.Scan);
                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                        {
                            if (name.Contains(DeviceName))
                            {
                                if (PlayerPrefs.GetString("AMC_DeviceAddr") == "")
                                {
                                    PlayerPrefs.SetString("AMC_DeviceAddr", address);

                                    // it is always a good idea to stop scanning while you connect to a device
                                    // and get things set up
                                    BluetoothLEHardwareInterface.StopScan();

                                    // add it to the list and set to connect to it
                                    _deviceAddress = address;

                                    // 스캔해서 찾음 이벤트로 뿌려줌
                                    connectState?.Invoke(States.Connect);
                                    SetState(States.Connect, 0.5f);
                                }
                                else if (PlayerPrefs.GetString("AMC_DeviceAddr") == address) // 기존에 센서가 등록되어있다면 걸림
                                {
                                    // it is always a good idea to stop scanning while you connect to a device
                                    // and get things set up
                                    BluetoothLEHardwareInterface.StopScan();

                                    // 기존센서 발견
                                    _anotherSensorDiscovered = false;

                                    // add it to the list and set to connect to it
                                    _deviceAddress = address;

                                    // 스캔해서 찾음 이벤트로 뿌려줌
                                    connectState?.Invoke(States.Connect);
                                    SetState(States.Connect, 0.5f);
                                }
                                else if (PlayerPrefs.GetString("AMC_DeviceAddr") != address)
                                {
                                    // 다른센서 발견
                                    _anotherSensorDiscovered = true;
                                    _deviceAddress = address;
                                }
                            }
                        }, null, false, false);
                        break;
                    case States.Connect:

                        // note that the first parameter is the address, not the name. I have not fixed this because
                        // of backwards compatiblity.
                        // also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
                        // the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
                        // large enough that it will be finished enumerating before you try to subscribe or do any other operations.
                        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null,
                            (address, serviceUUID, characteristicUUID) =>
                            {
                                if (IsEqual(serviceUUID, FullUUID(ServiceUUID)))
                                {
                                    // if we have found the characteristic that we are waiting for
                                    // set the state. make sure there is enough timeout that if the
                                    // device is still enumerating other characteristics it finishes
                                    // before we try to subscribe
                                    if (IsEqual(characteristicUUID, FullUUID(Characteristic)))
                                    {
                                        _connected = true;
                                        SetState(States.Subscribe, 1f);
                                    }
                                }
                            }, (disconnectedAddress) =>
                            {
                                BluetoothLEHardwareInterface.Log("Device disconnected in States.Connect : " + disconnectedAddress);
                                _connected = false;
                                connectState?.Invoke(States.None);
                                SetState(States.None, 0.5f);
                            });
                        break;

                    case States.Subscribe:

                        connectState?.Invoke(States.Subscribe);

                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress,
                        FullUUID(ServiceUUID),
                        FullUUID(Characteristic), null,
                        (address, characteristicUUID, bytes) =>
                        {
                            if (bytes.Length == 16)
                            {
                                qx = BitConverter.ToSingle(bytes, 0);
                                qy = BitConverter.ToSingle(bytes, 4);
                                qz = BitConverter.ToSingle(bytes, 8);
                                qw = BitConverter.ToSingle(bytes, 12);


                                rawQt = new Quaternion(qx, qy, -qz, qw);
                                cube.rotation = rawQt * inverseQt;
                            }
                        });

                        // set to the none state and the user can start sending and receiving data
                        _state = States.None;
                        break;

                    case States.Unsubscribe:
                        //BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress,
                        //    FullUUID(ServiceUUID),
                        //    FullUUID(Characteristic),
                        //    null);
                        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress,
                        FullUUID(ServiceUUID),
                        FullUUID(Characteristic),
                        (address) => {
                            SetState(States.Disconnect, 0.5f);
                        }
                        , (error) => {
                            SetState(States.Error, 0.5f);
                        });

                        break;

                    case States.Disconnect:
                        if (_connected)
                        {
                            BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                            {
                                BluetoothLEHardwareInterface.DeInitialize(() =>
                                {
                                    _connected = false;
                                    connectState?.Invoke(States.None);
                                    SetState(States.None, 0.5f);
                                });
                            });
                        }
                        else
                        {
                            BluetoothLEHardwareInterface.DeInitialize(() => { _state = States.None; });
                            connectState?.Invoke(States.None);
                        }
                        break;
                    case States.Error:                        
                        break;
                }
            }
        }
    }


    string FullUUID(string uuid)
    {
        return "389b" + uuid + "-5aab-4ee3-9f3d-d847c3b1a4e9";
        //return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

    public void resetRotation()
    {
        inverseQt = Quaternion.Inverse(rawQt);
    }

}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConnectAlarm : MonoBehaviour
{
    public Image fitTagStateImg;
    public Sprite fitTagRedImg;
    public Sprite fitTagGreenImg;
    public Sprite fitTagGrayImg;

    [Header("Alarm")]
    private Image blinkImg;
    private Color alphaDump = new Color(1, 1, 1, 1);
    public float fadeTime = 1.5f;
    Tween twBlink;

    void Awake()
    {
        blinkImg = GetComponent<Image>();
    }

    private void OnEnable()
    {
        SensorManager.instance.connectState += ConnectState;

        if (SensorManager.instance._connected)
        {
            fitTagStateImg.sprite = fitTagGreenImg;
            blinkImg.enabled = false;
        }
        else
        {
            fitTagStateImg.sprite = fitTagRedImg;
            blinkImg.enabled = true;
            Blink().Play().SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnDisable()
    {
        SensorManager.instance.connectState -= ConnectState;
    }
    private void ConnectState(SensorManager.States reciever)
    {
        Debug.Log($"ConnectState is {reciever} in ConnectAlarm");

        if (SensorManager.States.None == reciever)
        {
            // 스캔중에 연결에 실패함
            fitTagStateImg.sprite = fitTagRedImg;
            Blink().Play().SetLoops(-1, LoopType.Yoyo);
        }
        else if (SensorManager.States.Scan == reciever || SensorManager.States.Connect == reciever)
        {
            // 연결중
            fitTagStateImg.sprite = fitTagGrayImg;
        }
        else if (SensorManager.States.Subscribe == reciever)
        {
            fitTagStateImg.sprite = fitTagGreenImg;
            Debug.Log($"ConnectAlarm is {reciever}");
            Blink().Pause();
            blinkImg.enabled = false;
        }
    }

    private Tween Blink()
    {
        return twBlink = DOTween.Sequence()
            .SetAutoKill(false)
            .OnStart(() =>
            {
                Debug.Log("OnStart");
                blinkImg.enabled = true;
                blinkImg.color = alphaDump;
            })
            .Append(blinkImg.DOFade(0f, fadeTime))
            .Append(blinkImg.DOFade(1f, fadeTime))
            .OnComplete(() =>
            {
                Debug.Log("OnComplete");
            });
    }
}

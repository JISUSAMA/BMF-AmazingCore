using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    [Header("Alarm")]
    private TMP_Text touchText;    
    public float fadeTime = 1.5f;
    Tween twBlink;

    private void Awake()
    {
        touchText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        Debug.Log("OnEn");
        BlinkText().Play().SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        Debug.Log("OnDis");
        BlinkText().Pause();        
    }

    private Tween BlinkText()
    {
        return twBlink = DOTween.Sequence()
            .SetAutoKill(false)
            .OnStart(() =>
            {
                Debug.Log("OnStart");                
            })
            .Append(touchText.DOFade(0f, fadeTime))
            .Append(touchText.DOFade(1f, fadeTime))
            .OnComplete(() =>
            {
                Debug.Log("OnComplete");                
            });
    }
}

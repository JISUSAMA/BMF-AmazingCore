using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;


public class LocaleSelectors : MonoBehaviour
{
    private bool active = false;
    public void ChangeLocale(int localID)
    {
        if (active == true) return;
        StartCoroutine(SetLocal(localID));
    }
    IEnumerator SetLocal(int _localID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localID];
        active = false;
    }


    public void SetResizeIMG(Image _img)
    {
        //kr 일 경우
        float width =_img.GetComponent<Sprite>().bounds.size.x;
        float height =_img.GetComponent<Sprite>().bounds.size.y;

        RectTransform rect = (RectTransform)_img.transform;
        rect.sizeDelta = new Vector2(width, height);
        Debug.Log(width);
        Debug.Log(height);
    }

}

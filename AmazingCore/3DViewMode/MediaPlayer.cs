using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaPlayer : MonoBehaviour
{
    enum OnState
    {
        PLAYING,
        PAUSE,
        STOP
    }

    public bool ischaracterSample;

    [Header("CharacterSample")]
    public ExerciseDataSet[] storedCharacaterDataSet;
    [Header("Storage")]
    public ExerciseDataSet[] storedDataSet;
    public ExerciseDataSet storedData;
    [Space(20)]
    [Header("Current Playing")]
    public List<GameObject> modeings;
    public GameObject OnLoad;
    [Space(20)]
    [Header("Play Option")]
    public float speed = 1;
    public float intervalTime = 0.2f;    
    private int modeings_idx = 1;

    //public delegate void StartTouchEvent(Vector2 position, float time);
    //public event StartTouchEvent OnStartTouch;
    //public delegate void EndTouchEvent(Vector2 position, float time);
    //public event EndTouchEvent OnEndTouch;

    //void OnDisable()
    //{

    //}
    //void OnEnable()
    //{

    //}

    void Start()
    {
        Initialize();        
    }

    private void Initialize()
    {
        if (ischaracterSample)
        {
            storedData = storedCharacaterDataSet[(int)GameManager.instance.viewModeList];
        }
        else
        {
            storedData = storedDataSet[(int)GameManager.instance.viewModeList];
        }
        ModelingSettingOnScene();
    }
    //void Update()
    //{
    //    #region Touch
    //    // 오브젝트 회전/줌인/줌아웃
    //    if (true)
    //    {

    //    }

    //    #endregion

    //    #region Timebar
    //    // 타임바
    //    if (true)
    //    {

    //    }

    //    #endregion
    //}

    public void ModelingSettingOnScene()
    {        
        Debug.Log("Setting");

        foreach (var obj in storedData.objPerFrame)
        {
            GameObject go = Instantiate(obj, OnLoad.transform);
            if (ischaracterSample)
            {
                go.transform.position = new Vector3(1.16f, -1.164f, 0f);                
                go.transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            else
            {
                go.transform.position = new Vector3(-2.57f, -1.17f, -4.223f);
                go.transform.localEulerAngles = new Vector3(0, 0, 90);
            }
            go.SetActive(false);
            modeings.Add(go);
        }
        // 처음 프레임
        modeings[modeings_idx - 1].SetActive(true);
    }

    public void Empty()
    {
        modeings.Clear();

        for (var i = OnLoad.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(OnLoad.transform.GetChild(i).gameObject);
        }
    }

    public void OnStart()
    {
        Debug.Log("Start");
        storedData.onState = 0;
        StartCoroutine(_OnStart());
    }

    IEnumerator _OnStart()
    {
        WaitForSeconds frame = new WaitForSeconds(intervalTime * speed);

        while (storedData.onState != (ExerciseDataSet.OnState)OnState.STOP)
        {
            yield return frame;

            if (storedData.onState == (ExerciseDataSet.OnState)OnState.PAUSE) { continue; }

            modeings[modeings_idx - 1].SetActive(false);
            modeings[modeings_idx].SetActive(true);
            modeings_idx++;

            if (storedData.objPerFrame.Count == modeings_idx)
            {
                storedData.onState = (ExerciseDataSet.OnState)OnState.STOP;
                modeings_idx = 1;
            }

            yield return null;
        }
    }

    public void OnStop()
    {
        Debug.Log("Stop");
        modeings_idx = 1;
        storedData.onState = (ExerciseDataSet.OnState)OnState.STOP;
        StopAllCoroutines();
    }

    public void OnPause()
    {
        if (storedData.onState == (ExerciseDataSet.OnState)OnState.PLAYING)
        {
            storedData.onState = (ExerciseDataSet.OnState)OnState.PAUSE;
        }
        else if (storedData.onState == (ExerciseDataSet.OnState)OnState.PAUSE)
        {
            storedData.onState = (ExerciseDataSet.OnState)OnState.PLAYING;
        }        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandle : MonoBehaviour
{
    public static VideoHandle instance { get; private set; }
    [Header("Taining Player Video")]
    public RenderTexture Tutorial_VideoRawImage;
    public RenderTexture Training_VideoRawImage;
   
    public VideoPlayer Tutorial_VideoPlayer;
    public VideoPlayer Training_VideoPlayer;
    public VideoPlayer Finish_VideoPlayer;

    //public Slider slider;

    bool isDone;    //비디오플레이어 속성


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    public bool IsPlaying_tutorial
    {
        get { return Tutorial_VideoPlayer.isPlaying; }
    }

    public bool IsLooping_tutorial
    {
        get { return Tutorial_VideoPlayer.isLooping; }
    }

    public bool IsPrepared_tutorial
    {
        get { return Tutorial_VideoPlayer.isPrepared; }
    }

    public bool IsDone
    {
        get { return isDone; }
    }

    public double Times_tutorial
    {
        get { return Tutorial_VideoPlayer.time; }
    }

    public ulong Duration_tutorial
    {
        get { return (ulong)(Tutorial_VideoPlayer.frameCount / Tutorial_VideoPlayer.frameRate); }
    }

    public bool IsPlaying_training
    {
        get { return Training_VideoPlayer.isPlaying; }
    }

    public bool IsLooping_training
    {
        get { return Training_VideoPlayer.isLooping; }
    }

    public bool IsPrepared_training
    {
        get { return Training_VideoPlayer.isPrepared; }
    }

    public bool IsDone_training
    {
        get { return isDone; }
    }

    public double Times_training
    {
        get { return Training_VideoPlayer.time; }
    }

    public ulong Duration_training
    {
        get { return (ulong)(Training_VideoPlayer.frameCount / Training_VideoPlayer.frameRate); }
    }


    private void OnEnable()
    {
        Tutorial_VideoPlayer.errorReceived += errorReceived;
        Tutorial_VideoPlayer.frameReady += frameReady;
        Tutorial_VideoPlayer.loopPointReached += loopPointReached;
        Tutorial_VideoPlayer.prepareCompleted += prepareCompleted;
        Tutorial_VideoPlayer.seekCompleted += seekCompleted;
        Tutorial_VideoPlayer.started += started;

        Training_VideoPlayer.errorReceived += errorReceived;
        Training_VideoPlayer.frameReady += frameReady;
        Training_VideoPlayer.loopPointReached += loopPointReached;
        Training_VideoPlayer.prepareCompleted += prepareCompleted;
        Training_VideoPlayer.seekCompleted += seekCompleted;
        Training_VideoPlayer.started += started;
    }

    private void OnDisable()
    {
        Tutorial_VideoPlayer.errorReceived -= errorReceived;
        Tutorial_VideoPlayer.frameReady -= frameReady;
        Tutorial_VideoPlayer.loopPointReached -= loopPointReached;
        Tutorial_VideoPlayer.prepareCompleted -= prepareCompleted;
        Tutorial_VideoPlayer.seekCompleted -= seekCompleted;
        Tutorial_VideoPlayer.started -= started;

        Training_VideoPlayer.errorReceived -= errorReceived;
        Training_VideoPlayer.frameReady -= frameReady;
        Training_VideoPlayer.loopPointReached -= loopPointReached;
        Training_VideoPlayer.prepareCompleted -= prepareCompleted;
        Training_VideoPlayer.seekCompleted -= seekCompleted;
        Training_VideoPlayer.started -= started;
    }

    void errorReceived(VideoPlayer v, string msg)
    {
        Debug.Log("video player error : " + msg);
    }

    void frameReady(VideoPlayer v, long frame)
    {

    }

    void loopPointReached(VideoPlayer v)
    {
        //Debug.Log("video player loop point reached");
        isDone = true;
    }

    void prepareCompleted(VideoPlayer v)
    {
        //Debug.Log("video player loop point preparing");
        isDone = false;
    }

    void seekCompleted(VideoPlayer v)
    {
        //Debug.Log("video player loop point seking");
        isDone = false;
    }

    void started(VideoPlayer v)
    {
        //Debug.Log("video player loop point started");
    }
  //  public void LoadVideo(string _folderName, int _videoIndex)
  //  {
  //      string temp = "";
  //      temp = Application.persistentDataPath + "/Video/" + _folderName + "/" + _videoIndex.ToString() + ".mp4";
  //
  //      if (mVideoPlayer.url.Equals(temp))
  //          return;
  //
  //      mVideoPlayer.url = temp;
  //      mVideoPlayer.Play();
  //
  //
  //      Debug.Log("can set direct audio volume: " + mVideoPlayer.canSetDirectAudioVolume);
  //  }
  //
  //  public void LoadNullVideo()
  //  {
  //      mVideoPlayer.clip = null;
  //  }
  //
    public void Tutorial_PlayVideo()
    {
        Tutorial_VideoRawImage.Release();
        if (!IsPrepared_tutorial)
            return;
        Tutorial_VideoPlayer.Play();
        Time.timeScale = 1; //재생
    }
   
    public void Tutorial_PauseVideo()
    {
        if (!IsPlaying_tutorial)
            return;
        Tutorial_VideoPlayer.Pause();
        Time.timeScale = 0; //일시정지
    }

    public void Tutorial_StopVideo()
    {
        if (!IsPlaying_tutorial)
            return;
        Tutorial_VideoPlayer.Stop();
    }

    
    //Training
    public void Training_PlayVideo()
    {
        Training_VideoRawImage.Release();
        if (!IsPrepared_training)
            return;
        Training_VideoPlayer.Play();
        Time.timeScale = 1; //재생
    }
    public void Training_PauseVideo()
    {
        if (!IsPlaying_training)
            return;
        Training_VideoPlayer.Pause();
        Time.timeScale = 0; //일시정지
    }

    public void Training_StopVideo()
    {
        if (!IsPlaying_training)
            return;
        Training_VideoPlayer.Stop();
    }
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }

    //public void Training_RestartVideo()
    //{
    //    if (!IsPrepared)
    //        return;
    //    Training_PauseVideo();
    //    Seek(0);
    //}
    //public void Seek(float nTime)
    //{
    //    if (!mVideoPlayer.canSetTime) return;
    //    if (!IsPrepared) return;
    //    nTime = Mathf.Clamp(nTime, 0, 1);
    //    mVideoPlayer.time = nTime * Duration;
    //
    //    //Debug.Log(mVideoPlayer.time  +"  "+ Duration);
    //}

    //public void LoopViedo(bool toggle)
    //{
    //    if (!IsPrepared)
    //        return;
    //    mVideoPlayer.isLooping = toggle;
    //}
    //
    //  public void Seek(float nTime)
    //  {
    //      if (!mVideoPlayer.canSetTime) return;
    //      if (!IsPrepared) return;
    //      nTime = Mathf.Clamp(nTime, 0, 1);
    //      mVideoPlayer.time = nTime * Duration;
    //  
    //      //Debug.Log(mVideoPlayer.time  +"  "+ Duration);
    //  }
    //  
    //public void IncrementPlaybackSpeed()
    //{
    //    if (!mVideoPlayer.canSetPlaybackSpeed) return;
    //    mVideoPlayer.playbackSpeed += 1;
    //    mVideoPlayer.playbackSpeed = Mathf.Clamp(mVideoPlayer.playbackSpeed, 0, 10);
    //}
    //
    //public void DecrementPlaybackSpeed()
    //{
    //    if (!mVideoPlayer.canSetPlaybackSpeed) return;
    //    mVideoPlayer.playbackSpeed += 1;
    //    mVideoPlayer.playbackSpeed = Mathf.Clamp(mVideoPlayer.playbackSpeed, 0, 10);
    //}
    //
    //
    //public void ExcerciseTime(Text _timer)
    //{
    //    float seconds = (float)mVideoPlayer.time;
    //
    //    TimeSpan timespan = TimeSpan.FromSeconds(seconds);
    //    string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
    //    //Debug.Log(" timer  " + timer);
    //    _timer.text = timer;
    //}


}


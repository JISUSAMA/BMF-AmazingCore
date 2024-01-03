using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoTest : MonoBehaviour
{
    public Button RestBtn,Next;

    private void Awake()
    {
        RestBtn.onClick.AddListener(() => OnClick_ResetBtn());
    }
    private void Start()
    {
      //  VideoHandle.instance.Tutorial_PlayVideo();
    }
    public void VideoStart()
    {
        VideoHandle.instance.Tutorial_PlayVideo();
    }
    public void OnClick_ResetBtn()
    {
        ClearOutRenderTexture(VideoHandle.instance.Tutorial_VideoRawImage);
    }
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        renderTexture.Release();
    }
}

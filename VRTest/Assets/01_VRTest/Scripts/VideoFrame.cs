using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoFrame : MonoBehaviour
{
    VideoPlayer videoPlayer = default;

    // Start is called before the first frame update
    void Start()
    {
        //! 현재 오브젝트의 비디오 플레이어 컴포넌트 정보를 가지고 온다
        videoPlayer = GetComponent<VideoPlayer>();
        //! 자동 재생되는 것을 막는다
        videoPlayer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //! S 를 누르면 정지
        if (Input.GetKeyDown(KeyCode.S))
        {
            videoPlayer.Stop();
        }

        //! 스페이스 바를 눌렀을 때 재생 또는 일시정지
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            //! 현재 비디오 플레이어가 플레이 상태인지 확인한다
            if (videoPlayer.isPlaying == true) 
            {
                videoPlayer.Pause();
            }
            else 
            {
                videoPlayer.Play();
            }
        }
    }

    //! GazePointer 에서 영상 재생을 컨트롤 하기 위한 함수
    public void CheckVideoFrame(bool checker) 
    {
        if (checker)
        {
            if (videoPlayer.isPlaying == false)
            {
                videoPlayer.Play();
            }
        }
        else
        {
            videoPlayer.Stop();
        }
    }
}

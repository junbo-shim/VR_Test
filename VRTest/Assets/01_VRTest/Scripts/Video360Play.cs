using Oculus.Voice.Bindings.Android;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class Video360Play : MonoBehaviour
{
    //! 비디오 플레이어 컴포넌트
    VideoPlayer videoPlayer = default;
    //! 재생해야 할 VR 360 영상을 위한 설정
    public VideoClip[] vcList = default;
    int currentClipIdxNum = default;

    // Start is called before the first frame update
    void Start()
    {
        //! 비디오 플레이어 컴포넌트의 정보를 받아온다
        videoPlayer = GetComponent<VideoPlayer>();
        currentClipIdxNum = 0;
        videoPlayer.clip = vcList[currentClipIdxNum];
        videoPlayer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //! 컴퓨터에서 영상을 변경하기 위한 기능
        if (Input.GetKeyDown(KeyCode.LeftBracket)) 
        {
            SwapVideoClip(false);
            //videoPlayer.clip = vcList[0];
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) 
        {
            SwapVideoClip(true);
            //videoPlayer.clip = vcList[1];
        }
    }

    /**
     * 인터랙션을 위해서 함수를 퍼블릭으로 선언
     * @brief배열의 인덱스 번호를 기준으로 영상을 교체, 재생하기 위한 함수
     * @param isNext 가 true 이면 다음 영상, false 이면 이전 영상을 재생
     */
    public void SwapVideoClip(bool isNext) 
    {
        //! 현재 재생 중인 영상의 번호를 기준으로 체크
        //! 이전 영상 번호는 현재 영상보다 배열에서 인덱스 번호가 1이 작다
        //! 다음 영상 번호는 현재 영상보다 배열에서 인덱스 번호가 1이 크다

        int setVCIdxNum = currentClipIdxNum;
        videoPlayer.Stop();

        //! 재생할 영상을 고르기 위한 과정
        if (isNext) 
        {
            //! 배열의 다음 영상을 재생한다
            //! 리스트 전체길이보다 크면 클립을 리스트의 첫번째 영상으로 지정한다
            setVCIdxNum = (setVCIdxNum + 1) % vcList.Length;

            #region Legacy
            //setVCIdxNum++;
            //if (vcList.Length <= setVCIdxNum) 
            //{
            //    //! 리스트 전체 길이보다 크면 리스트의 클립을 첫 번째 영상으로 지정한다
            //    videoPlayer.clip = vcList[0];
            //}
            //else 
            //{
            //    //! 리스트 길이보다 작으면 해당 번호의 영상을 실행한다
            //    videoPlayer.clip = vcList[setVCIdxNum];
            //}
            #endregion
        }
        else 
        {
            // 배열의 이전 영상을 재생한다
            setVCIdxNum = ((setVCIdxNum - 1) + vcList.Length) % vcList.Length;

        }

        videoPlayer.clip = vcList[setVCIdxNum];
        videoPlayer.Play();
        currentClipIdxNum = setVCIdxNum;
    }

    public void SetVideoPlay(int num)
    {
        // 현재 재생 중인 번호가 전달 받은 번호와 다를 때만 실행
        if (currentClipIdxNum != num) 
        {
            videoPlayer.Stop();
            videoPlayer.clip = vcList[num];
            currentClipIdxNum = num;
            videoPlayer.Play();
        }
    }
}

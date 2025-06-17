using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadVideo : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the Video Player")] private VideoPlayer videoPlayer;
    [SerializeField, Tooltip("File name in streaming assets")] private string fileName;

    private void Awake()
    {
        string videoURL = System.IO.Path.Combine(Application.streamingAssetsPath, fileName.Trim());
        videoPlayer.url = videoURL;
    }

}

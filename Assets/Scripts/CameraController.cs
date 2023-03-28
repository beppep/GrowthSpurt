using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const int layerDepth = 10;
    Grid gridPrefab;
    public Vector3 startPos;
    float normalCameraSize = 6.5f;
    float targetSize;
    Vector3 targetPos;
    public float moveSpeed;
    public float yOffset = -1;
    bool movable = false;
    bool playing = false;
    AudioSource[] layerMusic = new AudioSource[4];
    float originYPos;
    public float overshootPower;

    // Start is called before the first frame update
    void Start()
    {
        setCameraSize();
        targetPos = startPos;
        gridPrefab = FindObjectOfType<Grid>();
        // Gather music
        var audioSrcs = GetComponents<AudioSource>();
        layerMusic[0] = audioSrcs[6];
        layerMusic[1] = audioSrcs[5];
        layerMusic[2] = audioSrcs[4];
        layerMusic[3] = audioSrcs[3];
        print(layerMusic.Length);

        // Start all music at the same time but muted
        foreach (var audio in layerMusic)
        {
            audio.Play();
            audio.loop = true;
            audio.volume = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (movable && playing)
        {
            handleScroll();
            handleMovement();
        }
        else if(movable)
            handleMovement();
        handleSize();
        if (Input.GetKeyDown(KeyCode.Escape))
            targetPos.y = FindObjectOfType<Tree>().getPossibleHeight();
    }

    private void setCameraSize()
    {
        float aspectRatio = 9f * Screen.height / Screen.width + 0.1f;
        //print(aspectRatio);
        if (aspectRatio >= 12)
            normalCameraSize = 5f;
        if (aspectRatio >= 12.5f)
            normalCameraSize = 5.35f;
        if (aspectRatio >= 14)
            normalCameraSize = 6.6f;
        if (aspectRatio >= 15)
            normalCameraSize = 6.25f;
        if (aspectRatio >= 16)
            normalCameraSize = 6.65f;
        if (aspectRatio >= 18)
            normalCameraSize = 7.5f;
        if (aspectRatio >= 18.5f)
            normalCameraSize = 7.7f;
        if (aspectRatio >= 19.5f)
            normalCameraSize = 8.1f;

        GetComponent<Camera>().orthographicSize = targetSize = normalCameraSize;
    }

    private void FixedUpdate()
    {
        handleSound();
    }

    public void setOriginPosition(float yPos)
    {
        movable = false;
        originYPos = yPos;
    }

    public void updatePos(float yPos)
    {
        float yDiff = yPos - originYPos;
        transform.position = targetPos -= new Vector3(0, yDiff, 0);
    }

    public void overshoot(float yDiff)
    {
        targetPos -= new Vector3(0, yDiff * overshootPower * Time.deltaTime, 0);
        movable = true;
    }


    public void win()
    {
        playing = false;
        targetPos.y = FindObjectOfType<Tree>().getPossibleHeight();
        moveSpeed = 3;
    }

    public void start()
    {
        targetPos = new Vector3(0, 1, -10);
        //moveSpeed = 10;
        movable = true;
        playing = true;
    }

    public float getHeight()
    {
        return targetPos.y;
    }

    void handleSound()
    {
        var crossfadeSpeed = 0.005f; // per update
        var activeSongIndex = Math.Min(Math.Floor(targetPos.y*-1 / layerDepth), layerMusic.Length - 1);

        for (int i = 0; i < layerMusic.Length; i++)
        {
            if (i == activeSongIndex) layerMusic[i].volume += crossfadeSpeed;
            else layerMusic[i].volume -= crossfadeSpeed;
        }
    }

    void handleScroll()
    {
        int height = FindObjectOfType<Tree>().getPossibleHeight();
        Vector2 scrollDelta = Input.mouseScrollDelta;
        if (scrollDelta == Vector2.zero)
            return;
        Vector3 newPos = targetPos + new Vector3(scrollDelta.x, scrollDelta.y, -10);
        float yValue = Mathf.Max(newPos.y, gridPrefab.getCurrentDepth()-5);
        yValue = Mathf.Min(yValue, height);
        targetPos = new Vector3(newPos.x, yValue, -10);
    }

    void handleMovement()
    {
        Vector3 dist = targetPos - transform.position;
        transform.position += dist * Time.deltaTime * moveSpeed;

        // Avoid unnessesary updates
        if (dist.magnitude < 0.001f) transform.position = targetPos;
    }

    public void moveTo(int d)
    {
        Vector3 newPos = new Vector3(targetPos.x, Mathf.Min(targetPos.y,d - 2), -10);
        targetPos = new Vector3(newPos.x, newPos.y, -10);
    }

    void handleSize()
    {
        targetSize = normalCameraSize;
        if (targetPos.y > 3)
            targetSize = 7f;
        if (targetPos.y > 4)
            targetSize = 9f;
        if (targetPos.y > 5)
            targetSize = 12f;
        float dist = targetSize - GetComponent<Camera>().orthographicSize;
        GetComponent<Camera>().orthographicSize += dist * Time.deltaTime * moveSpeed;
    }
}

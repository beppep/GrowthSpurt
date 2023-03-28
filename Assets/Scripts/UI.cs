using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    CameraController myCam;
    public GameObject[] children;
    PlayButton playButton;
    QuitButton quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton = FindObjectOfType<PlayButton>();
        quitButton = FindObjectOfType<QuitButton>();
        myCam = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (myCam.transform.position.y >= 2.5f)
            activateChildren();
        else
            deactivateChildren();
    }

    void activateChildren()
    {
        foreach (GameObject child in children)
        {
            child.gameObject.SetActive(true);
        }

    }

    void deactivateChildren()
    {
        foreach (GameObject child in children)
        {
            child.gameObject.SetActive(false);
        }
    }
}

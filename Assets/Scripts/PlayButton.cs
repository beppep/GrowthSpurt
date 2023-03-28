using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public triggerStartSound sound;
    public GameObject[] activations;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(start);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void start()
    {
        foreach (GameObject go in activations)
        {
            go.SetActive(true);
        }
        sound.playStartSound();
        FindObjectOfType<CameraController>().start();
        FindObjectOfType<CardManager>().init();
        FindObjectOfType<Grid>().reset();
        FindObjectOfType<Tree>().reset();

        //gameObject.SetActive(false);
        //FindObjectOfType<QuitButton>().gameObject.SetActive(false);
        //transform.parent.gameObject.SetActive(false);
    }

}

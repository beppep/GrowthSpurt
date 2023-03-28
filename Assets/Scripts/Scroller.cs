using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scroller : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CameraController myCamera;
    float currentMouseYPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float yPos = mouseWorldPos.y;
        myCamera.setOriginPosition(yPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentMouseYPos = Input.mousePosition.y;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float yPos = mouseWorldPos.y;
        myCamera.updatePos(yPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        myCamera.overshoot(Input.mousePosition.y - currentMouseYPos);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

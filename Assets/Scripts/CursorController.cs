using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D[] cursorTextures;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTextures[0], hotSpot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchCursor(int i)
    {
        Cursor.SetCursor(cursorTextures[i], hotSpot, cursorMode);
    }
}

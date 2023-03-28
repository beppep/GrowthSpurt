using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public AudioClip[] drawCardSounds;
    public CardManager cardManagerPrefab;
    Sprite cardSprite;
    Vector4 entryPoints;
    Image image;
    Vector2 startPos;
    bool drag;
    CursorController cursor;

    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<CursorController>();
        cardManagerPrefab = FindObjectOfType<CardManager>();
        int cardType = Random.Range(0, 11);
        //cardSprite = image.sprite = cardManagerPrefab.rootSprites[cardType];
        //entryPoints = cardManagerPrefab.entryPointsList[cardType];
        GetComponent<RectTransform>().localScale = new Vector3(1.581028f, 1.581028f, 1.581028f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!drag)
            handleMovement();
    }

    public void setCardSprite(Sprite sprite)
    {
        image = GetComponent<Image>();
        cardSprite = image.sprite = sprite;
    }

    public void setEntryPoints(Vector4 v4)
    {
        entryPoints = v4;
    }

    public void setTargetPosition(Vector3 pos)
    {
        startPos = pos;
    }

    public void OnBeginDrag(PointerEventData eventdata)
    {
        cursor.switchCursor(2);
        drag = true;
        //startPos = GetComponent<RectTransform>().anchoredPosition;
        playSound();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void handleMovement()
    {
        Vector2 dist = startPos - GetComponent<RectTransform>().anchoredPosition;
        GetComponent<RectTransform>().anchoredPosition += dist * Time.deltaTime * 10;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cursor.switchCursor(0);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
        Cell closestCell = null;
        float minDist = Mathf.Infinity;
        Cell[] cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
        {
            float dist = Vector3.Distance(cell.transform.position, mouseWorldPos);
            if (dist < minDist)
            {
                closestCell = cell;
                minDist = dist;
            }
        }
        print(closestCell);
        print(minDist);
        if (minDist < 1 && Input.mousePosition.y > 250)
        {
            if(closestCell.use(this, entryPoints, cardSprite))
                deleteCard();
        }
        drag = false;
    }

    public void deleteCard()
    {
        cardManagerPrefab.removeCard(this);
        Destroy(transform.gameObject);
    }

    void playSound()
    {
        int i = Random.Range(0, drawCardSounds.Length);
        cardManagerPrefab.playSound(drawCardSounds[i]);
    }
}

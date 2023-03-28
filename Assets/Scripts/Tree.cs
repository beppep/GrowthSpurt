using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public Sprite[] sprites;
    SpriteRenderer spriteRend;
    int layerDepth = 10;
    int possibleHeight = 3;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void levelUp()
    {
        
        //level++;
        //spriteRend.sprite = sprites[level];
    }

    public void updateLevel(int depth)
    {
        int level = Mathf.Min(4, Mathf.FloorToInt(Mathf.Abs(depth) / layerDepth));
        if (level == 1)
        {
            transform.position = new Vector3(transform.position.x, 5.5f, transform.position.z);
            possibleHeight = 4;

        }
            
        if (level == 2)
        {
            transform.position = new Vector3(transform.position.x, 9, transform.position.z);
            possibleHeight = 9;
        }
            
        if (level == 3)
        {
            transform.position = new Vector3(transform.position.x, 9.17f, transform.position.z);
        }
            
        if (level == 4 || depth == -39)
        {
            transform.position = new Vector3(transform.position.x, 9.17f, transform.position.z);

        }
            
        spriteRend.sprite = sprites[level];
    }

    public int getPossibleHeight()
    {
        return possibleHeight;
    }

    public void reset()
    {
        spriteRend.sprite = sprites[0];
        transform.position = new Vector3(transform.position.x, 3.56f, transform.position.z);
    }

    public void win()
    {
        spriteRend.sprite = sprites[4];
        transform.position = new Vector3(transform.position.x, 9.17f, transform.position.z);
    }
}

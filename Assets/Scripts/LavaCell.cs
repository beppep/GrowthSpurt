using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCell : NonplacableCell
{

    public override void unlock(Vector4 entryPoint)
    {
        spriteRend.sprite = unlockedSprite;
        state = CellState.used;
        burnRoots(entryPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void burnRoots(Vector4 entryPoint)
    {
        print("start the burning");
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, new Vector2(-1 * entryPoint.x, 0), 1);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(0, entryPoint.y), 1);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, new Vector2(entryPoint.z, 0), 1);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, new Vector2(0, -1 * entryPoint.w), 1);

        if (hit1)
        {
            PlacableCell cell = hit1.transform.gameObject.GetComponent<PlacableCell>();
            if (!cell.GetComponent<FoodCell>())
            {
                print("BURNN!");
                cell.burnNeighbours();
            }
                
        }

        if (hit2)
        {
            PlacableCell cell = hit2.transform.gameObject.GetComponent<PlacableCell>();
            if (!cell.GetComponent<FoodCell>())
            {
                print("BURNN!");
                cell.burnNeighbours();
            }
        }

        if (hit3)
        {
            PlacableCell cell = hit3.transform.gameObject.GetComponent<PlacableCell>();
            if (!cell.GetComponent<FoodCell>())
            {
                print("BURNN!");
                cell.burnNeighbours();
            }
        }

        if (hit4)
        {
            PlacableCell cell = hit4.transform.gameObject.GetComponent<PlacableCell>();
            if (!cell.GetComponent<FoodCell>())
            {
                print("BURNN!");
                cell.burnNeighbours();
            }
        }

        GetComponent<BoxCollider2D>().enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlacableCell : Cell
{
    public AudioClip[] rootSounds;
    Grid gridPrefab;
    public Vector4 possibleEntries;
    Vector4 exitPoints;
    bool burning;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void unlock(Vector4 entryPoint)
    {
        if (state == CellState.locked)
        {
            spriteRend.sprite = unlockedSprite;
            state = CellState.available;
        }
        possibleEntries += entryPoint;
        possibleEntries = new Vector4(  
            Mathf.Min(possibleEntries.x, 1),
            Mathf.Min(possibleEntries.y, 1),
            Mathf.Min(possibleEntries.z, 1),
            Mathf.Min(possibleEntries.w, 1)
        );
    }

    public void removeEntryPoint(Vector4 entryPoint)
    {
        possibleEntries -= entryPoint;
    }

    public void freeze()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, new Vector2(-1 * exitPoints.x, 0), 1);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(0, exitPoints.y), 1);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, new Vector2(exitPoints.z, 0), 1);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, new Vector2(0, -1 * exitPoints.w), 1);

        if (hit1)
        {
            PlacableCell cell = hit1.transform.gameObject.GetComponent<PlacableCell>();
            if(cell)
                cell.removeEntryPoint(new Vector4(0, 0, 1, 0));
        }

        if (hit2)
        {
            PlacableCell cell = hit2.transform.gameObject.GetComponent<PlacableCell>();
            if (cell)
                cell.removeEntryPoint(new Vector4(0, 0, 0, 1));
        }

        if (hit3)
        {
            PlacableCell cell = hit3.transform.gameObject.GetComponent<PlacableCell>();
            if (cell)
                cell.removeEntryPoint(new Vector4(1, 0, 0, 0));
        }

        if (hit4)
        {
            PlacableCell cell = hit4.transform.gameObject.GetComponent<PlacableCell>();
            if (cell)
                cell.removeEntryPoint(new Vector4(0, 1, 0, 0));
        }

    }

    public override bool use(CardController card, Vector4 entryPoints, Sprite sprite)
    {
        
        if (state != CellState.available)
        {
            playErrorSound();
            return false;
        }
        Vector4 v4 = entryPoints + possibleEntries;
        float maxValue = Mathf.Max(v4.x, v4.y, v4.z, v4.w);
        if (maxValue < 2)
        {
            playErrorSound();
            return false;
        }
        exitPoints = entryPoints;
        state = CellState.used;
        spriteRend.sprite = sprite;
        unlockNeighbours(entryPoints);
        gridPrefab = FindObjectOfType<Grid>();
        gridPrefab.updateCurrentDepth(depth);
        playSound();
        handleUse();
        foreach (RootListener listener in FindObjectsOfType<RootListener>())
        {
            listener.trigger(this);
        }
        return true;
    }
    protected abstract void handleUse();

    protected void unlockNeighbours(Vector4 entryPoints)
    {
        /*
        Cell[,] grid = FindObjectOfType<Grid>().getCellGrid();
        int row = (int) gridPos.x;
        int col = (int) gridPos.y;

        if(row - 1 > - 1 && entryPoints.x != 0)
            grid[row - 1 * (int) entryPoints.x, col].unlock(new Vector4(0, 0, 1, 0));
        //DSADSA
        grid[row, col + (int) entryPoints.y].unlock(new Vector4(0, 0, 0, 1));
        grid[row + (int) entryPoints.z, col].unlock(new Vector4(1, 0, 0, 0));
        grid[row, col - 1 * (int) entryPoints.w].unlock(new Vector4(0, 1, 0, 0));
        */
        burning = false;
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, new Vector2(-1 * entryPoints.x, 0), 1);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(0, entryPoints.y), 1);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, new Vector2(entryPoints.z, 0), 1);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, new Vector2(0, -1 * entryPoints.w), 1);
        GetComponent<BoxCollider2D>().enabled = true;
        if (hit1 && !burning)
        {
            Cell cell = hit1.transform.gameObject.GetComponent<Cell>();
            cell.unlock(new Vector4(0, 0, 1, 0));
        }

        if (hit2 && !burning)
        {
            Cell cell = hit2.transform.gameObject.GetComponent<Cell>();
            cell.unlock(new Vector4(0, 0, 0, 1));
        }

        if (hit3 && !burning)
        {
            Cell cell = hit3.transform.gameObject.GetComponent<Cell>();
            cell.unlock(new Vector4(1, 0, 0, 0));
        }

        if (hit4 && !burning)
        {
            Cell cell = hit4.transform.gameObject.GetComponent<Cell>();
            cell.unlock(new Vector4(0, 1, 0, 0));
        }
        
        
    }

    bool hasExitPoint(Vector4 exitPoint)
    {
        Vector4 v4 = exitPoints + exitPoint;
        return Mathf.Max(v4.x, v4.y, v4.z, v4.w) >= 2;
    }

    void deletePossibleEntry(Vector4 entryPoint)
    {
        possibleEntries -= entryPoint;
    }

    public void burnNeighbours()
    {
        burning = true;
        GetComponent<BoxCollider2D>().enabled = false;
        spriteRend.sprite = unlockedSprite;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, new Vector2(-1 * exitPoints.x, 0), 1);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(0, exitPoints.y), 1);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, new Vector2(exitPoints.z, 0), 1);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, new Vector2(0, -1 * exitPoints.w), 1);

        Vector4 waterRootDirections = Vector4.zero;

        if (hit1)
        {
            
            PlacableCell cell = hit1.transform.gameObject.GetComponent<PlacableCell>();
            Vector4 rootDirection = new Vector4(0, 0, 1, 0);
            if (cell.isAvailable())
                cell.deletePossibleEntry(new Vector4(1, 0, 0, 0));
            if (cell.hasExitPoint(rootDirection) && cell.isUsed())
            {
                if (cell.GetComponent<FoodCell>())
                    waterRootDirections += rootDirection;
                else
                {
                    possibleEntries -= new Vector4(1, 0, 0, 0);
                    exitPoints = Vector4.zero;
                    cell.burnNeighbours();
                }
                    
            }
        }

        if (hit2)
        {
            PlacableCell cell = hit2.transform.gameObject.GetComponent<PlacableCell>();
            Vector4 rootDirection = new Vector4(0, 0, 0, 1);
            if (cell.isAvailable())
                cell.deletePossibleEntry(new Vector4(0, 1, 0, 0));
            if (cell.hasExitPoint(rootDirection) && cell.isUsed())
            {
                if (cell.GetComponent<FoodCell>())
                    waterRootDirections += rootDirection;
                else
                {
                    possibleEntries -= new Vector4(0, 1, 0, 0);
                    exitPoints = Vector4.zero;
                    cell.burnNeighbours();
                }
                    
            }
        }

        if (hit3)
        {
            PlacableCell cell = hit3.transform.gameObject.GetComponent<PlacableCell>();
            Vector4 rootDirection = new Vector4(1, 0, 0, 0);
            if (cell.isAvailable())
                cell.deletePossibleEntry(new Vector4(0, 0, 1, 0));
            if (cell.hasExitPoint(rootDirection) && cell.isUsed())
            {
                if (cell.GetComponent<FoodCell>())
                    waterRootDirections += rootDirection;
                else
                {
                    possibleEntries -= new Vector4(0, 0, 1, 0);
                    exitPoints = Vector4.zero;
                    cell.burnNeighbours();
                }
                    
            }
        }

        if (hit4)
        {
            PlacableCell cell = hit4.transform.gameObject.GetComponent<PlacableCell>();
            Vector4 rootDirection = new Vector4(0, 1, 0, 0);
            if (cell.isAvailable())
                cell.deletePossibleEntry(new Vector4(0, 0, 0, 1));
            if (cell.hasExitPoint(rootDirection) && cell.isUsed())
            {
                if (cell.GetComponent<FoodCell>())
                    waterRootDirections += rootDirection;
                else
                {
                    possibleEntries -= new Vector4(0, 0, 0, 1);
                    exitPoints = Vector4.zero;
                    cell.burnNeighbours();
                }
                    
            }
        }
        if (waterRootDirections == Vector4.zero)
            state = CellState.locked;
        else
            state = CellState.available;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void playSound()
    {
        int i = Random.Range(0, rootSounds.Length-1);
        audioSource.clip = rootSounds[i];
        audioSource.Play();
    }

    void playErrorSound()
    {
        audioSource.clip = errorSounds[Random.Range(0, errorSounds.Length - 1)];
        audioSource.Play();
    }

}

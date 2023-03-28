using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using static LevelGenerator;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public Sprite level;
    public TextAsset startingHandParameter;
    public TextAsset waterParameters;
    public TextAsset mechanicParameters;

    private const float totalLayersPlusOne = 8f;
    private const float rowsPerLayer = 9;
    CameraController cameraController;
    public Cell[] cellPrefabs;
    public DirtCell dirtCellPrefab;
    public FoodCell foodCellPrefab;
    public NonplacableCell rockCellPrefab;
    public LavaCell lavaCellPrefab;
    public Sprite[] sprites;
    Cell[,] cellGrid;
    int rows;
    int cols;
    public int currentDepth;


    // Start is called before the first frame update
    void Start()
    {
        if (false /*has saved level*/)
        {
            //READ FROM FILE
        }
        else
        {
            createLevel();
        }
        cameraController = FindObjectOfType<CameraController>();
        currentDepth = 0;
        //createGrid();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
          //  win();
    }

    public List<Vector4> getStartHand()
    {
        return rowToVectors(startingHandParameter.ToString().Split("\n")[0]);
        /*
        List<Vector4> vectors = new List<Vector4>();
        foreach(string s in cardParameters.ToString().Split("\n")[0].Split(","))
        {
            vectors.Add(stringToVector(s));
        }
        return vectors;
        */
    }

    List<Vector4> rowToVectors(string row)
    {
        List<Vector4> vectors = new List<Vector4>();
        foreach (string s in row.Trim().Split(","))
        {
            vectors.Add(stringToVector(s));
        }
        return vectors;
    }

    Vector4 stringToVector(string s)
    {
        int x = (int)Char.GetNumericValue(s[0]);
        int y = (int)Char.GetNumericValue(s[1]);
        int z = (int)Char.GetNumericValue(s[2]);
        int w = (int)Char.GetNumericValue(s[3]);
        Vector4 v4 = new Vector4(x,y,z,w);
        while (v4.x + v4.y + v4.z + v4.w <= 1)
            v4 = new Vector4(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        return v4;            
    }

    void createLevel()
    {
        
        Texture2D texture = level.texture;
        int height = texture.height;
        rows = 1 + Mathf.RoundToInt((height - 7) / 8);
        cols = 7;
        cellGrid = new Cell[rows, cols];
        //Color32[] pixels = texture.GetPixels32();
        
        int cellNum = 0;
        List<Color32> waterOrder = new List<Color32>();
        Dictionary<Color32, List<Vector3>> waterPositions = new Dictionary<Color32, List<Vector3>>();
        Dictionary<Color32, List<Vector3>> rockPositions = new Dictionary<Color32, List<Vector3>>();
        Dictionary<Color32, List<Vector3>> lavaPositions = new Dictionary<Color32, List<Vector3>>();
        //foreach(Color32 p in pixels)
        //    print(p);
        while(cellNum < rows*cols)
        {
            int c = cellNum % cols;
            int r = Mathf.FloorToInt(cellNum / cols);
            Color32 pixel = texture.GetPixel(c * 8, height - r * 8 - 1);
            Vector3 position = new Vector3(c - cols / 2, -r, 0);

            if (pixel.b  == 255)
            {
                if(!waterPositions.ContainsKey(pixel))
                {
                    waterPositions[pixel] = new List<Vector3>();
                    waterOrder.Add(pixel);
                }
                position.z = cellNum;
                waterPositions[pixel].Add(position);
            }
            else if(pixel.r == 255)
            {
                if (!lavaPositions.ContainsKey(pixel))
                {
                    lavaPositions[pixel] = new List<Vector3>();
                }
                position.z = cellNum;
                lavaPositions[pixel].Add(position);
            }
            else if(pixel.r == pixel.g && pixel.r == pixel.b)
            {
                if (!rockPositions.ContainsKey(pixel))
                {
                    rockPositions[pixel] = new List<Vector3>();
                }
                position.z = cellNum;
                rockPositions[pixel].Add(position);
            }
            else if(pixel.r == 136 && pixel.b == 21)
            {
                //Synligt hinder
            }
            else
            {
                spawnCell(c, r, position, dirtCellPrefab);
            }
            cellNum++;
        }
        spawnWaterCells(waterPositions, waterOrder);
        spawnRockCells(rockPositions);
        spawnLavaCells(lavaPositions);

        int middlecolumn = cols / 2;
        cellGrid[0, middlecolumn].unlock(new Vector4(0, 1, 0, 0));
    }

    private void spawnCell(int c, int r, Vector3 position, Cell prefab)
    {
        Cell newCell = Instantiate(prefab);
        newCell.transform.position = position;
        newCell.transform.SetParent(transform);
        newCell.setDepth((int)position.y);
        cellGrid[r, c] = newCell;
    }

    private void spawnWaterCell(int c, int r, Vector3 position, List<Vector4> vectors)
    {
        FoodCell newCell = Instantiate(foodCellPrefab);
        newCell.addRoots(vectors);
        newCell.transform.position = position;
        newCell.transform.SetParent(transform);
        newCell.setDepth((int)position.y);
        cellGrid[r, c] = newCell;
    }


    void spawnWaterCells(Dictionary<Color32, List<Vector3>> waterPositions, List<Color32> waterOrder)
    {
        string[] parameters = waterParameters.ToString().Trim().Split("\n");
        for(int i = 0; i < waterOrder.Count; i++)
        {
            List<Vector4> vectors = new List<Vector4>();
            if(i < parameters.Length)
            {
                vectors = rowToVectors(parameters[i]);
            }
            else
            {
                vectors = rowToVectors("0000,0000,0000,0000");
            }
            List<Vector3> positions = waterPositions[waterOrder[i]];

            int rnd = Random.Range(0, positions.Count);
            Vector2 chosenPosition = positions[rnd];
            int cellNum = (int)positions[rnd].z;
            positions.RemoveAt(rnd);
            int c = cellNum % cols;
            int r = Mathf.FloorToInt(cellNum / cols);

            //SPAWN WATERTILE//
            spawnWaterCell(c, r, chosenPosition, vectors);

            foreach (Vector3 pos in positions)
            {
                cellNum = (int)pos.z;
                c = cellNum % cols;
                r = Mathf.FloorToInt(cellNum / cols);
                Vector2 v2Pos = new Vector2(pos.x, pos.y);
                spawnCell(c, r, v2Pos, dirtCellPrefab);

            }
        }
    }

    void spawnRockCells(Dictionary<Color32, List<Vector3>> rockPositions)
    {
        foreach(List<Vector3> positions in rockPositions.Values)
        {
            int rnd = Random.Range(0, positions.Count);
            Vector2 chosenPosition = positions[rnd];
            int cellNum = (int)positions[rnd].z;
            positions.RemoveAt(rnd);
            int c = cellNum % cols;
            int r = Mathf.FloorToInt(cellNum / cols);
            spawnCell(c, r, chosenPosition, rockCellPrefab);

            foreach (Vector2 pos in positions)
            {
                spawnCell(c, r, pos, dirtCellPrefab);
            }
        }
    }

    void spawnLavaCells(Dictionary<Color32, List<Vector3>> lavaPositions)
    {
        foreach (List<Vector3> positions in lavaPositions.Values)
        {
            int rnd = Random.Range(0, positions.Count);
            Vector2 chosenPosition = positions[rnd];
            int cellNum = (int)positions[rnd].z;
            positions.RemoveAt(rnd);
            int c = cellNum % cols;
            int r = Mathf.FloorToInt(cellNum / cols);
            spawnCell(c, r, chosenPosition, lavaCellPrefab);

            foreach (Vector2 pos in positions)
            {
                spawnCell(c, r, pos, dirtCellPrefab);
            }
        }
    }


    void createGrid()
    {
        /*
        int[][] level = LevelGenerator.getLevel();
        cols = level.Length;
        rows = level[0].Length;
        cellGrid = new Cell[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int i = level[c][r];
                //int i = 1;
                Cell newCell = Instantiate(cellPrefabs[i]);
                if (i == (int) Tiles.Food)
                {
                    FoodCell foodCell = (FoodCell)newCell;
                    int minPower = Mathf.Abs(c - cols / 2) - 1;
                    int maxPower = Mathf.Abs(c - cols / 2) + 1;
                    //foodCell.setPower(Random.Range(minPower, maxPower + 1) + 3);
                    foodCell.setPower(4);
                }

                if (i == (int) Tiles.Dirt || i == (int) Tiles.Food || i == (int) Tiles.Rock)
                {
                    int layer = (int) Math.Floor(r / rowsPerLayer);
                    System.Random rnd = new System.Random();

                    float layerDepth = (r % rowsPerLayer);    // 0 - 14
                    float layerMidDepth = (rowsPerLayer / 2); // 7

                    var distFromLayer = Math.Abs(layerDepth - layerMidDepth);
                    var isAboveLayer = layerDepth > layerMidDepth+3;

                    if (rnd.Next((int) (layerMidDepth * 1.5)) <= distFromLayer)
                    {
                        if (isAboveLayer)
                            layer += 1;
                    }

                    // Delat med layers +1 för sista layern är svart.
                    float tint =  1 - layer / totalLayersPlusOne;

                    // tint = 1 - (r / 60f);
                    newCell.GetComponent<SpriteRenderer>().color = new Color(tint, tint, tint, 1f);
                }
                newCell.transform.position = new Vector3(c-cols/2, -r, 0);
                newCell.transform.SetParent(transform);
                newCell.setDepth(-r);
                newCell.setGridPos(new Vector2(r,c));
                cellGrid[r, c] = newCell;
            }
        }

        float tint2 =  1 - (4 / totalLayersPlusOne);
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.name == "ground-water")
            {
                obj.GetComponent<SpriteRenderer>().color = new Color(tint2, tint2, tint2, 1f);
            }
        }

        int middlecolumn = cols / 2;
        cellGrid[0, middlecolumn].unlock(new Vector4(0,1,0,0));
        */
    }

    public Cell[,] getCellGrid()
    {
        return cellGrid;
    }

    public void updateCurrentDepth(int d)
    {
        //Debug.Log(d);
        //Debug.Log(currentDepth);
        cameraController = FindObjectOfType<CameraController>();
        currentDepth = Mathf.Min(d, currentDepth);
        cameraController.moveTo(d);
        FindObjectOfType<Tree>().updateLevel(currentDepth);
        if (d == -39)
            win();
    }

    public int getCurrentDepth()
    {
        return currentDepth;
    }

    void win()
    {
        cameraController.win();
        FindObjectOfType<CardManager>().removeAllCards();
        GetComponent<AudioSource>().Play();
        FindObjectOfType<Tree>().win();
    }

    public void reset()
    {
        //foreach(Cell cell in cellGrid)
        //{
            //Destroy(cell.gameObject);
        //}
        createGrid();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class LevelGenerator : MonoBehaviour
{

    static private int layer_1 = 10;
    static private int layer_2 = 10;
    static private int layer_3 = 10;
    static private int layer_4 = 10;

    //colums och rows är roterade 90 grader
    static private int n_rows = 25;
    static private int n_colums = layer_1 + layer_2 + layer_3 + layer_4;
    static private Random rnd = new Random();

    public enum Tiles
    {
        Path = -1,
        Dirt = 0,
        Food = 1,
        Rock = 2,
    }

    public static int[][] getLevel(bool debug = false)
    {
        print("Level Generation Started");

        List<int[][]> layers = new List<int[][]>();

        // Create an empty layer
        int[][] layer = new int[n_rows][];
        int[] row = new int[n_colums];
        for (int i = 0; i < n_rows; i++)
        {
            layer[i] = (int[]) row.Clone();
        }

        // Spawn rocks!
        int spawnChance = 0; // 1-1000
        for (int y = 0; y < layer[0].Length; y++)
        {
            for (int x = 0; x < layer.Length; x++)
            {
                if (y >= 5)
                {
                    spawnChance = 30;
                }

                if (y > layer_1) {
                    spawnChance = 70;
                }
                if (y > layer_1 + layer_2)
                {
                    spawnChance = 100;
                }
                if (y > layer_1 + layer_2 + layer_3)
                {
                    spawnChance = 150;
                }

                if (rnd.Next(1000) < spawnChance
                || x == 2 || x == 2 + 20)
                    layer[x][y] = (int) Tiles.Rock;
            }
        }

        // Spawn food!
        for (int y = 0; y < layer[0].Length; y++)
        {
            for (int x = 0; x < layer.Length; x++)
            {
                if (y < 1
                || x <= 2 || x >= 2 + 20) continue;

                spawnChance = 150;

                if (y > layer_1) {
                    spawnChance = 100;
                }
                if (y > layer_1 + layer_2)
                {
                    spawnChance = 60;
                }
                if (y > layer_1 + layer_2 + layer_3)
                {
                    spawnChance = 0;
                }

                if (rnd.Next(1000) < spawnChance)
                    layer[x][y] = (int)Tiles.Food;
            }
        }

        // DEBUG: Add path to layer
        var path = generatePath(layer);
        if (debug) { 
            foreach (var pos in path)
            {
                layer[pos.Item1][pos.Item2] = (int) Tiles.Path;
            }
        }

        // Copy into layers array
        //layer[0][0] = 1;
        layers.Add(cloneJagged(layer));

        // Create level from generated layers
        int[][] level = new int[n_rows*layers.Count][];
        for (int layerIndex = 0; layerIndex < layers.Count; layerIndex++)
        {
            layer = layers[layerIndex];
            for (int layerRow = 0; layerRow < n_rows; layerRow++)
            {
                level[layerIndex * n_rows + layerRow] = layer[layerRow];
            }
        }

        return level;

        //printJagged(level);
        //print("DONE");
    }

    static HashSet<(int, int)> generatePath(int[][] layer)
    {
        int middle = (int) Math.Round(n_colums / 2.0);
        return takeStep(0, middle, new HashSet<(int, int)>(), layer);
    }

    private static HashSet<(int, int)> takeStep(int newRow, int newCol, HashSet<(int, int)> steps, int[][]  layer)
    {
        //print(newRow + " " + newCol);

        // If outside of layer
        if (newRow < 0 || newRow >= n_rows || newCol < 0 || newCol >= n_colums) return steps;

        // If already in path
        if (steps.Contains((newRow, newCol))) return steps;

        // If rock
        if (layer[newRow][newCol] == (int) Tiles.Rock) return steps;

        // Valid step, add
        steps.Add((newRow, newCol));

        //print("Did not get cancelled");
        if (steps.Count > 5 && rnd.Next(100) < 1) return steps;
        if (rnd.Next(100) < 75) steps.UnionWith(takeStep( 1 + newRow,  0 + newCol, steps, layer));
        if (rnd.Next(100) < 25) steps.UnionWith(takeStep(-1 + newRow,  0 + newCol, steps, layer));
        if (rnd.Next(100) < 50) steps.UnionWith(takeStep( 0 + newRow,  1 + newCol, steps, layer));
        if (rnd.Next(100) < 50) steps.UnionWith(takeStep( 0 + newRow, -1 + newCol, steps, layer));

        return steps;
    }

    public static void printJagged<T>(T[][] matrix)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            print(string.Join(" ", matrix[i]));
        }
    }

    public static T[][] cloneJagged<T>(T[][] matrix)
    {
        T[][] newMatrix = new T[matrix.Length][];
        for (int i = 0; i < matrix.Length; i++)
        {
            newMatrix[i] = (T[]) matrix[i].Clone();
        }
        return newMatrix;
    }

    // One layer should fill up the screen, making sure you always see the next layer
    // First layer will be sky / menu

    enum Layers {
        Sky,
        Ground,
        Rock,
        Caves,
        Magma,
    }

    /// <summary>
    /// Returns an 2d array that is used in level generation. 
    /// </summary>
    /// <param name="width">Level width</param>
    /// <param name="height">Level height</param>
    /// <param name="layers">Defineds what type of layer and in which order layers should spawn</param>
    /// <returns>2D level array</returns>
    int[,] getLevel(int width, int height, Layers[] layers)
    {
        int[,] level = new int[height*layers.Length, width];
        
        for (int i = 0; i < layers.Length; i++)
        {
            int[,] layer = new int[height, width];
            
        }

        return level;
    }

    private static int[][] Concat(int[][] array1, int[][] array2)
    {
        int[][] result = new int[array1.Length + array2.Length][];

        array1.CopyTo(result, 0);
        array2.CopyTo(result, array1.Length);

        return result;
    }

}

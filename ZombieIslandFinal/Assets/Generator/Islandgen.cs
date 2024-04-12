using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Islandgen : MonoBehaviour
{
    public int width;
    public int height;
    [Range(0, 101)]
    public int fillProbability;
    [Range(1, 20)]
    public int Iteration;

    private int[,] grid;
    [Range(1, 8)]
    public int birthLimit;
    [Range(1, 8)]
    public int deathLimit;

   

    public GameObject Zombie;
   
    public GameObject Survivor;

    public GameObject Sand;

    private Vector3 spawnpoint;

    private string mapcheck;

    public int survivorAmount = 5;

    public int ZombieCount;

  


    public GameObject LandPrefab; 
    public GameObject WaterPrefab;
    List<Vector3> Islandedge = new List<Vector3>();


    void Start()
    {
        
        GenerateIsland();
       


        spawnZombie();
        spawnsurvivor(survivorAmount);
        

    }

    void GenerateIsland()
    {
        grid = new int[width, height];
        RandomFillIsland();
        boarderpos();
        
    }

    void RandomFillIsland()
    {
        grid = new int[width, height];
        for (int x = 15; x < width - 15; x++)
        {
            for (int y = 15; y < height - 15; y++)
            {
                grid[x, y] = Random.Range(1, 101) < fillProbability ? 1 : 0;
            }
        }
        // Apply cellular automata rules
        for (int i = 0; i < Iteration; i++) 
        {
            int[,] newGrid = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int aliveNeighbors = CountAliveNeighbors(x, y);


                  
                    if (grid[x, y] == 1)
                    {
                        if (aliveNeighbors < deathLimit) newGrid[x, y] = 0;

                        else
                        {
                            newGrid[x, y] = 1;

                        }
                    }

                    if (grid[x, y] == 0)
                    {
                        if (aliveNeighbors > birthLimit) newGrid[x, y] = 1;

                        else
                        {
                            newGrid[x, y] = 0;
                        }
                    }
                }
            }
            // Update the grid with the new values
            grid = newGrid;
        }
        // Generate map based on the final grid state
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 1) // Wall
                {
                    GameObject newLand = Instantiate(LandPrefab, new Vector3(x, y, 0.0f), Quaternion.identity);
                    
                    newLand.transform.parent = transform;
                }
                else
                {
                    GameObject newLand = Instantiate(WaterPrefab, new Vector3(x, y, 0.0f), Quaternion.identity);
                    newLand.transform.parent = transform;
                }
            }
        }
    }
    int CountAliveNeighbors(int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighborX = x + i;
                int neighborY = y + j;
                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    count += grid[neighborX, neighborY];
                }
            }
        }
        count -= grid[x, y]; // Exclude the cell itself
        return count;
    }






    void spawnZombie()
    {
        List<Vector3> LandPositions = new List<Vector3>();

        for (int i = 1; i < ZombieCount; i++) 
        {
            // Find all land positions on the land
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] == 1) 
                    {
                        LandPositions.Add(new Vector3(x, y, 0.0f));
                    }
                }
            }

            // Select a random land position
            if (LandPositions.Count > 0)
            {
                Vector3 randomPosition = LandPositions[Random.Range(0, LandPositions.Count)];


                spawnpoint = randomPosition;
                GameObject zombie = Instantiate(Zombie, randomPosition, Quaternion.identity);
                
            }
            else
            {
                Debug.Log("No  land available to spawn zombie.");
                spawnpoint = new Vector3(width / 2, height / 2, 0.0f);
                GameObject zombie = Instantiate(Zombie, spawnpoint, Quaternion.identity);
                
            }
        }
    }
    void boarderpos()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 1) 
                {
                    int neighbor = CountAliveNeighbors(x, y);
                    // Add tree position to the list (except spawnpoint)
                    if (neighbor < 7)
                    {

                        Islandedge.Add(new Vector3(x, y, 0.0f));
                    }

                }
            }


        }
        foreach (Vector3 position in Islandedge)
        {
            // Instantiate the GameObject at the current position
            GameObject boat = Instantiate(Sand, position, Quaternion.identity);
            print("sand instantiated at position: " + position);
        }

        // Clear the list after spawning all boats
        Islandedge.Clear();
    }

    void spawnsurvivor(int Count)
    {
        List<Vector3> LandPositions = new List<Vector3>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 1) 
                {
                    if (x != spawnpoint.x && y != spawnpoint.y)
                    {
                        LandPositions.Add(new Vector3(x, y, 0.0f));
                    }
                    
                }
            }
        }

        
        for (int i = 0; i < Count; i++)
        {
            // Select a random land position
            if (LandPositions.Count > 0)
            {
                int randomIndex = Random.Range(0, LandPositions.Count);
                Vector3 randomPosition = LandPositions[randomIndex];

                LandPositions.RemoveAt(randomIndex);

                GameObject survivor = Instantiate(Survivor, randomPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No land available to spawn survivor.");
                return; 
            }
        }
    }


}

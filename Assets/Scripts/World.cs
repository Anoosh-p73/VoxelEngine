using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class World : MonoBehaviour
{
    public int worldSize = 5; // Size of the world in number of chunks
    public int chunkSize = 16; // Assuming chunk size is 16x16x16

    private Dictionary<Vector3, Chunk> chunks;

    public static World Instance { get; private set; }

    public Material VoxelMaterial;

    public int noiseSeed = 1234;
    public float maxHeight = 0.2f;
    public float noiseScale = 0.015f;
    public float[,] noiseArray;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want this to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
        noiseArray = GlobalNoise.GetNoise();
    }
    void Start()
    {
        chunks = new Dictionary<Vector3, Chunk>();

        GenerateWorld();
    }

    private void GenerateWorld()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int chunksCreated = 0;
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                for (int z = 0; z < worldSize; z++)
                {
                    Vector3 chunkPosition = new Vector3(x * chunkSize, y * chunkSize, z * chunkSize);
                    GameObject newChunkObject = new GameObject($"Chunk_{x}_{y}_{z}");
                    newChunkObject.transform.position = chunkPosition;
                    newChunkObject.transform.parent = this.transform;

                    Chunk newChunk = newChunkObject.AddComponent<Chunk>();
                    newChunk.Initialize(chunkSize);
                    chunks.Add(chunkPosition, newChunk);

                    chunksCreated++;
                }
            }
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log($"Generated {chunksCreated} chunks in {stopwatch.ElapsedMilliseconds}ms " +
                  $"(Average: {(float)stopwatch.ElapsedMilliseconds / chunksCreated}ms per chunk)");
    }

    public Chunk GetChunkAt(Vector3 globalPosition)
    {
        // Calculate the chunk's starting position based on the global position
        Vector3Int chunkCoordinates = new Vector3Int(
            Mathf.FloorToInt(globalPosition.x / chunkSize) * chunkSize,
            Mathf.FloorToInt(globalPosition.y / chunkSize) * chunkSize,
            Mathf.FloorToInt(globalPosition.z / chunkSize) * chunkSize
        );

        // Retrieve and return the chunk at the calculated position
        if (chunks.TryGetValue(chunkCoordinates, out Chunk chunk))
        {
            return chunk;
        }

        // Return null if no chunk exists at the position
        return null;
    }
}
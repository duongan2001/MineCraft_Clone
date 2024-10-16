using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Transform player;
    public Vector3 spawnPosition;

    public Material material;
    public BlockType[] blockTypes;

    Chunk[,] chunks = new Chunk[VoxelData.worldSizeInChunks, VoxelData.worldSizeInChunks];
    List<ChunkCoord> activeChunks = new List<ChunkCoord>();

    private void Start()
    {
        spawnPosition = new Vector3((VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2, VoxelData.chunkHeight + 1, (VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2);
        GenerateWorld();
    }

    private void Update()
    {
        CheckViewDistance();
    }

    void GenerateWorld()
    {
        for (int x = VoxelData.worldSizeInChunks / 2 - VoxelData.viewDistanceInChunks; x < VoxelData.worldSizeInChunks / 2 + VoxelData.viewDistanceInChunks; x++)
        {
            for (int z = VoxelData.worldSizeInChunks / 2 - VoxelData.viewDistanceInChunks; z < VoxelData.worldSizeInChunks / 2 + VoxelData.viewDistanceInChunks; z++)
            {
                CreateNewChunk(x, z);
            }
        }

        player.position = spawnPosition;
    }

    ChunkCoord GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.chunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.chunkWidth);
        return new ChunkCoord(x, z);
    }

    void CheckViewDistance()
    {
        ChunkCoord coord = GetChunkCoordFromVector3(player.position);

        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);

        for (int x = coord.x - VoxelData.viewDistanceInChunks; x < coord.x + VoxelData.viewDistanceInChunks; x++)
        {
            for (int z = coord.z - VoxelData.viewDistanceInChunks; z < coord.z + VoxelData.viewDistanceInChunks; z++)
            {
                if (IsChunkInWorld(new ChunkCoord(x, z)))
                {
                    if (chunks[x, z] == null)
                    {
                        CreateNewChunk(x, z);
                    }
                    else if (!chunks[x, z].isActive)
                    {
                        chunks[x, z].isActive = true;
                        activeChunks.Add(new ChunkCoord(x, z));
                    }

                    for (int i = 0; i < previouslyActiveChunks.Count; i++)
                    {
                        if (previouslyActiveChunks[i].Equals(new ChunkCoord(x, z)))
                        {
                            previouslyActiveChunks.RemoveAt(i);
                        }
                    }
                }
            }
        }

        foreach (ChunkCoord c in previouslyActiveChunks)
        {
            chunks[c.x, c.z].isActive = false;
        }
    }

    public byte GetVoxel(Vector3 pos)
    {
        if (!IsVoxelInWorld(pos))
            return 0;

        if (pos.y < 1) //Day
            return 1;
        else if (pos.y == VoxelData.chunkHeight - 1)
            return 0; //Dinh
        else
            return 2; //Giua
    }

    void CreateNewChunk(int x, int z)
    {
        chunks[x, z] = new Chunk(new ChunkCoord(x, z), this);
        activeChunks.Add(new ChunkCoord(x, z));
    }

    bool IsChunkInWorld(ChunkCoord coord)
    {
        if (coord.x > 0 && coord.x < VoxelData.worldSizeInChunks - 1 && coord.z < VoxelData.worldSizeInChunks - 1)
            return true;
        else 
            return false;
    }

    bool IsVoxelInWorld(Vector3 pos)
    {
        if (pos.x >= 0 && pos.x < VoxelData.worldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.chunkHeight && pos.z >= 0 && pos.z < VoxelData.worldSizeInVoxels)
            return true;
        else
            return false;
    }
}

[System.Serializable]
public class BlockType
{
    public string blockName;
    public bool isSolid;

    [Header("Texture value")]
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int botFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;

    public int GetTextureID(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return botFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("Error");
                return 0;
        }
    }
}
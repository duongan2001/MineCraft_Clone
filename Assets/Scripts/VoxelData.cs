using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData //Class contain data of a box
{
    public static readonly int chunkWidth = 5;
    public static readonly int chunkHeight = 15;
    public static readonly int worldSizeInChunks = 100;

    public static int worldSizeInVoxels
    {
        get { return worldSizeInChunks * chunkWidth; }
    }

    public static readonly int viewDistanceInChunks = 5;

    public static readonly int textureAtlasSizeinBlock = 4;
    public static float nomalizedBlockTextureSize
    {
        get { return 1f / (float)textureAtlasSizeinBlock; }
    }

    public static readonly Vector3[] voxelVerts = new Vector3[8]{ //8 corner coordinates of a box
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, 1, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, 1),
        new Vector3(1, 0, 1),
        new Vector3(1, 1, 1),
        new Vector3(0, 1, 1)
    };

    public static readonly Vector3[] faceChecks = new Vector3[6] {
        new Vector3(0, 0, -1),
        new Vector3(0, 0, 1),
        new Vector3(0, 1, 0),
        new Vector3(0, -1, 0),
        new Vector3(-1, 0, 0),
        new Vector3(1, 0, 0),
    };

    public static readonly int[,] voxelTris = new int[6, 4]
    {
        {0, 3, 1, 2}, //Back face
        {5, 6, 4, 7}, //Front face
        {3, 7, 2, 6}, //Top face
        {1, 5, 0, 4}, //Bot face
        {4, 7, 0, 3}, //Left face
        {1, 2, 5, 6}, //Right face      
    };

    public static readonly Vector2[] voxelUvs = new Vector2[4]
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(1, 1)
    };
}

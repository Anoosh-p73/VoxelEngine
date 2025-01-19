using UnityEngine;

// Define a simple Voxel struct
public struct Voxel
{
    public Vector3 position;
    public bool isActive;
    public VoxelType type; // Using the VoxelType enum
    public enum VoxelType
    {
        AIR,    // Represents empty space
        SOLID,  // Represents solid space
    }
    public Voxel(Vector3 position, VoxelType type, bool isActive = true)
    {
        this.position = position;
        this.type = type;
        this.isActive = isActive;
    }
}

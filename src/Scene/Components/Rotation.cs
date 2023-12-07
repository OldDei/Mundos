using OpenTK.Mathematics;

public struct Rotation
{
    public Vector3 rotation;
    public Rotation(float x, float y, float z)
    {
        rotation = new Vector3(x, y, z);
    }
}

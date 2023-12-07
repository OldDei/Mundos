using OpenTK.Mathematics;

public struct Scale
{
    public Vector3 scale;
    public Scale(float x, float y, float z)
    {
        scale = new Vector3(x, y, z);
    }
}
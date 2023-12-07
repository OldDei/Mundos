using OpenTK.Mathematics;

public struct Position
{
    public Vector3 position;

    public Position(float x, float y, float z)
    {
        position = new Vector3(x, y, z);
    }
}
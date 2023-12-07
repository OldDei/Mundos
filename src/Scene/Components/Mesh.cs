using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Mundos
{
    /// <summary>
    /// Mesh component containing index of mesh in MeshManager
    /// </summary>
    public struct Mesh
    {
        public int meshIndex; // Index of mesh in MeshManager
        public Mesh(int meshIndex)
        {
            this.meshIndex = meshIndex;
        }
    }
}
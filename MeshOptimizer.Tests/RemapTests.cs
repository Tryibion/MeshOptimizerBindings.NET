namespace MeshOptimizer.Tests;

using System.Runtime.InteropServices;
using Xunit;

public class RemapTests
{
    [StructLayout(LayoutKind.Sequential)]
    private struct Vertex
    {
        public float X, Y, Z;
        public Vertex(float x, float y, float z) => (X, Y, Z) = (x, y, z);
    }

    [Fact]
    public void GenerateVertexRemap_IdentifiesDuplicatesCorrectly()
    {
        var vertices = new Vertex[]
        {
            new(0f, 0f, 0f),
            new(1f, 0f, 0f),
            new(0f, 0f, 0f),
            new(0f, 1f, 0f),
        };

        var remap = new uint[vertices.Length];
        nuint uniqueCount = MeshOpt.GenerateVertexRemap<Vertex>(remap, vertices);

        Assert.Equal((nuint)3, uniqueCount);
        Assert.Equal(remap[0], remap[2]);
    }
}

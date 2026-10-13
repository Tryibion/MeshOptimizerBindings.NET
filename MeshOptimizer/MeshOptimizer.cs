namespace MeshOptimizer;

using System;

/// <summary>
/// High-level, allocation-free safe wrapper over meshoptimizer routines using Spans.
/// </summary>
public static partial class MeshOpt
{
    public static unsafe nuint GenerateVertexRemap<TVertex>(
        Span<uint> destination,
        ReadOnlySpan<TVertex> vertices,
        ReadOnlySpan<uint> indices = default) where TVertex : unmanaged
    {
        if (destination.Length < vertices.Length)
            throw new ArgumentException("Destination span must be at least as large as the vertex count.", nameof(destination));

        fixed (uint* pDest = destination)
        fixed (TVertex* pVerts = vertices)
        fixed (uint* pIndices = indices)
        {
            return MeshOpt.GenerateVertexRemap(
                pDest,
                indices.IsEmpty ? null : pIndices,
                indices.IsEmpty ? (nuint)vertices.Length : (nuint)indices.Length,
                pVerts,
                (nuint)vertices.Length,
                (nuint)sizeof(TVertex));
        }
    }

    public static unsafe void RemapVertexBuffer<TVertex>(
        Span<TVertex> destination,
        ReadOnlySpan<TVertex> vertices,
        ReadOnlySpan<uint> remap) where TVertex : unmanaged
    {
        if (destination.Length < remap.Length)
            throw new ArgumentException("Destination span must be large enough to hold all remapped vertices.", nameof(destination));
        if (remap.Length > vertices.Length)
            throw new ArgumentException("Remap table contains more entries than source vertices.", nameof(remap));

        fixed (TVertex* pDest = destination)
        fixed (TVertex* pVerts = vertices)
        fixed (uint* pRemap = remap)
        {
            MeshOpt.RemapVertexBuffer(
                pDest,
                pVerts,
                (nuint)vertices.Length,
                (nuint)sizeof(TVertex),
                pRemap);
        }
    }

    public static unsafe void RemapIndexBuffer(
        Span<uint> destination,
        ReadOnlySpan<uint> indices,
        ReadOnlySpan<uint> remap)
    {
        if (destination.Length < indices.Length)
            throw new ArgumentException("Destination span must be at least as large as the index count.", nameof(destination));

        fixed (uint* pDest = destination)
        fixed (uint* pIndices = indices)
        fixed (uint* pRemap = remap)
        {
            MeshOpt.RemapIndexBuffer(
                pDest,
                pIndices,
                (nuint)indices.Length,
                pRemap);
        }
    }

    public static unsafe void OptimizeVertexCache(
        Span<uint> destination,
        ReadOnlySpan<uint> indices,
        int vertexCount)
    {
        if (destination.Length < indices.Length)
            throw new ArgumentException("Destination span must be at least as large as the index count.", nameof(destination));
        if (indices.Length % 3 != 0)
            throw new ArgumentException("Index count must be a multiple of 3 for triangle meshes.", nameof(indices));

        fixed (uint* pDest = destination)
        fixed (uint* pIndices = indices)
        {
            MeshOpt.OptimizeVertexCache(
                pDest,
                pIndices,
                (nuint)indices.Length,
                (nuint)vertexCount);
        }
    }

    public static unsafe void OptimizeOverdraw(
        Span<uint> destination,
        ReadOnlySpan<uint> indices,
        ReadOnlySpan<float> vertexPositions,
        int vertexCount,
        int strideBytes,
        float threshold = 1.05f)
    {
        if (destination.Length < indices.Length)
            throw new ArgumentException("Destination span must be at least as large as the index count.", nameof(destination));

        fixed (uint* pDest = destination)
        fixed (uint* pIndices = indices)
        fixed (float* pPositions = vertexPositions)
        {
            MeshOpt.OptimizeOverdraw(
                pDest,
                pIndices,
                (nuint)indices.Length,
                pPositions,
                (nuint)vertexCount,
                (nuint)strideBytes,
                threshold);
        }
    }

    public static unsafe nuint OptimizeVertexFetch<TVertex>(
        Span<TVertex> destination,
        Span<uint> indices,
        ReadOnlySpan<TVertex> vertices) where TVertex : unmanaged
    {
        if (destination.Length < vertices.Length)
            throw new ArgumentException("Destination span must be at least as large as source vertex count.", nameof(destination));

        fixed (TVertex* pDest = destination)
        fixed (uint* pIndices = indices)
        fixed (TVertex* pVerts = vertices)
        {
            return MeshOpt.OptimizeVertexFetch(
                pDest,
                pIndices,
                (nuint)indices.Length,
                pVerts,
                (nuint)vertices.Length,
                (nuint)sizeof(TVertex));
        }
    }

    public static unsafe nuint Simplify(
        Span<uint> destination,
        ReadOnlySpan<uint> indices,
        ReadOnlySpan<float> vertexPositions,
        int vertexCount,
        int strideBytes,
        nuint targetIndexCount,
        float targetError,
        uint options = 0,
        float* resultError = null)
    {
        if (destination.Length < indices.Length)
            throw new ArgumentException("Destination span must be at least as large as the source index count.", nameof(destination));

        fixed (uint* pDest = destination)
        fixed (uint* pIndices = indices)
        fixed (float* pPositions = vertexPositions)
        {
            return MeshOpt.Simplify(
                pDest,
                pIndices,
                (nuint)indices.Length,
                pPositions,
                (nuint)vertexCount,
                (nuint)strideBytes,
                targetIndexCount,
                targetError,
                options,
                resultError);
        }
    }
}

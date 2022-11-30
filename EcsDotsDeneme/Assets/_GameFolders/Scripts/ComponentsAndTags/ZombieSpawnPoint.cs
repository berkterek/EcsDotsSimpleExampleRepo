using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsDeneme
{
    public struct ZombieSpawnPoint : IComponentData
    {
        public NativeArray<float3> Value;
    }
}
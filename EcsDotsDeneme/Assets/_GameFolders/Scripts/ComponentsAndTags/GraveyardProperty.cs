using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsDeneme
{
    public struct GraveyardProperty : IComponentData
    {
        public float2 FieldDimensions;
        public int NumberTombstonesToSpawn;
        public Entity TombstonePrefab;
        public Entity ZombiePrefab;
        public float ZombieSpawnRate;
    }
}


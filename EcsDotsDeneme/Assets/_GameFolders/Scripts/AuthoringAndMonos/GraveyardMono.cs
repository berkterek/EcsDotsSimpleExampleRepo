using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace EcsDotsDeneme
{
    public class GraveyardMono : MonoBehaviour
    {
        public float2 FieldDimensions;
        public int NumberTombstonesToSpawn;
        public GameObject TombstonePrefab;
        public GameObject ZombiePrefab;
        public float ZombieSpawnRate = 0f;
        public uint RandomSeed;
    }
    
    public class GraveyardBaker : Baker<GraveyardMono>
    {
        public override void Bake(GraveyardMono authoring)
        {
            AddComponent(new GraveyardProperty()
            {
                FieldDimensions = authoring.FieldDimensions,
                NumberTombstonesToSpawn = authoring.NumberTombstonesToSpawn,
                TombstonePrefab = GetEntity(authoring.TombstonePrefab),
                ZombiePrefab = GetEntity(authoring.ZombiePrefab),
                ZombieSpawnRate = authoring.ZombieSpawnRate
            });
            
            AddComponent(new GraveyardRandom()
            {
                Value = Random.CreateFromIndex(authoring.RandomSeed) 
            });
            
            AddComponent<ZombieSpawnPoint>();
            AddComponent<ZombieSpawnTimer>();
        }
    }
}
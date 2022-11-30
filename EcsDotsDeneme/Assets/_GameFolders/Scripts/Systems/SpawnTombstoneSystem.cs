using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsDeneme
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GraveyardProperty>();
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperty>();
            var graveyard = SystemAPI.GetAspectRW<GraveyardAspect>(graveyardEntity);

            var entityCommandBufferTemp = new EntityCommandBuffer(Allocator.Temp);
            var spawnPoints = new NativeList<float3>(Allocator.Temp);
            var tombstoneOffset = new float3(0f, -2f, 1f);

            var loopNumber = graveyard.NumberTombstonesToSpawn;
            for (int i = 0; i < loopNumber; i++)
            {
                var newEntity = entityCommandBufferTemp.Instantiate(graveyard.TombstonePrefab);
                var newTombstoneTransform = graveyard.GetRandomTombstoneTransform();

                entityCommandBufferTemp.SetComponent(newEntity, new LocalToWorldTransform
                {
                    Value = newTombstoneTransform
                });

                var newZombieSpawnPoint = newTombstoneTransform.Position + tombstoneOffset;
                spawnPoints.Add(newZombieSpawnPoint);
            }

            graveyard.ZombieSpawnPoints = spawnPoints.ToArray(Allocator.Persistent);

            entityCommandBufferTemp.Playback(state.EntityManager);
        }
    }
}
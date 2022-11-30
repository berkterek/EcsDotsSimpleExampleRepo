using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsDeneme
{
    [BurstCompile]
    [UpdateAfter(typeof(ZombieWalkSystem))]
    public partial struct ZombieEatSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var entitySingletonBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
            var brainScale = SystemAPI.GetComponent<LocalToWorldTransform>(brainEntity).Value.Scale;
            var brainRadius = brainScale * 5f + 1f;

            new ZombieEatJob
            {
                DeltaTime = deltaTime,
                BrainEntity = brainEntity,
                EntityCommandBuffer = entitySingletonBuffer.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                BrainRadiusSq = brainRadius
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieEatJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;
        public Entity BrainEntity;
        public float BrainRadiusSq;

        private void Execute(ZombieEatAspect zombie, [EntityInQueryIndex] int sortKey)
        {
            if (zombie.IsInEatingRange(float3.zero, BrainRadiusSq))
            {
                zombie.Eat(DeltaTime, EntityCommandBuffer, sortKey, BrainEntity);
            }
            else
            {
                EntityCommandBuffer.SetComponentEnabled<ZombieEatProperty>(sortKey, zombie.Entity, false);
                EntityCommandBuffer.SetComponentEnabled<ZombieWalkProperty>(sortKey, zombie.Entity, true);
            }
        }
    }
}
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsDeneme
{
    [BurstCompile]
    [UpdateAfter(typeof(ZombieRiseSystem))]
    public partial struct ZombieWalkSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var entitySingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
            var brainScale = SystemAPI.GetComponent<LocalToWorldTransform>(brainEntity).Value.Scale;
            var brainRadius = brainScale * 5f + 0.5f;
            
            new ZombieWalkJob
            {
                DeltaTime = deltaTime,
                BrainRadiusSq = brainRadius,
                EntityCommandBuffer = entitySingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity
    {
        public float DeltaTime;
        public float BrainRadiusSq;
        public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;

        private void Execute(ZombieWalkAspect zombieWalkAspect, [EntityInQueryIndex] int sortKey)
        {
            zombieWalkAspect.Walk(DeltaTime);
            if (zombieWalkAspect.IsInStoppingRange(float3.zero, BrainRadiusSq))
            {
                EntityCommandBuffer.SetComponentEnabled<ZombieWalkProperty>(sortKey, zombieWalkAspect.Entity, false);
                EntityCommandBuffer.SetComponentEnabled<ZombieEatProperty>(sortKey,zombieWalkAspect.Entity,true);
            }
        }
    }
}
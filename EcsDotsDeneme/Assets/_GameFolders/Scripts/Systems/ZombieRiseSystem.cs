using Unity.Burst;
using Unity.Entities;

namespace EcsDotsDeneme
{
    [BurstCompile]
    [UpdateAfter(typeof(SpawnZombieSystem))]
    public partial struct ZombieRiseSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var entityCommandBufferSingleton =
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new ZombieRiseJob
            {
                DeltaTime = deltaTime,
                EntityCommandBuffer = entityCommandBufferSingleton
                    .CreateCommandBuffer(state.WorldUnmanaged)
                    .AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieRiseJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;

        [BurstCompile]
        private void Execute(ZombieRiseAspect zombie, [EntityInQueryIndex] int sortKey)
        {
            zombie.Rise(DeltaTime);

            if (!zombie.IsAboveGround) return;

            zombie.SetAtGroundLevel();
            EntityCommandBuffer.RemoveComponent<ZombieRiseRate>(sortKey, zombie.Entity);
            EntityCommandBuffer.SetComponentEnabled<ZombieWalkProperty>(sortKey, zombie.Entity, true);
        }
    }
}
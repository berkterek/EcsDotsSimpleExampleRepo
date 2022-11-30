using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace EcsDotsDeneme
{
    [BurstCompile]
    public partial struct SpawnZombieSystem : ISystem
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
            var createSingletonBuffer = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new SpawnZombieJob()
            {
                DeltaTime = deltaTime,
                EntityCommandBuffer = createSingletonBuffer.CreateCommandBuffer(state.WorldUnmanaged)
            }.Run();
        }
    }

    [BurstCompile]
    public partial struct SpawnZombieJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBuffer;

        private void Execute(GraveyardAspect graveyardAspect)
        {
            graveyardAspect.ZombieSpawnTimer -= DeltaTime;

            if (!graveyardAspect.TimeToSpawnZombie) return;
            if (graveyardAspect.ZombieSpawnPoints.Length.Equals(0)) return;

            graveyardAspect.ZombieSpawnTimer = graveyardAspect.ZombieSpawnRate;
            var entity = EntityCommandBuffer.Instantiate(graveyardAspect.ZombiePrefab);

            var newZombieTransform = graveyardAspect.GetRandomZombieTransform();
            EntityCommandBuffer.SetComponent(entity, new LocalToWorldTransform {Value = newZombieTransform});

            var zombieHeading = MathHelper.GetHeading(newZombieTransform.Position, graveyardAspect.Position);
            EntityCommandBuffer.SetComponent(entity, new ZombieHeading{Value = zombieHeading});
        }
    }
}
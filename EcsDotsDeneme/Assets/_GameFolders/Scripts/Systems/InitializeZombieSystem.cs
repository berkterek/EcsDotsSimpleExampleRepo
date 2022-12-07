using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace EcsDotsDeneme
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitializeZombieSystem : ISystem
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
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach (var zombie in SystemAPI.Query<ZombieWalkAspect>().WithAll<NewZombieTag>())
            {
                entityCommandBuffer.RemoveComponent<NewZombieTag>(zombie.Entity);
                entityCommandBuffer.SetComponentEnabled<ZombieWalkProperty>(zombie.Entity,false);
                entityCommandBuffer.SetComponentEnabled<ZombieEatProperty>(zombie.Entity, false);
            }
            
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
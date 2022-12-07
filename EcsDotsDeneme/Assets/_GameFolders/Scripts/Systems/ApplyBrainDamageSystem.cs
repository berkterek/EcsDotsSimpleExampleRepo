using Unity.Burst;
using Unity.Entities;

namespace EcsDotsDeneme
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct ApplyBrainDamageSystem : ISystem
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
            state.Dependency.Complete();
            foreach (var brainAspect in SystemAPI.Query<BrainAspect>())
            {
                brainAspect.DamageBrain();
            }
        }
    }
}
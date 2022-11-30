using Unity.Entities;

namespace EcsDotsDeneme
{
    public struct ZombieWalkProperty : IComponentData, IEnableableComponent
    {
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;
    }
}
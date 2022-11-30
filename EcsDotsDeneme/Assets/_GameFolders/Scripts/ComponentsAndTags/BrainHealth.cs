using Unity.Entities;

namespace EcsDotsDeneme
{
    public struct BrainHealth : IComponentData
    {
        public float Value;
        public float Max;
    }
}
using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsDeneme
{
    public struct GraveyardRandom : IComponentData
    {
        public Random Value;
    }
}
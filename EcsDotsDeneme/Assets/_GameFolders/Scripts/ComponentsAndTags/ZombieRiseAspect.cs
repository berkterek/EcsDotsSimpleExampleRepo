using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsDeneme
{
    public readonly partial struct ZombieRiseAspect : IAspect
    {
        public readonly Entity Entity;

        readonly TransformAspect _transformAspect;
        readonly RefRO<ZombieRiseRate> _zombieRiseRateRO;

        public bool IsAboveGround => _transformAspect.Position.y >= 0f;

        public void SetAtGroundLevel()
        {
            var position = _transformAspect.Position;
            position.y = 0f;
            _transformAspect.Position = position;
        }

        public void Rise(float deltaTime)
        {
            _transformAspect.Position += math.up() * _zombieRiseRateRO.ValueRO.Value * deltaTime;
        }
    }
}
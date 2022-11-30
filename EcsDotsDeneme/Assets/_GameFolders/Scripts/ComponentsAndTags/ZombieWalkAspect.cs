using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsDeneme
{
    public readonly partial struct ZombieWalkAspect : IAspect
    {
        readonly TransformAspect _transformAspect;
        readonly RefRW<ZombieTimer> _zombieTimerRW;
        readonly RefRO<ZombieWalkProperty> _zombieWalkPropertyRO;
        readonly RefRO<ZombieHeading> _zombieHeadingRO; 

        public readonly Entity Entity;

        public float WalkSpeed => _zombieWalkPropertyRO.ValueRO.WalkSpeed;
        public float WalkAmplitude => _zombieWalkPropertyRO.ValueRO.WalkAmplitude;
        public float WalkFrequency => _zombieWalkPropertyRO.ValueRO.WalkFrequency;
        public float Heading => _zombieHeadingRO.ValueRO.Value;

        public float WalkTimer
        {
            get => _zombieTimerRW.ValueRO.Value;
            set => _zombieTimerRW.ValueRW.Value = value;
        }

        public void Walk(float deltaTime)
        {
            WalkTimer += deltaTime;
            _transformAspect.Position += _transformAspect.Forward * WalkSpeed * deltaTime;

            var swayAngle = WalkAmplitude * math.sin(WalkFrequency * WalkTimer);
            _transformAspect.Rotation = quaternion.Euler(0, Heading, swayAngle);
        }

        public bool IsInStoppingRange(float3 brainPosition, float brainRadiusSq)
        {
            return math.distancesq(brainPosition, _transformAspect.Position) <= brainRadiusSq;
        }
    }
}
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EcsDotsDeneme
{
    public readonly partial struct ZombieEatAspect : IAspect
    {
        readonly TransformAspect _transformAspect;
        readonly RefRW<ZombieTimer> _zombieTimerRW;
        readonly RefRO<ZombieEatProperty> _zombieEatPropertyRO;
        readonly RefRO<ZombieHeading> _zombieHeadingRO;

        public readonly Entity Entity;

        public float EatDamagePerSecond => _zombieEatPropertyRO.ValueRO.EatDamagePerSecond;
        public float EatAmplitude => _zombieEatPropertyRO.ValueRO.EatAmplitude;
        public float EatFrequency => _zombieEatPropertyRO.ValueRO.EatFrequency;
        public float Heading => _zombieHeadingRO.ValueRO.Value;

        public float ZombieTimer
        {
            get => _zombieTimerRW.ValueRO.Value;
            set => _zombieTimerRW.ValueRW.Value = value;
        }

        public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter entityCommandBuffer, int sortKey, Entity brainEntity)
        {
            ZombieTimer += deltaTime;
            var eatAngle = EatAmplitude * math.sin(EatFrequency * ZombieTimer);
            _transformAspect.Rotation = Quaternion.Euler(eatAngle, Heading, 0f);

            var eatDamage = EatDamagePerSecond * deltaTime;
            var curBrainDamage = new BrainDamageBufferElement() { Value = eatDamage };
            entityCommandBuffer.AppendToBuffer(sortKey,brainEntity, curBrainDamage);
        }

        public bool IsInEatingRange(float3 brainPosition, float brainRadiusSq)
        {
            return math.distancesq(brainPosition, _transformAspect.Position) <= brainRadiusSq -1f;
        }
    }
}
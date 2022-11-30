using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsDeneme
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        const float BRAIN_SAFETY_RADIUS_SQ = 100f;

        readonly TransformAspect _transformAspect;
        readonly RefRO<GraveyardProperty> _graveyardPropertyRO;
        readonly RefRW<GraveyardRandom> _graveyardRandomRW;
        readonly RefRW<ZombieSpawnPoint> _zombieSpawnPointsRW;
        readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimerRW;

        float3 MinCorner => _transformAspect.Position - HalfDimensions;
        float3 MaxCorner => _transformAspect.Position + HalfDimensions;

        float3 HalfDimensions => new()
        {
            x = _graveyardPropertyRO.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = _graveyardPropertyRO.ValueRO.FieldDimensions.y * 0.5f
        };

        public float ZombieSpawnTimer
        {
            get => _zombieSpawnTimerRW.ValueRO.Value;
            set => _zombieSpawnTimerRW.ValueRW.Value = value;
        }

        public NativeArray<float3> ZombieSpawnPoints
        {
            get => _zombieSpawnPointsRW.ValueRO.Value;
            set => _zombieSpawnPointsRW.ValueRW.Value = value;
        }

        public float3 Position => _transformAspect.Position;
        public bool TimeToSpawnZombie => ZombieSpawnTimer <= 0f;
        public readonly Entity Entity;

        public int NumberTombstonesToSpawn => _graveyardPropertyRO.ValueRO.NumberTombstonesToSpawn;
        public Entity TombstonePrefab => _graveyardPropertyRO.ValueRO.TombstonePrefab;
        public Entity ZombiePrefab => _graveyardPropertyRO.ValueRO.ZombiePrefab;
        public float ZombieSpawnRate => _graveyardPropertyRO.ValueRO.ZombieSpawnRate;

        public UniformScaleTransform GetRandomTombstoneTransform() =>
            new UniformScaleTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale(0.5f)
            };

        public UniformScaleTransform GetRandomZombieTransform()
        {
            var position = GetRandomZombieSpawnPoint();
            
            return new UniformScaleTransform
            {
                Position = GetRandomZombieSpawnPoint(),
                Rotation = quaternion.RotateY(MathHelper.GetHeading(position,_transformAspect.Position)),
                Scale = 1f
            };
        }
            

        private float3 GetRandomPosition()
        {
            float3 randomPosition;

            do
            {
                randomPosition = _graveyardRandomRW.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(_transformAspect.Position, randomPosition) <= BRAIN_SAFETY_RADIUS_SQ);

            return randomPosition;
        }

        private quaternion GetRandomRotation() =>
            quaternion.RotateY(_graveyardRandomRW.ValueRW.Value.NextFloat(-0.25f, 0.25f));

        private float GetRandomScale(float min) => _graveyardRandomRW.ValueRW.Value.NextFloat(min, 1f);

        private float3 GetRandomZombieSpawnPoint()
        {
            return ZombieSpawnPoints[_graveyardRandomRW.ValueRW.Value.NextInt(ZombieSpawnPoints.Length)];
        }
    }
}
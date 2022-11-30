using Unity.Entities;
using Unity.Transforms;

namespace EcsDotsDeneme
{
    public readonly partial struct BrainAspect : IAspect
    {
        readonly TransformAspect _transformAspect;
        readonly RefRW<BrainHealth> _brainHealthRW;
        readonly DynamicBuffer<BrainDamageBufferElement> _brainDamageBufferDynamic;

        public readonly Entity Entity;

        public void DamageBrain()
        {
            foreach (var brainDamageBufferElement in _brainDamageBufferDynamic)
            {
                _brainHealthRW.ValueRW.Value -= brainDamageBufferElement.Value;
            }
            
            _brainDamageBufferDynamic.Clear();

            var localToWorld = _transformAspect.LocalToWorld;
            localToWorld.Scale = 10f*(_brainHealthRW.ValueRO.Value / _brainHealthRW.ValueRO.Max);
            _transformAspect.LocalToWorld = localToWorld;
        }
    }
}
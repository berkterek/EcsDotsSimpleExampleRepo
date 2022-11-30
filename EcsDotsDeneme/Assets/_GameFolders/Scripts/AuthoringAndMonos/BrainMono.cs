using Unity.Entities;
using UnityEngine;

namespace EcsDotsDeneme
{
    public class BrainMono : MonoBehaviour
    {
        public float Max;
    }
    
    public class BrainBaker : Baker<BrainMono>
    {
        public override void Bake(BrainMono authoring)
        {
            AddComponent<BrainTag>();
            AddComponent(new BrainHealth()
            {
                Max = authoring.Max,
                Value = authoring.Max
            });
           
            AddBuffer<BrainDamageBufferElement>();
        }
    }
}
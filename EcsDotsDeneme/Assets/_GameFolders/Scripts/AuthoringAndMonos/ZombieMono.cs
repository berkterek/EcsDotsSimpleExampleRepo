using Unity.Entities;
using UnityEngine;

namespace EcsDotsDeneme
{
    public class ZombieMono : MonoBehaviour
    {
        [Header("Rise Data")]
        public float RiseRate;
        
        [Header("Walk Data")]
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;
        
        [Header("Eat Data")]
        public float EatDamagePerSecond;
        public float EatAmplitude;
        public float EatFrequency;
    }
    
    public class ZombieBaker : Baker<ZombieMono>
    {
        public override void Bake(ZombieMono authoring)
        {
            AddComponent(new ZombieRiseRate
            {
                Value = authoring.RiseRate
            });
            
            AddComponent(new ZombieWalkProperty()
            {
                WalkSpeed = authoring.WalkSpeed,
                WalkAmplitude = authoring.WalkAmplitude,
                WalkFrequency = authoring.WalkFrequency
            });
            
            AddComponent(new ZombieEatProperty
            {
                EatAmplitude = authoring.EatAmplitude,
                EatFrequency = authoring.EatFrequency,
                EatDamagePerSecond = authoring.EatDamagePerSecond
            });
            
            AddComponent<ZombieTimer>();
            AddComponent<ZombieHeading>();
            AddComponent<NewZombieTag>();
        }
    }
}
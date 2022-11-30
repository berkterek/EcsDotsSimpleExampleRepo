using UnityEngine;

namespace EcsDotsDeneme
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float _startRadius;
        [SerializeField] float _endRadius;
        [SerializeField] float _startHeight;
        [SerializeField] float _endHeight;
        [SerializeField] float _speed;
        
        public static CameraController Instance { get; private set; }

        public float Speed => _speed;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public float RadiusAtScale(float scale) => Mathf.Lerp(_startRadius, _endRadius, 1f - scale);
        public float HeightAtScale(float scale) => Mathf.Lerp(_startHeight, _endHeight, 1f - scale);
    }    
}


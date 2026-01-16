using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Babies
{
    [CreateAssetMenu(menuName = "Baby/Personality")]
    public class BabyPersonality : ScriptableObject
    {
        public float hungerRate = 1.0f;
        public float comfortDecay = 1.0f;
        public float cryTolerance = 1.0f;
        public float immunity = 0.5f;
    }
}
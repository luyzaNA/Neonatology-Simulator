using UnityEngine;

namespace Assets.Scripts.Cribs
{
    public class Crib : MonoBehaviour
    {

        public BabyController babyInCrib;

        public void PlaceBabyInCrib(BabyController baby)
        {
            babyInCrib = baby;
            baby.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            baby.transform.parent = transform;
            baby.SetInIncubator(true);
        }
    }
}
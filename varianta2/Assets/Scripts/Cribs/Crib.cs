using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Cribs
{
    public class Crib : MonoBehaviour
    {

        public Babies.Baby babyInCrib;

        public void PlaceBabyInCrib(Babies.Baby baby)
        {
            babyInCrib = baby;
            baby.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            baby.transform.parent = transform;
            baby.SetInCrib(true);
        }
    }
}
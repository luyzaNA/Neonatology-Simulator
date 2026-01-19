using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Cribs
{
    public class Bed : MonoBehaviour
    {

        public BabyController babyInBed;
        public Transform carryPoint;

        public void PlaceBabyInBed(BabyController baby)
        {
            babyInBed = baby;
            baby.transform.SetParent(carryPoint);
            baby.transform.localPosition = Vector3.zero;
            baby.transform.localRotation = Quaternion.identity;

            baby.SetInBed(true);
        }
    }
}
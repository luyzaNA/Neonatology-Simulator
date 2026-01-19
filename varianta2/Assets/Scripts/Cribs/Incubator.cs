using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Cribs
{
    public class Incubator : MonoBehaviour
    {

        public BabyController babyInIncubator;
        public Transform carryPoint;

        public void PlaceBabyIncubator(BabyController baby)
        {
            babyInIncubator = baby;
            baby.transform.SetParent(carryPoint);
            baby.transform.localPosition = Vector3.zero;
            baby.transform.localRotation = Quaternion.identity;

            baby.SetInIncubator(true);
        }
    }
}
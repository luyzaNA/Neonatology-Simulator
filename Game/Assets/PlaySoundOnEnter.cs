using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Undercooked
{
    public class PlaySoundOnEnter : MonoBehaviour
    {
        AudioSource source;
        BoxCollider collider;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            source.Play();
        }
        private void OnTriggerExit(Collider other)
        {
            source.Stop();
        }
    }
}

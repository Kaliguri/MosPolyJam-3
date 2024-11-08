using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExtentions : MonoBehaviour
{
    [SerializeField] bool DestoryWhenStopped = false;
    private void Start()
    {
        var particleSystem = GetComponent<ParticleSystem>();

        var main = particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;

    }
    public void OnParticleSystemStopped()
    {
        if (DestoryWhenStopped)
        {
            Destroy(gameObject.transform.parent.gameObject);
            //Debug.Log("Destroy");
        }
    }
}

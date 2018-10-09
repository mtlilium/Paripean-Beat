using UnityEngine;
using System.Collections;
//FOR TEST
using UnityEngine.Events;

public class ParticleManager : MonoBehaviour {

    //パーティクルを指定
    [SerializeField]
    ParticleSystem[] particle;

    void Awake()
    {
        StartCoroutine(testLoop(2f,()=>{
			foreach(ParticleSystem p in particle){
            p.Simulate(0.0f,true,true);
            p.Play();
			}
        }));
    }

    //FOR TEST
    IEnumerator testLoop(float waitTme,UnityAction action)
    {
        var waitTime = new WaitForSeconds(waitTme);
        while (true){
            yield return waitTime;
            action ();
        }
    }
}﻿

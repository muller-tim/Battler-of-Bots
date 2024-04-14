using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ParticleSystemExecutor : MonoBehaviour
{
    public List<ParticleSystem> startParticleSystems = new List<ParticleSystem>();
    public List<ParticleSystem> pushBackParticleSystems = new List<ParticleSystem>();
    public AudioSource whooshSound;


    public void PlayStartParticle()
    {
        
        whooshSound.Play();
        foreach(ParticleSystem particleSystem in startParticleSystems)
        {
            particleSystem.Play();
        }
    }
    public void StartPushBackParticleSystem()
    {
        whooshSound.Play();
        foreach(ParticleSystem particleSystem in pushBackParticleSystems)
        {
            particleSystem.Play();
        }
    }

}

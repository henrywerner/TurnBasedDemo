using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager current;
    public bool animationInProgress;
    // private AudioClip _assultRifleSound;
    [SerializeField] private AudioSource _shootSound, _reloadSound, _emptySound, _moveSound, _hurtSound;
    [SerializeField] private TrailRenderer tracerEffect;
    [SerializeField] private ParticleSystem bulletImpactEnv, bulletImpactPerson;

    void Awake() {
        current = this;
        //_assultRifleSource = GetComponent<AudioSource>();
        animationInProgress = false;
    }

    public void FireShots(List<RaycastHit> shots, float shotDelay, Soldier shooter)
    {
        animationInProgress = true;
        List<RaycastHit> shotsCache = new List<RaycastHit>();
        foreach (var shot in shots)
        {
            shotsCache.Add(shot);
        }
        StartCoroutine(burstFireAnimation(shotsCache, shotDelay, shooter));
    }

    public void PlayMoveSound()
    {
        _moveSound.Play();
    }

    public void PlayReloadSound()
    {
        _reloadSound.Play();
    }

    private IEnumerator burstFireAnimation(List<RaycastHit> shots, float shotDelay, Soldier shooter)
    {
        int shotsHit = 0;
        foreach (var shot in shots)
        {
            // debug print what was hit
            //Debug.Log("Target hit: " + shot.transform.name);
            
            if (shooter.AmmoCount == 0)
            {
                // play gun empty sound
                shooter.evtFeed.AddEvent("Out of Ammo");
                _emptySound.Play();
                continue;
            }
            
            // remove 1 ammo from shooter
            shooter.DepleteAmmo();
            
            // play sound effect
            _shootSound.Play();
            
            // muzzle flash?
            
            // set up tracer values
            Vector3 gunBarrel = shooter.gameObject.transform.position;
            gunBarrel.y = 1.25f;
            TrailRenderer tracer = Instantiate(tracerEffect, gunBarrel, Quaternion.identity);

            // check if hit enemy
            Soldier s = shot.transform.gameObject.GetComponent<Soldier>();
            if (s != null)
            {
                // draw hit tracer
                StartCoroutine(SpawnTracer(tracer, shot, bulletImpactPerson));
                
                shotsHit++;
                s.Damage();
                s.evtFeed.AddEvent("-1 Health");
                // Play soldier hurt noise (unless that should be handled in the soldiers damage method??)
                _hurtSound.Play();
                
                // Track if this is a new kill
                if (s.IsDead)
                    shooter.evtFeed.AddEvent("Target Killed");
                
                // soldier specific impact particles
                Debug.DrawLine(gameObject.transform.position, shot.point, Color.cyan, 3f);
                
                // add in additional math for collateral shots here
            }
            else
            {
                // draw miss tracer
                StartCoroutine(SpawnTracer(tracer, shot, bulletImpactEnv));
                
                // impact particles
                Debug.DrawLine(gameObject.transform.position, shot.point, Color.grey, 3f);
            }

            yield return new WaitForSecondsRealtime(shotDelay);
        }

        animationInProgress = false;
        Debug.Log("Shots Hit: " + shotsHit);
        if (shotsHit == 0)
            shooter.evtFeed.AddEvent("Total Miss");
        
        yield return null;
    }

    private IEnumerator SpawnTracer(TrailRenderer trail, RaycastHit hit, ParticleSystem impact)
    {
        float time = 0, duration = trail.time;
        Vector3 startPos = trail.transform.position;

        while (time < duration)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal)); // play hit particles
        Destroy(trail.gameObject, trail.time); // destroy trail after time has expired
    }
}

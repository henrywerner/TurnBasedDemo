using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;

public class GunFireAnimator : MonoBehaviour
{
    public static GunFireAnimator current;
    public bool animationInProgress;
    // private AudioClip _assultRifleSound;
    private AudioSource _assultRifleSource;

    void Awake() {
        current = this;
        _assultRifleSource = GetComponent<AudioSource>();
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

    private IEnumerator burstFireAnimation(List<RaycastHit> shots, float shotDelay, Soldier shooter)
    {
        int shotsHit = 0;
        foreach (var shot in shots)
        {
            // play sound effect
            _assultRifleSource.Play();
            
            // muzzle flash?

            // check if hit enemy
            Soldier s = shot.transform.gameObject.GetComponent<Soldier>();
            if (s != null)
            {
                shotsHit++;
                s.Damage();
                s.evtFeed.AddEvent("-1 Health");
                // Play soldier hurt noise (unless that should be handled in the soldiers damage method??)
                
                // Track if this is a new kill
                if (s.IsDead)
                    shooter.evtFeed.AddEvent("Target Killed");
                
                // soldier specific impact particles
                Debug.DrawLine(gameObject.transform.position, shot.point, Color.cyan, 3f);
                
                // add in additional math for collateral shots here
            }
            else
            {
                // impact particles
                Debug.DrawLine(gameObject.transform.position, shot.point, Color.grey, 3f);
            }

            yield return new WaitForSecondsRealtime(shotDelay);
        }

        animationInProgress = false;
        Debug.Log("Shots Hit: " + shotsHit);
        yield return null;
    }
}

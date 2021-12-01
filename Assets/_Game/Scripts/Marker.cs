using System;
using System.Collections;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public static Marker current;
    public Action<int, int> OnMoveMarker;
    [SerializeField] private GameObject _markerGO;
    

    private void Awake()
    {
        current = this;
        OnMoveMarker += MoveMarker;
    }

    private void Start()
    {
        _markerGO.SetActive(false);
    }

    private void MoveMarker(int x, int z)
    {
        gameObject.transform.position = new Vector3(x, 0, z);
        StartCoroutine(SizePulse());
    }

    private IEnumerator SizePulse()
    {
        float duration = 0.4f, time = 0;
        float startScale = 1, endScale = 0.5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            
            float t = time / duration;
            t = t * t * (3f - 2f * t);
            
            float scale = Mathf.Lerp(startScale, endScale, t);
            _markerGO.transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        _markerGO.transform.localScale = new Vector3(endScale, endScale, 1);
    }

    public void SetVisable(bool isVisable)
    {
        _markerGO.SetActive(isVisable);
    }
}

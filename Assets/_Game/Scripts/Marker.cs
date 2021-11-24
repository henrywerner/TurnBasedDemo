using System;
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
    }

    public void SetVisable(bool isVisable)
    {
        _markerGO.SetActive(isVisable);
    }
}

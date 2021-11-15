using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    public static SelectionIndicator current;
    public Action<int, int> OnMoveIndicator;
    [SerializeField] private GameObject _highlight;

    private void Awake()
    {
        current = this;
        OnMoveIndicator += MoveIndicator;
    }

    private void Start()
    {
        _highlight.SetActive(false);
    }

    private void MoveIndicator(int x, int z)
    {
        gameObject.transform.position = new Vector3(x, 0, z);
    }

    public void Highlight(bool isVisable)
    {
        _highlight.SetActive(isVisable);
    }
}

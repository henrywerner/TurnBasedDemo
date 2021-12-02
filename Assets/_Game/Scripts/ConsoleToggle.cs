using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleToggle : MonoBehaviour
{
    [SerializeField] private TMP_Text console;
    private bool isVisible = false;
    
    void Start()
    {
        console.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isVisible = !isVisible;
            console.enabled = isVisible;
        }
    }
}

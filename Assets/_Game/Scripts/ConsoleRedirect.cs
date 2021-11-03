using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleRedirect : MonoBehaviour
{
    //[SerializeField] private Text _text;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        Application.logMessageReceived += Log;
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        _text.text += logString + "\n";
    }

}

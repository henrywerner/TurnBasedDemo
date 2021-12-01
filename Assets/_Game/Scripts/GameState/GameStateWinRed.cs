using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateWinRed : IState
{
    public void Enter()
    {
        Debug.Log("State Entered: Win Red");
        Debug.Log("<color=#fdbb43>PRESS [R] Restart; [M] Main menu</color>");
        
        HUD.qt.SetApHeaderText("<color=#ec4b4c>RED TEAM WINS</color> \n<color=#fdbb43>PRESS [R] Restart; [M] Main menu</color>");
        HUD.qt.SetApSubheaderText("");
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(1);
        else if (Input.GetKeyDown(KeyCode.M))
            SceneManager.LoadScene(0);
    }

    public void FixedTick()
    {
    }

    public void Exit()
    {
    }
}
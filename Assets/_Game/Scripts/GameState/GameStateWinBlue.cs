using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateWinBlue: IState
{
    public void Enter()
    {
        Debug.Log("State Entered: Win Blue");
        Debug.Log("<color=#fdbb43>PRESS [R] Restart; [M] Main menu</color>");
        
        HUD.qt.SetApHeaderText("<color=#46a8e0>BLUE TEAM WINS</color> \n<color=#fdbb43>PRESS [R] Restart; [M] Main menu</color>");
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
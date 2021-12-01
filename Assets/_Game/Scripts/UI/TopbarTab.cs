using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class TopbarTab : MonoBehaviour
    {
        public Image portrait, teamIndicator, action1, action2;
        public GameObject hurtBar, deathOverlay, turnIndicator;
        public TMP_Text respawnCounter;

        private Color _actionUnused = Color.white;
        private Color _actionUsed = Color.gray;
        private Color _blueTeam = new Color(70 / 255f, 168 / 255f, 224 / 255f);
        private Color _redTeam = new Color(236 / 255f, 75 / 255f, 76 / 255f);

        public void SetActionsRemaining(int a)
        {
            action1.color = a > 0 ? _actionUnused : _actionUsed;
            action2.color = a > 1 ? _actionUnused : _actionUsed;
        }

        public void SetTeamColor(bool isRedTeam)
        {
            teamIndicator.color = isRedTeam ? _redTeam : _blueTeam;
        }

        public void SetPortrait(Sprite s)
        {
            portrait.sprite = s;
        }

        public void UpdateHealth(int current, int max)
        {
            float i = 1 - (float)current / max;
            Vector3 v = new Vector3(1, i, 1);
            hurtBar.transform.localScale = v;
        }

        public void SetDead(bool isDead)
        {
            deathOverlay.SetActive(isDead);
        }

        public void UpdateRespawnCounter(int i)
        {
            respawnCounter.text = i + "";
        }

        public void SetTurnIndicator(bool isVisible)
        {
            turnIndicator.SetActive(isVisible);
        }
    }
}
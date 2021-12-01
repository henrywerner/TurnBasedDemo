using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using _Game.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD qt;
    
    [Header("Top Bar")]
    [SerializeField] private GameObject _tabPrefab;
    [SerializeField] private GameObject _blueTeamContainer, _redTeamContainer;

    [Header("Soldier Detail Panel")]
    [SerializeField] private Image dpPortrait;
    [SerializeField] private Image dpHealthBar;
    [SerializeField] private TMP_Text dpName, dpAmmoClip, dpAmmoReserve, dpHP, dpMaxHP;

    [Header("Action Panel")] 
    [SerializeField] private TMP_Text apHeader;
    [SerializeField] private TMP_Text apSubheader;
    [SerializeField] private GameObject apControls, apActionButtons;
    public Action OnMoveButtonPress, OnAttackButtonPress, OnReloadButtonPress;
    
    private Dictionary<Soldier, TopbarTab> _combatants;
    private TopbarTab _previousTurn = null;
    private Soldier _currentTurn;

    private void Awake()
    {
        qt = this;

        _combatants = new Dictionary<Soldier, TopbarTab>();
    }

    public void MoveButtonPress()
    {
        OnMoveButtonPress?.Invoke();
    }

    public void AttackButtonPress()
    {
        OnAttackButtonPress?.Invoke();
    }

    public void ReloadButtonPress()
    {
        OnReloadButtonPress?.Invoke();
    }
    
    // build new tab based on soldier data
    public void BuildSoldierTab(Soldier s)
    {
        // instantiate obj. Set correct parent obj based on team color
        GameObject g = Instantiate(_tabPrefab,
            (int)s.Team == 0 ? _blueTeamContainer.transform : _redTeamContainer.transform);
        TopbarTab t = g.GetComponent<TopbarTab>();

        // set profile icon
        t.SetPortrait(s.charPortrait);

        // set hurt indicator default
        t.UpdateHealth(s.CurrentHP, s.MaxHP);

        // set death indicator default
        t.SetDead(false);

        // set action indicators default
        t.SetActionsRemaining(2);

        // set current turn indicator default
        t.SetTurnIndicator(false);
        
        // set team color
        t.SetTeamColor((int)s.Team == 1);

        // add new entry to dictionary
        _combatants.Add(s, t);
    }
    
    // update soldier health
    public void UpdateSoldierHealth(Soldier s)
    {
        // Update top bar
        if (_combatants.ContainsKey(s))
            _combatants[s].UpdateHealth(s.CurrentHP, s.MaxHP);
        
        // Update the soldier hover display
        
        // Update the detail panel if needed
        if (_currentTurn == s)
            SetSoldierDetailPanel(s); 
            // there isn't a way for a soldier to get hurt on its turn? so this can be un-optimal
    }
    
    // update soldier ammo
    public void UpdateSoldierAmmo(Soldier s)
    {
        if (_currentTurn == s)
            dpAmmoClip.text = s.AmmoCount + "";
    }

    public void UpdateSoldierDead(Soldier s)
    {
        if (_combatants.ContainsKey(s))
            _combatants[s].SetDead(s.IsDead);
    }

    public void UpdateSoldierRespawnCounter(Soldier s)
    {
        if (_combatants.ContainsKey(s))
            _combatants[s].UpdateRespawnCounter(s.RespawnTimer);
    }
    
    // update soldier actions
    public void SetActionsRemaining(Soldier s, int a)
    {
        // Update top bar
        if (_combatants.ContainsKey(s))
            _combatants[s].SetActionsRemaining(a);
        
        // Update bottom left display
        if (_currentTurn == s)
            apSubheader.text = "Unit Actions Remaining " + a + "/2";
    }
    
    // update current selected
    public void SetSoldierTurn(Soldier s)
    {
        // remove the highlighted bg from prev soldier on top bar
        if (_previousTurn != null)
            _previousTurn.SetTurnIndicator(false);

        if (_combatants.ContainsKey(s))
        {
            _combatants[s].SetTurnIndicator(true);
            _previousTurn = _combatants[s];
            _currentTurn = s;
        }
        else
            Debug.Log("Unable to highlight tab; Soldier not found in dictionary.");
        
        // Update the detail panel
        SetSoldierDetailPanel(s);
    }

    private void SetSoldierDetailPanel(Soldier s)
    {
        // set name header
        dpName.text = s.name;

        // set portrait
        dpPortrait.sprite = s.charPortrait;

        // set weapon info
        dpAmmoClip.text = s.AmmoCount + "";
        dpAmmoReserve.text = s.MaxAmmo + "";

        // set health info
        dpHP.text = s.CurrentHP + "";
        dpMaxHP.text = s.MaxHP + "";
        
        // set health bar
        float x = (float)s.CurrentHP / s.MaxHP;
        dpHealthBar.transform.localScale = new Vector3(x, 1, 1);
    }

    public void ShowActionButtons(bool isVisible)
    {
        apActionButtons.SetActive(isVisible);
    }

    public void ShowControls(bool isVisible)
    {
        apControls.SetActive(isVisible);
    }

    public void SetApHeaderText(string s)
    {
        apHeader.text = s;
    }

    public void SetApSubheaderText(string s)
    {
        apSubheader.text = s;
    }
}

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class BetweenInnings : MonoBehaviour {

    public List<InputField> suggestions = new List<InputField>();
    public GameObject betweenInningUI;
    public Text suggestionsText, randomGameText;
    public List<string> randomGames = new List<string>();
    public Text hTeamInning1Score, hTeamInning2Score, hTeamInning3Score, aTeamInning1Score, aTeamInning2Score, aTeamInning3Score;
    public Text hTeamRuns, hTeamHits, hTeamPitches, aTeamRuns, aTeamHits, aTeamPitches;

    private void Start()
    {
        //turn off input listening for the numbers
        HandleInput.listenForHits = false;
        foreach (var suggestion in suggestions)
        {
            suggestion.text = "";
        }
    }

    public void EnableBetweenInningsUI()
    {
        betweenInningUI.SetActive(true);
        Enable();
    }

    private void Enable()
    {
        HandleInput.listenForHits = false;
        foreach (var suggestion in suggestions)
        {
            suggestion.text = "";
        }
        if(GameControl.curInning.inningNumber != 3)
        {
            randomGameText.text = randomGames[UnityEngine.Random.Range(0, randomGames.Count - 2)];
        } else
        {
            randomGameText.text = randomGames[4];
        }
        SetUpScoreboardText();
    }

    void Update () {
        if (Controls.GetButtonDown("escape") && betweenInningUI.activeSelf)
        {
            suggestionsText.text = "";
            GameControl.curInning.suggestions.Clear();
            int i = 0;
            foreach (var suggestion in suggestions)
            {
                if(suggestion.textComponent.color.a == 1)
                {
                    suggestionsText.text += i == 0 ? "-" + suggestion.text : Environment.NewLine + "-" + suggestion.text;
                    i++;
                }
            }
            betweenInningUI.SetActive(false);
            HandleInput.listenForHits = true;
        } else if (Controls.GetButtonDown("hit1"))
        {
            Color col = suggestions[0].textComponent.color;
            col.a = col.a < 1 ? 1 : .4f;
            suggestions[0].textComponent.color = col;
        }
        else if (Controls.GetButtonDown("hit2"))
        {
            Color col = suggestions[1].textComponent.color;
            col.a = col.a < 1 ? 1 : .4f;
            suggestions[1].textComponent.color = col;
        }
        else if (Controls.GetButtonDown("hit3"))
        {
            Color col = suggestions[2].textComponent.color;
            col.a = col.a < 1 ? 1 : .4f;
            suggestions[2].textComponent.color = col;
        }
        else if (Controls.GetButtonDown("hit4"))
        {
            Color col = suggestions[3].textComponent.color;
            col.a = col.a < 1 ? 1 : .4f;
            suggestions[3].textComponent.color = col;
        }
        else if (Controls.GetButtonDown("hit5"))
        {
            Color col = suggestions[4].textComponent.color;
            col.a = col.a < 1 ? 1 : .4f;
            suggestions[4].textComponent.color = col;
        }
        else if (Controls.GetButtonDown("hit6"))
        {
            Color col = suggestions[5].textComponent.color;
            col.a = col.a < 1 ? 1 : .4f;
            suggestions[5].textComponent.color = col;
        }
        else if (Controls.GetButtonDown("hit7"))
        {
            Color col = suggestions[6].textComponent.color;
            col.a = col.a < 1 ? 1 : .4f;
            suggestions[6].textComponent.color = col;
        }
    }

    private void SetUpScoreboardText()
    {
        hTeamInning1Score.text = GameControl.instance.activeTeams[0].scoreByInning[0].ToString();
        hTeamInning2Score.text = GameControl.instance.activeTeams[0].scoreByInning[1].ToString();
        hTeamInning3Score.text = GameControl.instance.activeTeams[0].scoreByInning[2].ToString();
        aTeamInning1Score.text = GameControl.instance.activeTeams[1].scoreByInning[0].ToString();
        aTeamInning2Score.text = GameControl.instance.activeTeams[1].scoreByInning[1].ToString();
        aTeamInning3Score.text = GameControl.instance.activeTeams[1].scoreByInning[2].ToString();
        hTeamHits.text = GameControl.instance.activeTeams[0].GetHits().ToString();
        aTeamHits.text = GameControl.instance.activeTeams[1].GetHits().ToString();
        hTeamRuns.text = GameControl.instance.activeTeams[0].score.ToString();
        aTeamRuns.text = GameControl.instance.activeTeams[1].score.ToString();
        aTeamPitches.text = GameControl.instance.activeTeams[0].GetPitches().ToString();
        hTeamPitches.text = GameControl.instance.activeTeams[1].GetPitches().ToString();
    }
}

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
    public Text hTeamAbr, aTeamAbr;
    public Color textColor;

    private void Start()
    {
        //turn off input listening for the numbers
        HandleInput.listenForHits = false;
        foreach (var suggestion in suggestions)
        {
            suggestion.text = "";
        }
        Enable();
    }

    public void EnableBetweenInningsUI()
    {
        Invoke("ActivateUI", 2f);
    }

    private void ActivateUI()
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
            suggestion.textComponent.color = textColor;
        }
        if(GameControl.curInning.inningNumber != 3)
        {
            int index = UnityEngine.Random.Range(0, randomGames.Count - 1);
            randomGameText.text = randomGames[index];
            randomGames.Remove(randomGames[index]);
        } else
        {
            randomGameText.text = randomGames[randomGames.Count - 1];
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
        } 
        else if (Controls.GetButtonDown("hit1"))
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
        ActiveTeam hTeam = GameControl.instance.activeTeams[0];
        ActiveTeam aTeam = GameControl.instance.activeTeams[1];
        hTeamInning1Score.text = hTeam.scoreByInning[0].ToString();
        hTeamInning2Score.text = hTeam.scoreByInning[1].ToString();
        hTeamInning3Score.text = hTeam.scoreByInning[2].ToString();
        aTeamInning1Score.text = aTeam.scoreByInning[0].ToString();
        aTeamInning2Score.text = aTeam.scoreByInning[1].ToString();
        aTeamInning3Score.text = aTeam.scoreByInning[2].ToString();
        hTeamHits.text = hTeam.GetHits().ToString();
        aTeamHits.text = aTeam.GetHits().ToString();
        hTeamRuns.text = hTeam.score.ToString();
        aTeamRuns.text = aTeam.score.ToString();
        aTeamPitches.text = aTeam.GetPitches().ToString();
        hTeamPitches.text = hTeam.GetPitches().ToString();
        aTeamAbr.text = aTeam.abbreviation;
        hTeamAbr.text = hTeam.abbreviation;
    }
}

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class HRDUI : MonoBehaviour {

    public List<InputField> suggestions = new List<InputField>();
    public GameObject hrdUI;
    public Text suggestionsText, randomGameText, leaderboardText;
    public List<string> randomGames = new List<string>();
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
        Invoke("ActivateUI", 4f);
    }

    private void ActivateUI()
    {
        hrdUI.SetActive(true);
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
    }

    void Update () {
        if (Controls.GetButtonDown("escape") && hrdUI.activeSelf)
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
            hrdUI.SetActive(false);
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
}

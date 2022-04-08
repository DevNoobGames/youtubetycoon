using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BayatGames.SaveGameFree;

public class SponsorManager : MonoBehaviour
{
    public AudioSource clickAudio;
    public AudioSource moneyAudio;

    public StatsManager statsMan;

    public TextMeshProUGUI sponsorTitle;
    public Button activateButton;
    public TextMeshProUGUI buttontext;
    public RawImage sponsorIcon;

    public Sponsors[] sponsorList;
    public int selectedSponsor = 0;

    [System.Serializable]
    public class Sponsors
    {
        public string SponsorName;
        public float SponsorReward;
        public Texture SponsorIcon;
        public float MinimalSubs;
        public bool active;
    }

    private void Start()
    {
        if (SaveGame.Exists("activeSponsor"))
        {
            int activ = SaveGame.Load<int>("activeSponsor");
            sponsorList[activ].active = true;
        }
        SetButtonAndText();
    }

    private void OnEnable()
    {
        SetButtonAndText();
    }

    public void nextSponsor()
    {
        if (selectedSponsor < (sponsorList.Length - 1))
        {
            selectedSponsor += 1;
        }
        else
        {
            selectedSponsor = 0;
        }

        SetButtonAndText();
        clickAudio.Play();

    }

    public void previousSponsor()
    {
        if (selectedSponsor > 0)
        {
            selectedSponsor -= 1;
        }
        else
        {
            selectedSponsor = (sponsorList.Length - 1);
        }
        SetButtonAndText();
        clickAudio.Play();

    }

    public void SetButtonAndText()
    {
        sponsorTitle.text = sponsorList[selectedSponsor].SponsorName + " - " + sponsorList[selectedSponsor].SponsorReward + "$ / video";
        sponsorIcon.texture = sponsorList[selectedSponsor].SponsorIcon;

        if (sponsorList[selectedSponsor].active)
        {
            activateButton.interactable = false;
            buttontext.text = "Active";
        }
        else if (statsMan.subscribers >= sponsorList[selectedSponsor].MinimalSubs)
        {
            activateButton.interactable = true;
            buttontext.text = "Activate";
        }
        else
        {
            activateButton.interactable = false;
            buttontext.text = "Unable";
        }
    }

    public void selectSponsor()
    {
        if (!sponsorList[selectedSponsor].active && statsMan.subscribers >= sponsorList[selectedSponsor].MinimalSubs)
        {
            for (int i = 0; i < sponsorList.Length; i++)
            {
                sponsorList[i].active = false;
            }

            sponsorList[selectedSponsor].active = true;
            SaveGame.Save<int>("activeSponsor", selectedSponsor);
            SetButtonAndText();
            moneyAudio.Play();
        }
    }

    public void addMoneyAfterVideo()
    {
        for (int i = 0; i < sponsorList.Length; i++)
        {
            if (sponsorList[i].active)
            {
                statsMan.addMoney(sponsorList[i].SponsorReward);
                return; //stops the whole function. Otherwise use break if something comes after his :)
            }
        }
    }
}


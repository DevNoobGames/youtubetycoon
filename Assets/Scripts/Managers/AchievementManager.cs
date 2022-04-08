using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BayatGames.SaveGameFree;

public class AchievementManager : MonoBehaviour
{
    public AudioSource achievementAudio;

    public StatsManager statsMan;
    public bool allSubs;
    public bool allMoney;

    public GameObject achievementPanelObj;
    public Animation achievementPanelAnim;

    public GameObject[] reward;
    public TextMeshProUGUI[] textColor;

    [Header("Counters")]
    public int videosCreated;

    private void Start()
    {
        if (SaveGame.Exists("VideosCreated"))
        {
            videosCreated = SaveGame.Load<int>("VideosCreated");
            videosCreated -= 1;
            addVideoCreated();
        }
        if (SaveGame.Exists("OwnedEverything"))
        {
            ownedAllUpgrades();
        }
        if (SaveGame.Exists("allsubsnMoney"))
        {
            reward[9].SetActive(true);
            textColor[9].color = Color.green;
        }
    }

    //Done in statsman
    public void checkSubsReward()
    {
        if (statsMan.subscribers >= 100 && !reward[0].activeInHierarchy)
        {
            reward[0].SetActive(true);
            textColor[0].color = Color.green;
            achievementAudio.Play();
        }
        if (statsMan.subscribers >= 200000 && !reward[1].activeInHierarchy)
        {
            reward[1].SetActive(true);
            textColor[1].color = Color.green;
            achievementAudio.Play();
        }
        if (statsMan.subscribers >= 300000000 && !reward[2].activeInHierarchy)
        {
            reward[2].SetActive(true);
            textColor[2].color = Color.green;
            achievementAudio.Play();
        }
    }

    //saved
    public void addVideoCreated()
    {
        videosCreated += 1;

        if (videosCreated >= 5 && !reward[3].activeInHierarchy)
        {
            reward[3].SetActive(true);
            textColor[3].color = Color.green;
            achievementAudio.Play();
        }
        if (videosCreated >= 50 && !reward[4].activeInHierarchy)
        {
            reward[4].SetActive(true);
            textColor[4].color = Color.green;
            achievementAudio.Play();
        }
        if (videosCreated >= 100 && !reward[5].activeInHierarchy)
        {
            reward[5].SetActive(true);
            textColor[5].color = Color.green;
            achievementAudio.Play();
        }

        SaveGame.Save<int>("VideosCreated", videosCreated);
    }

    //done in statsman
    public void checkBankMoney()
    {
        if (statsMan.money >= 500000 && !reward[6].activeInHierarchy)
        {
            reward[6].SetActive(true);
            textColor[6].color = Color.green;
            achievementAudio.Play();
        }
        if (statsMan.money >= 5000000000 && !reward[7].activeInHierarchy)
        {
            reward[7].SetActive(true);
            textColor[7].color = Color.green;
            achievementAudio.Play();
        }
    }

    //saved
    public void ownedAllUpgrades()
    {
        reward[8].SetActive(true);
        textColor[8].color = Color.green;
        achievementAudio.Play();
        SaveGame.Save<bool>("OwnedEverything", true);
    }

    //saved
    public void checkSubsAndMoney()
    {
        if (allMoney && allSubs && !reward[9].activeInHierarchy)
        {
            reward[9].SetActive(true);
            textColor[9].color = Color.green;
            achievementAudio.Play();
            SaveGame.Save<bool>("allsubsnMoney", true);
        }
    }

    public void openAchievementMenu()
    {
        achievementPanelAnim.Play("OpenSideBar");
    }

    public void closeAchievementMenu()
    {
        achievementPanelAnim.Play("CloseSideBar");
    }
}

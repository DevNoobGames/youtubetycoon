using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using BayatGames.SaveGameFree;

public class StatsManager : MonoBehaviour
{
    public SponsorManager sponsorMan;
    public PropertyManager propertyMan;
    public AchievementManager achievementMan;

    public float money = 200;
    public TextMeshProUGUI moneyText;

    public float subscribers = 0;
    public TextMeshProUGUI subText;

    public float days = 0;
    public TextMeshProUGUI daysText;

    public bool schoolEducated;
    public GameObject schoolEd;
    public bool collegeEducated;
    public GameObject collegeEd;
    public bool uniEducated;
    public GameObject uniEd;
    public bool ivyEducated;
    public GameObject ivyEd;

    public bool menuInteractable = true;

    public static readonly int charA = Convert.ToInt32('a');

    private void Start()
    {
        if (SaveGame.Exists("subscribers"))
        {
            float subscribersToAdd = SaveGame.Load<float>("subscribers");
            addSubscribers(subscribersToAdd);
        }
        else
        {
            SaveGame.Save<float>("subscribers", 0);
        }

        if (SaveGame.Exists("money"))
        {
            float moneyToAdd = SaveGame.Load<float>("money");
            addMoney(moneyToAdd - 200);
        }
        else
        {
            SaveGame.Save<float>("money", 200);
        }

        if (SaveGame.Exists("days"))
        {
            float daysToAdd = SaveGame.Load<float>("days");
            addDays(daysToAdd);
        }
        else
        {
            SaveGame.Save<float>("days", 0);
        }

        if (SaveGame.Exists("educated_school"))
        {
            schoolEd.SetActive(true);
        }
        if (SaveGame.Exists("educated_college"))
        {
            collegeEd.SetActive(true);
        }
        if (SaveGame.Exists("educated_uni"))
        {
            uniEd.SetActive(true);
        }
        if (SaveGame.Exists("educated_ivy"))
        {
            ivyEd.SetActive(true);
        }
    }

    public void addMoney(float Addedmoney)
    {
        money += Addedmoney;

        if (money >= 40000000000000)
        {
            money = 40000000000000;
            moneyText.text = "All the money in the world";
            achievementMan.allMoney = true;
            achievementMan.checkSubsAndMoney();
        }
        else
        {
            string moneyAbbreviated = FormatNumber(money);
            moneyText.text = moneyAbbreviated;
            achievementMan.allMoney = false;
        }

        SaveGame.Save<float>("money", money);
        achievementMan.checkBankMoney();

    }

    public void addSubscribers(float addedSubs)
    {
        subscribers += addedSubs;

        if (subscribers >= 8000000000)
        {
            subscribers = 8000000000;
            subText.text = "Everyone in the world subbed";
            achievementMan.allSubs = true;
            achievementMan.checkSubsAndMoney();
        }
        else
        {
            string subscribersAbbreviated = FormatNumber(subscribers);
            subText.text = subscribersAbbreviated;
        }

        SaveGame.Save<float>("subscribers", subscribers);
        achievementMan.checkSubsReward();
        sponsorMan.SetButtonAndText();  
    }

    public void addDays(float addedDays)
    {
        days += addedDays;
        daysText.text = "Day " + days.ToString();

        for (int i = 0; i < addedDays; i++)
        {
            propertyMan.addRewards();
        }

        SaveGame.Save<float>("days", days);
    }

    public static readonly Dictionary<int, string> Numberunits = new Dictionary<int, string>
    {
        {0, ""},
        {1, "K"},
        {2, "M"},
        {3, "B"},
        {4, "T"}
    };
    public static string FormatNumber(double value)
    {
        if (value < float.MaxValue)
        {

            if (value < 1d)
            {
                return "0";
            }

            var n = (int)Math.Log(value, 1000);
            var m = value / Math.Pow(1000, n);
            var unit = "";

            if (n < Numberunits.Count)
            {
                unit = Numberunits[n];
            }
            else
            {
                var unitInt = n - Numberunits.Count;
                var secondUnit = unitInt % 26;
                var firstUnit = unitInt / 26;
                unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
            }

            // Math.Floor(m * 100) / 100) fixes rounding errors
            return (Math.Floor(m * 100) / 100).ToString("0.##") + unit;
            //return (Math.Floor(m * 100) / 100).ToString("0") + unit;
        }
        else
        {
            return "MAX";
        }
    }

    public void resetSaveGame()
    {
        SaveGame.DeleteAll();
    }

}

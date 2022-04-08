using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using BayatGames.SaveGameFree;

public class PropertyManager : MonoBehaviour
{
    public AudioSource clickAudio;
    public AudioSource moneyAudio;
    public AudioSource notenoughmoneyAudio;

    public StatsManager statsMan;

    public RawImage icon;
    public TextMeshProUGUI propertyName;
    public TextMeshProUGUI propertyCost;
    public Button buyButton;
    public TextMeshProUGUI buttonText;

    public properties[] propertyList;
    public int activeProperty;

    [System.Serializable]
    public class properties
    {
        public string name;
        public float reward;
        public Texture icon;
        public float cost;
        public bool owned;
    }

    private void Start()
    {
        for (int i = 0; i < propertyList.Length; i++)
        {
            if (SaveGame.Exists("property" + propertyList[i].name))
            {
                propertyList[i].owned = SaveGame.Load<bool>("property" + propertyList[i].name);
            }
        }
        setPropertyText();
    }

    private void OnEnable()
    {
        setPropertyText();
    }

    public void nextProperty()
    {
        if (activeProperty < (propertyList.Length - 1))
        {
            activeProperty += 1;
        }
        else
        {
            activeProperty = 0;
        }

        setPropertyText();
        clickAudio.Play();
    }

    public void previousProperty()
    {
        if (activeProperty > 0)
        {
            activeProperty -= 1;
        }
        else
        {
            activeProperty = (propertyList.Length - 1);
        }

        setPropertyText();
        clickAudio.Play();

    }

    public void setPropertyText()
    {
        icon.texture = propertyList[activeProperty].icon;
        string CostAbbre = AbbrevationUtility.AbbreviateNumber(propertyList[activeProperty].cost);
        propertyName.text = propertyList[activeProperty].name + " - $" + propertyList[activeProperty].reward + " / day";
        //propertyCost.text = "$" + propertyList[activeProperty].cost.ToString();
        propertyCost.text = "$" + CostAbbre;

        if (propertyList[activeProperty].owned)
        {
            buyButton.interactable = false;
            buttonText.text = "Bought";
        }
        else
        {
            buyButton.interactable = true;
            buttonText.text = "Buy";
        }
    }

    public void buyProperty()
    {
        if (statsMan.money >= propertyList[activeProperty].cost)
        {
            statsMan.addMoney(-propertyList[activeProperty].cost);
            propertyList[activeProperty].owned = true;

            SaveGame.Save<bool>("property" + propertyList[activeProperty].name, true);

            setPropertyText();
            moneyAudio.Play();
        }
        else
        {
            notenoughmoneyAudio.Play();
        }
    }

    public void addRewards()
    {
        for (int i = 0; i < propertyList.Length; i++)
        {
            if (propertyList[i].owned)
            {
                statsMan.addMoney(propertyList[i].reward);
            }
        }
    }

    public static class AbbrevationUtility
    {
        private static readonly SortedDictionary<int, string> abbrevations = new SortedDictionary<int, string>
     {
         {1000,"K"},
         {1000000, "M" },
         {1000000000, "B" }
     };

        public static string AbbreviateNumber(float number)
        {
            for (int i = abbrevations.Count - 1; i >= 0; i--)
            {
                KeyValuePair<int, string> pair = abbrevations.ElementAt(i);
                if (Mathf.Abs(number) >= pair.Key)
                {
                    int roundedNumber = Mathf.FloorToInt(number / pair.Key);
                    return roundedNumber.ToString() + pair.Value;
                }
            }
            return number.ToString();
        }
    }

}

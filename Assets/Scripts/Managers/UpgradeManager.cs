using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BayatGames.SaveGameFree;

public class UpgradeManager : MonoBehaviour
{
    public AudioSource clickAudio;
    public AudioSource moneyAudio;
    public AudioSource notenoughmoneyAudio;

    public StatsManager statsMan;
    public AchievementManager achievementMan;

    public TextMeshProUGUI categoryName;
    public RawImage upgradeIcon;
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI upgradeCost;

    public Button buyButton;
    public TextMeshProUGUI buttonText;

    public upgradeCategory[] upgradeCategories;
    public int activeCategory = 0;

    //public GameObject wall;
    public Material wallMaterial;
    public Material floorMaterial;

    [System.Serializable]
    public class upgradeCategory
    {
        public string catName;
        public bool replaceable; //replacable means stuff like a desk. Non replacable is like school
        public bool colorWall;
        public bool colorFloor;

        public upgrades[] upgradeList;
        public int activeUpgrade = 0;

        [System.Serializable]
        public class upgrades
        {
            public string name;
            public float cost;
            public Texture icon;
            public bool owned;
            public GameObject objectToActivate;
            public bool needSchool;
            public bool needCollege;
            public bool needUni;
            public Color color;
        }
    }

    private void Start()
    {
        for (int i = 0; i < upgradeCategories.Length; i++)
        {
            for (int a = 0; a < upgradeCategories[i].upgradeList.Length; a++)
            {
                if (SaveGame.Exists(upgradeCategories[i].upgradeList[a].name + "saved"))
                {
                    upgradeCategories[i].upgradeList[a].owned = SaveGame.Load<bool>(upgradeCategories[i].upgradeList[a].name + "owned");
                    upgradeCategories[i].upgradeList[a].cost = SaveGame.Load<float>(upgradeCategories[i].upgradeList[a].name + "cost");

                    if (upgradeCategories[i].upgradeList[a].owned && upgradeCategories[i].replaceable)
                    {
                        if (upgradeCategories[i].colorWall)
                        {
                            wallMaterial.SetColor("_Color", upgradeCategories[i].upgradeList[a].color);
                        }
                        if (upgradeCategories[i].colorFloor)
                        {
                            floorMaterial.SetColor("_Color", upgradeCategories[i].upgradeList[a].color);
                        }

                        if (upgradeCategories[i].upgradeList[a].objectToActivate)
                        {
                            upgradeCategories[i].upgradeList[a].objectToActivate.SetActive(true);
                        }
                    }
                    if (!upgradeCategories[i].upgradeList[a].owned && upgradeCategories[i].replaceable)
                    {
                        if (upgradeCategories[i].upgradeList[a].objectToActivate)
                        {
                            upgradeCategories[i].upgradeList[a].objectToActivate.SetActive(false);
                        }
                    }
                    if (upgradeCategories[i].upgradeList[a].owned && upgradeCategories[i].upgradeList[a].objectToActivate && !upgradeCategories[i].replaceable)
                    {
                        upgradeCategories[i].upgradeList[a].objectToActivate.SetActive(true);
                    }


                }
            }
        }
    }
    
    private void OnEnable()
    {
        setCategoryText();
    }

    public void nextCategory()
    {
        if (activeCategory < (upgradeCategories.Length - 1))
        {
            activeCategory += 1;
        }
        else
        {
            activeCategory = 0;
        }

        setCategoryText();
        clickAudio.Play();
    }

    public void previousCategory()
    {
        if (activeCategory > 0)
        {
            activeCategory -= 1;
        }
        else
        {
            activeCategory = (upgradeCategories.Length - 1);
        }
        setCategoryText();
        clickAudio.Play();

    }


    public void nextUpgrade()
    {
        if (upgradeCategories[activeCategory].activeUpgrade < (upgradeCategories[activeCategory].upgradeList.Length - 1))
        {
            upgradeCategories[activeCategory].activeUpgrade += 1;
        }
        else
        {
            upgradeCategories[activeCategory].activeUpgrade = 0;
        }

        setUpgradeText();
        clickAudio.Play();

    }

    public void previousUpgrade()
    {
        if (upgradeCategories[activeCategory].activeUpgrade > 0)
        {
            upgradeCategories[activeCategory].activeUpgrade -= 1;
        }
        else
        {
            upgradeCategories[activeCategory].activeUpgrade = (upgradeCategories[activeCategory].upgradeList.Length - 1);
        }

        setUpgradeText();
        clickAudio.Play();

    }

    public void setCategoryText()
    {
        upgradeCategories[activeCategory].activeUpgrade = 0; //set back to first active upgrade;
        categoryName.text = upgradeCategories[activeCategory].catName;
        setUpgradeText();
    }

    public void setUpgradeText()
    {
        upgradeName.text = upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].name;
        upgradeCost.text = "$" + upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].cost.ToString();
        if (upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].icon)
        {
            upgradeIcon.texture = upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].icon;
        }

        //Check if currently owned or not
        if (upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].owned)
        {
            buyButton.interactable = false;
            buttonText.text = "Bought";
        }
        else
        {
            buyButton.interactable = true;
            buttonText.text = "Buy";
        }

        //Check school education
        if (upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].needSchool)
        {
            if (!statsMan.schoolEducated)
            {
                buyButton.interactable = false;
                buttonText.text = "Need school education";
            }
        }
        if (upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].needCollege)
        {
            if (!statsMan.collegeEducated)
            {
                buyButton.interactable = false;
                buttonText.text = "Need college education";
            }
        }
        if (upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].needUni)
        {
            if (!statsMan.uniEducated)
            {
                buyButton.interactable = false;
                buttonText.text = "Need university education";
            }
        }
    }

    public void buyTheUpgrade()
    {
        if (statsMan.money >= upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].cost)
        {

            //Reset if it is a replaceable object (remove owned and setactive false)
            if (upgradeCategories[activeCategory].replaceable)
            {
                for (int i = 0; i < upgradeCategories[activeCategory].upgradeList.Length; i++)
                {
                    upgradeCategories[activeCategory].upgradeList[i].owned = false;
                    if (upgradeCategories[activeCategory].upgradeList[i].objectToActivate)
                    {
                        upgradeCategories[activeCategory].upgradeList[i].objectToActivate.SetActive(false);
                    }
                }
            }

            //If color
            if (upgradeCategories[activeCategory].colorWall)
            {
                wallMaterial.SetColor("_Color", upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].color);
            }
            if (upgradeCategories[activeCategory].colorFloor)
            {
                floorMaterial.SetColor("_Color", upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].color);
            }

            //If there is an object to activate, activate
            if (upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].objectToActivate)
            {
                upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].objectToActivate.SetActive(true);
            }

            //Set owned true to ACTIVE object, remove cost
            upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].owned = true;
            statsMan.addMoney(-upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].cost);
            upgradeCategories[activeCategory].upgradeList[upgradeCategories[activeCategory].activeUpgrade].cost = 0;

            //Savegame this shit - First remove all saves then save what is active
            for (int i = 0; i < upgradeCategories.Length; i++)
            {
                for (int a = 0; a < upgradeCategories[i].upgradeList.Length; a++)
                {
                    if (SaveGame.Exists(upgradeCategories[i].upgradeList[a].name + "saved"))
                    {
                        SaveGame.Delete(upgradeCategories[i].upgradeList[a].name + "saved");
                        SaveGame.Delete(upgradeCategories[i].upgradeList[a].name + "cost");
                        SaveGame.Delete(upgradeCategories[i].upgradeList[a].name + "owned");
                    }
                }
            }

            for (int i = 0; i < upgradeCategories.Length; i++)
            {
                for (int a = 0; a < upgradeCategories[i].upgradeList.Length; a++)
                {
                    if (upgradeCategories[i].upgradeList[a].cost == 0)
                    {
                        SaveGame.Save<float>(upgradeCategories[i].upgradeList[a].name + "cost", 0);
                        SaveGame.Save<bool>(upgradeCategories[i].upgradeList[a].name + "owned", upgradeCategories[i].upgradeList[a].owned);
                        SaveGame.Save<bool>(upgradeCategories[i].upgradeList[a].name + "saved", true);
                        Debug.Log(upgradeCategories[i].upgradeList[a].name + " has been saved and owned is " + upgradeCategories[i].upgradeList[a].owned);
                    }
                }
            }

            checkAllOwned();
            setUpgradeText();
            moneyAudio.Play();
        }
        else
        {
            notenoughmoneyAudio.Play();
        }
    }

    public void checkAllOwned()
    {
        for (int i = 0; i < upgradeCategories.Length; i++)
        {
            for (int a = 0; a < upgradeCategories[i].upgradeList.Length; a++)
            {
                if (upgradeCategories[i].upgradeList[a].cost != 0)
                {
                    return; //If it doesnt have the upgrade, stop the function
                }
            }
        }
        achievementMan.ownedAllUpgrades();
    }



}

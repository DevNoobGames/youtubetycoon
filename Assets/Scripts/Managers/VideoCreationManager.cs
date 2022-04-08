using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoCreationManager : MonoBehaviour
{
    [Header("Other references")]
    public SideMenubar sideMenuB;
    public StatsManager statsMan;
    public Animation devnoobAnim;
    public Animation sidebarAnim;
    public SponsorManager sponsorMan;
    public AchievementManager achievementMan;
    public CreatedVidManager createdVidMan;

    public AudioSource clickAudio;
    public AudioSource notenoughmoneyAudio;

    [Header ("Menu 1")]
    public string vidName;
    public TMP_InputField inputName;

    public string[] type;
    public int activeType;
    public TextMeshProUGUI typeText;

    //public int[] budget;
    public budgetNReward[] budgetReward;
    public int activeBudget;
    public TextMeshProUGUI budgetText;

    public int[] days;
    public TextMeshProUGUI daysText;

    [Header ("Thumbnail Creator")]
    public TMP_FontAsset[] fonts;
    public int activeFont;
    public TextMeshProUGUI thumbnailTitle;

    public Texture[] backgrounds;
    public int activeTexture;
    public RawImage actualBG;

    public Texture[] logos;
    public int activeLogo;
    public RawImage actualLogo;

    [System.Serializable]
    public class budgetNReward
    {
        public int budget;
        public float reward;
    }

    public void GoToThumbnailCreator()
    {
        if (statsMan.money >= budgetReward[activeBudget].budget) 
        {
            if (string.IsNullOrWhiteSpace(vidName))
            {
                vidName = "Follow Devnoob on Youtube";
                thumbnailTitle.text = vidName;
            }

            sideMenuB.closeMenus();
            sideMenuB.openMenu(1);
            clickAudio.Play();
        }
        else
        {
            notenoughmoneyAudio.Play();
            Debug.Log("No moneys");
        }
    }

    public void createVideo()
    {
        if (statsMan.menuInteractable)
        {
            statsMan.menuInteractable = false;
            statsMan.addMoney(-budgetReward[activeBudget].budget);
            sidebarAnim.Play("CloseSideBar");
            vidName = "";
            thumbnailTitle.text = vidName;
            clickAudio.Play();
            StartCoroutine(videoCreating());
        }
    }
    public IEnumerator videoCreating()
    {
        sideMenuB.closeMenus();
        devnoobAnim.Play("WorkingOngame");

        yield return new WaitForSeconds(5);

        if (statsMan.subscribers < 50)
        {
            statsMan.addSubscribers(Mathf.RoundToInt((Random.Range(1, 15)) + budgetReward[activeBudget].reward));
        }
        else if (statsMan.subscribers < 200)
        {
            statsMan.addSubscribers(Mathf.RoundToInt((Random.Range(15, 30)) + budgetReward[activeBudget].reward));
        }
        else if (statsMan.subscribers < 1000)
        {
            statsMan.addSubscribers(Mathf.RoundToInt((Random.Range(30, 250)) + budgetReward[activeBudget].reward));
        }
        else
        {
            float currentSub = statsMan.subscribers * 0.08f; //take 8% of current subs
            float chanceAdded = Random.Range(0, 30); //add a random of 0 to 30%
            float subpluschance = currentSub * (1 + (chanceAdded / 100)); //That's a total of this
            float finalAdditive = Mathf.RoundToInt(subpluschance + budgetReward[activeBudget].reward); //add budget reward

            statsMan.addSubscribers(finalAdditive);
        }

        //Add video to created list
        createdVidMan.addVideo(vidName, activeFont, activeTexture, activeLogo);

        //reset vid name
        vidName = "";


        sidebarAnim.Play("OpenSideBar");
        statsMan.addDays(days[activeBudget]);
        statsMan.menuInteractable = true;
        sponsorMan.SetButtonAndText(); //Comes after adding subs
        sponsorMan.addMoneyAfterVideo();
        achievementMan.addVideoCreated();

    }

    public void onChangeTitle()
    {
        vidName = inputName.text;
        thumbnailTitle.text = vidName;
    }

    public void nextType()
    {
        if (activeType < (type.Length - 1))
        {
            activeType += 1;
        }
        else
        {
            activeType = 0;
        }
        clickAudio.Play();
        typeText.text = type[activeType];
    }

    public void previousType()
    {
        if (activeType > 0)
        {
            activeType -= 1;
        }
        else
        {
            activeType = (type.Length - 1);
        }
        clickAudio.Play();
        typeText.text = type[activeType];
    }

    public void nextBudget()
    {
        if (activeBudget < (budgetReward.Length - 1))
        {
            activeBudget += 1;
        }
        else
        {
            activeBudget = 0;
        }
        budgetText.text = "$" + budgetReward[activeBudget].budget.ToString();
        //budgetText.text = "$" + budget[activeBudget].ToString();
        clickAudio.Play();
        daysText.text = days[activeBudget].ToString() + " days";
    }

    public void previousBudget()
    {
        if (activeBudget > 0)
        {
            activeBudget -= 1;
        }
        else
        {
            activeBudget = (budgetReward.Length - 1);
        }
        budgetText.text = "$" + budgetReward[activeBudget].budget.ToString();
        clickAudio.Play();
        daysText.text = days[activeBudget].ToString() + " days";
    }

    public void nextBG()
    {
        if (activeTexture < (backgrounds.Length - 1))
        {
            activeTexture += 1;
        }
        else
        {
            activeTexture = 0;
        }
        actualBG.texture = backgrounds[activeTexture];
        clickAudio.Play();
    }
    public void previousBG()
    {
        if (activeTexture > 0)
        {
            activeTexture -= 1;
        }
        else
        {
            activeTexture = (backgrounds.Length - 1);
        }
        actualBG.texture = backgrounds[activeTexture];
        clickAudio.Play();
    }

    public void nextFont()
    {
        if (activeFont < (fonts.Length - 1))
        {
            activeFont += 1;
        }
        else
        {
            activeFont = 0;
        }
        thumbnailTitle.font = fonts[activeFont];
        float chanc = Random.Range(0f, 1f);
        clickAudio.Play();
    }
    public void previousFont()
    {
        if (activeFont > 0)
        {
            activeFont -= 1;
        }
        else
        {
            activeFont = (fonts.Length - 1);
        }
        thumbnailTitle.font = fonts[activeFont];
        clickAudio.Play();
    }

    public void nextLogo()
    {
        if (activeLogo < (logos.Length - 1))
        {
            activeLogo += 1;
        }
        else
        {
            activeLogo = 0;
        }
        actualLogo.texture = logos[activeLogo];
        clickAudio.Play();
    }
    public void previousLogo()
    {
        if (activeLogo > 0)
        {
            activeLogo -= 1;
        }
        else
        {
            activeLogo = (logos.Length - 1);
        }
        actualLogo.texture = logos[activeLogo];
        clickAudio.Play();
    }
}

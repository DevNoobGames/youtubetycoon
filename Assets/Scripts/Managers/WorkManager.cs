using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkManager : MonoBehaviour
{
    public AudioSource clickAudio;

    public SideMenubar sidebarM;
    public StatsManager statsMan;
    public Animation devnoobAnim;
    public Animation sidebarAnim;

    public Button workBtn;
    public TextMeshProUGUI btnText;

    public TextMeshProUGUI jobTitle;
    public TextMeshProUGUI jobRequirements;
    public RawImage jobIcon;

    public jobs[] JobList;
    public int activeJob;

    [System.Serializable]
    public class jobs
    {
        public string jobName;
        public float jobReward;
        public Texture icon;
        public bool School;
        public bool College;
        public bool University;
        public bool ivyLeague; 
    }

    private void OnEnable()
    {
        activeJob = 0;

        jobTitle.text = JobList[activeJob].jobName + " - " + JobList[activeJob].jobReward + "$";
        jobIcon.texture = JobList[activeJob].icon;
        jobRequirementsText();
    }

    public void nextJob()
    {
        if (activeJob < (JobList.Length - 1))
        {
            activeJob += 1;
        }
        else
        {
            activeJob = 0;
        }
        jobRequirementsText();
        clickAudio.Play();

    }

    public void previousJob()
    {
        if (activeJob > 0)
        {
            activeJob -= 1;
        }
        else
        {
            activeJob = (JobList.Length - 1);
        }
        jobRequirementsText();
        clickAudio.Play();

    }

    public void jobRequirementsText()
    {
        jobTitle.text = JobList[activeJob].jobName + " - " + JobList[activeJob].jobReward + "$";
        jobIcon.texture = JobList[activeJob].icon;

        if (JobList[activeJob].ivyLeague)
        {
            jobRequirements.text = "-Ivy League";
        }
        else if (JobList[activeJob].University)
        {
            jobRequirements.text = "-University";
        }
        else if (JobList[activeJob].College)
        {
            jobRequirements.text = "-College";
        }
        else if (JobList[activeJob].School)
        {
            jobRequirements.text = "-School";
        }
        else
        {
            jobRequirements.text = "";
        }

        //Set button positive before checking requirements
        workBtn.interactable = true;
        btnText.text = "Work";

        //check requirements
        if (JobList[activeJob].School && !statsMan.schoolEducated)
        {
            workBtn.interactable = false;
            btnText.text = "Need school education";
        }
        if (JobList[activeJob].College && !statsMan.collegeEducated)
        {
            workBtn.interactable = false;
            btnText.text = "Need college education";
        }
        if (JobList[activeJob].University && !statsMan.uniEducated)
        {
            workBtn.interactable = false;
            btnText.text = "Need university education";
        }
        if (JobList[activeJob].ivyLeague && !statsMan.ivyEducated)
        {
            workBtn.interactable = false;
            btnText.text = "Need ivy league education";
        }
    }

    public void workButton()
    {
        if (statsMan.menuInteractable)
        {
            statsMan.menuInteractable = false;
            sidebarM.closeMenus();
            sidebarAnim.Play("CloseSideBar");
            StartCoroutine(youBetterWorkB());
            clickAudio.Play();

        }
    }

    IEnumerator youBetterWorkB()
    {
        devnoobAnim.Play("WorkingOnJob");

        yield return new WaitForSeconds(5);

        statsMan.addMoney(JobList[activeJob].jobReward);
        sidebarAnim.Play("OpenSideBar");
        statsMan.addDays(1);
        statsMan.menuInteractable = true;
    }
}

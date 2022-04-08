using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FanInteractionManager : MonoBehaviour
{
    public AudioSource clickAudio;

    public SideMenubar sidebarM;
    public StatsManager statsMan;
    public Button rutsButton;
    public Animation devnoobAnim;
    public Animation sidebarAnim;

    public void Ruts()
    {
        rutsButton.interactable = false;
        statsMan.addSubscribers(1);
        StartCoroutine(RutsTimer());
        clickAudio.Play();
    }
    public IEnumerator RutsTimer()
    {
        yield return new WaitForSeconds(2);
        rutsButton.interactable = true;
    }

    public void answerComments()
    {
        statsMan.menuInteractable = false;
        sidebarM.closeMenus();
        sidebarAnim.Play("CloseSideBar");
        StartCoroutine(commentsNum());
        clickAudio.Play();
    }
    IEnumerator commentsNum()
    {
        devnoobAnim.Play("AnsweringComments");
        yield return new WaitForSeconds(4);

        float currentSubs = statsMan.subscribers;
        float newSubs = currentSubs * 0.05f;
        float chance = Random.Range(0.3f, 1.5f);
        statsMan.addSubscribers(Mathf.RoundToInt(newSubs * chance));

        sidebarAnim.Play("OpenSideBar");
        statsMan.addDays(1);
        statsMan.menuInteractable = true;
    }

}

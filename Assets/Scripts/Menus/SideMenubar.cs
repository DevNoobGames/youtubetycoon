using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenubar : MonoBehaviour
{
    public GameObject[] menus;
    public StatsManager statsMan;

    public AudioSource clickAud;

    public void openMenu(int menuInt)
    {
        if (!menus[menuInt].activeInHierarchy && statsMan.menuInteractable)
        {
            closeMenus();

            menus[menuInt].SetActive(true);
            menus[menuInt].GetComponent<Animation>().Play("OpenSideMenu");
            clickAud.Play();
        }
        else
        {
            closeMenus();
        }
    }

    public void closeMenus()
    {
        foreach (GameObject menu in menus)
        {
            if (menu.activeInHierarchy)
            {
                StartCoroutine(closeMenu(menu));
            }
        }
    }
    
    public IEnumerator closeMenu(GameObject menu)
    {
        menu.GetComponent<Animation>().Play("CloseSideMenu");
        yield return new WaitForSeconds(0.41f);
        menu.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class EducationManager : MonoBehaviour
{
    public StatsManager statsMan;

    public bool school, college, uni, ivy;

    private void OnEnable()
    {
        if (school)
        {
            statsMan.schoolEducated = true;
            SaveGame.Save<bool>("educated_school", true);
        }
        if (college)
        {
            statsMan.collegeEducated = true;
            SaveGame.Save<bool>("educated_college", true);
        }
        if (uni)
        {
            statsMan.uniEducated = true;
            SaveGame.Save<bool>("educated_uni", true);
        }
        if (ivy)
        {
            statsMan.ivyEducated = true;
            SaveGame.Save<bool>("educated_ivy", true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BayatGames.SaveGameFree;

public class CreatedVidManager : MonoBehaviour
{
    public TextMeshProUGUI vidCName;
    public RawImage vidCThumbnail;
    public RawImage vidCIcon;

    public VideoCreationManager videoCreationMan;

    public GameObject[] activateIfHave;

    public AudioSource clickAudio;

    public List<videos> createdVids = new List<videos>();
    public int activeVid;

    [System.Serializable]
    public class videos
    {
        public string vidName;
        public int vidNr;

        public int fontNumber;
        public int thumbnailNumber;
        public int iconNumber;

        //public Texture vidThumbnail;
        //public Texture vidIcon;
    }

    private void OnEnable()
    {
        counter();
    }

    public void counter()
    {
        if (SaveGame.Exists("Video 0" ) && createdVids.Count <= 0)
        {
            loadSave(0);
        }

        if (createdVids.Count > 0)
        {
            foreach (GameObject obj in activateIfHave)
            {
                obj.SetActive(true);
            }
        }
    }

    public void loadSave(int nr)
    {
        if (SaveGame.Exists("Video " + nr))
        {
            string nam = SaveGame.Load<string>("Video " + nr + "vidName");
            int fon = SaveGame.Load<int>("Video " + nr + "fontNumber");
            int thumb = SaveGame.Load<int>("Video " + nr + "thumbnailNumber");
            int icon = SaveGame.Load<int>("Video " + nr + "iconNumber");

            addVideo(nam, fon, thumb, icon);

            nr += 1;
            loadSave(nr);
        }
    }

    public void next()
    {
        if (activeVid < (createdVids.Count - 1))
        {
            activeVid += 1;
        }
        else
        {
            activeVid = 0;
        }
        clickAudio.Play();
        setVideo();
    }

    public void previous()
    {
        if (activeVid > 0)
        {
            activeVid -= 1;
        }
        else
        {
            activeVid = (createdVids.Count - 1);
        }
        clickAudio.Play();
        setVideo();
    }

    public void setVideo()
    {
        vidCName.text = createdVids[activeVid].vidName;
        vidCName.font = videoCreationMan.fonts[createdVids[activeVid].fontNumber];
        vidCThumbnail.texture = videoCreationMan.backgrounds[createdVids[activeVid].thumbnailNumber];
        vidCIcon.texture = videoCreationMan.logos[createdVids[activeVid].iconNumber];
    }

    public void addVideo(string name, int font, int bg, int icon)
    {
        videos c = new videos();
        c.vidName = name;
        c.vidNr = createdVids.Count;

        c.fontNumber = font;
        c.thumbnailNumber = bg;
        c.iconNumber = icon;

        createdVids.Add(c);
        counter();

        SaveGame.Save<string>("Video " + c.vidNr, "Video " + c.vidNr);
        SaveGame.Save<string>("Video " + c.vidNr + "vidName", c.vidName);
        SaveGame.Save<int>("Video " + c.vidNr + "fontNumber", c.fontNumber);
        SaveGame.Save<int>("Video " + c.vidNr + "thumbnailNumber", c.thumbnailNumber);
        SaveGame.Save<int>("Video " + c.vidNr + "iconNumber", c.iconNumber);
    }
    
    /*public void addVideo(string name, Texture bg, Texture icon)
    {
        videos c = new videos();
        c.vidName = name;
        c.vidNr = createdVids.Count;
        c.vidThumbnail = bg;
        c.vidIcon = icon;

        createdVids.Add(c);
        counter();
    }*/
}

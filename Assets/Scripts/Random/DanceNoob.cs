using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceNoob : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public Animation danceAnim;

    private void Start()
    {
        StartCoroutine(danceBitch());
    }

    IEnumerator danceBitch()
    {
        cam3.SetActive(false);
        cam1.SetActive(true);
        danceAnim.Play("DanceNoob");
        yield return new WaitForSeconds(3);
        cam1.SetActive(false);
        cam2.SetActive(true);
        yield return new WaitForSeconds(3);
        cam2.SetActive(false);
        cam3.SetActive(true);
        danceAnim.Play("DanceNoob2");
        yield return new WaitForSeconds(5);
        StartCoroutine(danceBitch());
    }
}

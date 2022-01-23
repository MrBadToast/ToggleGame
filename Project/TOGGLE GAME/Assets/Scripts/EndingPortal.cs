using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndingPortal : MonoBehaviour
{

    public DOTweenAnimation anim;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GoToEnding();

    }

    public void GoToEnding()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        StopAllCoroutines();
        StartCoroutine(GotoEnding());

        IEnumerator GotoEnding()
        {
            anim.DOPlayAllById("FADE_OUT");

            if (GetComponent<SimpleSoundModule>())
                GetComponent<SimpleSoundModule>().FadeOut(1.0f);

            yield return new WaitForSeconds(1.0f);
            yield return null;

            SceneManager.LoadScene("Ending");

        }
    }


}


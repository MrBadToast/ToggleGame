using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public DOTweenAnimation transition; 

    public void MoveSceneWithTransition(string SceneName)
    {
        StopAllCoroutines();
        StartCoroutine("Cor_TransitionScenechange",SceneName);
    }

    private IEnumerator Cor_TransitionScenechange(string sceneName)
    {
       transition.DOPlayAllById("FADE_OUT");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}

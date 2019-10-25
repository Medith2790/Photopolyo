using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


    [Header("Canvas Group")]
    public CanvasGroup fadeCg;              //CanvasGroup 저장할 변수

    [Range(0.5f, 2.0f)]                      //Fade in 처리시간
    public float fadeDuration = 1.5f;

    [Header("Scene Load")]
    public Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>();
    public static SceneLoader Instance;


    //호출할 씬의 정보설정
    void InitSceneInfo()
    {
        Instance = this;
        //호출할 씬의 정보를 딕셔너리에 추가
        loadScenes.Add("Level1", LoadSceneMode.Additive);
        loadScenes.Add("Play", LoadSceneMode.Additive);

    }


    public void MoveToOtherScene(GameObject[] sHero, int sceneNum)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(sceneNum);      //Play씬을 반환

        for (int i = 0; i < 4; i++)
        {
            SceneManager.MoveGameObjectToScene(sHero[i], scene);
        }
    }



    IEnumerator Start () {

        InitSceneInfo();

        //처음 알파값을 설정(불투명)
        fadeCg.alpha = 1.0f;

        //여러개의 씬을 코루틴으로 호출
        foreach(var _loadScene in loadScenes)
        {
            yield return StartCoroutine(LoadScene(_loadScene.Key, _loadScene.Value));
        }

        StartCoroutine(Fade(0.0f));
	}

    IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        //비동기 방식으로 씬을 로드하고 로드가 완료될 때까지 대기
        yield return SceneManager.LoadSceneAsync(sceneName, mode);

        //호출된 씬을 활성화
        Scene loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(loadedScene);
    }


    //Fade in& out 시키는 함수
    IEnumerator Fade(float finalAlpha)
    {
        //라이트맵이 깨지는 것을 방지하기 위해 스테이지 씬을 활성화
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level1"));
        fadeCg.blocksRaycasts = true;

        //절대값 함수로 백분율을 계산
        float fadeSpeed = System.Math.Abs(fadeCg.alpha - finalAlpha) / fadeDuration;

        //알파값을 조정
        while(!Mathf.Approximately(fadeCg.alpha, finalAlpha))
        {
            //MoveToward함수는 Lerp함수와 동일한 함수로 알파값을 보간
            fadeCg.alpha = Mathf.MoveTowards(fadeCg.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        fadeCg.blocksRaycasts = false;

        //fade in이 완료된 후 SceneLoader씬은 삭제(Unload)
        SceneManager.UnloadSceneAsync("SceneLoader");


    }

}

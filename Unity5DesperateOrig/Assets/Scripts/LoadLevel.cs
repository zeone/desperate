using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
    //  public GameObject Button;
    public Texture Backgroundbar;
    public Texture ProgBar;
    private AsyncOperation async = null;
    public string LevelName = "";
    public bool isLoading = false;
    public float LoadDelay = 5;
    public static LoadLevel entity { get; set; }


    void Awake()
    {
        entity = this;
    }
    // Use this for initialization

    public void LoadSurvivle()
    {
        if (!LoadLevel.entity.isLoading)
        {
            isLoading = true;
            StartCoroutine(LoadLevelAsync());
        }
    }

    private void OnGUI()
    {
        //async.allowSceneActivation = false;
        /*if (async.progress >= 0.9f)
        {
            StartCoroutine(Preload());
        }*/
        if (LoadLevel.entity.isLoading)
        {
            //   Button.SetActive(false);
            // GUI.HorizontalScrollbar(new Rect(25, 12, 300, 25), 0, async.progress * 100, 0, 100);

            GUI.DrawTexture(new Rect((Screen.width / 2) - 300, (Screen.height / 2) + (Screen.height / 3), 512, 30),
                Backgroundbar, ScaleMode.StretchToFill, true, 1F);
            //Сдесь выводится текстура полосы, которая будед ПОД полоской загрузки (Фон)
            GUI.DrawTexture(
                new Rect((Screen.width / 2) - 300, (Screen.height / 2) + (Screen.height / 3), async.progress * 512, 30), ProgBar,
                ScaleMode.StretchToFill, true, 1F);
            //А тут непосредственно сама полоса загрузки в с координатами  такими же как и у фона, но длина расчитивается прогресс загрузки умноженное на длину текстуры


            Debug.Log(string.Format("=> isLoading {0}%", async.progress * 100));
            //if (GUI.Button(new Rect((Screen.width * 0.5f) - 60, Screen.height * 0.6f, 120, 40), "press to continue"))
            //{
            //    isLoading = true;
            //   // StartCoroutine("_Start"); //код согласия
            //}
        }
    }


    private IEnumerator LoadLevelAsync()
    {
        Debug.Log("Loading... ");
        async = Application.LoadLevelAsync(LevelName);
        //   async.allowSceneActivation = false;
        while (!async.isDone)
        {
            Debug.Log(string.Format("Loading {0}%", async.progress * 100));
            yield return null;
        }

        Debug.Log("Loading complete");
        isLoading = false;

        yield return async;
    }

    IEnumerator Preload()
    {
        yield return new WaitForSeconds(LoadDelay);
        async.allowSceneActivation = true;
    }

}

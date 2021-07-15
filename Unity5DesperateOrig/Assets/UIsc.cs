using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using UnityEngine.UI;

public class UIsc : MonoBehaviour
{


    [Header("Панель опыта")]
    public Text ScoreText;

    public Text XPTest;

    public Image XpBar;
    public Image ScoreBar;

    [Header("Панель здоровья")]
    public Image HPBar;

    public Image Shield;

    public float BlinkedBar = 2;
    private float _BlinkedTimmer;
    [Header("Панельо оружия")]
    public Image WeaponIcon;

    public Image AmmoBar;

    [Header("UI панели")]
    public GameObject WeaponPanel;

    public GameObject HealthPanel;
    public GameObject XpPanel;
    public GameObject Background;
    public GameObject Menu;
    public GameObject PerfMenu;
    public GameObject DeadMenuObj;
    public GameObject ShowMenuButton;

    [Header("Окно при гибели")]
    public Text PlayTime;

    public Text KillsCount;
    public Text AccuracyCount;
    public Text ScoreCount;

    public Text QualityParam;
    [Header("Джойстики")]
    public GameObject RJoystick;
    public GameObject LJoystick;

    [Header("Дополнительные инструменты управления")]
    public Scrollbar QualitySlider;
    public static UIsc entity { get; set; }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                BackToGame();
                return;
            }
        }


        //_________________ScoreUI_____________________________
        ScoreText.text = GameMeneger.entity.score.ToString("D4");
        XPTest.text = "XP: " + GameMeneger.entity.xp.ToString("D4");
        if (GameMeneger.entity.xp != 0 && GameMeneger.entity.TargetXp != 0)
        {
            XpBar.fillAmount = (float)GameMeneger.entity.xp / GameMeneger.entity.TargetXp;
        }
        else
        {
            XpBar.fillAmount = 0;
        }
        if (GameMeneger.entity.score != 0 && GameMeneger.entity.TargetScore != 0)
        {
            ScoreBar.fillAmount = (float)GameMeneger.entity.score / GameMeneger.entity.TargetScore;
        }
        else
        {
            ScoreBar.fillAmount = 0;
        }


        //_______________HP______________________________________
        if (!GameMeneger.entity.Shield)
        {
            if (Shield.gameObject.active) Shield.gameObject.SetActive(false);
            if (!HPBar.gameObject.active) HPBar.gameObject.SetActive(true);
            HPBar.fillAmount = (float)Player.entity.heals / Player.entity.maxHeals;
            if (HPBar.fillAmount < 0.25)
            {
                _BlinkedTimmer += Time.deltaTime;
                if (HPBar.enabled && _BlinkedTimmer > BlinkedBar)
                {
                    HPBar.enabled = false;
                }
                if (!HPBar.enabled && _BlinkedTimmer > BlinkedBar + 0.5)
                {
                    HPBar.enabled = true;
                    _BlinkedTimmer = 0;
                }
            }
        }
        else
        {
            if (!Shield.gameObject.active) Shield.gameObject.SetActive(true);
            if (HPBar.gameObject.active) HPBar.gameObject.SetActive(false);
        }

        //___________________Weapon____________________
        if (Weapon.entity) AmmoBar.fillAmount = (float)Weapon.entity.curentAmmo / Weapon.entity.ammo;
        if (Weapon.entity && WeaponIcon.name != Weapon.entity.WeaponIcon.name) WeaponIcon.sprite = Weapon.entity.WeaponIcon;
    }

    //menu
    public void ShowMenu()
    {
        LJoystick.SetActive(false);
        RJoystick.SetActive(false);
        ShowMenuButton.SetActive(false);
        Background.SetActive(true);
        WeaponPanel.SetActive(false);
        XpPanel.SetActive(false);
        HealthPanel.SetActive(false);
        Menu.SetActive(true);
        BonusInfo.slowTime = 0;
    }
    // Панель настроек
    public void ShowPrefMenu()
    {
        /* LJoystick.SetActive(false);
         RJoystick.SetActive(false);
         ShowMenuButton.SetActive(false);
         Background.SetActive(true);
         WeaponPanel.SetActive(false);
         XpPanel.SetActive(false);
         HealthPanel.SetActive(false);*/
        if (QualitySettings.GetQualityLevel() == 0) QualitySlider.value = 0;
        if (QualitySettings.GetQualityLevel() == 1) QualitySlider.value = 0.2f;
        if (QualitySettings.GetQualityLevel() == 2) QualitySlider.value = 0.4f;
        if (QualitySettings.GetQualityLevel() == 3) QualitySlider.value = 0.6f;
        if (QualitySettings.GetQualityLevel() == 4) QualitySlider.value = 0.8f;
        if (QualitySettings.GetQualityLevel() == 5) QualitySlider.value = 1f;
        Menu.SetActive(false);
        PerfMenu.SetActive(true);
        QualityParam.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void HidePrefMenu()
    {

        PerfMenu.SetActive(false);
        Menu.SetActive(true);

    }
    public void QualityChange()
    {

        float _par = QualitySlider.value;
        Debug.Log(QualitySlider.value);
        //if (_par == 0) QualitySettings.SetQualityLevel(0);
        //if (_par >= 0.15f && _par <= 0.25f) QualitySettings.SetQualityLevel(1);
        //if (_par >= 0.35f && _par <= 0.45f) QualitySettings.SetQualityLevel(2);
        //if (_par >= 0.55f && _par <= 0.65f) QualitySettings.SetQualityLevel(3);
        //if (_par >= 0.75f && _par <= 0.85f) QualitySettings.SetQualityLevel(4);
        if (_par == 0) QualitySettings.SetQualityLevel(1);
        if (_par == 0.5) QualitySettings.SetQualityLevel(3);
        if (_par == 1) QualitySettings.SetQualityLevel(4);
        QualityParam.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    //Reload Level
    public void ReloadLevel()
    {
        BonusInfo.slowTime = 1;
        Application.LoadLevel(Application.loadedLevelName);
    }

    //Back To Game
    public void BackToGame()
    {
        LJoystick.SetActive(true);
        RJoystick.SetActive(true);
        ShowMenuButton.SetActive(true);
        Menu.SetActive(false);
        Background.SetActive(false);
        WeaponPanel.SetActive(true);
        XpPanel.SetActive(true);
        HealthPanel.SetActive(true);
        BonusInfo.slowTime = 1;
    }


    //Load Main Menu
    public void LoadMainMenu()
    {
        BonusInfo.slowTime = 1;
        Application.LoadLevel(0);
    }

    //Load Dead menu
    public void DeadMenu()
    {
        LJoystick.SetActive(false);
        RJoystick.SetActive(false);
        ShowMenuButton.SetActive(false);
        Background.SetActive(true);
        WeaponPanel.SetActive(false);
        XpPanel.SetActive(false);
        HealthPanel.SetActive(false);
        DeadMenuObj.SetActive(true);
        KillsCount.text = GameMeneger.entity.KilledMobs.ToString();
        ScoreCount.text = GameMeneger.entity.score.ToString();
        AccuracyCount.text = (int)GameMeneger.entity.GetAccuracy() + "%";
        var _time = GameMeneger.entity.PlayedTime();
        var minutes = (int)_time / 60;
        var seconds = +(int)_time % 60;
        PlayTime.text = minutes + ":" + seconds;

    }



}

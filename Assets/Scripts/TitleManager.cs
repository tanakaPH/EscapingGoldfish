using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class TitleManager : MonoBehaviour
{
    private void Awake()
    {
        SetUpFireBase();
        SetUpAdMob();

        if (PlayerPrefs.GetString("Language", "") == "")
        {
            //端末の設定言語を取得
            SystemLanguage sl = Application.systemLanguage;

            switch (sl)
            {
                case SystemLanguage.Japanese:
                    PlayerPrefs.SetString("Language", "Japanese");
                    break;
                case SystemLanguage.English:
                    PlayerPrefs.SetString("Language", "English");
                    break;
                default:// デフォルトでは英語に
                    PlayerPrefs.SetString("Language", "English");
                    break;
            }
        }
    }

    void Start()
    {
        StartBGM();
        SetActiveButton();
        InitializeStaticVariable();
    }

    void SetUpFireBase()
    {
        /*
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        */

        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventAppOpen);
    }

    void SetUpAdMob()
    {
        MobileAds.Initialize(initStatus => { });
    }

    void StartBGM()
    {
        GetComponent<AudioSource>().Play();
    }

    void SetActiveButton()
    {
        if (PlayerPrefs.GetString("LatestStage", "") != "" && PlayerPrefs.GetString("LatestStage", "") != "0-1")
        {
            GameObject.Find("ContinueButton").GetComponent<Button>().interactable = true;
        }

        if (PlayerPrefs.GetString("Language", "Japanese") == "English")
        {
            GameObject.Find("Dropdown").GetComponent<Dropdown>().value = 1;
        }

        if (PlayerPrefs.GetInt("TutorialSkip", 0) == 1)
        {
            GameObject.Find("SkipButton").GetComponent<Toggle>().isOn = true;
        }
    }

    void InitializeStaticVariable()
    {
        Static.isStory = false;
        Static.isPopUp = false;
        Static.isMenu = false;
        Static.mainStageNum = 0;
        Static.subStageNum = 1;
        Static.stageName = "0-1";
        Static.saltMode = 0;
        Static.saltCount = 0;
        Static.hardMode = 0;
        Static.life = 0;
        Static.continueCount = 0;
    }

    public async void PressStartButton()
    {
        GameObject.Find("StartButton").GetComponent<AudioSource>().Play();
        await Task.Delay(200);

        if(PlayerPrefs.GetInt("TutorialSkip", 0) == 1)
        {
            SceneManager.LoadScene("Stage1-1");
        }
        else
        {
            SceneManager.LoadScene("Stage0-1");
        }
    }

    public async void PressContinueButton()
    {
        GameObject.Find("ContinueButton").GetComponent<AudioSource>().Play();
        await Task.Delay(200);

        Static.life = PlayerPrefs.GetInt("LatestLife", 1);
        Static.continueCount = PlayerPrefs.GetInt("ContinueCount", 0);
        SceneManager.LoadScene("Stage" + PlayerPrefs.GetString("LatestStage", "0-1"));
    }

    public void PressConfigButton()
    {
        GameObject.Find("StartButton").GetComponent<AudioSource>().Play();
        GameObject.Find("ConfigPanel").GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
        GameObject.Find("ConfigButton").GetComponent<Button>().interactable = false;
    }

    public void PressOKButton()
    {
        //言語設定
        if (GameObject.Find("Dropdown").GetComponent<Dropdown>().value == 0)
        {
            PlayerPrefs.SetString("Language", "Japanese");
        }
        else if (GameObject.Find("Dropdown").GetComponent<Dropdown>().value == 1)
        {
            PlayerPrefs.SetString("Language", "English");
        }

        //チュートリアルスキップ設定
        if (GameObject.Find("SkipButton").GetComponent<Toggle>().isOn == true)
        {
            PlayerPrefs.SetInt("TutorialSkip", 1);
        }
        else
        {
            PlayerPrefs.SetInt("TutorialSkip", 0);
        }

        GameObject.Find("ConfigPanel").GetComponent<RectTransform>().localScale = new Vector3(0, 1, 0);
        GameObject.Find("ConfigButton").GetComponent<Button>().interactable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

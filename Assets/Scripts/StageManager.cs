using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static AudioSource[] bgm;
    public AudioClip tapSE;

    public int mainStageNum;
    public int subStageNum;

    private int storyPanelCount;
    private int storyTextCount;
    private int popUpTextCount;

    GameObject timerText;
    public static float time;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        StartBGM();

        LoadStageInfo();
        SaveState();

        SetTextWindowInfo();

        if (mainStageNum != 9)
        {
            SetLife();
            SetObject();
        }

        if (mainStageNum == 4 || Static.stageName == "5-3")
        {
            SetTime();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ForwardText();
        }
        if ((mainStageNum == 4 || Static.stageName == "5-3") &&
            Static.isMenu == false && Static.isPopUp == false && Static.isMenu == false &&
            GameObject.Find("GoldFish").GetComponent<PlayerController>().isTrap == false &&
            GameObject.Find("GoldFish").GetComponent<PlayerController>().isGoal == false)
        {
            DecreaseTime();
        }
    }

    void StartBGM()
    {
        bgm = GetComponents<AudioSource>();
        bgm[0].Play();
    }

    void LoadStageInfo()
    {
        Static.mainStageNum = mainStageNum;
        Static.subStageNum = subStageNum;
        Static.stageName = mainStageNum + "-" + subStageNum;
    }

    void SaveState()
    {
        PlayerPrefs.SetString("LatestStage", Static.stageName);
        PlayerPrefs.SetInt("LatestLife", Static.life);
        PlayerPrefs.SetInt("ContinueCount", Static.continueCount);
        PlayerPrefs.Save();

        if (!Debug.isDebugBuild)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelStart,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevelName, "Stage" + Static.stageName),
            });
        }
    }

    public static void SetLife()
    {
        if (Static.stageName == "1-1")
        {
            Static.life = 3;
        }
        for(int i = 1; i <= Static.life; i++)
        {
            GameObject.Find("Life" + i).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
        }
    }

    void SetObject()
    {
        if(Static.isStageFirst == true && Static.stageName == "4-3")
        {
            GameObject.FindGameObjectWithTag("life").transform.localScale = new Vector3(1, 1, 0);
        }
    }

    void SetTextWindowInfo()
    {
        if (Static.stageName == "0-1")
        {
            Static.isStory = true;
            storyTextCount = 2;
            storyPanelCount = 1;

            Static.isPopUp = false;
            popUpTextCount = 2;
        }
        else if (Static.mainStageNum == 9)
        {
            Static.isStory = true;
            storyTextCount = 2;
            storyPanelCount = 1;
        }
        else if (Static.stageName.Contains("-2"))
        {
            Static.isStory = false;
            Static.isPopUp = false;
        }
        else
        {
            Static.isStory = false;
            Static.isPopUp = true;
            popUpTextCount = 2;
        }
    }

    void SetTime()
    {
        time = float.Parse(GameObject.Find("Time").GetComponent<Text>().text);
    }

    void DecreaseTime()
    {
        if (time > 0.00f)
        {
            time -= Time.deltaTime;
            GameObject.Find("Time").GetComponent<Text>().text = time.ToString("F2");
        }
        else if(Static.stageName == "5-3")
        {
            GameObject.Find("Time").GetComponent<Text>().text = "0.00";
            GameObject.Find("GoldFish").GetComponent<PlayerController>().isGoal = true;
            GameObject.Find("TitleButton").GetComponent<Button>().interactable = false;
            GameObject.Find("GoldFish").GetComponent<PlayerController>().MoveNextStage();
        }
        else
        {
            GameObject.Find("Time").GetComponent<Text>().text = "0.00";
            GameObject.Find("GoldFish").GetComponent<PlayerController>().isTrap = true;
            GameObject.Find("GoldFish").GetComponent<PlayerController>().Miss();
        }
    }

    void ForwardText()
    {
        if (Static.isStory == true)
        {
            ForwardStory();
        }
        else if (Static.isPopUp == true)
        {
            ForwardPopUp();
        }
    }

    void ForwardStory()
    {
        if (storyTextCount == 8)
        {
            if (Static.stageName == "0-1")
            {
                GameObject.Find("StoryPanel" + storyPanelCount).SetActive(false);
                Static.isStory = false;
                Static.isPopUp = true;
            }
            else if (Static.mainStageNum == 9)
            {
                GameObject.Find("StoryPanel" + storyPanelCount).SetActive(false);
                GetComponent<AudioSource>().PlayOneShot(tapSE);
                storyPanelCount++;
                storyTextCount = 2;
            }
        }
        else
        {
            if (storyTextCount == 7 && Static.stageName == "0-1")
            {
                bgm[0].Stop();
                bgm[1].Play();
            }

            GetComponent<AudioSource>().PlayOneShot(tapSE);

            if (storyPanelCount == 3 && Static.stageName == "9-1")
            {
                Static.saltMode = 1;
                PlayerPrefs.SetInt("SaltMode", Static.saltMode);
                PlayerPrefs.DeleteKey("LatestStage");
                PlayerPrefs.DeleteKey("LatestLife");
                SceneManager.LoadScene("Title");
            }
            else
            {
                GameObject.Find("StoryPanel" + storyPanelCount + "/Text" + storyTextCount).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
                storyTextCount++;
            }
        }
    }

    void ForwardPopUp()
    {
        if (popUpTextCount == 6)
        {
            GameObject.Find("PopUpPanel").SetActive(false);
            Static.isPopUp = false;
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(tapSE);
            GameObject.Find("PopUpPanel/Text" + popUpTextCount).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
            popUpTextCount++;
        }
    }

    public void PressTitleButton()
    {
        GetComponent<AudioSource>().PlayOneShot(tapSE);
        Static.isMenu = true;
        GameObject.Find("TitlePanel").GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
        GameObject.Find("TitleButton").GetComponent<Button>().interactable = false;
    }

    public void PressGoTitleButton()
    {
        GetComponent<AudioSource>().PlayOneShot(tapSE);
        SceneManager.LoadScene("Title");
    }

    public async void PressCancelButton()
    {
        GetComponent<AudioSource>().PlayOneShot(tapSE);
        GameObject.Find("TitlePanel").GetComponent<RectTransform>().localScale = new Vector3(0, 1, 0);
        GameObject.Find("TitleButton").GetComponent<Button>().interactable = true;
        await Task.Delay(10);
        Static.isMenu = false;
    }
}

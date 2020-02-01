using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    //ゲームクリア、ミス、ゲームオーバーの表示オブジェクト
    GameObject gameClear;
    GameObject miss;
    GameObject gameOver;

    //ゴールと障害物オブジェクト
    GameObject goal;
    GameObject[] trap;
    GameObject[] obstacle;

    //ゴール処理、トラップ処理中かを判断する
    public bool isGoal;
    public bool isTrap;

    //インスペクタの変数にアタッチされているクリップ
    public AudioClip moveSE;
    public AudioClip gameClearSE;
    public AudioClip missSE;
    public AudioClip gameOverSE;
    public AudioClip recoverSE;

    //フリック検知用
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;

    //動画リワード広告表示用
    private RewardedAd continueAd;
    private string adUnitId;

    void Start()
    {
        AssignStageObject();
        InitializeVariable();
        PreloadRewardedAd();
    }

    void Update()
    {
        if (Static.isPopUp == false && Static.isMenu == false && Static.isStory == false 
            && isGoal == false && isTrap == false)
        {
            DetectKeyDown();
            DetectFlick();
        }

        DetectIsGoal();
        DetectIsTrap();
    }

    void AssignStageObject()
    {
        this.animator = GetComponent<Animator>();
        this.gameClear = GameObject.Find("GameClear");
        this.miss = GameObject.Find("Miss");
        this.gameOver = GameObject.Find("GameOver");
        this.goal = GameObject.FindGameObjectWithTag("goal");
        this.trap = GameObject.FindGameObjectsWithTag("trap");
        this.obstacle = GameObject.FindGameObjectsWithTag("obstacle");
    }

    void InitializeVariable()
    {
        isGoal = false;
        isTrap = false;
    }

    void PreloadRewardedAd()
    {
        //Release Id is below.
        adUnitId = "ca-app-pub-6565480179137292~5239877250";

        //Test Id is below.
        //adUnitId = "ca-app-pub-3940256099942544/1712485313";

        this.continueAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.continueAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.continueAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.continueAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.continueAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.continueAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.continueAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.continueAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        StageManager.bgm[0].Stop();
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Static.life = 3;
        Static.continueCount++;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        if (Static.life == 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            StageManager.bgm[0].Play();
            PreloadRewardedAd();
        }
    }

    #region 操作系の処理

    void DetectKeyDown()
    {
        //Click the left key
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LButtonDown();
        }
        //Click the right key
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RButtonDown();
        }
        //Click the Down key
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DButtonDown();
        }
        //Click the Up key
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UButtonDown();
        }
    }

    void DetectFlick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            GetDirection();
        }

    }

    void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;

        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (30 < directionX)
            {
                RButtonDown();
            }
            else if (-30 > directionX)
            {
                LButtonDown();
            }
        }
        else if (Mathf.Abs(directionX) < Mathf.Abs(directionY))
        {
            if (30 < directionY)
            {
                UButtonDown();
            }
            else if (-30 > directionY)
            {
                DButtonDown();
            }
        }
    }

    //左ボタンが押された時
    public void LButtonDown()
    {
        this.animator.SetTrigger("RightTrigger");           //x軸移動アニメ読み込み
        transform.localScale = new Vector2(-1.2f, 1.2f);    //x軸方向に反転（元のアニメは右向きのため）

        //ゴール時でない　かつ　左エリア境界内
        if (-290 < transform.localPosition.x && !ExistLeftObstacle())
        {
            GetComponent<AudioSource>().PlayOneShot(moveSE);    //移動SE
            transform.localPosition = new Vector3(transform.localPosition.x - 150, transform.localPosition.y, 0);   //x軸方向に移動
        }
    }

    public void RButtonDown()
    { 
        this.animator.SetTrigger("RightTrigger");
        transform.localScale = new Vector2(1.2f, 1.2f);

        if (transform.localPosition.x < 290 && !ExistRightObstacle())
        {  
            GetComponent<AudioSource>().PlayOneShot(moveSE);
            transform.localPosition = new Vector3(transform.localPosition.x + 150, transform.localPosition.y, 0);
        }
    }

    public void DButtonDown()
    {
        this.animator.SetTrigger("UpTrigger");
        transform.localScale = new Vector2(1.2f, -1.2f);

        if (-520 < transform.localPosition.y && !ExistDownObstacle())
        {
            GetComponent<AudioSource>().PlayOneShot(moveSE);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 150, 0);
        }
    }

    public void UButtonDown()
    {
        this.animator.SetTrigger("UpTrigger");
        transform.localScale = new Vector2(1.2f, 1.2f);

        if (transform.localPosition.y < 520 && !ExistUpObstacle())
        {
            GetComponent<AudioSource>().PlayOneShot(moveSE);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 150, 0);
        }
    }

    //操作方向に障害物があるかを返す
    public bool ExistLeftObstacle()
    {
        //操作方向と各オブジェクトの位置の差をx軸とy軸それぞれ求め、四捨五入（小数点が完全に一致しないため）する
        for (int i = 0; i < obstacle.Length; i++)
        {
            if ((Mathf.Round((transform.position.x - 1.12f) - obstacle[i].transform.position.x) == 0) &&
            (Mathf.Round(transform.position.y - obstacle[i].transform.position.y) == 0))
            {
                return true;
            }
        }
        return false;
    }

    public bool ExistRightObstacle()
    {
        for (int i = 0; i < obstacle.Length; i++)
        {
            if ((Mathf.Round((transform.position.x + 1.12f) - obstacle[i].transform.position.x) == 0) &&
            (Mathf.Round(transform.position.y - obstacle[i].transform.position.y) == 0))
            {
                return true;
            }
        }
        return false;
    }

    public bool ExistDownObstacle()
    {
        for (int i = 0; i < obstacle.Length; i++)
        {
            if ((Mathf.Round(transform.position.x - obstacle[i].transform.position.x) == 0) &&
            (Mathf.Round((transform.position.y - 1.12f) - obstacle[i].transform.position.y) == 0))
            {
                return true;
            }
        }
        return false;
    }

    public bool ExistUpObstacle()
    {
        for (int i = 0; i < obstacle.Length; i++)
        {
            if ((Mathf.Round(transform.position.x - obstacle[i].transform.position.x) == 0) &&
            (Mathf.Round((transform.position.y + 1.12f) - obstacle[i].transform.position.y) == 0))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    void DetectIsGoal()
    {
        //ゴールした際に、クリアマークをy=2.60fの位置まで毎秒-0.1fずつ移動
        if (isGoal == true && this.gameClear.transform.position.y >= 2.60f)
        {
            this.gameClear.transform.Translate(0, -0.1f, 0);
            this.goal.GetComponent<ParticleSystem>().Play();
        }
    }

    void DetectIsTrap()
    {
        if (isTrap == true)
        {
            //ゲームオーバー時自身を回転
            transform.Rotate(new Vector3(0, 0, 3.5f));

            if (this.miss.transform.position.y >= 2.60f)
            {
                this.miss.transform.Translate(0, -0.1f, 0);
            }
        }
    }

    //衝突を検出した場合
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ゴールした場合
        if (collision.gameObject == goal)
        {
            GameObject.Find("TitleButton").GetComponent<Button>().interactable = false;
            isGoal = true;
            MoveNextStage();
        }
        //罠に触れた場合
        if (collision.tag == "trap")
        {
            GameObject.Find("TitleButton").GetComponent<Button>().interactable = false;
            isTrap = true;
            Miss();
        }

        if (collision.tag == "life")
        {
            GetComponent<AudioSource>().PlayOneShot(recoverSE);
            GameObject.FindWithTag("life").SetActive(false);
            Static.life++;
            StageManager.SetLife();
        }
    }

    //次のStageに移動
    public async void MoveNextStage()
    {
        GetComponent<AudioSource>().PlayOneShot(gameClearSE);
        await Task.Delay(3000);

        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelEnd,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevelName, "Stage" + Static.stageName),
                new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterSuccess, 1)
            });

        Static.isStageFirst = true;

        if (Static.stageName == "0-1")
        {
            SceneManager.LoadScene("Stage1-1");
        }
        else if (Static.stageName == "5-3")
        {
            SceneManager.LoadScene("Stage9-1");
        }
        else if(Static.subStageNum < 3)
        {
            SceneManager.LoadScene("Stage" + Static.mainStageNum + "-" + (Static.subStageNum + 1));
        }
        else if(Static.mainStageNum < 6)
        {
            SceneManager.LoadScene("Stage" + (Static.mainStageNum + 1) + "-1");
        }
        else
        {
            SceneManager.LoadScene("Title");
        }
    }

    //ゲームオーバー演出のあとステージ初期に戻る
    public async void Miss()
    {
        GetComponent<AudioSource>().PlayOneShot(missSE);
        await Task.Delay(3000);

        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLevelEnd,
            new Firebase.Analytics.Parameter[] {
                new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevelName, "Stage" + Static.stageName),
                new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterSuccess, 0)
            });

        Static.isStageFirst = false;
        Static.life--;

        if (Static.life <= 0)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
                new Firebase.Analytics.Parameter[]{
                    new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterItemName, "Stage" + Static.stageName),
                    new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "ContinueCount"),
                    new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterValue, Static.continueCount)
                });

            GameObject.Find("GameOverPanel").GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void PressContinueButton()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameObject.Find("NetworkErrorText").GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
        }
        else if (this.continueAd.IsLoaded())
        {
            this.continueAd.Show();
        }
        else
        {
            PreloadRewardedAd();
        }
    }

    public async void PressGameOverButton()
    {
        GameObject.Find("GameOver").GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
        GameObject.Find("GameOverPanel").GetComponent<RectTransform>().localScale = new Vector3(0, 1, 0);
        StageManager.bgm[0].Stop();
        GetComponent<AudioSource>().PlayOneShot(gameOverSE);
        await Task.Delay(6000);
        PlayerPrefs.DeleteKey("LatestStage");
        PlayerPrefs.DeleteKey("LatestLife");
        SceneManager.LoadScene("Title");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{

    float speed;
    float step;

    float span;
    float delta;

    Vector3 initPosition;
    Vector3 currentPosition;

    int mode;

    public AudioClip SE;

    void Start()
    {
        initPosition = GetComponent<RectTransform>().anchoredPosition;
        initPosition.x = Mathf.Round(initPosition.x);
        initPosition.y = Mathf.Round(initPosition.y);

        mode = 1;
    }

    void Update()
    {
        //ミス、ゴール、メニュー動作時は演出重複を避けるため、動作を止める
        if (GameObject.Find("GoldFish").GetComponent<PlayerController>().isTrap == false &&
            GameObject.Find("GoldFish").GetComponent<PlayerController>().isGoal == false &&
            Static.isMenu == false)
        {
            currentPosition = GetComponent<RectTransform>().anchoredPosition;
            currentPosition.x = Mathf.Round(currentPosition.x);
            currentPosition.y = Mathf.Round(currentPosition.y);

            if (transform.name == "FingerLR")   MoveFingerLR();
            if (transform.name == "FingerDU")   MoveFingerDU();

            if (transform.name == "Poi")        MovePoi();

            if (transform.name == "CrayFish")   MoveCrayFish();
            if (transform.name == "CrayFishX")  MoveCrayFishX();
            if (transform.name == "Dog" &&
                Static.isPopUp == false)        MoveDog();

            if (transform.name == "BlueGill(Clone)")    MoveBlueGill();
            if (transform.name == "WhirlPool")          MoveWhirlPool();
            if (transform.name == "WhirlPoolX")         MoveWhirlPoolX();
            if (transform.name.Contains("WhirlPool_"))  MoveWhirlPoolClone();

            if (transform.name == "FishBorn")   MoveFishBorn();
            if (transform.name == "FishBornX")   MoveFishBornX();
            if (transform.name == "FishBornXR")   MoveFishBornXR();

            if (transform.name == "CatFish" &&
                Static.isPopUp == false)        MoveCatFish();

            if (transform.name == "HumanBall(Clone)") MoveHumanBall();

            if (transform.name == "SharkL(Clone)") MoveSharkL();
            if (transform.name == "SharkR(Clone)") MoveSharkR();
        }
    }

    void MoveFingerLR()
    {
        speed = 300;
        step = speed * Time.deltaTime;

        if(Static.isStory == false && Static.isPopUp == false)
        {
            if (GameObject.Find("GoldFish").GetComponent<RectTransform>().anchoredPosition.x >= 100)
            {
                mode = 1;
                currentPosition = new Vector3(50, -50, 0);
                gameObject.name = "FingerDU";
            }

            if (currentPosition.x <= 150 && mode == 1)
            {
                transform.localScale = new Vector3(1, 1, 0);
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(150, 0, 0), step);

                if (currentPosition.x.Equals(Mathf.Round(150)))
                {
                    transform.localScale = new Vector3(0, 1, 0);
                    mode = 0;
                }
            }
            else if(currentPosition.x >= -50 && mode == 0)
            {
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(-50, 0, 0), step);

                if (currentPosition.x.Equals(Mathf.Round(-50)))
                {
                    transform.localScale = new Vector3(1, 1, 0);
                    mode = 1;
                }
            }
        }
    }

    void MoveFingerDU()
    {
        speed = 300;
        step = speed * Time.deltaTime;

        if (Static.isStory == false && Static.isPopUp == false)
        {
            if (GameObject.Find("GoldFish").GetComponent<RectTransform>().anchoredPosition.y >= 100)
            {
                Destroy(this.gameObject);
            }

            if (currentPosition.y <= 150 && mode == 1)
            {
                transform.localScale = new Vector3(1, 1, 0);
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(50, 150, 0), step);

                if (currentPosition.y.Equals(Mathf.Round(150)))
                {
                    transform.localScale = new Vector3(0, 1, 0);
                    mode = 0;
                }
            }
            else if (currentPosition.y >= -50 && mode == 0)
            {
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(50, -50, 0), step);

                if (currentPosition.y.Equals(Mathf.Round(-50)))
                {
                    transform.localScale = new Vector3(1, 1, 0);
                    mode = 1;
                }
            }
        }
    }


    void MovePoi()
    {
        speed = 300;
        step = speed * Time.deltaTime;

        if (currentPosition.y.Equals(480) && currentPosition.x < 500)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(500, 480, 0), step);
        }
        else if (currentPosition.x.Equals(500) && currentPosition.y < 780)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(500, 780, 0), step);
        }
        else if (currentPosition.y.Equals(780) && currentPosition.x > 200)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(200, 780, 0), step);
        }
        else if (currentPosition.x.Equals(200) && currentPosition.y > 480)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(200, 480, 0), step);
        }
    }

    void MoveCrayFish()
    {
        speed = 50;
        step = speed * Time.deltaTime;

        span = 1.5f;
        delta += Time.deltaTime;

        if(this.span < this.delta)
        {
            if (currentPosition.x <= initPosition.x + 150 && mode == 1)
            {
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(initPosition.x + 150, initPosition.y, 0), step);
                if(currentPosition.x.Equals(initPosition.x + 150))
                {
                    this.delta = 0;
                    mode = 0;
                }

            }
            else if (initPosition.x <= currentPosition.x && mode == 0)
            {
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(initPosition.x, initPosition.y, 0), step);
                if (currentPosition.x.Equals(initPosition.x))
                {
                    this.delta = 0;
                    mode = 1;
                }
            }
        }
    }

    void MoveCrayFishX()
    {
        speed = 100;
        step = speed * Time.deltaTime;

        span = 0.5f;
        delta += Time.deltaTime;

        if (this.span < this.delta)
        {
            if (currentPosition.x <= initPosition.x + 150 && mode == 1)
            {
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(initPosition.x + 150, initPosition.y, 0), step);
                if (currentPosition.x.Equals(initPosition.x + 150))
                {
                    this.delta = 0;
                    mode = 0;
                }

            }
            else if (initPosition.x <= currentPosition.x && mode == 0)
            {
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(initPosition.x, initPosition.y, 0), step);
                if (currentPosition.x.Equals(initPosition.x))
                {
                    this.delta = 0;
                    mode = 1;
                }
            }
        }
    }

    void MoveDog()
    {
        Vector3 targetPosition = GameObject.Find("GoldFish").GetComponent<RectTransform>().anchoredPosition;

        speed = 100;
        step = speed * Time.deltaTime;

        span = 0.5f;
        delta += Time.deltaTime;

        GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, targetPosition, step);
    }

    void MoveBlueGill()
    {
        transform.Translate(-0.01f, 0, 0);
        if (gameObject.GetComponent<RectTransform>().anchoredPosition.x < -75.0f)
        {
            Destroy(gameObject);
        }
    }

    void MoveWhirlPool()
    {
        transform.Rotate(new Vector3(0, 0, 3.5f));
    }

    void MoveWhirlPoolX()
    {
        MoveWhirlPool();

        if(Static.isPopUp == false && transform.localScale.x > 1.3f && mode == 1)
        {
            transform.localScale -= new Vector3(0.001f, 0.001f, 0);
            if (Mathf.Round(100 * transform.localScale.x).Equals(130))
            {
                mode = 0;
            }
        }
        else if(Static.isPopUp == false && transform.localScale.x <= 2 && mode ==0)
        {
            transform.localScale += new Vector3(0.001f, 0.001f, 0);
            if (Mathf.Round(100 * transform.localScale.x).Equals(200))
            {
                mode = 1;
            }
        }
    }

    void MoveWhirlPoolClone()
    {
        MoveWhirlPool();

        speed = 100;
        step = speed * Time.deltaTime;

        if(transform.name == "WhirlPool_LD")
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(825, 1125, 0), step);
        }
        if (transform.name == "WhirlPool_LU")
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(-75, 1125, 0), step);
        }

        if (gameObject.GetComponent<RectTransform>().anchoredPosition.x <= -75.0f || gameObject.GetComponent<RectTransform>().anchoredPosition.x >= 825.0f)
        {
            Destroy(gameObject);
        }
    }

    void MoveFishBorn()
    {
        speed = 300;
        step = speed * Time.deltaTime;

        if (currentPosition.x >= 75 && mode == 1)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(75, currentPosition.y, 0), step);

            if (currentPosition.x.Equals(75))
            {
                transform.Rotate(0, 180, 0);
                mode = 0;
            }
        }
        else if (currentPosition.x <= 675 && mode == 0)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(675, currentPosition.y, 0), step);

            if (currentPosition.x.Equals(675))
            {
                transform.Rotate(0, 180, 0);
                mode = 1;
            }
        }
    }

    void MoveFishBornX()
    {
        speed = 300;
        step = speed * Time.deltaTime;

        if (currentPosition.x >= initPosition.x - 225 && mode == 1)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(initPosition.x - 225, currentPosition.y, 0), step);

            if (currentPosition.x.Equals(initPosition.x - 225))
            {
                transform.Rotate(0, 180, 0);
                mode = 0;
            }
        }
        else if (currentPosition.x <= initPosition.x && mode == 0)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(initPosition.x, currentPosition.y, 0), step);

            if (currentPosition.x.Equals(initPosition.x))
            {
                transform.Rotate(0, 180, 0);
                mode = 1;
            }
        }
    }

    void MoveFishBornXR()
    {
        speed = 300;
        step = speed * Time.deltaTime;

        if (currentPosition.x <= initPosition.x + 225 && mode == 1)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(initPosition.x + 225, currentPosition.y, 0), step);

            if (currentPosition.x.Equals(initPosition.x + 225))
            {
                transform.Rotate(0, 180, 0);
                mode = 0;
            }
        }
        else if (currentPosition.x >= initPosition.x && mode == 0)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(initPosition.x, currentPosition.y, 0), step);

            if (currentPosition.x.Equals(initPosition.x))
            {
                transform.Rotate(0, 180, 0);
                mode = 1;
            }
        }
    }

    void MoveCatFish()
    {
        speed = 100;
        step = speed * Time.deltaTime;

        span = 1.5f;
        delta += Time.deltaTime;

        if (this.span < this.delta)
        {
            if (currentPosition.x >= 675 && mode == 1)
            {
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(675, initPosition.y, 0), step);
                iTween.ShakePosition(GameObject.Find("Stage"), iTween.Hash("x", 0.3f, "y", 0.3f, "time", 3.5f));
                if (currentPosition.x.Equals(975))
                {
                    GetComponent<AudioSource>().PlayOneShot(SE);
                }
                if (currentPosition.x.Equals(675))
                {
                    this.delta = 0;
                    mode = 0;
                }

            }
            else if (975 >= currentPosition.x && mode == 0)
            {
                GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(currentPosition, new Vector3(975, initPosition.y, 0), step);
                iTween.ShakePosition(GameObject.Find("Stage"), iTween.Hash("x", 0.3f, "y", 0.3f, "time", 3.5f));
                if (currentPosition.x.Equals(675))
                {
                    GetComponent<AudioSource>().PlayOneShot(SE);
                }
                if (currentPosition.x.Equals(975))
                {
                    this.delta = 0;
                    mode = 1;
                }
            }
        }
    }

    void MoveHumanBall()
    {
        transform.Translate(-0.01f, 0, 0);
        if (gameObject.GetComponent<RectTransform>().anchoredPosition.x < -150.0f)
        {
            Destroy(gameObject);
        }
    }

    void MoveSharkL()
    {
        transform.Translate(-0.02f, 0, 0);
        if (gameObject.GetComponent<RectTransform>().anchoredPosition.x < -75.0f)
        {
            Destroy(gameObject);
        }
    }

    void MoveSharkR()
    {
        transform.Translate(-0.02f, 0, 0);
        if (gameObject.GetComponent<RectTransform>().anchoredPosition.x > 825.0f)
        {
            Destroy(gameObject);
        }
    }
}

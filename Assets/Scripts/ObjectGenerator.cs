﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectGenerator : MonoBehaviour
{
    public GameObject prefabA;
    public GameObject prefabB;

    GameObject objectA;
    GameObject objectB;

    //何秒毎に生成するか
    public float spanA;
    public float spanB;

    float deltaA;
    float deltaB;

    void Start()
    {
        //ステージ開始時のみ即時生成するための代入
        deltaA = spanA;
        deltaB = spanB;
    }

    void Update()
    {
        if (GameObject.Find("GoldFish").GetComponent<PlayerController>().isGoal == false &&
            GameObject.Find("GoldFish").GetComponent<PlayerController>().isTrap == false &&
            Static.isPopUp == false && Static.isMenu == false && Static.isStory == false )
        {
            this.deltaA += Time.deltaTime;
            this.deltaB += Time.deltaTime;

            if (this.deltaA > this.spanA)
            {
                this.deltaA = 0;

                if (Static.mainStageNum == 3 || Static.stageName == "5-1") GenerateBlueGill();
                if (Static.stageName == "5-3") GenerateHumanBall();
            }

            if (this.deltaB > this.spanB)
            {
                this.deltaB = 0;

                //if (Static.stageName == "3-3") GenerateWhirlPool();
            }
        }

    }

    void GenerateBlueGill()
    {
        objectA = Instantiate(prefabA) as GameObject;
        objectA.transform.SetParent(this.transform);
        objectA.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        objectA.GetComponent<RectTransform>().anchoredPosition = new Vector3(825f, GenerateRandomY(), 0);
    }

    void GenerateWhirlPool()
    {
        if (GameObject.Find("WhirlPoolX").GetComponent<RectTransform>().localScale.x <= 1.7f)
        {
            objectB = Instantiate(prefabB) as GameObject;
            objectB.name = "WhirlPool_LD";
            objectB.transform.SetParent(this.transform);
            objectB.transform.localScale = new Vector3(0.4f, 0.4f, 1);
            objectB.GetComponent<RectTransform>().anchoredPosition = new Vector3(375f, 675f, 0);

            GameObject objectC = Instantiate(prefabB) as GameObject;
            objectC.name = "WhirlPool_LU";
            objectC.transform.SetParent(this.transform);
            objectC.transform.localScale = new Vector3(0.4f, 0.4f, 1);
            objectC.GetComponent<RectTransform>().anchoredPosition = new Vector3(375f, 675f, 0);
        }
    }

    void GenerateHumanBall()
    {
        objectA = Instantiate(prefabA) as GameObject;
        objectA.transform.SetParent(this.transform);
        objectA.transform.localScale = new Vector3(1f, 1f, 1);
        objectA.GetComponent<RectTransform>().anchoredPosition = new Vector3(900f, GenerateRandomY(), 0);

        if(StageManager.time < 15.0f)
        {
            this.deltaA = 1.0f;
        }
    }

    public float GenerateRandomY()
    {
        if(Static.stageName == "3-1")
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0: return 375f;
                case 1: return 525f;
                case 2: return 675f;
                default: return 675f;
            }
        }
        else if (Static.stageName == "3-2")
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0: return 375f;
                case 1: return 675f;
                case 2: return 975f;
                default: return 975f;
            }
        }
        else if (Static.stageName == "3-3")
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0: return 225f;
                case 1: return 375f;
                case 2: return 975f;
                default: return 975f;
            }
        }
        else if (Static.stageName == "5-1")
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0: return 225f;
                case 1: return 525f;
                case 2: return 675f;
                default: return 675f;
            }
        }
        else if (Static.stageName == "5-3")
        {
            int rand = Random.Range(0, 7);
            switch (rand)
            {
                case 0: return 300f;
                case 1: return 450f;
                case 2: return 600f;
                case 3: return 750f;
                case 4: return 900f;
                case 5: return 1050f;
                case 6: return 150f;
                default: return 1050f;
            }
        }
        else
        {
            return 375f;
        }
    }
}

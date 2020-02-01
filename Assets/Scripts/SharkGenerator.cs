using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SharkGenerator : MonoBehaviour
{
    public GameObject sharkL;
    public GameObject sharkR;
    float span = 2.0f;
    float delta = 0;
    int counter = 0;
    string stageNum;

    // Start is called before the first frame update
    void Start()
    {
        stageNum = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        //サメ生成までにラグがあるため、初期のみ意図的に２匹生成
        for (; counter < 2; counter++)
        {
            GameObject shL = Instantiate(sharkL) as GameObject;
            shL.transform.position = new Vector3(3.5f, generateRandomY(), 0);

            //Stage5以降、左からもサメが出現
            if (stageNum != "Stage3" && stageNum != "Stage4")
            {
                GameObject shR = Instantiate(sharkR) as GameObject;
                shR.transform.position = new Vector3(-3.5f, generateRandomY(), 0);
            }
        }

        //2s（span）ごとにサメを生成
        this.delta += Time.deltaTime;
        if(this.delta > this.span)
        {
            this.delta = 0;
            GameObject shL = Instantiate(sharkL) as GameObject;
            shL.transform.position = new Vector3(3.5f, generateRandomY(), 0);
            if (stageNum != "Stage3" && stageNum != "Stage4")
            {
                GameObject shR = Instantiate(sharkR) as GameObject;
                shR.transform.position = new Vector3(-3.5f, generateRandomY(), 0);
            }
        }
    }

    //２行目〜４行目にランダムにサメを生成（defaultは機能していない）
    public float generateRandomY()
    {
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0: return 1.07f;
            case 1: return 2.16f;
            case 2: return 3.35f;
            default: return 4.45f;
        }
    }
}

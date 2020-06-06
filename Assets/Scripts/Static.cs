using UnityEngine;

public class Static : MonoBehaviour
{
    //ストーリー（パネル透過率0%）表示中かどうか
    public static bool isStory;

    //ポップアップ（パネル透過率50%）表示中かどうか
    public static bool isPopUp;

    //メニュー表示中かどうか
    public static bool isMenu;

    public static int mainStageNum;
    public static int subStageNum;
    public static string stageName;

    //2周目以降（1周クリアしている）かどうか
    public static int saltMode;
    public static int saltCount;

    //ハードモード（3周目以降開放）かどうか
    public static int hardMode;

    //当該ステージに遷移して1回目かどうか
    public static bool isStageFirst;

    public static int life;

    //コンティニューした回数
    public static int continueCount;

    //Firebase多重登録防止変数
    public static bool isFirstOpen;
}

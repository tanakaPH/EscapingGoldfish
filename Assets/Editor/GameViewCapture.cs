using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections;

public class GameViewCapture : EditorWindow {

    //ディレクトリのパス
    //エスケープシーケンスに注意（\ → \\）
    public static string directoryPath = "./ScreenShots";

    //ファイルの名前
    //拡張子は不要
    public static string filename = "ScreenShot";

    //日付をつけるかどうか
    public static bool isYear = true;
    public static bool isMonth = true;
    public static bool isDate = true;
    public static bool isHour = true;
    public static bool isMinute = true;
    public static bool isSecond = true;

    //撮影したらゲームを停止するかどうか
    public static bool isPausePlaying = true;

    //名前をつけて保存のパネルを開くかどうか
    public static bool isOpenSaveFilePanel = true;

    //エクスプローラーで開くかどうか
    public static bool isOpenExplorer = true;

    //関連付けられたアプリケーションで開くかどうか
    public static bool isOpenApplication = false;

    //出力中のファイルのパス
    public static string generatedFilePath = null;

    // オブジェクトの共通Openコマンド
    // % (ctrl on Windows, cmd on macOS), # (shift), & (alt)
    [MenuItem("Tools/GameViewCapture &#c")]

    //ゲームビューをキャプチャする
    static void Capture() {

        if (isPausePlaying) {
            //再生中だったら
            if (EditorApplication.isPlaying) {
                //ゲームを停止
                EditorApplication.isPaused = true;
            }
        }

        //ファイルを出力中だったら
        if (generatedFilePath != null) {
            //ダイアログを表示
            if (EditorUtility.DisplayDialog("ファイルを出力中です", "出力中のファイル" + generatedFilePath + "は破棄されます。" + "\n" + "新しいファイルを保存しますか？", "保存", "中止")) {
                //出力中を解除
                generatedFilePath = null;
            }
            else {
                //処理を中止
                return;
            }
        }

        //日付文字列を作成
        string time = "";
        if (isYear) {
            time += DateTime.Now.ToString("_yyyy");
        }
        if (isMonth) {
            time += DateTime.Now.ToString("_MM");
        }
        if (isDate) {
            time += DateTime.Now.ToString("_dd");
        }
        if (isHour) {
            time += DateTime.Now.ToString("_HH");
        }
        if (isMinute) {
            time += DateTime.Now.ToString("_mm");
        }
        if (isSecond) {
            time += DateTime.Now.ToString("_ss");
        }

        //ファイルのパス
        string filePath = null;

        if (isOpenSaveFilePanel) {

            //ファイルの保存先を指定
            filePath = EditorUtility.SaveFilePanel("名前を付けて保存", "", filename + time, "png");

            //パスの指定がなかったら
            if (string.IsNullOrEmpty(filePath)) {
                //処理を中止
                return;
            }

        }
        else {

            //ファイルの保存先を作成
            filePath = string.Format(Path.Combine(directoryPath, filename + time + ".png"));

            //フォルダが存在していなかったら
            if (!Directory.Exists(directoryPath)) {

                //ダイアログを表示
                if (EditorUtility.DisplayDialog("フォルダが存在しません", "保存先のフォルダ" + directoryPath + "は存在しません。" + "\n" + "新しくフォルダを作成しますか？", "作成", "中止")) {
                    //フォルダを作成
                    Directory.CreateDirectory(directoryPath);
                    UnityEngine.Debug.Log("generated : " + directoryPath);
                }
                else {
                    //処理を中止
                    return;
                }

            }

        }

        //ゲームビューをキャプチャ
        ScreenCapture.CaptureScreenshot(string.Format(filePath));

        //キャプチャしたことを通知
        UnityEngine.Debug.Log("Captured : " + filePath);

        //EditorApplication.updateにコルーチンの進行を追加
        IEnumerator coroutine = GenerateFile(filePath);
        EditorApplication.update += () => coroutine.MoveNext();

    }

    public static IEnumerator GenerateFile(string path) {

        //出力中に設定
        generatedFilePath = path;

        //出力されるまで待つ
        while (!File.Exists(path)) {
            yield return null;
        }

        //出力中を解除
        generatedFilePath = null;

        //出力されたことを通知
        UnityEngine.Debug.Log("generatedd : " + path);

        //ファイルを開く
        //OpenFile(path);

    }

    public static void OpenFile(string path) {

        if (isOpenExplorer) {
            //エクスプローラーで開く
            Process.Start("explorer.exe", "/select," + path.Replace("/", "\\"));
        }

        if (isOpenApplication) {
            //関連付けられたアプリケーションで開く
            Process.Start(path);
        }

    }

}
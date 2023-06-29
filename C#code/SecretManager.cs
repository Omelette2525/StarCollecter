using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretManager : MonoBehaviour
{
    //インスペクターでパネルをアタッチ
    [SerializeField] private GameObject secretPanel;
    [SerializeField] private GameObject endingPanel;

    //シークレット譜面開放フラグリスト
    public static List<bool> secretOpenFlag = new List<bool>() { false, false };	//曲数が増えたら増やす

    //表示フラグ
    public static bool secretPrintFlag = false;
    public static bool secretPrintCompleteFlag = false;
    public static bool endingPrintFlag = false;
    public static bool endingPrintCompleteFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (secretPrintFlag == true && secretPrintCompleteFlag == false)    //フラグがtrueで、まだ表示していなかったら
        {
            Debug.Log("open");
            secretPanel.SetActive(true);    //表示
            secretPrintCompleteFlag = true; //表示完了フラグをtrue
        }
        if (Input.anyKeyDown && secretPrintCompleteFlag == true && endingPrintFlag == false)    //表示完了していて、キーが押されて、エンディング曲開放パネルが出ていなかったら
        {
            secretPanel.SetActive(false);
            StartCoroutine(SecretPrintFlagFalse());
        }
        /*if (secretPrintFlag == true)
        {
            secretPanel.SetActive(true);
            if (Input.anyKeyDown && endingPrintFlag == false)
                secretPrintFlag = false;
        }
        else
            secretPanel.SetActive(false);*/

        if (endingPrintFlag == true && endingPrintCompleteFlag == false)    //フラグがtrueで、まだ表示していなかったら
        {
            Debug.Log("open");
            endingPanel.SetActive(true);    //表示
            endingPrintCompleteFlag = true; //表示完了フラグをtrue
        }
        if (Input.anyKeyDown && endingPrintCompleteFlag == true)    //表示完了していて、キーが押されたら
        {
            endingPanel.SetActive(false);
            StartCoroutine(EndingPrintFlagFalse());
        }
            
    }

    private IEnumerator EndingPrintFlagFalse()  //少しディレイをかける(一回キーを押すと全て閉じられるのを防ぐ)
    {
        yield return new WaitForSeconds(0.1f);

        endingPrintFlag = false;
    }

    private IEnumerator SecretPrintFlagFalse()  //少しディレイをかける(一回キーを押すと全て閉じられるのを防ぐ)
    {
        yield return new WaitForSeconds(0.1f);

        secretPrintFlag = false;
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;

public class ResultManager : MonoBehaviour
{
	[SerializeField] private Text textScore;
	[SerializeField] private Text textRank;
	[SerializeField] private Text textExScore;
	[SerializeField] private Text textJudgement;
	[SerializeField] private Text textMusicName;
	[SerializeField] private Text textEarlyCount;
	[SerializeField] private Text textLateCount;
	[SerializeField] private Text textDetail;
	[SerializeField] private Slider targetGaugeSlider;
	[SerializeField] private GameObject panelTargetGaugeToStarGauge;
	[SerializeField] private AudioMixer AudioMixer;

	private string scoreValueFormat;
	private string exScoreValueFormat;
	private string judgementValueFormat;
	private string musicNameFormat;
	private string earlyCountFormat;
	private string lateCountFormat;

	public static bool convertCompleteFlag;

	private void Start()
	{
		scoreValueFormat = textScore.text;
		exScoreValueFormat = textExScore.text;
		judgementValueFormat = textJudgement.text;
		musicNameFormat = textMusicName.text;
		earlyCountFormat = textEarlyCount.text;
		lateCountFormat = textLateCount.text;

		//各判定の表示フラグをリセットする
		PrintJudgement.printCOMBOflag = 0;
		PrintJudgement.printEPflag = 0;
		PrintJudgement.printPflag = 0;
		PrintJudgement.printGRflag = 0;
		PrintJudgement.printGflag = 0;
		PrintJudgement.printMflag = 0;
		PrintJudgement.printEARLYflag = 0;
		PrintJudgement.printLATEflag = 0;
		//SeFlagとprintJudgementFlagとスクロールスピードとオフセットはリセットしない

		panelTargetGaugeToStarGauge.SetActive(false);   //変換するパネルは最初非表示
		convertCompleteFlag = false;    //初期化

		textScore.text = string.Format(scoreValueFormat,
		// {0}: スコア
		Mathf.CeilToInt(EvaluationManager.Score));

		//ランク
		if (EvaluationManager.Score >= 997500)
			textRank.text = "SSS+";
		else if (EvaluationManager.Score >= 990000)
			textRank.text = "SSS";
		else if (EvaluationManager.Score >= 980000)
			textRank.text = "SS";
		else if (EvaluationManager.Score >= 960000)
			textRank.text = "S";
		else if (EvaluationManager.Score >= 900000)
			textRank.text = "AAA";
		else if (EvaluationManager.Score >= 800000)
			textRank.text = "AA";
		else if (EvaluationManager.Score >= 700000)
			textRank.text = "A";
		else if (EvaluationManager.Score >= 600000)
			textRank.text = "BBB";
		else if (EvaluationManager.Score >= 500000)
			textRank.text = "BB";
		else if (EvaluationManager.Score >= 400000)
			textRank.text = "B";
		else if (EvaluationManager.Score >= 200000)
			textRank.text = "C";
		else
			textRank.text = "D";

		textExScore.text = string.Format(exScoreValueFormat,
		// {0}: EXスコア
		EvaluationManager.ExScore,
		// {1}: コンボ数
		EvaluationManager.Combo,
		// {2}: 最大コンボ数
		EvaluationManager.MaxCombo);

		textJudgement.text = string.Format(judgementValueFormat,


		// {0}: EXPERFECT
		EvaluationManager.GetJudgementCountsExPerfect,
		// {1}: PERFECT
		EvaluationManager.GetJudgementCountsPerfect,
		// {2}: GREAT
		EvaluationManager.GetJudgementCountsGreat,
		// {3}: BAD
		EvaluationManager.GetJudgementCountsGood,
		// {4}: MISS
		EvaluationManager.GetJudgementCountsMiss
		);

		//楽曲名
		textMusicName.text = string.Format(musicNameFormat,
		SelectorController.GetTitle);

		//EarlyLate数
		textEarlyCount.text = string.Format(earlyCountFormat,
		EvaluationManager.Early);
		textLateCount.text = string.Format(lateCountFormat,
		EvaluationManager.Late);

		//detail(FULLCOMBO,ALLPERFECTなど)
		if (EvaluationManager.PassedNotes < EvaluationManager.TheoreticalCombo)    //最後までプレイしていない
		{
			textDetail.text = "Track Skiped";
		}
		else if (EvaluationManager.Combo == EvaluationManager.TheoreticalCombo &&   //ALL PERFECT
		EvaluationManager.GetJudgementCountsMiss == 0 &&
		EvaluationManager.GetJudgementCountsGood == 0 &&
		EvaluationManager.GetJudgementCountsGreat == 0)
		{
			textDetail.text = "ALL PERFECT!!!";
			TotalPlayManager.totalPlay++;   //累計プレイ曲数を増やす
		}
		else if (EvaluationManager.Combo == EvaluationManager.TheoreticalCombo) //FULL COMBO
		{
			textDetail.text = "FULL COMBO!!";
			TotalPlayManager.totalPlay++;   //累計プレイ曲数を増やす
		}
		/*else if (EvaluationManager.Score >= 990000)	//得点が99万点以上
			textDetail.text = "Excellent!";*/
		else if (EvaluationManager.TargetGauge >= 70)   //ノルマゲージが70%以上
		{
			textDetail.text = "Track Complete!";
			TotalPlayManager.totalPlay++;   //累計プレイ曲数を増やす
		}
		else//70%未達成
		{
			textDetail.text = "Track Failed...";
			TotalPlayManager.totalPlay++;   //累計プレイ曲数を増やす
		}


		//ノルマゲージ
		targetGaugeSlider.value = EvaluationManager.TargetGauge;

		//LoveU_EXの解放条件を満たしていたら解放する(フリープレイモード、累計プレイ曲数10以上、レベル18以上の楽曲を最後までプレイ、99万点以上を獲得)
		//SecretManagerへの移動も視野に入れる
		if(TotalPlayManager.totalPlay >= 10 &&
		SelectorController.GetLevel >= 18 && 
		EvaluationManager.PassedNotes >= EvaluationManager.TheoreticalCombo && 
		EvaluationManager.Score >= 990000 &&
		TitleManager.SelectModeFlag == 0 && SecretManager.secretOpenFlag[1] == false)
        {
			SecretManager.secretOpenFlag[1] = true;
			SecretManager.secretPrintFlag = true;
			SecretManager.secretPrintCompleteFlag = false;
		}

		//設定を保存する
		SaveData.SetInt("printSE", OptionToggleController.SEFlag);
		SaveData.SetInt("printJudgement", OptionToggleController.printJudgementFlag);
		SaveData.SetInt("printEL", OptionToggleController.printELFlag);
		SaveData.SetInt("setCamera", OptionToggleController.cameraSetFlag);
		SaveData.SetInt("printCombo", OptionToggleController.printComboFlag);
		SaveData.SetFloat("scrollSpeed", SelectorController.scrollSpeed);
		SaveData.SetFloat("musicOffset", SelectorController.musicOffset);
		//SaveData.SetInt("selectedIndex", SelectorController.selectedIndex);
		float volume;
		if (AudioMixer.GetFloat("MASTER", out volume))
		{
			SaveData.SetFloat("masterVolume", volume);
		}
		if (AudioMixer.GetFloat("BGM", out volume))
		{
			SaveData.SetFloat("bgmVolume", volume);
		}
		if (AudioMixer.GetFloat("SE", out volume))
		{
			SaveData.SetFloat("seVolume", volume);
		}
		SaveData.SetInt("totalPlay", TotalPlayManager.totalPlay);
		SaveData.Save();
	}

	private void Update()
	{
		

		//スペースかESCキーが押されたら次の処理へ
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
		{
			//コレクトモードで、楽曲クリアで、エンディング曲ならエンディングシーンへ移動
			if (TitleManager.SelectModeFlag == 1 && EvaluationManager.TargetGauge >= 70 && EvaluationManager.PassedNotes >= EvaluationManager.TheoreticalCombo && SelectorController.endingMusic == true)
				SceneManager.LoadScene("EndingScene");
			else if (TitleManager.SelectModeFlag == 1 && StarGaugeManager.starGauge < 100)   //コレクトモードで、スターゲージがマックスじゃないならノルマゲージをスターゲージへ変換
            {
				panelTargetGaugeToStarGauge.SetActive(true);
				StartCoroutine(TargetGaugeToStarGauge());
			}
			else
				SceneManager.LoadScene("SelectScene");	//フリープレイモードかスターゲージmaxならそのまま選曲画面へ移動
		}

		//変換が終わったらセレクト画面に戻る
		if(convertCompleteFlag == true)
        {
			StartCoroutine(ToSelectScene());
        }
	}
	private IEnumerator TargetGaugeToStarGauge()
    {
		yield return new WaitForSeconds(2);

		StarGaugeManager.convertStartFlag = true;
	}

	private IEnumerator ToSelectScene()
    {
		yield return new WaitForSeconds(2);

		SceneManager.LoadScene("SelectScene");
	}
}

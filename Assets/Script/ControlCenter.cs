using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControlCenter : MonoBehaviour {

    //players
    public List<Player> players;
	//ui
	public Text _describe;
	public Text[] _options = new Text[4];

    public Text time_txt;
    public Animator timeAnim;

    //point & score
    public float _correctPoint;
    public float _totalPoint;
    //question data
    public int _curQuesNo;  //START FROM 1
	public string _curAns;
    //timer
    public float time_cur;
    public float time_total = 15;
    int countPlayer = 0;    //該提已做答人數

    void Awake () {
		_options = GameObject.Find("Options").GetComponentsInChildren<Text>();
        RefreshUI();
    }
	
	void Update () {
        Timer();
    }

    //註冊player，回傳編號
    public int SetPlayer(Player who) {
        int myNumer;
        players.Add(who);
        myNumer = players.Count;
        return myNumer;
    }

    //載入新題目
	void LoadQuestion(int num){
		if(LocalJson.qDatas == null){
			//load Json
			LocalJson.AddressToClass("question");
			Debug.Log("Load qData");
		}
        if (num <= LocalJson.qDatas.Length && num > 0) {

            LocalJson.QuestionData qData = LocalJson.qDatas[num - 1];
            //load describe
            _describe.text = qData.describe;
            //load option
            _options[0].text = qData.option_A;
            _options[1].text = qData.option_B;
            _options[2].text = qData.option_C;
            _options[3].text = qData.option_D;
            //load ans
            _curAns = qData.ans;

            _curQuesNo = num;            
            Debug.Log("Load Question No: " + _curQuesNo);
        } else {
            //reset num
            LoadQuestion(1);
		}
	}

    //判斷答案
    public void SentAnswer(string option, Player who){
		if(option == _curAns){
			Debug.Log(who + " is correct");
			CountScore(true, who);
		}else{
			Debug.Log(who + " is wrong");
			CountScore(false, who);
		}
	}

    
	//分數計算
	void CountScore(bool isCorr, Player who){
        countPlayer += 1;

		if(isCorr)
            who._curScore += _correctPoint;

        //雙方做答完畢
        if (countPlayer == 2) {
            time_cur = -.5f;    //進入間隔時間(break time)
            countPlayer = 0;
        }
		RefreshUI();
	}

    //重啟按鈕(關閉在player.cs做)
    void ActiveBtn() {
        for (int i = 0; i < players.Count; i++) {
            players[i].ActiveBtn(true);
        }
    }

    //刷新UI
	void RefreshUI(){
        for (int i = 0; i < players.Count; i++) {
            players[i].ReFreshUI();
        }
	}

    void Timer() {
        //題目倒數計時中
        if (time_cur > 0) {
            timeAnim.SetBool("counting", true);
            time_txt.text = Mathf.FloorToInt(time_cur).ToString();
            time_cur -= Time.deltaTime;
        }
        //進入間隔時間(break time)
        else {
            timeAnim.SetBool("counting", false);
            FadeTxt(false);
            time_cur = 0;
            time_txt.text = "";
            BreakTime();
        }
    }

    float curBreakT = 2;
    public float tolBreakT = 2;     //間隔時間長度

    void BreakTime() {
        //間隔時間結束，重置
        if (curBreakT <= 0) {
            Debug.Log("time's up, next question!");
            LoadQuestion(_curQuesNo + 1);
            time_cur = time_total;
            curBreakT = tolBreakT;
            ActiveBtn();
            FadeTxt(true);
        }
        //間隔執行中
        else 
        {
            curBreakT -= Time.deltaTime;
        }
    }
    void FadeTxt(bool isEnable) {
        for (int i = 0; i < _options.Length; i++) {
            _options[i].enabled = isEnable;
        }
        _describe.enabled = isEnable;
    }
}

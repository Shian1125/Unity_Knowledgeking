using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ControlCenter : NetworkBehaviour {

    //玩家註冊
    public List<Player> players;
    public Player _selfPlayer;
	//ui
    public Text time_txt;
    public Animator timeAnim;

    public int _curQuesNo;  //START FROM 1

    //timer
    [SyncVar]
    public float time_cur;
    public float time_total = 15;

    public int _ansCount = 0;    //目前作答人數
    [SyncVar]
    public bool _timerIsOn;

    void Awake () {
       
    }
	
	void Update () {
        Timer();
    }

    //註冊player，回傳編號
    public int SetPlayer(Player who) {
        int myNumer;
        players.Add(who);
        myNumer = players.Count;
        //註冊自己的UI
        if (who.name == "Player_self") {
            _selfPlayer = who;
        }
        //人數湊齊，開始載入題目(sever端)
        if (isServer && players.Count == 2) {
            Debug.Log("GAME START!");
            //開始計時
            //CmdTimerStart();
            _timerIsOn = true;
            //載入題目
            CmdLoadNextQuestion();
        }
        return myNumer;
    }

    //檢查作答人數
    [Command]
    public void CmdCheckAnsCount(string name) {
        Debug.Log(name + " is answered");
        _ansCount += 1;
        //雙方做答完畢
        if (_ansCount == 2) {
            Debug.Log("all answered");
            //CmdReSetTimer();    //進入間隔時間(break time)
            time_cur = -.5f;
            _ansCount = 0;
        }
    }

    //載入題目
    [Command]
    public void CmdLoadNextQuestion() {

        for (int i = 0; i < 2; i++) {
            players[i].RpcLoadQuestion(_curQuesNo + 1);
        }
        _curQuesNo += 1;
    }

    //重啟按鈕(關閉在player.cs做)
    [Command]
    void CmdActiveBtn() {
        for (int i = 0; i < players.Count; i++) {
            players[i].RpcActiveBtn(true);
        }
    }

    void Timer() {
        if(_timerIsOn) {
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
    }

    [SyncVar]
    public float curBreakT = 2;
    public float tolBreakT = 2;     //間隔時間長度

    void BreakTime() {
        //間隔時間結束，重置
        if (curBreakT <= 0) {
            Debug.Log("time's up, next question!");
            CmdLoadNextQuestion();
            time_cur = time_total;
            curBreakT = tolBreakT;
            CmdActiveBtn();
            _ansCount = 0;
            FadeTxt(true);
        }
        //間隔執行中
        else 
        {
            curBreakT -= Time.deltaTime;
        }
    }

    //隱藏UI
    void FadeTxt(bool isEnable) {
        //ops
        for (int i = 0; i < _selfPlayer._options.Length; i++) {
            //_selfPlayer._options[i].GetComponentInChildren<Text>().enabled = isEnable;
        }
        //des
        //_selfPlayer._qusDescribe.enabled = isEnable;

        //set color
        
        if (isEnable) {
            for (int i = 0; i < _selfPlayer._options.Length; i++) {
                _selfPlayer._options[i].GetComponent<Image>().color = _selfPlayer._colorDefault;
            }
        }
    }
}

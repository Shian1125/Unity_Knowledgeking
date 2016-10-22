using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    public ControlCenter _controlCenter;
    public int myPlayerNum;
    //data
    public float _curScore;     //目前分數
    public float _tolScore;     //總分
    public float _addPoint;     //答對加分
    //UI
    public Image _icon;
    public Button[] _options;
    public Slider _slider;
    public Text _scoreTxt;
    public Text _qusDescribe;
    public int _curAns;
    //color
    public Color _colorDefault;
    public Color _colorCorrect;
    public Color _colorWrong;
    //icon
    public Image _iconImage;
    public Sprite p1;
    public Sprite p2;
    // Use this for initialization
    void Start () {
        StartCoroutine("WaitSpawnController");
        SetUIPosition();        
    }
    void SetUIPosition() {
        if (isLocalPlayer) {
            this.name = "Player_self";
            _iconImage.sprite = p1;
            _icon.rectTransform.anchorMax = new Vector2(1, 1);
            _icon.rectTransform.anchorMin = new Vector2(1, 1);
            _icon.rectTransform.pivot = new Vector2(1, 1);
            _slider.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            _slider.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            _slider.GetComponent<RectTransform>().pivot = new Vector2(1, 0);
            _slider.GetComponent<RectTransform>().anchoredPosition = new Vector3(_slider.GetComponent<RectTransform>().anchoredPosition.x * -1, _slider.GetComponent<RectTransform>().anchoredPosition.y, 0);
        } else {
            _iconImage.sprite = p2;
            //其他人的UI關閉
            _options[0].transform.parent.gameObject.SetActive(false);
            _qusDescribe.gameObject.transform.parent.gameObject.SetActive(false);
        }
        
    }
    private IEnumerator WaitSpawnController() {
        yield return new WaitForSeconds(1.0f);
        _controlCenter = GameObject.FindObjectOfType<ControlCenter>();
        print("_controlCenter spawned " + Time.time);
        SetPlayer();
    }
    //註冊玩家編號和選項
    void SetPlayer() {

        myPlayerNum = _controlCenter.SetPlayer(this);
        //this.name = "Player_" + myPlayerNum.ToString();
    }
    //載入新題目
    [ClientRpc]
    public void RpcLoadQuestion(int num) {
        if (LocalJson.qDatas == null) {
            //load Json
            LocalJson.AddressToClass("question");
            Debug.Log("Load qData");
        }
        if (num <= LocalJson.qDatas.Length && num > 0) {
            LocalJson.QuestionData qData = LocalJson.qDatas[num - 1];
            //load describe
            _qusDescribe.text = qData.describe;

            _options[0].GetComponentInChildren<Text>().text = qData.option_A;
            _options[1].GetComponentInChildren<Text>().text = qData.option_B;
            _options[2].GetComponentInChildren<Text>().text = qData.option_C;
            _options[3].GetComponentInChildren<Text>().text = qData.option_D;

            //load ans
            _curAns = int.Parse(qData.ans);
            Debug.Log(this.name + " is loaded");
        } else {
            //reset num
            _controlCenter.CmdLoadNextQuestion();
        }
    }

    //選項按鈕
    public void Btn_OptionA() { CmdSendAnsToCenter(1); }
    public void Btn_OptionB() { CmdSendAnsToCenter(2); }
    public void Btn_OptionC() { CmdSendAnsToCenter(3); }
    public void Btn_OptionD() { CmdSendAnsToCenter(4); }


    //開關Btn
    [ClientRpc]
    public void RpcActiveBtn(bool isActive) {
        for (int i = 0; i < _options.Length; i++) {
            _options[i].enabled = isActive;
        }
    }

    //傳送給server答案
    [Command]
    void CmdSendAnsToCenter(int op) {
        RpcActiveBtn(false);
        CmdSentAnswer(op);
        _controlCenter.CmdCheckAnsCount(this.name);
    }

    //判斷答案
    [Command]
    public void CmdSentAnswer(int op) {
        if (op == _curAns) {
            RpcReFreshUI(_addPoint);
            RpcAnsUI(true, op);
            Debug.Log(this.name + " is correct");
        } else {
            RpcReFreshUI(0);
            RpcAnsUI(false, op);
            Debug.Log(this.name + " is wrong");
        }
    }
    //刷新UI
    [ClientRpc]
    void RpcReFreshUI(float adpoint) {
        _curScore += adpoint;
        _slider.value = _curScore / _tolScore;
        _scoreTxt.text = _curScore.ToString();
    }
    //是否答對按鈕亮燈提示
    [ClientRpc]
    void RpcAnsUI(bool isCorrect, int op) {
        
        if (isCorrect) {
            _options[op - 1].GetComponent<Image>().color = _colorCorrect;
        } else {
            _options[op - 1].GetComponent<Image>().color = _colorWrong;
            _options[_curAns - 1].GetComponent<Image>().color = _colorCorrect;
        }
    }
}

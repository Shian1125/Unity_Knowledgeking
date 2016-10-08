using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    ControlCenter _controlCenter;
    public int myPlayerNum;
    //data
    public float _curScore;
    //UI
    public Button[] _options;
    public Slider _slider;
    public Text _scoreTxt;
    // Use this for initialization
    void Start () {
        _controlCenter = GameObject.Find("ControlCenter").GetComponent<ControlCenter>();
        SetPlayer();
    }

    //註冊玩家編號
    void SetPlayer() {
        myPlayerNum = _controlCenter.SetPlayer(this);
    }
    public void Btn_OptionA(){
        ActiveBtn(false);
        _controlCenter.SentAnswer("A", this);
    }
    public void Btn_OptionB() {
        ActiveBtn(false);
        _controlCenter.SentAnswer("B", this);
    }
    public void Btn_OptionC() {
        ActiveBtn(false);
        _controlCenter.SentAnswer("C", this);
    }
    public void Btn_OptionD() {
        ActiveBtn(false);
        _controlCenter.SentAnswer("D", this);
    }

    public void ActiveBtn(bool isActive) {
        for (int i = 0; i < _options.Length; i++) {
            _options[i].enabled = isActive;
        }
    }
    public void ReFreshUI() {
        _slider.value = _curScore / _controlCenter._totalPoint;
        _scoreTxt.text = _curScore.ToString();
    }
}

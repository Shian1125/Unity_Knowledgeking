using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlCenter : MonoBehaviour {

	//ui
	public Text _describe;
	public Text[] _options = new Text[4];
	public Slider _sliderPlayer;
	public Slider _sliderAI;
	Text _sliderTextPlayer;
	Text _sliderTextAI;
	//score data
	public float _totalPoint;
	public float _correctPoint;
	public float _curScorePlayer = 0;
	public float _curScoreAI = 0;
	//question data
	public int _curQuesNo;
	public string _curAns;
	// Use this for initialization
	void Start () {
		_options = GameObject.Find("Options").GetComponentsInChildren<Text>();
		_sliderTextPlayer = _sliderPlayer.gameObject.transform.FindChild("Text").GetComponent<Text>();
		_sliderTextAI = _sliderAI.gameObject.transform.FindChild("Text").GetComponent<Text>();
		RefreshUI();
		Btn_Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Btn_Start(){
		LoadQuestion(_curQuesNo);
	}
	void LoadQuestion(int num){
		if(LocalJson.qDatas == null){
			//load Json
			LocalJson.AddressToClass("question");
			Debug.Log("Load qData");
		}
		if(num <= LocalJson.qDatas.Length && num > 0){
			
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
			Debug.Log("Load Question No: " + _curQuesNo);
			_curQuesNo = num;
		}else{
			Debug.Log("num is out of range");
		}
		

	}
	//處理答案判斷
	public void SentAnswer(string option){
		if(option == _curAns){
			Debug.Log("correct");
			//add points
			CountScore(true);
			//load next question
			LoadQuestion(_curQuesNo + 1);
		}else{
			Debug.Log("wrong");
			CountScore(false);
		}
			
	}
	//分數計算
	void CountScore(bool isCorr){
		if(isCorr)
			_curScorePlayer += _correctPoint;
		RefreshUI();
	}
	public void Btn_OptionA(){	SentAnswer("A");	}
	public void Btn_OptionB(){	SentAnswer("B");	}
	public void Btn_OptionC(){	SentAnswer("C");	}
	public void Btn_OptionD(){	SentAnswer("D");	}
	void RefreshUI(){
		_sliderPlayer.value = _curScorePlayer / _totalPoint;
		_sliderTextPlayer.text = _curScorePlayer.ToString();
		_sliderAI.value = _curScoreAI / _totalPoint;
		_sliderTextAI.text = _curScoreAI.ToString();
	}
}

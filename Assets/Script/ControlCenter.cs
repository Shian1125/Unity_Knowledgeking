using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlCenter : MonoBehaviour {

	public Text _describe;
	public Text[] _options = new Text[4];
	public int questionNum;
	public string _ans;
	// Use this for initialization
	void Start () {
		_options = GameObject.Find("Options").GetComponentsInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Btn_Start(){
		LoadQuestion(questionNum);
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
			_ans = qData.ans;
		}else{
			Debug.Log("num is out of range");
		}
		

	}
}

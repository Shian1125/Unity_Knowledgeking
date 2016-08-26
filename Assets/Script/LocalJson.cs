using UnityEngine;
using System.Collections;
using JsonFx.Json;

public class LocalJson : MonoBehaviour {

	public static TextAsset _txtJson;
	public static QuestionData[] qDatas;
	public class QuestionData{
		public string id;
		public string describe;
		public string option_A;
		public string option_B;
		public string option_C;
		public string option_D;
		public string ans;
	}
	public static void AddressToClass(string address){
		//load txt from Resources
		_txtJson = Resources.Load(address) as TextAsset;

		//transform txt to json object(QuestionData[])
		qDatas = JsonReader.Deserialize<QuestionData[]>(_txtJson.text);
	}
}

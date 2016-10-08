using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UITween : MonoBehaviour {

    public Image timer;
    public Text _textDescribe;
    public Text _textOpA;
    public Text _textOpB;
    public Text _textOpC;
    public Text _textOpD;
    // Use this for initialization
    void Start () {
        TextUp();
        Rota();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void FadeIn() {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(timer.DOFade(0,1));
        mySequence.OnComplete(FadeInOver);
    }
    void FadeInOver() {
        Debug.Log("gg");
    }
    void TextUp() {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Join(_textDescribe.DOFade(0, 1));
        mySequence.Join(_textOpA.DOFade(0, 1));
        mySequence.Join(_textOpB.DOFade(0, 1));
        mySequence.Join(_textOpC.DOFade(0, 1));
        mySequence.Join(_textOpD.DOFade(0, 1));
        mySequence.OnComplete(FadeInOver);
    }
    void Rota() {
        timer.gameObject.transform.DORotate(new Vector3(0, 0, -30), 1);
    }
}

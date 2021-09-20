using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Wheel : MonoBehaviour
{
    public GameObject slice;

    public float sliceSize;

    public float spinSpeed;
    private float curSpinSpeed;

    public float oldZ;
    public float newZ;
    public int index;

    public string[] content;
    public Color[] colors;
    public Text contentText;
    public Text titleText;


    public void OpenWheel(string wheel){
        string w = PlayerPrefs.GetString(wheel);
        string[] wheelInfo = w.Split(':');

        titleText.text = wheelInfo[0];

        content = wheelInfo[1].Split(',');
        colors = new Color[content.Length];

        string[] temp = wheelInfo[2].Split(',');
        for(int x = 0; x < temp.Length; x++){
            Color n;
            Debug.Log(temp[x]);
            if(ColorUtility.TryParseHtmlString("#"+temp[x], out n)){
                colors[x] = n;
            }
        }

        SetWheel();

    }


    void SetWheel(){
        sliceSize = 1f/content.Length;

        for(int x = 0; x < content.Length; x++){
            GameObject s = Instantiate(slice, transform.position, transform.rotation).gameObject;
            s.transform.SetParent(this.transform);
            s.transform.Rotate(new Vector3(0, 0, sliceSize*x*360));
            s.GetComponent<Image>().fillAmount = sliceSize;
            s.GetComponent<Image>().color = colors[x];
        }
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            curSpinSpeed = UnityEngine.Random.Range(spinSpeed+15, spinSpeed-5);
            //oldZ = transform.localEulerAngles.z;
        }

        if(curSpinSpeed > .1){
            transform.Rotate(0,0, curSpinSpeed);
            curSpinSpeed *= .99f;
        }


        newZ = transform.localEulerAngles.z;

        if(oldZ + sliceSize*360 < newZ){
            oldZ = oldZ + sliceSize*360;
            index ++;
        }

        if(newZ < sliceSize*360){
            oldZ = 0;
            index = 0;
        }

        contentText.text = content[index];
    }
}

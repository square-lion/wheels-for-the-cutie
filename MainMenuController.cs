using System.Collections;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject wheelSlot;

    public GameObject createWheelScreen;
    public GameObject wheelScreen;
    
    void Start(){
        LoadWheels();
    }
    
    public void LoadWheels(){
        if(PlayerPrefs.GetInt("Wheels") == 0)
            return;

        string w = PlayerPrefs.GetString("W");

        while(w.Substring(0,1) != "W"){
            w = w.Substring(1);
        }
        PlayerPrefs.SetString("W", w);

        string[] ws = w.Split(':');

        Debug.Log(w);

        for(int x = 0; x < ws.Length; x++){
            GameObject slot = Instantiate(wheelSlot,transform.position, transform.rotation);
            slot.transform.parent = transform;
            slot.transform.localScale = new Vector3(1,1,1);
            slot.transform.localPosition = new Vector3(0, 680 - 280 * x);
            slot.GetComponent<WheelSlot>().id = ws[x];

            string wheel = PlayerPrefs.GetString(ws[x]);
            string[] wheelInfo = wheel.Split(':');

            slot.transform.GetChild(1).GetComponent<Text>().text = wheelInfo[0];
        }
    }

    public void NewWheel(){
        createWheelScreen.SetActive(true);
    }

    public void CloseNewWheel(){
        //createWheelScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WheelClicked(string id){
        wheelScreen.SetActive(true);

        FindObjectOfType<Wheel>().OpenWheel(id);
    }

    public void CloseWheelView(){
        //wheelScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EditWheel(string id){
        createWheelScreen.SetActive(true);
        createWheelScreen.GetComponent<CreateWheelController>().OpenWheel(id);
    }
}

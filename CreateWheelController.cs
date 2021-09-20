using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateWheelController : MonoBehaviour
{
    public CUIColorPicker colorPicker;
    public Transform wheelContent;

    public int numOfSlots = 2;
    public GameObject slot;
    public List<GameObject> slots;

    public InputField wheelName;

    public Image curColorImage;

    public Button save;
    public Button create;
    public Button delete;

    //For edit
    public string wID;

    void Awake(){
        colorPicker.transform.parent.gameObject.SetActive(false);

        save.gameObject.SetActive(false);
        delete.gameObject.SetActive(false);
        create.gameObject.SetActive(true);
    }

    public void ColorPreviewClicked(Image p){
        colorPicker.transform.parent.gameObject.SetActive(true);
        curColorImage = p;
    }

    //When Done Picking Color
    public void SetColor(){
        curColorImage.color = colorPicker.Color;
        colorPicker.transform.parent.gameObject.SetActive(false);
    } 

    public void NewSlot(){
        if(numOfSlots >= 12)
            return;

        GameObject g = CreateSlot();

    }

    public GameObject CreateSlot(){
        numOfSlots++;
        GameObject g = Instantiate(slot, transform.position, transform.rotation);
        g.transform.SetParent(wheelContent);
        g.transform.localPosition = new Vector3(0, -120 * numOfSlots, 0);
        g.transform.localScale = new Vector3(1,1,1);
        g.transform.GetChild(1).GetComponent<Text>().text = "" + numOfSlots;
        g.transform.GetChild(2).GetComponent<Image>().color = new Color(UnityEngine.Random.Range(0f,1), UnityEngine.Random.Range(0f,1), UnityEngine.Random.Range(0f,1));
        slots.Add(g);
        return g;
    }

    public void RemoveSlot(){
        if(numOfSlots <= 2)
            return;

        numOfSlots--;
        Destroy(slots[numOfSlots]);
        slots.RemoveAt(numOfSlots);
    } 

    public void Create(){
        int numOfWheels = PlayerPrefs.GetInt("Wheels");
        numOfWheels++;
        string wID =  wheelName.text + ":" + GetNames() + ":" + GetColors();
        PlayerPrefs.SetString("W"+numOfWheels, wID);
        PlayerPrefs.SetInt("Wheels", numOfWheels);
        PlayerPrefs.SetString("W", PlayerPrefs.GetString("W") + ":W" + numOfWheels);

        FindObjectOfType<MainMenuController>().LoadWheels();
        gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    string GetNames(){
        string f = "";

        foreach(GameObject s in slots){
            f += s.transform.GetChild(0).GetComponent<InputField>().text + ",";
        }

        return f.Substring(0,f.Length-1);
    }

    string GetColors(){
        string f = "";

        foreach(GameObject s in slots){
            f += ColorUtility.ToHtmlStringRGB(s.transform.GetChild(2).GetComponent<Image>().color) + ",";
        }

        return f.Substring(0,f.Length-1);
    }

    public void OpenWheel(string id){
        save.gameObject.SetActive(true);
        delete.gameObject.SetActive(true);
        create.gameObject.SetActive(false);

        wID = id;

        string[] wheelInfo = PlayerPrefs.GetString(id).Split(':');
        wheelName.text = wheelInfo[0];

        string[] contents = wheelInfo[1].Split(',');

        string[] temp = wheelInfo[2].Split(',');
        Color[] colors =  new Color[contents.Length];
        Color n;
        for(int x = 0; x < contents.Length; x++){

            if(ColorUtility.TryParseHtmlString("#"+temp[x], out n)){
                colors[x] = n;
            }
        }

        for(int x = 0; x < contents.Length; x++){
            if(x < 2){
                slots[x].transform.GetChild(0).GetComponent<InputField>().text = contents[x];
                slots[x].transform.GetChild(2).GetComponent<Image>().color = colors[x];
            }
            else{
                GameObject g = CreateSlot();
                g.transform.GetChild(0).GetComponent<InputField>().text = contents[x];
                g.transform.GetChild(2).GetComponent<Image>().color = colors[x];
            }
        }     
    }

    public void Save(){
        PlayerPrefs.SetString(wID, wheelName.text + ":" + GetNames() + ":" + GetColors());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }

    public void Delete(){
        string wheels = PlayerPrefs.GetString("W");
        PlayerPrefs.SetString("W", wheels.Replace(":" + wID, ""));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

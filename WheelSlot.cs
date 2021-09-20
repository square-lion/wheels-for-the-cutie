using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSlot : MonoBehaviour
{
    public string id;

    public void Clicked(){
        FindObjectOfType<MainMenuController>().WheelClicked(id);
    }

    public void Edit(){
        FindObjectOfType<MainMenuController>().EditWheel(id);
    }
}

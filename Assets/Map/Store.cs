﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Store : MonoBehaviour
{
    GameObject ItemSlot;
    Vector3 Pos;
    // Start is called before the first frame update
    void Start()
    {
        ItemSlot = GameObject.Find("ItemSlot");
        Pos = ItemSlot.transform.position;//상점 가기 전 위치 
    }

    // Update is called once per frame
    public void OnClick()
    {
        ItemSlot.transform.position = new Vector3(Pos.x + 10, Pos.y, Pos.z);
        SceneManager.LoadScene("Store_Scene");
    }
}

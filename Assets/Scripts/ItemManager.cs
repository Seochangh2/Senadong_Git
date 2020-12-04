﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject Bab_Prefab;
    public GameObject Egg_Prefab;
    public GameObject Milk_Prefab;

    bool isbap=false;

    public int hungry_item = 0;
    public int poop_item = 0;
    public int play_item = 0;
    public int egg_item = 0;
    public int milk_item = 0;
    public TextMesh count;

    public int coin = 0;    

    // Start is called before the first frame update
    void Start()
    {
        hungry_item = 5;
        poop_item = 5;
        play_item = 5;

        coin = 2000;
     }
   
    // Update is called once per frame
    void Update()
    {       
        count = GameObject.FindWithTag("hungry_count").GetComponent<TextMesh>();
        count.text = hungry_item.ToString();

        count = GameObject.FindWithTag("play_count").GetComponent<TextMesh>();
        count.text = play_item.ToString();
 
        count = GameObject.FindWithTag("egg_count").GetComponent<TextMesh>();
        count.text = egg_item.ToString();

        count = GameObject.FindWithTag("milk_count").GetComponent<TextMesh>();
        count.text = milk_item.ToString();

        count = GameObject.FindWithTag("poop_count").GetComponent<TextMesh>();
        count.text = poop_item.ToString();

        count = GameObject.FindWithTag("coin").GetComponent<TextMesh>();
        count.text = coin.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "hungry_item_slot" && hungry_item > 0)  //hungry 아이템 슬롯 누름 
                {
                    Vector3 bapPos;
                    GameObject bap = GameObject.Instantiate(Bab_Prefab);
                    
                    bapPos = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
                    bap.transform.position = bapPos;
                }
                if (hit.transform.gameObject.tag == "egg_item_slot" && egg_item > 0)  //hungry 아이템 슬롯 누름 
                {
                    Vector3 eggPos;
                    GameObject egg = GameObject.Instantiate(Egg_Prefab);

                    eggPos = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
                    egg.transform.position = eggPos;
                }
                if (hit.transform.gameObject.tag == "milk_item_slot" && milk_item > 0)  //hungry 아이템 슬롯 누름 
                {
                    Vector3 milkPos;
                    GameObject milk = GameObject.Instantiate(Milk_Prefab);

                    milkPos = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
                    milk.transform.position = milkPos;
                }
            }
        }
    }
}

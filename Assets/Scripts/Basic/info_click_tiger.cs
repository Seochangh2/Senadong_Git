﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class info_click_tiger : MonoBehaviour
{
    GameObject FloatingValue;
    Tiger_Move tiger;

    GameObject hp_bar;      //hp바
    float hpbar_sx;         //hp바 스케일 x값
    float hpbar_tx;         //hp바 위치 x값
    float hpbar_tmp;        //hp바 감소 정도
    public int hungry_pre = 1000, poop_pre = 1000, play_pre = 1000; //이전 속성 값
    public int hungry_child = 2, poop_child = 4, play_child = 6;

    // Start is called before the first frame update
    void Start()
    {
        tiger = transform.GetComponent<Tiger_Move>();
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        if (this.transform.gameObject == transform.gameObject) //info_icon 클릭
        {
            FloatingValue = transform.GetChild(0).gameObject;

            FloatingValue.SetActive(true);
            hpMove(hungry_child, ref hungry_pre, (tiger.hungry > hungry_pre ? tiger.hungry - hungry_pre : hungry_pre - tiger.hungry));
            hpMove(poop_child, ref poop_pre, (tiger.poop > poop_pre ? tiger.poop - poop_pre : poop_pre - tiger.poop));
            hpMove(play_child, ref play_pre, (tiger.play > play_pre ? tiger.play - play_pre : play_pre - tiger.play));
            StartCoroutine(Disabled(2.0f));

        }
    }
  
    IEnumerator Disabled(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        FloatingValue.SetActive(false);
    }
    public void hpMove(int child, ref int value, int delta)    //hp바 동작 구현
    {
        hp_bar = FloatingValue.transform.GetChild(child).gameObject;
        hpbar_sx = hp_bar.transform.localScale.x;
        hpbar_tx = hp_bar.transform.localPosition.x;
        hpbar_tmp = hpbar_sx / tiger.valueMax;   //최대 체력에 따른 hp바 이동량 설정

        if (delta < 0)
        {
            float move = ((tiger.valueMax - value) + delta) * hpbar_tmp; //hp바 이동할 크기
            value -= delta; //hp 재설정

            Vector3 Scale = hp_bar.transform.localScale;    //현재 스케일 값
            hp_bar.transform.localScale = new Vector3(hpbar_sx - move, Scale.y, Scale.z);

            Vector3 Pos = hp_bar.transform.localPosition;   //현재 포지션 값
            hp_bar.transform.localPosition = new Vector3(hpbar_tx - move / 2.0f, Pos.y, Pos.z);
        }
        if (delta > 0)
        {
            if (value + delta > tiger.valueMax)
                delta = (tiger.valueMax - value);

            float move = ((tiger.valueMax - value) + delta) * hpbar_tmp; //hp바 이동할 크기
            value -= delta; //hp 재설정

            Vector3 Scale = hp_bar.transform.localScale;    //현재 스케일 값
            hp_bar.transform.localScale = new Vector3(hpbar_sx + move, Scale.y, Scale.z);

            Vector3 Pos = hp_bar.transform.localPosition;   //현재 포지션 값
            hp_bar.transform.localPosition = new Vector3(hpbar_tx + move / 2.0f, Pos.y, Pos.z);
        }
    }
}

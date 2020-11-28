﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Tiger_Move : MonoBehaviour
{
    int hungryTime = 0; // 배고픔 재는 시간
    int BasicTime = 0; // 기본 움직임 재는 시간
    public int playTime = 0; // 심심한 시간 재는 시간

    public bool playing = false; // 노는중 인지
    bool hunger; // 배고픈 상태인지 
    bool moving; // 움직이고 있는 상태 인지
    float move_length = 0; // 얼마나 움직였는지의 벡터
    float trace_length = 0;
    int check = 0;
    public bool trace_mouse; // 마우스를 향해 달리는 중인지

    Vector2 move_vec; // 움직일 방향벡터
    Vector3 Start_Point; // 움직일때의 시작점
    Vector3 trace; // 마우스와 오브젝트 사이의 벡터 
    Vector3 Mouse;

    public GameObject t_Poop;//똥
    public GameObject t_Hungry;//밥
    public GameObject t_Play;//놀이

    //밥 추적 위함 
    GameObject Bap;
    public float follow_distance = 15;//밥 추적 범위 
    float distance;
    bool is_follow_food = false;//밥 추적 중인지
    //

    //속성값 관련
    int statTime = 200, statMax = 400;  //말풍선 지속 시간
    public int timer = 0;               //타이머
    int nonTimer = 300, ninTimerMax = 300; bool isNon = false;
    int hungryTimer = 500, hungryTimerMax;
    int poopTimer = 700, poopTimerMax;
    int playTimer = 600, playTimerMax;

    int valueMax = 1000;
    public int hungry = 0; bool isHungry = false;
    public int poop = 0; bool isPoop = false;
    public int play = 0; bool isPlay = false;

    //속성관련 오브젝트 -자식
    public GameObject fHungry;                 //체력 오브젝트
    public GameObject fPoop;                   //청결 오브젝트
    public GameObject fPlay;                   //흥미 오브젝트 

    //청결 관련
    public int countPoop = 0;           //똥 개수
    public GameObject TigerPoopPrefab;    //똥 오브젝트
    Vector3 toiletPos;                  //화장실 내 목표 위치
    float tx, ty;                       //화장실 내 목표 위치 x,y (랜덤)

    // Start is called before the first frame update
    public bool Hungry()
    {
        if ((hungry != valueMax && hungryTimer == 0 /*&& hungry<60*/)
            && (!isHungry && !isPoop && !isPlay && !isNon))
        {
            hungryTimerMax = Random.Range(300, 900);
            hungryTimer = hungryTimerMax;
            isHungry = true;
            fHungry.SetActive(true);
        }

        if(isHungry)    // 상태 유지
        {
            statTime--;
            if(statTime == 0)
            {
                isHungry = false;
                fHungry.SetActive(false);
                statTime = statMax;
                nonTimer = 0;
            }
        }

        return true;

    }
    public bool Poop()
    {
        if ((poop != valueMax && poopTimer == 0/*&& poop<60*/)
            && (!isHungry && !isPoop && !isPlay && !isNon))
        {
            poopTimerMax = Random.Range(300, 900);
            poopTimer = poopTimerMax;
            isPoop = true;
            fPoop.SetActive(true);

            //화장실 내 랜덤한 위치 설정
            tx = Random.Range(-12.6f, -7.0f);
            ty = Random.Range(4.19f, 6.76f);
            toiletPos = new Vector3(tx, ty, transform.position.z);
            Debug.Log("toiletPos: " + toiletPos);
        }
        return true;
    }

    public bool Play()
    {
        if ((play != valueMax && playTimer == 0/*&& play<60*/)
            && (!isHungry && !isPoop && !isPlay && !isNon))
        {
            playTimerMax = Random.Range(300, 900);
            playTimer = playTimerMax;
            isPlay = true;
            fPlay.SetActive(true);
        }

        if (isPlay)    // 상태 유지
        {
            statTime--;
            if (statTime == 0)
            {
                isPlay = false;
                fPlay.SetActive(false);
                statTime = statMax;
                nonTimer = 0;
            }
        }
        return true;
    }

    public bool Tiger_Follow_Food()//알아서 바꿔서 쓰셈 
    {
        if (is_follow_food)//밥 생성 되었는지 
        {
            Debug.Log("밥 생성, 거리 추적 범위");
            //Debug.Log("밥 위치: ", Bap.transform);
            if (Bap.transform.position.x < transform.position.x)//밥이 왼쪽 이라면 
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 1);
            }
            else//밥이 오른쪽이라면 
            {
                transform.localScale = new Vector3(-0.8f, 0.8f, 1);
            }
            transform.position = Vector3.Lerp(transform.position, Bap.transform.position, 0.008f);
            return true;
        }
        else
        {
            return true;
        }
    }

    public bool FollowMouse()
    {
        if (playing) // 놀고 있는 상태
        {

            if (trace_mouse == true) // 추적 중일때
            {
                float x = Input.mousePosition.x / 1368.0f; // 화면 비율에 맞춘 마우스 좌표 0 ~ 1
                float y = Input.mousePosition.y / 768.0f;
                Mouse = new Vector3(x, y, -8);
                x = gameObject.transform.position.x / 26f + 0.5f; // 화면 비율에 맞춘 호랑이좌표 0~1 
                y = gameObject.transform.position.y / 13f + 0.5f;
                Start_Point = new Vector3(x, y, -8);

                trace = (Mouse - Start_Point).normalized; // 호랑이와 마우스 사이의 벡터
                if (trace.x >= 0)
                    gameObject.transform.localScale = new Vector3(-1, 1, 1); // 왼쪽으로 움직인다면 왼쪽을 봄
                else
                    gameObject.transform.localScale = new Vector3(1, 1, 1); // 오른쪽이라면 오른쪽을 봄


                x = (Start_Point.x + (trace.x * trace_length) - 0.5f) * 26f; // (시작점 + 방향벡터 * 거리)를 화면이 아닌 유니티의 좌표로 바꿔줌
                y = (Start_Point.y + (trace.y * trace_length) - 0.5f) * 13f;



                gameObject.transform.position = new Vector3(x, y, Start_Point.z); // 이동

                trace_length += 0.0001f; // 빨라지는 추적속도
                if (Vector3.Distance(Start_Point, Mouse) < 0.02f) // 마우스를 잡았다면
                {
                    if (check == 5) //총 5번 잡았다면
                    {
                        playing = false; // 놀이끝 
                        check = 0; //초기화
                        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Start_Point.z); // 이동

                    }
                    else
                    {
                        check++; //잡은 횟수 +
                        trace_mouse = false; //잠시 추적종료
                        playTime = 0;
                    }
                }
            }
            else // 추적중이지 않을떄 = 마우스를 잡고 잠시 기다림
            {
                playTime++;
                if (playTime > 50) // 기다리는 시간
                {
                    trace_mouse = true; // 다시 추적
                }
            }
        }
        return true;
    }
    public bool Eat()
    {
        return true;
    }
    public bool BasicMove()
    {
        if (isPoop)    //화장실로 이동
        {
            //목표지점을 향하는 벡터 이용해 이동
            transform.position += (toiletPos - transform.position).normalized * Time.deltaTime;

            if ((int)transform.position.x == (int)toiletPos.x
                && (int)transform.position.y == (int)toiletPos.y) //목표 지점 도달한 경우
            {
                GameObject mini_poop = Instantiate(TigerPoopPrefab);
                mini_poop.tag = "tiger_poop";
                mini_poop.transform.position = transform.position;  //현재 위치에 똥 싸기

                statTime = statMax;
                countPoop++;
                isPoop = false;
                fPoop.SetActive(false); //똥 싼 후 말풍선 비활성화   
                nonTimer = 0;
            }

        }
        else
        {
            if (!playing)
            {
                if (moving) // 노는중 아닐 때 , 움직이는 중
                {
                    float x = Start_Point.x + move_vec.x * move_length; // 시작점 + 방향벡터 * 거리
                    float y = Start_Point.y + move_vec.y * move_length;

                    if (x >= 13f) // 울타리를 넘어가지 않기 위해 
                        x = 13f;
                    if (x <= -13f)
                        x = -13f;
                    if (y >= 6.5f)
                        y = 6.5f;
                    if (y <= -6.5f)
                        y = -6.5f;

                    gameObject.transform.position = new Vector3(x, y, Start_Point.z); // 이동

                    move_length += 0.05f; // 거리를 차근차근 움직임
                    if (move_length > 10f) // 다 움직였다면 다시 움직임 타이머를 잼
                    {
                        moving = false;
                        BasicTime = 0;
                    }
                    return true;
                }
                else
                {
                    if (BasicTime % 55 == 0 && BasicTime > 0) // 일정시간마다 혼자 돌아다님
                    {
                        BasicTime = 0;
                        moving = true;
                        Start_Point = gameObject.transform.position; // 시작점 저장
                        move_vec = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)); // 상하좌우,대각 랜덤으로 정함
                        if (move_vec.x >= 0)
                            gameObject.transform.localScale = new Vector3(-1, 1, 1); // 왼쪽으로 움직인다면 왼쪽을 봄
                        else
                            gameObject.transform.localScale = new Vector3(1, 1, 1); // 오른쪽이라면 오른쪽을 봄
                        move_length = 0;
                    }
                    return true;
                }
            }
            else
            {
                BasicTime = 0;
                moving = false;
            }
        }
        
        return true;
    }
    void Start()
    {
        //속성값 초기 설정
        hungry = valueMax;
        poop = valueMax;
        play = valueMax;

        //말풍선 비활성화
        fHungry = transform.GetChild(0).gameObject;
        fHungry.SetActive(false);
        fPoop = transform.GetChild(1).gameObject;
        fPoop.SetActive(false);
        fPlay = transform.GetChild(2).gameObject;
        fPlay.SetActive(false);

        hunger = false; // 변수 초기화
        moving = false;
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        if (nonTimer == 0)
        {
            isNon = true;
            statTime--;
            if (statTime == 0)
            {
                isNon = false;
                statTime = statMax;
            }
        }
        if (!isNon)
            nonTimer--;
        if (!isHungry)
            hungryTimer--;
        if (!isPoop)
            poopTimer--;
        if (!isPlay)
            playTimer--;

        if (timer % 100 == 0)
        {
            hungry--;
            poop--;
            play--;
            //poop -= countPoop * 5; //똥 개수에 비례하여 감소
        }

        //밥 추적 
        Bap = GameObject.FindWithTag("hungry_follow_item");//밥 아이템 찾기 -> 문제: 여러 개 생성되었으면 제일 위에것만 따라감 
        if (Bap != null)//밥 생성 되었는지 
        {
            //Debug.Log("밥 생성");
            distance = Vector3.Distance(this.gameObject.transform.position, Bap.transform.position);//거리 파악
        }
        is_follow_food = (Bap != null && distance < follow_distance);//밥이 생성 되었고 거리가 follow_distance 미만이라면 is_follow_food true
        //
    }
    private void FixedUpdate()
    {
        hungryTime++; // 타이머 
        BasicTime++;
        playTime++;
        
    }
}
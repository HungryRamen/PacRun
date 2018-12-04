using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text;
public enum EPLAYERSTATE //상태
{
    E_RUN,
    E_JUMP,
    E_DEATH,
    E_NOT,
}

public enum EGHOSTPICK //고스트 캐릭터
{
    E_BLINKY,
    E_PINKY,
    E_INKY,
    E_CLYDE,
}

public class PlayerMgr : MonoBehaviour {

    public int iJumpMax = 2;
    public float fPlayerJumpPower = 5f; //점프 속도
    public EGHOSTPICK eGhostPick;                          //선택된 캐릭터
    public EPLAYERSTATE ePlayerState = EPLAYERSTATE.E_RUN; //플레이어 상태
    public int iGhostMAX = 3;
    public GameObject CoolTimeObj;
    public GameObject ChangeEffectObj;
    public GameObject BulletObj;
    public GameObject UIObj;
    public GameObject GameOverObj;
    public GameObject GameEndObj;
    public GameObject[] GameBtnObj;
    public Sprite[] CoolTimeImg;
    private float fRandomChangeTime = 0.0f;
    private float fRecordTime = 0.0f;
    private int iRandomOverlap = 0;
    private int iJumpCount = 0;
    private Rigidbody PlayerRb;
    private Animator PlayerAni;
    private float fAtkCoolTime = 2.0f;
    private float fGravityY = -7.0f;    //중력 고정값
    private float fAccel = 0.0f;        //중력 가속도
    private float fChangeCoolTime = 3.0f;
    public AudioClip[] PlayerJumpFxSound;
    public AudioClip[] PlayerShootFxSound;
    public AudioClip[] PlayerDeadFxSound;
    public AudioClip[] PlayerEndFxSound;
    public AudioClip[] PlayerChangeFxSound;
    private AudioSource fxSource;
    // Use this for initialization
    void Start () {
        fxSource = GetComponent<AudioSource>(); 
        PlayerRb = GetComponent<Rigidbody>();
        PlayerAni = GetComponentInChildren<Animator>();
        PlayerAni.SetInteger("EGHOSTPICK", (int)eGhostPick);
    }

    // Update is called once per frame
    void Update () {
        if (ePlayerState != EPLAYERSTATE.E_DEATH)
        {
            fRecordTime += Time.deltaTime;
            GhostChange();
            MoveInput();
            Gravity();
            ChangeCoolTime();
            GhostAbility();
        }
    }

    private void GhostChange()
    {
        if (fChangeCoolTime >= 3.0f)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                eGhostPick = (EGHOSTPICK)iRandomOverlap;
                PlayerAni.SetInteger("EGHOSTPICK", iRandomOverlap);
                fChangeCoolTime = 0.0f;
                Effect();
                SoundPlay(PlayerChangeFxSound[UnityEngine.Random.Range(0, PlayerChangeFxSound.Length)]);
            }
        }
    }

    private void MoveInput() //플레이어 이동 관리
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ePlayerState == EPLAYERSTATE.E_RUN || ePlayerState == EPLAYERSTATE.E_NOT)
                NormalJump();
            else if (ePlayerState == EPLAYERSTATE.E_JUMP && iJumpCount < iJumpMax && eGhostPick == EGHOSTPICK.E_INKY)
                InkyJump();
        }
    }
    
    private void GhostAbility()
    {
        fAtkCoolTime += Time.deltaTime;
        switch (eGhostPick)
        {
            case EGHOSTPICK.E_BLINKY:
                BlinkyAtkInput();
                TimeSet(1.0f);
                break;
            case EGHOSTPICK.E_PINKY:
                TimeSet(1.5f);
                break;
            case EGHOSTPICK.E_INKY:
                TimeSet(1.0f);
                break;
            case EGHOSTPICK.E_CLYDE:
                TimeSet(0.75f);
                break;
        }
    }


    private void BlinkyAtkInput()
    {
        if (Input.GetMouseButton(0) && fAtkCoolTime >= 1.0f)
        {
            GameObject.Instantiate(BulletObj, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-0.1f),Quaternion.Euler(0f,0f,-90f));
            SoundPlay(PlayerShootFxSound[UnityEngine.Random.Range(0, PlayerShootFxSound.Length)]);
            fAtkCoolTime = 0.0f;
        }
    }
    private void TimeSet(float fTime)
    {
        Time.timeScale = fTime;
    }

    private void InkyJump()
    {
        PlayerRb.velocity = new Vector3(PlayerRb.velocity.x, fPlayerJumpPower, PlayerRb.velocity.z);
        fAccel = 0.0f;
        iJumpCount++;
        SoundPlay(PlayerJumpFxSound[UnityEngine.Random.Range(0, PlayerJumpFxSound.Length)]);
    }

    private void Effect() //캐릭터가 바뀔때
    {
        GameObject TempObj = GameObject.Instantiate(ChangeEffectObj, new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z - 1f),this.transform.rotation);
        Destroy(TempObj, TempObj.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    private void NormalJump()
    {
        PlayerRb.velocity = new Vector3(PlayerRb.velocity.x, fPlayerJumpPower, PlayerRb.velocity.z);
        ePlayerState = EPLAYERSTATE.E_JUMP;
        iJumpCount++;
        SoundPlay(PlayerJumpFxSound[UnityEngine.Random.Range(0, PlayerJumpFxSound.Length)]);
    }


    private void Gravity() //중력 가속 설정
    {
        if (ePlayerState != EPLAYERSTATE.E_RUN)
        {
            Physics.gravity = new Vector3(0, fGravityY + fAccel, 0);
            fAccel -= Time.deltaTime * 10f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Blocks")
        {
            if (ePlayerState != EPLAYERSTATE.E_RUN && this.transform.position.y >= collision.transform.position.y)
            {
                iJumpCount = 0;
                ePlayerState = EPLAYERSTATE.E_RUN;
                fAccel = 0.0f;
                Physics.gravity = new Vector3(0, fGravityY, 0);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Blocks")
        {
            if(ePlayerState == EPLAYERSTATE.E_RUN)
            {
                ePlayerState = EPLAYERSTATE.E_NOT;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "DeadZone" && ePlayerState != EPLAYERSTATE.E_DEATH)
        {
            GameOver();
        }
        else if(collider.tag == "PackMan" && ePlayerState != EPLAYERSTATE.E_DEATH)
        {
            GameEnd();
        }
    }

    void ChangeCoolTime() //캐릭터 바꾸는 쿨타임
    {
        Image img = CoolTimeObj.GetComponent<Image>();
        fRandomChangeTime += Time.deltaTime;
        if(fChangeCoolTime <= 3.0f)
        {
            fChangeCoolTime += Time.deltaTime;
            img.fillAmount = (fChangeCoolTime / 3);
            
        }
        if(fRandomChangeTime >= 0.5f)
        {
            int iRand;
            do
            {
                iRand = UnityEngine.Random.Range(0, CoolTimeImg.Length);
            } while (iRandomOverlap == iRand);
            img.sprite = CoolTimeImg[iRand];
            iRandomOverlap = iRand;
            fRandomChangeTime = 0f;
        }
    }

    private void SoundPlay(AudioClip a)
    {
        fxSource.PlayOneShot(a);
    }

    private void GameOver()
    {
        UIObj.GetComponent<AudioSource>().Pause();
        UIObj.GetComponent<MainUIMgr>().SaveRecord(fRecordTime);
        ePlayerState = EPLAYERSTATE.E_DEATH;
        SoundPlay(PlayerDeadFxSound[UnityEngine.Random.Range(0, PlayerDeadFxSound.Length)]);
        GameEndObj.SetActive(true);
        for (int i =0;i<GameBtnObj.Length;i++)
        {
            GameBtnObj[i].SetActive(true);
        }
    }
    private void GameEnd()
    {
        UIObj.GetComponent<AudioSource>().Pause();
        UIObj.GetComponent<MainUIMgr>().SaveRecord(fRecordTime);
        ePlayerState = EPLAYERSTATE.E_DEATH;
        SoundPlay(PlayerEndFxSound[UnityEngine.Random.Range(0, PlayerEndFxSound.Length)]);
        GameOverObj.SetActive(true);
        for (int i = 0; i < GameBtnObj.Length; i++)
        {
            GameBtnObj[i].SetActive(true);
        }
    }
}

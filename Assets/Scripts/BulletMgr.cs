using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMgr : MonoBehaviour {

    private GameObject SoundObj;
    public GameObject BulletBoomEffectObj;
    public float fSpeed = 15.0f;
    // Use this for initialization
    void Start () {
    }
   
    // Update is called once per frame
    void Update () {
        SoundObj = GameObject.Find("Main Camera");
        this.transform.Translate(0.0f,fSpeed * Time.deltaTime, 0.0f);
        if (this.transform.position.x >= 19.5f)
            Destroy(this.gameObject);
    }
   
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Blocks")
        {
            SoundObj.GetComponent<SoundMgr>().PickShotPlay();
            GameObject TempObj = GameObject.Instantiate(BulletBoomEffectObj, new Vector3(this.transform.position.x+ (Random.Range(0.2f,0.8f)), this.transform.position.y+(Random.Range(-0.2f,0.2f)), this.transform.position.z), Quaternion.Euler(0f, 0f, 0f));
            Destroy(TempObj, TempObj.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
    }
}

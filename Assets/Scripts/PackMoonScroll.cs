using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackMoonScroll : MonoBehaviour {

    public GameObject BlockLoopObj;
    public float fSpeed = -1.0f;
    private float fX = 19.5f;
    private Animator PackMoonAni;
    private int iCount = 0;
    // Use this for initialization
    void Start () {
        PackMoonAni = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(fSpeed * Time.deltaTime, 0, 0);
        if(this.transform.position.x <= -9.5f)
        {
            this.transform.position = new Vector3(fX,this.transform.position.y,this.transform.position.z);
            if (++iCount > 2)
            {
                BlockLoopObj.GetComponent<BlockLoop>().SetGameEnd();
                Destroy(this);
                return;
            }
            PackMoonAni.SetInteger("MoonState", iCount);
        }
    }
}

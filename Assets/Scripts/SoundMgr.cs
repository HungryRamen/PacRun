using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour {

    public AudioClip[] PickShootFxSound;
    private AudioSource fxSource;
    // Use this for initialization
    void Start () {
        fxSource = GetComponent<AudioSource>();
    }
   
    // Update is called once per frame
    void Update () {
    }
    public void PickShotPlay()
    {
        fxSource.PlayOneShot(PickShootFxSound[Random.Range(0, PickShootFxSound.Length)]);
    }
}

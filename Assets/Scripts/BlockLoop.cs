using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SBlock
{
    public GameObject Block;
    public int iArrayNum;
}

public class BlockLoop : MonoBehaviour {

    public bool bGamenEndCheck = false;
    public GameObject[] array_Blocks;
    public GameObject PackManObj;
    public const int MAX = 3;
    SBlock current_Block;
    SBlock[] sBlocks;

    public void SetGameEnd()
    {
        bGamenEndCheck = true;
        GameObject.Instantiate(PackManObj,
             new Vector3(sBlocks[sBlocks.Length - 1].Block.gameObject.transform.position.x + sBlocks[sBlocks.Length - 1].Block.GetComponent<BlockMove>().fBlockLength, 6, 0)
                             , Quaternion.Euler(0f, 0f, 180f));
    }

    // Use this for initialization
    void Start()
    {
        current_Block.Block = GameObject.Instantiate(array_Blocks[0]);
        current_Block.iArrayNum = 0;
        sBlocks = new SBlock[MAX];
        sBlocks[0].iArrayNum = Random.Range(1, array_Blocks.Length);
        sBlocks[0].Block = GameObject.Instantiate(
                        array_Blocks[sBlocks[0].iArrayNum],
                        new Vector3(current_Block.Block.GetComponent<BlockMove>().fBlockLength, 0, 0)
                         , current_Block.Block.transform.rotation);
        for (int i =1; i < MAX; i++)
        {
            sBlocks[i].iArrayNum = Random.Range(1, array_Blocks.Length);
            sBlocks[i].Block = GameObject.Instantiate(
                            array_Blocks[sBlocks[i].iArrayNum],
                            new Vector3(sBlocks[i-1].Block.gameObject.transform.position.x + sBlocks[i - 1].Block.GetComponent<BlockMove>().fBlockLength , 0 ,0)
                             , current_Block.Block.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move_block();
    }

    void Move_block()
    {

        if (current_Block.Block.transform.position.x <= -40)
        {
            // 지나간 블럭은 삭제
            Destroy(current_Block.Block);

            // 현재 블럭을 새로 생성한 블럭으로 참조 변경
            current_Block.Block = sBlocks[0].Block;
            for(int i = 0; i <sBlocks.Length-1; i++)
            {
                sBlocks[i] = sBlocks[i + 1];
            }
            // 다시 새로운 블럭을 생성
            Make_Block();
        }
    }

    void Make_Block()
    {
        if (!bGamenEndCheck)
        {

            bool bCheck = OverlapCheck(Random.Range(1, array_Blocks.Length));
            while (bCheck)
            {
                bCheck = OverlapCheck(Random.Range(1, array_Blocks.Length));
            }
            sBlocks[sBlocks.Length - 1].Block = GameObject.Instantiate(
                            array_Blocks[sBlocks[sBlocks.Length - 1].iArrayNum],
                            new Vector3(sBlocks[sBlocks.Length - 2].Block.gameObject.transform.position.x + sBlocks[sBlocks.Length - 2].Block.GetComponent<BlockMove>().fBlockLength, 0, 0)
                             , current_Block.Block.transform.rotation);
        }
        else
        {
            sBlocks[sBlocks.Length-1].iArrayNum = 0;
            sBlocks[sBlocks.Length - 1].Block = GameObject.Instantiate(
                            array_Blocks[sBlocks[sBlocks.Length - 1].iArrayNum],
                            new Vector3(sBlocks[sBlocks.Length - 2].Block.gameObject.transform.position.x + sBlocks[sBlocks.Length - 2].Block.GetComponent<BlockMove>().fBlockLength, 0, 0)
                             , current_Block.Block.transform.rotation);
        }
    }

    bool OverlapCheck(int iValue)
    {
        if (current_Block.iArrayNum == iValue)
            return true;
        for(int i = 0; i < sBlocks.Length;i++)
        {
            if (sBlocks[i].iArrayNum == iValue)
                return true;
        }
        sBlocks[sBlocks.Length - 1].iArrayNum = iValue;
        return false;
    }
}

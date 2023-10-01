
//using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBuffManage:MonoBehaviour
{
    public PlayerEnum PlayerEnum ; 
    public float playerBuffTime = 1f;
    public GameObject rushPosition;
    public float rushPositionTime = 9f;
    public int playerAttribute;
    public float playerAttributeTime = 3f;
    public float timerBuff =9f;
    public float timerRush = 3f;
    private float timerAttribute;
    private GridCell[] gridCells;
    int[] counts = new int[7]{5,6,4,6,6,6,6};

    public int p1Score;
    public int p2Score;
    private PlayerController playerController;
    private Dictionary<string, float> PlayerBuff = new Dictionary<string, float>()
    {
        {"Speed", 1f},
        {"CoolTime", 0.2f},
        {"ColliderScale", 0.29f},
        {"FlashDistance", 1f},
        {"PushTime", 1f},
        {"CaptureTime", 0.2f},
        {"BounceCount",1f}
    };

    private PlayerData playerData;
    
    private void Start()
    {
        playerData = GetComponent<PlayerController>().PlayerData;
        gridCells = GameObject.Find("Grid").GetComponentsInChildren<GridCell>();
        rushPosition = GameObject.Find("RushPosition");
        playerController = GetComponent<PlayerController>();
    }


    private void Update()
    {

        (p1Score, p2Score) = Score();
        timerBuff += Time.deltaTime;
        float p1Buff = ((p1Score+p2Score)!=0) ? p1Score / (p1Score + p2Score):1;
        float p2Buff = ((p1Score+p2Score)!=0) ? 1 - p1Buff:1;
        float probability = Random.Range(0, 1);
        float buff = playerController.PlayerEnum == PlayerEnum ? p1Buff : p2Buff;

        bool buffProbability = (probability < buff) ? false : true;
        Debug.Log(buffProbability);
        if (timerBuff >=playerBuffTime)
        {
            if (buffProbability)
            {
                var random = new System.Random();
                var key = PlayerBuff.Keys.ElementAt(random.Next(PlayerBuff.Count));
                //  var buff = BuffSelect(key, ref playerData);
                switch (key)
                {
                    case "Speed":
                        if (counts[0] > 0)
                        {
                            playerData.Speed += PlayerBuff[key];
                            counts[0]--;
                        }

                        break;
                    case "CoolTime":
                        if (counts[1] > 0)
                        {
                            playerData.CoolTime -= PlayerBuff[key];
                            counts[1]--;
                        }

                        break;
                    case "ColliderScale":
                        if (counts[2] > 0)
                        {
                            playerData.ColliderScale += PlayerBuff[key];
                            counts[2]--;
                        }

                        break;
                    case "FlashDistance":
                        if (counts[3] > 0)
                        {
                            playerData.FlashDistance += PlayerBuff[key];
                            counts[3]--;
                        }

                        break;
                    case "PushTime":
                        if (counts[4] > 0)
                        {
                            playerData.PushTime += PlayerBuff[key];
                            counts[4]--;
                        }

                        break;
                    case "CaptureTime":
                        if (counts[5] > 0)
                        {
                            playerData.CaptureTime += PlayerBuff[key];
                            counts[5]--;
                        }

                        break;
                    case "BounceCount":
                        if (counts[6] > 0)
                        {
                            playerData.BounceCount += (int)PlayerBuff[key];
                            counts[6]--;
                        }

                        break;
                }
            }

//            Debug.Log(key);
            //addBuff(ref buff, PlayerBuff[key]);
            timerBuff = 0f;
        }

        timerRush += Time.deltaTime;
        if (timerRush >= rushPositionTime)
        {
            int a = Random.Range(0,gridCells.Length);
            BoxCollider2D cellBoxCollider = gridCells[a].GetComponent<BoxCollider2D>();
            Vector2 size = cellBoxCollider.size;
            Vector3 center = new Vector3(size.x/2, size.y/2, 0);
            rushPosition.transform.position = gridCells[a].transform.position + center;
            timerRush = 0;
        }

    }
    public Dictionary<PlayerController, int> playerScore = new Dictionary<PlayerController, int>();
    /// <summary>
    /// 判定游戏是否结束
    /// </summary>
    /// <returns></returns>
    public (int,int) Score()
    {
        playerScore.Clear();
        GridCell[] gridCells = GameObject.Find("Grid").GetComponentsInChildren<GridCell>();
        foreach (var cell in gridCells)
        {
            var pCtrl = cell.GetCurrentPlayerController();
            if (pCtrl != null)
            {
                if (!playerScore.ContainsKey(pCtrl))
                {
                    playerScore.Add(pCtrl, 0);
                }
                playerScore[pCtrl]++;
                //这两个if我查看分数用的，不用管
                if (pCtrl.PlayerEnum == PlayerEnum.P1)
                {
                    p1Score = playerScore[pCtrl];
                }

                if (pCtrl.PlayerEnum == PlayerEnum.P2)
                {
                    p2Score = playerScore[pCtrl];
                }
            }
        }

        return (p1Score, p2Score);

    }
}
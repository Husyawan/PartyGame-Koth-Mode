using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Realtime;

public class CapPointManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> SpawnedGORedTeam;
    public List<GameObject> SpawnedGOBlueTeam;
    public GameObject[] capPoints;
    public float captureDuration = 5f; 
    public float capPointActiveDuration = 30f; 
    public int captureScoreThreshold = 50; 
    public Text captureTimerText; 
    public Text CapPointActiveDuration; 

    private int currentCapIndex = 0;
    public bool isCapturing = false;
    private bool captureInProgress = false;
    [HideInInspector]
    public GameObject activePlayer = null;
    public GameObject LastRedPlayerwhoStartCoroutine = null;
    public string RedCapturePointName_WhoStartCoroutine;

    public GameObject LastBluePlayerwhoStartCoroutine = null;
    public string BlueCapturePointName_WhoStartCoroutine;
    public Coroutine CurrentACoroutine;
    public Coroutine CurrentBCoroutine;
    public Coroutine CurrentCCoroutine;
    public Coroutine CaptureCoroutine;
    public Coroutine ManageActiveCapPointCor;

    public bool isAactive;
    public bool isBactive;
    public bool isCactive;

    public bool isACaptured;
    public bool isBCaptured;
    public bool isCCaptured;

    public bool isAFullyCaptured;
    public bool isBFullyCaptured;
    public bool isCFullyCaptured;

    public Text A_Score_Text_Red;
    public Text B_Score_Text_Red; 
    public Text C_Score_Text_Red;

    public Text A_Score_Text_Blue;
    public Text B_Score_Text_Blue;
    public Text C_Score_Text_Blue;

    public int A_Score_Red;
    public int B_Score_Red;
    public int C_Score_Red;

    public int A_Score_Blue;
    public int B_Score_Blue;
    public int C_Score_Blue;

    public GameObject Score_Red;
    public GameObject Score_Blue;
    public GameObject Timer;

    public Image circularImage;

    public GameObject GameCompleted;
    public Text GameCompletedtxt;

    private PhotonView _photonView;


  
    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }
    public void StartKOTH()
    {
        ActivateNextCapPoint();
          //  _photonView.RPC("ActivateNextCapPoint", RpcTarget.All);
    }
   
    public void defaultvaluetext()
    {
        A_Score_Text_Red.text = "0" +"/"+ captureScoreThreshold;
        B_Score_Text_Red.text = "0" +"/"+ captureScoreThreshold;
        C_Score_Text_Red.text = "0" +"/"+ captureScoreThreshold;
        A_Score_Text_Blue.text = "0" +"/"+ captureScoreThreshold;
        B_Score_Text_Blue.text = "0" +"/"+ captureScoreThreshold;
        C_Score_Text_Blue.text = "0" +"/"+ captureScoreThreshold;
        isAactive = false;
        isBactive = false;
        isCactive = false;
        isAFullyCaptured = false;
        isBFullyCaptured = false;
        isCFullyCaptured = false;
        isACaptured = false;
        isBCaptured = false;
        isCCaptured = false;
        
    }
    private void ActivateNextCapPoint()
    {
        Score_Red.SetActive(true);
        Score_Blue.SetActive(true);
        isCapturing = false;
        captureInProgress = false;
        activePlayer = null;
        circularImage.fillAmount = 0f;
        captureTimerText.gameObject.SetActive(false);
        circularImage.gameObject.SetActive(false);
        for (int i = 0; i < capPoints.Length; i++)
        {
           
            if(currentCapIndex == 0&&!isAFullyCaptured)
            {
                Debug.Log("0 index");
                isAactive = true;
                isBactive = false;
                isCactive = false;
                if(isACaptured)
                {
               
                    if (LastRedPlayerwhoStartCoroutine != null && RedCapturePointName_WhoStartCoroutine != "")
                    {
                        CurrentACoroutine = StartCoroutine(IncrementACounter(LastRedPlayerwhoStartCoroutine, RedCapturePointName_WhoStartCoroutine));
                    }else if (LastBluePlayerwhoStartCoroutine != null && BlueCapturePointName_WhoStartCoroutine != "")
                    {
                        CurrentACoroutine = StartCoroutine(IncrementACounter(LastBluePlayerwhoStartCoroutine, BlueCapturePointName_WhoStartCoroutine));
                    }
                }
            }
            else if(currentCapIndex==1 && !isBFullyCaptured)
            {
                Debug.Log("1 index");
                isAactive = false;
                isBactive = true;
                isCactive = false;
                if (isBCaptured)
                {

                    if (LastRedPlayerwhoStartCoroutine != null && RedCapturePointName_WhoStartCoroutine != "")
                    {
                        CurrentACoroutine = StartCoroutine(IncrementBCounter(LastRedPlayerwhoStartCoroutine, RedCapturePointName_WhoStartCoroutine));
                    }else if (LastBluePlayerwhoStartCoroutine != null && BlueCapturePointName_WhoStartCoroutine != "")
                    {
                        CurrentACoroutine = StartCoroutine(IncrementBCounter(LastBluePlayerwhoStartCoroutine, BlueCapturePointName_WhoStartCoroutine));
                    }
                }
            }
            else if(currentCapIndex==2 && !isCFullyCaptured)
            {
                Debug.Log("2 index");
                isAactive = false;
                isBactive = false;
                isCactive = true;
                if (isCCaptured)
                {

                    if (LastRedPlayerwhoStartCoroutine != null && RedCapturePointName_WhoStartCoroutine != "")
                    {
                        CurrentACoroutine = StartCoroutine(IncrementCCounter(LastRedPlayerwhoStartCoroutine, RedCapturePointName_WhoStartCoroutine));
                    }else if (LastBluePlayerwhoStartCoroutine != null && BlueCapturePointName_WhoStartCoroutine != "")
                    {
                        CurrentACoroutine = StartCoroutine(IncrementCCounter(LastBluePlayerwhoStartCoroutine, BlueCapturePointName_WhoStartCoroutine));
                    }
                }
            }
             
                if(isAFullyCaptured&currentCapIndex==0)
                {
                Debug.Log("0 index 0");
                currentCapIndex++;
                }
                if(isBFullyCaptured&&currentCapIndex==1)
                {
                Debug.Log("1 index 1");
                currentCapIndex++;
                }    
                if(isCFullyCaptured&&currentCapIndex==2)
                {
                Debug.Log("2 index 2");
                currentCapIndex =0;
                }
           
            capPoints[i].SetActive(i == currentCapIndex);
        }
        ManageActiveCapPointCor= StartCoroutine(ManageActiveCapPoint());
    }
    public void StopCapturing(GameObject player, string CapturePointName)
    {
        isCapturing = false;
        captureInProgress = false;
        activePlayer = null;
        circularImage.fillAmount = 0f;
        captureTimerText.gameObject.SetActive(false);
        circularImage.gameObject.SetActive(false);
        StopCoroutine(CaptureCoroutine);
    }
   
    private IEnumerator ManageActiveCapPoint()
    {
        Timer.SetActive(true);
        float timer = capPointActiveDuration;
        while (timer > 0)
        {
            if (!captureInProgress) 
            {
                timer -= Time.deltaTime;
                _photonView.RPC("SyncTimer", RpcTarget.All, timer);
            }
            yield return null;
        }

        currentCapIndex = (currentCapIndex + 1) % capPoints.Length;
        CapPointActiveDuration.text = "00:00";
        Timer.SetActive(false);
        if(CurrentACoroutine!=null)
        {
            StopCoroutine(CurrentACoroutine);
        }
        if (CurrentBCoroutine != null)
        {
            StopCoroutine(CurrentBCoroutine);
        }
        if (CurrentCCoroutine != null)
        {
            StopCoroutine(CurrentCCoroutine);
        }
        ActivateNextCapPoint();
    }
    [PunRPC]
    public void SyncTimer(float timer)
    {
        timer = Mathf.Max(timer, 0f);
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        CapPointActiveDuration.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }
    public void StartCapturing(GameObject player,string CapturePointName)
    {
        if (isCapturing || captureInProgress) return; 

        isCapturing = true;
        captureInProgress = true;
        activePlayer = player;
        CaptureCoroutine= StartCoroutine(CapturePoint(player,CapturePointName));
    }

    private IEnumerator CapturePoint(GameObject player,string CapturePointName)
    {
        float timer = captureDuration;
        float secondCounter = 1f; // Counter for one-second intervals
        while (timer > -1)
        {

            timer -= Time.deltaTime;
            secondCounter -= Time.deltaTime;
                if (photonView.IsMine && player.CompareTag("RedPlayer"))
                {
                captureTimerText.gameObject.SetActive(true);
                circularImage.gameObject.SetActive(true);
                captureTimerText.text = $"{Mathf.Ceil(timer)}s";
                }
                else if (!photonView.IsMine && player.CompareTag("BluePlayer"))
                {
                captureTimerText.gameObject.SetActive(true);
                circularImage.gameObject.SetActive(true);
                captureTimerText.text = $"{Mathf.Ceil(timer)}s";
                }
            if (secondCounter <= 0f)
            {
                if (photonView.IsMine && player.CompareTag("RedPlayer"))
                {
                    circularImage.fillAmount += 0.2f;
                   
                }
                else if (!photonView.IsMine && player.CompareTag("BluePlayer"))
                {
                    circularImage.fillAmount += 0.2f;
                    
                }
                secondCounter = 1;
            }
            yield return null;
            //if (activePlayer != player)
            //{
            //    captureInProgress = false;
            //    isCapturing = false;
            //    activePlayer = null;
            //    captureTimerText.gameObject.SetActive(false);
            //    circularImage.gameObject.SetActive(false);
            //    yield break;
            //}
        }
        if (CapturePointName=="A")
        {
            isACaptured = true;
            if (CurrentACoroutine != null)
            {
                StopCoroutine(CurrentACoroutine);
            }
            CurrentACoroutine = StartCoroutine(IncrementACounter(player,CapturePointName));
            if (player.CompareTag("RedPlayer"))
            {
                LastRedPlayerwhoStartCoroutine = player;
                RedCapturePointName_WhoStartCoroutine = CapturePointName;
            }else if(player.CompareTag("BluePlayer"))
            {
                LastBluePlayerwhoStartCoroutine = player;
                BlueCapturePointName_WhoStartCoroutine = CapturePointName;
            }
            foreach (var item in SpawnedGORedTeam)
            {
                item.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Remove("A");
               
            }
            foreach (var item in SpawnedGOBlueTeam)
            {
                item.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Remove("A");
               
            }
            player.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Add("A");

        }else if(CapturePointName=="B")
        {
            isBCaptured = true;
            if (CurrentBCoroutine != null)
            {
                StopCoroutine(CurrentBCoroutine);
            }
            CurrentBCoroutine = StartCoroutine(IncrementBCounter(player, CapturePointName));
            if (player.CompareTag("RedPlayer"))
            {
                LastRedPlayerwhoStartCoroutine = player;
                RedCapturePointName_WhoStartCoroutine = CapturePointName;
            }
            else if (player.CompareTag("BluePlayer"))
            {
                LastBluePlayerwhoStartCoroutine = player;
                BlueCapturePointName_WhoStartCoroutine = CapturePointName;
            }
            foreach (var item in SpawnedGORedTeam)
            {
                item.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Remove("B");
               
            }
            foreach (var item in SpawnedGOBlueTeam)
            {
                item.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Remove("B");
               
            }
            player.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Add("B");
        }
        else if(CapturePointName=="C")
        {
            isCCaptured = true;
            if (CurrentCCoroutine != null)
            {
                StopCoroutine(CurrentCCoroutine);
            }
            CurrentCCoroutine = StartCoroutine(IncrementCCounter(player, CapturePointName));
            if (player.CompareTag("RedPlayer"))
            {
                LastRedPlayerwhoStartCoroutine = player;
                RedCapturePointName_WhoStartCoroutine = CapturePointName;
            }
            else if (player.CompareTag("BluePlayer"))
            {
                LastBluePlayerwhoStartCoroutine = player;
                BlueCapturePointName_WhoStartCoroutine = CapturePointName;
            }
            foreach (var item in SpawnedGORedTeam)
            {
                item.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Remove("C");
               
            }
            foreach (var item in SpawnedGOBlueTeam)
            {
                item.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Remove("C");
               
            }
            player.gameObject.GetComponentInChildren<PlayerCappedPoint>().cappedpointlist.Add("C");
        }
       
        captureTimerText.gameObject.SetActive(false);
        circularImage.gameObject.SetActive(false);
        circularImage.fillAmount = 0f;
        //CapPointActiveDuration.text = "00:00";
        //StopAllCoroutines();
        //currentCapIndex = (currentCapIndex + 1) % capPoints.Length;
        //ActivateNextCapPoint();
        isCapturing = false;
        captureInProgress = false;
        activePlayer = null;
    }
    IEnumerator IncrementACounter(GameObject player, string CapturePointName)
    {
        
        while (isACaptured &&((A_Score_Blue<captureScoreThreshold)||(A_Score_Red< captureScoreThreshold))&&!isAFullyCaptured)
        {

            if (CapturePointName == "A")
            {
                if (player.CompareTag("RedPlayer"))
                {
                    A_Score_Red += 1;
                    _photonView.RPC("Add_UI_Score_Red", RpcTarget.All, 0, A_Score_Red);

                }
                else if (player.CompareTag("BluePlayer"))
                {
                    A_Score_Blue += 1;
                    _photonView.RPC("Add_UI_Score_Blue", RpcTarget.All, 0, A_Score_Blue);
                }
            }
          
         
            Check_Win_Red();
            Check_Win_Blue();
            if(isAFullyCaptured)
            {

                StopCoroutine(CurrentACoroutine);
                StopCoroutine(ManageActiveCapPointCor);
                currentCapIndex = (currentCapIndex + 1) % capPoints.Length;
                CapPointActiveDuration.text = "00:00";
                Timer.SetActive(false);
                ActivateNextCapPoint();

            }
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator IncrementBCounter(GameObject player, string CapturePointName)
    {
        while (isBCaptured && ((B_Score_Red < captureScoreThreshold) || (B_Score_Blue < captureScoreThreshold)) && !isBFullyCaptured)
        {
           
                if (CapturePointName == "B")
                {
                    if (player.CompareTag("RedPlayer"))
                    {

                        B_Score_Red += 1;
                        _photonView.RPC("Add_UI_Score_Red", RpcTarget.All, 1, B_Score_Red);
                    }
                    else if (player.CompareTag("BluePlayer"))
                    {

                        B_Score_Blue += 1;
                        _photonView.RPC("Add_UI_Score_Blue", RpcTarget.All, 1, B_Score_Blue);

                    }
                }
           
            Check_Win_Red();
            Check_Win_Blue();
            if (isBFullyCaptured)
            {
                StopCoroutine(CurrentBCoroutine);
                StopCoroutine(ManageActiveCapPointCor);
                currentCapIndex = (currentCapIndex + 1) % capPoints.Length;
                CapPointActiveDuration.text = "00:00";
                Timer.SetActive(false);
                ActivateNextCapPoint();
            }

            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator IncrementCCounter(GameObject player, string CapturePointName)
    {
        while (isCCaptured && ((C_Score_Red < captureScoreThreshold) || (C_Score_Blue < captureScoreThreshold)) && !isCFullyCaptured)
        {
                if (CapturePointName == "C")
                {
                    if (player.CompareTag("RedPlayer"))
                    {
                        C_Score_Red += 1;
                        _photonView.RPC("Add_UI_Score_Red", RpcTarget.All, 2, C_Score_Red);
                    }
                    else if (player.CompareTag("BluePlayer"))
                    {
                        C_Score_Blue += 1;
                        _photonView.RPC("Add_UI_Score_Blue", RpcTarget.All, 2, C_Score_Blue);
                    }
                }
            Check_Win_Red();
            Check_Win_Blue();
            if (isCFullyCaptured)
            {
                StopCoroutine(CurrentCCoroutine);
                StopCoroutine(ManageActiveCapPointCor);
                currentCapIndex = (currentCapIndex + 1) % capPoints.Length;
                CapPointActiveDuration.text = "00:00";
                Timer.SetActive(false);
                ActivateNextCapPoint();
            }
            yield return new WaitForSeconds(1f);
        }
    }
    [PunRPC]
            public void Add_UI_Score_Red(int textfieldcount, int score)
            {
                if (textfieldcount == 0)
                {
                    A_Score_Text_Red.text = score + "/" + captureScoreThreshold;
                }
                else if (textfieldcount == 1)
                {
                    B_Score_Text_Red.text = score + "/" + captureScoreThreshold;
                }
                else if (textfieldcount == 2)
                {
                    C_Score_Text_Red.text = score + "/" + captureScoreThreshold;
                }

            }
            [PunRPC]
            public void Add_UI_Score_Blue(int textfieldcount, int score)
            {
                if (textfieldcount == 0)
                {
                    A_Score_Text_Blue.text = score + "/" + captureScoreThreshold;
                }
                else if (textfieldcount == 1)
                {
                    B_Score_Text_Blue.text = score + "/" + captureScoreThreshold;
                }
                else if (textfieldcount == 2)
                {
                    C_Score_Text_Blue.text = score + "/" + captureScoreThreshold;
                }
            }
            private void Check_Win_Red()
            {
               if(A_Score_Red==captureScoreThreshold)
               {
                isAFullyCaptured = true;
               }
               if (B_Score_Red == captureScoreThreshold)
               {
                isBFullyCaptured = true;
               }
               if (C_Score_Red  == captureScoreThreshold)
               {
                  isCFullyCaptured = true;
               }
                if (A_Score_Red == captureScoreThreshold && B_Score_Red == captureScoreThreshold)
                {
                    _photonView.RPC("Red_Winner", RpcTarget.All);

                }
                else if (A_Score_Red == captureScoreThreshold && C_Score_Red == captureScoreThreshold)
                {
                    _photonView.RPC("Red_Winner", RpcTarget.All);
                }
                else if (B_Score_Red == captureScoreThreshold && C_Score_Red == captureScoreThreshold)
                {
                    _photonView.RPC("Red_Winner", RpcTarget.All);
                }
            }
            private void Check_Win_Blue()
            {
        if (A_Score_Blue == captureScoreThreshold)
        {
            isAFullyCaptured = true;
        }
        if (B_Score_Blue == captureScoreThreshold)
        {
            isBFullyCaptured = true;
        }
        if (C_Score_Blue == captureScoreThreshold)
        {
            isCFullyCaptured = true;
        }
        if (A_Score_Blue == captureScoreThreshold && B_Score_Blue == captureScoreThreshold)
                {

                    _photonView.RPC("Blue_Winner", RpcTarget.All);
                }
                else if (A_Score_Blue == captureScoreThreshold && C_Score_Blue == captureScoreThreshold)
                {
                    _photonView.RPC("Blue_Winner", RpcTarget.All);
                }
                else if (B_Score_Blue == captureScoreThreshold && C_Score_Blue == captureScoreThreshold)
                {
                    _photonView.RPC("Blue_Winner", RpcTarget.All);
                }
            }
            [PunRPC]
            public void Red_Winner()
            {
                isCapturing = false;
                captureInProgress = false;
                captureTimerText.gameObject.SetActive(false);
                circularImage.gameObject.SetActive(false);
                StopAllCoroutines();
                activePlayer = null;
                Debug.Log("RED Winner ");
                GameCompletedtxt.text = "Red Winner";
                GameCompleted.SetActive(true);
                Time.timeScale = 0;
            }
            [PunRPC]
            public void Blue_Winner()
            {
                isCapturing = false;
                captureInProgress = false;
                captureTimerText.gameObject.SetActive(false);
                circularImage.gameObject.SetActive(false);
                StopAllCoroutines();
                activePlayer = null;
                Debug.Log("Blue Winner ");
                GameCompletedtxt.text = "Blue Winner";
                GameCompleted.SetActive(true);
                Time.timeScale = 0;
            }
           
   
}

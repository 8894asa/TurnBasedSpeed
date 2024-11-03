using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field instance = null;
    public int turn = -1; //手番
    public bool isPlaying = false; //カード出し待機状態か
    public int playnumber; //出す場の数字
    public int playwhere;　//出す場の番号
    public bool isplayJOKER = false;
    public bool isDrawing = false;
    public bool isSynthesizing = false; //合成待機状態か
    public bool isCompressing = false; //圧縮待機状態か
    public bool isRecombining = false;
    public bool isRDraw = false;
    public bool canback = true; //待機状態から戻れるか、ドローやデッキトップができるか
    public bool canRback = true; //組み替え待機状態から戻れるか
    public bool p1canFill = true; //デッキトップができるか
    public bool p2canFill = true;
    public bool isLastJOKER = false; //デッキの一番下がジョーカーか
    public bool burstposib = false;
    public string waitingname;
    GameObject zero;
    GameObject one;
    public Deck p1Deck;
    public Deck p2Deck;
    public GameObject SCobject;
    public Card f0;
    public Card f1;
    public Card SCcard;
    public string winnername;
    public GameObject retry;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        zero = transform.Find("0").gameObject;
        f0 = zero.GetComponent<Card>();
        one = transform.Find("1").gameObject;
        f1 = one.GetComponent<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        Win();
    }

    //与えられた場のオブジェクトを与えられたカードオブジェクトにする
    public void SetField(Card card, int where)
    {
        if (card.canSC)
        {
            if (where == 0)
            {
                f0.ImageOnOff("Synthesize", false);
                f0.SetCard(card.number, card.mark);
            }
            else
            {
                f1.ImageOnOff("Synthesize", false);
                f1.SetCard(card.number, card.mark);
            }
        }
        //合成されたカードを出す時
        else if(!(card.number == card.largeSCnum && card.number == card.smallSCnum))
        {
            if (where == 0)
            {
                f0.SetImage(card.largeSCnum, card.largeSCmar);
                f0.ImageOnOff("Synthesize", true);
                GameObject syn = f0.transform.Find("Synthesize").gameObject;
                Card img = syn.GetComponent<Card>();
                img.SetImage(card.smallSCnum, card.smallSCmar);
                f0.number = card.number;
                f0.mark = card.mark;
            }
            else
            {
                f1.SetImage(card.largeSCnum, card.largeSCmar);
                f1.ImageOnOff("Synthesize", true);
                GameObject syn = f1.transform.Find("Synthesize").gameObject;
                Card img = syn.GetComponent<Card>();
                img.SetImage(card.smallSCnum, card.smallSCmar);
                f1.number = card.number;
                f1.mark = card.mark;
            }
        }
        //圧縮されたカードを出す時
        else
        {
            if (where == 0)
            {
                f0.SetImage(card.smallSCnum, card.smallSCmar);
                f0.number = card.number;
                f0.mark = card.smallSCmar;
            }
            else
            {
                f1.SetImage(card.smallSCnum, card.smallSCmar);
                f1.number = card.number;
                f1.mark = card.smallSCmar;
            }
        }
    }

    public void TurnEnd()
    {
        isPlaying = false;
        isSynthesizing = false;
        isCompressing = false;
        canback = true;
        canRback = true;
        if (turn % 2 == 1)
        {
            p1canFill = true;
        }
        else
        {
            p2canFill = true;
        }
        burstposib = false;
        f0.LightOff();
        f1.LightOff();
        if (SCobject != null)
        {
            SCcard.LightOff();
            SCobject = null;
            SCcard = null;
        }
        //組み替えドロー
        if (!isRDraw)
        {
            turn++;
        }
        else
        {
            isRDraw = false;
        }
        if (isRecombining)
        {
            if (turn % 2 == 0)
            {
                p1Deck.layoutGroup.SetMargin(80f);
            }
            else
            {
                p2Deck.layoutGroup.SetMargin(80f);
            }
            isRecombining = false;
            isRDraw = true;
        }
        p1Deck.maxhand = 0;
        p2Deck.maxhand = 0;
    }

    public void FillTurnEnd()
    {
        isPlaying = false;
        /*
        isSynthesizing = false;
        isCompressing = false;
        if (isRecombining)
        {
            if (turn % 2 == 1)
            {
                p1Deck.layoutGroup.SetMargin(80f);
            }
            else
            {
                p2Deck.layoutGroup.SetMargin(80f);
            }
        }
        isRecombining = false;
        canback = true;
        canRback = true;
        burstposib = false;
        */
        f0.LightOff();
        f1.LightOff();
        /*
        if (SCobject != null)
        {
            SCcard.LightOff();
            SCobject = null;
            SCcard = null;
        }
        */
        //組み替えドロー
        if (!isRDraw)
        {
            turn++;
        }
        else
        {
            isRDraw = false;
        }
    }
      
    public void GetSC(GameObject SCobj)
    {
        SCobject = SCobj;
        SCcard = SCobject.GetComponent<Card>();
    }

    public void p1Draw_or_Fill()
    {
        if (burstposib)
        {
            Burst();
        }
        if (turn % 2 == 1 && canback)
        {
            if (isPlaying && p1canFill)
            {
                p1Deck.Fill(playwhere);
                //組み替えドローか
                if (!isRDraw)
                {
                    p1canFill = false;
                }
                else
                {
                    isPlaying = false;
                    f0.LightOff();
                    f1.LightOff();
                    isRDraw = false;
                    return;
                }
                if (!isLastJOKER)
                {
                    FillTurnEnd();
                }
                else
                {
                    isLastJOKER = false;
                    canback = false;
                }
            }
            else if(p1Deck.transform.childCount < 6 && p1Deck.GetReserved() > 0 && !isPlaying && !isRDraw)
            {
                p1Deck.Draw();
                TurnEnd();
            }
        }
    }

    public void p2Draw_or_Fill()
    {
        if (burstposib)
        {
            Burst();
        }
        if (turn % 2 == 0 && canback)
        {
            if (isPlaying && p2canFill)
            {
                p2Deck.Fill(playwhere);
                if (!isRDraw)
                {
                    p2canFill = false;
                }
                else
                {
                    isPlaying = false;
                    f0.LightOff();
                    f1.LightOff();
                    isRDraw = false;
                    return;
                }
                if (!isLastJOKER)
                {
                    FillTurnEnd();
                }
                else
                {
                    isLastJOKER = false;
                    canback = false;
                }
            }
            else if (p2Deck.transform.childCount < 6 && p2Deck.GetReserved() > 0 && !isPlaying && !isRDraw)
            {
                p2Deck.Draw();
                TurnEnd();
            }
        }
    }

    public void OneDraw()
    {
        if (IsNoWaiting() || isDrawing)
        {
            canback = false;
            isDrawing = true;
            if (turn % 2 == 1 && p1Deck.transform.childCount < 6)
            {
                p1Deck.OneDraw();
            }
            else if (p2Deck.transform.childCount < 6)
            {
                p2Deck.OneDraw();
            }
        }
    }

    public void Burst()
    {
        if (turn % 2 == 1 && p1Deck.transform.childCount == 0)
        {
            p1Deck.Draw();
        }
        else if (turn % 2 == 0 && p2Deck.transform.childCount == 0)
        {
            p2Deck.Draw();
        }
        burstposib = false;
    }

    public void Synthesize()
    {
        //何も待機状態でない
        if (!isPlaying && !isSynthesizing && !isCompressing && !isRDraw && !isDrawing)
        {
            isSynthesizing = true;
        }
        else if (isSynthesizing == true && canback && SCobject == null)
        {
            isSynthesizing = false;
        }
    }

    public void Compress()
    {
        if (!isPlaying && !isSynthesizing && !isCompressing && !isRDraw && !isDrawing)
        {
            isCompressing = true;
        }
        else if (isCompressing == true && canback && SCobject == null)
        {
            isCompressing = false;
        }
    }

    public void Recombine()
    {
        if (IsNoWaiting())
        {
            isRecombining = true;
            if (turn % 2 == 1)
            {
                p1Deck.layoutGroup.SetMargin(15f);
            }
            else
            {
                p2Deck.layoutGroup.SetMargin(15f);
            }
        }
        else if (isRecombining == true && canRback && SCobject == null)
        {
            isRecombining = false;
            if (turn % 2 == 1)
            {
                p1Deck.layoutGroup.SetMargin(80f);
            }
            else
            {
                p2Deck.layoutGroup.SetMargin(80f);
            }
        }
    }

    void Win()
    {
        if (p1Deck.transform.childCount == 0 && p1Deck.GetReserved() == 0)
        {
            winnername = "player1";
            retry.SetActive(true);
        }
        else if(p2Deck.transform.childCount == 0 && p2Deck.GetReserved() == 0)
        {
            winnername = "player2";
            retry.SetActive(true);
        }
    }

    bool IsNoWaiting()
    {
        if (!isPlaying && !isSynthesizing && !isCompressing && !isRecombining && !isRDraw && !isDrawing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

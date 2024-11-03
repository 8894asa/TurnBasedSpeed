using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum Mark
    {
        SPADE, HEART, DIAMOND, CLUB, JOKER
    }

    public Mark mark;
    public int number;
    public Sprite[] resourses;
    public bool canSC = true; //圧縮、合成可能か
    public int largeSCnum;
    public Mark largeSCmar;
    public int smallSCnum;
    public Mark smallSCmar;
    Sprite sprite;
    Image image;
    Image Back;
    bool isLight = false;
    bool Lighting = false;
    bool isLightOff = false;

    private void Start()
    {
        /*GameObject image = GetComponent<Card>().transform.Find("Light").gameObject;
        Back = image.GetComponent<Image>();*/
    }

    private void Update()
    {
        //カードを光らせる
        if (isLightOff)
        {
            //Back.enabled = false;
            ImageOnOff("Light", false);
            isLight = false;
            Lighting = false;
            isLightOff = false;
        }
        else if (isLight && !Lighting)
        {
            //Back.enabled = true;
            ImageOnOff("Light", true);
            Lighting = true;
        }
    }

    /// <summary>
    /// カードオブジェクトに数字とマークを与え、与えられた数字とマークに対応する画像にする
    /// </summary>
    /// <param name="number"></数字>
    /// <param name="mark"></マーク>
    public void SetCard(int num, Mark mar)
    {
        number = num;
        mark = mar;
        SetImage(num, mar);
    }

    /// <summary>
    /// 与えられた数字とマークに対応する画像にする
    /// </summary>
    /// <param name="num"></param>
    /// <param name="mar"></param>
    public void SetImage(int num, Mark mar)
    {
        sprite = resourses[13 * (int)mar + num - 1];
        image = GetComponent<Image>();
        image.sprite = sprite;
    }

    public void Clicking()
    {
        //カード出し待機状態にする
        if (!Field.instance.isPlaying && !Field.instance.isSynthesizing && !Field.instance.isCompressing && !Field.instance.isRecombining)
        {
            if (transform.gameObject.name == "0")
            {
                Field.instance.playnumber = number;
                Field.instance.playwhere = 0;
                Field.instance.isPlaying = true;
                isLight = true;
                Field.instance.waitingname = "0";
            }
            else if (transform.gameObject.name == "1")
            {
                Field.instance.playnumber = number;
                Field.instance.playwhere = 1;
                Field.instance.isPlaying = true;
                isLight = true;
                Field.instance.waitingname = "1";
            }
        }
        //カードを場に出す
        else if (Field.instance.isPlaying && !Field.instance.isRDraw)
        {
            if (Field.instance.turn % 2 == 1 && transform.parent.name == "p1Hand")
            {
                Play();
            }
            if (Field.instance.turn % 2 == 0 && transform.parent.name == "p2Hand")
            {
                Play();
            }
            if (transform.gameObject.name == Field.instance.waitingname && Field.instance.canback)
            {
                Field.instance.isPlaying = false;
                isLightOff = true;
            }
        }
        //合成
        else if (Field.instance.isSynthesizing && canSC && !Field.instance.isRecombining)
        {
            if (Field.instance.turn % 2 == 1 && transform.parent.name == "p1Hand")
            {
                Synthesize();
            }
            if (Field.instance.turn % 2 == 0 && transform.parent.name == "p2Hand")
            {
                Synthesize();
            }
        }
        //圧縮
        else if (Field.instance.isCompressing && canSC && !Field.instance.isRecombining)
        {
            if (Field.instance.turn % 2 == 1 && transform.parent.name == "p1Hand")
            {
                Compress();
            }
            if (Field.instance.turn % 2 == 0 && transform.parent.name == "p2Hand")
            {
                Compress();
            }
        }
        //組み替え
        else
        {
            if (Field.instance.turn % 2 == 1 && transform.parent.name == "p1Hand")
            {
                Recombine();
            }
            if (Field.instance.turn % 2 == 0 && transform.parent.name == "p2Hand")
            {
                Recombine();
            }
        }
    }

    //カードを場に出す
    void Play()
    {
        //ジョーカーが選択された場合
        if (mark == Mark.JOKER)
        {
            Field.instance.SetField(this, Field.instance.playwhere);
            Field.instance.isplayJOKER = true;
            Destroy(this.gameObject);
        }
        //場にジョーカーがある場合
        else if (Field.instance.isplayJOKER)
        {
            Field.instance.SetField(this, Field.instance.playwhere);
            Field.instance.playnumber = number;
            Field.instance.isplayJOKER = false;
            Destroy(this.gameObject);
        }
        //場との差が1の場合（普通に繋がる場合）
        else if (Field.instance.playnumber - number == 1 || Field.instance.playnumber - number == -1)
        {
            Field.instance.SetField(this, Field.instance.playwhere);
            Field.instance.playnumber = number;
            Destroy(this.gameObject);
        }
        //1と13が繋がる場合
        else if ((Field.instance.playnumber == 1 && number == 13) || (Field.instance.playnumber == 13 && number == 1))
        {
            Field.instance.SetField(this, Field.instance.playwhere);
            Field.instance.playnumber = number;
            Destroy(this.gameObject);
        }
        //山札がない場合
        else if ((Field.instance.turn % 2 == 1 && Field.instance.p1Deck.GetReserved() == 0) || (Field.instance.turn % 2 == 0 && Field.instance.p2Deck.GetReserved() == 0))
        {
            if (Field.instance.canback)
            {
                Field.instance.SetField(this, Field.instance.playwhere);
                Field.instance.playnumber = number;
                Destroy(this.gameObject);
            }
        }
        Field.instance.canback = false;
        Field.instance.burstposib = true;
    }

    //合成
    void Synthesize()
    {
        //ジョーカーかどうか
        if (mark == Card.Mark.JOKER)
        {
            return;
        }

        if (Field.instance.SCobject == null)
        {
            Field.instance.GetSC(this.gameObject);
            isLight = true;
        }
        //自己合成防止
        else if (number != Field.instance.SCcard.number || mark != Field.instance.SCcard.mark)
        {
            if (number > Field.instance.SCcard.number)
            {
                largeSCnum = number;
                largeSCmar = mark;
                smallSCnum = Field.instance.SCcard.number;
                smallSCmar = Field.instance.SCcard.mark;
                SetImage(number, mark);
                ImageOnOff("Synthesize", true);
                GameObject syn = transform.Find("Synthesize").gameObject;
                Card img = syn.GetComponent<Card>();
                img.SetImage(Field.instance.SCcard.number, Field.instance.SCcard.mark);
                canSC = false;
            }
            else
            {
                largeSCnum = Field.instance.SCcard.number;
                largeSCmar = Field.instance.SCcard.mark;
                smallSCnum = number;
                smallSCmar = mark;
                SetImage(Field.instance.SCcard.number, Field.instance.SCcard.mark);
                ImageOnOff("Synthesize", true);
                GameObject syn = transform.Find("Synthesize").gameObject;
                Card img = syn.GetComponent<Card>();
                img.SetImage(number, mark);
                canSC = false;
            }
            number = (number + Field.instance.SCcard.number) % 13 != 0 ? (number + Field.instance.SCcard.number) % 13 : 13;
            Destroy(Field.instance.SCobject);
            Field.instance.canback = false;
        }
        else
        {
            LightOff();
            Field.instance.SCobject = null;
            Field.instance.SCcard = null;
        }
    }

    //圧縮
    void Compress()
    {
        if (mark == Card.Mark.JOKER)
        {
            return;
        }

        if (Field.instance.SCobject == null)
        {
            Field.instance.GetSC(this.gameObject);
            isLight = true;
        }
        else if (number == Field.instance.SCcard.number && mark != Field.instance.SCcard.mark)
        {
            largeSCnum = number;
            largeSCmar = mark;
            smallSCnum = Field.instance.SCcard.number;
            smallSCmar = Field.instance.SCcard.mark;
            SetImage(number, mark);
            ImageOnOff("Compress", true);
            GameObject syn = transform.Find("Compress").gameObject;
            Card img = syn.GetComponent<Card>();
            img.SetImage(Field.instance.SCcard.number, Field.instance.SCcard.mark);
            canSC = false;
            Destroy(Field.instance.SCobject);
            Field.instance.canback = false;
        }
        else if (number == Field.instance.SCcard.number && mark == Field.instance.SCcard.mark)
        {
            LightOff();
            Field.instance.SCobject = null;
            Field.instance.SCcard = null;
        }
    }

    //組み替え
    void Recombine()
    {
        if (canSC)
        {
            if (Field.instance.isSynthesizing)
            {
                Synthesize();
            }
            else if (Field.instance.isCompressing)
            {
                Compress();
            }
            Field.instance.canback = true;
        }
        else
        {
            GameObject prefab = (GameObject)Resources.Load("Card");
            GameObject card = Instantiate(prefab);
            card.transform.SetParent(transform.parent, false);
            Card cardscr = card.GetComponent<Card>();
            cardscr.SetCard(largeSCnum, largeSCmar);

            card = Instantiate(prefab);
            card.transform.SetParent(transform.parent, false);
            cardscr = card.GetComponent<Card>();
            cardscr.SetCard(smallSCnum, smallSCmar);

            Destroy(this.gameObject);
        }
        Field.instance.canRback = false;
    }

    public void LightOn()
    {
        isLight = true;
    }

    public void LightOff()
    {
        isLightOff = true;
    }

    public void ImageOnOff(string name, bool ismakeOn)
    {
        GameObject image = GetComponent<Card>().transform.Find(name).gameObject;
        image.GetComponent<Image>().enabled = ismakeOn;
    }
}

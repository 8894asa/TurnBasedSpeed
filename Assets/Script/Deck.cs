using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [Header("黒色のカードを使うか")] public bool isblack = true;

    int count = 0; //デッキカーソル
    Card[] deck; //デッキ
    int handcount = 0;
    public int maxhand = 0;
    public CustomHorizontalLayoutGroup layoutGroup;
    

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        deck = new Card[27];
        Number_arrange();
        Mark_arrange();
        Fill(isblack ? 1 : 0);
        Draw();
        Field.instance.turn = 1;
        Field.instance.p1canFill = true;
        Field.instance.p2canFill = true;
    }

    // Update is called once per frame
    void Update()
    {
        var targetList = new List<RectTransform>();
        for (var i = 0; i < layoutGroup.transform.childCount; i++)
        {
            var child = layoutGroup.transform.GetChild(i);
            targetList.Add(child.transform as RectTransform);
        }

        layoutGroup.SetLayoutTarget(targetList);
        layoutGroup.SetLayoutTarget(layoutGroup.TargetList).Align();
    }

    //山札の数字をシャッフル
    void Number_arrange()
    {
        int i, j;
        int[] remain = new int[27];

        for (i = 0; i < 27; i++)
        {
            remain[i] = i + 1;
        }

        for (i = 0; i < 27; i++)
        {
            if (i > 0)
            {
                for (j = 0; j < 27; j++)
                {
                    if (deck[i - 1].number == remain[j])
                    {
                        break;
                    }
                }
                for (; j < 27; j++)
                {
                    if (j == 26)
                    {
                        remain[26] = 0;
                    }
                    else
                    {
                        remain[j] = remain[j + 1];
                    }
                }
            }
            if (i == 0)
            {
                deck[i] = new Card();
                deck[i].number = remain[(int)Random.Range(0.0f,4095.0f) % 26];
            }
            else
            {
                deck[i] = new Card();
                deck[i].number = remain[i != 26 ? (int)Random.Range(0.0f, 4095.0f) % (27 - i) : 0];
            }
        }
    }

    //山札の数字にマークを割り当て
    void Mark_arrange()
    {
        if (isblack)
        {
            for (int i = 0; i < 27; i++)
            {
                switch (deck[i].number % 13 != 0 ? deck[i].number / 13 : deck[i].number / 13 - 1)
                {
                    case 0:
                        deck[i].mark = Card.Mark.SPADE;
                        break;

                    case 1:
                        deck[i].mark = Card.Mark.CLUB;
                        deck[i].number = deck[i].number - 13;
                        break;

                    default:
                        deck[i].mark = Card.Mark.JOKER;
                        deck[i].number = 1;
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 27; i++)
            {
                switch (deck[i].number % 13 != 0 ? deck[i].number / 13 : deck[i].number / 13 - 1)
                {
                    case 0:
                        deck[i].mark = Card.Mark.HEART;
                        break;

                    case 1:
                        deck[i].mark = Card.Mark.DIAMOND;
                        deck[i].number = deck[i].number - 13;
                        break;

                    default:
                        deck[i].mark = Card.Mark.JOKER;
                        deck[i].number = 2;
                        break;
                }
            }
        }
    }

    //残り枚数の計算
    public int GetReserved()
    {
        return 27 - count;
    }

    //ドロー
    public void Draw()
    {
        if (transform.childCount < 3)
        {
            while (transform.childCount < 4 && GetReserved() > 0)
            {
                GameObject prefab = (GameObject)Resources.Load("Card");
                GameObject card = Instantiate(prefab);
                card.transform.SetParent(transform, false);
                Card cardscr = card.GetComponent<Card>();
                cardscr.SetCard(deck[count].number, deck[count].mark);
                count++;
            }
        }
        else
        {
            GameObject prefab = (GameObject)Resources.Load("Card");
            GameObject card = Instantiate(prefab);
            card.transform.SetParent(transform, false);
            Card cardscr = card.GetComponent<Card>();
            cardscr.SetCard(deck[count].number, deck[count].mark);
            count++;
        }
    }

    public void OneDraw()
    {
        if (maxhand == 0)
        {
            maxhand = transform.childCount < 4 ? 4 : transform.childCount + 1;
        }
        if (transform.childCount < maxhand)
        {
            GameObject prefab = (GameObject)Resources.Load("Card");
            GameObject card = Instantiate(prefab);
            card.transform.SetParent(transform, false);
            Card cardscr = card.GetComponent<Card>();
            cardscr.SetCard(deck[count].number, deck[count].mark);
            count++;
        }
    }

    //デッキトップ
    public void Fill(int where)
    {
        if (deck[count].mark == Card.Mark.JOKER && count < 26)
        {
            //Field.instance.SetField(deck[count], where);
            count++;
        }
        Field.instance.SetField(deck[count], where);
        count++;
        if (deck[count - 1].mark == Card.Mark.JOKER && count == 27)
        {
            Field.instance.isLastJOKER = true;
            Field.instance.isplayJOKER = true;
        }
    }

}

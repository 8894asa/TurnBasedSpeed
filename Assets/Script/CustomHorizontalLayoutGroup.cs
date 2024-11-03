using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHorizontalLayoutGroup : MonoBehaviour
{
    /// <summary>
    /// レイアウト対象
    /// </summary>
    public List<RectTransform> TargetList => _targetList;

    /// <summary>
    /// このオブジェクトのレクトトランスフォーム
    /// </summary>
    private RectTransform _rectTransform;
    private RectTransform RectTransform => _rectTransform != null ? _rectTransform : _rectTransform = transform as RectTransform;

    [SerializeField, Header("レイアウト対象")] private List<RectTransform> _targetList = new List<RectTransform>();

    [SerializeField, Header("マージン")] private float _margin;

    [SerializeField, Header("逆方向から並べるか")] private bool _reverse;

    /// <summary>
    /// 整列させる対象を設定
    /// </summary>
    public CustomHorizontalLayoutGroup SetLayoutTarget(List<RectTransform> targetList)
    {
        _targetList = targetList;
        return this;
    }

    /// <summary>
    /// マージンを設定
    /// </summary>
    public CustomHorizontalLayoutGroup SetMargin(float margin)
    {
        _margin = margin;
        return this;
    }

    /// <summary>
    /// 整列させる
    /// </summary>
    public void Align()
    {
        AlignInternal(_targetList);
    }

    /// <summary>
    /// 整列させる処理の本体
    /// </summary>
    /// <param name="targetList"></param>
    private void AlignInternal(List<RectTransform> targetList)
    {
        var currentX = 0f;
        var totalWidth = 0f;
        for (var index = 0; index < targetList.Count; index++)
        {
            var rectTransform = targetList[index];
            if (!rectTransform.gameObject.activeSelf)
            {
                continue;
            }

            // 整列させる
            rectTransform.anchorMax = _reverse
                ? new Vector2(1f, rectTransform.anchorMax.y)
                : new Vector2(0f, rectTransform.anchorMax.y);
            rectTransform.anchorMin = _reverse
                ? new Vector2(1f, rectTransform.anchorMin.y)
                : new Vector2(0f, rectTransform.anchorMin.y);
            rectTransform.pivot = _reverse
                ? new Vector2(1f, rectTransform.pivot.y)
                : new Vector2(0f, rectTransform.pivot.y);
            rectTransform.anchoredPosition = new Vector2(currentX, rectTransform.anchoredPosition.y);

            var width = rectTransform.rect.width;
            currentX = _reverse ? currentX - width - _margin : currentX + width + _margin;

            // 末尾の場合は幅の合計にマージンを含めない
            if (index == targetList.Count - 1)
            {
                totalWidth += width;
            }
            else
            {
                totalWidth += width + _margin;
            }
        }

        // 中央揃えなどの並べ方を簡単にするためにアタッチされているオブジェクトのサイズを変える
        SetWidth(RectTransform, totalWidth);
    }

    /// <summary>
    /// サイズをセットする
    /// </summary>
    private void SetSize(RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax +
                          new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    /// <summary>
    /// 幅をセットする
    /// </summary>
    private void SetWidth(RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }

}

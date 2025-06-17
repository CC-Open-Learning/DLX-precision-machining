using NodeCanvas.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class for a more dynamic grid layout in Unity's UI.
/// </summary>
public class DynamicGridLayout : LayoutGroup
{
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    private bool hasFirstUpdateRun = false;

    protected override void Start()
    {
        Canvas.ForceUpdateCanvases();
    }

    public override void CalculateLayoutInputHorizontal()
    {
        if (!hasFirstUpdateRun)
        {
            hasFirstUpdateRun = true;
            Canvas.ForceUpdateCanvases();
        }

        base.CalculateLayoutInputHorizontal();

        float sqrRt = Mathf.Sqrt(transform.childCount);
        rows = Mathf.CeilToInt(sqrRt);
        columns = Mathf.CeilToInt(sqrRt);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float spacingX = (spacing.x / (float)columns) * 2;
        float spacingY = (spacing.y / (float)rows) * 2;
        float paddingLeft = padding.left / (float)columns;
        float paddingRight = padding.right / (float)columns;
        float paddingTop = padding.top / (float)rows;
        float paddingBottom = padding.bottom / (float)rows;

        float cellWidth = (parentWidth / (float)columns) - spacingX - paddingLeft - paddingRight;
        float cellHeight = (parentHeight / (float)rows) - spacingY - paddingTop - paddingBottom;

        cellSize.x = cellWidth;
        cellSize.y = cellHeight;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}

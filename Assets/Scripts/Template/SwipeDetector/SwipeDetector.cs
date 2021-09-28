using System;
using Template;
using UnityEngine;

public class SwipeDetector : Singleton<SwipeDetector>
{
    [SerializeField] private bool detectSwipeOnlyAfterRelease = false;
    [SerializeField] private float minDistanceForSwipe = 20f;

    public event Action<SwipeData> OnSwipe = delegate { };

    private Vector2 startPosition;
    private Vector2 endPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            endPosition = Input.mousePosition;
        }

        if (!detectSwipeOnlyAfterRelease && Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;

            if (Vector2.Distance(startPosition, endPosition) > 0)
            {
                DetectSwipe();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPosition = Input.mousePosition;
            DetectSwipe();
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = endPosition.y - startPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = endPosition.x - startPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
            startPosition = endPosition;
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(endPosition.y - startPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(endPosition.x - startPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = endPosition,
            EndPosition = startPosition
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}
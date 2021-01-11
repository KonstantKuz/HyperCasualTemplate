using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private bool invertVertical;
    public static TapPhase tapPhase { get; private set; }
    public static Swipe swipe { get; private set; }
    public static Vector2 InputDeltaNormalized { get; private set; }

    private static Vector2 _lastTapPos = Vector2.zero;
    private static Vector2 _startSwipePos = Vector2.zero;
    #region MonoBehaviour

    // Start is called before the first frame update
    void Start()
    {
        InputDeltaNormalized = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    #endregion

#region Private
    private void HandleInput()
    {
        InputDeltaNormalized = Vector2.zero;
        tapPhase = TapPhase.NONE;
        swipe = Swipe.NONE;
        Vector2 deltaMove = Vector2.zero;
        Vector2 swipeDeltaMove = Vector2.zero;
#if UNITY_STANDALONE || UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            tapPhase = TapPhase.BEGAN;
            _startSwipePos = Input.mousePosition;
            _lastTapPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            deltaMove = (Vector2)Input.mousePosition - _lastTapPos;
            swipeDeltaMove = (Vector2)Input.mousePosition - _startSwipePos;

            if (deltaMove == Vector2.zero)
            {
                tapPhase = TapPhase.STATIONARY;
            }
            else
            {
                tapPhase = TapPhase.MOVED;
                if (invertVertical)
                {
                    deltaMove.y = -deltaMove.y;
                }
                InputDeltaNormalized = deltaMove.normalized;
                if ((Mathf.Abs(deltaMove.x) > 50f || Mathf.Abs(deltaMove.y) > 50f))
                {
                    //Debug.Log("Swiped!!!! " + deltaMove);
                    if (InputDeltaNormalized != Vector2.zero)
                    {
                        swipe = GetSwipe(deltaMove);
                    }
                }
            }
            
            InputDeltaNormalized = deltaMove.normalized;
            _lastTapPos = Input.mousePosition;
        }
        
        
        if (Input.GetMouseButtonUp(0))
        {
            tapPhase = TapPhase.ENDED;
            _startSwipePos = Vector2.zero;
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (IsTapped())
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    tapPhase = TapPhase.BEGAN;
                    _startSwipePos = Input.mousePosition;
                    _lastTapPos = Input.mousePosition;
                    break;
                case TouchPhase.Canceled:
                    tapPhase = TapPhase.CANCELED;
                    _startSwipePos = Vector2.zero;
                    break;
                case TouchPhase.Ended:
                    tapPhase = TapPhase.ENDED;
                    _startSwipePos = Vector2.zero;
                    break;
                case TouchPhase.Moved:
                    tapPhase = TapPhase.MOVED;
                    deltaMove = touch.deltaPosition;
                    if (invertVertical)
                    {
                        deltaMove.y = -deltaMove.y;
                    }
                    MoveVector = deltaMove.normalized;
                    if (MoveVector != Vector2.zero)
                    {
                        swipe = GetSwipe(deltaMove);
                    }
                    if ((Mathf.Abs(deltaMove.x) > 50f || Mathf.Abs(deltaMove.y) > 50f))
                    {
                        //Debug.Log("Swiped!!!! " + deltaMove);
                        if (MoveVector != Vector2.zero)
                        {
                            swipe = GetSwipe(deltaMove);
                        }
                    }
                    break;
                case TouchPhase.Stationary:
                    tapPhase = TapPhase.STATIONARY;
                    break;
            }
        }
#endif
    }
    #endregion

    private Swipe GetSwipe(Vector2 move)
    {
        if(Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            if(move.x > 0)
            {
                return Swipe.RIGHT;
            }
            else if(move.x < 0)
            {
                return Swipe.LEFT;
            }
            return Swipe.NONE;
        }
        else
        {
            if (move.y > 0)
            {
                return Swipe.UP;
            }
            else if (move.y < 0)
            {
                return Swipe.DOWN;
            }
            return Swipe.NONE;
        }
        
    }

    public static bool IsTapped()
    {
        bool tap = false;
#if UNITY_STANDALONE
        tap = Input.GetMouseButton(0);
#elif UNITY_ANDROID || UNITY_IOS
        tap = Input.touchCount > 0;
#endif
        return tap;
    }
    
    public override string ToString()
    {
        string info = string.Format("Phase: {0}, Current position - {2}, Direction - {1}", tapPhase.ToString(), InputDeltaNormalized, _lastTapPos);

        return base.ToString();
        
    }

}

public enum TapPhase
{
    NONE,
    BEGAN,
    MOVED,
    STATIONARY,
    ENDED,
    CANCELED,
}

public enum Swipe
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private bool invertVertical;
    public static TapPhase tapPhase { get; private set; }
    public static Vector2 MoveVector { get; private set; }

    private static Vector2 _lastTapPos = Vector2.zero;

#region MonoBehaviour

    // Start is called before the first frame update
    void Start()
    {
        MoveVector = Vector2.zero;
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
        MoveVector = Vector2.zero;
        tapPhase = TapPhase.NONE;
        Vector2 deltaMove = Vector2.zero;
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            deltaMove = (Vector2)Input.mousePosition - _lastTapPos;

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
                MoveVector = deltaMove.normalized;
            }
            MoveVector = deltaMove.normalized;
            _lastTapPos = Input.mousePosition;
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            tapPhase = TapPhase.BEGAN;
        }
        if (Input.GetMouseButtonUp(0))
        {
            tapPhase = TapPhase.ENDED;
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (IsTapped())
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    tapPhase = TapPhase.BEGAN;
                    break;
                case TouchPhase.Canceled:
                    tapPhase = TapPhase.CANCELED;
                    break;
                case TouchPhase.Ended:
                    tapPhase = TapPhase.ENDED;
                    break;
                case TouchPhase.Moved:
                    tapPhase = TapPhase.MOVED;
                    deltaMove = touch.deltaPosition;
                    if (invertVertical)
                    {
                        deltaMove.y = -deltaMove.y;
                    }
                    MoveVector = deltaMove.normalized;
                    break;
                case TouchPhase.Stationary:
                    tapPhase = TapPhase.STATIONARY;
                    break;
            }
        }
#endif
    }
    #endregion

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
        string info = string.Format("Phase: {0}, Current position - {2}, Direction - {1}", tapPhase.ToString(), MoveVector, _lastTapPos);

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

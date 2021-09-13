using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private bool invertVertical;
    public Vector2 InputDelta { get; private set; }
    public Vector2 InputDeltaNormalized { get; private set; }

    private Vector2 lastMousePosition = Vector2.zero;

    private void Start()
    {
        InputDelta = Vector2.zero;
        InputDeltaNormalized = Vector2.zero;
    }

    private void Update()
    {
        HandleInput();
    }
    
    private void HandleInput()
    {
        InputDelta = Vector2.zero;
        InputDeltaNormalized = Vector2.zero;
        Vector2 deltaMove = Vector2.zero;
        
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            deltaMove = (Vector2)Input.mousePosition - lastMousePosition;

            if (invertVertical)
            {
                deltaMove.y = -deltaMove.y;
            }

            InputDelta = deltaMove;
            InputDeltaNormalized = deltaMove.normalized;
            
            lastMousePosition = Input.mousePosition;
        }
    }
}
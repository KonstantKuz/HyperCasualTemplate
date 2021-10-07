using UnityEngine;

namespace Templates.UI
{
    public class StartOnClick : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Observer.Instance.OnStartGame();
                gameObject.SetActive(false);
            }
        }
    }
}

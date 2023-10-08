using UnityEngine;

public abstract class MenuButton : MonoBehaviour
{
    protected GameStateBase _state;

    private Camera _camera;
    private Collider2D _collider;

    private void Start()
    {
        _camera = Camera.main;
        _collider = GetComponent<BoxCollider2D>();
        SetState();
    }

    protected virtual void SetState() { }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 origin = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D[] results = Physics2D.CircleCastAll(origin, 1f, Vector2.zero);
            foreach (RaycastHit2D result in results)
            { 
                if (result.collider == _collider)
                {
                    EventBus.Invoke(new GameStateChangedEvent(_state));
                    break;
                }
            }
        }
    }
}


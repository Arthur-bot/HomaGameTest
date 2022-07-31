using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerInput : MonoBehaviour
{
    #region Fields

    [SerializeField] private EventTrigger _trigger;

    private UIMainMenu _menu;
    private Vector2 _screenPosition;

    #endregion

    #region Properties

    public PlayerCharacter Player { get; set; }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        var triggerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        triggerDown.callback.AddListener(OnPointerDown);
        _trigger.triggers.Add(triggerDown);
        
        var triggerDrag = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
        triggerDrag.callback.AddListener(OnDrag);
        _trigger.triggers.Add(triggerDrag);
        
        var triggerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        triggerUp.callback.AddListener(OnPointerUp);
        _trigger.triggers.Add(triggerUp);

        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return null;
            
            _menu = GameUI.Instance.MainMenu;
        }
    }

    #endregion

    #region Private Methods
    
    private void OnPointerDown(BaseEventData data)
    {
        if (Player == null) return;

        var pointerEventData = (PointerEventData)data;
        Player.MoveDirection = pointerEventData.position;
        
        if (_menu.isActiveAndEnabled)
        {
            _menu.Show(false);
        }
    }

    private void OnDrag(BaseEventData data)
    {
        if (Player == null) return;
        
        var pointerEventData = (PointerEventData)data;
        Player.MoveDirection = pointerEventData.position;
    }

    private void OnPointerUp(BaseEventData data)
    {
        if (Player == null) return;
        
        Player.MoveDirection = Vector2.zero;
    }

    #endregion
}
using UnityEngine.EventSystems;

public interface ISelectable : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    void Select();
    void Deselect();
}

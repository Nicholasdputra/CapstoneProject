public interface IClickable
{
    int Health { get; set; }
    void OnHover();
    void OnUnhover();
    void OnClick();
    void OnDestroy();
}
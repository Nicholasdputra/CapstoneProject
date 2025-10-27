public interface IClickable
{
    bool isClickable{ get; set; }
    int CurrentHealth { get; set; }
    int MaxHealth { get; set; }
    void OnHover();
    void OnUnhover();
    void OnClick();
    void HandleDestroy();
}
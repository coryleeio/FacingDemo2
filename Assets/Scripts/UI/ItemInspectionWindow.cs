namespace Gamepackage
{
    public class ItemInspectionWindow : UIComponent
    {
        public override void Hide()
        {
            GetComponent<ItemInspectionWindow>().gameObject.SetActive(false);
        }

        public void ShowFor(Item item)
        {
            Show();
        }

        public override void Refresh()
        {

        }

        public override void Show()
        {
            GetComponent<ItemInspectionWindow>().gameObject.SetActive(true);
            Context.UIController.PushWindow(this);
        }
    }
}

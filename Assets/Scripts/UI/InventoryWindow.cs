namespace Gamepackage
{
    public class InventoryWindow : UIComponent
    {
        private bool active = false;

        public override void Hide()
        {
            active = false;
            GetComponent<InventoryWindow>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            active = true;
            GetComponent<InventoryWindow>().gameObject.SetActive(true);
        }

        public void Toggle()
        {
            if (active)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}

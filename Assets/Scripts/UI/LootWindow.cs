namespace Gamepackage
{
    public class LootWindow : UIComponent
    {
        private bool active = false;

        public override void Hide()
        {
            active = false;
            GetComponent<LootWindow>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            active = true;
            GetComponent<LootWindow>().gameObject.SetActive(true);
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

namespace MyFinance.WebBlazorUI.Shared
{
	public partial class NavMenu
	{
		private bool collapseNavMenu = true;
		private bool expandSubMenu;
		private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;
		private void ToggleNavMenu()
		{
			collapseNavMenu = !collapseNavMenu;
		}
	}
}
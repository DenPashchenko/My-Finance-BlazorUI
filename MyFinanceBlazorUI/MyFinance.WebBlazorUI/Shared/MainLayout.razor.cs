using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MyFinance.WebBlazorUI.Shared
{
	public partial class MainLayout
	{
		private ErrorBoundary? errorBoundary;
		protected override void OnParametersSet()
		{
			errorBoundary?.Recover();
		}

		private void RedirectToHome()
		{
			NavigationManager.NavigateTo("/");
		}
	}
}
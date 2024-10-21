using Cysharp.Threading.Tasks;
using UnityEngine;

public class UISplash : UIServiceComponent
{
	[SerializeField]
	private UIScreenController m_uiController;

	/*protected override void Initialize()
	{
		
	}*/

	public void OpenLevelUpScreen()
	{
		m_uiController.Show<UIBlockScreen>();

		OpenLevelUpScreenProcess().Forget();
	}

	private async UniTaskVoid OpenLevelUpScreenProcess()
	{
		await UniTask.WaitForSeconds(2.0f, cancellationToken: destroyCancellationToken);

		m_uiController.Show<UILevelUpScreen>();
	}
}
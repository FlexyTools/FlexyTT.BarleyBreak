using Flexy.Core.Extensions;
using UnityEngine.Profiling;

namespace FlexyTT.BarleyBreak.Menu.Settings
{
	public class Window_GameSettings : UIWindowEx, ISubStates
	{
		[SerializeField]	RectTransform	_tabsContainer = null!;
	
		[Callable]			void	OpenAudioTab	( ) => Game.UI.Settings_Audio.Open();
		[Callable]			void	OpenColorTab	( ) => Game.UI.Settings_Color.Open();

		[Bindable]			Boolean	IsOpened_TabAudio => Game.UI.Settings_Audio.Ctx.StateRef == Node.FirstBaseChild?.State.PrefabRef;
		[Bindable]			Boolean	IsOpened_TabColor => Game.UI.Settings_Color.Ctx.StateRef == Node.FirstBaseChild?.State.PrefabRef;

		protected override	UniTask	OnShow			( )		
		{
			if (Node.BaseLayer?.FirstNode == null)
				Game.UI.Settings_Audio.Open();
			
			return base.OnShow();
		}
		protected override	void	OnOpen			( FlowNode node )								
		{
			
		}
		protected override	void	OnChildOpen		( FlowNode node, FlowNode child, String layer )	
		{
			if (node.BaseLayer?.FirstNode != child)
				node.BaseLayer?.FirstNode?.Close();
				
			RebindAll();
		}

		StateTransition						ISubStates.TransitionStore	{ get; } = new ();
		Dictionary<AssetRef<State>, State>	ISubStates.SubStatesCache	{ get; } = new (4);
		
		String[]	ISubStates.GetSupportedLayers	( )								=> new []{StateLayers.Base};
		State		ISubStates.InstantiateSubState	( State prefab, String layer )	=> Instantiate(prefab, _tabsContainer);
		void		ISubStates.DestroySubState		( State state )					=> state.Destroy();
	}
}
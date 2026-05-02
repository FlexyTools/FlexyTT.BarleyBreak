using Flexy.Core.Extensions;
using UnityEngine.Profiling;

namespace FlexyTT.BarleyBreak.Menu.Settings
{
	public class Window_GameSettings : UIWindowEx, ISubStates
	{
		[SerializeField]	RectTransform	_tabsContainer = null!;
	
		[Callable]			void	OpenAudioTab	( ) => Game.UI.Settings_Audio.Open();
		[Callable]			void	OpenInputTab	( ) => Game.UI.Settings_Input.Open();

		private		void	Update		( )		
		{
			
			Debug.Log($"Hello from update in GameSettings enabled: {enabled} " +
			          $"active: {gameObject.activeInHierarchy} " +
			          $"at {Time.realtimeSinceStartup:F3} {Time.frameCount:000}");
			
		}
		private		void	FixedUpdate	( )		
		{
			Profiler.BeginSample("Window_GameSettings.FixedUpdate");
			Debug.Log($"Hello from update i am {"name"} adn i am is {enabled} and {gameObject.activeInHierarchy} at {Time.renderedFrameCount:000} {Time.realtimeSinceStartupAsDouble:F3} {Time.frameCount:000}");
			Profiler.EndSample();
		}  

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
		}

		StateTransition						ISubStates.TransitionStore	{ get; } = new ();
		Dictionary<AssetRef<State>, State>	ISubStates.SubStatesCache	{ get; } = new (4);
		
		String[]	ISubStates.GetSupportedLayers	( )								=> new []{StateLayers.Base};
		State		ISubStates.InstantiateSubState	( State prefab, String layer )	=> Instantiate(prefab, _tabsContainer);
		void		ISubStates.DestroySubState		( State state )					=> state.Destroy();
	}
} 
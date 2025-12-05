using System;
using System.Collections.Generic;
using System.Linq;
using Flexy.AssetRefs.Editor;
using Flexy.AssetRefs.Pipelines;
using UnityEditor;

namespace FlexyTemplates.BarleyBreak.Build;

public class SetMapsToBuildSettings : IPipelineTask
{
	private const String MapsPathStart = "Assets/!Game/Maps/Field";

	public void Run( Pipeline ppln, Context ctx )
	{
		var refs		= ctx.Get<RefsList>();
		var sceneList	= refs.Where(r => r is SceneAsset).Distinct().Select(AssetDatabase.GetAssetPath).Where(p => p.StartsWith(MapsPathStart)).ToList();

		var currentScenes	= EditorBuildSettings.scenes;
		var newScenes		= new List<EditorBuildSettingsScene>( sceneList.Count + 1 );
		var bootSceneName	= "";

		foreach (var scn in currentScenes)
		{
			if (scn.path.StartsWith(MapsPathStart)) 
				continue;
		
			newScenes.Add( scn );
		}

		foreach (var s in sceneList)
			newScenes.Add(new EditorBuildSettingsScene(s, true));

		EditorBuildSettings.scenes = newScenes.ToArray();
	}
}
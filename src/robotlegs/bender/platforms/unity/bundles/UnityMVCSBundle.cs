//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using robotlegs.bender.extensions.contextview;
using robotlegs.bender.extensions.directCommandMap;
using robotlegs.bender.extensions.enhancedLogging;
using robotlegs.bender.extensions.eventCommandMap;
using robotlegs.bender.extensions.eventDispatcher;
using robotlegs.bender.extensions.localEventMap;
using robotlegs.bender.extensions.mediatorMap;
using robotlegs.bender.extensions.modularity;
using robotlegs.bender.extensions.viewManager;
using robotlegs.bender.extensions.viewProcessorMap;
using robotlegs.bender.extensions.vigilance;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.platforms.unity.extensions.contextview;
using robotlegs.bender.platforms.unity.extensions.debugLogging;
using robotlegs.bender.platforms.unity.extensions.viewManager;
using robotlegs.bender.platforms.unity.extensions.viewManager.impl;
using robotlegs.bender.platforms.unity.extensions.unitySingletons;
using robotlegs.bender.platforms.unity.extensions.unityMediatorManager;
using UnityEngine;

namespace robotlegs.bender.bundles
{
	public class UnityMVCSBundle : IExtension
	{
		public void Extend (IContext context)
		{
			context.LogLevel = LogLevel.INFO;

			if (Application.isEditor)
			{
				context.Install (typeof(UnitySingletonsExtension));
				context.Install (typeof(UnityMediatorManagerExtension));
			}
			context.Install(typeof(DebugLoggingExtension));
			context.Install(typeof(VigilanceExtension));
			context.Install(typeof(InjectableLoggerExtension));
			context.Install(typeof(UnityParentFinderExtension));
			context.Install(typeof(ContextViewExtension));
			context.Install(typeof(EventDispatcherExtension));
			context.Install(typeof(ModularityExtension));
			context.Install(typeof(DirectCommandMapExtension));
			context.Install(typeof(EventCommandMapExtension));
			context.Install(typeof(LocalEventMapExtension));
			context.Install(typeof(ViewManagerExtension));
			context.Install(typeof(MediatorMapExtension));
			context.Install(typeof(ViewProcessorMapExtension));
			context.Install(typeof(StageCrawlerExtension));
			context.Install(typeof(StageSyncExtension));
			context.Install(typeof(UnityViewStateWatcherExtension));

			context.Configure(typeof(UnityStageCrawlerConfig));
			context.Configure(typeof(ContextViewListenerConfig));
		}
	}
}

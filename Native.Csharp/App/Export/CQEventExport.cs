/*
 * 此文件由T4引擎自动生成, 请勿修改此文件中的代码!
 */
using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using Native.Csharp.Sdk.Cqp;
using Native.Csharp.Sdk.Cqp.EventArgs;
using Native.Csharp.Sdk.Cqp.Interface;
using Native.Csharp.Sdk.Cqp.Expand;
using Unity;
using Unity.Injection;

namespace Native.Csharp.App.Export
{
	/// <summary>	
	/// 表示酷Q事件导出的类	
	/// </summary>	
	public class CQEventExport	
	{	
		#region --字段--	
		private static CQApi api = null;	
		private static CQLog log = null;	
		#endregion	
		
		#region --构造函数--	
		/// <summary>	
		/// 由托管环境初始化的 <see cref="CQEventExport"/> 的新实例	
		/// </summary>	
		static CQEventExport ()	
		{	
			// 初始化 Costura.Fody	
			CosturaUtility.Initialize ();	
			
			Type type = typeof (Common.AppInfo);	// 反射初始化容器	
			type.GetProperty ("Id", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { "cn.mikualpha.livesign" });	
			type.GetProperty ("ResultCode", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { 1 });	
			type.GetProperty ("ApiVersion", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { 9 });	
			type.GetProperty ("Name", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { "直播开播提醒" });	
			type.GetProperty ("Version", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { new Version ("1.4.1") });	
			type.GetProperty ("VersionId", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { 1017 });	
			type.GetProperty ("Author", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { "初音未来Alpha" });	
			type.GetProperty ("Description", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { "直播平台开播提醒（斗鱼、Bilibili、Twitch、KingKong等）" });	
			type.GetProperty ("UnityContainer", BindingFlags.Public | BindingFlags.Static).SetMethod.Invoke (null, new object[] { new UnityContainer () });	
			
			// 调用方法进行注册	
			CQMain.Register (Common.AppInfo.UnityContainer);	
			
			// 调用方法进行实例化	
			ResolveBackcall ();	
		}	
		#endregion	
		
		#region --核心方法--	
		/// <summary>	
		/// 返回酷Q用于识别本应用的 AppID 和 ApiVer	
		/// </summary>	
		/// <returns>酷Q用于识别本应用的 AppID 和 ApiVer</returns>	
		[DllExport (ExportName = "AppInfo", CallingConvention = CallingConvention.StdCall)]	
		private static string AppInfo ()	
		{	
			return "9,cn.mikualpha.livesign";	
		}	
		
		/// <summary>	
		/// 接收应用 Authcode, 用于注册接口	
		/// </summary>	
		/// <param name="authCode">酷Q应用验证码</param>	
		/// <returns>返回注册结果给酷Q</returns>	
		[DllExport (ExportName = "Initialize", CallingConvention = CallingConvention.StdCall)]	
		private static int Initialize (int authCode)	
		{	
			// 向容器注册一个 CQApi 实例	
			api = new CQApi (authCode);	
			Common.AppInfo.UnityContainer.RegisterInstance<CQApi> ("cn.mikualpha.livesign", api);	
			// 向容器注册一个 CQLog 实例	
			log = new CQLog (authCode);	
			Common.AppInfo.UnityContainer.RegisterInstance<CQLog> ("cn.mikualpha.livesign", log);	
			// 注册插件全局异常捕获回调, 用于捕获未处理的异常, 回弹给 酷Q 做处理	
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;	
			// 本函数【禁止】处理其他任何代码，以免发生异常情况。如需执行初始化代码请在Startup事件中执行（Type=1001）。	
			return 0;	
		}	
		#endregion	
		
		#region --私有方法--	
		/// <summary>	
		/// 全局异常捕获, 用于捕获开发者未处理的异常, 此异常将回弹至酷Q进行处理	
		/// </summary>	
		/// <param name="sender">事件来源对象</param>	
		/// <param name="e">附加的事件参数</param>	
		private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)	
		{	
			Exception ex = e.ExceptionObject as Exception;	
			if (ex != null)	
			{	
				StringBuilder innerLog = new StringBuilder ();	
				innerLog.AppendLine ("发现未处理的异常!");	
				innerLog.AppendLine (ex.ToString ());	
				log.SetFatalMessage (innerLog.ToString ());	
			}	
		}	
		
		/// <summary>	
		/// 读取容器中的注册项, 进行事件分发	
		/// </summary>	
		private static void ResolveBackcall ()	
		{	
			/*	
			 * Id: 1	
			 * Type: 21	
			 * Name: 私聊消息处理	
			 * Function: _eventPrivateMsg	
			 * Priority: 30000	
			 */	
			if (Common.AppInfo.UnityContainer.IsRegistered<IPrivateMessage> ("私聊消息处理"))	
			{	
				Event_eventPrivateMsgHandler += Common.AppInfo.UnityContainer.Resolve<IPrivateMessage> ("私聊消息处理").PrivateMessage;	
			}	
			
			/*	
			 * Id: 2	
			 * Type: 2	
			 * Name: 群消息处理	
			 * Function: _eventGroupMsg	
			 * Priority: 30000	
			 */	
			if (Common.AppInfo.UnityContainer.IsRegistered<IGroupMessage> ("群消息处理"))	
			{	
				Event_eventGroupMsgHandler += Common.AppInfo.UnityContainer.Resolve<IGroupMessage> ("群消息处理").GroupMessage;	
			}	
			
			/*	
			 * Id: 3	
			 * Type: 4	
			 * Name: 讨论组消息处理	
			 * Function: _eventDiscussMsg	
			 * Priority: 30000	
			 */	
			if (Common.AppInfo.UnityContainer.IsRegistered<IDiscussMessage> ("讨论组消息处理"))	
			{	
				Event_eventDiscussMsgHandler += Common.AppInfo.UnityContainer.Resolve<IDiscussMessage> ("讨论组消息处理").DiscussMessage;	
			}	
			
			/*	
			 * Id: 1001	
			 * Type: 1001	
			 * Name: 酷Q启动事件	
			 * Function: _eventStartup	
			 * Priority: 30000	
			 */	
			if (Common.AppInfo.UnityContainer.IsRegistered<ICQStartup> ("酷Q启动事件"))	
			{	
				Event_eventStartupHandler += Common.AppInfo.UnityContainer.Resolve<ICQStartup> ("酷Q启动事件").CQStartup;	
			}	
			
			/*	
			 * Id: 1002	
			 * Type: 1002	
			 * Name: 酷Q关闭事件	
			 * Function: _eventExit	
			 * Priority: 30000	
			 */	
			if (Common.AppInfo.UnityContainer.IsRegistered<ICQExit> ("酷Q关闭事件"))	
			{	
				Event_eventExitHandler += Common.AppInfo.UnityContainer.Resolve<ICQExit> ("酷Q关闭事件").CQExit;	
			}	
			
			/*	
			 * Id: 1003	
			 * Type: 1003	
			 * Name: 应用已被启用	
			 * Function: _eventEnable	
			 * Priority: 30000	
			 */	
			if (Common.AppInfo.UnityContainer.IsRegistered<IAppEnable> ("应用已被启用"))	
			{	
				Event_eventEnableHandler += Common.AppInfo.UnityContainer.Resolve<IAppEnable> ("应用已被启用").AppEnable;	
			}	
			
			/*	
			 * Id: 1004	
			 * Type: 1004	
			 * Name: 应用将被停用	
			 * Function: _eventDisable	
			 * Priority: 30000	
			 */	
			if (Common.AppInfo.UnityContainer.IsRegistered<IAppDisable> ("应用将被停用"))	
			{	
				Event_eventDisableHandler += Common.AppInfo.UnityContainer.Resolve<IAppDisable> ("应用将被停用").AppDisable;	
			}	
			
		}	
		#endregion	
		
		#region --导出方法--	
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 1</para>	
		/// <para>Type: 21</para>	
		/// <para>Name: 私聊消息处理</para>	
		/// <para>Function: _eventPrivateMsg</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQPrivateMessageEventArgs> Event_eventPrivateMsgHandler;	
		[DllExport (ExportName = "_eventPrivateMsg", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventPrivateMsg (int subType, int msgId, long fromQQ, IntPtr msg, int font)	
		{	
			if (Event_eventPrivateMsgHandler != null)	
			{	
				CQPrivateMessageEventArgs args = new CQPrivateMessageEventArgs (api, log, 1, 21, "私聊消息处理", "_eventPrivateMsg", 30000, subType, msgId, fromQQ, msg.ToString(CQApi.DefaultEncoding), false);	
				Event_eventPrivateMsgHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 2</para>	
		/// <para>Type: 2</para>	
		/// <para>Name: 群消息处理</para>	
		/// <para>Function: _eventGroupMsg</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQGroupMessageEventArgs> Event_eventGroupMsgHandler;	
		[DllExport (ExportName = "_eventGroupMsg", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventGroupMsg (int subType, int msgId, long fromGroup, long fromQQ, string fromAnonymous, IntPtr msg, int font)	
		{	
			if (Event_eventGroupMsgHandler != null)	
			{	
				CQGroupMessageEventArgs args = new CQGroupMessageEventArgs (api, log, 2, 2, "群消息处理", "_eventGroupMsg", 30000, subType, msgId, fromGroup, fromQQ, fromAnonymous, msg.ToString(CQApi.DefaultEncoding), false);	
				Event_eventGroupMsgHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 3</para>	
		/// <para>Type: 4</para>	
		/// <para>Name: 讨论组消息处理</para>	
		/// <para>Function: _eventDiscussMsg</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQDiscussMessageEventArgs> Event_eventDiscussMsgHandler;	
		[DllExport (ExportName = "_eventDiscussMsg", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventDiscussMsg (int subType, int msgId, long fromDiscuss, long fromQQ, IntPtr msg, int font)	
		{	
			if (Event_eventDiscussMsgHandler != null)	
			{	
				CQDiscussMessageEventArgs args = new CQDiscussMessageEventArgs (api, log, 3, 4, "讨论组消息处理", "_eventDiscussMsg", 30000, subType, msgId, fromDiscuss, fromQQ, msg.ToString(CQApi.DefaultEncoding), false);	
				Event_eventDiscussMsgHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 1001</para>	
		/// <para>Type: 1001</para>	
		/// <para>Name: 酷Q启动事件</para>	
		/// <para>Function: _eventStartup</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQStartupEventArgs> Event_eventStartupHandler;	
		[DllExport (ExportName = "_eventStartup", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventStartup ()	
		{	
			if (Event_eventStartupHandler != null)	
			{	
				CQStartupEventArgs args = new CQStartupEventArgs (api, log, 1001, 1001, "酷Q启动事件", "_eventStartup", 30000);	
				Event_eventStartupHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 1002</para>	
		/// <para>Type: 1002</para>	
		/// <para>Name: 酷Q关闭事件</para>	
		/// <para>Function: _eventExit</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQExitEventArgs> Event_eventExitHandler;	
		[DllExport (ExportName = "_eventExit", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventExit ()	
		{	
			if (Event_eventExitHandler != null)	
			{	
				CQExitEventArgs args = new CQExitEventArgs (api, log, 1002, 1002, "酷Q关闭事件", "_eventExit", 30000);	
				Event_eventExitHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 1003</para>	
		/// <para>Type: 1003</para>	
		/// <para>Name: 应用已被启用</para>	
		/// <para>Function: _eventEnable</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQAppEnableEventArgs> Event_eventEnableHandler;	
		[DllExport (ExportName = "_eventEnable", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventEnable ()	
		{	
			if (Event_eventEnableHandler != null)	
			{	
				CQAppEnableEventArgs args = new CQAppEnableEventArgs (api, log, 1003, 1003, "应用已被启用", "_eventEnable", 30000);	
				Event_eventEnableHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		/// <summary>	
		/// 事件回调, 以下是对应 Json 文件的信息	
		/// <para>Id: 1004</para>	
		/// <para>Type: 1004</para>	
		/// <para>Name: 应用将被停用</para>	
		/// <para>Function: _eventDisable</para>	
		/// <para>Priority: 30000</para>	
		/// <para>IsRegex: False</para>	
		/// </summary>	
		public static event EventHandler<CQAppDisableEventArgs> Event_eventDisableHandler;	
		[DllExport (ExportName = "_eventDisable", CallingConvention = CallingConvention.StdCall)]	
		public static int Event_eventDisable ()	
		{	
			if (Event_eventDisableHandler != null)	
			{	
				CQAppDisableEventArgs args = new CQAppDisableEventArgs (api, log, 1004, 1004, "应用将被停用", "_eventDisable", 30000);	
				Event_eventDisableHandler (typeof (CQEventExport), args);	
			}	
			return 0;	
		}	
		
		#endregion	
	}	
}

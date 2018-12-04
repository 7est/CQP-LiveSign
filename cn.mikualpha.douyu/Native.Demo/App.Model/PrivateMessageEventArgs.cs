﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Native.Demo.App.Model
{
	public class PrivateMessageEventArgs : EventArgs
	{
		/// <summary>
		/// 消息ID
		/// </summary>
		public int MsgId { get; set; }
		/// <summary>
		/// 来源QQ
		/// </summary>
		public long FromQQ { get; set; }
		/// <summary>
		/// 消息内容
		/// </summary>
		public string Msg { get; set; }
		/// <summary>
		/// 字体指针
		/// </summary>
		public int Font { get; set; }

		/// <summary>
		/// 获取或设置一个值，该值指示是否处理过此事件
		/// </summary>
		public bool Handled { get; set; }
	}
}

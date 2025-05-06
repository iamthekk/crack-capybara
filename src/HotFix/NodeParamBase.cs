using System;

namespace HotFix
{
	public abstract class NodeParamBase
	{
		public abstract NodeKind GetNodeKind();

		public virtual double FinalCount { get; protected set; }
	}
}

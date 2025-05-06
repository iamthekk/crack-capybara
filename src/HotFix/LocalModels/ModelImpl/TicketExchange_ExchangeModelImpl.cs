using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TicketExchange_ExchangeModelImpl : BaseLocalModelImpl<TicketExchange_Exchange, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TicketExchange_Exchange();
		}

		protected override int GetBeanKey(TicketExchange_Exchange bean)
		{
			return bean.id;
		}
	}
}

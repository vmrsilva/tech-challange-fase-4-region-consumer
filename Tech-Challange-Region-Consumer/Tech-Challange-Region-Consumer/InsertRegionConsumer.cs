using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallange.Region.Domain.Base.Entity;
using TechChallange.Region.Domain.Region.Messaging;

namespace Tech_Challange_Region_Consumer
{
    public class InsertRegionConsumer : IConsumer<RegionCreateMessageDto>
    {
        public Task Consume(ConsumeContext<RegionCreateMessageDto> context)
        {
            var pedido = context.Message;

            Console.WriteLine($"{pedido.Name}");

            

            return Task.CompletedTask;
        }
    }

    
}

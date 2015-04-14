using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisameSi.ServiceLibrary.Entities
{
    public class ChannelEntity
    {
        string Name { get; set; }
        UserEntity Owner { get; set; }
    }
}

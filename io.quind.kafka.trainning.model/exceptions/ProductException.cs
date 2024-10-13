using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.model.exceptions
{
    public class ProductException(string message):ApplicationException(message);
   
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.quind.kafka.trainning.model.model
{
    public class Product
    {
        public int Id { get; set; }        
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int OrderPoint { get; set; }


    }
}

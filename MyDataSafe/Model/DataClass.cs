﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataSafe.Model
{
    public class DataClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TypeFile { get; set; }
        public byte[] Data { get; set; }
        public DateTime DateOfCreate { get; set; }
        public DataColor Color { get; set; }
        public int DataColorId { get; set; } = 7;

        public override string ToString()
        {
            return Name;
        }

    }
}

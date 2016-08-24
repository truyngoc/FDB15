﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
  public  class DPHANLOAI_GIA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
    }
}

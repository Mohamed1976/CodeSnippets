﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Data.Entities
{
  public class Product
  {
    public int Id { get; set; }
    public string Category { get; set; }
    public string Size { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DataType(DataType.Currency)]
    [Range(1, 100)]
    public decimal Price { get; set; }
    public string Title { get; set; }
    public string ArtDescription { get; set; }
    public string ArtDating { get; set; }
    public string ArtId { get; set; }
    public string Artist { get; set; }
    public DateTime ArtistBirthDate { get; set; }
    public DateTime ArtistDeathDate { get; set; }
    public string ArtistNationality { get; set; }
  }
}
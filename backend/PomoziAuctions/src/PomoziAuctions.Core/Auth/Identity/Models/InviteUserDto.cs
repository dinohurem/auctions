﻿namespace PomoziAuctions.Core.Auth.Identity.Models;

public class InviteUserDto
{
  public int? AuctioneerId { get; set; }

  public string Email { get; set; }

  public string Phone { get; set; }

  public ICollection<string> Roles { get; set; }

  public string Name { get; set; }
}

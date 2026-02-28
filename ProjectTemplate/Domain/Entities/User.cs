using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ProjectTemplate.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
}

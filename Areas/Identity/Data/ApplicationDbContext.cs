﻿using Flexi_Arm.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Flexi_Arm.Models;

namespace Flexi_Arm.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        
        //permet la configuration de nouveaux champs d'insciption
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }

    public DbSet<Flexi_Arm.Models.Recette>? Recette { get; set; }
}

//champ ou l'on configure l'inscription d'inscription details: https://www.youtube.com/watch?v=I-ZzFLruiuo&ab_channel=ISeeSharp 

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder. Property(u => u.Prenom).HasMaxLength(128); //c'est ici que l'on créee les propriété des champs principaux 
        }
    }

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using courseA4.Models;

namespace courseA4.Data;

public partial class CookingRecipeContext : DbContext
{
    public CookingRecipeContext()
    {
    }

    public CookingRecipeContext(DbContextOptions<CookingRecipeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientType> IngredientTypes { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeDetail> RecipeDetails { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<RecipeStep> RecipeSteps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseSqlServer("Server=db9093.public.databaseasp.net; Database=db9093; User Id=db9093; Password=C#o2@m6HP-h8; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True; ");
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-VR0EEHC\\SQLEXPRESS;Initial Catalog=CookingRecipes;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("PK__Ingredie__BEAEB27AD88FC7C8");

            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Ingredien__TypeI__3B75D760");
        });

        modelBuilder.Entity<IngredientType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Ingredie__516F03957DFCE257");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__Recipes__FDD988D05DCA6EEA");

            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");
            entity.Property(e => e.DifficultyLevel).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Recipes__UserID__3E52440B");
        });

        modelBuilder.Entity<RecipeDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RecipeDetails");

            entity.Property(e => e.DifficultyLevel).HasMaxLength(50);
            entity.Property(e => e.IngredientName).HasMaxLength(100);
            entity.Property(e => e.IngredientQuantity).HasMaxLength(50);
            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");
            entity.Property(e => e.RecipeName).HasMaxLength(100);
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => e.RecipeIngredientId).HasName("PK__RecipeIn__A2C34276BDD55559");

            entity.Property(e => e.RecipeIngredientId).HasColumnName("RecipeIngredientID");
            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.Quantity).HasMaxLength(50);
            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("FK__RecipeIng__Ingre__45F365D3");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("FK__RecipeIng__Recip__44FF419A");
        });

        modelBuilder.Entity<RecipeStep>(entity =>
        {
            entity.HasKey(e => e.StepId).HasName("PK__RecipeSt__24343337446A7153");

            entity.Property(e => e.StepId).HasColumnName("StepID");
            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.Quantity).HasMaxLength(50);
            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeSteps)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__RecipeSte__Ingre__4222D4EF");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeSteps)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("FK__RecipeSte__Recip__412EB0B6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC1F533663");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

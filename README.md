# AutoContext
AutoContext.EntityFrameworkCore is a lightweight extension of Entity Framework Core for entity to entity configuration mapping.

## How to build
AutoContext.EntityFrameworkCore is built against the latest ASP.NET Core 3.
* [Install](https://www.microsoft.com/net/download/core#/current) the latest .NET Core 3.x SDK
* Run `build.ps1` in the root of the repo

## Getting Started
* Install nuget package to a new or existing project
```sh
Install-Package AutoContext.EntityFrameworkCore
```
* Define your entities
```csharp
public class YourEntity
{
    public int Id { get; set; }
}
```
* Define your context
```csharp
public class YourContext : AutoContextBase
{
    public YourContext(DbContextOptions options) 
      : base(options)
    {
    }
}

// or for context without inheritance from AutoContext
public class YourContextWithoutInheritanceFromAutoContext : DbContext
{
    public YourContextWithoutInheritance(DbContextOptions options) 
      : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Database.ApplyConfigurations(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}
```
* Define your mapping configurations in any assembly
```csharp
[MappingConfiguration(contextType: typeof(YourContext))]
public abstract class YourBaseMapping<TEntity> : AbstractEntityTypeConfiguration<TEntity> 
  where TEntity : class
{
}

public class YourCustomEntityMapping : YourBaseMapping<YourEntity>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
      // Your configuration here
    }
}
```
* Register your context
```csharp
services.AddDbContext<YourContext>(options =>
{
    // other registrations
    
    // If the mapping assembly name is null then will be used assembly of YourContext.
    options.UseMappingAssembly();
    // or
    options.UseMappingAssembly(<yourMappingAssemblyName>);
});
  ```

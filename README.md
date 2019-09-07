# AutoContext
AutoContext.EntityFrameworkCore is a lightweight extension of Entity Framework Core for entity to entity configuration mapping.

## How to build
AutoContext.EntityFrameworkCore is built against the latest ASP.NET Core 2.
* [Install](https://www.microsoft.com/net/download/core#/current) the latest .NET Core 2.x SDK
* Run `build.ps1` in the root of the repo

## Getting Started
* Install nuget package to a new or existing project
```sh
Install-Package AutoContext.EntityFrameworkCore
```
* If you have only one context
  
  * Define your entities
  ```csharp
  public class YourEntity1
  {
      public int Id { get; set; }
  }
  
  public class YourQueryEntity2
  {
      public int Id { get; set; }
  }
  ```
  * Define your mapping configurations
  ```csharp
  public class YourEntityMapping1 : AutoContextEntityTypeConfiguration<YourEntity1>
  {
      public virtual void Configure(EntityTypeBuilder<TEntity> builder)
      {
        // Your configuration here
      }
  }
  
  // or for query type
  
  public class YourEntityMapping2 : AutoContextQueryTypeConfiguration<YourQueryEntity2>
  {
      public virtual void Configure(QueryTypeBuilder<TEntity> builder)
      {
        // Your configuration here
      }
  }
  ```
  * Register the AutoContext
  ```csharp
  services.AddDbContext<AutoContext>(options =>
  {
      // other registrations
      options.UseMappingAssembly();
      // or
      options.UseMappingAssembly(<yourMappingAssemblyName>);
  });
  ```
* if you have many contexts or the first approach does not suit you
  * Define your context and inherit from AutoContext
  ```csharp
  public class YourContext : AutoContext
  {
      public YourContext(DbContextOptions options) : base(options)
      {
      }
  }
  ```
  * Define base mapping configuration
  ```csharp
  [MappingConfiguration(contextType: typeof(YourContext))]
  public abstract class YourBaseMapping<TEntity> : AbstractEntityTypeConfiguration<TEntity> where TEntity : class
  {
  }
  ```
  * Use your base mapping configuration
  ```csharp
  public class YourEntity1
  {
      public int Id { get; set; }
  }

  public class YourCustomEntityMapping1 : YourBaseMapping<YourEntity1>
  {
      public virtual void Configure(EntityTypeBuilder<TEntity> builder)
      {
        // Your configuration here
      }
  }
  ```

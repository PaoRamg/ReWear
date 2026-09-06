# ReWear

## Descripción del proyecto

ReWear es una aplicación web desarrollada con ASP.NET Core MVC que tiene como objetivo mostrar publicaciones de prendas de segunda mano. El proyecto permite gestionar información relacionada con publicaciones, categorías y estados de las prendas.

Para el desarrollo se utilizó ASP.NET Core MVC junto con Entity Framework Core y SQL Server.



## Tecnologías utilizadas

* ASP.NET Core 8.0
* C#
* Entity Framework Core 8.0
* SQL Server
* Razor
* HTML y CSS
* Visual Studio 2022



## Estructura del proyecto

El proyecto se organizó siguiendo la estructura de ASP.NET Core MVC:

* **Models:** contiene las entidades del sistema.
* **Views:** contiene las interfaces que se muestran al usuario.
* **Controllers:** recibe las solicitudes y coordina la obtención de información.
* **Data:** contiene el `ApplicationDbContext`, encargado de establecer la comunicación entre la aplicación y la base de datos mediante Entity Framework Core.

Las principales entidades utilizadas son:

* `Publicacion`
* `Categoria`
* `EstadoPrenda`



# Enfoque de Entity Framework Core

## Enfoque seleccionado: Code First

Para el proyecto ReWear se seleccionó el enfoque **Code First** de Entity Framework Core.

La principal razón para elegir este enfoque es que el proyecto se está desarrollando desde cero y las entidades pueden definirse directamente mediante clases de C#. A partir de estas clases, Entity Framework Core permite crear y modificar la estructura de la base de datos mediante migraciones.

Además, la actividad solicita generar y aplicar una migración inicial, por lo que el enfoque Code First se adapta directamente a los requerimientos del proyecto.

En ReWear, las entidades principales fueron representadas mediante las clases:

* `Publicacion`
* `Categoria`
* `EstadoPrenda`

Estas clases se registraron en `ApplicationDbContext` mediante `DbSet`.

\---

## Implementación de Code First

El contexto utilizado en el proyecto es `ApplicationDbContext`, ubicado en la carpeta `Data`.

Este contexto contiene los `DbSet` correspondientes a las entidades:

C#
public DbSet<Publicacion> Publicaciones { get; set; }
public DbSet<Categoria> Categorias { get; set; }
public DbSet<EstadoPrenda> EstadosPrenda { get; set; }


También se configuraron las relaciones entre las entidades `Publicacion`, `Categoria` y `EstadoPrenda`.

La conexión con SQL Server se registró mediante inyección de dependencias en `Program.cs`.

Posteriormente, se utilizaron migraciones de Entity Framework Core para crear y actualizar la estructura de la base de datos.

Los principales comandos utilizados fueron:


Add-Migration Inicial


y:


Update-Database


También se creó una segunda migración para incorporar los datos iniciales:


Add-Migration DatosIniciales


seguida de:


Update-Database




# Datos iniciales

Para disponer de información de prueba en el proyecto se utilizó el método `HasData()` de Entity Framework Core.

Se agregaron datos iniciales para las entidades `Categoria`, `EstadoPrenda` y `Publicacion`.

Esto permite que la aplicación pueda mostrar publicaciones de prueba al ejecutar el proyecto y comprobar que la conexión entre ASP.NET Core, Entity Framework Core y SQL Server funciona correctamente.



# ¿Por qué se descartó Database First?

El enfoque **Database First** no fue seleccionado porque está orientado principalmente a escenarios donde ya existe una base de datos establecida y se desea generar las clases del modelo a partir de ella.

En este proyecto, aunque se dispone de un archivo SQL de referencia, la base de datos se está construyendo como parte del desarrollo de la aplicación. Por ello, resulta más conveniente definir las entidades en C# y utilizar migraciones para generar la estructura correspondiente en SQL Server.

El archivo SQL proporcionado se utilizó como referencia para conocer las entidades, propiedades y relaciones necesarias.



# ¿Por qué se descartó Model First?

El enfoque **Model First** tampoco fue seleccionado porque requiere trabajar con un modelo visual para representar las entidades y posteriormente generar la estructura correspondiente.

Para ReWear se consideró innecesario utilizar este enfoque debido a que el modelo solicitado es pequeño y está compuesto únicamente por tres entidades principales.

Definir directamente las entidades mediante clases C# resulta más sencillo y permite trabajar de forma directa con Entity Framework Core y las migraciones.



# Conclusión

Se seleccionó **Code First** porque se adapta mejor a las características del proyecto ReWear y a los requerimientos de la actividad.

Este enfoque permite definir las entidades directamente en C#, establecer sus relaciones mediante `ApplicationDbContext` y utilizar migraciones para crear y actualizar la base de datos en SQL Server.

Además, facilita la incorporación de datos iniciales mediante `HasData()` y permite mantener sincronizado el modelo de la aplicación con la estructura de la base de datos durante el desarrollo.


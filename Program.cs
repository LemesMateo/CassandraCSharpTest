using System;
using ApacheCassandra.Controllers;
using ApacheCassandra.Models;
using Cassandra;
namespace CassandraCSharpTest;
public class Program
{
    public static async Task Main()
    {
        var cluster = Cluster.Builder()
            .AddContactPoint("127.0.0.1")
            .WithPort(9042)
            .Build();
        var session = cluster.Connect("testingks");

        DbController dbController = new DbController();

        Console.WriteLine("Hola, bienvenido a esta prueba de Cassandra");
        Console.WriteLine("--------------------------------------------------");        
        int input = 5;
        do
        {
            
            Console.WriteLine("Que desea hacer?");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("1-Ver listado de productos");
            Console.WriteLine("2-Insertar nuevo producto");
            Console.WriteLine("3-Actualizar producto");
            Console.WriteLine("4-Eliminar producto");
            Console.WriteLine("0-Salir");
            Console.WriteLine("--------------------------------------------------");
            input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("Listado de productos:");
                    var listaProductos = await dbController.List();
                    foreach(Productos producto in listaProductos)
                    {
                        Console.WriteLine($"Id:{producto.id} Name:{producto.name} Price:{producto.price} Units:{producto.units}");
                    }
                    Console.WriteLine("--------------------------------------------------");
                    break;
                case 2:
                    Console.Write("Ingrese el id del producto: ");
                    int idInsert = Convert.ToInt32(Console.ReadLine());
                    Console.Write("\nIngrese el nombre del producto: ");
                    string nameInsert = Console.ReadLine()!;
                    Console.Write("\nIngrese el precio del producto: ");
                    double priceInsert = Convert.ToDouble(Console.ReadLine());
                    Console.Write("\nIngrese las unidades del producto: ");
                    int unitsInsert = Convert.ToInt32(Console.ReadLine());
                    Productos nuevoProducto = new Productos( idInsert, nameInsert, priceInsert, unitsInsert );
                    await dbController.Insert(nuevoProducto);
                    //session.Execute($"insert into products (id, name, price, units) values ({idInsert}, '{nameInsert}', {priceInsert}, {unitsInsert})");
                    Console.WriteLine("\n--------------------------------------------------");
                    break;
                case 3:
                    Console.WriteLine("Ingrese el id del producto a actualizar:");
                    int idUpdate = int.Parse(Console.ReadLine());
                    Console.WriteLine("Ingrese el nuevo nombre del producto:");
                    string nameUpdate = Console.ReadLine();
                    Console.WriteLine("Ingrese el nuevo precio del producto:");
                    double priceUpdate = double.Parse(Console.ReadLine());
                    Console.WriteLine("Ingrese las nuevas unidades del producto:");
                    int unitsUpdate = int.Parse(Console.ReadLine());
                    Productos productoActualizar = new Productos(idUpdate, nameUpdate, priceUpdate, unitsUpdate);
                    await dbController.Update(productoActualizar);
                    //session.Execute($"update products set name = '{nameUpdate}', price = {priceUpdate}, units = {unitsUpdate} where id = {idUpdate}");
                    Console.WriteLine("--------------------------------------------------");
                    break;
                case 4:
                    Console.WriteLine("Ingrese el id del producto a eliminar:");
                    string idDelete = Console.ReadLine();
                    await dbController.Delete(idDelete);
                    //session.Execute($"delete from products where id = {idDelete}");
                    Console.WriteLine("--------------------------------------------------");
                    break;
                case 0:
                    break;
            }

            
        }
        while (input != 0);




    }
}
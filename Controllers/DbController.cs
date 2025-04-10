﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using ApacheCassandra.Models;
using Cassandra;
using Cassandra.Mapping;
namespace ApacheCassandra.Controllers;

public class DbController 
{
    //private readonly IConfiguration _config;
    private Cluster _cluster;
    private Session _session;
    private IMapper _mapper;
    public DbController()
    {
        _cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .WithPort(9042)
                .Build();
        _session = (Session)_cluster.Connect("testingks");
        _mapper = new Mapper(_session);
    }
    //public HomeController(IConfiguration config)
    //{
    //    _config = config;
    //    _cluster = Cluster.Builder().WithPort(9042).AddContactPoint(_config.GetSection("Cassandra").GetValue<string>("Server")).Build();
    //    _session = (Session)_cluster.Connect("testingks");
    //    _mapper = new Mapper(_session);
    //}

    public async Task<IEnumerable<Productos>> List()
    {
        IEnumerable<Productos> productos = await _mapper.FetchAsync<Productos>("SELECT id, name, price, units FROM products");
        return productos;
    }
    public async Task<Productos> Get(string id)
    {
        Productos producto = await _mapper.FirstOrDefaultAsync<Productos>("SELECT id, name, price, units FROM products WHERE id = ?", int.Parse(id));
        return producto;
    }
    public async Task Delete(string id)
    {
       
        try
        {
            var producto = await Get(id);
            if (producto == null)
            {
                Console.WriteLine("\nEl id ingresado no existe");
                return;
            }
            var stm = await _session.PrepareAsync("DELETE FROM products WHERE id = ?");
            BoundStatement bound = stm.Bind(producto.id);
            await _session.ExecuteAsync(bound);
            Console.WriteLine("\nEl producto fue eliminado correctamente");
            return;

        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }        
    }
    public async Task Update(Productos producto)
    {
        try
        {
                Productos result = await _mapper.FirstOrDefaultAsync<Productos>("SELECT * FROM products WHERE id = ?", producto.id);
            if (result == null)
            {
                Console.WriteLine("\nEl id ingresado no existe");
                return;
            }
            var stm = await _session.PrepareAsync("UPDATE products SET name = ?, price = ?, units = ? WHERE id = ?");
            BoundStatement bound = stm.Bind(producto.name, producto.price, producto.units, producto.id);
            await _session.ExecuteAsync(bound);
            Console.WriteLine("\nEl producto fue actualizado correctamente");
            // Necesito saber que me devuelve _session.ExecuteAsync(bound) para poder avisarle al usuario que pasó
            // también necesitamos un try catch para manejar excepciones, tendría que empezar desde el comienzo(linea 57)
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    public async Task Insert(Productos producto)
    {
        try
        {
            Productos result = await _mapper.FirstOrDefaultAsync<Productos>("SELECT * FROM products WHERE id = ?", producto.id);
            if (result != null)
            {
                Console.WriteLine("\nEl id ingresado ya existe");
                return;
            } else
            {
                var stm = await _session.PrepareAsync("INSERT INTO products (id, name, price, units) VALUES (?, ?, ?, ?)");
                BoundStatement bound = stm.Bind(producto.id, producto.name, producto.price, producto.units);
                await _session.ExecuteAsync(bound);
                Console.WriteLine("\nEl producto fue insertado correctamente");
                return;

            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        } 
        // Necesito saber que me devuelve _session.ExecuteAsync(bound) para poder avisarle al usuario que pasó
        // también necesitamos un try catch para manejar excepciones, tendría que empezar desde el comienzo(linea 71)
    }
}

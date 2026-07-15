INSERT INTO TiposProductos (Nombre)
VALUES ('Motherboard'), ('Procesador'), ('Memoria RAM');


INSERT INTO Proveedores (RazonSocial, CUIT, Nombre)
VALUES ('TechWorld SA', '30-12345678-9', 'TechWorld'),
       ('CompuGlobal', '30-98765432-1', 'CompuGlobal');


	   INSERT INTO Productos (Nombre, Codigo, Precio, Stock, TipoProductoId, ProveedorId)
VALUES ('ASUS Prime B450', 'MB001', 120000, 10, 1, 1),
       ('Intel Core i5', 'CPU001', 200000, 15, 2, 2),
       ('Kingston 8GB DDR4', 'RAM001', 45000, 30, 3, 1);


	   INSERT INTO Clientes (Nombre, Apellido, Email)
VALUES ('Juan', 'Pérez', 'juan.perez@email.com'),
       ('María', 'Gómez', 'maria.gomez@email.com');


	   INSERT INTO Compras (Fecha, ProveedorId, Cantidad, PrecioUnitario)
VALUES ('2026-06-01', 1, 5, 120000),
       ('2026-06-02', 2, 3, 200000);


	   INSERT INTO CompraDetalles (CompraId, ProductoId, Cantidad, PrecioUnitario)
VALUES (1, 1, 5, 120000),
       (2, 2, 3, 200000);


	   INSERT INTO Ventas (Fecha, ClienteId)
VALUES ('2026-06-03', 1),
       ('2026-06-03', 2);


	   INSERT INTO VentaDetalles (VentaId, ProductoId, Cantidad, PrecioUnitario)
VALUES (1, 1, 1, 120000),
       (1, 3, 2, 45000),
       (2, 2, 1, 200000);
--Ver productos con su proveedor:

	   SELECT p.Nombre AS Producto, pr.Nombre AS Proveedor
FROM Productos p
JOIN Proveedores pr ON p.ProveedorId = pr.Id;

--Ver compras con detalles:
SELECT c.Id AS Compra, cd.Cantidad, cd.PrecioUnitario, p.Nombre AS Producto
FROM Compras c
JOIN CompraDetalles cd ON c.Id = cd.CompraId
JOIN Productos p ON cd.ProductoId = p.Id;

---Ver ventas con detalles:


SELECT v.Id AS Venta, cl.Nombre AS Cliente, vd.Cantidad, vd.PrecioUnitario, p.Nombre AS Producto
FROM Ventas v
JOIN Clientes cl ON v.ClienteId = cl.Id
JOIN VentaDetalles vd ON v.Id = vd.VentaId
JOIN Productos p ON vd.ProductoId = p.Id;


--Stock después de una venta  
--Verifica que el stock de un producto se reduce correctamente:


SELECT Nombre, Stock
FROM Productos
WHERE Id = 1; -- ASUS Prime B450

--Total gastado por cliente  
--Calcula cuánto gastó cada cliente:


SELECT c.Nombre, SUM(vd.Cantidad * vd.PrecioUnitario) AS TotalGastado
FROM Clientes c
JOIN Ventas v ON c.Id = v.ClienteId
JOIN VentaDetalles vd ON v.Id = vd.VentaId
GROUP BY c.Nombre;
---Total comprado a proveedores  
---muestra cuánto se compró a cada proveedor:


SELECT p.Nombre AS Proveedor, SUM(cd.Cantidad * cd.PrecioUnitario) AS TotalComprado
FROM Proveedores p
JOIN Compras c ON p.Id = c.ProveedorId
JOIN CompraDetalles cd ON c.Id = cd.CompraId
GROUP BY p.Nombre;
--Productos más vendidos  
---Identifica cuáles son los productos con más ventas:


SELECT pr.Nombre AS Producto, SUM(vd.Cantidad) AS TotalVendido
FROM Productos pr
JOIN VentaDetalles vd ON pr.Id = vd.ProductoId
GROUP BY pr.Nombre
ORDER BY TotalVendido DESC;


SELECT Nombre, Stock FROM Productos;



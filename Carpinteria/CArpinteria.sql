CREATE DATABASE Carpinteria;
go
USE Carpinteria;
go
-- Tabla T_PRESUPUESTOS
CREATE TABLE T_PRESUPUESTOS (
    presupuesto_nro INT PRIMARY KEY,
    fecha DATE,
    cliente VARCHAR(100),
    costoMO DECIMAL(10, 2),
    descuento DECIMAL(10, 2),
    fecha_baja DATE
);

-- Tabla T_PRODUCTOS
CREATE TABLE T_PRODUCTOS (
    id_producto INT PRIMARY KEY,
    n_producto VARCHAR(100),
    precio DECIMAL(10, 2),
    activo BIT
);

-- Tabla T_DETALLES_PRESUPUESTO
CREATE TABLE T_DETALLES_PRESUPUESTO (
    presupuesto_nro INT,
    detalle_nro INT,
    id_producto INT,
    cantidad INT,
    PRIMARY KEY (presupuesto_nro, detalle_nro),
    FOREIGN KEY (presupuesto_nro) REFERENCES T_PRESUPUESTOS(presupuesto_nro),
    FOREIGN KEY (id_producto) REFERENCES T_PRODUCTOS(id_producto)
);



INSERT INTO T_PRODUCTOS (id_producto, n_producto, precio, activo) VALUES
(1, 'Mesa de Roble', 250.00, 1),
(2, 'Silla de Pino', 75.50, 1),
(3, 'Estante de Cedro', 180.75, 1),
(4, 'Puerta de Nogal', 320.00, 1),
(5, 'Escritorio de Caoba', 450.25, 1);


--SP

CREATE PROCEDURE SP_ULT_ID
    @nro INT OUTPUT
AS
BEGIN
    SET @nro = (SELECT ISNULL(MAX(presupuesto_nro), 0) + 1 FROM T_PRESUPUESTOS);
END;

go
CREATE PROCEDURE [dbo].[SP_INSERTAR_PRESUPUESTO]
    @fecha DATE,
    @cliente VARCHAR(255), 
    @costoMO DECIMAL(10, 2),
    @descuento DECIMAL(10, 2),
    @nro INT OUTPUT
AS
BEGIN
    -- Insertar un nuevo presupuesto en la tabla T_PRESUPUESTOS
    INSERT INTO T_PRESUPUESTOS(fecha, cliente, costoMO, descuento, fecha_baja)
    VALUES (@fecha, @cliente, @costoMO, @descuento, NULL);

    -- Obtener el ID del nuevo presupuesto insertado
    SET @nro = SCOPE_IDENTITY();
END
GO


CREATE PROCEDURE [dbo].[SP_INSERTAR_DETALLES_PRESUPUESTO]
    @presupuesto_nro INT,
    @detalle_nro INT,
    @id_producto INT,
    @cantidad INT
AS
BEGIN
    INSERT INTO T_DETALLES_PRESUPUESTO(presupuesto_nro, detalle_nro, id_producto, cantidad)
    VALUES (@presupuesto_nro, @detalle_nro, @id_producto, @cantidad);
END
GO


CREATE PROCEDURE [dbo].[SP_CONSULTAR_PRODUCTOS]
AS
BEGIN
	
	SELECT * FROM T_PRODUCTOS ORDER BY 2;
END
CREATE DATABASE DBCLIENTES

USE  DBCLIENTES

CREATE TABLE CLIENTE(
IdCliente int identity,
Nombres varchar(100),
Nit varchar(100),
Telefono varchar(100),
Direccion varchar(100),
Ciudad varchar(100),
Correo varchar(100)
)


insert into CLIENTE(Nombres,Nit,Telefono,Direccion, Ciudad, Correo) values
('Juan José','001','12345678910','cr 12 #12 - 03','Medellín','juanjose@gmail.com'),
('Juan','001','12345678910','cr 86 #18 - 32','Medellín','juan@gmail.com'),
('José','002','12345678910','cr 14 #15 - 12','Calí','jose@gmail.com'),
('Pedro','003','12345678910','cr 58 #13 - 24','Bogotá','pedro445@gmail.com'),
('Adriana','004','12345678910','cll 12 #19 - 65','Bogotá','adriana455@gmail.com')

select * from CLIENTE


create procedure sp_Registrar(
@Nombres varchar(100),
@Nit varchar(100),
@Telefono varchar(100),
@Direccion varchar(100),
@Ciudad varchar(100),
@Correo varchar(100)
)

as
begin
	insert into CLIENTE(Nombres,Nit,Telefono,Direccion, Ciudad, Correo) values (@Nombres, @Nit, @Telefono, @Direccion, @Ciudad, @Correo)
end


create procedure sp_Editar(
@IdCliente int,
@Nombres varchar(100),
@Nit varchar(100),
@Telefono varchar(100),
@Direccion varchar(100),
@Ciudad varchar(100),
@Correo varchar(100)
)
as
begin
	update CLIENTE set Nombres = @Nombres, Nit = @Nit, Telefono = @Telefono , Direccion = @Direccion, Ciudad = @Ciudad, Correo = @Correo where IdCliente = @IdCliente
end



create procedure sp_Eliminar(
@IdCliente int
)
as
begin
	delete from CLIENTE where IdCliente = @IdCliente
end


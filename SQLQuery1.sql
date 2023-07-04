CREATE TABLE dbo.Roles
        (
            Id int identity(1,1) primary key,
            Description varchar(15)
        )

INSERT INTO Roles VALUES ('ADMIN');
INSERT INTO Roles VALUES ('OTHER');

CREATE TABLE dbo.RolesUsuario
        (
            Id int identity(1,1) primary key,
            IdRole int not null,
            IdUser int not null
        )

INSERT INTO RolesUsuario values (1,5);
INSERT INTO RolesUsuario values (2,3);



select * from RolesUsuario;
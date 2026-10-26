/* =========================================================================================
   SCRIPT DE HIDRATACIÓN ORGÁNICA PARA REPORTES Y GRÁFICOS - VETERINARIADB
   Arquitectura: .NET 10 / SQL Server
   Objetivo: Poblar 18 meses de historia clínica continua, censo demográfico con distribución
             de Pareto en propietarios, proporciones clínicas realistas y cobros consistentes.
   Carácter: Idempotente (preserva usuarios base, roles, especies, métodos de pago y backups).
   ========================================================================================= */

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

-- =========================================================================================
-- 1. LIMPIEZA CONTROLADA DE DATOS TRANSACCIONALES Y CATÁLOGOS CLÍNICOS DE PRUEBA
-- =========================================================================================
DELETE FROM dbo.Pago;
DELETE FROM dbo.AplicacionVacuna;
DELETE FROM dbo.DetalleConsulta;
DELETE FROM dbo.Consulta;
DELETE FROM dbo.Mascota;
DELETE FROM dbo.Propietario;
DELETE FROM dbo.Vacuna;
DELETE FROM dbo.Tratamiento;
DELETE FROM dbo.Raza;

DBCC CHECKIDENT ('dbo.Pago', RESEED, 0);
DBCC CHECKIDENT ('dbo.AplicacionVacuna', RESEED, 0);
DBCC CHECKIDENT ('dbo.DetalleConsulta', RESEED, 0);
DBCC CHECKIDENT ('dbo.Consulta', RESEED, 0);
DBCC CHECKIDENT ('dbo.Mascota', RESEED, 0);
DBCC CHECKIDENT ('dbo.Propietario', RESEED, 0);
DBCC CHECKIDENT ('dbo.Vacuna', RESEED, 0);
DBCC CHECKIDENT ('dbo.Tratamiento', RESEED, 0);
DBCC CHECKIDENT ('dbo.Raza', RESEED, 0);

-- =========================================================================================
-- 2. NORMALIZACIÓN DE ESPECIES, PRECIOS DE CONSULTA, MÉTODOS DE PAGO Y VETERINARIOS
-- =========================================================================================
UPDATE dbo.PrecioConsulta SET Precio = 18000.00 WHERE IdEspecie = 1; -- Canino
UPDATE dbo.PrecioConsulta SET Precio = 16500.00 WHERE IdEspecie = 2; -- Felino
UPDATE dbo.PrecioConsulta SET Precio = 14000.00 WHERE IdEspecie = 3; -- Ave
UPDATE dbo.PrecioConsulta SET Precio = 14000.00 WHERE IdEspecie = 4; -- Roedor

UPDATE dbo.MetodoPago SET Nombre = N'Efectivo', Activo = 1 WHERE Id = 1;
UPDATE dbo.MetodoPago SET Nombre = N'Tarjeta de Débito', Activo = 1 WHERE Id = 2;
UPDATE dbo.MetodoPago SET Nombre = N'Tarjeta de Crédito', Activo = 1 WHERE Id = 3;
UPDATE dbo.MetodoPago SET Nombre = N'Transferencia Bancaria', Activo = 1 WHERE Id = 4;

-- Desactivar usuario de prueba basura ("da FDDFSDF") y normalizar nombres de veterinarios activos
UPDATE dbo.Usuario SET Activo = 0 WHERE NombreUsuario IN (N'DNJANKA', N'LLOPEZ');

UPDATE dbo.Usuario
SET Nombre = N'Mariano', Apellido = N'Rodríguez', Matricula = N'MP-2045', Activo = 1
WHERE NombreUsuario = N'MROGRIGUEZ.VET';

UPDATE dbo.Usuario
SET Nombre = N'Lucía', Apellido = N'Pérez', Matricula = N'MN-1024', Activo = 1
WHERE NombreUsuario = N'vet1' OR DNI = N'23456789';

UPDATE dbo.Usuario
SET Nombre = N'María', Apellido = N'Sánchez', Matricula = N'MP-3118', Activo = 1
WHERE NombreUsuario = N'MSANCHEZ';

UPDATE dbo.Usuario
SET Nombre = N'Ricardo Daniel', Apellido = N'Mormina', Matricula = N'MP-4092', Activo = 1
WHERE NombreUsuario = N'RMORMINA';

DECLARE @IdVetPrincipal BIGINT = (SELECT TOP 1 Id FROM dbo.Usuario WHERE NombreUsuario = N'MROGRIGUEZ.VET' AND Activo = 1);
DECLARE @IdVetSenior    BIGINT = (SELECT TOP 1 Id FROM dbo.Usuario WHERE (NombreUsuario = N'vet1' OR DNI = N'23456789') AND Activo = 1);
DECLARE @IdVetClinico   BIGINT = (SELECT TOP 1 Id FROM dbo.Usuario WHERE NombreUsuario = N'MSANCHEZ' AND Activo = 1);
DECLARE @IdVetJunior    BIGINT = (SELECT TOP 1 Id FROM dbo.Usuario WHERE NombreUsuario = N'RMORMINA' AND Activo = 1);

-- Fallback defensivo por si algún usuario no existiera
IF @IdVetPrincipal IS NULL SELECT TOP 1 @IdVetPrincipal = Id FROM dbo.Usuario WHERE IdTipoUsuario = 2 AND Activo = 1 ORDER BY Id;
IF @IdVetSenior IS NULL    SET @IdVetSenior = @IdVetPrincipal;
IF @IdVetClinico IS NULL   SET @IdVetClinico = @IdVetPrincipal;
IF @IdVetJunior IS NULL    SET @IdVetJunior = @IdVetSenior;

-- =========================================================================================
-- 3. CATÁLOGO DE RAZAS POR ESPECIE
-- =========================================================================================
INSERT INTO dbo.Raza (IdEspecie, Nombre, Activo) VALUES
-- Caninos (IdEspecie = 1) -> IDs 1..6
(1, N'Mestizo Canino', 1),
(1, N'Caniche', 1),
(1, N'Labrador Retriever', 1),
(1, N'Bulldog Francés', 1),
(1, N'Ovejero Alemán', 1),
(1, N'Golden Retriever', 1),
-- Felinos (IdEspecie = 2) -> IDs 7..10
(2, N'Común Europeo', 1),
(2, N'Siamés', 1),
(2, N'Persa', 1),
(2, N'Maine Coon', 1),
-- Aves (IdEspecie = 3) -> IDs 11..12
(3, N'Cacatúa Ninfa', 1),
(3, N'Canario Doméstico', 1),
-- Roedores (IdEspecie = 4) -> IDs 13..14
(4, N'Cobayo', 1),
(4, N'Conejo Californiano', 1);

-- =========================================================================================
-- 4. CATÁLOGO DE VACUNAS Y TRATAMIENTOS CON PRECIOS REALISTAS (ARS)
-- =========================================================================================
INSERT INTO dbo.Vacuna (IdEspecie, Nombre, PeriodoMesesRecomendado, Precio, Activo) VALUES
-- Caninos (IDs 1..4)
(1, N'Séxtuple Canina (DHPPI+L)', 12, 24500.00, 1),
(1, N'Antirrábica Anual Canina', 12, 19500.00, 1),
(1, N'Refuerzo Séxtuple Cachorro', 1, 22000.00, 1),
(1, N'Bordetella (Tos de las Perreras)', 12, 26000.00, 1),
-- Felinos (IDs 5..7)
(2, N'Triple Felina (FVRCP)', 12, 23500.00, 1),
(2, N'Antirrábica Anual Felina', 12, 19500.00, 1),
(2, N'Leucemia Felina (FeLV)', 12, 29000.00, 1),
-- Otros / Roedores (ID 8)
(4, N'Mixomatosis Viral', 6, 21000.00, 1);

INSERT INTO dbo.Tratamiento (TipoTratamiento, Descripcion, Dosis, Precio, Activo) VALUES
-- IDs 1..10
(N'Desparasitación', N'Desparasitación interna de amplio espectro (Praziquantel + Pirantel)', N'1 comp. cada 10 kg (dosis única)', 14500.00, 1),
(N'Desparasitación', N'Aplicación ectoparasiticida Spot-On (Pipeta pulgas y garrapatas)', N'1 pipeta tópica según rango de peso', 18500.00, 1),
(N'Medicación Clínica', N'Terapia antiemética y gastroprotectora parenteral', N'Maropitant 1 mg/kg SC + Omeprazol c/24h', 22500.00, 1),
(N'Medicación Clínica', N'Antibioticoterapia sistémica de amplio espectro', N'Amoxicilina + Ác. Clavulánico 12.5 mg/kg c/12h x 7 días', 25000.00, 1),
(N'Medicación Clínica', N'Analgesia y terapia antiinflamatoria (AINE)', N'Meloxicam 0.1 mg/kg c/24h vía oral x 5 días', 17500.00, 1),
(N'Dermatología y Otología', N'Lavaje ótico antiséptico y medicación tópica polivalente', N'4 gotas en conducto auditivo c/12h x 10 días', 21000.00, 1),
(N'Dermatología y Otología', N'Tratamiento dermatológico antialérgico y champú medicado', N'Prednisolona 0.5 mg/kg + Clorhexidina 2% semanal', 24000.00, 1),
(N'Procedimiento Ambulatorio', N'Fluidoterapia endovenosa de rehidratación y electrolitos', N'Ringer Lactato 40 ml/kg EV en internación diurna', 38000.00, 1),
(N'Procedimiento Ambulatorio', N'Toilette quirúrgico, antisepsia y sutura de herida superficial', N'Iodopovidona + puntos simples + Cefalexina 25 mg/kg', 34000.00, 1),
(N'Procedimiento Ambulatorio', N'Profilaxis periodontal por ultrasonido (destartraje)', N'Limpieza ultrasónica bajo sedación ligera', 42000.00, 1);

-- =========================================================================================
-- 5. PROPIETARIOS (50 PROPIETARIOS ARGENTINOS REALISTAS)
--    Los primeros 8 propietarios forman una escalera de Pareto (7, 6, 5, 5, 4, 4, 3, 3 mascotas)
-- =========================================================================================
INSERT INTO dbo.Propietario (DNI, Nombre, Apellido, Telefono, CorreoElectronico, Direccion, Activo) VALUES
-- Top 8 Propietarios para el gráfico de Barras Horizontales (IDs 1..8)
(N'28451901', N'Valeria Soledad', N'Montes de Oca', N'387-4510291', N'valeria.montesdeoca@gmail.com', N'Av. Belgrano 1420, Salta', 1),         -- 7 mascotas
(N'31094822', N'Santiago Ezequiel', N'Quiroga', N'387-5129043', N'santiago.quiroga85@hotmail.com', N'Caseros 845, Salta', 1),                  -- 6 mascotas
(N'29847110', N'Carolina Inés', N'Figueroa', N'387-4892310', N'caro.figueroa.vet@gmail.com', N'Juramento 312, Salta', 1),                      -- 5 mascotas
(N'33491025', N'Martín Alejandro', N'Cornejo', N'387-5301928', N'mcornejo.salta@gmail.com', N'Av. Reyes Católicos 1890, Salta', 1),            -- 5 mascotas
(N'35102948', N'Florencia Belén', N'Aráoz', N'387-4771209', N'flor.araoz@yahoo.com.ar', N'Leguizamón 654, Salta', 1),                          -- 4 mascotas
(N'26748391', N'Gustavo Adolfo', N'Torino', N'387-5908123', N'gustavo.torino@gmail.com', N'Mitre 1105, Salta', 1),                             -- 4 mascotas
(N'37891204', N'Jorgelina Paz', N'Alvarado', N'387-4665432', N'jorgelina.alvarado@gmail.com', N'Zuviría 920, Salta', 1),                       -- 3 mascotas
(N'32509184', N'Nicolás Federico', N'Saravia', N'387-5214789', N'nico.saravia@outlook.com', N'Santiago del Estero 480, Salta', 1),             -- 3 mascotas
-- Propietarios con 2 mascotas (IDs 9..29 -> 21 propietarios = 42 mascotas)
(N'34112098', N'Luciana María', N'SOLÁ', N'387-4321098', N'luciana.sola@gmail.com', N'Gral. Güemes 740, Salta', 1),
(N'27901234', N'Federico Gabriel', N'Ledesma', N'387-5432109', N'fede.ledesma@gmail.com', N'España 1250, Salta', 1),
(N'39012345', N'Camila Victoria', N'Ovejero', N'387-4543210', N'cami.ovejero@gmail.com', N'Pueyrredón 530, Salta', 1),
(N'30123456', N'Diego Sebastián', N'Uriburu', N'387-5654321', N'diego.uriburu@hotmail.com', N'Deán Funes 815, Salta', 1),
(N'41234567', N'Agustina Rocío', N'Castellanos', N'387-4765432', N'agus.castellanos@gmail.com', N'Alvarado 1340, Salta', 1),
(N'25345678', N'Roberto Carlos', N'San Millán', N'387-5876543', N'roberto.sanmillan@gmail.com', N'Urquiza 690, Salta', 1),
(N'36456789', N'Paula Andrea', N'Usandivaras', N'387-4987654', N'paula.usandivaras@gmail.com', N'20 de Febrero 410, Salta', 1),
(N'38567890', N'Gonzalo Matías', N'Bavastro', N'387-5098765', N'gonzalo.bavastro@gmail.com', N'Balcarce 950, Salta', 1),
(N'40678901', N'Micaela Abril', N'Arias', N'387-4109876', N'mica.arias@gmail.com', N'Necochea 720, Salta', 1),
(N'29789012', N'Hernán Darío', N'Peñalba', N'387-5210987', N'hernan.penalba@gmail.com', N'Ameghino 560, Salta', 1),
(N'33890123', N'Romina Natalia', N'Ceballos', N'387-4320987', N'romina.ceballos@hotmail.com', N'Entre Ríos 1120, Salta', 1),
(N'35901234', N'Facundo Tomás', N'Rovaletti', N'387-5430987', N'facu.rovaletti@gmail.com', N'Rivadavia 890, Salta', 1),
(N'42012345', N'Valentina Sofía', N'Dávalos', N'387-4540987', N'valen.davalos@gmail.com', N'Alsina 630, Salta', 1),
(N'24123456', N'Eduardo Jorge', N'Patrón Costas', N'387-5650987', N'eduardo.patron@gmail.com', N'Av. Virrey Toledo 450, Salta', 1),
(N'37234567', N'Natalia Lorena', N'Sylvester', N'387-4760987', N'natalia.sylvester@gmail.com', N'Adolfo Güemes 380, Salta', 1),
(N'31345678', N'Leandro Javier', N'Villagrán', N'387-5870987', N'leandro.villagran@gmail.com', N'Alvear 710, Salta', 1),
(N'39456789', N'Julieta Mercedes', N'Becerra', N'387-4980987', N'juli.becerra@gmail.com', N'Bolívar 540, Salta', 1),
(N'28567890', N'Cristian Emanuel', N'Mendoza', N'387-5090987', N'cristian.mendoza@gmail.com', N'Córdoba 620, Salta', 1),
(N'34678901', N'Gabriela Fernanda', N'Peralta', N'387-4198765', N'gaby.peralta@gmail.com', N'Lerma 830, Salta', 1),
(N'36789012', N'Maximiliano José', N'Aguirre', N'387-5298765', N'maxi.aguirre@gmail.com', N'Catamarca 470, Salta', 1),
(N'43890123', N'Martina Candelaria', N'Ibarra', N'387-4398765', N'martina.ibarra@gmail.com', N'Santa Fe 910, Salta', 1),
-- Propietarios con 1 mascota (IDs 30..50 -> 21 propietarios = 21 mascotas)
(N'30901234', N'Esteban Rodrigo', N'Benítez', N'387-5498765', N'esteban.benitez@gmail.com', N'San Juan 1040, Salta', 1),
(N'44012345', N'Delfina María', N'Zavaleta', N'387-4598765', N'delfi.zavaleta@gmail.com', N'San Luis 780, Salta', 1),
(N'26123456', N'Marcelo Alejandro', N'Giménez', N'387-5698765', N'marcelo.gimenez@gmail.com', N'La Rioja 650, Salta', 1),
(N'32234567', N'Verónica Cecilia', N'Molina', N'387-4798765', N'vero.molina@gmail.com', N'Tucumán 520, Salta', 1),
(N'38345678', N'Ignacio Agustín', N'Ortiz', N'387-5898765', N'nacho.ortiz@gmail.com', N'Orán 390, Salta', 1),
(N'41456789', N'Josefina Pilar', N'Vega', N'387-4998765', N'josefina.vega@gmail.com', N'Pellegrini 870, Salta', 1),
(N'27567890', N'Pablo Daniel', N'Ríos', N'387-5012345', N'pablo.rios@gmail.com', N'Ituzaingó 610, Salta', 1),
(N'35678901', N'Noelia Beatriz', N'Silva', N'387-4123456', N'noelia.silva@gmail.com', N'Florida 740, Salta', 1),
(N'40789012', N'Franco Nicolás', N'Castro', N'387-5234567', N'franco.castro@gmail.com', N'Alberdi 830, Salta', 1),
(N'29890123', N'Silvina Andrea', N'Romero', N'387-4345678', N'silvina.romero@gmail.com', N'Mendoza 435, Salta', 1),
(N'33901234', N'Ramiro Ezequiel', N'Suárez', N'387-5456789', N'ramiro.suarez@gmail.com', N'Ayacucho 510, Salta', 1),
(N'37012345', N'Laura Patricia', N'Navarro', N'387-4567890', N'laura.navarro@gmail.com', N'Junín 690, Salta', 1),
(N'42123456', N'Bautista Mateo', N'Domínguez', N'387-5678901', N'bautista.dominguez@gmail.com', N'Maipú 770, Salta', 1),
(N'25234567', N'Héctor Raúl', N'Ponce', N'387-4789012', N'hector.ponce@gmail.com', N'moldes 320, Salta', 1),
(N'31345679', N'Lorena Elizabeth', N'Cabrera', N'387-5890123', N'lorena.cabrera@gmail.com', N'Chacabuco 440, Salta', 1),
(N'36456780', N'Matías Nicolás', N'Vargas', N'387-4901234', N'matias.vargas@gmail.com', N'Lavalle 590, Salta', 1),
(N'39567891', N'Rocío Belén', N'Medina', N'387-5023456', N'rocio.medina@gmail.com', N'Corrientes 810, Salta', 1),
(N'43678902', N'Tomás Joaquín', N'Rojas', N'387-4134567', N'tomas.rojas@gmail.com', N'Av. Sarmiento 960, Salta', 1),
(N'28789013', N'Mariana Soledad', N'Acosta', N'387-5245678', N'mariana.acosta@gmail.com', N'Siria 640, Salta', 1),
(N'34890124', N'Sebastián Lucas', N'Campos', N'387-4356789', N'sebastian.campos@gmail.com', N'Ibazeta 730, Salta', 1),
(N'37901235', N'Cecilia Inés', N'Vázquez', N'387-5467890', N'cecilia.vazquez@gmail.com', N'Aniceto Latorre 480, Salta', 1);

UPDATE dbo.Propietario SET Apellido = N'Solá' WHERE DNI = N'34112098';

-- =========================================================================================
-- 6. CENSO DEMOGRÁFICO DE MASCOTAS (100 PACIENTES ACTIVOS)
--    Distribución por Especie:
--      - Caninos: 58% (58 mascotas: 16 Mestizo, 11 Caniche, 10 Labrador, 8 Bulldog Francés, 7 Ovejero, 6 Golden)
--      - Felinos: 34% (34 mascotas: 14 Común Europeo, 9 Siamés, 6 Persa, 5 Maine Coon)
--      - Otros (Aves y Roedores): 8% (8 mascotas: 4 Aves, 4 Roedores)
--    Distribución por Propietario:
--      - ID 1: 7 | ID 2: 6 | ID 3: 5 | ID 4: 5 | ID 5: 4 | ID 6: 4 | ID 7: 3 | ID 8: 3 (37 mascotas)
--      - IDs 9..29 (21 propietarios x 2 mascotas = 42 mascotas)
--      - IDs 30..50 (21 propietarios x 1 mascota = 21 mascotas)
--      - Total = 100 mascotas
-- =========================================================================================
DECLARE @OwnersSlot TABLE (Slot INT IDENTITY(1,1) PRIMARY KEY, IdPropietario BIGINT);

-- Insertar slots exactos según Pareto
-- Prop 1 (7)
INSERT INTO @OwnersSlot (IdPropietario) VALUES (1),(1),(1),(1),(1),(1),(1);
-- Prop 2 (6)
INSERT INTO @OwnersSlot (IdPropietario) VALUES (2),(2),(2),(2),(2),(2);
-- Prop 3 (5)
INSERT INTO @OwnersSlot (IdPropietario) VALUES (3),(3),(3),(3),(3);
-- Prop 4 (5)
INSERT INTO @OwnersSlot (IdPropietario) VALUES (4),(4),(4),(4),(4);
-- Prop 5 (4)
INSERT INTO @OwnersSlot (IdPropietario) VALUES (5),(5),(5),(5);
-- Prop 6 (4)
INSERT INTO @OwnersSlot (IdPropietario) VALUES (6),(6),(6),(6);
-- Prop 7 (3)
INSERT INTO @OwnersSlot (IdPropietario) VALUES (7),(7),(7);
-- Prop 8 (3)
INSERT INTO @OwnersSlot (IdPropietario) VALUES (8),(8),(8);
-- Props 9..29 (2 cada uno = 42)
DECLARE @p INT = 9;
WHILE @p <= 29
BEGIN
    INSERT INTO @OwnersSlot (IdPropietario) VALUES (@p), (@p);
    SET @p += 1;
END
-- Props 30..50 (1 cada uno = 21)
SET @p = 30;
WHILE @p <= 50
BEGIN
    INSERT INTO @OwnersSlot (IdPropietario) VALUES (@p);
    SET @p += 1;
END

DECLARE @MascotasSeed TABLE (
    Num INT IDENTITY(1,1) PRIMARY KEY,
    IdRaza BIGINT,
    Nombre NVARCHAR(80),
    Sexo NVARCHAR(10),
    FechaNacimiento DATE,
    Color NVARCHAR(50)
);

INSERT INTO @MascotasSeed (IdRaza, Nombre, Sexo, FechaNacimiento, Color) VALUES
-- 16 Mestizo Canino (IdRaza = 1)
(1, N'Rocco', N'Macho', '2021-04-12', N'Negro y fuego'),
(1, N'Luna', N'Hembra', '2022-08-20', N'Blanco y marrón'),
(1, N'Toby', N'Macho', '2020-11-05', N'Atigrado'),
(1, N'Mora', N'Hembra', '2023-02-14', N'Negro azabache'),
(1, N'Barto', N'Macho', '2019-06-30', N'Rubio tostado'),
(1, N'Canela', N'Hembra', '2022-01-18', N'Canela'),
(1, N'Chicho', N'Macho', '2021-09-09', N'Marrón chocolate'),
(1, N'Lola', N'Hembra', '2023-07-22', N'Blanco con manchas'),
(1, N'Panchito', N'Macho', '2020-03-15', N'Gris y blanco'),
(1, N'Kira', N'Hembra', '2024-01-10', N'Dorado'),
(1, N'Cartucho', N'Macho', '2018-12-01', N'Negro'),
(1, N'Nala', N'Hembra', '2023-05-19', N'Arena'),
(1, N'Gaucho', N'Macho', '2021-07-09', N'Tostado'),
(1, N'Uma', N'Hembra', '2022-10-28', N'Blanco'),
(1, N'Capitán', N'Macho', '2020-05-04', N'Marrón y negro'),
(1, N'Frida', N'Hembra', '2024-03-12', N'Tricolor'),
-- 11 Caniche (IdRaza = 2)
(2, N'Copito', N'Macho', '2022-04-15', N'Blanco'),
(2, N'Nina', N'Hembra', '2021-11-23', N'Apricot'),
(2, N'Milo', N'Macho', '2023-03-08', N'Blanco nieve'),
(2, N'Pompón', N'Macho', '2020-08-19', N'Gris plata'),
(2, N'Olivia', N'Hembra', '2023-09-14', N'Blanco'),
(2, N'Teo', N'Macho', '2022-06-02', N'Rojo'),
(2, N'Emma', N'Hembra', '2021-02-27', N'Champagne'),
(2, N'Ciro', N'Macho', '2024-02-05', N'Blanco'),
(2, N'Mia', N'Hembra', '2020-10-11', N'Negro'),
(2, N'Felipe', N'Macho', '2023-01-25', N'Apricot'),
(2, N'Lulú', N'Hembra', '2022-07-16', N'Blanco'),
-- 10 Labrador Retriever (IdRaza = 3)
(3, N'Max', N'Macho', '2021-05-20', N'Dorado'),
(3, N'Bella', N'Hembra', '2022-09-12', N'Chocolate'),
(3, N'Bruno', N'Macho', '2020-02-14', N'Negro'),
(3, N'India', N'Hembra', '2023-04-03', N'Arena'),
(3, N'Simón', N'Macho', '2019-11-28', N'Dorado'),
(3, N'Gala', N'Hembra', '2022-12-19', N'Negro'),
(3, N'Thor', N'Macho', '2021-08-07', N'Chocolate'),
(3, N'Juana', N'Hembra', '2023-10-21', N'Dorado'),
(3, N'Apolo', N'Macho', '2020-06-15', N'Arena'),
(3, N'Zoe', N'Hembra', '2024-04-02', N'Chocolate'),
-- 8 Bulldog Francés (IdRaza = 4)
(4, N'Tyson', N'Macho', '2022-03-17', N'Vaquita'),
(4, N'gorda', N'Hembra', '2021-07-29', N'Fawn'),
(4, N'Homero', N'Macho', '2023-06-11', N'Atigrado oscuro'),
(4, N'Renata', N'Hembra', '2022-11-04', N'Crema'),
(4, N'Bono', N'Macho', '2021-01-22', N'Blue'),
(4, N'Matilda', N'Hembra', '2023-08-30', N'Vaquita'),
(4, N'Ragnar', N'Macho', '2020-12-09', N'Fawn'),
(4, N'Chloe', N'Hembra', '2024-01-19', N'Atigrado'),
-- 7 Ovejero Alemán (IdRaza = 5)
(5, N'Rex', N'Macho', '2020-04-10', N'Manto negro'),
(5, N'Sasha', N'Hembra', '2021-10-05', N'Negro y fuego'),
(5, N'Duque', N'Macho', '2019-09-18', N'Manto negro'),
(5, N'Athena', N'Hembra', '2022-05-24', N'Sable'),
(5, N'Balkan', N'Macho', '2023-02-09', N'Manto negro'),
(5, N'Greta', N'Hembra', '2021-12-14', N'Negro y fuego'),
(5, N'Zeus', N'Macho', '2020-07-31', N'Negro sólido'),
-- 6 Golden Retriever (IdRaza = 6)
(6, N'Balto', N'Macho', '2021-03-13', N'Dorado claro'),
(6, N'Ámbar', N'Hembra', '2022-08-01', N'Dorado intenso'),
(6, N'Benito', N'Macho', '2020-09-25', N'Crema'),
(6, N'Kiara', N'Hembra', '2023-04-17', N'Dorado'),
(6, N'OLIVER', N'Macho', '2021-11-08', N'Dorado'),
(6, N'Bianca', N'Hembra', '2023-12-03', N'Dorado claro'),
-- 14 Común Europeo - Felino (IdRaza = 7)
(7, N'Michi', N'Macho', '2022-02-10', N'Atigrado gris'),
(7, N'Cleo', N'Hembra', '2021-06-18', N'Carey'),
(7, N'Simba', N'Macho', '2023-01-05', N'Naranja'),
(7, N'Minerva', N'Hembra', '2020-10-22', N'Negro'),
(7, N'Garfield', N'Macho', '2021-12-11', N'Naranja atigrado'),
(7, N'Ágata', N'Hembra', '2022-07-04', N'Blanco y negro'),
(7, N'Félix', N'Macho', '2023-05-29', N'Bicolor smoking'),
(7, N'Isis', N'Hembra', '2020-04-16', N'Tricolor calicó'),
(7, N'Tom', N'Macho', '2022-09-03', N'Gris azulado'),
(7, N'Mishi', N'Hembra', '2021-03-27', N'Blanco'),
(7, N'Baco', N'Macho', '2023-08-14', N'Negro'),
(7, N'Pandora', N'Hembra', '2022-11-30', N'Atigrado marrón'),
(7, N'León', N'Macho', '2024-02-18', N'Rubio'),
(7, N'Bastet', N'Hembra', '2021-08-09', N'Carey'),
-- 9 Siamés (IdRaza = 8)
(8, N'Ramsés', N'Macho', '2021-05-14', N'Seal Point'),
(8, N'Zafiro', N'Hembra', '2022-10-08', N'Blue Point'),
(8, N'Koko', N'Macho', '2020-07-19', N'Chocolate Point'),
(8, N'Duquesa', N'Hembra', '2023-03-21', N'Seal Point'),
(8, N'Orión', N'Macho', '2022-01-12', N'Lilac Point'),
(8, N'Suri', N'Hembra', '2021-09-25', N'Seal Point'),
(8, N'Dante', N'Macho', '2023-06-30', N'Blue Point'),
(8, N'Perla', N'Hembra', '2020-11-17', N'Crema y marrón'),
(8, N'Bambú', N'Macho', '2024-01-28', N'Seal Point'),
-- 6 Persa (IdRaza = 9)
(9, N'Nube', N'Hembra', '2021-04-05', N'Blanco puro'),
(9, N'pelusa', N'Hembra', '2020-08-12', N'Gris humo'),
(9, N'Sultan', N'Macho', '2022-06-23', N'Dorado himalayo'),
(9, N'Amélie', N'Hembra', '2023-02-11', N'Crema'),
(9, N'Gaspar', N'Macho', '2021-10-19', N'Plata sombreado'),
(9, N'Fiona', N'Hembra', '2022-12-07', N'Blanco'),
-- 5 Maine Coon (IdRaza = 10)
(10, N'Aslan', N'Macho', '2020-05-18', N'Tabby marrón'),
(10, N'Freya', N'Hembra', '2022-03-09', N'Gris y blanco'),
(10, N'Odín', N'Macho', '2021-07-22', N'Rojo atigrado'),
(10, N'Valkiria', N'Hembra', '2023-09-15', N'Humo negro'),
(10, N'Merlín', N'Macho', '2022-11-01', N'Plata tabby'),
-- 4 Aves (IdRaza 11 y 12)
(11, N'Paco', N'Macho', '2023-01-15', N'Gris perlado y amarillo'),
(11, N'Pepita', N'Hembra', '2022-05-10', N'Lutino amarillo'),
(12, N'Piolín', N'Macho', '2023-04-20', N'Amarillo intenso'),
(12, N'Caruso', N'Macho', '2022-08-14', N'Naranja bronce'),
-- 4 Roedores (IdRaza 13 y 14)
(13, N'Maní', N'Macho', '2023-07-01', N'Tricolor roseta'),
(13, N'Avellana', N'Hembra', '2023-09-12', N'Blanco y caramelo'),
(14, N'Tambor', N'Macho', '2022-11-18', N'Blanco con extremidades negras'),
(14, N'Copo', N'Hembra', '2023-06-05', N'Blanco californiano');

UPDATE @MascotasSeed SET Nombre = N'Gorda' WHERE Nombre = N'gorda';
UPDATE @MascotasSeed SET Nombre = N'Oliver' WHERE Nombre = N'OLIVER';
UPDATE @MascotasSeed SET Nombre = N'Pelusa' WHERE Nombre = N'pelusa';
UPDATE @MascotasSeed SET Nombre = N'Sultán' WHERE Nombre = N'Sultan';

INSERT INTO dbo.Mascota (IdPropietario, IdRaza, Nombre, Sexo, FechaNacimiento, Color, Activo)
SELECT
    os.IdPropietario,
    ms.IdRaza,
    ms.Nombre,
    ms.Sexo,
    ms.FechaNacimiento,
    ms.Color,
    1
FROM @MascotasSeed ms
JOIN @OwnersSlot os ON os.Slot = ((ms.Num * 13 + 7) % 100) + 1
ORDER BY ms.Num;

-- =========================================================================================
-- 7. GENERACIÓN DE ACTIVIDAD CLÍNICA EN HORIZONTE DE 18 MESES (CURVA ORGÁNICA + ESTACIONALIDAD)
--    - Desde hace 17 meses calendario + mes actual = 18 períodos mensuales
--    - Crecimiento progresivo de ~15 consultas/mes a ~58 consultas/mes
--    - Picos en primavera/verano (Sep-Feb) y moderación en invierno (Jun-Jul)
--    - Solo días hábiles y sábados (sin domingos)
--    - Carga de trabajo veterinarios:
--        * Mariano Rodríguez (45%)
--        * Lucía Pérez (30%)
--        * María Sánchez (15%)
--        * Ricardo Daniel Mormina (10%)
-- =========================================================================================
DECLARE @Hoy DATE = CAST(SYSDATETIME() AS DATE);
DECLARE @PrimerDiaMesActual DATE = DATEFROMPARTS(YEAR(@Hoy), MONTH(@Hoy), 1);

DECLARE @CurvaMensual TABLE (
    OffsetMes INT PRIMARY KEY,
    CantidadConsultas INT,
    EsTemporadaAltaVacunas BIT
);

INSERT INTO @CurvaMensual (OffsetMes, CantidadConsultas, EsTemporadaAltaVacunas) VALUES
(-17, 15, 0), -- Hace 17 meses
(-16, 14, 0), -- Invierno año previo
(-15, 16, 0),
(-14, 20, 0),
(-13, 27, 1), -- Inicio primavera año previo
(-12, 32, 1),
(-11, 36, 1), -- Pico primavera
(-10, 34, 1), -- Verano
(-9,  29, 1), -- Enero
(-8,  33, 1), -- Febrero
(-7,  36, 0), -- Otoño
(-6,  38, 0),
(-5,  35, 0),
(-4,  31, 0), -- Invierno actual (más alto que año previo por crecimiento)
(-3,  34, 0),
(-2,  43, 0), -- Fin invierno
(-1,  54, 1), -- Primavera reciente
(0,   58, 1); -- Mes en curso (hasta @Hoy)
-- Total = 585 consultas en 18 meses

DECLARE @MotivosDiagnosticos TABLE (
    Idx INT PRIMARY KEY,
    Motivo NVARCHAR(250),
    Diagnostico NVARCHAR(500),
    Observaciones NVARCHAR(500)
);

INSERT INTO @MotivosDiagnosticos (Idx, Motivo, Diagnostico, Observaciones) VALUES
(0, N'Control sanitario anual y plan de vacunación', N'Paciente clínicamente sano. Mucosas rosadas, linfonódulos normales. Apto para inmunización activa.', N'Se entrega certificado oficial y libreta sanitaria actualizada.'),
(1, N'Refuerzo de vacunación e inspección general', N'Constantes fisiológicas dentro de parámetros normales. Auscultación cardiopulmonar sin particularidades.', N'Control preventivo sin reacciones adversas post-aplicación.'),
(2, N'Vómitos biliosos y decaimiento desde hace 24 hs', N'Gastroenteritis aguda por transgresión alimentaria. Deshidratación leve (<5%).', N'Dieta blanda hipograsa por 72 hs y control evolutivo.'),
(3, N'Prurito intenso y sacudidas frecuentes de cabeza', N'Otitis externa bilateral eritematosa-ceruminosa asociada a Malassezia spp.', N'Higiene del conducto auditivo y tratamiento tópico por 10 días.'),
(4, N'Rascado persistente en dorso y base de la cola', N'Dermatitis alérgica por picadura de pulgas (DAPP) con lesiones alopécicas secundarias.', N'Control ambiental estricto y ectoparasiticida mensual.'),
(5, N'Desparasitación trimestral y control de peso', N'Estado corporal óptimo (BCS 3/5). Control coproparasitológico preventivo.', N'Se indica repetir esquema antiparasitario interno en 90 días.'),
(6, N'Claudicación en miembro posterior derecho', N'Esguince articular leve de rodilla sin ruptura de ligamento cruzado craneal (cajón negativo).', N'Reposo relativo por 7 días y terapia analgésica antiinflamatoria.'),
(7, N'Tos seca paroxística luego del ejercicio', N'Traqueobronquitis infecciosa leve. Reflejo tusígeno positivo a la palpación traqueal.', N'Evitar collares de ahorque, ambiente templado y control en 5 días.'),
(8, N'Halitosis marcada y dificultad al masticar', N'Enfermedad periodontal grado II con presencia de placa calcificada supragingival.', N'Se programa higiene ultrasónica y dieta seca dental.'),
(9, N'Herida cortante superficial en almohadilla plantar', N'Laceración traumática limpia en miembro anterior sin compromiso tendinoso.', N'Antisepsia local, vendaje protector y control en 48 hs.');

DECLARE @MesOffset INT = -17;
DECLARE @GlobalCounter INT = 0;

WHILE @MesOffset <= 0
BEGIN
    DECLARE @InicioMes DATE = DATEADD(MONTH, @MesOffset, @PrimerDiaMesActual);
    DECLARE @FinMes DATE = CASE
        WHEN @MesOffset = 0 THEN @Hoy
        ELSE EOMONTH(@InicioMes)
    END;

    DECLARE @DiasDisponibles INT = DATEDIFF(DAY, @InicioMes, @FinMes) + 1;
    IF @DiasDisponibles < 1 SET @DiasDisponibles = 1;

    DECLARE @CantMes INT, @TempAlta BIT;
    SELECT @CantMes = CantidadConsultas, @TempAlta = EsTemporadaAltaVacunas
    FROM @CurvaMensual WHERE OffsetMes = @MesOffset;

    DECLARE @i INT = 1;
    WHILE @i <= @CantMes
    BEGIN
        SET @GlobalCounter += 1;

        -- Distribuir el día dentro del mes evitando domingos (DATEPART dw con lunes/domingo independiente usando DATEDIFF desde 1900-01-07 que fue domingo)
        DECLARE @DiaOffset INT = ((@i - 1) * @DiasDisponibles / @CantMes + (@GlobalCounter % 3)) % @DiasDisponibles;
        DECLARE @FechaConsulta DATE = DATEADD(DAY, @DiaOffset, @InicioMes);

        -- Si cae domingo (DATEDIFF(DAY, '19000107', @FechaConsulta) % 7 = 0), mover al sábado o lunes dentro del rango
        IF (DATEDIFF(DAY, '19000107', @FechaConsulta) % 7) = 0
        BEGIN
            IF @FechaConsulta > @InicioMes
                SET @FechaConsulta = DATEADD(DAY, -1, @FechaConsulta);
            ELSE IF @FechaConsulta < @FinMes
                SET @FechaConsulta = DATEADD(DAY, 1, @FechaConsulta);
        END

        -- Hora de consulta entre las 08:30 y las 19:45
        DECLARE @Hora INT = 8 + ((@GlobalCounter * 7 + @i * 3) % 12);
        DECLARE @Minuto INT = ((@GlobalCounter * 15) % 60);
        DECLARE @FechaHora DATETIME2(0) = DATEADD(MINUTE, @Hora * 60 + @Minuto, CAST(@FechaConsulta AS DATETIME2(0)));

        -- Selección de mascota (ponderada hacia perros y gatos, incluyendo exóticos)
        DECLARE @IdMascota BIGINT = ((@GlobalCounter * 17 + @i * 5) % 100) + 1;
        DECLARE @IdEspecieMascota BIGINT;
        DECLARE @PrecioBaseConsulta DECIMAL(18,2);

        SELECT
            @IdEspecieMascota = r.IdEspecie,
            @PrecioBaseConsulta = pc.Precio
        FROM dbo.Mascota m
        JOIN dbo.Raza r ON r.Id = m.IdRaza
        JOIN dbo.PrecioConsulta pc ON pc.IdEspecie = r.IdEspecie
        WHERE m.Id = @IdMascota;

        -- Selección de veterinario según carga de trabajo:
        -- 45% Principal (Mariano Rodríguez), 30% Senior (Lucía Pérez), 15% Clínico (María Sánchez), 10% Junior (Ricardo Mormina)
        DECLARE @ModVet INT = (@GlobalCounter * 7 + @i * @i * 3 + ABS(@MesOffset) * 5) % 20;
        DECLARE @IdVet BIGINT = CASE
            WHEN @ModVet < 9  THEN @IdVetPrincipal -- 9/20 = 45%
            WHEN @ModVet < 15 THEN @IdVetSenior    -- 6/20 = 30%
            WHEN @ModVet < 18 THEN @IdVetClinico   -- 3/20 = 15%
            ELSE @IdVetJunior                      -- 2/20 = 10%
        END;

        -- Selección de cuadro clínico coherente
        DECLARE @IdxClinico INT = (@GlobalCounter * 3 + @i * @i + ABS(@MesOffset)) % 10;
        DECLARE @Motivo NVARCHAR(250), @Diagnostico NVARCHAR(500), @Obs NVARCHAR(500);
        SELECT @Motivo = Motivo, @Diagnostico = Diagnostico, @Obs = Observaciones
        FROM @MotivosDiagnosticos WHERE Idx = @IdxClinico;

        DECLARE @Peso DECIMAL(6,2) = CASE @IdEspecieMascota
            WHEN 1 THEN CAST(4.50 + ((@IdMascota * 7) % 28) + ((@GlobalCounter % 9) * 0.30) AS DECIMAL(6,2))
            WHEN 2 THEN CAST(2.80 + ((@IdMascota * 3) % 4) + ((@GlobalCounter % 7) * 0.20) AS DECIMAL(6,2))
            WHEN 3 THEN 0.35
            ELSE 0.95
        END;

        DECLARE @Temp DECIMAL(4,2) = CAST(38.10 + ((@GlobalCounter % 14) * 0.10) AS DECIMAL(4,2));

        INSERT INTO dbo.Consulta (IdUsuario, IdMascota, FechaHora, Motivo, PesoKg, Temperatura, Diagnostico, Observaciones, Importe, Activo)
        VALUES (@IdVet, @IdMascota, @FechaHora, @Motivo, @Peso, @Temp, @Diagnostico, @Obs, @PrecioBaseConsulta, 1);

        DECLARE @IdConsultaNueva BIGINT = SCOPE_IDENTITY();
        DECLARE @TotalTratamientos DECIMAL(18,2) = 0;
        DECLARE @TotalVacunas DECIMAL(18,2) = 0;

        -- =================================================================================
        -- ASOCIACIÓN DE VACUNAS (Proporción objetivo ~35% del volumen total de eventos)
        -- Más intensa en temporada alta (Sep-Feb) que en invierno
        -- =================================================================================
        DECLARE @UmbralVacuna INT = CASE WHEN @TempAlta = 1 THEN 74 ELSE 50 END;
        DECLARE @RollVacuna INT = (@GlobalCounter * 37 + @i * @i * 11 + ABS(@MesOffset) * 17) % 100;

        IF @RollVacuna < @UmbralVacuna AND @IdEspecieMascota IN (1, 2, 4)
        BEGIN
            DECLARE @IdVac1 BIGINT = CASE
                WHEN @IdEspecieMascota = 1 THEN ((@GlobalCounter % 4) + 1)         -- Vacunas caninas 1..4
                WHEN @IdEspecieMascota = 2 THEN ((@GlobalCounter % 3) + 5)         -- Vacunas felinas 5..7
                ELSE 8                                                             -- Roedor 8
            END;

            DECLARE @PrecioVac1 DECIMAL(18,2), @MesesVac1 INT;
            SELECT @PrecioVac1 = Precio, @MesesVac1 = PeriodoMesesRecomendado FROM dbo.Vacuna WHERE Id = @IdVac1;

            INSERT INTO dbo.AplicacionVacuna (IdConsulta, IdVacuna, FechaAplicacion, ProximaDosis, PrecioUnitario, Observaciones)
            VALUES (
                @IdConsultaNueva,
                @IdVac1,
                @FechaConsulta,
                DATEADD(MONTH, @MesesVac1, @FechaConsulta),
                @PrecioVac1,
                N'Aplicación subcutánea en región interescapular sin reacciones locales.'
            );

            SET @TotalVacunas += @PrecioVac1;

            -- ~18% de las consultas vacunatorias aplican una segunda vacuna simultánea (ej. Séxtuple + Antirrábica)
            IF @RollVacuna < 18 AND @IdEspecieMascota IN (1, 2)
            BEGIN
                DECLARE @IdVac2 BIGINT = CASE
                    WHEN @IdEspecieMascota = 1 THEN CASE WHEN @IdVac1 = 2 THEN 1 ELSE 2 END
                    ELSE CASE WHEN @IdVac1 = 6 THEN 5 ELSE 6 END
                END;

                DECLARE @PrecioVac2 DECIMAL(18,2), @MesesVac2 INT;
                SELECT @PrecioVac2 = Precio, @MesesVac2 = PeriodoMesesRecomendado FROM dbo.Vacuna WHERE Id = @IdVac2;

                INSERT INTO dbo.AplicacionVacuna (IdConsulta, IdVacuna, FechaAplicacion, ProximaDosis, PrecioUnitario, Observaciones)
                VALUES (
                    @IdConsultaNueva,
                    @IdVac2,
                    @FechaConsulta,
                    DATEADD(MONTH, @MesesVac2, @FechaConsulta),
                    @PrecioVac2,
                    N'Dosis complementaria de esquema inmunológico anual.'
                );

                SET @TotalVacunas += @PrecioVac2;
            END
        END

        -- =================================================================================
        -- ASOCIACIÓN DE TRATAMIENTOS (Proporción objetivo ~20% del volumen total de eventos)
        -- Mayor incidencia en otoño/invierno y cuadros sintomáticos
        -- =================================================================================
        DECLARE @UmbralTrat INT = CASE WHEN @TempAlta = 0 THEN 46 ELSE 32 END;
        DECLARE @RollTrat INT = (@GlobalCounter * 43 + @i * @i * 7 + ABS(@MesOffset) * 23) % 100;

        IF @RollTrat < @UmbralTrat
        BEGIN
            DECLARE @IdTrat1 BIGINT = ((@GlobalCounter * 3 + @i * 7) % 10) + 1;
            DECLARE @PrecioTrat1 DECIMAL(18,2);
            SELECT @PrecioTrat1 = Precio FROM dbo.Tratamiento WHERE Id = @IdTrat1;

            INSERT INTO dbo.DetalleConsulta (IdConsulta, IdTratamiento, Cantidad, PrecioUnitario, Subtotal, Indicaciones, Activo)
            VALUES (
                @IdConsultaNueva,
                @IdTrat1,
                1,
                @PrecioTrat1,
                @PrecioTrat1,
                N'Administrar respetando horario e indicaciones clínicas entregadas al propietario.',
                1
            );

            SET @TotalTratamientos += @PrecioTrat1;

            -- ~8% reciben un segundo tratamiento complementario
            IF @RollTrat < 8
            BEGIN
                DECLARE @IdTrat2 BIGINT = ((@IdTrat1 + 3) % 10) + 1;
                DECLARE @PrecioTrat2 DECIMAL(18,2);
                SELECT @PrecioTrat2 = Precio FROM dbo.Tratamiento WHERE Id = @IdTrat2;

                INSERT INTO dbo.DetalleConsulta (IdConsulta, IdTratamiento, Cantidad, PrecioUnitario, Subtotal, Indicaciones, Activo)
                VALUES (
                    @IdConsultaNueva,
                    @IdTrat2,
                    1,
                    @PrecioTrat2,
                    @PrecioTrat2,
                    N'Tratamiento coadyuvante de sostén clínico.',
                    1
                );

                SET @TotalTratamientos += @PrecioTrat2;
            END
        END

        -- =================================================================================
        -- REGISTRO DE COBRO / PAGO (Consistente con el total de la consulta)
        -- Distribución de Métodos de Pago:
        --   - 40% Transferencia Bancaria (Id = 4)
        --   - 30% Efectivo (Id = 1)
        --   - 20% Tarjeta de Débito (Id = 2)
        --   - 10% Tarjeta de Crédito (Id = 3)
        -- Estado: 96.5% Completado, 2% Pendiente, 1.5% Anulado
        -- =================================================================================
        DECLARE @ImporteTotal DECIMAL(18,2) = @PrecioBaseConsulta + @TotalTratamientos + @TotalVacunas;
        DECLARE @ModPago INT = (@GlobalCounter * 3 + ABS(@MesOffset)) % 10;

        DECLARE @IdMetodoPago BIGINT = CASE
            WHEN @ModPago IN (0, 1, 2, 3) THEN 4 -- 40% Transferencia Bancaria
            WHEN @ModPago IN (4, 5, 6)    THEN 1 -- 30% Efectivo
            WHEN @ModPago IN (7, 8)       THEN 2 -- 20% Tarjeta de Débito
            ELSE 3                               -- 10% Tarjeta de Crédito
        END;

        DECLARE @Cuotas INT = CASE
            WHEN @IdMetodoPago = 3 THEN CASE WHEN (@GlobalCounter % 2) = 0 THEN 3 ELSE 1 END
            ELSE NULL
        END;

        DECLARE @EstadoPago NVARCHAR(30) = CASE
            WHEN (@GlobalCounter % 65) = 0 THEN N'Anulado'
            WHEN (@GlobalCounter % 45) = 0 THEN N'Pendiente'
            ELSE N'Completado'
        END;

        INSERT INTO dbo.Pago (IdConsulta, IdMetodoPago, Fecha, Importe, Estado, Cuotas, Activo)
        VALUES (
            @IdConsultaNueva,
            @IdMetodoPago,
            DATEADD(MINUTE, 25, @FechaHora),
            @ImporteTotal,
            @EstadoPago,
            @Cuotas,
            1
        );

        SET @i += 1;
    END

    SET @MesOffset += 1;
END

COMMIT TRANSACTION;
PRINT 'Hidratación orgánica completada exitosamente.';

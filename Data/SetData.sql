DELETE FROM GarageContext.dbo.Contracts;
DELETE FROM GarageContext.dbo.ParkedVehicles;

INSERT INTO GarageContext.[dbo].[ParkedVehicles] ([RegistrationNumber], [Type], [Colour], [Manufacturer], [Model], [NumberOfWheels], [ParkingDate]) VALUES (N'AAA123', 2, N'White', N'MAN', N'Buss', 6, N'2019-12-13 10:35:26')
INSERT INTO GarageContext.[dbo].[ParkedVehicles] ([RegistrationNumber], [Type], [Colour], [Manufacturer], [Model], [NumberOfWheels], [ParkingDate]) VALUES (N'AHI742', 1, N'Black', N'BMW', N'RS1000RR', 2, N'2019-12-13 08:29:47')
INSERT INTO GarageContext.[dbo].[ParkedVehicles] ([RegistrationNumber], [Type], [Colour], [Manufacturer], [Model], [NumberOfWheels], [ParkingDate]) VALUES (N'GEX033', 0, N'Red', N'Volkswagen', N'Golf Variant CL', 4, N'2019-12-16 11:10:41')
INSERT INTO GarageContext.[dbo].[ParkedVehicles] ([RegistrationNumber], [Type], [Colour], [Manufacturer], [Model], [NumberOfWheels], [ParkingDate]) VALUES (N'HUJ341', 1, N'White', N'BMW', N'RS1000RR', 2, N'2019-12-17 09:25:12')
INSERT INTO GarageContext.[dbo].[ParkedVehicles] ([RegistrationNumber], [Type], [Colour], [Manufacturer], [Model], [NumberOfWheels], [ParkingDate]) VALUES (N'PAY276', 0, N'Red', N'Skoda', N'Fabia Combi 1.2 TSI', 4, N'2019-12-17 10:15:34')
INSERT INTO GarageContext.[dbo].[ParkedVehicles] ([RegistrationNumber], [Type], [Colour], [Manufacturer], [Model], [NumberOfWheels], [ParkingDate]) VALUES (N'PSA321', 1, N'White', N'BMW', N'RS1000RR', 2, N'2019-12-17 10:16:37')
INSERT INTO GarageContext.[dbo].[ParkedVehicles] ([RegistrationNumber], [Type], [Colour], [Manufacturer], [Model], [NumberOfWheels], [ParkingDate]) VALUES (N'REO32L', 1, N'Orange', N'Honda', N'Super Cub', 2, N'2019-12-17 11:29:25')
INSERT INTO GarageContext.[dbo].[ParkedVehicles] ([RegistrationNumber], [Type], [Colour], [Manufacturer], [Model], [NumberOfWheels], [ParkingDate]) VALUES (N'WQR43A', 1, N'Yellow', N'Honda', N'CRF 1000L AFRICA TWIN RALLY', 2, N'2019-12-17 11:42:22');
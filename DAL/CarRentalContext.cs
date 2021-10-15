using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Reflection;

namespace DAL
{

    /// <summary>
    /// the data base context. the constructer of this class creating a ready dataBase.
    /// </summary>
    public class CarsRentalContext : DbContext
    {
        public CarsRentalContext()
            //: base("CarRentalDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CarsRentalContext, CarRentalMigrationConfig>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<CarRental> CarsRentals { get; set; }
        public DbSet<Role> Roles { get; set; }


    }

    /// <summary>
    /// building the dataBase.
    /// </summary>
    public class CarRentalMigrationConfig : DbMigrationsConfiguration<CarsRentalContext>
    {
        public CarRentalMigrationConfig()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CarsRentalContext context)
        {
            base.Seed(context);
            if (!context.Users.Any())
            {
                Role rAdmin = new Role()
                {
                    RoleName = "Admin"
                };

                Role rUser = new Role()
                {
                    RoleName = "User"
                };
                Role rEmp = new Role()
                {
                    RoleName = "Employee"
                };
                context.Roles.Add(rAdmin);
                context.Roles.Add(rUser);
                context.Roles.Add(rEmp);

                User u1 = new User()
                {
                    BirthDate = new DateTime(1987, 7, 5),
                    Email = "Nadav@inf.co.il",
                    FirstName = "Nadav",
                    Gender = Gender.male,
                    LastName = "Hury",
                    Password = "nadavHury",
                    UserId = 300330842,
                    Username = "nadav",
                };
                u1.Roles.Add(rUser);
                User u2 = new User()
                {
                    BirthDate = new DateTime(1990, 1, 11),
                    Email = "orgalil21@gmail.com",
                    FirstName = "Or",
                    Gender = Gender.male,
                    LastName = "Galili",
                    Password = "orGalili",
                    UserId = 302813464,
                    Username = "galili",
                };
                u2.Roles.Add(rAdmin);
                u2.Roles.Add(rUser);
                User u3 = new User()
                {
                    BirthDate = new DateTime(1984, 4, 11),
                    Email = "haim@gmail.com",
                    FirstName = "Haim",
                    Gender = Gender.male,
                    LastName = "Galili",
                    Password = "haimGalili",
                    UserId = 454738232,
                    Username = "haim"
                };
                u3.Roles.Add(rUser);
                User u4 = new User()
                {
                    BirthDate = new DateTime(1987, 2, 25),
                    Email = "ella@gmail.com",
                    FirstName = "Ella",
                    Gender = Gender.female,
                    LastName = "Galili",
                    Password = "EllaHury",
                    UserId = 437659322,
                    Username = "Ella"
                };
                u4.Roles.Add(rUser);
                User u5 = new User()
                {
                    BirthDate = new DateTime(1991, 4, 2),
                    Email = "Hadar@gmail.com",
                    FirstName = "Hadar",
                    Gender = Gender.female,
                    LastName = "Galili",
                    Password = "HadarGalili",
                    UserId = 459286132,
                    Username = "Hadar"
                };
                u5.Roles.Add(rEmp);
                u5.Roles.Add(rUser);
                User u6 = new User()
                {
                    BirthDate = new DateTime(1983, 1, 20),
                    Email = "shani@gmail.com",
                    FirstName = "Shani",
                    Gender = Gender.female,
                    LastName = "Galili",
                    Password = "ShaniGabai",
                    UserId = 983213267,
                    Username = "Shani"
                };
                u6.Roles.Add(rUser);
                User u7 = new User()
                {
                    BirthDate = new DateTime(1958, 3, 2),
                    Email = "rozi@gmail.com",
                    FirstName = "Rozet",
                    Gender = Gender.female,
                    LastName = "Galili",
                    Password = "RozetGalili",
                    UserId = 984534327,
                    Username = "Rozet"
                };
                u7.Roles.Add(rUser);
                User u8 = new User()
                {
                    BirthDate = new DateTime(1952, 6, 10),
                    Email = "david@gmail.com",
                    FirstName = "David",
                    Gender = Gender.male,
                    LastName = "Galili",
                    Password = "DavidGalili",
                    UserId = 989833267,
                    Username = "David"
                };
                u8.Roles.Add(rUser);

                context.Users.Add(u1);
                context.Users.Add(u2);
                context.Users.Add(u3);
                context.Users.Add(u4);
                context.Users.Add(u5);
                context.Users.Add(u6);
                context.Users.Add(u7);
                context.Users.Add(u8);

                var carTypeHonda = new CarType()
                {
                    Model = "Civic",
                    Manufacturer = "Honda",
                    DailyCost = 200,
                    Gear = Gear.Auto
                };
                var carTypeMazda = new CarType()
                {
                    Model = "6",
                    Manufacturer = "Mazda",
                    DailyCost = 300,
                    Gear = Gear.Auto
                };
                var carTypeIsuzu = new CarType()
                {
                    Model = "D-Max",
                    Manufacturer = "Isuzu",
                    DailyCost = 400,
                    Gear = Gear.Manual
                };
                var carTypeIsuzu2 = new CarType()
                {
                    Model = "Abola",
                    Manufacturer = "Isuzu",
                    DailyCost = 400,
                    Gear = Gear.Manual
                };
                var carTypeIsuzu3 = new CarType()
                {
                    Model = "Bonderal",
                    Manufacturer = "Isuzu",
                    DailyCost = 400,
                    Gear = Gear.Manual
                };
                context.CarTypes.Add(carTypeHonda);
                context.CarTypes.Add(carTypeIsuzu);
                context.CarTypes.Add(carTypeMazda);
                context.CarTypes.Add(carTypeIsuzu2);
                context.CarTypes.Add(carTypeIsuzu3);

                var northenBranch = new Branch();
                var southernBranch = new Branch();
                var westernBranch = new Branch();
                var easternBranch = new Branch();

                northenBranch.Address = "bla bla bla st.";
                northenBranch.Name = "Elbar North";
                northenBranch.Cars = new Collection<Car>();


                MemoryStream ms = new MemoryStream();
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                string rootPath = path.Substring(6, path.Length - 9);

                Image.FromFile(rootPath + @"App_Data\Images\Cars\honda_civic.jpg").Save(ms, ImageFormat.Jpeg);
                byte[] imageHonda = ms.ToArray();
                ms = new MemoryStream();
                Image.FromFile(rootPath + @"App_Data\Images\Cars\isuzu_abola.jpg").Save(ms, ImageFormat.Jpeg);
                byte[] imageAbola = ms.ToArray();
                ms = new MemoryStream();
                Image.FromFile(rootPath + @"App_Data\Images\Cars\isuzu_bonderal.jpg").Save(ms, ImageFormat.Jpeg);
                byte[] imageBonderal = ms.ToArray();
                ms = new MemoryStream();
                Image.FromFile(rootPath + @"App_Data\Images\Cars\isuzu_d-max.jpg").Save(ms, ImageFormat.Jpeg);
                byte[] imageDmax = ms.ToArray();
                ms = new MemoryStream();
                Image.FromFile(rootPath + @"App_Data\Images\Cars\mazda_6.jpg").Save(ms, ImageFormat.Jpeg);
                byte[] imageMazda6 = ms.ToArray();
                ms = new MemoryStream();

                Car northenCar1 = new Car()
                {
                    CarNumber = 6548799,
                    CurrentMileage = 66988,
                    RightToRent = true,
                    CarType = carTypeIsuzu,
                    CostOfDayDelay = 50,
                    Image = imageDmax
                };
                Car northenCar2 = new Car()
                {
                    CarNumber = 2233665,
                    CurrentMileage = 100000,
                    RightToRent = true,
                    CarType = carTypeHonda,
                    CostOfDayDelay = 50,
                    Image = imageHonda
                };
                Car northenCar3 = new Car()
                {
                    CarNumber = 9988775,
                    CurrentMileage = 3214,
                    RightToRent = true,
                    CarType = carTypeMazda,
                    CostOfDayDelay = 50,
                    Image = imageMazda6
                };
                northenBranch.Cars.Add(northenCar1);
                northenBranch.Cars.Add(northenCar2);
                northenBranch.Cars.Add(northenCar3);

                southernBranch.Address = "Hamasger 3";
                southernBranch.Name = "Kal oto South";
                southernBranch.Cars = new Collection<Car>();

                Car southernCar1 = new Car()
                {
                    CarNumber = 8145678,
                    CurrentMileage = 4000,
                    RightToRent = true,
                    CarType = carTypeIsuzu,
                    CostOfDayDelay = 30,
                    Image = imageDmax
                };

                Car southernCar2 = new Car()
                {
                    CarNumber = 5423789,
                    CarType = carTypeMazda,
                    RightToRent = true,
                    CostOfDayDelay = 45,
                    CurrentMileage = 2345,
                    Image = imageMazda6
                };
                Car southernCar3 = new Car()
                {
                    CarNumber = 4536789,
                    CostOfDayDelay = 60,
                    RightToRent = true,
                    CurrentMileage = 5000,
                    CarType = carTypeHonda,
                    Image = imageHonda
                };
                southernBranch.Cars.Add(southernCar1);
                southernBranch.Cars.Add(southernCar2);
                southernBranch.Cars.Add(southernCar3);

                easternBranch.Address = "haorgim st. tel-aviv";
                easternBranch.Name = "kalabasa rent";
                easternBranch.Cars = new Collection<Car>();

                Car easternCar1 = new Car()
                {
                    CarNumber = 4356223,
                    CurrentMileage = 2000,
                    CostOfDayDelay = 32,
                    CarType = carTypeMazda,
                    RightToRent = true,
                    Image = imageMazda6
                };
                Car easternCar2 = new Car()
                {
                    CarType = carTypeIsuzu,
                    CurrentMileage = 1550,
                    RightToRent = true,
                    CostOfDayDelay = 50,
                    CarNumber = 8402156,
                    Image = imageDmax
                };
                Car easternCar3 = new Car()
                {
                    CarNumber = 4683245,
                    RightToRent = true,
                    CurrentMileage = 5632,
                    CarType = carTypeHonda,
                    CostOfDayDelay = 45,
                    Image = imageHonda
                };
                easternBranch.Cars.Add(easternCar1);
                easternBranch.Cars.Add(easternCar2);
                easternBranch.Cars.Add(easternCar3);

                westernBranch.Address = "rogozin st. Ashdod";
                westernBranch.Name = "cool cars";
                westernBranch.Cars = new Collection<Car>();
                Car westernCar1 = new Car()
                {
                    CarNumber = 3256758,
                    RightToRent = true,
                    CarType = carTypeIsuzu,
                    CurrentMileage = 6045,
                    CostOfDayDelay = 20,
                    Image = imageDmax
                };
                Car westernCar2 = new Car()
                {
                    CarNumber = 4536782,
                    RightToRent = true,
                    CarType = carTypeHonda,
                    CurrentMileage = 3425,
                    CostOfDayDelay = 47,
                    Image = imageHonda
                };
                Car westernCar3 = new Car()
                {
                    CarType = carTypeMazda,
                    CurrentMileage = 5647,
                    CarNumber = 5385443,
                    CostOfDayDelay = 30,
                    RightToRent = true,
                    Image = imageMazda6
                };
                westernBranch.Cars.Add(westernCar1);
                westernBranch.Cars.Add(westernCar2);
                westernBranch.Cars.Add(westernCar3);

                context.Branchs.Add(southernBranch);
                context.Branchs.Add(northenBranch);
                context.Branchs.Add(westernBranch);
                context.Branchs.Add(easternBranch);

                CarRental rent1 = new CarRental()
                {
                    Car = westernCar1,
                    StartDate = new DateTime(2015, 5, 6),
                    ReturnDate = new DateTime(2015, 6, 6),
                    User = u1,
                    ActualReturnDate = new DateTime(2015, 6, 6),
                };
                CarRental rent2 = new CarRental()
                {
                    Car = southernCar2,
                    StartDate = new DateTime(2015, 7, 5),
                    ReturnDate = new DateTime(2015, 7, 15),
                    ActualReturnDate = new DateTime(2015, 7, 15),
                    User = u2,
                };
                CarRental rent3 = new CarRental()
                {
                    Car = northenCar3,
                    StartDate = new DateTime(2015, 6, 8),
                    ReturnDate = new DateTime(2015, 9, 24),
                    ActualReturnDate = new DateTime(2015, 9, 24),
                    User = u3,
                };
                CarRental rent4 = new CarRental()
                {
                    Car = southernCar3,
                    StartDate = new DateTime(2015, 5, 26),
                    ReturnDate = new DateTime(2015, 10, 22),
                    User = u4,
                    ActualReturnDate = new DateTime(2015, 10, 22),
                };
                CarRental rent5 = new CarRental()
                {
                    Car = easternCar2,
                    StartDate = new DateTime(2015, 7, 20),
                    ReturnDate = new DateTime(2016, 1, 15),
                    User = u5,
                    ActualReturnDate = new DateTime(2016, 1, 15),
                };
                CarRental rent6 = new CarRental()
                {
                    Car = westernCar2,
                    StartDate = new DateTime(2015, 3, 26),
                    ReturnDate = new DateTime(2015, 8, 20),
                    User = u6,
                    ActualReturnDate = new DateTime(2015, 8, 20),
                };
                CarRental rent7 = new CarRental()
                {
                    Car = easternCar3,
                    StartDate = new DateTime(2015, 3, 21),
                    ReturnDate = new DateTime(2015, 6, 21),
                    User = u7,
                    ActualReturnDate = new DateTime(2015, 10, 21),
                };
                CarRental rent8 = new CarRental()
                {
                    Car = westernCar3,
                    StartDate = new DateTime(2015, 5, 23),
                    ReturnDate = new DateTime(2015, 7, 1),
                    User = u8,
                    ActualReturnDate = new DateTime(2015, 9, 1),
                };
                context.CarsRentals.Add(rent1);
                context.CarsRentals.Add(rent2);
                context.CarsRentals.Add(rent3);
                context.CarsRentals.Add(rent4);
                context.CarsRentals.Add(rent5);
                context.CarsRentals.Add(rent6);
                context.CarsRentals.Add(rent7);
                context.CarsRentals.Add(rent8);
                context.SaveChanges();
            }

        }
    }
}

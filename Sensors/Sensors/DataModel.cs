using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Sensors
{
	public class DataModel : DbContext
	{
		public DbSet<Measurement> Measurements { get; set; } = default!;
		public DbSet<Sensor> Sensors { get; set; } = default!;
		public DbSet<IotSensor> IotSensors { get; set; } = default!;
		public DbSet<Unit> Units { get; set; } = default!;
		public DbSet<MeasurementType> MeasurementTypes { get; set; } = default!;

		public string DbPath { get; }

		public DataModel()
		{
			var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		    DbPath =Path.Join(path, "Sensors.db");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source={DbPath}");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Sensor>()
				.HasKey(x => x.sensorID);

			modelBuilder.Entity<IotSensor>()
				.HasKey(x => x.url);
			//klappt noch nicht
		//	modelBuilder.Entity<IotSensor>()
		//		.HasIndex(x => new{ x.sensorFK })
		//		.IsUnique(true);
		}

	}

    public class Measurement 
    {
        public int measurementID { get; set; }
        public DateTime timestamp { get; set; } 
        public double value { get; set; }
        public Sensor sensorFK { get; set; } = default!;
		public Unit unitFK { get; set; } = default!;
		public MeasurementType measurementTypeFK { get; set; } = default!;
	}

	public class Unit
    {
		[Key]
		public string unit { get; set; } = default!;
	}

	public class MeasurementType
	{
		[Key]
		public string measurementType { get; set; } = default!;
	}

	public class Sensor
	{
        public int sensorID { get; set; }
        public string? name { get; set; }
	}

	public class IotSensor
	{
		public string url { get; set; } = default!;
		public Sensor? sensorFK { get; set; }
	}
}
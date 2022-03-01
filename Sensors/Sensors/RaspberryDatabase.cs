using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql;
namespace Sensors
{
	public class RaspberryDatabase : DbContext
	{
		public DbSet<_Sensor2Messort> Sensor2Messort { get; set; } = default!;
		public string DbPath { get; }

		const string piIp = "192.168.178.27";
		const string piMySQLUser = "root";
		const string piMySQLPW = "root";
		const string databaseName = "heizung";


		public RaspberryDatabase()
		{

		}

		const string connectionString = $"server={piIp},1433;" +
				$"database={databaseName};" +
				$"User ID={piMySQLUser};" +
				$"Password={piMySQLPW}";


		protected override void OnConfiguring(DbContextOptionsBuilder options) => options
				.UseMySql(connectionString: connectionString, serverVersion: ServerVersion.AutoDetect(connectionString));

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Sensor>()
				.HasKey(x => x.sensorID);

		}
	}

	/*MariaDB [heizung]> describe Sensor2Messort;
+---------+------------------+------+-----+-------------------+----------------+
| Field   | Type             | Null | Key | Default           | Extra          |
+---------+------------------+------+-----+-------------------+----------------+
| id      | int(10) unsigned | NO   | PRI | NULL              | auto_increment |
| time    | timestamp        | NO   | MUL | CURRENT_TIMESTAMP |                |
| Sensor  | varchar(20)      | YES  | MUL | NULL              |                |
| Messort | varchar(20)      | YES  | MUL | NULL              |                |
| Wert    | double           | YES  |     | NULL              |                |
| status  | tinyint(1)       | YES  |     | NULL              |                |
+---------+------------------+------+-----+-------------------+----------------+
6 rows in set (0.05 sec)
*/

	public class _Sensor2Messort
		{
			public int id { get; set; }
			public DateTime time { get; set; }
			public string? Sensor { get; set; }
			public string? Messort { get; set; }
			public double? Wert { get; set; }
			public bool? status { get; set; }
		}


}

